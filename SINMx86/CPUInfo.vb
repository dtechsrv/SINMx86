﻿Imports System
Imports System.Management
Imports System.Math
Imports System.Convert
Imports Microsoft.Win32

Imports SINMx86.Localization

' Processzor információs ablak osztálya
Public Class CPUInfo

    ' WMI feldolgozási objektumok
    Public objPR As ManagementObjectSearcher
    Public objMgmt As ManagementObject

    ' CPU-infó tábla változói
    Public CPUCount As Int32 = 0                                ' Processzorok száma
    Public SelectedCPU As Int32 = 0                             ' Kiválasztott processzor
    Public L2Cache(2) As Double                                 ' L2 cache mérete
    Public L3Cache(2) As Double                                 ' L3 cache mérete

    ' *** FŐ ELJÁRÁS: CPU-infó ablak betöltése (MyBase.Load -> CPUInfo) ***
    ' Eseményvezérelt: Ablak megnyitása
    Private Sub CPUInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Értékek definiálása
        Dim ArchID As Int32                                      ' CPU architektúra azonosítója
        Dim ArchStr As String                                    ' CPU architektúra (szöveges)

        ' CPU feketelistás sztringek (Dummy szövegek, amelyeket az alaplap gyártója "elfelejtett" kitölteni.)
        Dim Blacklist() As String = {"PROCESSOR", "PROCESSOR 0", "SOCKET", "SOCKET 0", "CPU", "CPU0", "CPU 0"}

        ' Értékek átvétele a főablaktól
        Dim SelectedCPU = MainWindow.SelectedCPU
        Dim CPUName As String = MainWindow.ComboBox_CPUList.Items(SelectedCPU)

        ' Ablak nevének  átvétele a főablakból
        Me.Text = GetLoc("CPUTitle")

        ' Billentyűk figyelése
        Me.KeyPreview = True

        ' GroupBox szövegének beállítása
        GroupBox_Table.Text = GetLoc("CPUTable") + " " + CPUName

        ' Bezárás gomb
        Button_Close.Text = GetLoc("Button_Close")

        ' Tábla fejléc szövegek átvétele a főablakból
        CPU_Table.Columns(1).Text = GetLoc("CPUDescription")
        CPU_Table.Columns(2).Text = GetLoc("CPUValue")

        ' Ablak láthatósága
        If MainWindow.TopMost Then
            Me.TopMost = True
        Else
            Me.TopMost = False
        End If

        ' Sorok törlése
        CPU_Table.Items.Clear()

        ' WMI érték definiálása
        objPR = New ManagementObjectSearcher("SELECT * FROM Win32_Processor")

        ' Értékek kinyerése a WMI-ből
        For Each Me.objMgmt In objPR.Get()
            If CPUCount = SelectedCPU Then

                ' Architektúra ellenőrzése
                ArchID = objMgmt("Architecture")
                If ArchID = 0 Then
                    ArchStr = "x86"
                ElseIf ArchID = 1 Then
                    ArchStr = "MIPS"
                ElseIf ArchID = 2 Then
                    ArchStr = "Alpha"
                ElseIf ArchID = 3 Then
                    ArchStr = "PowerPC"
                ElseIf ArchID = 6 Then
                    ArchStr = "IA64"
                ElseIf ArchID = 9 Then
                    ArchStr = "x64"
                Else
                    ArchStr = GetLoc("Unknown")
                End If

                ' Sorok felvitele
                CPUTableAddRow(GetLoc("CPUVendor"), MainWindow.RemoveSpaces(objMgmt("Manufacturer")), Nothing)
                CPUTableAddRow(GetLoc("CPUName"), MainWindow.RemoveSpaces(objMgmt("Name")), Nothing)
                CPUTableAddRow(GetLoc("CPUIdent"), MainWindow.RemoveSpaces(objMgmt("Description")), Nothing)
                CPUTableAddRow(GetLoc("CPUCores"), objMgmt("NumberOfCores").ToString, Nothing)
                CPUTableAddRow(GetLoc("CPUThreads"), objMgmt("NumberOfLogicalProcessors").ToString, Nothing)

                ' Gyártói "lustaság" sztingek keresése
                If Not MainWindow.CheckStrMatch(objMgmt("SocketDesignation"), Blacklist, False) Then
                    CPUTableAddRow(GetLoc("CPUSocket"), objMgmt("SocketDesignation"), Nothing)
                End If

                ' Üres feszültségérték ellenőrzése
                If objMgmt("CurrentVoltage") <> 0 Then
                    CPUTableAddRow(GetLoc("CPUVoltage"), FixDigitSeparator(objMgmt("CurrentVoltage") / 10, 1, False), "V")
                End If

                ' Architektúra hozzáadása
                CPUTableAddRow(GetLoc("CPUArchitect"), ArchStr, Nothing)

                ' Üres aktuális órajel ellenőrzése
                If objMgmt("CurrentClockSpeed") <> 0 Then
                    CPUTableAddRow(GetLoc("CPUCurrentSpeed"), objMgmt("CurrentClockSpeed").ToString, "MHz")
                End If

                ' Üres gyári órajel ellenőrzése
                If objMgmt("MaxClockSpeed") <> 0 Then
                    CPUTableAddRow(GetLoc("CPUMaxSpeed"), objMgmt("MaxClockSpeed").ToString, "MHz")
                End If

                ' Üres busz órajel ellenőrzése
                If objMgmt("ExtClock") <> 0 Then
                    CPUTableAddRow(GetLoc("CPUBusClock"), objMgmt("ExtClock").ToString, "MHz")
                End If

                ' L2 Cache méretének konvertálása
                L2Cache = MainWindow.DynByteConv(objMgmt("L2CacheSize") * 1024, 0)
                CPUTableAddRow(GetLoc("CPUL2"), L2Cache(0).ToString, MainWindow.PrefixTable(L2Cache(1)) + "B")

                ' XP alatt nem szereplő értékek kihagyása
                If MainWindow.OSMajorVersion >= 6 Then

                    ' L3 Cache méretének konvertálása
                    L3Cache = MainWindow.DynByteConv(objMgmt("L3CacheSize") * 1024, 0)
                    CPUTableAddRow(GetLoc("CPUL3"), L3Cache(0).ToString, MainWindow.PrefixTable(L3Cache(1)) + "B")

                End If
            End If

            ' Számláló növelése
            CPUCount += 1
        Next

    End Sub

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