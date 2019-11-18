Imports System.Convert
Imports System.Management

Imports SINMx86.Functions
Imports SINMx86.Localization

' Processzor információs ablak osztálya
Public Class CPUInfo

    ' WMI feldolgozási objektumok
    Public objPR As ManagementObjectSearcher
    Public objMgmt As ManagementObject

    ' CPU-infó tábla változói
    Public CPUCount As Int32 = 0                                ' Processzorok száma

    ' *** FŐ ELJÁRÁS: CPU-infó ablak betöltése (MyBase.Load -> CPUInfo) ***
    ' Eseményvezérelt: Ablak megnyitása
    Private Sub CPUInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Értékek definiálása
        Dim VendorCount, NameCount, ArchCount As Int32          ' Ciklusszámlálók (gyártó, név, architektúra)
        Dim NameString As String                                ' Processzor neve
        Dim DeviceID As String = Nothing                        ' Processzor azonosítója
        Dim L2Cache(2) As Double                                ' Level 2 cache mérete
        Dim L3Cache(2) As Double                                ' Level 3 cache mérete

        ' Sztring cserék változói (eredeti, csere)
        Dim NameSearch() As String = {"MHz", "GHz"}
        Dim NameReplace() As String = {" MHz", " GHz"}

        ' CPU architektúra tömbök értékei (keresett azonosító, valódi érték)
        Dim ArchID() As Int32 = {0, 1, 2, 3, 6, 9}
        Dim ArchStr() As String = {"x86", "MIPS", "Alpha", "PowerPC", "IA64", "x64"}

        ' CPU feketelistás sztringek (Dummy szövegek, amelyeket az alaplap gyártója "elfelejtett" kitölteni.)
        Dim Blacklist() As String = {"PROCESSOR", "PROCESSOR 0", "SOCKET", "SOCKET 0", "CPU", "CPU0", "CPU 0", "Intel CPU"}

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

        ' WMI értékek lekérdezése: Win32_Processor -> CPU-információk
        ' Megjegyzés: Ha wildcard lenne, akkor túl lassú lenne a lekérdezés, ezért ilyen hosszú!
        objPR = New ManagementObjectSearcher("SELECT DeviceID, Manufacturer, Name, Description, NumberOfCores, NumberOfLogicalProcessors, SocketDesignation, " +
                                             "CurrentVoltage, Architecture, CurrentClockSpeed, MaxClockSpeed, ExtClock, L2CacheSize FROM Win32_Processor")

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

                ' Név korrekciós sztringek keresése és cseréje
                NameString = objMgmt("Name")
                For NameCount = 0 To UBound(NameSearch)
                    NameString = Replace(NameString, NameSearch(NameCount), NameReplace(NameCount))
                Next

                ' Módosított név hozzáadása
                CPUTableAddRow(GetLoc("CPUName"), RemoveSpaces(NameString), Nothing)

                ' Módosítás nélküli sorok felvitele
                CPUTableAddRow(GetLoc("CPUIdent"), RemoveSpaces(objMgmt("Description")), Nothing)
                CPUTableAddRow(GetLoc("CPUCores"), objMgmt("NumberOfCores").ToString, Nothing)
                CPUTableAddRow(GetLoc("CPUThreads"), objMgmt("NumberOfLogicalProcessors").ToString, Nothing)

                ' Gyártói "lustaság" sztingek keresése
                If Not CheckStrMatch(objMgmt("SocketDesignation"), Blacklist, False) Then
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