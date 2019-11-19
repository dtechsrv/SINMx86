Imports System.Convert
Imports System.Management

Imports SINMx86.Functions
Imports SINMx86.Localization

' Memória információs ablak osztálya
Public Class RAMInfo

    ' WMI feldolgozási objektumok
    Public objPM As ManagementObjectSearcher
    Public objMgmt As ManagementObject

    ' RAM-infó tábla változói
    Public RAMCount As Int32 = 0                                ' Modulok száma

    ' *** FŐ ELJÁRÁS: RAM-infó ablak betöltése (MyBase.Load -> RAMInfo) ***
    ' Eseményvezérelt: Ablak megnyitása
    Private Sub RAMInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Értékek definiálása
        Dim MemorySize(1) As Double                             ' Formázott modul összméret és a rendszer által felhasználható memória
        Dim MemoryCount = 0                                     ' Memória modul számláló
        Dim MemoryClock, TypeValue, MemorySocket As Int32       ' Órajel, típus és tokozás
        Dim MemoryBank, MemoryLocation As String                ' Fizikai elhelyezkedés
        Dim MemoryType As String = Nothing                      ' Memória modulok típusa
        Dim MemoryVendor, MemoryModel, MemorySerial As String   ' Modul gyártója, modellszáma és sorozatszáma
        Dim MemoryIdent As String = Nothing                     ' Modul JEDEC azonosítója
        Dim MemoryWidth As Int32 = 0                            ' Memória sávszélessége
        Dim BankCount As Int32                                  ' Sztring keresési ciklusszámláló

        ' Sztring cserék változói (eredeti, csere)
        Dim BankSearch() As String = {"_", "-", "CHANNEL", "BANK", "DIMM"}
        Dim BankReplace() As String = {" ", " ", "Channel ", "Bank ", "DIMM "}

        ' Értékek átvétele a főablaktól
        Dim TableName As String = MainWindow.ComboBox_RAMList.Items(SelectedMemory)

        ' Ablak láthatóságának átvétele -> Megegyezik a főablakkal!
        Me.TopMost = MainWindow.TopMost

        ' Ablak nevének beállítása
        Me.Text = MyName + " - " + GetLoc("RAMTitle")

        ' Billentyűk figyelése
        Me.KeyPreview = True

        ' GroupBox szövegének beállítása
        GroupBox_Table.Text = GetLoc("RAMTable") + " " + TableName

        ' Bezárás gomb
        Button_Close.Text = GetLoc("Button_Close")

        ' Tábla fejléc szövegek beállítása
        RAM_Table.Columns(1).Text = GetLoc("RAMDescription")
        RAM_Table.Columns(2).Text = GetLoc("RAMValue")

        ' Sorok törlése
        RAM_Table.Items.Clear()

        ' WMI értékek lekérdezése: Win32_PhysicalMemory -> Memória információk
        objPM = New ManagementObjectSearcher("SELECT Manufacturer, PartNumber, SerialNumber, Capacity, BankLabel, " +
                                             "DeviceLocator, Speed, TotalWidth, FormFactor, MemoryType FROM Win32_PhysicalMemory")

        ' Értékek beállítása -> memória modulok tulajdonságai
        For Each Me.objMgmt In objPM.Get()
            If RAMCount = SelectedMemory Then

                ' Gyártóspecifikus értékek beállítása
                MemoryVendor = RemoveInvalidChars(objMgmt("Manufacturer"))
                MemoryModel = RemoveInvalidChars(objMgmt("PartNumber"))
                MemorySerial = RemoveInvalidChars(objMgmt("SerialNumber"))

                ' Gyártóspecifikus értékek ellenőrzése
                If RemoveSpaces(MemoryVendor) <> Nothing And InStr(MemoryVendor, "Manufacturer") = 0 Then
                    RAMTableAddRow(GetLoc("RAMVendor"), RemoveSpaces(MemoryVendor), Nothing)
                End If

                If RemoveSpaces(MemoryModel) <> Nothing And InStr(MemoryModel, "PartNum") = 0 Then
                    RAMTableAddRow(GetLoc("RAMModel"), RemoveSpaces(MemoryModel), Nothing)
                End If

                If RemoveSpaces(MemorySerial) <> Nothing And InStr(MemorySerial, "SerNum") = 0 Then
                    RAMTableAddRow(GetLoc("RAMSerial"), RemoveSpaces(MemorySerial), Nothing)
                End If

                ' Bank és hely értékek beállítása
                MemoryBank = RemoveInvalidChars(objMgmt("BankLabel"))
                MemoryLocation = RemoveInvalidChars(objMgmt("DeviceLocator"))

                ' Bank és hely értékek ellenőrzése
                If RemoveSpaces(MemoryBank) <> Nothing Then

                    ' Bank és hely korrekciós sztringek keresése és cseréje
                    For BankCount = 0 To UBound(BankSearch)
                        MemoryBank = Replace(MemoryBank, BankSearch(BankCount), BankReplace(BankCount))
                        MemoryLocation = Replace(MemoryLocation, BankSearch(BankCount), BankReplace(BankCount))
                    Next

                    ' Hely formázása
                    If RemoveSpaces(MemoryLocation) <> Nothing Then
                        MemoryLocation = "(" + RemoveSpaces(MemoryLocation) + ")"
                    End If

                    ' Sor hozzáadása
                    RAMTableAddRow(GetLoc("RAMBank"), RemoveSpaces(MemoryBank), MemoryLocation)

                End If

                ' Memória kapacitás beállítása
                If objMgmt("Capacity") <> 0 Then
                    MemorySize = ScaleConversion(objMgmt("Capacity"), 0, True)
                    RAMTableAddRow(GetLoc("RAMSize"), FixNumberFormat(MemorySize(0), 0, False), BytePrefix(MemorySize(1)) + "B (" + FixNumberFormat(objMgmt("Capacity"), 0, False) + " " + GetLoc("Byte") + ")")
                End If

                ' További értékek beállítása későbbi kiértékelésre
                MemorySocket = objMgmt("FormFactor")
                TypeValue = objMgmt("MemoryType")
                MemoryClock = objMgmt("Speed")
                MemoryWidth = objMgmt("TotalWidth")

            End If

            ' Modul számának növelése
            RAMCount += 1

        Next

        ' Memória modul típusának beállítása (Ha tartományon belül van, és nem üres!)
        If MemorySocket <= UBound(MemorySocketType) Then
            If MemorySocketType(MemorySocket) <> Nothing Then
                RAMTableAddRow(GetLoc("RAMSocket"), MemorySocketType(MemorySocket), Nothing)
            End If
        End If

        ' Win10 SMBIOS memória típus lekérdezése
        If OSVersion(0) >= 10 Then

            ' WMI értékek lekérdezése: Win32_PhysicalMemory -> Memória információk
            objPM = New ManagementObjectSearcher("SELECT SMBIOSMemoryType FROM Win32_PhysicalMemory")

            ' Értékek beállítása -> SMBIOS memória típus
            For Each Me.objMgmt In objPM.Get()
                TypeValue = objMgmt("SMBIOSMemoryType")
            Next

            ' Ismeretlen típus beállítása (Újabb, mint ami az SMBIOS listában szerepel!)
            If TypeValue <= UBound(SMBIOSMemoryType) Then
                MemoryType = SMBIOSMemoryType(TypeValue)
            Else
                MemoryType = Nothing
            End If

        Else

            ' Valós kiolvasott érték kiírása
            If TypeValue <= UBound(WMIMemoryType) Then
                MemoryType = WMIMemoryType(TypeValue)
            Else
                MemoryType = Nothing
            End If

        End If

        ' SDRAM korrekció (Néhány alaplap hibásan jelzi!)
        If MemoryType = "SDRAM" And MemoryClock >= 200 Then
            MemoryType = Nothing
        End If

        ' Memóriatípus megtippelése órajel alapján (Némi OC-val számolva!)
        If MemoryType = Nothing Then
            If MemoryClock < 66 Then
                MemoryType = Nothing
            ElseIf MemoryClock <= 175 Then
                MemoryType = "SDRAM"
            ElseIf MemoryClock <= 500 Then
                MemoryType = "DDR"
            ElseIf MemoryClock <= 950 Then
                MemoryType = "DDR2"
            ElseIf MemoryClock <= 1750 Then
                MemoryType = "DDR3"
            Else
                MemoryType = "DDR4"
            End If
        End If

        ' Memória típus beállítása
        If MemoryType <> Nothing Then
            RAMTableAddRow(GetLoc("RAMType"), MemoryType, Nothing)
        End If

        ' Memória órajel beállítása
        If MemoryType <> Nothing And MemoryClock <> 0 Then
            RAMTableAddRow(GetLoc("RAMClock"), FixNumberFormat(MemoryClock, 0, False), "MHz")
        End If

        ' Memória JEDEC típus azonosító beállítása
        If MemoryType <> Nothing And MemoryClock <> 0 Then
            If MemoryType = "SDRAM" Then
                MemoryIdent = "PC"
            ElseIf MemoryType = "DDR" Then
                MemoryIdent = "PC-"
            ElseIf MemoryType = "DDR2" Then
                MemoryIdent = "PC2-"
            ElseIf MemoryType = "DDR3" Then
                MemoryIdent = "PC3-"
            ElseIf MemoryType = "DDR4" Then
                MemoryIdent = "PC4-"
            End If

            ' Sor hozzáadása, ha nem üres
            If MemoryIdent <> Nothing Then

                ' SDRAM esetén csak az órajel, DDR esetén a 8-cal szorzott, 100-asra lefelé kerekített érték lesz az azonosító.
                If MemoryType = "SDRAM" Then
                    RAMTableAddRow(GetLoc("RAMIdent"), MemoryIdent + MemoryClock.ToString, Nothing)
                Else
                    RAMTableAddRow(GetLoc("RAMIdent"), MemoryIdent + (Fix(8 * MemoryClock / 100) * 100).ToString, Nothing)
                End If

            End If

        End If
        ' Memóriamodul sávszélességének beállítása
        If MemoryWidth <> 0 Then
            RAMTableAddRow(GetLoc("RAMWidth"), MemoryWidth.ToString, "bit")
        End If

    End Sub

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVÉNY: Sor hozzáadása a RAM-táblához ***
    ' Bemenet: Name  -> név (String)
    '          Value -> érték (String)
    '          Unit  -> mértékegység (String)
    ' Kimenet: *     -> hamis érték (Boolean)
    Private Function RAMTableAddRow(ByVal Name As String, ByVal Value As String, ByVal Unit As String)

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
                ListItem.SubItems(ListColumn).Font = New Font(RAM_Table.Font, FontStyle.Bold)
            Else

                ' Normál
                ListItem.SubItems(ListColumn).Font = New Font(RAM_Table.Font, FontStyle.Regular)
            End If

            ' Formátum beállítása
            ListItem.SubItems.Add(ListFields(ListColumn))
        Next

        ' Sor hozzáadása a lsitához
        RAM_Table.Items.Add(ListItem)


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