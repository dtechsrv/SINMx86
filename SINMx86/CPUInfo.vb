Imports System.Convert
Imports System.Management

Imports SINMx86.Functions
Imports SINMx86.Localization

' Processzor információs ablak osztálya
Public Class CPUInfo

    ' WMI feldolgozási objektumok
    Public objPR, objPE As ManagementObjectSearcher
    Public objMgmt As ManagementObject

    ' CPU-infó tábla változói
    Public CPUCount As Int32 = 0                                ' Processzorok száma

    ' *** FŐ ELJÁRÁS: CPU-infó ablak betöltése (MyBase.Load -> CPUInfo) ***
    ' Eseményvezérelt: Ablak megnyitása
    Private Sub CPUInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Értékek definiálása
        Dim VendorCount, NameCount, ArchCount As Int32          ' Ciklusszámlálók (gyártó, név, architektúra)
        Dim CPUCores, CPUThreads As Int32                       ' Magok és szálak száma
        Dim NameString As String = Nothing                      ' Processzor neve
        Dim CorrString As String = Nothing                      ' Processzor javított neve (KB953955)
        Dim DeviceID As String = Nothing                        ' Processzor azonosítója
        Dim L2Cache(2) As Double                                ' Level 2 cache mérete
        Dim L3Cache(2) As Double                                ' Level 3 cache mérete

        ' Sztring cserék változói (eredeti, csere)
        Dim NameSearch() As String = {"MHz", "GHz"}
        Dim NameReplace() As String = {" MHz", " GHz"}

        ' Névből törlendő sztringek tömbje
        Dim NameDelete() As String = {"CPU", "processor"}

        ' CPU architektúra tömbök értékei (keresett azonosító, valódi érték)
        Dim ArchID() As Int32 = {0, 1, 2, 3, 6, 9}
        Dim ArchStr() As String = {"x86", "MIPS", "Alpha", "PowerPC", "IA64", "x64"}

        ' CPU feketelistás sztringek (Dummy szövegek, amelyeket az alaplap gyártója "elfelejtett" kitölteni.)
        Dim Blacklist() As String = {"PROCESSOR", "SOCKET 0", "CPU"}

        ' Értékek átvétele a főablaktól
        Dim TableName As String = MainWindow.ComboBox_CPUList.Items(SelectedCPU)

        ' Ablak láthatóságának átvétele -> Megegyezik a főablakkal!
        Me.TopMost = MainWindow.TopMost

        ' Ablak nevének beállítása
        Me.Text = MyName + " - " + GetLoc("CPUTitle")

        ' Billentyűk figyelése
        Me.KeyPreview = True

        ' GroupBox szövegének beállítása
        GroupBox_Table.Text = GetLoc("CPUTable") + " " + TableName

        ' Bezárás gomb
        Button_Close.Text = GetLoc("Button_Close")

        ' Tábla fejléc szövegek beállítása
        CPU_Table.Columns(1).Text = GetLoc("CPUDescription")
        CPU_Table.Columns(2).Text = GetLoc("CPUValue")

        ' Sorok törlése
        CPU_Table.Items.Clear()

        ' WMI értékek lekérdezése: Win32_Processor -> Magok és szálak száma
        objPR = New ManagementObjectSearcher("SELECT NumberOfCores, NumberOfLogicalProcessors FROM Win32_Processor")

        ' Processzor mag és szál értékek kiértékelése (XP: KB936235, 2003: KB932370)
        ' Megjegyzés: Ha üres a tábla, akkor 'ManagementException'-t okoz, ezért kell a 'Try'!
        ' Elsősorban XP-nél és Server 2003-nál kell rá számítani, ahol hiányzik a HT/Multicore kezelési frissítés!
        Try

            ' Értékek beállítása -> Processzor: magok, szálak
            For Each Me.objMgmt In objPR.Get()
                If CPUCount = SelectedCPU Then
                    CPUCores = objMgmt("NumberOfCores")
                    CPUThreads = objMgmt("NumberOfLogicalProcessors")
                End If
                CPUCount += 1
            Next

        Catch

            ' Statikus értékek beállítása -> Processzor: magok, szálak (1/1)
            CPUCores = 1
            CPUThreads = 1

        End Try

        ' Processzor számláló visszaállítása
        CPUCount = 0

        ' WinXP/2003 CPU nevének javítása (KB953955)
        ' Megjegyzés: Mivel minden processzor neve egyezik, ezért csak az első kerül beállításra.
        If OSVersion(0) <= 6 Then

            ' WMI lekérdezés: Win32_PnPEntity -> Processzorok listája
            objPE = New ManagementObjectSearcher("SELECT Caption FROM Win32_PnPEntity WHERE ClassGuid='{50127DC3-0F36-415E-A6CC-4CB3BE910B65}'")

            ' Értékek beállítása -> CPU neve (Csak az első!)
            For Each Me.objMgmt In objPE.Get()
                While CPUCount < 1
                    CorrString = RemoveParentheses(objMgmt("Caption"))
                    CPUCount += 1
                End While
            Next

        End If

        ' Processzor számláló visszaállítása
        CPUCount = 0

        ' WMI értékek lekérdezése: Win32_Processor -> CPU-információk
        objPR = New ManagementObjectSearcher("SELECT DeviceID, Manufacturer, Name, Description, SocketDesignation, CurrentVoltage, " +
                                             "Architecture, CurrentClockSpeed, MaxClockSpeed, ExtClock, L2CacheSize FROM Win32_Processor")

        ' Értékek beállítása -> Processzor
        For Each Me.objMgmt In objPR.Get()
            If CPUCount = SelectedCPU Then

                ' Eszközazonosító beállítása
                DeviceID = objMgmt("DeviceID")

                ' Gyártói sztring keresése és találat esetén a valódi név kiírása
                For VendorCount = 0 To UBound(CPUVendorID)
                    If objMgmt("Manufacturer") = CPUVendorID(VendorCount) Then
                        CPUTableAddRow(GetLoc("CPUVendor"), CPUVendorStr(VendorCount), Nothing)
                    End If
                Next

                ' Név beállítása
                NameString = RemoveParentheses(objMgmt("Name"))

                ' Értékek összehasonlítása (Ha eltér, akkor felül lesz írva!)
                If OSVersion(0) <= 6 Then
                    If NameString <> CorrString Then NameString = CorrString
                End If

                ' Név korrekciós sztringek keresése és cseréje
                For NameCount = 0 To UBound(NameSearch)
                    NameString = Replace(NameString, NameSearch(NameCount), NameReplace(NameCount))
                Next

                ' Névből törlendő sztringek keresése és törlése
                For NameCount = 0 To UBound(NameDelete)
                    NameString = Replace(NameString, NameDelete(NameCount), Nothing)
                Next

                ' Módosított név hozzáadása
                CPUTableAddRow(GetLoc("CPUName"), RemoveSpaces(NameString), Nothing)

                ' Azonosító hozzáadása
                CPUTableAddRow(GetLoc("CPUIdent"), RemoveSpaces(objMgmt("Description")), Nothing)

                ' Korábban kiértékelt értékek soainak felvitele
                CPUTableAddRow(GetLoc("CPUCores"), CPUCores.ToString, Nothing)
                CPUTableAddRow(GetLoc("CPUThreads"), CPUThreads.ToString, Nothing)

                ' Gyártói "lustaság" sztingek keresése
                If Not CheckStrContain(objMgmt("SocketDesignation"), Blacklist, False) Then
                    CPUTableAddRow(GetLoc("CPUSocket"), objMgmt("SocketDesignation"), Nothing)
                End If

                ' Üres feszültségérték ellenőrzése
                If objMgmt("CurrentVoltage") <> 0 Then
                    CPUTableAddRow(GetLoc("CPUVoltage"), FixNumberFormat(objMgmt("CurrentVoltage") / 10, 1, False), "V")
                End If

                ' Architektúra keresése
                For ArchCount = 0 To UBound(ArchID)
                    If objMgmt("Architecture") = ArchID(ArchCount) Then
                        CPUTableAddRow(GetLoc("CPUArchitect"), ArchStr(ArchCount), Nothing)
                    End If
                Next

                ' Üres aktuális órajel ellenőrzése
                If objMgmt("CurrentClockSpeed") <> 0 Then
                    CPUTableAddRow(GetLoc("CPUCurrentSpeed"), FixNumberFormat(objMgmt("CurrentClockSpeed"), 0, False), "MHz")
                End If

                ' Üres gyári órajel ellenőrzése
                If objMgmt("MaxClockSpeed") <> 0 Then
                    CPUTableAddRow(GetLoc("CPUMaxSpeed"), FixNumberFormat(objMgmt("MaxClockSpeed"), 0, False), "MHz")
                End If

                ' Üres busz órajel ellenőrzése
                If objMgmt("ExtClock") <> 0 Then
                    CPUTableAddRow(GetLoc("CPUBusClock"), FixNumberFormat(objMgmt("ExtClock"), 0, False), "MHz")
                End If

                ' L2 Cache méretének konvertálása
                If objMgmt("L2CacheSize") <> 0 Then
                    L2Cache = ScaleConversion(objMgmt("L2CacheSize") * 1024, 0, True)
                    CPUTableAddRow(GetLoc("CPUL2"), FixNumberFormat(L2Cache(0), 0, False), BytePrefix(L2Cache(1)) + "B")
                Else
                    CPUTableAddRow(GetLoc("CPUL2"), GetLoc("NotInstalled"), Nothing)
                End If

            End If

            ' Számláló növelése
            CPUCount += 1

        Next

        ' XP alatt nem szereplő értékek lekérdezése
        If OSVersion(0) >= 6 Then

            ' WMI értékek lekérdezése: Win32_Processor -> Azonosító alapján
            objPR = New ManagementObjectSearcher("SELECT L3CacheSize FROM Win32_Processor WHERE DeviceID='" + DeviceID + "'")

            ' Értékek beállítása -> L3 cache mérete
            For Each Me.objMgmt In objPR.Get()

                ' L3 Cache méretének konvertálása
                If objMgmt("L3CacheSize") <> 0 Then
                    L3Cache = ScaleConversion(objMgmt("L3CacheSize") * 1024, 0, True)
                    CPUTableAddRow(GetLoc("CPUL3"), FixNumberFormat(L3Cache(0), 0, False), BytePrefix(L3Cache(1)) + "B")
                Else
                    CPUTableAddRow(GetLoc("CPUL3"), GetLoc("NotInstalled"), Nothing)
                End If

            Next
        End If

    End Sub

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVÉNY: Sor hozzáadása a CPU-táblához ***
    ' Bemenet: Name  -> név (String)
    '          Value -> érték (String)
    '          Unit  -> mértékegység (String)
    ' Kimenet: *     -> hamis érték (Boolean)
    Private Function CPUTableAddRow(ByVal Name As String, ByVal Value As String, ByVal Unit As String)

        ' Értékek definiálása
        Dim ListItem As ListViewItem                            ' Egy sor elemei listanézetben
        Dim ListFields(3) As String                             ' Egy sor értékeinek tömbje (A kezdő oszlop miatt 1-gyel nagyobb!)
        Dim ListColumn As Int32                                 ' Aktuális oszlop száma

        ' Formázás -> Félkövér és normál betűk soron belül
        Dim ListBold() As Boolean = {False, True, False}

        ' Név hozzáadása (1)
        If Name <> Nothing Then
            ListFields(1) = Name + ":"
        End If

        ' Érték hozzáadása (2)
        If Value = "0" Or Value = Nothing Then

            ' Üres érték kezelése
            ListFields(2) = GetLoc("NotAvailable")
        Else

            ' Mértékegység ellenőrzése
            If Unit <> Nothing Then
                ListFields(2) = Value + " " + Unit
            Else
                ListFields(2) = Value
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
                ListItem.SubItems(ListColumn).Font = New Font(CPU_Table.Font, FontStyle.Bold)
            Else

                ' Normál
                ListItem.SubItems(ListColumn).Font = New Font(CPU_Table.Font, FontStyle.Regular)
            End If

            ' Formátum beállítása
            ListItem.SubItems.Add(ListFields(ListColumn))
        Next

        ' Sor hozzáadása a lsitához

        CPU_Table.Items.Add(ListItem)


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