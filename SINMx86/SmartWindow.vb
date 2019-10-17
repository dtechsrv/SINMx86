Imports System.Math
Imports System.Convert
Imports System.Management

Imports SINMx86.Functions
Imports SINMx86.Localization

' S.M.A.R.T ablak osztálya
Public Class SmartWindow

    ' WMI feldolgozási objektumok
    Public objSM, objST, objDD As ManagementObjectSearcher
    Public objMgmt, objRes As ManagementObject

    ' S.M.A.R.T tábla változói
    Public SmartPnPID(32) As String                             ' PnP azonosító a S.M.A.R.T-hoz
    Public DiskCount As Int32 = 0                               ' Lemezek száma
    Public SmartRecord(255) As String                           ' Rekord nevek tömbje

    ' *** FŐ ELJÁRÁS: S.M.A.R.T ablak betöltése (MyBase.Load -> SmartWindow) ***
    ' Eseményvezérelt: Ablak megnyitása
    Private Sub SmartWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Értékek definiálása
        Dim SmartColunms() As Int32 = {2, 3, 5, 6, 7}           ' Rekordok kezdeti helye, egy soron belül
        Dim SmartStep As Int32 = 12                             ' S.M.A.R.T bájtok ugrásköze (12-esével)
        Dim SmartCount As Int32 = 0                             ' S.M.A.R.T bájtok léptetése (beállítás ciklus közben)
        Dim ValueSum As Int64 = 0                               ' Adatok összege (4 bájtból, növekvő nagyságrenddel)
        Dim ValueDiff As Int64 = 0                              ' Matematikai segédváltozó
        Dim SmartData() As Byte                                 ' Nyers adatok tömbje
        Dim SmartTreshold() As Byte                             ' Nyers küszöbértékek tömbje
        Dim ByteDigit As Int32                                  ' Bájt helyiérték (RAW adat számításhoz)

        ' Listaelemek definiálása
        Dim ListItem As ListViewItem                            ' Egy sor elemei listanézetben
        Dim ListFields(UBound(SmartColunms) + 2) As String      ' Egy sor értékeinek tömbje (A kezdő oszlop és a szöveges feliratok miatt 2-vel nagyobb!)
        Dim ListColumn As Int32                                 ' Aktuális oszlop száma

        ' Értékek átvétele a főablaktól
        Dim TableName As String = MainWindow.ComboBox_DiskList.Items(SelectedDisk)
        Dim SmartID As String = DiskSmart(SelectedDisk)

        ' Ablak láthatóságának átvétele -> Megegyezik a főablakkal!
        Me.TopMost = MainWindow.TopMost

        ' Ablak nevének beállítása
        Me.Text = GetLoc("SmartTitle")

        ' Billentyűk figyelése
        Me.KeyPreview = True

        ' GroupBox szövegének beállítása
        GroupBox_Table.Text = GetLoc("SmartTable") + " " + TableName

        ' Bezárás gomb
        Button_Close.Text = GetLoc("Button_Close")

        ' Tábla fejléc szövegek átvétele a főablakból
        SMART_Table.Columns(1).Text = "#"
        SMART_Table.Columns(2).Text = GetLoc("SmartRecord")
        SMART_Table.Columns(3).Text = GetLoc("SmartTreshold")
        SMART_Table.Columns(4).Text = GetLoc("SmartValue")
        SMART_Table.Columns(5).Text = GetLoc("SmartWorst")
        SMART_Table.Columns(6).Text = GetLoc("SmartData")

        ' Formázás -> Félkövér és normál betűk soron belül
        Dim ListBold() As Boolean = {False, True, True, False, False, False, True}

        ' Sorok törlése
        SMART_Table.Items.Clear()

        ' Smart rekord nevek betöltése
        LoadSmartRecords()

        ' WMI lekérdezés -> S.M.A.R.T alapértékek (Rekord száma, legrosszabb és az aktuális nyers adat)
        objSM = New ManagementObjectSearcher("ROOT\WMI", "SELECT VendorSpecific FROM MSStorageDriver_ATAPISmartData WHERE InstanceName = '" + SmartID + "'")

        For Each Me.objMgmt In objSM.Get()
            SmartData = objMgmt("VendorSpecific")

            ' WMI lekérdezés -> S.M.A.R.T küszöbértékek
            objST = New ManagementObjectSearcher("ROOT\WMI", "SELECT VendorSpecific FROM MSStorageDriver_FailurePredictThresholds  WHERE InstanceName = '" + SmartID + "'")

            For Each Me.objRes In objST.Get()
                SmartTreshold = objMgmt("VendorSpecific")

                ' Értékek rendezése tömbbe (Ameddig tart a lista!)
                While SmartData(SmartCount + SmartColunms(0)) <> 0

                    ' Rekord száma (1)
                    ListFields(1) = SmartData(SmartCount + SmartColunms(0)).ToString

                    ' Rekord neve (A 'SmartRecord' tömbből!)
                    ListFields(2) = SmartRecord(ToInt32(SmartData(SmartCount + SmartColunms(0))))

                    ' Küszöbérték (3)
                    ListFields(3) = SmartTreshold(SmartCount + SmartColunms(1)).ToString

                    ' Legrosszabb (4)
                    ListFields(4) = SmartData(SmartCount + SmartColunms(2)).ToString

                    ' Legjobb (5)
                    ListFields(5) = SmartData(SmartCount + SmartColunms(3)).ToString

                    ' Kapott érték alaphelyzetbe állítása (Több értékből származtatva!)
                    ValueSum = 0

                    ' Eltérő értékek kezelése -> Hőmérséklet (Az aktuális érték az első helyen van, ezért van kikerülve az összeadás!)
                    If SmartData(SmartCount + SmartColunms(0)) = 190 Or SmartData(SmartCount + SmartColunms(0)) = 194 Then

                        ' Hőmérséklet értékek átalakítása
                        ValueSum = ToInt32(SmartData(SmartCount + SmartColunms(4)))
                        ListFields(6) = ValueSum.ToString + " °C"

                        ' Fahrenheit átalakítás
                        ValueSum = Round((9 * ValueSum / 5) + 32)
                        ListFields(6) += " / " + ValueSum.ToString + " °F"

                    Else

                        ' Eltérő értékek kezelése
                        If SmartData(SmartCount + SmartColunms(0)) = 9 Then

                            For ByteDigit = 0 To 2
                                ValueSum += ToInt32(SmartData(SmartCount + SmartColunms(4) + ByteDigit)) * (256 ^ ByteDigit)
                            Next

                            ' Üzemidő: napok
                            ValueDiff = Int(ValueSum / (24))
                            ListFields(6) = ValueDiff.ToString + " " + GetLoc("Days")

                            ' Üzemidő: órák
                            ValueDiff = ValueSum - (ValueDiff * 24)
                            ListFields(6) += ", " + ValueDiff.ToString + " " + GetLoc("Hours")

                        Else

                            ' Alapértékek helyiérték szerinti összeadása
                            For ByteDigit = 0 To 4
                                ValueSum += ToInt32(SmartData(SmartCount + SmartColunms(4) + ByteDigit)) * (256 ^ ByteDigit)
                            Next

                            ' Nincs korrekció, átalakítás nélküli nyers értékek kiírása
                            ListFields(6) = FixNumberFormat(ValueSum, 0, False)

                        End If
                    End If

                    ' Új sor definiálása
                    ListItem = New ListViewItem(ListFields)

                    ' Elemek önálló formázásának engedélyezése
                    ListItem.UseItemStyleForSubItems = False

                    ' Fomrázási beállítások
                    For ListColumn = 0 To UBound(ListBold)
                        If ListBold(ListColumn) Then

                            ' Félkövér
                            ListItem.SubItems(ListColumn).Font = New Font(SMART_Table.Font, FontStyle.Bold)
                        Else

                            ' Normál
                            ListItem.SubItems(ListColumn).Font = New Font(SMART_Table.Font, FontStyle.Regular)
                        End If

                        ' Formátum beállítása
                        ListItem.SubItems.Add(ListFields(ListColumn))
                    Next

                    ' Sor hozzáadása a lsitához
                    SMART_Table.Items.Add(ListItem)

                    ' Számláló növelése az ugrásközzel
                    SmartCount += SmartStep

                End While
            Next
        Next

    End Sub

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVÉNY: S.M.A.R.T rekordok neveinek beállítása ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Private Function LoadSmartRecords()

        ' Minden elem feltöltése
        For i As Int32 = 0 To UBound(SmartRecord)
            SmartRecord(i) = "Vendor-specific or Unknown"
        Next

        ' Ismert elemek felülírása
        SmartRecord(1) = "Raw Read Error Rate"
        SmartRecord(2) = "Throughput Performance"
        SmartRecord(3) = "Spin Up Time"
        SmartRecord(4) = "Start/Stop Count"
        SmartRecord(5) = "Reallocated Sectors Count"
        SmartRecord(6) = "Read Channel Margin"
        SmartRecord(7) = "Seek Error Rate"
        SmartRecord(8) = "Seek Time Performance"
        SmartRecord(9) = "Power-On Time Count"
        SmartRecord(10) = "Spin Retry Count"
        SmartRecord(11) = "Drive Calibration Retry Count"
        SmartRecord(12) = "Drive Power Cycle Count"
        SmartRecord(13) = "Soft Read Error Rate"
        SmartRecord(100) = "Erase/Program Cycles"
        SmartRecord(103) = "Translation Table Rebuild"
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
        SmartRecord(180) = "Used Reserved Block Count"
        SmartRecord(181) = "Program Fail Count"
        SmartRecord(182) = "Erase Fail Count"
        SmartRecord(183) = "SATA Downshift Error Count or Runtime Bad Block"
        SmartRecord(184) = "End-to-End error"
        SmartRecord(185) = "Head Stability"
        SmartRecord(186) = "Induced Op-Vibration Detection"
        SmartRecord(187) = "Reported Uncorrectable Errors"
        SmartRecord(188) = "Command Timeout"
        SmartRecord(189) = "High Fly Writes (or Temperature)"
        SmartRecord(190) = "Airflow Temperature"
        SmartRecord(191) = "G-sense Error Rate"
        SmartRecord(192) = "Power off Retract Count"
        SmartRecord(193) = "Load/Unload Cycle Count"
        SmartRecord(194) = "Disk Temperature"
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
        SmartRecord(230) = "GMR Head Amplitude or Drive Life Protection Status"
        SmartRecord(231) = "Life Left or Temperature"
        SmartRecord(232) = "Endurance Remaining or Available Reserved Space"
        SmartRecord(233) = "Media Wearout Indicator or Power-On Hours"
        SmartRecord(234) = "Average/Maximum Erase Count"
        SmartRecord(235) = "POR Recovery Count or Good/Free Block Count"
        SmartRecord(240) = "Head Flying Hours"
        SmartRecord(241) = "Total LBAs Written"
        SmartRecord(242) = "Total LBAs Read"
        SmartRecord(243) = "Total LBAs Written Expanded"
        SmartRecord(244) = "Total LBAs Read Expanded"
        SmartRecord(249) = "NAND Writes"
        SmartRecord(250) = "Read Error Retry Rate"
        SmartRecord(251) = "Minimum Spares Remaining"
        SmartRecord(252) = "Newly Added Bad Flash Block"
        SmartRecord(254) = "Free Fall Event Count"

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