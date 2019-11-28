Imports System.Math
Imports System.Convert
Imports System.Management

Imports SINMx86.Functions
Imports SINMx86.Localization

' S.M.A.R.T ablak osztálya
Public Class SMARTInfo

    ' WMI feldolgozási objektumok
    Public objSM, objST As ManagementObjectSearcher
    Public objMgmt As ManagementObject

    ' S.M.A.R.T tábla változói
    Public SmartPnPID(32) As String                             ' PnP azonosító a S.M.A.R.T-hoz
    Public DiskCount As Int32 = 0                               ' Lemezek száma
    Public SmartRecord(255) As String                           ' Rekord nevek tömbje
    Public RecordFlag(255) As Int32                             ' Rekord jellemző: 0 -> nem kritikus, 1 -> figyelmeztető, 2 -> kritikus

    ' Eltérő riasztási értékek változói (kézzel felülbírált limitek)
    Public WarningTreshold As Int32 = 100                       ' Figyelmeztető köszöbérték 
    Public CriticalTreshold As Int32 = 100                      ' Kritikus köszöbérték
    Public TempWarning As Int32 = 45                            ' Figyelmeztetési hőmérséklet értéke (Celsius)
    Public TempCritical As Int32 = 55                           ' Kritikus hőmérséklet értéke (Celsius)

    ' *** FŐ ELJÁRÁS: S.M.A.R.T ablak betöltése (MyBase.Load -> SmartWindow) ***
    ' Eseményvezérelt: Ablak megnyitása
    Private Sub SmartWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Értékek definiálása
        Dim SmartColunms() As Int32 = {2, 3, 5, 6, 7}           ' Rekordok kezdeti helye, egy soron belül
        Dim SmartStep As Int32 = 12                             ' S.M.A.R.T bájtok ugrásköze (12-esével)
        Dim SmartCount As Int32 = 0                             ' S.M.A.R.T bájtok léptetése (beállítás ciklus közben)
        Dim ValueSum As Int64 = 0                               ' Adatok összege (4 bájtból, növekvő nagyságrenddel)
        Dim ValueDiff As Int64 = 0                              ' Matematikai segédváltozó
        Dim SmartData() As Byte = Nothing                       ' Nyers adatok tömbje
        Dim SmartTreshold() As Byte = Nothing                   ' Nyers küszöbértékek tömbje
        Dim ByteDigit As Int32                                  ' Bájt helyiérték (RAW adat számításhoz)
        Dim RowNumber As Int32 = 0                              ' Sorok száma a listanézetben

        ' Rekordok változói (szám, küszöb, érték, legrosszabb)
        Dim RecordNumber, RecordTreshold, RecordValue, RecordWorst As Int32
        Dim RecordName As String                                ' Rekord neve
        Dim RecordRawValue As Int64 = 0                         ' Nyers érték
        Dim RecordStatus As Int32 = 0                           ' Rekord állapota: 0 = OK, 1 = figyelmeztetés, 2 = kritikus (hiba)

        ' Formázás -> Félkövér és normál betűk soron belül (Ügyelni kell az elemszámra!)
        Dim ListBold() As Boolean = {False, True, True, True, False, False, True, True}

        ' Listaelemek definiálása
        Dim ListItem As ListViewItem                            ' Egy sor elemei listanézetben
        Dim ListFields(UBound(ListBold)) As String              ' Egy sor értékeinek tömbje
        Dim ListColumn As Int32                                 ' Aktuális oszlop száma

        ' Értékek átvétele a főablaktól
        Dim TableName As String = MainWindow.ComboBox_DiskList.Items(SelectedDisk)
        Dim DiskType As String = MainWindow.DiskType(SelectedDisk)
        Dim SmartID As String = MainWindow.DiskSmart(SelectedDisk)

        ' Ablak láthatóságának átvétele -> Megegyezik a főablakkal!
        Me.TopMost = MainWindow.TopMost

        ' Ablak nevének beállítása
        Me.Text = MyName + " - " + GetLoc("SMARTTitle")

        ' Billentyűk figyelése
        Me.KeyPreview = True

        ' GroupBox szövegének beállítása
        GroupBox_Table.Text = GetLoc("SMARTTable") + " " + TableName

        ' Bezárás gomb
        Button_Close.Text = GetLoc("Button_Close")

        ' Tábla fejléc szövegek beállítása
        SMART_Table.Columns(0).Text = Nothing
        SMART_Table.Columns(1).Text = GetLoc("SMARTNumber")
        SMART_Table.Columns(2).Text = GetLoc("SMARTRecord")
        SMART_Table.Columns(3).Text = GetLoc("SMARTTreshold")
        SMART_Table.Columns(4).Text = GetLoc("SMARTValue")
        SMART_Table.Columns(5).Text = GetLoc("SMARTWorst")
        SMART_Table.Columns(6).Text = GetLoc("SMARTStatus")
        SMART_Table.Columns(7).Text = GetLoc("SMARTData")

        ' Képek feltöltése az állapotokat jelző képlistába
        SMART_Status.Images.Add(0, My.Resources.SMART_OK)
        SMART_Status.Images.Add(1, My.Resources.SMART_Warning)
        SMART_Status.Images.Add(2, My.Resources.SMART_Critical)

        ' Sorok törlése
        SMART_Table.Items.Clear()

        ' Smart rekord nevek betöltése
        If DiskType = "SSD" Then
            LoadSmartRecords(True)
        Else
            LoadSmartRecords(False)
        End If

        ' WMI lekérdezés -> S.M.A.R.T alapértékek
        objSM = New ManagementObjectSearcher("ROOT\WMI", "SELECT VendorSpecific FROM MSStorageDriver_ATAPISmartData WHERE InstanceName = '" + SmartID + "'")

        ' Értékek beállítása -> A küszöbértékek kivételével minden itt van tárolva (rekord száma, legrosszabb és az aktuális nyers adat)
        For Each Me.objMgmt In objSM.Get()
            SmartData = objMgmt("VendorSpecific")
        Next

        ' WMI lekérdezés -> S.M.A.R.T küszöbértékek
        objST = New ManagementObjectSearcher("ROOT\WMI", "SELECT VendorSpecific FROM MSStorageDriver_FailurePredictThresholds WHERE InstanceName = '" + SmartID + "'")

        ' Értékek beállítása -> Küszöbértékek
        For Each Me.objMgmt In objST.Get()
            SmartTreshold = objMgmt("VendorSpecific")
        Next

        ' Értékek rendezése (Ameddig tart a lista!)
        While SmartData(SmartCount + SmartColunms(0)) <> 0

            ' Rekord változóinak beállítása
            RecordNumber = ToInt32(SmartData(SmartCount + SmartColunms(0)))
            RecordName = SmartRecord(RecordNumber)
            RecordTreshold = ToInt32(SmartTreshold(SmartCount + SmartColunms(1)))
            RecordValue = ToInt32(SmartData(SmartCount + SmartColunms(2)))
            RecordWorst = ToInt32(SmartData(SmartCount + SmartColunms(3)))

            ' Rekord nevének formázása (nyers és formázott értékek)
            RecordName = Replace(RecordName, "RAW", GetLoc("SMARTRaw"))
            RecordName = Replace(RecordName, "Converted", GetLoc("SMARTConvert"))

            ' Lista feltöltése
            ListFields(1) = RecordNumber.ToString
            ListFields(2) = RecordName
            ListFields(3) = RecordTreshold.ToString
            ListFields(4) = RecordValue.ToString
            ListFields(5) = RecordWorst.ToString

            ' Kapott érték alaphelyzetbe állítása (Több értékből származtatva!)
            ValueSum = 0

            ' Eltérő értékek kezelése
            If RecordNumber = 190 Or RecordNumber = 194 Then

                ' Hőmérséklet értékek formázása (Az aktuális érték az első helyen van, ezért csak az van figyelembe véve!)
                ValueSum = ToInt32(SmartData(SmartCount + SmartColunms(4)))
                ListFields(7) = ValueSum.ToString + " °C"

                ' Nyers adat felülbírálása (Celsius érték beállítása)
                RecordRawValue = ValueSum

                ' Hőmérséklet érték átalakítása (Celsius -> Fahrenheit) és hozzáadása a formázott sztringhez 
                ValueSum = Round((9 * ValueSum / 5) + 32)
                ListFields(7) += " / " + ValueSum.ToString + " °F"

            ElseIf RecordNumber = 9 Or RecordNumber = 240 Then

                ' Üzemórák és fejpozícionálással töltőtt órák száma (Csak az első három érték kerül feldolgozásra!)
                For ByteDigit = 0 To 2
                    ValueSum += ToInt32(SmartData(SmartCount + SmartColunms(4) + ByteDigit)) * (256 ^ ByteDigit)
                Next

                ' Nyers adat felülbírálása (összes óra beállítása)
                RecordRawValue = ValueSum

                ' Értékek formázása: napok
                ValueDiff = Int(ValueSum / (24))
                ListFields(7) = ValueDiff.ToString + " " + GetLoc("Days")

                ' Értékek formázása: órák (különbözet)
                ValueDiff = ValueSum - (ValueDiff * 24)
                ListFields(7) += ", " + ValueDiff.ToString + " " + GetLoc("Hours")

            Else

                ' Általános rekordok (Alapértékek helyiérték szerinti összeadása!)
                For ByteDigit = 0 To 4
                    ValueSum += ToInt32(SmartData(SmartCount + SmartColunms(4) + ByteDigit)) * (256 ^ ByteDigit)
                Next

                ' Nincs korrekció, átalakítás nélküli nyers értékek kiírása
                ListFields(7) = FixNumberFormat(ValueSum, 0, False)

                ' Nyers adat
                RecordRawValue = ValueSum

            End If

            ' Figyelmeztető és kritikus rekordok kiértékelése
            If RecordFlag(RecordNumber) = 1 Then

                ' Figyelmeztető rekordok kezelése
                ' Megjegyzés: A gyártói küszöböt figyelmen kívül hagyva, ha az érték eléri a beállított köszöböt, akkor figyelmeztetésre lesz módosítva!
                If RecordRawValue >= WarningTreshold Then
                    RecordStatus = 1
                    ListFields(6) = GetLoc("SMARTWarning")
                Else
                    RecordStatus = 0
                    ListFields(6) = GetLoc("SMARTOK")
                End If

            ElseIf RecordFlag(RecordNumber) = 2 Then

                ' Kritikus rekordok kezelése
                ' Megjegyzés: A gyártói küszöböt figyelmen kívül hagyva, ha az érték eléri a beállított köszöböt, akkor kritikusra lesz módosítva!
                If RecordRawValue = 0 Then
                    RecordStatus = 0
                    ListFields(6) = GetLoc("SMARTOK")
                ElseIf RecordRawValue >= CriticalTreshold Then
                    RecordStatus = 2
                    ListFields(6) = GetLoc("SMARTCritical")
                Else
                    RecordStatus = 1
                    ListFields(6) = GetLoc("SMARTWarning")
                End If

            ElseIf RecordFlag(RecordNumber) = 3 Then

                ' Hőmérsékleti figyelmeztetés beállítása (beállított limittel a kritikus hőmérsékletküszöb előtt)
                If RecordRawValue < TempWarning Then
                    RecordStatus = 0
                    ListFields(6) = GetLoc("SMARTOK")
                ElseIf RecordRawValue >= TempWarning And RecordRawValue < TempCritical Then
                    RecordStatus = 1
                    ListFields(6) = GetLoc("SMARTWarning")
                Else
                    RecordStatus = 2
                    ListFields(6) = GetLoc("SMARTCritical")
                End If

            Else

                ' Alapérték beállítása nem kritikus rekordoknál
                RecordStatus = 0
                ListFields(6) = GetLoc("SMARTOK")

            End If

            ' Köszüb és jelenlegi érték összehasonlítása
            ' Megjegyzés: Minden rekordnál érvényes és felülírja az első ellenőrzést!
            If RecordTreshold > RecordValue Then
                RecordStatus = 2
                ListFields(6) = GetLoc("SMARTCritical")
            End If

            ' Új sor definiálása
            ListItem = New ListViewItem(ListFields)

            ' Elemek önálló formázásának engedélyezése
            ListItem.UseItemStyleForSubItems = False

            ' Állapotkép beállítása
            ListItem.StateImageIndex = RecordStatus

            ' Fomrázási beállítások
            For ListColumn = 0 To UBound(ListBold)

                ' Eltérő és normál formázási feállítások
                If ListBold(ListColumn) Then
                    ListItem.SubItems(ListColumn).Font = New Font(SMART_Table.Font, FontStyle.Bold)
                Else
                    ListItem.SubItems(ListColumn).Font = New Font(SMART_Table.Font, FontStyle.Regular)
                End If

                ' Ismeretlen rekordok betűszínének megváltoztatása
                If ListFields(2) = SmartRecord(0) Then
                    ListItem.SubItems(ListColumn).ForeColor = Color.Gray

                    ' Dőlt betűs rekordnév
                    If ListColumn = 2 And ListBold(ListColumn) Then
                        ListItem.SubItems(ListColumn).Font = New Font(SMART_Table.Font, FontStyle.Bold Or FontStyle.Italic)
                    End If
                End If

                ' Kritikus rekordok betű- és háttér színének megváltoztatása
                If RecordFlag(RecordNumber) > 0 Then
                    ListItem.SubItems(ListColumn).BackColor = Color.WhiteSmoke
                    ListItem.SubItems(ListColumn).ForeColor = Color.ForestGreen
                End If

                ' Állapotjelölés színének megváltoztatása (Figyelmeztetés: narancs; Kritikus: piros)
                If RecordStatus = 1 Then
                    ListItem.SubItems(ListColumn).ForeColor = Color.DarkOrange
                ElseIf RecordStatus = 2 Then
                    ListItem.SubItems(ListColumn).ForeColor = Color.Red
                End If

                ' Formátum beállítása
                ListItem.SubItems.Add(ListFields(ListColumn))

            Next

            ' Sor hozzáadása a lsitához
            SMART_Table.Items.Add(ListItem)

            ' Számláló növelése az ugrásközzel
            SmartCount += SmartStep

            ' Sorok számának növelése
            RowNumber += 1

        End While

        ' Gördítősáv helyének kivonása, ha a lista nem fér el a táblában görgetés nélkül! (Ha az utolsó sor alja lejjebb van, mint a tábla magassága!)
        If SMART_Table.Height <= SMART_Table.Items(RowNumber - 1).GetBounds(ItemBoundsPortion.Entire).Bottom Then

            ' Rekordnév oszlop szélességének csökkentése (a gördítősáv szélességével)
            Me.Record.Width -= SystemInformation.VerticalScrollBarWidth

        End If

        ' Tábla kiválasztása (A gördítés miatt fontos!)
        SMART_Table.Select()

    End Sub

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVÉNY: S.M.A.R.T rekordok neveinek beállítása ***
    ' Bemenet: SSD -> Adathordozó típusa (Boolean)
    ' Kimenet: *   -> hamis érték (Boolean)
    Private Function LoadSmartRecords(ByVal DiskIsSSD As Boolean)

        ' Értékek definiálása
        Dim RecordCount As Int32                                ' Rekord számláló
        Dim ArrayCount As Int32                                 ' Tömb számláló

        ' Kritikus rekordok -> Ezeknek rendszerint 0 értéket kell mutatniuk, ha minden rendben!
        ' Megjegyzés: Ha nem 0, akkor figyelmeztetés, a beállított küszöböt átlépve kritikus értéket vesz fel!
        ' Rekordok: Reallocated Sectors, Spin Retry Count, End-to-End Errors, Reallocation Events, Current Pending Sectors, Off-Line Uncorrectable Sectors.
        Dim CriticalRecords() As Int32 = {5, 10, 184, 196, 197, 198}

        ' Figyelmeztető rekordok -> Ezeknek egy értéket meghaladóan csak figyelmeztetést kell megjeleníteni!
        ' Megjegyzés: A beállított köszöböt átlépve figyelmeztetés!
        ' Rekordok: Reported Uncorrectable Errors, Command Timeout, Off-Line Uncorrectable Sectors.
        Dim WarningRecords() As Int32 = {187, 188, 199}

        ' Hőmérsékletet tartalmazó rekordok -> Kizárólag a konvertáltak!
        ' Megjegyzés: A köszöb elérése előtt figyelmeztetési állapotot kap!
        ' Rekordok: Airflow Temperature, Disk Temperature.
        Dim TemperatureRecords() As Int32 = {190, 194}

        ' Alapértelmezett érték (A '0'-ás rekord nincs definiálva!)
        SmartRecord(0) = "Vendor Specific Record"

        ' Ismert elemek feltöltése: HDD-re és SSD-re is jellemző rekordok
        ' Megjegyzés: Az SSD-knél eltérő értékek később felül lesznek írva!
        SmartRecord(1) = "Read Error Rate"
        SmartRecord(2) = "Throughput Performance"
        SmartRecord(3) = "Spin-Up Time"
        SmartRecord(4) = "Start/Stop Count"
        SmartRecord(5) = "Reallocated Sector Count"
        SmartRecord(6) = "Read Channel Margin"
        SmartRecord(7) = "Seek Error Rate"
        SmartRecord(8) = "Seek Time Performance"
        SmartRecord(9) = "Power-On Hours (Converted)"
        SmartRecord(10) = "Spin Retry Count"
        SmartRecord(11) = "Calibration Retry Count"
        SmartRecord(12) = "Power Cycle Count"
        SmartRecord(13) = "Soft Read Error Rate"
        SmartRecord(22) = "Current Helium Level"
        SmartRecord(103) = "Translation Table Rebuild"
        SmartRecord(174) = "Power-off Retract Count"
        SmartRecord(183) = "SATA Downshift Error Count"
        SmartRecord(184) = "End-to-End Error"
        SmartRecord(185) = "Head Stability"
        SmartRecord(186) = "Induced Op-Vibration Detection"
        SmartRecord(187) = "Reported Uncorrectable Errors"
        SmartRecord(188) = "Command Timeout"
        SmartRecord(189) = "High Fly Writes"
        SmartRecord(190) = "Airflow Temperature (Converted)"
        SmartRecord(191) = "G-sense Error Rate"
        SmartRecord(192) = "Power off Retract Count"
        SmartRecord(193) = "Load/Unload Cycle Count"
        SmartRecord(194) = "Disk Temperature (Converted)"
        SmartRecord(195) = "Hardware ECC Recovered"
        SmartRecord(196) = "Reallocation Event Count"
        SmartRecord(197) = "Current Pending Sector Count"
        SmartRecord(198) = "Off-Line Uncorrectable Sector Count"
        SmartRecord(199) = "Ultra ATA CRC Error Count"
        SmartRecord(200) = "Write Error Rate"
        SmartRecord(201) = "Soft Read Error Rate"
        SmartRecord(202) = "Data Address Mark Errors"
        SmartRecord(203) = "Run Out Cancel"
        SmartRecord(204) = "Soft ECC Correction"
        SmartRecord(205) = "Thermal Asperity Rate"
        SmartRecord(206) = "Flying Height"
        SmartRecord(207) = "Spin High Current"
        SmartRecord(208) = "Spin Buzz"
        SmartRecord(209) = "Offline Seek Performance"
        SmartRecord(210) = "Vibration During Write"
        SmartRecord(211) = "Vibration During Write"
        SmartRecord(212) = "Shock During Write"
        SmartRecord(220) = "Disk Shift"
        SmartRecord(221) = "G-Sense Error Rate"
        SmartRecord(222) = "Loaded Hours"
        SmartRecord(223) = "Load/Unload Retry Count"
        SmartRecord(224) = "Load Friction"
        SmartRecord(225) = "Load/Unload Cycle Count"
        SmartRecord(226) = "Load-in Time"
        SmartRecord(227) = "Torque Amplification Count"
        SmartRecord(228) = "Power-off Retract Count"
        SmartRecord(230) = "GMR Head Amplitude"
        SmartRecord(231) = "Temperature (RAW)"
        SmartRecord(233) = "Power-On Hours (RAW)"
        SmartRecord(240) = "Head Flying Hours (Converted)"
        SmartRecord(241) = "Total LBAs Written (RAW)"
        SmartRecord(242) = "Total LBAs Read (RAW)"
        SmartRecord(250) = "Read Error Retry Rate"
        SmartRecord(251) = "Minimum Spares Remaining"
        SmartRecord(254) = "Free Fall Event Count"

        ' Csak SSD-re jellemző rekordok vagy eltérő rekordok felülírása
        If DiskIsSSD Then
            SmartRecord(100) = "Erase/Program Cycles"
            SmartRecord(170) = "Reserved Block Count"
            SmartRecord(171) = "Program Fail Count"
            SmartRecord(172) = "Erase Fail Count"
            SmartRecord(173) = "Wear Leveling Count"
            SmartRecord(174) = "Unexpected Power Loss Count"
            SmartRecord(175) = "Program Fail Count"
            SmartRecord(176) = "Erase Fail Count"
            SmartRecord(177) = "Wear Range Delta"
            SmartRecord(178) = "Used Reserved Block Count"
            SmartRecord(179) = "Used Reserved Block Count"
            SmartRecord(180) = "Unused Reserved Block Count"
            SmartRecord(181) = "Program Fail Count"
            SmartRecord(182) = "Erase Fail Count"
            SmartRecord(189) = "Temperature (RAW)"
            SmartRecord(230) = "Drive Life Protection Status"
            SmartRecord(231) = "Life Left"
            SmartRecord(232) = "Available Reserved Space"
            SmartRecord(233) = "Media Wearout Indicator"
            SmartRecord(241) = "Total LBAs or GBs Written (RAW)"
            SmartRecord(242) = "Total LBAs os GBs Read (RAW)"
            SmartRecord(249) = "NAND Writes"
            SmartRecord(252) = "Newly Added Bad Flash Block"
        End If

        ' Üres rekord nevek feltöltése
        For RecordCount = 1 To UBound(SmartRecord)
            If IsNothing(SmartRecord(RecordCount)) Then
                SmartRecord(RecordCount) = SmartRecord(0)
            End If
        Next

        ' Figyelmeztető rekordok beállítása
        For ArrayCount = 0 To UBound(WarningRecords)
            RecordFlag(WarningRecords(ArrayCount)) = 1
        Next

        ' Kritikus rekordok beállítása
        For ArrayCount = 0 To UBound(CriticalRecords)
            RecordFlag(CriticalRecords(ArrayCount)) = 2
        Next

        ' Hőmérséklet értéket tartalmazó rekordok beállítása
        For ArrayCount = 0 To UBound(TemperatureRecords)
            RecordFlag(TemperatureRecords(ArrayCount)) = 3
        Next

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' ----- ELJÁRÁSOK -----

    ' *** ELJÁRÁS: Kilépési procedúra megindítása (közvetett) ***
    ' Eseményvezérelt: Me.KeyDown -> ESC (Fizikai gomb lenyomása)
    Private Sub KeyDown_Escape_Close(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        ' Ablak bezárása ESC lenyomására
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If

    End Sub

    ' *** ELJÁRÁS: Kilépési procedúra megindítása (közvetett) ***
    ' Eseményvezérelt: Button_Close.Click -> Klikk (Gomb)
    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click

        ' Ablak bezárása
        Me.Close()

    End Sub

End Class