Imports System.Math
Imports System.Convert
Imports System.Management

Imports SINMx86.Functions
Imports SINMx86.Localization

' Főablak osztálya
Public Class MainWindow

    ' WMI feldolgozási objektumok
    Public objOS, objBB, objCS, objBS, objBT, objPR, objPM, objNA, objNI, objNC, objVC, objDD, objSM, objDP, objLP, objLD As ManagementObjectSearcher
    Public objMgmt, objRes As ManagementObject

    ' Főablak változói
    Public HWVendor(3), HWIdentifier(3) As String                                   ' Komponensinformációs tömbök
    Public OSRelease As Int32                                                       ' Kiadás típusa (32/64 bit)
    Public TraffGenCounter As Int32                                                 ' Diagram generálási időköz visszaszámlálója
    Public Hostname As String                                                       ' Hosztnév
    Public InterfaceList(32) As String                                              ' Interfészlista tömbje (lekérdezésekhez)
    Public InterfaceName(32) As String                                              ' Interfészek formázott neve (kiírásokhoz)
    Public InterfaceID(32) As String                                                ' Interfész lekérdezési indexe (ha az IP kapcsolat tiltott, akkor üres lesz)
    Public InterfacePresent As Boolean                                              ' Interfészek ellenőrzése (ha nincs egy sem, akkor hamis)
    Public DiskList(32) As String                                                   ' Meghajtóindexek tömbje (lekérdezésekhez)
    Public DiskName(32) As String                                                   ' Meghajtók neve (kiírásokhoz)
    Public DiskType(32) As String                                                   ' Meghajtó típusa: SSD/HDD (Ha nincs S.M.A.R.T, akkor üres)
    Public DiskSmart(32) As String                                                  ' Meghajtó S.M.A.R.T azonosítója (ha van, egyékbént üres)
    Public SmartException As Boolean = False                                        ' Hibakezelés S.M.A.R.T tábla lekérése esetén
    Public PartLabel(32) As String                                                  ' Partíció betűjele (kiírásokhoz)
    Public PartInfo(32) As String                                                   ' Partíció információk (kiírásokhoz)
    Public VideoName(32) As String                                                  ' Videókártyák nevei (kiírásokhoz)
    Public TraffResolution As Int32 = 60                                            ' Diagramon jelzett értékek száma (ennyi időegységre van felosztva a diagram)
    Public VerticalGrids As Int32 = 1                                               ' Fuggőleges osztóvonalak száma (másodpercre vetítve)
    Public GridSlip As Int32                                                        ' Függőleges rács eltolás
    Public GridUpdate As Boolean = False                                            ' Rácsfrissítés engedélyezése (kell az eltolás számításához)
    Public TimerLastTick As DateTime                                                ' Közvetlen időzítő időbélyegzője
    Public LatestDownload, LatestUpload As Double                                   ' Utolsó kiolvasott le- és feltöltési bájtok száma (az aktuális sebességszámításhoz kell)
    Public ChartStop As Boolean = False                                             ' Diagram leképezés leállítva (az időzítő által)
    Public ChartCreationTime As DateTime                                            ' Az utolsó diagram elkészülésének ideje
    Public DisableBalloon As Boolean = False                                        ' A "Kis méret ikonként" mellett felbukkanú üzenet tiltása (csak először jelenik meg)
    Public OpenFile As Boolean = False                                              ' Fájl megnyitása buboréküzenetnél (csak, ha mentés történt, egyéb esetben nem)
    Public SavePath As String = Nothing                                             ' Az utolsó mentett fájl elérési útja

    ' Forgalmi diagramok (2-vel több eleműnek kell lennie, mint a kijelzett érték!)
    Public TraffDownArray(TraffResolution + 2), TraffUpArray(TraffResolution + 2) As Double

    ' Checkboxok és menüelemek változói
    Public CheckedDownChart As Boolean = True                                       ' Letöltési diagram engedélyezése (alapérték: engedélyezve)
    Public CheckedUpChart As Boolean = True                                         ' Feltöltési diagram engedélyezése (alapérték: engedélyezve)

    ' *** FŐ ELJÁRÁS: Főablak betöltése (MyBase.Load -> MainWindow) ***
    ' Eseményvezérelt: Indítás
    Private Sub MainWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Főablak betöltés alatt
        MainWindowDone = False

        ' Alapértelmezett nyelv betöltése, a hiányzó nyelvi sztringek kiküszöbölése miatt!
        LoadLocalization(0)

        ' Taskbar ikon nevének beállítása -> Kezdeti (csak a név)
        MainNotifyIcon.Text = MyName

        ' ----- REGISTRY LEKÉRDEZÉSEK ÉS LISTÁK FELTÖLTÉSE -----

        ' Betöltési állapot beállítása -> Registry
        DebugLoadStage(GetLoc("LoadRegistry"))

        ' Registry értékek lekérdezése
        GetRegValues()

        ' Betöltési állapot beállítása -> Nyelv
        DebugLoadStage(GetLoc("LoadLanguage"))

        ' *** LISTAFELTÖLTÉS: Nyelv kiválasztása ***
        Dim LanguageList(UBound(Languages)) As String                               ' Nyelvlista
        Dim LanguageCount As Int32                                                  ' Nyelv sorszáma

        ' Nyelvi lista feltöltése
        For LanguageCount = 0 To UBound(Languages)
            LanguageList(LanguageCount) = ComboBox_LanguageList.Items.Add(Languages(LanguageCount))
        Next

        ' Kiválasztott listaelem állapotának beállítása
        ComboBox_LanguageList.SelectedIndex = LanguageList(SelectedLanguage)

        ' Taskbar ikon nevének beállítása -> Betöltés (név és betöltés felirat)
        MainNotifyIcon.Text = MyName + " - " + GetLoc("SplashLoad") + "..."

        ' Betöltési állapot beállítása -> Alapértékek
        DebugLoadStage(GetLoc("LoadDefaults"))

        ' XP esetén Splash letiltása (Felülírja a registry beállítást, mivel XP alatt hibásan jelenik meg!)
        If OSVersion(0) < 6 Then
            CheckedSplashDisable = True
            MainContextMenuItem_DisableSplash.Enabled = False
            MainMenu_SettingsItem_DisableSplash.Enabled = False
        End If

        ' Menüelem állapotának beállítása
        MainMenu_SettingsItem_DisableSplash.Checked = CheckedSplashDisable
        MainContextMenuItem_DisableSplash.Checked = CheckedSplashDisable

        ' Splash Screen betöltése és státusz frissítése: Registry
        If Not CheckedSplashDisable Then
            LoadSplash.Visible = True
        End If

        ' *** LISTAFELTÖLTÉS: Frissítési időköz ***
        Dim RefreshList(UBound(RefreshInterval)) As String                          ' Frissítési lista tömbje
        Dim RefreshItems As Int32                                                   ' Tömbelemek sorszáma

        ' Frissítési értékeket tartalmazó lista feltöltése
        For RefreshItems = 0 To UBound(RefreshInterval)
            RefreshList(RefreshItems) = ComboBox_UpdateList.Items.Add(RefreshInterval(RefreshItems).ToString)
        Next

        ' Lista állapotának beállítása
        ComboBox_UpdateList.SelectedIndex = RefreshList(SelectedRefresh)
        TraffGenCounter = ComboBox_UpdateList.SelectedIndex

        ' Checkbox és menüelemek állapotának beállítása
        MainMenu_ChartItem_DownloadVisible.Checked = CheckedDownChart
        MainMenu_ChartItem_UploadVisible.Checked = CheckedUpChart
        ChartMenuItem_DownloadVisible.Checked = CheckedDownChart
        ChartMenuItem_UploadVisible.Checked = CheckedUpChart
        CheckBoxChart_DownloadVisible.Checked = CheckedDownChart
        CheckBoxChart_UploadVisible.Checked = CheckedUpChart

        ' Ablak láthatóságának beállítása
        Me.TopMost = CheckedTopMost

        ' Jelző kép lecserélése
        If CheckedTopMost Then
            StatusLabel_TopMost.Image = My.Resources.Resources.Status_Pin_Green
        Else
            StatusLabel_TopMost.Image = My.Resources.Resources.Status_Pin_Red
        End If

        ' Menüelemek állapotának beállítása
        MainMenu_SettingsItem_TopMost.Checked = CheckedTopMost
        MainContextMenuItem_TopMost.Checked = CheckedTopMost

        ' Menüelemek állapotának beállítása
        MainMenu_SettingsItem_TaskbarMinimize.Checked = CheckedMinToTray
        MainContextMenuItem_TaskbarMinimize.Checked = CheckedMinToTray

        ' Menüelem állapotának beállítása
        MainMenu_SettingsItem_DisableConfirm.Checked = CheckedNoQuitAsk
        MainContextMenuItem_DisableConfirm.Checked = CheckedNoQuitAsk

        ' ----- WMI LEKÉRDEZÉSEK -----

        ' Betöltési állapot beállítása -> Uptime
        DebugLoadStage(GetLoc("LoadHostname"))

        ' *** WMI LEKÉRDEZÉS: Win32_ComputerSystem -> Számítógép információi ***
        objCS = New ManagementObjectSearcher("SELECT Name FROM Win32_ComputerSystem")

        ' Értékek beállítása -> Hosztnév
        For Each Me.objMgmt In objCS.Get()
            Hostname = objMgmt("Name")
        Next

        ' Hosztnév beálítása az állapotsorban
        StatusLabel_Host.Text = GetLoc("Hostname") + ": " + Hostname

        ' Diagram leképezés állapotának alaphelyzetbe állítása
        StatusLabel_ChartStatus.Image = My.Resources.Resources.Status_Check
        StatusLabel_ChartStatus.Text = GetLoc("ChartReset")

        ' ----- KEZDŐÉRTÉK BEÁLLÍTÁSOK -----
        ' Megjegyzés: a betöltési állapot beállítása innentől nincs kommentezve!

        ' *** KEZDŐÉRTÉK BEÁLLÍTÁS: Futásidő ***
        DebugLoadStage(GetLoc("LoadUptime"))
        SetUptime()

        ' *** KEZDŐÉRTÉK BEÁLLÍTÁS: Memória információk ***
        DebugLoadStage(GetLoc("LoadMemory"))
        SetMemoryInformation()

        ' ----- LISTÁK FELTÖLTÉSE -----

        ' *** LISTAFELTÖLTÉS: Memóriamodulok ***
        UpdateMemList(True)

        ' *** LISTAFELTÖLTÉS: Hardver komponensek ***
        DebugLoadStage(GetLoc("LoadHardware"))
        UpdateHWList(True)

        ' *** LISTAFELTÖLTÉS: Processzorok ***
        DebugLoadStage(GetLoc("LoadProcessor"))
        UpdateCPUList(True)

        ' *** LISTAFELTÖLTÉS: Videokártyák ***
        DebugLoadStage(GetLoc("LoadVideo"))
        UpdateVideoList(True)

        ' *** LISTAFELTÖLTÉS: Lemezmeghajtók ***
        DebugLoadStage(GetLoc("LoadDisk"))
        UpdateDiskList(True)

        ' *** LISTAFELTÖLTÉS: Interfészek ***
        DebugLoadStage(GetLoc("LoadNetwork"))
        UpdateInterfaceList(True)

        ' ----- ZÁRÓ MŰVELETEK -----

        ' Kezdeti időbélyegző beállítása az időzítőhöz
        TimerLastTick = DateTime.Now

        ' Időzítő indítása: EventTimer (1 másodperc)
        EventTimer.Interval = 1000
        EventTimer.Enabled = True

        ' Diagram frissítése (gyakorlatilag ez a kezdő reset, mert az óra már ketyeg, de betöltés előtt nem mér!)
        MakeChart(True)

        ' Debug sztring kiürítése
        Value_Debug.Text = Nothing

        ' Splash ablak bezárása
        If Not CheckedSplashDisable Then
            LoadSplash.Close()
        End If

        ' Taskbar ikon nevének beállítása -> Végleges (név és verziószám)
        MainNotifyIcon.Text = MyName + " - " + GetLoc("Version") + " " + VersionString

        ' Főablak betöltése kész
        MainWindowDone = True

        ' Előtérbe hozás
        Me.BringToFront()

    End Sub

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVÉNY: Hardver komponensek értékeinek beállítása ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Private Function SetHWInformation()

        ' Értékek definiálása
        Dim Vendor As String = Nothing                      ' Gyártó
        Dim Model As String = Nothing                       ' Modell
        Dim Identifier As String = Nothing                  ' Azonosító
        Dim BattCount As Int32 = 0                          ' Akkumulátorok száma
        Dim BattVolt As Int32 = 0                           ' Akkumulátor névleges feszültsége

        ' OEM feketelistás sztringek (Dummy szövegek, amelyeket a gyártó "elfelejtett" kitölteni.)
        Dim Blacklist() As String = {"To be filled by O.E.M.", "Not Available", "Default string", "System manufacturer", "System Product Name", "System Serial Number", "Base Board Serial Number"}

        ' WMI értékek lekérdezése: Win32_Baseboard -> Alaplap információ
        objBB = New ManagementObjectSearcher("SELECT Manufacturer, Product, SerialNumber FROM Win32_Baseboard")

        ' Értékek beállítása -> Alaplap: gyártó, modell, sorozatszám
        For Each Me.objMgmt In objBB.Get()
            Vendor = RemoveSpaces(objMgmt("Manufacturer"))
            Model = RemoveSpaces(objMgmt("Product"))
            Identifier = RemoveSpaces(objMgmt("SerialNumber"))
        Next

        ' Értéktároló tömb frissítése -> Alaplap
        If Vendor = Nothing Or CheckStrMatch(Vendor, Blacklist, False) Then
            HWVendor(0) = Nothing
        Else
            HWVendor(0) = RemoveInvalidChars(Vendor)
        End If

        If Model = Nothing Or CheckStrMatch(Model, Blacklist, False) Then
            HWIdentifier(0) = Nothing
        ElseIf Identifier = Nothing Or CheckStrMatch(Identifier, Blacklist, False) Then
            HWIdentifier(0) = RemoveInvalidChars(Model)
        Else
            HWIdentifier(0) = RemoveInvalidChars(Model) + ", " + GetLoc("Serial") + ": " + RemoveInvalidChars(Identifier)
        End If

        ' WMI értékek lekérdezése: Win32_ComputerSystem -> Számítógép információi
        objCS = New ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem")

        ' Értékek beállítása -> Számítógép: gyártó, modell
        For Each Me.objMgmt In objCS.Get()
            Vendor = RemoveSpaces(objMgmt("Manufacturer"))
            Model = RemoveSpaces(objMgmt("Model"))
        Next

        ' WMI értékek lekérdezése: Win32_BIOS -> BIOS információk
        ' Megjegyzés: A rendszer sorozatszáma is itt van tárolva!
        objBS = New ManagementObjectSearcher("SELECT SerialNumber, Manufacturer, SMBIOSBIOSVersion, ReleaseDate FROM Win32_BIOS")

        ' Értékek beállítása -> Számítógép: sorozatszám
        For Each Me.objMgmt In objBS.Get()
            Identifier = RemoveSpaces(objMgmt("SerialNumber"))
        Next

        ' Értéktároló tömb frissítése -> Számítógép
        If Vendor = Nothing Or CheckStrMatch(Vendor, Blacklist, False) Then
            HWVendor(1) = Nothing
        Else
            HWVendor(1) = RemoveInvalidChars(Vendor)
        End If

        If Model = Nothing Or CheckStrMatch(Model, Blacklist, False) Then
            HWIdentifier(1) = Nothing
        Else
            If Identifier = Nothing Or CheckStrMatch(Identifier, Blacklist, False) Then
                HWIdentifier(1) = RemoveInvalidChars(Model)
            Else
                HWIdentifier(1) = RemoveInvalidChars(Model) + ", " + GetLoc("Serial") + ": " + RemoveInvalidChars(Identifier)
            End If
        End If

        ' Értékek beállítása -> BIOS: gyártó, verziószám, dátum
        For Each Me.objMgmt In objBS.Get()
            Vendor = RemoveSpaces(objMgmt("Manufacturer"))
            Model = RemoveSpaces(objMgmt("SMBIOSBIOSVersion"))
            Identifier = Format(ManagementDateTimeConverter.ToDateTime(objMgmt("ReleaseDate").ToString), "yyyy-MM-dd")
        Next

        ' Értéktároló tömb frissítése -> BIOS
        If Vendor = Nothing Or CheckStrMatch(Vendor, Blacklist, False) Then
            HWVendor(2) = Nothing
        Else
            HWVendor(2) = RemoveInvalidChars(Vendor)
        End If

        If Model = Nothing Then
            HWIdentifier(2) = Nothing
        Else
            HWIdentifier(2) = RemoveInvalidChars(Model) + ", " + GetLoc("Date") + ": " + Identifier
        End If

        ' WMI értékek lekérdezése: Win32_BIOS -> Akkumulátor információ
        objBT = New ManagementObjectSearcher("SELECT Name, DeviceID, DesignVoltage FROM Win32_Battery")

        ' Akkumulátorok számának meghatározása
        BattCount = objBT.Get().Count

        ' Hibakezelés: nincs akkumulátor
        If BattCount = 0 Then
            HWVendor(3) = Nothing
            HWIdentifier(3) = Nothing
        Else

            ' Értékek beállítása -> Akkumulátor: név, azonosító, feszültség
            For Each Me.objMgmt In objBT.Get()
                Vendor = RemoveSpaces(RemoveInvalidChars(objMgmt("DeviceID")))
                Model = RemoveSpaces(RemoveInvalidChars(objMgmt("Name")))
                BattVolt = objMgmt("DesignVoltage")
            Next

            ' Gyártó leválasztása a modellről
            Vendor = Replace(Vendor, Model, "")

            ' Hibakezelés: hibás feszültségérték (0 V)
            If BattVolt <> 0 Then
                Identifier = FixNumberFormat((BattVolt / 1000), 1, False).ToString + " V"
            Else
                Identifier = Nothing
            End If

            ' Hibakezelés: üres gyártói sztring
            If Vendor = Nothing Then
                HWVendor(3) = Nothing
            Else
                HWVendor(3) = Vendor
            End If

            ' Hibakezelés: üres modell sztring
            If Model = Nothing Then
                HWIdentifier(3) = Nothing
            Else
                If Identifier = Nothing Then
                    HWIdentifier(3) = Model
                Else
                    HWIdentifier(3) = Model + ", " + GetLoc("Volt") + ": " + Identifier
                End If
            End If
        End If

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Processzor aktuális órajelének beállítása ***
    ' Bemenet: UpdateCores -> magok és szálak frissítésének szükségessége (Boolean)
    ' Kimenet: *           -> hamis érték (Boolean)
    Private Function SetCPUInformation(ByVal UpdateCores As Boolean)

        ' Értékek definiálása
        Dim CPUCoreNumber As Int32 = 0                          ' Magok száma
        Dim CPUThreadNumber As Int32 = 0                        ' Logikai szálak száma
        Dim CPUCurrentClock, CPUMaximumClock As Int32           ' Jelenlegi és natív órajel
        Dim CPUCount As Int32 = 0                               ' Processzor sorszáma

        ' Magok és szálak számának lekérdezése (Kiválasztáskor!)
        If UpdateCores Then

            ' WMI értékek lekérdezése: Win32_Processor -> Magok és szálak száma
            objPR = New ManagementObjectSearcher("SELECT NumberOfCores, NumberOfLogicalProcessors FROM Win32_Processor")

            ' Értékek beállítása -> Processzor: magok, szálak
            For Each Me.objMgmt In objPR.Get()
                If CPUCount = SelectedCPU Then
                    CPUCoreNumber = objMgmt("NumberOfCores")
                    CPUThreadNumber = objMgmt("NumberOfLogicalProcessors")
                End If
                CPUCount += 1
            Next

            ' Kiírások frissítése -> Magok / Szálak
            If UpdateCores Then Value_CPUCore.Text = CPUCoreNumber.ToString + " / " + CPUThreadNumber.ToString

        End If

        ' Processzor számláló nullázása
        CPUCount = 0

        ' WMI értékek lekérdezése: Win32_Processor -> Órajelek
        objPR = New ManagementObjectSearcher("SELECT CurrentClockSpeed, MaxClockSpeed FROM Win32_Processor")

        ' Értékek beállítása -> Processzor: aktuális és gyári órajelek
        For Each Me.objMgmt In objPR.Get()
            If CPUCount = SelectedCPU Then
                CPUCurrentClock = objMgmt("CurrentClockSpeed")
                CPUMaximumClock = objMgmt("MaxClockSpeed")
            End If
            CPUCount += 1
        Next

        ' Kiírások frissítése -> Órajelek
        Value_CPUClock.Text = FixNumberFormat(CPUCurrentClock, 0, False) + " MHz"
        Value_CPUMaximum.Text = FixNumberFormat(CPUMaximumClock, 0, False) + " MHz"

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Operációs rendszer információk beállítása ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Private Function SetOSInformation()

        ' Értékek definiálása
        Dim OSName As String = Nothing                          ' OS neve
        Dim OSService As Int32 = 0                              ' Szervizcsomag
        Dim OSLanguage(32) As String                            ' Nyelv
        Dim OSRelease As String = Nothing                       ' Kiadás típusa

        ' WMI értékek lekérdezése: Win32_OperatingSystem -> Operációs rendszer információi
        objOS = New ManagementObjectSearcher("SELECT Caption, ServicePackMajorVersion FROM Win32_OperatingSystem")

        ' Értékek beállítása -> Operációs rendszer: gyártó, modell
        For Each Me.objMgmt In objOS.Get()
            OSName = RemoveSpaces(objMgmt("Caption"))
            OSService = objMgmt("ServicePackMajorVersion")
        Next

        ' WMI értékek lekérdezése: Win32_Processor -> Operációs rendszer kiadás (32/64-bit)
        ' Megjegyzés: Az OS kiadásnál egyes esetekben sajnos le van fordítva a sztring (pl. '32-bites'),
        ' de a processzor címbusz szélessége mindig megegyezik vele, mivel ezt az OS korlátozza, ez viszont integer!
        ' XP-nél nincs alapból az OS kiadásnál ilyen WMI érték, de a CPU címbuszból ott is származtatható.
        objPR = New ManagementObjectSearcher("SELECT AddressWidth FROM Win32_Processor")

        ' Értékek beállítása
        For Each Me.objMgmt In objPR.Get()
            OSRelease = objMgmt("AddressWidth").ToString
        Next

        ' Kiírások frissítése
        If OSService = 0 Then
            Value_OSName.Text = OSName
        Else
            Value_OSName.Text = OSName + ", " + GetLoc("SvcPack") + " " + OSService.ToString
        End If

        ' Kiírások frissítése
        Value_OSRelease.Text = OSRelease + "-bit"
        Value_OSVersion.Text = OSVersion(0).ToString + "." + OSVersion(1).ToString + "." + OSVersion(2).ToString

        ' NT6+ értékek lekérdezése (A korábbiaknál üres értékkel való feltöltés!)
        If OSVersion(0) >= 6 Then

            ' WMI értékek lekérdezése: Win32_OperatingSystem -> Operációs rendszer információi
            objOS = New ManagementObjectSearcher("SELECT MUILanguages FROM Win32_OperatingSystem")

            ' Értékek beállítása -> Operációs rendszer: nyelv
            For Each Me.objMgmt In objOS.Get()
                OSLanguage = objMgmt("MUILanguages")
            Next

            ' Kiírások frissítése
            Value_OSLang.Enabled = True
            Value_OSLang.Text = OSLanguage(0)
        Else
            Value_OSLang.Enabled = False
            Value_OSLang.Text = GetLoc("Unknown")
        End If

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Memória információk beállítása ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Private Function SetMemoryInformation()

        ' Értékek definiálása
        Dim ModuleSum As Int64                                  ' Memória modulok összmérete (rekurzívan összeadódik)
        Dim MemorySize(1), MemoryUsable(1) As Double            ' Formázott modul összméret és a rendszer által felhasználható memória
        Dim MemoryCount = 0                                     ' Memória modul számláló
        Dim MemoryClock, TypeValue As Int32                     ' Órajel és típus azonosító
        Dim MemoryType As String                                ' Memória modulok típusa

        ' WMI értékek lekérdezése: Win32_PhysicalMemory -> Memória információk
        objPM = New ManagementObjectSearcher("SELECT Capacity, Speed, MemoryType FROM Win32_PhysicalMemory")

        ' Értékek beállítása -> memória modulok tulajdonságai
        For Each Me.objMgmt In objPM.Get()

            ' Összméret növelése a jelenlegi modullal
            ModuleSum += objMgmt("Capacity")

            ' Órajel és memória típus beállítása (Csak egyszer!)
            If MemoryCount = 0 Then
                MemoryClock = objMgmt("Speed")
                TypeValue = objMgmt("MemoryType")
            End If

            ' Modul számának növelése
            MemoryCount += 1

        Next

        ' Memória összméret konvertálása és formázása
        MemorySize = ScaleConversion(ModuleSum, 0, True)
        Value_MemSize.Text = FixNumberFormat(MemorySize(0), 2, False) + " " + BytePrefix(MemorySize(1)) + "B"

        ' WMI értékek lekérdezése: Win32_OperatingSystem -> Memória információk (kiB-ban vannak az értékek!)
        objOS = New ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem")

        ' Értékek beállítása -> Számítógép: felhasználható memória mérete
        For Each Me.objMgmt In objOS.Get()
            MemoryUsable = ScaleConversion(objMgmt("TotalVisibleMemorySize") * 1024, 2, True)
        Next

        ' Felhasználható memória formázása és kiírása
        Value_MemVisible.Text = FixNumberFormat(MemoryUsable(0), 2, False) + " " + BytePrefix(MemoryUsable(1)) + "B"

        ' Memória órajel beállítása
        If MemoryClock = 0 Then
            Value_MemClock.Enabled = False
            Value_MemClock.Text = GetLoc("Unknown")
        Else
            Value_MemClock.Enabled = True
            Value_MemClock.Text = FixNumberFormat(MemoryClock, 0, False) + " MHz"
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
            If TypeValue > UBound(SMBIOSMemoryType) Then
                Value_MemType.Enabled = False
                MemoryType = GetLoc("Unknown")
            Else
                MemoryType = SMBIOSMemoryType(TypeValue)
            End If

        ElseIf TypeValue > UBound(WMIMemoryType) Then

            ' Ismeretlen típus beállítása (Újabb, mint ami a WMI listában szerepel!)
            Value_MemType.Enabled = False
            MemoryType = GetLoc("Unknown")

        Else

            ' Valós kiolvasott érték kiírása
            MemoryType = WMIMemoryType(TypeValue)

        End If

        ' SDRAM korrekció (Néhány alaplap hibásan jelzi!)
        If MemoryType = "SDRAM" And MemoryClock >= 200 Then
            MemoryType = Nothing
        End If

        ' Memóriatípus megtippelése órajel alapján (Némi OC-val számolva!)
        If MemoryType = Nothing Then
            If MemoryClock < 66 Then
                Value_MemType.Enabled = False
                MemoryType = GetLoc("Unknown")
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

        ' Memóriatípus érékének beállítása
        Value_MemType.Text = MemoryType

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Lemezmeghajtó információk beállítása ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Private Function SetDiskInformation()

        ' Értékek definiálása
        Dim DiskIndex As Int32 = 0                          ' Lemez sorszáma
        Dim Connector As String = Nothing                   ' Csatolófelület
        Dim Firmware As String = Nothing                    ' Firmware revízió
        Dim SerialNumber As String = Nothing                ' Sorozatszám
        Dim DiskID As String = Nothing                      ' Lemez azonosító

        ' WMI értékek lekérdezése: Win32_DiskDrive -> Lemezmeghajtó információk
        objDD = New ManagementObjectSearcher("SELECT InterfaceType, Index, DeviceID FROM Win32_DiskDrive WHERE Index = '" + DiskList(SelectedDisk) + "'")

        ' Értékek beállítása -> Lemezmeghajtó: index, interfész, azonosító
        For Each Me.objMgmt In objDD.Get()
            Connector = objMgmt("InterfaceType")
            DiskIndex = objMgmt("Index")
            DiskID = objMgmt("DeviceID")
        Next

        ' NT6+ értékek lekérdezése (A korábbiaknál üres értékkel való feltöltés!)
        If OSVersion(0) >= 6 Then

            ' WMI értékek lekérdezése: Win32_DiskDrive -> Lemezmeghajtó információk
            objDD = New ManagementObjectSearcher("SELECT SerialNumber, FirmwareRevision FROM Win32_DiskDrive WHERE Index = '" + DiskList(SelectedDisk) + "'")

            ' Értékek beállítása -> Lemezmeghajtó: firmware, sorozatszám
            For Each Me.objMgmt In objDD.Get()
                SerialNumber = RemoveSpaces(objMgmt("SerialNumber"))
                Firmware = RemoveSpaces(objMgmt("FirmwareRevision"))
            Next
        Else
            SerialNumber = Nothing
            Firmware = Nothing
        End If

        ' Csatolófelület beállítása
        Value_DiskInterface.Enabled = True

        If Connector = "IDE" Then
            Value_DiskInterface.Text = "IDE / SATA"
        ElseIf Connector = "SCSI" Then
            Value_DiskInterface.Text = "SCSI / SAS"
        Else
            Value_DiskInterface.Text = RemoveInvalidChars(Connector)
        End If

        ' Lemez típus beállítása
        If DiskType(SelectedDisk) <> Nothing Then
            Value_DiskType.Enabled = True
            Value_DiskType.Text = DiskType(SelectedDisk)
        Else
            ' Hiányzó információk feltöltése (SCSI -> RAID tömb, USB -> USB lemez)
            If Connector = "SCSI" Then
                Value_DiskType.Enabled = True
                Value_DiskType.Text = GetLoc("RAIDDisk")
            ElseIf Connector = "USB" Then
                Value_DiskType.Enabled = True
                Value_DiskType.Text = GetLoc("USBDisk")
            Else
                Value_DiskType.Enabled = False
                Value_DiskType.Text = GetLoc("Unknown")
            End If
        End If

        ' Firmware revízió beállítása
        If RemoveInvalidChars(Firmware) = Nothing Then
            Value_DiskFirmware.Enabled = False
            Value_DiskFirmware.Text = GetLoc("NotAvailable")
        Else
            Value_DiskFirmware.Enabled = True
            Value_DiskFirmware.Text = RemoveInvalidChars(Firmware)
        End If

        ' OS Verzióellenőrzés: XP -> Ismeretlen, Vista és 7 -> Konverzió indítása
        If OSVersion(0) < 6 Then
            SerialNumber = Nothing
        ElseIf OSVersion(0) = 6 And OSVersion(1) <= 1 And SerialNumber <> Nothing Then
            SerialNumber = RemoveSpaces(DiskSerialNumberConv(SerialNumber))
        End If

        ' Sorozatszám beállítása
        If RemoveInvalidChars(SerialNumber) = Nothing Then
            Value_DiskSerial.Enabled = False
            Value_DiskSerial.Text = GetLoc("NotAvailable")
        Else
            Value_DiskSerial.Enabled = True
            Value_DiskSerial.Text = RemoveInvalidChars(SerialNumber)
        End If

        ' Partícióelemzéshez használt változók
        Dim PartNum As Int32 = 0                            ' Partíciók száma
        Dim PartList(32) As String                          ' Partíciók listája
        Dim PartCount As Int32 = 0                          ' Partíció sorszáma
        Dim PartID As String = Nothing                      ' Partíció azonosítója
        Dim PartName As String = Nothing                    ' Partíció neve
        Dim PartSize(2) As Double                           ' Partíció mérete
        Dim PartFS As String = Nothing                      ' Partíció fájlrendszere
        Dim PartUnit As String = Nothing                    ' Partíció méret mértékegység előtag

        ' WMI értékek összevetése: Win32_DiskDrive -> Win32_DiskDriveToDiskPartition (Referencia: DeviceID)
        objDP = New ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + DiskID + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition")

        ' Értékek beállítása -> Lemez azonosító
        For Each Me.objMgmt In objDP.Get()
            PartID = objMgmt("DeviceID")

            ' WMI értékek összevetése: Win32_DiskPartition -> Win32_LogicalDiskToPartition (Referencia: DeviceID)
            objLP = New ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + PartID + "'} WHERE AssocClass = Win32_LogicalDiskToPartition")

            ' Értékek beállítása -> Partíció azonosító
            For Each Me.objRes In objLP.Get()
                PartList(PartNum) = objRes("DeviceID")
                PartNum += 1
            Next
        Next

        ' Tömbök és lista kiürítése -> Partíció információk
        Array.Clear(PartLabel, 0, UBound(PartLabel))
        Array.Clear(PartInfo, 0, UBound(PartInfo))
        ComboBox_PartList.Items.Clear()

        ' Partíció információk lekérdezése
        If PartNum = 0 Then
            ComboBox_PartList.Enabled = False
            ComboBox_PartList.Items.Add(GetLoc("NotAvailable"))
        Else
            ComboBox_PartList.Enabled = True
            For PartCount = 0 To PartNum - 1

                ' WMI értékek lekérdezése: Win32_LogicalDisk -> Partíció információk
                objLD = New ManagementObjectSearcher("SELECT DeviceID, FileSystem, Size, VolumeName FROM Win32_LogicalDisk WHERE DeviceID = '" + PartList(PartCount) + "'")

                ' Értékek beállítása -> Partíció: azonosító, fájlrendszer, méret, kötetcímke
                For Each Me.objMgmt In objLD.Get()

                    ' Meghajtó betűjel
                    PartList(PartCount) = objMgmt("DeviceID")

                    If objMgmt("FileSystem") = Nothing Then
                        PartFS = "RAW"
                    Else
                        PartFS = objMgmt("FileSystem")
                    End If

                    If objMgmt("VolumeName") = Nothing Then
                        PartName = GetLoc("NoName")
                    Else
                        PartName = objMgmt("VolumeName")
                    End If

                    ' Kötetcímke beéllítása
                    PartLabel(PartCount) = PartList(PartCount) + "\"

                    ' Listaelem hozzáadása
                    ComboBox_PartList.Items.Add("# " + (PartCount + 1).ToString + "/" + PartNum.ToString + " (" + PartFS + ")")

                    ' Partícióinformáció feltöltése
                    If objMgmt("Size") = 0 Then
                        PartInfo(PartCount) = Nothing
                    Else
                        ' Partícióméret konvertálása
                        PartSize = ScaleConversion(objMgmt("Size"), 2, True)
                        PartInfo(PartCount) = PartName + " (" + FixNumberFormat(PartSize(0), 2, True) + " " + BytePrefix(PartSize(1)) + "B)"
                    End If

                Next
            Next
        End If

        ' Partíciólista beállítása (kiválasztott elem)
        ComboBox_PartList.SelectedIndex = SelectedPartition

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Videokártya információk beállítása ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Private Function SetVideoInformation()

        ' WMI LEKÉRDEZÉS: Win32_VideoController -> Videovezérlő
        objVC = New ManagementObjectSearcher("SELECT AdapterRAM, CurrentHorizontalResolution, CurrentVerticalResolution, CurrentBitsPerPixel FROM Win32_VideoController")

        ' Értékek definiálása
        Dim VideoMemory As Double                               ' Video memória
        Dim VideoResolution(2) As Int32                         ' Képernyőfelbontás
        Dim VideoCount As Int32 = 0                             ' Videokártya sorszáma

        ' Értékek beállítása
        For Each Me.objMgmt In objVC.Get()
            If VideoCount = SelectedVideo Then
                VideoMemory = objMgmt("AdapterRAM")
                VideoResolution(0) = objMgmt("CurrentHorizontalResolution")
                VideoResolution(1) = objMgmt("CurrentVerticalResolution")
                VideoResolution(2) = objMgmt("CurrentBitsPerPixel")
            End If
            VideoCount += 1
        Next

        ' Nulla bájtos memória korrekció -> Ismeretlen!
        If VideoMemory = 0 Then
            Value_VideoMemory.Enabled = False
            Value_VideoMemory.Text = GetLoc("NotAvailable")
        Else
            ' Memóriaérték formázása
            Dim VideoMemoryConv(2) As Double
            VideoMemoryConv = ScaleConversion(VideoMemory, 2, True)

            ' Kiírás értékének frissítése
            Value_VideoMemory.Enabled = True
            Value_VideoMemory.Text = FixNumberFormat(VideoMemoryConv(0), 2, True) + " " + BytePrefix(VideoMemoryConv(1)) + "B"
        End If

        ' Ismeretlen felbontás (Pl.: 0 x 0, 0 bit)
        If VideoResolution(0) = 0 Or VideoResolution(1) = 0 Or VideoResolution(2) = 0 Then
            Value_VideoResolution.Enabled = False
            Value_VideoResolution.Text = GetLoc("Inactive")
        Else
            Value_VideoResolution.Enabled = True
            Value_VideoResolution.Text = VideoResolution(0).ToString + " x " + VideoResolution(1).ToString + " (" + VideoResolution(2).ToString + " bit)"
        End If

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Rendszer futási idő beállítása ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Private Function SetUptime()

        ' Értékek definiálása
        Dim UptimeSeconds As Int32                              ' Indítás óta eltelt másodpercek száma
        Dim SysStartTime, CurrentTime As DateTime               ' Indításkori és jelenlegi rendszeridő
        Dim Days, Hours, Minutes, Seconds As Int32              ' Időváltozók (nap, óra, perc, másodperc)
        Dim UptimeString As String = Nothing                    ' Futásidő sztring

        ' *** WMI LEKÉRDEZÉS: Win32_OperatingSystem -> Indításkori és jelenlegi rendszeridő ***
        ' Megjegyés: eltérő értéket mutat, ezért ez a referencia az uptime-hoz.
        objOS = New ManagementObjectSearcher("SELECT LastBootUpTime, LocalDateTime FROM Win32_OperatingSystem")

        ' Értékek beállítása -> Indítási és jelenlegi idő
        For Each Me.objMgmt In objOS.Get()
            SysStartTime = ManagementDateTimeConverter.ToDateTime(objMgmt("LastBootUpTime"))
            CurrentTime = ManagementDateTimeConverter.ToDateTime(objMgmt("LocalDateTime"))
        Next

        ' Futásidő érték számítása (másodperc)
        UptimeSeconds = DateDiff("s", SysStartTime, CurrentTime)

        ' Érvénytelen időintervallum keresése
        If UptimeSeconds < 0 Then

            ' Érvénytelen idő (A jelenlegi idő korábban van, mint az indításkori idő!)
            StatusLabel_Uptime.Text = GetLoc("Uptime") + ": " + GetLoc("Invalid")

        Else

            ' Egységekre bontás (napok, órák, percek, másodpercek)
            Days = Int(UptimeSeconds / (24 * 3600))
            UptimeSeconds = UptimeSeconds - (Days * 24 * 3600)
            Hours = Int(UptimeSeconds / 3600)
            UptimeSeconds = UptimeSeconds - (Hours * 3600)
            Minutes = Int(UptimeSeconds / 60)
            UptimeSeconds = UptimeSeconds - (Minutes * 60)
            Seconds = Int(UptimeSeconds)

            ' Kiírás frissítése
            StatusLabel_Uptime.Text = GetLoc("Uptime") + ": " + Days.ToString + " " + GetLoc("Days") + ", " + Hours.ToString + " " + GetLoc("Hours") + ", " +
                                      Minutes.ToString + " " + GetLoc("Mins") + " " + GetLoc("And") + " " + Seconds.ToString + " " + GetLoc("Secs") + "."
        End If

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Statisztikai név konverzió (Nem visszafordítható névátalakítás!) ***
    ' Bemenet: RawString -> formázandó sztring (String)
    ' Kimenet: RawString -> formázott sztring (String)
    Public Function StatNameConv(ByVal RawString As String)

        ' Értékek definiálása
        Dim SearchCount As Int32                                ' Keresendő sztring sorszáma
        Dim ModifyCount As Int32                                ' Törlendő sztring sorszáma

        ' Sztring cserék változói (eredeti, csere)
        Dim SearchList() As String = {"(", ")"}
        Dim ReplaceList() As String = {"[", "]"}

        ' Névben átírandó sztringek tömbje
        Dim ModifyList() As String = {"/", "\", "#", "+"}

        ' Korrekciós sztringek keresése és cseréje
        For SearchCount = 0 To UBound(SearchList)
            RawString = Replace(RawString, SearchList(SearchCount), ReplaceList(SearchCount))
        Next

        ' Átírandó sztringek keresése és cseréje
        For ModifyCount = 0 To UBound(ModifyList)
            RawString = Replace(RawString, ModifyList(ModifyCount), "_")
        Next

        ' Visszatérési érték beállítása
        Return RawString

    End Function

    ' *** FÜGGVÉNY: Lemez sorozatszám korrekció (Csak Windows 7 esetén szükséges!) ***
    ' Bemenet: Value     -> sorozatszám (String)
    ' Kimenet: ConvValue -> formázott érték (String)
    Private Function DiskSerialNumberConv(ByVal Value As String)

        ' Értékek definiálása
        Dim TempHex As String                                   ' Aktuális karakter ASCII kódja (hexadecimális)
        Dim TempChar As Int32                                   ' Aktuális karakter ASCII kódja (decimális)
        Dim Position As Int32                                   ' Aktuális karakter pozíciója
        Dim ConvValue As String = Nothing                       ' Visszatérési érték
        Dim ConvCount As Int32                                  ' Konvertálandó karakter sorszáma

        ' Függő értékek definiálása
        Dim HexArr() As Char = Value.ToCharArray                ' Hexadecimális karakterek tömbje
        Dim CharNum As Int32 = UBound(HexArr) + 1               ' Karakterek száma
        Dim CharArr((CharNum / 2) - 1) As String                ' Összefűzési karaktertömb

        ' Érvénytelen sorozatszám ellenőrzés
        If Value.Length <> 40 Then

            ' Ha nem pont 40 karakter, akkor konvertálás nélkül írja ki!
            ConvValue = Value

        Else

            ' Konverzió indítása
            For Position = 0 To (CharNum / 2) - 1
                TempHex = HexArr(Position * 2) + HexArr((Position * 2) + 1)
                TempChar = ToInt32(TempHex, 16)

                ' Bájtsorrend javítása (Páros-páratlan csere)
                If Position Mod 2 <> 0 Then
                    CharArr(Position - 1) = Chr(TempChar).ToString
                Else
                    CharArr(Position + 1) = Chr(TempChar).ToString
                End If
            Next

            ' Konvertált sztring összefűzése
            For ConvCount = 0 To UBound(CharArr)
                ConvValue = ConvValue + CharArr(ConvCount)
            Next

        End If

        ' Visszatérési érték beállítása
        Return ConvValue

    End Function

    ' *** FÜGGVÉNY: Sebesség értékek frissítése a statisztikához (folyamatos) ***
    ' Bemenet: TraffReset -> forgalom nullázása (Boolean)
    ' Kimenet: *          -> hamis érték (Boolean)
    Private Function UpdateSpeedStatistics(ByVal TraffReset As Boolean)

        ' Értékek definiálása
        Dim MaxBandwidth(2) As Double                           ' Maximális sávszélesség (érték, prefixum sorszáma)
        Dim MaxRelativeSpeed As Double                          ' Legnagyobb felvehető sebesség érték
        Dim CurrentDownload, CurrentUpload As Double            ' Jelenlegi le- és feltöltött bájtok száma
        Dim DownloadSpeed(2), UploadSpeed(2) As Double          ' Jelenlegi sebesség értékek (2 dimenziós: érték, prefixum sorszáma)
        Dim CurrentUsage As Int32                               ' Jelenlegi kihasználtság
        Dim UsageValue As Double                                ' Kihasználtsági érték
        Dim UIntCorrection As Double                            ' Előjel nélküli integer korrekció

        ' Előjel nélküli integer korrekció -> Előjel átfordulás kikerülése
        ' Megjegyzés: XP alatt csak UInt32-ben voltak tárolva ezek az értékek, de ez NT 6.0-tól UInt64-re változott!
        If OSVersion(0) < 6 Then
            UIntCorrection = 2 ^ 31
        Else
            UIntCorrection = 2 ^ 63
        End If

        ' Interfész jelenlétének ellenőrzése
        If InterfacePresent Then

            ' WMI értékek lekérdezése: Win32_PerfRawData_Tcpip_NetworkInterface -> Forgalmi adatok
            objNI = New ManagementObjectSearcher("SELECT CurrentBandwidth, BytesReceivedPersec, BytesSentPersec FROM Win32_PerfRawData_Tcpip_NetworkInterface WHERE Name = '" + InterfaceList(SelectedInterface) + "'")

            ' Interfész eltűnésének ellenőrzése
            If objNI.Get().Count = 0 Then

                ' Diagram leképezés leállítása
                ChartStop = True

                ' Diagram állapotkijelzés frissítése
                StatusLabel_ChartStatus.Image = My.Resources.Resources.Status_Check
                StatusLabel_ChartStatus.Text = GetLoc("ChartStop")

                ' Üzenet megjelenítése
                MsgBox(GetLoc("MsgInterfaceText"), vbExclamation, GetLoc("MsgInterfaceTitle") + ": " + ComboBox_InterfaceList.Items(SelectedInterface))

                ' Interfész lista újratöltése
                UpdateInterfaceList(True)

            Else

                ' Diagram leképezés indítása
                ChartStop = False

                ' Értékek beállítása -> Forgalmi adatok: fogadott és küldött bájtok
                For Each Me.objMgmt In objNI.Get()

                    ' Maximálisan felvehető sebességérték (A sávszélesség nyolcadrésze)
                    MaxRelativeSpeed = objMgmt("CurrentBandwidth") / 8

                    ' Aktuális érték az első helyre
                    If objMgmt("BytesReceivedPersec") > (UIntCorrection - 1) Then
                        CurrentDownload = objMgmt("BytesReceivedPersec") - (UIntCorrection)
                    Else
                        CurrentDownload = objMgmt("BytesReceivedPersec")
                    End If

                    ' Aktuális érték az első helyre
                    If objMgmt("BytesSentPersec") > (UIntCorrection - 1) Then
                        CurrentUpload = objMgmt("BytesSentPersec") - (UIntCorrection)
                    Else
                        CurrentUpload = objMgmt("BytesSentPersec")
                    End If
                Next

                ' Sávszélesség érték konverziója
                MaxBandwidth = ScaleConversion(objMgmt("CurrentBandwidth"), 2, False)

                ' Kiírási értékek láthatóságának beállítása
                Value_Bandwidth.Enabled = True
                Value_BandwidthUnit.Enabled = True

            End If

        Else

            ' Sávszélesség alapérték beállítása (nincs hálózati interfész)
            MaxBandwidth = {0, 0}
            TraffReset = True

            ' Kiírási értékek láthatóságának beállítása
            Value_Bandwidth.Enabled = False
            Value_BandwidthUnit.Enabled = False

            ' Diagram leképezés leállítása
            ChartStop = True

            ' Diagram állapotkijelzés frissítése
            StatusLabel_ChartStatus.Image = My.Resources.Resources.Status_Check
            StatusLabel_ChartStatus.Text = GetLoc("ChartStop")

        End If

        ' Sávszélesség kiírás formázása
        Value_Bandwidth.Text = FixNumberFormat(MaxBandwidth(0), 2, True)
        Value_BandwidthUnit.Text = SIPrefix(MaxBandwidth(1)) + "bps"

        ' Forgalomtörlés ellenőrzése és 0-val való osztás elkerülése
        If TraffReset Or MaxRelativeSpeed = 0 Then

            ' Forgalomtörlés esetére hibakorrekció
            LatestDownload = CurrentDownload
            LatestUpload = CurrentUpload

            ' Interfész kihasználtság nullázása
            CurrentUsage = 0

            ' Kiírási értékek láthatóságának beállítása
            Value_InterfaceUsage.Enabled = False
            Value_DownloadSpeed.Enabled = False
            Value_DownloadSpeedUnit.Enabled = False
            Value_UploadSpeed.Enabled = False
            Value_UploadSpeedUnit.Enabled = False

        Else

            ' Hibakorrekció túlcsordulás ellen - Letöltés
            If CurrentDownload < LatestDownload Then
                DownloadSpeed = {0, 0}
            End If

            ' Hibakorrekció túlcsordulás ellen - Feltöltés
            If CurrentUpload < LatestUpload Then
                UploadSpeed = {0, 0}
            End If

            ' Kihasználtsági érték kiszámítása (Mivel duplexitást nem lehet lekérdezni, ezért a kettő összege adja a kihasználtság mértékét -> Ez csak half-duplex kapcsolatnál igaz ebben a formában!)
            UsageValue = (CurrentDownload - LatestDownload) + (CurrentUpload - LatestUpload)
            CurrentUsage = Round(Abs(UsageValue / MaxRelativeSpeed) * 100)

            ' Kiírási értékek láthatóságának beállítása
            Value_InterfaceUsage.Enabled = True
            Value_DownloadSpeed.Enabled = True
            Value_DownloadSpeedUnit.Enabled = True
            Value_UploadSpeed.Enabled = True
            Value_UploadSpeedUnit.Enabled = True

        End If

        ' Sebesség értékek kiszámítása és konvertálása
        DownloadSpeed = ScaleConversion(Abs(CurrentDownload - LatestDownload), 2, True)
        UploadSpeed = ScaleConversion(Abs(CurrentUpload - LatestUpload), 2, True)

        ' Kiírási értékek frissítése
        Value_DownloadSpeed.Text = FixNumberFormat(DownloadSpeed(0), 2, False)
        Value_DownloadSpeedUnit.Text = BytePrefix(DownloadSpeed(1)) + "B/s"
        Value_UploadSpeed.Text = FixNumberFormat(UploadSpeed(0), 2, False)
        Value_UploadSpeedUnit.Text = BytePrefix(UploadSpeed(1)) + "B/s"

        ' Számítási hibakorrekció (100-nál nem lehet több!)
        If CurrentUsage > 100 Then
            CurrentUsage = 100
        End If

        ' Kiírás formázása
        Value_InterfaceUsage.Text = CurrentUsage.ToString + " %"

        ' Publikus tömb felülírása a jelenlegi értékekkel
        LatestDownload = CurrentDownload
        LatestUpload = CurrentUpload

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Forgalomszámláló tömb feltöltése és értékek frissítése ***
    ' Bemenet: TraffReset -> forgalom nullázása (Boolean)
    ' Kimenet: *          -> hamis érték (Boolean)
    Private Function UpdateTraffArray(ByVal TraffReset As Boolean)

        ' Értékek definiálása
        Dim Download(UBound(TraffDownArray)) As Double          ' Letöltött bájtok tömbje
        Dim Upload(UBound(TraffUpArray)) As Double              ' Feltöltött bájtok tömbje
        Dim UIntCorrection As Double                            ' Előjel nélküli integer korrekció

        ' Előjel nélküli integer korrekció -> Előjel átfordulás kikerülése
        ' Megjegyzés: XP alatt csak UInt32-ben voltak tárolva ezek az értékek, de ez NT 6.0-tól UInt64-re változott!
        If OSVersion(0) < 6 Then
            UIntCorrection = 2 ^ 31
        Else
            UIntCorrection = 2 ^ 63
        End If

        ' WMI értékek lekérdezése: Win32_PerfRawData_Tcpip_NetworkInterface -> Forgalmi adatok
        objNI = New ManagementObjectSearcher("SELECT BytesReceivedPersec, BytesSentPersec FROM Win32_PerfRawData_Tcpip_NetworkInterface WHERE Name = '" + InterfaceList(SelectedInterface) + "'")

        ' Értékek beállítása -> Forgalmi adatok: fogadott és küldött bájtok
        For Each Me.objMgmt In objNI.Get()

            ' Aktuális letöltési érték az első helyre
            If objMgmt("BytesReceivedPersec") >= UIntCorrection Then
                Download(0) = objMgmt("BytesReceivedPersec") - UIntCorrection
            Else
                Download(0) = objMgmt("BytesReceivedPersec")
            End If

            ' Aktuális feltöltési érték az első helyre
            If objMgmt("BytesSentPersec") >= UIntCorrection Then
                Upload(0) = objMgmt("BytesSentPersec") - UIntCorrection
            Else
                Upload(0) = objMgmt("BytesSentPersec")
            End If

        Next

        ' Forgalomtörlés ellenőrzése
        If TraffReset Then

            ' A teljes forgalomi tömb feltöltése az aktuális értékekkel
            For Count = 1 To UBound(Download)
                Download(Count) = Download(0)
                Upload(Count) = Upload(0)
            Next

        Else

            ' Korábbi értékek hátrébb tolása 1 hellyel
            For Count = 1 To UBound(Download)
                Download(Count) = TraffDownArray(Count - 1)
                Upload(Count) = TraffUpArray(Count - 1)
            Next

        End If

        ' Publikus tömb felülírása a jelenlegi értékekkel
        TraffDownArray = Download
        TraffUpArray = Upload

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Hardver lista újratöltése ***
    ' Bemenet: ResetFlag -> alapértelmezett listaelem beállítása újratöltés után (Boolean)
    ' Kimenet: *         -> hamis érték (Boolean)
    Private Function UpdateHWList(ByVal ResetFlag As Boolean)

        ' Lista kiürítése
        ComboBox_HWList.Items.Clear()

        ' Lista feltöltése
        ComboBox_HWList.Items.AddRange(New Object() {GetLoc("Motherboard"), GetLoc("System"), GetLoc("BIOS"), GetLoc("Battery")})

        ' Alapértelmezett érték visszaállítása (a lista legelső eleme)
        If ResetFlag Then
            SelectedHardware = 0
        End If

        ' Utoljára kiválasztott érték beállítása
        ComboBox_HWList.SelectedIndex = SelectedHardware

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Processzor lista újratöltése ***
    ' Bemenet: ResetFlag -> alapértelmezett listaelem beállítása újratöltés után (Boolean)
    ' Kimenet: *         -> hamis érték (Boolean)
    Private Function UpdateCPUList(ByVal ResetFlag As Boolean)

        ' Értékek definiálása
        Dim CPUNumber As Int32 = 0                              ' Processzorok száma
        Dim CPUString(32) As String                             ' Processzor neve
        Dim CPUDataWidth As Int32 = 0                           ' Processzor adatbusz szélessége
        Dim ListCount As Int32                                  ' Lista sorszám
        Dim SearchCount As Int32                                ' Keresendő sztring sorszáma
        Dim DeleteCount As Int32                                ' Törlendő sztring sorszáma

        ' Sztring cserék változói (eredeti, csere)
        Dim SearchList() As String = {"MHz", "GHz"}
        Dim ReplaceList() As String = {" MHz", " GHz"}

        ' Névből törlendő sztringek tömbje
        Dim DeleteList() As String = {"CPU", "Processor", "(C)", "(R)", "(TM)"}

        ' Lista kiürítése
        ComboBox_CPUList.Items.Clear()

        ' WMI lekérdezés: Win32_Processor -> Processzor információk
        objPR = New ManagementObjectSearcher("SELECT Name, DataWidth FROM Win32_Processor")

        ' Értékek beállítása -> Számítógép: név, adatbusz szélessége
        For Each Me.objMgmt In objPR.Get()
            CPUString(CPUNumber) = objMgmt("Name")
            CPUDataWidth = objMgmt("DataWidth")
            CPUNumber += 1
        Next

        ' CPU nevéből a felesleges karakterek eltávolítása
        For ListCount = 0 To CPUNumber - 1

            ' Korrekciós sztringek keresése és cseréje
            For SearchCount = 0 To UBound(SearchList)
                CPUString(ListCount) = Replace(CPUString(ListCount), SearchList(SearchCount), ReplaceList(SearchCount))
            Next

            ' Törlendő sztringek keresése és törlése
            For DeleteCount = 0 To UBound(DeleteList)
                CPUString(ListCount) = Replace(CPUString(ListCount), DeleteList(DeleteCount), " ")
            Next

            ' Listaelem hozzáadása
            ComboBox_CPUList.Items.Add("# " + (ListCount + 1).ToString + "/" + CPUNumber.ToString + " - " + RemoveSpaces(CPUString(ListCount)) + " (" + CPUDataWidth.ToString + "-bit)")

        Next

        ' Alapértelmezett érték visszaállítása (a lista legelső eleme)
        If ResetFlag Then SelectedCPU = 0

        ' Utoljára kiválasztott érték beállítása
        ComboBox_CPUList.SelectedIndex = SelectedCPU

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Memóriamodul lista újratöltése ***
    ' Bemenet: ResetFlag -> alapértelmezett listaelem beállítása újratöltés után (Boolean)
    ' Kimenet: *         -> hamis érték (Boolean)
    Private Function UpdateMemList(ByVal ResetFlag As Boolean)

        ' Értékek definiálása
        Dim ModuleNum As Int32 = 0                              ' Memória modulok száma
        Dim ModuleCount As Int32 = 0                            ' Memória modul sorszáma
        Dim ModuleSize(1) As Double                             ' Memória modul mérete (formázott)
        Dim ModuleSocket As Int32 = Nothing                     ' Memória foglalat típusának azonosítója
        Dim ModuleString As String = Nothing                    ' Memória foglalat típusa

        ' WMI értékek lekérdezése: Win32_PhysicalMemory -> Memória információk
        objPM = New ManagementObjectSearcher("SELECT Capacity, FormFactor FROM Win32_PhysicalMemory")

        ModuleNum = objPM.Get().Count

        ' Lista kiürítése -> Memória modulok
        ComboBox_RAMList.Items.Clear()

        ' Értékek beállítása -> Modul információk
        For Each Me.objMgmt In objPM.Get()

            ' Modul mérete
            ModuleSocket = objMgmt("FormFactor")
            ModuleSize = ScaleConversion(objMgmt("Capacity"), 0, True)

            ' Memória modul típusának beállítása (Ha tartományon belül van, és nem üres!)
            If ModuleSocket <= UBound(MemorySocketType) Then
                If MemorySocketType(ModuleSocket) <> Nothing Then
                    ModuleString = " (" + MemorySocketType(ModuleSocket) + ")"
                End If
            End If

            ' Listaelem hozzáadása
            ComboBox_RAMList.Items.Add("# " + (ModuleCount + 1).ToString + "/" + ModuleNum.ToString + " - " + FixNumberFormat(ModuleSize(0), 2, True) + " " + BytePrefix(ModuleSize(1)) + "B" + ModuleString)

            ' Modul számának növelése
            ModuleCount += 1

        Next

        ' Üres lista, ha nincs elérhető memóriamodul információ
        If ModuleCount = 0 Then
            ComboBox_RAMList.Enabled = False
            ComboBox_RAMList.Items.Add(GetLoc("NotAvailable"))
        Else
            ComboBox_RAMList.Enabled = True
        End If

        ' Alapértelmezett érték visszaállítása (a lista legelső eleme)
        If ResetFlag Then
            SelectedMemory = 0
        End If

        ' Utoljára kiválasztott érték beállítása
        ComboBox_RAMList.SelectedIndex = SelectedMemory

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Lemezlista újratöltése ***
    ' Bemenet: ResetFlag -> alapértelmezett listaelem beállítása újratöltés után (Boolean)
    ' Kimenet: *         -> hamis érték (Boolean)
    Private Function UpdateDiskList(ByVal ResetFlag As Boolean)

        ' Értékek definiálása
        Dim DiskCount As Int32 = 0                              ' Lemezek sorszáma
        Dim DiskIndex As Int32 = 0                              ' Lemez index azonosítója
        Dim SortCount As Int32 = 0                              ' Sorbarendezési sorszámok
        Dim DiskPnPID(32) As String                             ' Lemez PnP azonosítója
        Dim Capacity(32) As Double                              ' Lemez kapacitása
        Dim FormattedCapacity(2) As Double                      ' Formázott kapacitás érték
        Dim Listlabel As String = Nothing                       ' Lemez megjelenítendő neve
        Dim SmartPnPID As String = Nothing                      ' Eredeti PnP azonosító a S.M.A.R.T-hoz
        Dim ConvertID As String = Nothing                       ' Konvertált PnP azonosító az összehasonlításhoz
        Dim SmartData() As Byte                                 ' S.M.A.R.T adatok tömbje
        Dim SmartStart As Int32 = 2                             ' S.M.A.R.T rekord kezdő bájtja (az első 2-es)
        Dim SmartStep As Int32 = 12                             ' S.M.A.R.T bájtok ugrásköze (12-esével)
        Dim SmartCount As Int32 = 0                             ' S.M.A.R.T bájtok léptetése (beállítás ciklus közben)
        Dim ListCount As Int32                                  ' Lista sorszám
        Dim DeleteCount As Int32                                ' Törlendő sztring sorszáma

        ' Névből törlendő sztringek tömbje
        Dim DeleteList() As String = {"ATA Device", "SCSI Disk Device", "USB Device"}

        ' Lista kiürítése
        ComboBox_DiskList.Items.Clear()

        ' WMI lekérdezés: Win32_DiskDrive -> Lemezmeghajtók
        objDD = New ManagementObjectSearcher("SELECT Index, Model, Size, PNPDeviceID FROM Win32_DiskDrive")

        ' Értékek beállítása -> Lemezmeghajtók: index, modell, azonosító, kapacitás
        For Each Me.objMgmt In objDD.Get()
            DiskIndex = ToInt32(objMgmt("Index"))
            DiskName(DiskIndex) = RemoveSpaces(objMgmt("Model"))
            DiskPnPID(DiskIndex) = objMgmt("PNPDeviceID")
            Capacity(DiskIndex) = objMgmt("Size")
            DiskList(DiskCount) = DiskIndex

            ' Ismeretlen lemez ellenőrzése
            If IsNothing(objMgmt("Model")) Then
                DiskName(DiskIndex) = GetLoc("UnknownDisk")
            Else
                DiskName(DiskIndex) = RemoveSpaces(objMgmt("Model"))
            End If

            DiskCount += 1
        Next

        ' Függő értékek definiálása
        Dim DiskSort(DiskCount - 1) As Int32                    ' Lemezek sorrendje

        ' Sorbarendezési tömb feltöltése
        For SortCount = 0 To DiskCount - 1
            DiskSort(SortCount) = DiskList(SortCount)
        Next

        ' Sorbarendezés index alapján
        Array.Sort(DiskSort)

        ' Lemezek nevéből a felesleges jelölések eltávolítása
        For ListCount = 0 To DiskCount - 1

            ' Törlendő sztringek keresése és törlése
            For DeleteCount = 0 To UBound(DeleteList)
                DiskName(DiskSort(ListCount)) = Replace(DiskName(DiskSort(ListCount)), DeleteList(DeleteCount), Nothing)
            Next

            ' Felesleges szóközök eltávolítása (OEM lemezek esetén előfordul, hogy telenyomják szóközzel)
            DiskName(DiskSort(ListCount)) = RemoveSpaces(DiskName(DiskSort(ListCount)))

        Next

        ' WMI lekérdezés: MSStorageDriver_ATAPISmartData -> Lemez típusának meghatározása (S.M.A.R.T értékből)
        ' Megjegyzés: Ez nem a "ROOT\CIMV2"-ből való bejegyzés!)
        objSM = New ManagementObjectSearcher("ROOT\WMI", "SELECT InstanceName, VendorSpecific FROM MSStorageDriver_ATAPISmartData")

        ' S.M.A.R.T értékek kiértékelése
        ' Megjegyzés: Ha üres a tábla, akkor 'ManagementException'-t okoz, ezért kell a 'Try'!
        Try

            ' Értékek beállítása -> S.M.A.R.T: név, adatok tömbje
            For Each Me.objMgmt In objSM.Get()
                SmartPnPID = objMgmt("InstanceName")
                SmartData = objMgmt("VendorSpecific")

                ' S.M.A.R.T PnP átalakítása az összehasonlításhoz (Csupa nagybetű, az utolsó két karakter levágva!)
                ConvertID = UCase(SmartPnPID.Substring(0, SmartPnPID.Length - 2))

                ' S.M.A.R.T azonosíók keresése
                For ListCount = 0 To DiskCount - 1

                    ' Lemez PnP ID és a konvertált azonosító összehasonlítása
                    If ConvertID = DiskPnPID(ListCount) Then

                        ' Lemez S.M.A.R.T azonosítójának mentése
                        DiskSmart(DiskSort(ListCount)) = Replace(SmartPnPID, "\", "\\")

                        ' Ugrási kezdőérték beállítása
                        SmartCount = SmartStart

                        ' Léptetés a rekordok között, amíg el nem fogynak
                        While SmartData(SmartCount) <> 0

                            ' SSD-re jellemző rekord keresése -> Wear Leveling Count (173) vagy Wear Range Delta (177)
                            If SmartData(SmartCount) = 173 Or SmartData(SmartCount) = 177 Then
                                DiskType(DiskSort(ListCount)) = "SSD"
                            End If

                            ' Lépésköz beállítása
                            SmartCount += SmartStep
                        End While

                        ' HDD beállítása, ha lemez nem SSD
                        If DiskType(DiskSort(ListCount)) = Nothing Then
                            DiskType(DiskSort(ListCount)) = "HDD"
                        End If

                    End If
                Next
            Next
        Catch

            ' Kivételkezelés
            SmartException = True

        End Try

        ' Lemezlista neveinek legenerálása
        For ListCount = 0 To DiskCount - 1

            ' Címke beállítása
            Listlabel = "# " + DiskSort(ListCount).ToString + " - " + DiskName(DiskSort(ListCount))
            SortCount = DiskSort(ListCount)

            ' Lemezméret beállítása
            If Capacity(DiskSort(ListCount)) <> 0 Then

                ' Kapacitás érték konvertálása
                ' Megjegyzés: mivel itt nyers adat szerepel, így SI-re van konvertálva! (A gyártói címkén is ez van feltűntetve, az adathordozó tetején!)
                FormattedCapacity = ScaleConversion(Capacity(DiskSort(ListCount)), 2, False)
                Listlabel += " (" + FixNumberFormat(FormattedCapacity(0), 1, True) + " " + SIPrefix(FormattedCapacity(1)) + "B)"

            End If

            ' Lista feltöltése
            DiskName(ListCount) = ComboBox_DiskList.Items.Add(Listlabel)
            DiskList(ListCount) = DiskSort(ListCount)

        Next

        ' Alapértelmezett érték visszaállítása (a lista legelső eleme)
        If ResetFlag Then
            SelectedDisk = 0
            SelectedPartition = 0
        End If

        ' Utoljára kiválasztott érték beállítása
        ComboBox_DiskList.SelectedIndex = SelectedDisk

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Videokártya lista újratöltése ***
    ' Bemenet: ResetFlag -> alapértelmezett listaelem beállítása újratöltés után (Boolean)
    ' Kimenet: *         -> hamis érték (Boolean)
    Private Function UpdateVideoList(ByVal ResetFlag As Boolean)

        ' Értékek definiálása
        Dim VideoCount As Int32 = 0                             ' Kártya sorszáma
        Dim ListCount As Int32                                  ' Lista sorszám
        Dim DeleteCount As Int32                                ' Törlendő sztring sorszáma

        ' Névből törlendő sztringek tömbje
        Dim DeleteList() As String = {"(C)", "(R)", "(TM)", "(Microsoft Corporation - WDDM)", "(Microsoft Corporation - WDDM 1.0)",
                                      "(Microsoft Corporation - WDDM 1.1)", "(Microsoft Corporation - WDDM 1.2)"}

        ' Lista kiürítése
        ComboBox_VideoList.Items.Clear()

        ' WMI lekérdezés: Win32_VideoController -> Videokártyák
        objVC = New ManagementObjectSearcher("SELECT Name FROM Win32_VideoController")

        ' Értékek beállítása -> Videokártya: név
        For Each Me.objMgmt In objVC.Get()
            VideoName(VideoCount) = RemoveSpaces(objMgmt("Name"))

            ' Törlendő sztringek keresése és törlése
            For DeleteCount = 0 To UBound(DeleteList)
                VideoName(VideoCount) = Replace(VideoName(VideoCount), DeleteList(DeleteCount), Nothing)
            Next

            VideoCount += 1
        Next

        ' Lista feltöltése
        For ListCount = 1 To VideoCount
            ComboBox_VideoList.Items.Add("# " + ListCount.ToString + "/" + VideoCount.ToString + " - " + RemoveSpaces(VideoName(ListCount - 1)))
        Next

        ' Alapértelmezett érték visszaállítása (a lista legelső eleme)
        If ResetFlag Then
            SelectedVideo = 0
        End If

        ' Utoljára kiválasztott érték beállítása
        ComboBox_VideoList.SelectedIndex = SelectedVideo

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Interfész lista újratöltése ***
    ' Bemenet: ResetFlag -> alapértelmezett listaelem beállítása újratöltés után (Boolean)
    ' Kimenet: *         -> hamis érték (Boolean)
    Private Function UpdateInterfaceList(ByVal ResetFlag As Boolean)

        ' Értékek definiálása
        Dim AdapterNum As Int32 = 0                         ' Listaelemek darabszáma
        Dim AdapterCount As Int32 = 0                       ' Adapterek sorszáma
        Dim InterfaceCount As Int32 = 0                     ' Interfészek sorszáma
        Dim StatCount, ListCount As Int32                   ' Statisztikai összehasonlítás és lista sorszám
        Dim SearchCount As Int32                            ' Keresendő sztring sorszáma
        Dim DeleteCount As Int32                            ' Törlendő sztring sorszáma

        ' Sztring cserék változói (eredeti, csere)
        Dim SearchList() As String = {"[", "]", " _", "_100", "_AR", "PRO_", "_RTL"}
        Dim ReplaceList() As String = {"(", ")", " #", "/100", "/AR", "PRO/", "/RTL"}

        ' Névből törlendő sztringek tömbje
        Dim DeleteList() As String = {"_", "(C)", "(R)", "(TM)", " - Packet Scheduler Miniport"}

        ' Operációs rendszer függő változók definiálása
        Dim NameRecord, IndexRecord, SourceTable As String

        ' Név és index rekord, valamint a forrás tábla beállítása
        ' Megjegyzés: az adapter sorszáma is itt van feltűntetve a névben, ha több van ugyanabból a kártyatípusból!
        If OSVersion(0) >= 6 Then
            NameRecord = "Name"
            IndexRecord = "DeviceID"
            SourceTable = "Win32_NetworkAdapter"
        Else
            NameRecord = "Description"
            IndexRecord = "Index"
            SourceTable = "Win32_NetworkAdapterConfiguration"
        End If

        ' WMI lekérdezés: OS-függő -> Hálózati adapterek neve és sorszáma
        objNA = New ManagementObjectSearcher("SELECT " + NameRecord + ", " + IndexRecord + " FROM " + SourceTable)

        ' Eszközszám meghatározása
        AdapterNum = objNA.Get().Count

        ' Függő értékek definiálása
        Dim AdapterList(AdapterNum - 1) As String           ' Hálózati adapterek eszközneveinek tömbje
        Dim AdapterName(AdapterNum - 1) As String           ' Előformázott eszköznevek tömbje (összehasonlításhoz)
        Dim AdapterID(AdapterNum - 1) As String             ' Hálózati adapter azonosítója

        ' Értékek beállítása -> Hálózati adapterek neveinek lekérdezése 
        For Each Me.objMgmt In objNA.Get()

            ' Eredeti adapternév felvitele az adapterlistába (lekérdezéshez)
            AdapterList(AdapterCount) = objMgmt(NameRecord)

            ' Konvertált név felitele a névlistába (összehasonlításhoz)
            AdapterName(AdapterCount) = StatNameConv(objMgmt(NameRecord))

            ' Adapter azonosítója (IP-infó lekérdezéshez)
            AdapterID(AdapterCount) = objMgmt(IndexRecord)

            ' Adapter számláló növelése
            AdapterCount += 1

        Next

        ' Lista kiürítése
        ComboBox_InterfaceList.Items.Clear()

        ' WMI lekérdezés: Win32_PerfRawData_Tcpip_NetworkInterface -> Interfészek
        objNI = New ManagementObjectSearcher("SELECT Name FROM Win32_PerfRawData_Tcpip_NetworkInterface")

        ' Értékek beállítása -> Interfészlista feltöltése
        For Each Me.objMgmt In objNI.Get()

            ' Hibakezelés: ISATAP és virtuális ('*'-ot tartlmaz a neve, pl.: PAN) adapterek kihagyása
            If CheckStrContain(objMgmt("Name"), {"isatap", "*"}, False) = False Then

                ' Eredeti interfésznév felvitele az interfészlistába (lekérdezéshez)
                InterfaceList(InterfaceCount) = objMgmt("Name")

                ' Statisztikához átalakított eszköznevek keresése (Ha van egyezés, akkor az lesz a név, egyébként a gyári!)
                For StatCount = 0 To (AdapterNum - 1)

                    ' Konvertált adapter név és Interfésznév összehasonlítása (XP-s névkorrekcióval!)
                    If AdapterName(StatCount) = InterfaceList(InterfaceCount) Then

                        ' Interfész azonosító és név hozzáadása
                        InterfaceID(InterfaceCount) = AdapterID(StatCount)
                        InterfaceName(InterfaceCount) = AdapterList(StatCount)

                        ' Törlendő sztringek keresése és törlése
                        For DeleteCount = 0 To UBound(DeleteList)
                            InterfaceName(InterfaceCount) = Replace(InterfaceName(InterfaceCount), DeleteList(DeleteCount), Nothing)
                        Next

                    End If
                Next

                ' Konvertált név keresése -> Ha üres, akkor a kiolvasott név lesz átalakítva!
                If InterfaceName(InterfaceCount) = Nothing Then

                    ' Eredeti név felhasználása, ha az összehasonlítás nem járt sikerrel
                    InterfaceName(InterfaceCount) = InterfaceList(InterfaceCount)

                    ' Korrekciós sztringek keresése és cseréje
                    For SearchCount = 0 To UBound(SearchList)
                        InterfaceName(InterfaceCount) = Replace(InterfaceName(InterfaceCount), SearchList(SearchCount), ReplaceList(SearchCount))
                    Next

                    ' Törlendő sztringek keresése és törlése
                    For DeleteCount = 0 To UBound(DeleteList)
                        InterfaceName(InterfaceCount) = Replace(InterfaceName(InterfaceCount), DeleteList(DeleteCount), Nothing)
                    Next

                End If

                ' Interfész számláló növelése
                InterfaceCount += 1

            End If
        Next

        ' Interfész jelenlét ellenőrzése
        If InterfaceCount = 0 Then

            ' Függő változók beállítása
            InterfacePresent = False
            ChartStop = True

            ' Lista tiltása
            ComboBox_InterfaceList.Enabled = False

            ' Hamis listaelem hozzáadása
            InterfaceName(0) = GetLoc("NotAvailable")
            ComboBox_InterfaceList.Items.Add(InterfaceName(0))

        Else

            ' Függő változók beállítása
            InterfacePresent = True
            ChartStop = False

            ' Lista engedélyezése
            ComboBox_InterfaceList.Enabled = True

            ' Lista feltöltése
            For ListCount = 1 To InterfaceCount
                ComboBox_InterfaceList.Items.Add("# " + ListCount.ToString + "/" + InterfaceCount.ToString + " - " + RemoveSpaces(InterfaceName(ListCount - 1)))
            Next

        End If

        ' Alapértelmezett érték visszaállítása (a lista legelső eleme)
        If ResetFlag Then SelectedInterface = 0

        ' Utoljára kiválasztott érték beállítása
        ComboBox_InterfaceList.SelectedIndex = SelectedInterface

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Lemez elérhetőségének ellenőrzése ***
    ' Bemenet: *           -> üres (Void)
    ' Kimenet: DiskPresent -> engedélyezés értéke (Boolean)
    Private Function CheckDiskAvailable()

        ' Értékek definiálása
        Dim DiskPresent As Boolean = False                  ' Lemez elérhetősége

        ' WMI értékek lekérdezése: Win32_DiskDrive -> Lemezmeghajtó fizikai azonosítója
        objDD = New ManagementObjectSearcher("SELECT DeviceID FROM Win32_DiskDrive WHERE Index = '" + DiskList(SelectedDisk) + "'")

        ' Hiányzó lemez ellenőrzése
        If objDD.Get().Count = 0 Then

            ' Kiírások feltöltése üres adatokkal
            Button_SMARTInfoOpen.Enabled = False
            Value_DiskInterface.Enabled = False
            Value_DiskInterface.Text = GetLoc("NotAvailable")
            Value_DiskType.Enabled = False
            Value_DiskType.Text = GetLoc("Unknown")
            Value_DiskFirmware.Enabled = False
            Value_DiskFirmware.Text = GetLoc("Unknown")
            Value_DiskSerial.Enabled = False
            Value_DiskSerial.Text = GetLoc("NotAvailable")

            ' Tömbök és lista kiürítése -> Partíció információk
            Array.Clear(PartLabel, 0, UBound(PartLabel))
            Array.Clear(PartInfo, 0, UBound(PartInfo))
            ComboBox_PartList.Enabled = False
            ComboBox_PartList.Items.Clear()
            ComboBox_PartList.Items.Add(GetLoc("NotAvailable"))
            ComboBox_PartList.SelectedIndex = 0

            ' Üzenet megjelenítése
            MsgBox(GetLoc("MsgDiskText"), vbExclamation, GetLoc("MsgDiskTitle") + ": " + ComboBox_DiskList.Items(SelectedDisk))

            ' Lemezlista újratöltése
            UpdateDiskList(True)

        Else

            ' S.M.A.R.T információs gomb beállítása
            If DiskSmart(SelectedDisk) = Nothing Or SmartException Then
                Button_SMARTInfoOpen.Enabled = False
            Else
                Button_SMARTInfoOpen.Enabled = True
            End If

            ' Lemez elérhetőségének beállítása
            DiskPresent = True

        End If

        ' Visszatérési érték beállítása
        Return DiskPresent

    End Function

    ' *** FÜGGVÉNY: Interfész kapcsolat ellenőrzése ***
    ' Bemenet: *         -> üres (Void)
    ' Kimenet: IPEnabled -> kapcsolódás állapota (Boolean)
    Private Function CheckIPConnection()

        ' Értékek definiálása
        Dim IPEnabled As Boolean = False                    ' IP engedélyezés
        Dim IPAddress() As String                           ' IP címek tömbje

        ' Intefész azonosító meglétének ellenőrzése
        If Not IsNothing(InterfaceID(SelectedInterface)) Then

            ' WMI lekérdezés: Win32_NetworkAdapterConfiguration -> Interfész azonosító alapján történő lekérdezés
            objNC = New ManagementObjectSearcher("SELECT IPEnabled, IPAddress FROM Win32_NetworkAdapterConfiguration WHERE Index = '" + InterfaceID(SelectedInterface) + "'")

            ' Elemszám ellenőrzése
            If objNC.Get().Count <> 0 Then

                ' Értékek beállítása
                For Each Me.objMgmt In objNC.Get()
                    IPEnabled = objMgmt("IPEnabled")

                    ' XP ellenőrzés: az IPEnabled valós értéken marad kihúzott kábel mellett is!
                    If Not IsNothing(objMgmt("IPAddress")) Then
                        IPAddress = objMgmt("IPAddress")

                        ' Ha az IP-cím '0.0.0.0', akkor is tiltani kell!
                        If IPAddress(0) = "0.0.0.0" Then
                            IPEnabled = False
                        End If

                    End If
                Next
            End If
        End If

        ' Visszatérési érték beállítása
        Return IPEnabled

    End Function

    ' *** FÜGGVÉNY: Forgalmi diagram készítése ***
    ' Bemenet: TraffReset -> forgalom nullázása (Boolean)
    ' Kimenet: *          -> hamis érték (Boolean)
    Private Function MakeChart(ByVal TraffReset As Boolean)

        ' Értékek definiálása
        Dim DrawOffset(2) As Int32                              ' Rajzolási koordináta eltolási értékek: kézi eltolás (grafikai jellegű)
        Dim TextOffset(2) As Int32                              ' Szöveges koordináta letolásiértékek: relatív eltolás (pl.: sorugrás vagy távolság ugrás)
        Dim StartCorrection As Int32                            ' Koordináta eltilási értékek hibakorrekciónál, pl. kerekítési hiba kiküszöbölése (matematikai jellegű)
        Dim SignFont As New Font("Arial", 8)                    ' Betűtípus és méret beállítása a rajzon lévő szövegekhez
        Dim SignLine(1) As Point                                ' Rajzolási koordináták (X0, X1, Y0, Y1)
        Dim InterfaceReset As Boolean = False                   ' Hibakorrekció uint változó átbillenésére (előjelcsere)
        Dim ResetCount As Int32 = 0                             ' Hibakorrekció számlálója (lépések száma az átbillenáés után)
        Dim TraffCount As Int32                                 ' Forgalmi adatok sorszáma

        ' Diagram határérték definiálása és alapértékre állítása
        Dim ChartCanvas() As Int32 = {PictureBox_TrafficChart.Size.Width, PictureBox_TrafficChart.Size.Height}
        Dim ChartDimension() As Int32 = {420, 150}
        Dim ChartBegining() As Int32 = {0, ChartCanvas(1) - ChartDimension(1)}
        Dim DownPeak As Int32 = 0
        Dim UpPeak As Int32 = 0
        Dim Amplitude As Int32 = 0

        ' Sebesség tömb definiálása (1 elemmel nagyobb, mint a kijelzett értékek száma)
        Dim ChartDownNumbers(TraffResolution + 1) As Int32
        Dim ChartUpNumbers(TraffResolution + 1) As Int32

        ' Kép létrehozása
        Dim Picture As New Bitmap(ChartCanvas(0), ChartCanvas(1), Imaging.PixelFormat.Format24bppRgb)
        Dim Chart As Graphics = Graphics.FromImage(Picture)

        ' Argumentumvizsgálat (értékek nullázása, ha az érték valós) 
        If TraffReset Then

            ' Időzítő újraindítása -> EventTimer
            If MainWindowDone Then
                EventTimer.Enabled = False
                EventTimer.Enabled = True
            End If

            ' Generálási intervallum újraindítása (EventTimer által)
            TraffGenCounter = RefreshInterval(SelectedRefresh) - 1

            ' Forgalmi adatok frissítése
            UpdateTraffArray(True)

            ' Diagram rajzolásának kezdő állapotba hozása az állapotsorban
            StatusLabel_ChartStatus.Image = My.Resources.Resources.Status_Check
            If MainWindowDone Then
                StatusLabel_ChartStatus.Text = GetLoc("ChartReset")
            End If

        End If

        ' Sebességtömb feltöltése
        For TraffCount = 0 To TraffResolution
            ChartDownNumbers(TraffCount) = (TraffDownArray(TraffCount) - TraffDownArray(TraffCount + 1)) / RefreshInterval(SelectedRefresh)
            ChartUpNumbers(TraffCount) = (TraffUpArray(TraffCount) - TraffUpArray(TraffCount + 1)) / RefreshInterval(SelectedRefresh)

            ' Hibakorrekció túlcsordulás ellen -> Letöltés
            If TraffDownArray(TraffCount) - TraffDownArray(TraffCount + 1) < 0 Then
                ChartDownNumbers(TraffCount) = (TraffDownArray(TraffCount + 1) - TraffDownArray(TraffCount + 2)) / RefreshInterval(SelectedRefresh)
                If CheckedDownChart Then
                    InterfaceReset = True
                    ResetCount = (TraffResolution) - TraffCount
                End If
            End If

            ' Hibakorrekció túlcsordulás ellen -> Feltöltés
            If TraffUpArray(TraffCount) - TraffUpArray(TraffCount + 1) < 0 Then
                ChartUpNumbers(TraffCount) = (TraffUpArray(TraffCount + 1) - TraffUpArray(TraffCount + 2)) / RefreshInterval(SelectedRefresh)
                If CheckedUpChart Then
                    InterfaceReset = True
                    ResetCount = (TraffResolution) - TraffCount
                End If
            End If

            ' Maximum érték keresés -> Letöltés
            If CheckedDownChart Then
                If ChartDownNumbers(TraffCount) > DownPeak Then
                    DownPeak = ChartDownNumbers(TraffCount)
                End If

                If ChartDownNumbers(TraffCount) > Amplitude Then
                    Amplitude = ChartDownNumbers(TraffCount)
                End If
            End If

            ' Maximum érték keresés -> Feltöltés
            If CheckedUpChart Then
                If ChartUpNumbers(TraffCount) > UpPeak Then
                    UpPeak = ChartUpNumbers(TraffCount)
                End If

                If ChartUpNumbers(TraffCount) > Amplitude Then
                    Amplitude = ChartUpNumbers(TraffCount)
                End If
            End If
        Next

        ' ----- RÁCSVONALAK -----

        ' Értékek definiálása
        Dim GridLine(1) As Point                                    ' Koordináta értékek
        Dim DefaultSlip As Int32                                    ' Kezdeti rácscsúszás
        Dim Scale As Int32 = 10                                     ' Függőleges osztások száma
        Dim TraffDigit As Int32 = 2                                 ' Forgalmi értékek tizedesvessző utáni helyiértékeinek száma

        ' Rács időbeli csúszásának megállapítása (Reset-nél visszaállít, egyébként göngyölítve hoizzáadódik)
        If TraffReset Or InterfacePresent = False Then

            ' Rácscsúszás alaphelyzetbe állítása
            GridSlip = 0

        Else

            ' Alapértelmetett csúszás beállítása
            DefaultSlip = Round(ChartDimension(0) / (TraffResolution))

            ' Rács eltolási értékének frissítése
            If GridUpdate Then

                ' Rács csúszásának kiszámítása (kerekítési hibával járhat!)
                GridSlip = (GridSlip + DefaultSlip) Mod Round(ChartDimension(0) / (RefreshInterval(SelectedRefresh) * VerticalGrids))
                GridUpdate = False

            End If
        End If

        ' Diagram készítési idejének frissítése
        ChartCreationTime = DateTime.Now

        ' *** RÁCSVONAL - Vízszintes vonalak ***
        ' Rajzolási irány: vízszintesen balról jobbra, függőlegesen fentről lefelé.

        ' Koordinátacsúszás beállítása: vízszintesen változatlan, függőlegesen eggyel felfelé -> Az alsó vonal látszódik!
        DrawOffset = {0, -1}

        ' Vízszintes vonalak megrajzolása
        For Horizontal As Int32 = 0 To Scale

            ' Koordináták feltöltése (X0, X1, Y0, Y1)
            GridLine(0).X = ChartBegining(0) + DrawOffset(0)
            GridLine(1).X = ChartBegining(0) + ChartDimension(0) + DrawOffset(0)
            GridLine(0).Y = ChartBegining(1) + DrawOffset(1)
            GridLine(1).Y = GridLine(0).Y ' Egyezik a kezdeti koordinátákkal

            ' Aktuális vonal megrajzolása
            Chart.DrawLines(Pens.DarkGreen, GridLine)

            ' Y koordináta eltolása a következő ciklusra (Fentről lefelé)
            DrawOffset(1) += ToInt32(ChartDimension(1) / Scale)

        Next

        ' *** RÁCSVONAL - Függőleges vonalak ***
        ' Rajzolási irány: vízszintesen jobbról balra, függőlegesen fentről lefelé.

        ' Koordinátacsúszás beállítása: vízszintesen eggyel balra, függőlegesen eggyel felfelé -> A jobb szélső vonal látszódik, valamint a vízszintes vonalak eltolása miatt felfelé is el kell tolni!
        DrawOffset = {-1, -1}

        ' Függőleges vonalak megrajzolása
        For Vertical As Int32 = 0 To Round(RefreshInterval(SelectedRefresh) * VerticalGrids)

            ' Koordináták feltöltése (X0, X1, Y0, Y1)
            GridLine(0).X = ChartDimension(0) + DrawOffset(0) - GridSlip
            GridLine(1).X = GridLine(0).X ' Egyezik a kezdeti koordinátákkal
            GridLine(0).Y = ChartBegining(1) + DrawOffset(1)
            GridLine(1).Y = ChartBegining(1) + ChartDimension(1) + DrawOffset(1)

            ' Aktuális vonal megrajzolása
            Chart.DrawLines(Pens.DarkGreen, GridLine)

            ' X koordináta eltolása a következő ciklusra (jobbról balra
            DrawOffset(0) -= ToInt32(ChartDimension(0) / (RefreshInterval(SelectedRefresh))) * VerticalGrids

        Next

        ' ----- SEGÉDEGYENESEK -----

        ' Értékek definiálása
        Dim ScaleMax(2) As Double           ' Skála maximális értéke (2 dimenziós: alap, kitevő)
        Dim PeakDiv, PeakExp As Int32       ' Csúcsérték osztója (10 hatványa), csúcsérték hatványkitevője
        Dim LineCuts As Int32 = 50          ' Szaggatás beállítása a segédegyenesehkhez

        ' Skála maximumának meghatározása (2 tizedesjegyig)
        ScaleMax = ScaleConversion(Amplitude, 2, True)

        ' Osztó és kitevő beállítása
        If ScaleMax(0) > 100 Then
            PeakDiv = 1
            PeakExp = ScaleMax(1) + 1
        ElseIf ScaleMax(0) > 10 Then
            PeakDiv = 100
            PeakExp = ScaleMax(1)
        Else
            PeakDiv = 10
            PeakExp = ScaleMax(1)
        End If

        ' *** SEGÉDEGYENES - Feltöltési csúcssebesség ***
        ' Rajzolási irány: vízszintesen balról jobbra; függőlegesen: alulról felfelé.
        ' Megjegyzés: A csúsztatás miatt dupla szélességre kell megrajzolni. EZ VAN ALUL!

        ' Értékek definiálása
        Dim UpPeakLine As Int32
        Dim UpPeakConv(2) As Double
        UpPeakConv = ScaleConversion(UpPeak, TraffDigit, True)

        ' Koordinátacsúszás beállítása: vízszintesen változatlan, függőlegesen kettővel felfelé -> Mindig a csúcsérték felett látszódik, az meg már el van tolva eggyel!
        DrawOffset = {0, -2}

        ' Függőleges koordináta számítása
        If UpPeak <> 0 Then
            UpPeakLine = ChartCanvas(1) - Round(((UpPeakConv(0) * (1000 ^ UpPeakConv(1))) / (PeakDiv * (1000 ^ PeakExp))) * (ChartCanvas(1) - ChartBegining(1)))
        Else
            UpPeakLine = ChartCanvas(1)
        End If

        ' Feltöltési segédegyenes megrajzolása
        If CheckedUpChart And InterfacePresent Then
            For Vertical As Int32 = 0 To LineCuts ' Kétszer akkora területre kell kirajzolni, ha mozog!

                ' Koordináták feltöltése (X0, X1, Y0, Y1)
                GridLine(0).X = DrawOffset(0) - GridSlip
                GridLine(1).X = DrawOffset(0) + ToInt32(ChartDimension(0) / LineCuts) - GridSlip - 1 ' A szaggatásnál eggyel rövidebb szakasz kell, mert nem fedhetik egymást a segédegyenesek.
                GridLine(0).Y = DrawOffset(1) + ToInt32(UpPeakLine)
                GridLine(1).Y = GridLine(0).Y ' Egyezik a kezdeti koordinátákkal

                ' Aktuális szakasz megrajzolása
                Chart.DrawLines(Pens.DarkRed, GridLine)

                ' X koordináta eltolása a következő ciklusra
                DrawOffset(0) += Round((ChartDimension(0) / LineCuts) * 2)

            Next
        End If

        ' *** SEGÉDEGYENES - Letöltési csúcssebesség ***
        ' Rajzolási irány: vízszintesen balról jobbra; függőlegesen: alulról felfelé.
        ' Megjegyzés: A csúsztatás miatt dupla szélességre kell megrajzolni. EZ VAN FELÜL!

        ' Értékek definiálása
        Dim DownPeakLine As Int32
        Dim DownPeakConv(2) As Double
        DownPeakConv = ScaleConversion(DownPeak, TraffDigit, True)

        ' Koordinátacsúszás beállítása: Vízszintesen változatlan, kettővel felfelé -> Mindig a csúcsérték felett látszódik, az meg már el van tolva eggyel!
        DrawOffset = {0, -2}

        ' Kezdőkorrekció: Nem eshet egybe az alsó segédegyenessel, ha egyvonalba esnek! Vízszintesen el kell tolni a feltöltési segédegyenes szaggatásának üres részére!
        StartCorrection = Round(ChartDimension(0) / LineCuts)
        DrawOffset(0) = StartCorrection

        ' Függőleges koordináta számítása
        If DownPeak <> 0 Then
            DownPeakLine = ChartCanvas(1) - Round(((DownPeakConv(0) * (1000 ^ DownPeakConv(1))) / (PeakDiv * (1000 ^ PeakExp))) * (ChartCanvas(1) - ChartBegining(1)))
        Else
            DownPeakLine = ChartCanvas(1)
        End If

        ' Letöltési segédegyenes megrajzolása
        If CheckedDownChart And InterfacePresent Then
            For Vertical As Int32 = 0 To LineCuts ' Kétszer akkora területre kell kirajzolni, ha mozog!

                ' Koordináták feltöltése (X0, X1, Y0, Y1)
                GridLine(0).X = DrawOffset(0) - GridSlip
                GridLine(1).X = DrawOffset(0) + Round(ChartDimension(0) / LineCuts) - GridSlip - 1 ' A szaggatásnál eggyel rövidebb szakasz kell, mert nem fedhetik egymást a segédegyenesek.
                GridLine(0).Y = DrawOffset(1) + Round(DownPeakLine)
                GridLine(1).Y = GridLine(0).Y ' Egyezik a kezdeti koordinátákkal

                ' Aktuális szakasz megrajzolása
                Chart.DrawLines(Pens.Green, GridLine)

                ' X koordináta eltolása a következő ciklusra
                DrawOffset(0) += Round((ChartDimension(0) / LineCuts) * 2)

            Next
        End If

        ' ----- FORGALMI DIAGRAMOK -----

        ' Kerekítési hibakorrekció értékének kiszámítása
        StartCorrection = ChartDimension(0) - (Round(ChartDimension(0) / (TraffResolution)) * TraffResolution)

        ' *** FORGALMI DIAGRAM - Feltöltési sebesség ***
        ' Rajzolási irány: vízszintesen balról jobbra; függőlegesen: alulról felfelé.
        ' Megjegyzés: Mivel balról jobbra történik a rajzolás, így a kerekítési hibakorrekció hozzáadása szükséges az eltoláshoz! EZ VAN ALUL!

        ' Értékek definiálása
        Dim UpTraffLine(1) As Point
        Dim UpByteDiffCurrent(2), UpByteDiffLast(2) As Double

        ' Koordinátacsúszás beállítása: vízszintesen eggyel felfelé, függőlegesen eggyel felfelé -> Szegélyhez igazítva
        DrawOffset = {-1, -1}

        ' Kezdőkorrekció: kerekítési hibából adódó eltérés hozzáadása az eltoláshoz.
        DrawOffset(0) += StartCorrection

        ' Feltöltési diagram megrajzolása
        If CheckedUpChart Then
            For Count As Int32 = 0 To (TraffResolution - 1)

                ' Értékek számítása (jelenlegi, eggyel korábbi)
                UpByteDiffCurrent = ScaleConversion(ChartUpNumbers(TraffResolution - Count), 2, True)
                UpByteDiffLast = ScaleConversion(ChartUpNumbers(TraffResolution - (Count + 1)), 2, True)

                ' Koordináták feltöltése (X0, X1, Y0, Y1)
                UpTraffLine(0).X = DrawOffset(0)
                UpTraffLine(1).X = DrawOffset(0) + Round(ChartDimension(0) / (TraffResolution + 1))
                UpTraffLine(0).Y = DrawOffset(1) + ChartCanvas(1) - Round(((UpByteDiffCurrent(0) * (1000 ^ UpByteDiffCurrent(1))) / (PeakDiv * (1000 ^ PeakExp))) * (ChartCanvas(1) - ChartBegining(1)))
                UpTraffLine(1).Y = DrawOffset(1) + ChartCanvas(1) - Round(((UpByteDiffLast(0) * (1000 ^ UpByteDiffLast(1))) / (PeakDiv * (1000 ^ PeakExp))) * (ChartCanvas(1) - ChartBegining(1)))

                ' Aktuális szakasz megrajzolása
                Chart.DrawLines(Pens.Red, UpTraffLine)

                ' X koordináta eltolása a következő ciklusra
                DrawOffset(0) += Round(ChartCanvas(0) / (TraffResolution))

            Next
        End If

        ' *** FORGALMI DIAGRAM - Letöltési sebesség ***
        ' Rajzolási irány: vízszintesen balról jobbra; függőlegesen: alulról felfelé.
        ' Megjegyzés: Mivel balról jobbra történik a rajzolás, így a kerekítési hibakorrekció hozzáadása szükséges az eltoláshoz! EZ VAN FELÜL!

        ' Értékek definiálása
        Dim DownTraffLine(1) As Point
        Dim DownByteDiffCurrent(2), DownByteDiffLast(2) As Double

        ' Koordinátacsúszás beállítása: vízszintesen eggyel felfelé, függőlegesen eggyel felfelé -> Szegélyhez igazítva
        DrawOffset = {-1, -1}

        ' Kezdőkorrekció: kerekítési hibából adódó eltérés hozzáadása az eltoláshoz.
        DrawOffset(0) += StartCorrection

        ' Letöltési diagram megrajzolása
        If CheckedDownChart Then
            For Count As Int32 = 0 To (TraffResolution - 1)

                ' Értékek számítása (jelenlegi, eggyel korábbi)
                DownByteDiffCurrent = ScaleConversion(ChartDownNumbers(TraffResolution - Count), 2, True)
                DownByteDiffLast = ScaleConversion(ChartDownNumbers(TraffResolution - (Count + 1)), 2, True)

                ' Koordináták feltöltése (X0, X1, Y0, Y1)
                DownTraffLine(0).X = DrawOffset(0)
                DownTraffLine(1).X = DrawOffset(0) + Round(ChartCanvas(0) / (TraffResolution + 1))
                DownTraffLine(0).Y = DrawOffset(1) + ChartCanvas(1) - Round(((DownByteDiffCurrent(0) * (1000 ^ DownByteDiffCurrent(1))) / (PeakDiv * (1000 ^ PeakExp))) * (ChartCanvas(1) - ChartBegining(1)))
                DownTraffLine(1).Y = DrawOffset(1) + ChartCanvas(1) - Round(((DownByteDiffLast(0) * (1000 ^ DownByteDiffLast(1))) / (PeakDiv * (1000 ^ PeakExp))) * (ChartCanvas(1) - ChartBegining(1)))

                ' Aktuális szakasz megrajzolása
                Chart.DrawLines(Pens.Lime, DownTraffLine)

                ' X koordináta eltolása a következő ciklusra
                DrawOffset(0) += Round(ChartCanvas(0) / (TraffResolution))

            Next
        End If

        ' ----- SZÖVEGEK HOZZÁADÁSA -----

        ' Értékek definiálása
        Dim IntervalValue As Int32                              ' Időintervallum formázatlan értéke (másodperc)
        Dim IntervalTag As String = Nothing                     ' Időintervallum egységesített értéke (óra, perc, másodperc)
        Dim TextSpacing() As Int32 = {160, 295}                 ' Másod és harmadszintű kiírások vízszintes eltolása
        Dim DownCurrentConv(2), UpCurrentConv(2) As Double      ' Aktuális le- és feltöltési konvertált értékek tömbje (érték, prefixum sorszáma)

        ' Másod és harmadszintű kiírások vízszintes eltolása

        ' *** SZÖVEGES KIÍRÁSOK: Interfész (1. sor) ***

        ' Koordinátacsúszás visszaállítása: Kezdeti kiírás kezdetének beállítása (Vízszintes: 12, Függőleges: 12)
        TextOffset = {12, 12}

        ' Interfész nevének kiírása 
        Chart.DrawString(GetLoc("Interface") + ": " + InterfaceName(SelectedInterface), SignFont, Brushes.DeepSkyBlue, TextOffset(0), TextOffset(1))

        ' *** SZÖVEGES KIÍRÁSOK: Intervallumok (2.sor) ***

        ' Koordináták eltolása: sorugrás
        TextOffset(1) += 15

        ' Egységre állítás (Csak, ha maradék nélkül osztható!)
        IntervalValue = TraffResolution * RefreshInterval(SelectedRefresh)
        If IntervalValue >= 3600 And (IntervalValue Mod 3600) = 0 Then
            ' Másodperc -> Óra
            IntervalTag = (IntervalValue / 3600).ToString + " " + GetLoc("Hours")
        ElseIf IntervalValue >= 60 And (IntervalValue Mod 60) = 0 Then
            ' Másodperc -> Perc
            IntervalTag = (IntervalValue / 60).ToString + " " + GetLoc("Mins")
        Else
            ' Másodperc
            IntervalTag = IntervalValue.ToString + " " + GetLoc("Secs")
        End If

        ' Intervallum sor kiírása
        Chart.DrawString(GetLoc("Interval") + " - " + GetLoc("Traffic") + ": " + PeakDiv.ToString + " " + BytePrefix(PeakExp) + "B / " +
                         GetLoc("Time") + ": " + IntervalTag + " (" + GetLoc("Update") + ": " + RefreshInterval(SelectedRefresh).ToString + " " + GetLoc("Secs") + ")",
                         SignFont, Brushes.DeepSkyBlue, TextOffset(0), TextOffset(1))

        ' *** SZÖVEGES KIÍRÁSOK: Diagram készítési ideje (3.sor) ***

        ' Koordináták eltolása: sorugrás
        TextOffset(1) += 15

        ' Készítési idő kiírása
        Chart.DrawString(GetLoc("ChartTime") + " " + GetLocalizedDate(ChartCreationTime) + ".", SignFont, Brushes.DeepSkyBlue, TextOffset(0), TextOffset(1))

        ' *** SZÖVEGES KIÍRÁSOK: Letöltési jelmagyarázat, értékek (4.sor) ***

        ' Koordináták eltolása: sorugrás
        TextOffset(1) += 22

        ' Jelmagyarázat (X0, X1, Y0, Y1)
        SignLine(0).X = TextOffset(0) + 5
        SignLine(1).X = SignLine(0).X + 15
        SignLine(0).Y = TextOffset(1) + 7
        SignLine(1).Y = SignLine(0).Y ' Egyezik a kezdeti koordinátákkal

        ' Jelszakasz megrajzolása és jelmagyarázat kiírása
        Chart.DrawLines(Pens.Lime, SignLine)
        Chart.DrawString(GetLoc("ChartDown"), SignFont, Brushes.Lime, TextOffset(0) + 25, TextOffset(1))

        ' Jelenlegi és csúcssebesség kiírása (vagy, ha ki van kapcsolva, akkor az erre vonatkozó szöveg)
        DownCurrentConv = ScaleConversion(ChartDownNumbers(0), TraffDigit, True)

        If CheckedDownChart And InterfacePresent Then
            Chart.DrawString(GetLoc("Current") + ": " + FixNumberFormat(DownCurrentConv(0), TraffDigit, True) + " " +
                             BytePrefix(DownCurrentConv(1)) + "B/s", SignFont, Brushes.DarkGreen, TextOffset(0) + TextSpacing(0), TextOffset(1))
            Chart.DrawString(GetLoc("Peak") + ": " + FixNumberFormat(DownPeakConv(0), TraffDigit, True) + " " +
                             BytePrefix(DownPeakConv(1)) + "B/s", SignFont, Brushes.DarkGreen, TextOffset(0) + TextSpacing(1), TextOffset(1))
        Else
            Chart.DrawString(GetLoc("ChartHide"), SignFont, Brushes.Gray, TextOffset(0) + TextSpacing(0), TextOffset(1))
        End If

        ' *** SZÖVEGES KIÍRÁSOK: Letöltési jelmagyarázat, értékek (5.sor) ***

        ' Koordináták eltolása: sorugrás
        TextOffset(1) += 15

        ' Jelmagyarázat (X0, X1, Y0, Y1)
        SignLine(0).X = TextOffset(0) + 5
        SignLine(1).X = SignLine(0).X + 15
        SignLine(0).Y = TextOffset(1) + 7
        SignLine(1).Y = SignLine(0).Y ' Egyezik a kezdeti koordinátákkal

        ' Jelszakasz megrajzolása és jelmagyarázat kiírása
        Chart.DrawLines(Pens.Red, SignLine)
        Chart.DrawString(GetLoc("ChartUp"), SignFont, Brushes.Red, TextOffset(0) + 25, TextOffset(1))

        ' Jelenlegi és csúcssebesség kiírása (vagy, ha ki van kapcsolva, akkor az erre vonatkozó szöveg)
        UpCurrentConv = ScaleConversion(ChartUpNumbers(0), TraffDigit, True)

        If CheckedUpChart And InterfacePresent Then
            Chart.DrawString(GetLoc("Current") + ": " + FixNumberFormat(UpCurrentConv(0), TraffDigit, True) + " " +
                             BytePrefix(UpPeakConv(1)) + "B/s", SignFont, Brushes.DarkRed, TextOffset(0) + TextSpacing(0), TextOffset(1))
            Chart.DrawString(GetLoc("Peak") + ": " + FixNumberFormat(UpPeakConv(0), TraffDigit, True) + " " +
                             BytePrefix(UpPeakConv(1)) + "B/s", SignFont, Brushes.DarkRed, TextOffset(0) + TextSpacing(1), TextOffset(1))
        Else
            Chart.DrawString(GetLoc("ChartHide"), SignFont, Brushes.Gray, TextOffset(0) + TextSpacing(0), TextOffset(1))
        End If

        ' ----- KÉPMŰVELETEK -----

        ' Elkészült kép kirajzolása
        PictureBox_TrafficChart.Image = Picture

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Időzítő időbélyeg ellenőrzése ***
    ' Bemenet: Tolerance  -> eltűrhető késés mértéke másodpercben (Int32)
    ' Kimenet: StampValid -> időbélyeg érvényesség (Boolean)
    Private Function CheckTimerStamp(ByVal Tolerance As Int32)

        ' Értékek definiálása
        Dim TimeDiff As Int32                                   ' Az utolsó és a jelenlegi ciklus közti eltérés
        Dim StampValid As Boolean = False                       ' Bélyeg ellenőrzés -> Ha a tartományon kívül van, akkor hamis!

        ' Az utolsó ciklus és a jelenlegi idő összehasonlítása
        TimeDiff = DateDiff("s", TimerLastTick, DateTime.Now)

        ' Kiesett idő ellenőrzése (pl. készenléti állapot, óraállítás, időszinkron, stb.)
        If Abs(TimeDiff) >= (EventTimer.Interval / 1000) + Tolerance Then
            StampValid = False
        Else
            StampValid = True
        End If

        ' Visszatérési érték beállítása
        Return StampValid

    End Function

    ' *** FÜGGVÉNY: Külső ablakok bezárása ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Private Function CloseExtForms()

        ' Ablakok bezárása
        LoadSplash.Close()
        CPUInfo.Close()
        RAMInfo.Close()
        SMARTInfo.Close()
        IPInfo.Close()

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' ----- ELJÁRÁSOK -----

    ' *** ELJÁRÁS: Közvetlen időzítő ***
    ' Eseményvezérelt: EventTimer.Tick -> Óra ugrása
    Private Sub EventTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EventTimer.Tick

        ' Processzor információk lekérdezése (Csak az órajelek frissítése!)
        SetCPUInformation(False)

        ' Uptime frissítése
        SetUptime()

        ' Időbélyeg ellenőrzés (tolerancia: 3 másodperc)
        If CheckTimerStamp(3) Then

            ' Interfész statisztika frissítése -> Ő állítja be a ChartStop-ot is!
            ' Megjegyzés: Az ellenőrzés azért kell, hogy ne hurokba dobálja fel az üzenetet!
            If Not ChartStop Then UpdateSpeedStatistics(False)

            ' Diagram időzítő állapotának ellenőrzése
            If Not ChartStop Then

                ' IP-infó állapotának beállítása
                Button_IPInfoOpen.Enabled = CheckIPConnection()

                ' Diagramgenerálás időközének beállítása és kiírása az állapotsorban
                If TraffGenCounter = 0 Then

                    ' Diagram ToolTip beállítása (másodpercek kiírásával)
                    EventToolTip.SetToolTip(PictureBox_TrafficChart, GetLoc("Tip_Chart") + " (" + RefreshInterval(SelectedRefresh).ToString + " " + GetLoc("Tip_Average") + ")")

                    ' Forgalmi adatok frissítése
                    UpdateTraffArray(False)

                    ' Rácsfrissítés engedélyezése
                    GridUpdate = True

                    ' Diagram frissítése
                    MakeChart(False)

                    ' Számláló visszaállítása
                    TraffGenCounter = RefreshInterval(SelectedRefresh) - 1

                    ' Diagram állapotkijelzés frissítése
                    StatusLabel_ChartStatus.Image = My.Resources.Resources.Status_Check
                    StatusLabel_ChartStatus.Text = GetLoc("ChartDone") + " " + (TraffGenCounter + 1).ToString + " " + GetLoc("ChartCount") + "..."

                Else

                    ' Diagram állapotkijelzés frissítése
                    StatusLabel_ChartStatus.Image = My.Resources.Resources.Status_Load
                    StatusLabel_ChartStatus.Text = GetLoc("ChartRedraw") + " " + TraffGenCounter.ToString + " " + GetLoc("ChartCount") + "..."

                    ' Számláló csökkentése
                    TraffGenCounter = TraffGenCounter - 1

                End If
            Else

                ' IP-infó állapotának letiltása
                Button_IPInfoOpen.Enabled = False

                ' Diagram állapotkijelzés frissítése
                StatusLabel_ChartStatus.Image = My.Resources.Resources.Status_Check
                StatusLabel_ChartStatus.Text = GetLoc("ChartStop")

            End If
        Else

            ' Statisztika nullázása
            UpdateSpeedStatistics(True)

            ' Diagram frissítése
            MakeChart(True)

        End If

        ' Időbélyegző frissítése
        TimerLastTick = DateTime.Now

    End Sub

    ' *** ELJÁRÁS: Nyelvek betöltése *** 
    ' Eseményvezérelt: ComboBox_LanguageList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub LanguageList_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_LanguageList.SelectedIndexChanged

        ' Változás beállítása
        SelectedLanguage = ComboBox_LanguageList.SelectedIndex

        ' Nyelvi sztringek betöltése
        LoadLocalization(SelectedLanguage)

        ' Hosztnév beálítása az állapotsorban
        StatusLabel_Host.Text = GetLoc("Hostname") + ": " + Hostname

        ' Az ablak címének beállítása
        Me.Text = MyName + " - " + GetLoc("Title") + ", " + GetLoc("Version") + " " + VersionString

        ' Alsó link szövegének beállítása
        Link_Bottom.Text = GetLoc("Title") + " - " + GetLoc("Comment")

        ' Feliratok kitöltése
        Name_HWList.Text = GetLoc("HWList")
        Name_HWVendor.Text = GetLoc("HWVendor")
        Name_HWIdent.Text = GetLoc("HWIdent")
        Name_CPUList.Text = GetLoc("CPUList")
        Name_CPUCore.Text = GetLoc("CPUCore")
        Name_CPUClock.Text = GetLoc("CPUClock")
        Name_CPUMaximum.Text = GetLoc("CPUMaximum")
        Name_MemSize.Text = GetLoc("MemSize")
        Name_MemClock.Text = GetLoc("MemClock")
        Name_MemType.Text = GetLoc("MemType")
        Name_RAMList.Text = GetLoc("MemList")
        Name_MemVisible.Text = GetLoc("MemVisible")
        Name_OSName.Text = GetLoc("OSName")
        Name_OSVersion.Text = GetLoc("OSVersion")
        Name_OSLang.Text = GetLoc("OSLang")
        Name_OSRelease.Text = GetLoc("OSRelease")
        Name_DiskList.Text = GetLoc("DiskList")
        Name_DiskType.Text = GetLoc("MediaType")
        Name_DiskInterface.Text = GetLoc("DiskInterface")
        Name_DiskFirmware.Text = GetLoc("DiskFirmware")
        Name_PartList.Text = GetLoc("PartList")
        Name_DiskSerial.Text = GetLoc("DiskSerial")
        Name_VideoList.Text = GetLoc("VideoList")
        Name_VideoMemory.Text = GetLoc("VideoMemory")
        Name_VideoResolution.Text = GetLoc("VideoResolution")
        Name_InterfaceList.Text = GetLoc("InterfaceList")
        Name_Bandwidth.Text = GetLoc("Bandwidth")
        Name_InterfaceUsage.Text = GetLoc("InterfaceUsage")
        Name_DownloadSpeed.Text = GetLoc("DownloadSpeed")
        Name_UploadSpeed.Text = GetLoc("UploadSpeed")
        Name_ChartVisible.Text = GetLoc("ChartVisible")
        Name_UpdateList.Text = GetLoc("UpdateList")
        Name_UpdateUnit.Text = GetLoc("UpdateUnit")

        ' Checkbox és Combobox ToolTip értékek beállítása
        EventToolTip.SetToolTip(ComboBox_LanguageList, GetLoc("Tip_Language"))
        EventToolTip.SetToolTip(ComboBox_HWList, GetLoc("Tip_HW"))
        EventToolTip.SetToolTip(PictureBox_TrafficChart, GetLoc("Tip_Chart") + " (" + RefreshInterval(SelectedRefresh).ToString + " " + GetLoc("Tip_Average") + ")") ' Az átlagértékek is hozzáadva!
        EventToolTip.SetToolTip(CheckBoxChart_DownloadVisible, GetLoc("Tip_ChartDown"))
        EventToolTip.SetToolTip(CheckBoxChart_UploadVisible, GetLoc("Tip_ChartUp"))
        EventToolTip.SetToolTip(ComboBox_UpdateList, GetLoc("Tip_Refresh"))
        EventToolTip.SetToolTip(Link_Bottom, GetLoc("Tip_LinkBottom"))

        ' Gomb tooltipek
        EventToolTip.SetToolTip(Button_CPUInfoOpen, GetLoc("CPUTitle") + "...")
        EventToolTip.SetToolTip(Button_RAMInfoOpen, GetLoc("RAMTitle") + "...")
        EventToolTip.SetToolTip(Button_SMARTInfoOpen, GetLoc("SMARTTitle") + "...")
        EventToolTip.SetToolTip(Button_IPInfoOpen, GetLoc("IPTitle") + "...")
        EventToolTip.SetToolTip(Button_DiskListReload, GetLoc("Tip_Reload"))
        EventToolTip.SetToolTip(Button_VideoListReload, GetLoc("Tip_Reload"))
        EventToolTip.SetToolTip(Button_InterfaceListReload, GetLoc("Tip_Reload"))
        EventToolTip.SetToolTip(Button_Exit, GetLoc("Tip_Exit"))

        ' Állapot- és menüsori ToolTip kiírások beállítása
        StatusLabel_Host.ToolTipText = GetLoc("Tip_Hostname")
        StatusLabel_Uptime.ToolTipText = GetLoc("Tip_Uptime")
        StatusLabel_ChartStatus.ToolTipText = GetLoc("Tip_Status")
        StatusLabel_TopMost.ToolTipText = GetLoc("Tip_TopMost")
        ScreenshotToolStripMenuItem.ToolTipText = GetLoc("Tip_Screenshot")

        ' Csoportfeliratok
        GroupBox_HWInfo.Text = GetLoc("GB_HWInfo")
        GroupBox_CPUInfo.Text = GetLoc("GB_CPUInfo")
        GroupBox_MemoryInfo.Text = GetLoc("GB_MemoryInfo")
        GroupBox_OSInfo.Text = GetLoc("GB_OSInfo")
        GroupBox_DiskInfo.Text = GetLoc("GB_DiskInfo")
        GroupBox_VideoInfo.Text = GetLoc("GB_VideoInfo")
        GroupBox_Network.Text = GetLoc("GB_Network")

        ' Menüelemek
        MainMenuItem_Settings.Text = GetLoc("Menu_Settings")
        MainMenuItem_Chart.Text = GetLoc("Menu_Chart")
        MainMenuItem_Information.Text = GetLoc("Menu_Information")
        MainMenu_SettingsItem_TopMost.Text = GetLoc("Menu_TopMost")
        MainMenu_SettingsItem_TaskbarMinimize.Text = GetLoc("Menu_TaskbarMinimize")
        MainMenu_SettingsItem_DisableConfirm.Text = GetLoc("Menu_DisableConfirm")
        MainMenu_SettingsItem_DisableSplash.Text = GetLoc("Menu_DisableSplash")
        MainMenu_ActionItem_UpdateCheck.Text = GetLoc("Menu_UpdateCheck")
        MainMenu_ActionItem_About.Text = GetLoc("Menu_About")
        MainMenu_ActionItem_Exit.Text = GetLoc("Menu_Exit")
        MainMenu_ChartItem_DownloadVisible.Text = GetLoc("Menu_DownloadVisible")
        MainMenu_ChartItem_UploadVisible.Text = GetLoc("Menu_UploadVisible")
        MainMenu_ChartItem_SaveChart.Text = GetLoc("Menu_SaveChart")
        MainMenu_ChartItem_ClearChart.Text = GetLoc("Menu_ClearChart")

        ' Egyező menüelemek feltöltése (Egyező néven fut, csak másik menüben!)
        MainContextMenuItem_TopMost.Text = MainMenu_SettingsItem_TopMost.Text
        MainContextMenuItem_TaskbarMinimize.Text = MainMenu_SettingsItem_TaskbarMinimize.Text
        MainContextMenuItem_DisableConfirm.Text = MainMenu_SettingsItem_DisableConfirm.Text
        MainContextMenuItem_DisableSplash.Text = MainMenu_SettingsItem_DisableSplash.Text
        MainContextMenuItem_UpdateCheck.Text = MainMenu_ActionItem_UpdateCheck.Text
        MainContextMenuItem_About.Text = MainMenu_ActionItem_About.Text
        MainContextMenuItem_Exit.Text = MainMenu_ActionItem_Exit.Text
        ChartMenuItem_DownloadVisible.Text = MainMenu_ChartItem_DownloadVisible.Text
        ChartMenuItem_UploadVisible.Text = MainMenu_ChartItem_UploadVisible.Text
        ChartMenuItem_SaveChart.Text = MainMenu_ChartItem_SaveChart.Text
        ChartMenuItem_ClearChart.Text = MainMenu_ChartItem_ClearChart.Text

        ' Checkbox feliratok
        CheckBoxChart_DownloadVisible.Text = GetLoc("CB_DownloadVisible")
        CheckBoxChart_UploadVisible.Text = GetLoc("CB_UploadVisible")

        ' Gombfeliratok
        Button_Exit.Text = GetLoc("Button_Exit")

        ' Kis méretű buborék visszaállítása (Nyelvváltás után a választott nyelven is jelenjen meg adott esetben!)
        DisableBalloon = False

        ' Hardver és OS információk frissítése (Minden esetben!)
        SetHWInformation()
        SetOSInformation()

        ' Lemez és videokártya információk újratöltése
        If MainWindowDone Then

            ' Lemez információk frissítése, ha nem lett eltávolítva
            If CheckDiskAvailable() Then SetDiskInformation()

            ' Videokártya információk frissítése
            SetVideoInformation()

        End If

        ' Hardverlista frissítése
        UpdateHWList(False)

        ' Memória információk frissítése
        SetMemoryInformation()

        ' Hamis listaelem hozzáadása, ha nincs jelen interfész
        If InterfacePresent = False Then
            ComboBox_InterfaceList.Items.Clear()
            InterfaceName(0) = GetLoc("NotAvailable")
            ComboBox_InterfaceList.Items.Add(InterfaceName(0))
            ComboBox_InterfaceList.SelectedIndex = 0
        End If

        ' Taskbar ikon nevének beállítása (Csak betöltés utáni nyelvváltáskor!)
        If MainWindowDone Then
            MainNotifyIcon.Text = MyName + " - " + GetLoc("Version") + " " + VersionString
        End If

        ' Diagram frissítése
        MakeChart(False)

    End Sub

    ' *** ELJÁRÁS: Frissítési időköz kiválasztása ***
    ' Eseményvezérelt: ComboBox_UpdateList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub UpdateList_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_UpdateList.SelectedIndexChanged

        ' Változás beállítása
        SelectedRefresh = ComboBox_UpdateList.SelectedIndex

        ' Diagram ToolTip beállítása (másodpercek kiírásával)
        EventToolTip.SetToolTip(PictureBox_TrafficChart, GetLoc("Tip_Chart") + " (" + RefreshInterval(SelectedRefresh).ToString + " " + GetLoc("Tip_Average") + ")")

        ' Diagram frissítése
        MakeChart(True)

    End Sub

    ' *** ELJÁRÁS: Hardverkomponens kiválasztása ***
    ' Eseményvezérelt: ComboBox_UpdateList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub HWList_Change(sender As Object, e As EventArgs) Handles ComboBox_HWList.SelectedIndexChanged

        ' Változás beállítása
        SelectedHardware = ComboBox_HWList.SelectedIndex

        ' Komponensek kiírásának beállítása
        If HWVendor(SelectedHardware) = Nothing Then
            Value_HWVendor.Enabled = False
            Value_HWVendor.Text = GetLoc("NotAvailable")
        Else
            Value_HWVendor.Enabled = True
            Value_HWVendor.Text = HWVendor(SelectedHardware)
        End If

        If HWIdentifier(SelectedHardware) = Nothing Then
            Value_HWIdent.Enabled = False
            Value_HWIdent.Text = GetLoc("NotAvailable")
        Else
            Value_HWIdent.Enabled = True
            Value_HWIdent.Text = HWIdentifier(SelectedHardware)
        End If

    End Sub

    ' *** ELJÁRÁS: Processzor kiválasztása ***
    ' Eseményvezérelt: ComboBox_CPUList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub ComboBox_CPUList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_CPUList.SelectedIndexChanged

        ' Változás beállítása
        SelectedCPU = ComboBox_CPUList.SelectedIndex

        ' Processzor információk lekérdezése (Magok és szálak számával együtt!)
        SetCPUInformation(True)

        ' ToolTip érték beállítása
        EventToolTip.SetToolTip(ComboBox_CPUList, ComboBox_CPUList.Items(SelectedCPU))

    End Sub

    ' *** ELJÁRÁS: Memóriamodul kiválasztása ***
    ' Eseményvezérelt: ComboBox_MemList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub MemList_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_RAMList.SelectedIndexChanged

        ' Változás beállítása
        SelectedMemory = ComboBox_RAMList.SelectedIndex

        ' ToolTip érték beállítása
        EventToolTip.SetToolTip(ComboBox_RAMList, ComboBox_RAMList.Items(SelectedMemory))

    End Sub

    ' *** ELJÁRÁS: Meghajtó kiválasztása ***
    ' Eseményvezérelt: ComboBox_DiskList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub DiskList_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_DiskList.SelectedIndexChanged

        ' Változás ellenőrzése
        If ComboBox_DiskList.SelectedIndex <> SelectedDisk Then

            ' Változás beállítása
            SelectedDisk = ComboBox_DiskList.SelectedIndex

            ' Partíciólista beállításra alapértelmezettre 
            SelectedPartition = 0

        End If

        ' Lemez információk frissítése, ha nem lett eltávolítva
        If CheckDiskAvailable() Then SetDiskInformation()

        ' ToolTip érték beállítása
        EventToolTip.SetToolTip(ComboBox_DiskList, ComboBox_DiskList.Items(SelectedDisk))

    End Sub

    ' *** ELJÁRÁS: Partíció kiválasztása ***
    ' Eseményvezérelt: ComboBox_DiskList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub PartList_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_PartList.SelectedIndexChanged

        ' Változás beállítása
        SelectedPartition = ComboBox_PartList.SelectedIndex

        ' Meghajtóbetűjelek kiírása
        If PartLabel(SelectedPartition) = Nothing Then
            Value_PartLabel.Enabled = False
            Value_PartLabel.Text = GetLoc("None")
        Else
            Value_PartLabel.Enabled = True
            Value_PartLabel.Text = PartLabel(SelectedPartition)
        End If

        ' Partícióinformációk kiírása
        If PartInfo(SelectedPartition) = Nothing Then
            Value_PartInfo.Enabled = False
            Value_PartInfo.Text = GetLoc("NotAvailable")
        Else
            Value_PartInfo.Enabled = True
            Value_PartInfo.Text = PartInfo(SelectedPartition)
        End If

        ' ToolTip érték beállítása
        EventToolTip.SetToolTip(ComboBox_PartList, ComboBox_PartList.Items(SelectedPartition))

    End Sub

    ' *** ELJÁRÁS: Videokártya kiválasztása ***
    ' Eseményvezérelt: ComboBox_VideoList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub ComboBox_VideoList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_VideoList.SelectedIndexChanged

        ' Változás beállítása
        SelectedVideo = ComboBox_VideoList.SelectedIndex

        ' Videokártya információk lekérdezése
        SetVideoInformation()

        ' ToolTip érték beállítása
        EventToolTip.SetToolTip(ComboBox_VideoList, ComboBox_VideoList.Items(SelectedVideo))

    End Sub

    ' *** ELJÁRÁS: Interfész kiválasztása ***
    ' Eseményvezérelt: ComboBox_InterfaceList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub InterfaceList_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_InterfaceList.SelectedIndexChanged

        ' Változás beállítása
        SelectedInterface = ComboBox_InterfaceList.SelectedIndex

        ' IP-infó állotának beállítása
        Button_IPInfoOpen.Enabled = False

        ' Statisztika nullázása
        UpdateSpeedStatistics(True)

        ' Diagram frissítése
        MakeChart(True)

        ' ToolTip érték beállítása
        EventToolTip.SetToolTip(ComboBox_InterfaceList, ComboBox_InterfaceList.Items(SelectedInterface))

    End Sub

    ' *** ELJÁRÁS: Meghajtólista újratöltése ***
    ' Eseményvezérelt: Button_DiskListReload.Click -> Gomb megnyomása
    Private Sub DiskList_Reload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_DiskListReload.Click

        ' Lemezlista újratöltése
        UpdateDiskList(True)

    End Sub

    ' *** ELJÁRÁS: Videokártya lista újratöltése ***
    ' Eseményvezérelt: Button_VideoListReload.Click -> Gomb megnyomása
    Private Sub VideoList_Reload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_VideoListReload.Click

        ' Videokártya lista újratöltése
        UpdateVideoList(True)

    End Sub

    ' *** ELJÁRÁS: Interfész lista újratöltése ***
    ' Eseményvezérelt: Button_InterfaceListReload.Click -> Gomb megnyomása
    Private Sub InterfaceList_Reload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_InterfaceListReload.Click

        ' Interfész lista újratöltése
        UpdateInterfaceList(True)

    End Sub

    ' *** ELJÁRÁS: CPU-infó ablak megnyitása ***
    ' Eseményvezérelt: Button_CPUInfoOpen.Click -> Klikk (CPU-infó gomb)
    Private Sub CPUInfo_Open(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_CPUInfoOpen.Click

        ' Külső ablakok bezárása
        CloseExtForms()

        ' CPU-infó ablak megnyitása
        CPUInfo.Visible = True

    End Sub

    ' *** ELJÁRÁS: RAM-infó ablak megnyitása ***
    ' Eseményvezérelt: Button_RAMInfoOpen.Click -> Klikk (CPU-infó gomb)
    Private Sub RAMInfo_Open(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RAMInfoOpen.Click

        ' Külső ablakok bezárása
        CloseExtForms()

        ' CPU-infó ablak megnyitása
        RAMInfo.Visible = True

    End Sub

    ' *** ELJÁRÁS: S.M.A.R.T-infó ablak megnyitása ***
    ' Eseményvezérelt: Button_SMARTInfoOpen.Click -> Klikk (Lemez S.M.A.R.T gomb)
    Private Sub SMARTInfo_Open(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SMARTInfoOpen.Click

        ' Külső ablakok bezárása
        CloseExtForms()

        ' Lemez elérhetőségének és S.M.A.R.T lekérdezés hibájának ellenőrzése
        If CheckDiskAvailable() And SmartException = False Then

            ' S.M.A.R.T ablak megnyitása
            SMARTInfo.Visible = True

        End If

    End Sub

    ' *** ELJÁRÁS: IP-infó ablak megnyitása ***
    ' Eseményvezérelt: Button_IPInfoOpen.Click -> Klikk (IP-infó gomb)
    Private Sub IPInfo_Open(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_IPInfoOpen.Click

        ' Külső ablakok bezárása
        CloseExtForms()

        ' IP-infó ablak megnyitása
        IPInfo.Visible = True

    End Sub

    ' *** ELJÁRÁS: Letöltési diagram generálásának ellenőrzése ***
    ' Eseményvezérelt: MainMenu_ChartItem_DownloadVisible.Click, ChartMenuItem_DownloadVisible.Click, CheckBoxChart_DownloadVisible.Click -> Klikk (Menüelem, Checkbox)
    Private Sub DownloadChartVisible_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_ChartItem_DownloadVisible.Click, ChartMenuItem_DownloadVisible.Click, CheckBoxChart_DownloadVisible.Click

        ' Változás ellenőrzése és állapot invertálása
        If CheckedDownChart Then
            CheckedDownChart = False
        Else
            CheckedDownChart = True
        End If

        ' Checkbox és menüelemek állapotának beállítása
        MainMenu_ChartItem_DownloadVisible.Checked = CheckedDownChart
        ChartMenuItem_DownloadVisible.Checked = CheckedDownChart
        CheckBoxChart_DownloadVisible.Checked = CheckedDownChart

        ' Diagram frissítése
        MakeChart(False)

    End Sub

    ' *** ELJÁRÁS: Feltöltési diagram generálásának ellenőrzése ***
    ' Eseményvezérelt: MainMenu_ChartItem_UploadVisible.Click, ChartMenuItem_UploadVisible.Click, CheckBoxChart_UploadVisible.Click -> Klikk (Menüelem, Checkbox)
    Private Sub UploadChartVisible_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_ChartItem_UploadVisible.Click, ChartMenuItem_UploadVisible.Click, CheckBoxChart_UploadVisible.Click

        ' Változás ellenőrzése és állapot invertálása
        If CheckedUpChart Then
            CheckedUpChart = False
        Else
            CheckedUpChart = True
        End If

        ' Checkbox és menüelemek állapotának beállítása
        MainMenu_ChartItem_UploadVisible.Checked = CheckedUpChart
        ChartMenuItem_UploadVisible.Checked = CheckedUpChart
        CheckBoxChart_UploadVisible.Checked = CheckedUpChart

        ' Diagram frissítése
        MakeChart(False)

    End Sub

    ' *** ELJÁRÁS: Kilépési procedúra megindítása (közvetett) ***
    ' Eseményvezérelt: Button_Exit.Click, MainMenu_ActionItem_Exit.Click, MainContextMenuItem_Exit.Click -> Klikk (Gomb, Menüelem)
    Private Sub MainWindow_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click, MainMenu_ActionItem_Exit.Click, MainContextMenuItem_Exit.Click

        ' Főablak bezárása
        Me.Close()

    End Sub

    ' *** ELJÁRÁS: Kilépés a főablakból (közvetlen) ***
    ' Eseményvezérelt: Me.FormClosing -> Ablak bezárása (A 'Me.Close' által, vagy a jobb felső bezárás gombra kattintva)
    Private Sub MainWindow_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        ' Külső ablakok bezárása
        CloseExtForms()

        ' Beállításjegyzék értékeinek mentése, ha a főablak hiba nélkül betöltött
        If MainWindowDone Then SetRegValues()

        ' Kilépési megerősítés
        If CheckedNoQuitAsk = False Then

            ' Folyamtos láthatóság kikapcsolása (FONTOS! Ha ez nincs, akkor nem látszik a kilépési megerősítő ablak!!!)
            Me.TopMost = False

            ' Megerősítőablak megjelenítése (Igen -> Kilépés, Nem -> Mégse)
            e.Cancel = MsgBox(GetLoc("MsgQuitText"), vbQuestion + vbYesNo + vbMsgBoxSetForeground, GetLoc("MsgQuitTitle")) = MsgBoxResult.No

            ' Folyamatos láthatósag visszaállítása
            Me.TopMost = CheckedTopMost

        End If

    End Sub

    ' *** ELJÁRÁS: Kis méret gomb megnyomása ***
    ' Eseményvezérelt: Me.Resize -> Ablak átméretezése
    Private Sub MainWindow_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        ' Külső ablakok bezárása
        CloseExtForms()

        ' Kis méret állapotának beállítása
        If Me.WindowState = FormWindowState.Minimized And CheckedMinToTray Then
            Me.Visible = False

            ' Buboréküzenet megjelenítése, majd későbbi kikapcsolása (Csak az első tálcára tételkor jelenik meg!)
            If Not DisableBalloon Then
                OpenFile = False
                MainNotifyIcon.ShowBalloonTip(3000, MyName + " - " + GetLoc("Note"), GetLoc("Taskbar"), ToolTipIcon.Info)
                DisableBalloon = True
            End If
        End If

    End Sub

    ' *** ELJÁRÁS: Tálcaikon duplaklikk kezelése (Kis méret: oda-vissza) ***
    ' Eseményvezérelt: MainNotifyIcon.MouseDoubleClick -> Dupla klikk (Taskbar ikon)
    Private Sub MainNotifyIcon_DoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainNotifyIcon.MouseDoubleClick

        ' Főablak betöltésének figyelése
        If MainWindowDone Then

            ' Külső ablakok bezárása
            CloseExtForms()

            ' Változás ellenőrzése és állapot invertálása
            If Me.WindowState = FormWindowState.Normal Then
                If CheckedMinToTray Then
                    Me.Visible = False
                    If Not DisableBalloon Then
                        MainNotifyIcon.ShowBalloonTip(3000, MyName, GetLoc("Taskbar"), ToolTipIcon.Info)
                        DisableBalloon = True
                    End If
                End If
                Me.WindowState = FormWindowState.Minimized
            Else
                Me.Visible = True
                Me.WindowState = FormWindowState.Normal
            End If

        End If

    End Sub

    ' *** ELJÁRÁS: Kicsinyítés a tálcára ***
    ' Eseményvezérelt: MainMenu_SettingsItem_TaskbarMinimize.Click, MainContextMenuItem_TaskbarMinimize.Click -> Állapotváltozás (Menüelem)
    Private Sub MinToTray_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_SettingsItem_TaskbarMinimize.Click, MainContextMenuItem_TaskbarMinimize.Click

        ' Változás ellenőrzése és állapot invertálása
        If MainContextMenuItem_TaskbarMinimize.Checked Then
            CheckedMinToTray = False
            If Me.WindowState = FormWindowState.Minimized Then
                Me.Visible = True
            End If
        Else
            CheckedMinToTray = True
            If Me.WindowState = FormWindowState.Minimized Then
                Me.Visible = False
            End If
        End If

        ' Menüelem állapotának beállítása
        MainMenu_SettingsItem_TaskbarMinimize.Checked = CheckedMinToTray
        MainContextMenuItem_TaskbarMinimize.Checked = CheckedMinToTray

    End Sub

    ' *** ELJÁRÁS: Főablak láthatóságának beállítása ***
    ' Eseményvezérelt: MainMenu_SettingsItem_TopMost.Click, StatusLabel_TopMost.Click, MainContextMenuItem_TopMost.Click -> Állapotváltozás (StatusLabel, Menüelem)
    Private Sub TopMost_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_SettingsItem_TopMost.Click, StatusLabel_TopMost.Click, MainContextMenuItem_TopMost.Click

        ' Külső ablakok bezárása
        CloseExtForms()

        ' Változás ellenőrzése és állapot invertálása
        If Me.TopMost Then
            CheckedTopMost = False
            StatusLabel_TopMost.Image = My.Resources.Resources.Status_Pin_Red
        Else
            CheckedTopMost = True
            StatusLabel_TopMost.Image = My.Resources.Resources.Status_Pin_Green
        End If

        ' Láthatóság beállítása
        Me.TopMost = CheckedTopMost

        ' Menüelem állapotának beállítása
        MainMenu_SettingsItem_TopMost.Checked = CheckedTopMost
        MainContextMenuItem_TopMost.Checked = CheckedTopMost

    End Sub

    ' *** ELJÁRÁS: Forgalmi diagram törlése ***
    ' Eseményvezérelt: MainMenu_ChartItem_ClearChart.Click, ChartMenuItem_ClearChart.Click -> Állapotváltozás (Menüelem)
    Private Sub Chart_Clear(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_ChartItem_ClearChart.Click, ChartMenuItem_ClearChart.Click

        ' Diagram frissítése
        MakeChart(True)

    End Sub

    ' *** ELJÁRÁS: Kilépési megerősítés kikapcsolása ***
    ' Eseményvezérelt: MainMenu_SettingsItem_DisableConfirm.Click, MainContextMenuItem_DisableConfirm.Click -> Állapotváltozás (Menüelem)
    Private Sub NoQuitAsk_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_SettingsItem_DisableConfirm.Click, MainContextMenuItem_DisableConfirm.Click

        ' Változás ellenőrzése és állapot invertálása
        If MainContextMenuItem_DisableConfirm.Checked Then
            CheckedNoQuitAsk = False
        Else
            CheckedNoQuitAsk = True
        End If

        ' Menüelem állapotának beállítása
        MainMenu_SettingsItem_DisableConfirm.Checked = CheckedNoQuitAsk
        MainContextMenuItem_DisableConfirm.Checked = CheckedNoQuitAsk

    End Sub

    ' *** ELJÁRÁS: Betöltőképernyő elrejtése ***
    ' Eseményvezérelt: MainMenu_SettingsItem_DisableSplash.Click, MainContextMenuItem_DisableSplash.Click -> Állapotváltozás (Menüelem)
    Private Sub DisableSplash_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_SettingsItem_DisableSplash.Click, MainContextMenuItem_DisableSplash.Click

        ' Változás ellenőrzése és állapot invertálása
        If MainContextMenuItem_DisableSplash.Checked Then
            CheckedSplashDisable = False
        Else
            CheckedSplashDisable = True
        End If

        MainMenu_SettingsItem_DisableSplash.Checked = CheckedSplashDisable
        MainContextMenuItem_DisableSplash.Checked = CheckedSplashDisable

    End Sub

    ' *** ELJÁRÁS: Frissítés keresése ***
    ' Eseményvezérelt: MainMenu_ActionItem_UpdateCheck.Click, MainContextMenuItem_UpdateCheck.Click -> Klikk (Menüelem)
    Private Sub UpdateCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_ActionItem_UpdateCheck.Click, MainContextMenuItem_UpdateCheck.Click

        ' Link megnyitása
        System.Diagnostics.Process.Start(MyLink)

    End Sub

    ' *** ELJÁRÁS: Névjegy megjelenítése ***
    ' Eseményvezérelt: MainMenu_ActionItem_About.Click, MainContextMenuItem_About.Click, Link_Bottom.LinkClicked -> Klikk (Menüelem, Link)
    Private Sub LoadSplash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_ActionItem_About.Click, MainContextMenuItem_About.Click, Link_Bottom.LinkClicked

        ' Külső ablakok bezárása
        CloseExtForms()

        ' Splash időzítő újbóli elindítása és ablak megjelenítése
        LoadSplash.SplashTimer.Enabled = False
        LoadSplash.Visible = True

    End Sub

    ' *** ELJÁRÁS: Forgalmi diagram mentése az asztalra ***
    ' Eseményvezérelt: MainMenu_ChartItem_SaveChart.Click, ChartMenuItem_SaveChart.Click, PictureBox_TrafficChart.MouseDoubleClick -> Klikk (Menüelem), Duplaklikk (Kép)
    Private Sub Chart_Save(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_ChartItem_SaveChart.Click, ChartMenuItem_SaveChart.Click, PictureBox_TrafficChart.MouseDoubleClick

        ' Mentési kép létrhozása és feltöltése
        Dim SaveImage As New Bitmap(PictureBox_TrafficChart.Size.Width, PictureBox_TrafficChart.Size.Height, Imaging.PixelFormat.Format24bppRgb)
        SaveImage = PictureBox_TrafficChart.Image

        ' Fájnév generálása
        Dim FileName As String = MyName + "_TrafficChart_" + Hostname + "_" + Format(ChartCreationTime, "yyyyMMdd-HHmmss") + ".png"

        ' Elérési út beállítása (Desktop)
        Dim DesktopPath As String = My.Computer.FileSystem.SpecialDirectories.Desktop
        SavePath = DesktopPath + "\" + FileName

        ' Kép mentése (PNG)
        SaveImage.Save(SavePath, Imaging.ImageFormat.Png)

        ' Buboréküzenet állapotának beállítása (fájl megnyitása) és üzenet megjelenítése
        OpenFile = True
        MainNotifyIcon.ShowBalloonTip(5000, MyName + " - " + GetLoc("Note"), GetLoc("ImageSaved") + ": '" + SavePath + "'", ToolTipIcon.Info)

    End Sub

    ' *** ELJÁRÁS: Képernyőkép mentése ***
    ' Eseményvezérelt: ScreenshotToolStripMenuItem.Click -> Klikk (Menüelem)
    Private Sub Screenshot_Save(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScreenshotToolStripMenuItem.Click

        ' Mentési kép létrhozása és feltöltése
        Dim SaveImage As New Bitmap(Me.Width, Me.Height, Imaging.PixelFormat.Format24bppRgb)
        DrawToBitmap(SaveImage, New Rectangle(0, 0, Me.Width, Me.Height))

        ' Kép készítési idejének frissítése
        Dim ScreenCreationTime As DateTime = DateTime.Now

        ' Fájnév generálása
        Dim FileName As String = MyName + "_Screenshot_" + Hostname + "_" + Format(ScreenCreationTime, "yyyyMMdd-HHmmss") + ".png"

        ' Elérési út beállítása (Desktop)
        Dim DesktopPath As String = My.Computer.FileSystem.SpecialDirectories.Desktop
        SavePath = DesktopPath + "\" + FileName

        ' Kép mentése (PNG)
        SaveImage.Save(SavePath, Imaging.ImageFormat.Png)

        ' Buboréküzenet állapotának beállítása (fájl megnyitása) és üzenet megjelenítése
        OpenFile = True
        MainNotifyIcon.ShowBalloonTip(5000, MyName + " - " + GetLoc("Note"), GetLoc("ImageSaved") + ": '" + SavePath + "'", ToolTipIcon.Info)

    End Sub

    ' *** ELJÁRÁS: Buboréküzenetre kattintás kezelése ***
    ' Eseményvezérelt: MainNotifyIcon.BalloonTipClicked -> Klikk (Taskbar ikon buboréküzenet)
    Private Sub MainNotifyIcon_BalloonTipClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainNotifyIcon.BalloonTipClicked

        ' Kép megnyitása, a buboréközenet fájl mentésére vonatkozik (egyébként a főablak előtérbe hozása)
        If OpenFile Then
            Process.Start(SavePath)
        Else
            ' Főablak előtérbe hozása
            Me.Visible = True
            Me.WindowState = FormWindowState.Normal
        End If

    End Sub

End Class