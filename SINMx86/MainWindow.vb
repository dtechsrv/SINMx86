Imports System
Imports System.Timers
Imports System.Management
Imports System.Math
Imports System.Drawing
Imports System.Convert
Imports Microsoft.Win32

Public Class MainWindow

    ' Alkalmazás adatai
    Public MyVersion As String = Application.ProductVersion                 ' Saját verziószám
    Public MyName As String = My.Application.Info.Title                     ' Program neve
    Public MyLink As String = My.Application.Info.Description               ' Támogatási link
    Public SplashDefineAsAbout As Boolean = False                           ' Splash ablak funkciójának betöltése (True: névjegy, False: betöltőképernyő)

    ' WMI feldolgozási objektumok
    Public objOS, objBB, objCS, objBS, objPR, objNI, objVC, objDD As ManagementObjectSearcher
    Public objMgmt As ManagementObject

    ' Beállításjegyzék változói
    Public RegPath As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\\" + MyName, True)

    ' Nyelvi sztringek
    Public Str_Title, Str_Comment, Str_Version, Str_Build, Str_Loading, Str_LoadReg, Str_LoadWMI, Str_MemUse, Str_MemFree, Str_Uptime,
           Str_DayName(7), Str_MonthName(12), Str_DateFormat, Str_Days, Str_Hours, Str_Mins, Str_Secs, Str_And, Str_DigitSeparator,
           Str_Motherboard, Str_System, Str_BIOS, Str_Interface, Str_Interval, Str_Traffic, Str_Time, Str_Update, Str_Current, Str_Peak,
           Str_ChartTime, Str_ChartDown, Str_ChartUp, Str_ChartHide, Str_ChartRedraw, Str_ChartDone, Str_QuitAsk, Str_QuitTitle, Str_Hostname,
           Str_ChartCount, Str_Note, Str_NoDisk, Str_Unknown, Str_Invalid, Str_Inactive, Str_Taskbar, Str_ImageSaved, Str_Close As String

    ' ToolTip sztringek
    Public Tip_Language, Tip_CPU, Tip_Disk, Tip_Video, Tip_Interface, Tip_Chart, Tip_Average, Tip_Reload, Tip_ChartDown, Tip_ChartUp,
           Tip_Refresh, Tip_Exit, Tip_LinkBottom, Tip_Hostname, Tip_Uptime, Tip_Status, Tip_TopMost, Tip_Screenshot As String

    ' Checkboxok és menüelemek változói
    Public CheckedSplashDisable, CheckedDownChart, CheckedUpChart, CheckedTopMost, CheckedNoQuitAsk, CheckedMinToTray As Boolean

    ' Listák kiválasztott elemeinek sorszáma
    Public SelectedHardware, SelectedCPU, SelectedInterface, SelectedLanguage, SelectedRefresh, SelectedDisk, SelectedVideo As Int32

    ' További változók
    Public ReleaseStatus As String = Nothing                                ' Kiadás állapota ('BETA', 'RC', vagy stabil verzió esetén üres)
    Public VersionString As String                                          ' Formázott verziószám
    Public HWVendor(2), HWIdentifier(2) As String                           ' Komponensinformációs tömbök
    Public OSMajorVersion, OSMinorVersion As Int32                          ' OS fő- és alverzió száma (hibakereséshez)
    Public OSRelase As Int32                                                ' Kiadás típusa (32/64 bit)
    Public RefreshInterval() As Int32 = {1, 2, 3, 4, 5, 10, 15, 30, 60}     ' Frissítési intervallumok
    Public PrefixTable() As String = {"", "k", "M", "G", "T"}               ' Prefixumok tömbje (amíg szükséges lehet)
    Public TraffGenCounter As Int32                                         ' Diagram generálási időköz visszaszámlálója
    Public Hostname As String                                               ' Hosztnév
    Public HWList(2) As String                                              ' Hardver komonensek listája
    Public CPUName(32) As String                                            ' Processzorok nevei (kiírásokhoz)
    Public InterfaceList(32) As String                                      ' Interfészlista tömbje (lekérdezésekhez)
    Public InterfaceName(32) As String                                      ' Interfészek formázott neve (kiírásokhoz)
    Public DiskList(32) As String                                           ' Meghajtóindexek tömbje (lekérdezésekhez)
    Public DiskName(32) As String                                           ' Meghajtók neve (kiírásokhoz)
    Public VideoName(32) As String                                          ' Videókártyák nevei (kiírásokhoz)
    Public TraffResolution As Int32 = 60                                    ' Diagramon jelzett értékek száma (ennyi időegységre van felosztva a diagram)
    Public VerticalGrids As Int32 = 1                                       ' Fuggőleges osztóvonalak száma (másodpercre vetítve)
    Public GridSlip As Int32                                                ' Függőleges rács eltolás
    Public GridUpdate As Boolean = False                                    ' Rácsfrissítés engedélyezése (kell az eltolás számításához)
    Public TimerSeconds, UptimeSeconds As Int32                             ' Időértékek: időzítő, futásidő
    Public LatestDownload, LatestUpload As Int64                            ' Utolsó kiolvasott le- és feltöltési bájtok száma (az aktuális sebességszámításhoz kell)
    Public ChartCreationTime As DateTime                                    ' Az utolsó diagram elkészülésének ideje
    Public DisableBalloon As Boolean = False                                ' A "Kis méret ikonként" mellett felbukkanú üzenet tiltása (csak először jelenik meg)
    Public OpenFile As Boolean = False                                      ' Fájl megnyitása buboréküzenetnél (csak, ha mentés történt, egyéb esetben nem)
    Public SavePath As String = Nothing                                     ' Az utolsó mentett fájl elérési útja
    Public Languages() As String = {"English (EN)", "Magyar (HU)"}          ' Nyelvek (egyelőre statikus, 2 elemű)
    Public MainWindowDone As Boolean = False                                ' A főablak betöltődésének indikátora, néhány szükséges lekérdezés csak ezután történhet meg!

    ' Forgalmi diagramok (2-vel több eleműnek kell lennie, mint a kijelzett érték!)
    Public TraffDownArray(TraffResolution + 2), TraffUpArray(TraffResolution + 2) As Int64

    ' *** FŐ ELJÁRÁS: Főablak betöltése (MyBase.Load -> MainWindow) ***
    ' Eseményvezérelt: Indítás
    Private Sub MainWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Alapértelmezett értékek beállítása (Ismeretlen érték vagy üres registryváltozó esetén)
        Dim DefaultLaguage As Int32 = 0         ' Nyelv: angol
        Dim DefaultRefresh As Int32 = 2         ' Frissítési időköz: 3 másodperc
        Dim DefaultHardware As Int32 = 0        ' Kiválasztott komponens: alaplap

        ' Debug sztring kiürítése
        Value_Debug.Text = Nothing

        ' Veriószám felbontása (Build-del)
        Dim VersionArray() As String = Split(MyVersion, ".")                ' Verziószámok elemeinek tömbje (Major, Minor, Sub, Build)
        ReDim Preserve VersionArray(0 To 3)

        ' Alverzió ellenőrzés: BETA (900+) és RC (500+) tagek ellenőrzése
        Dim SubVersion As Int32 = ToInt32(VersionArray(2))                  ' Alverzió

        ' Alverzió és kiadási állapot módosítása (Pl. 901 = 'vx.y.1 BETA', 502 = 'vx.y.1 RC')
        If SubVersion >= 900 And SubVersion < 1000 Then
            ReleaseStatus = "BETA"
            VersionArray(2) = (SubVersion Mod 900).ToString + " " + ReleaseStatus
        ElseIf SubVersion >= 500 Then
            ReleaseStatus = "RC"
            VersionArray(2) = (SubVersion Mod 500).ToString + " " + ReleaseStatus
        End If

        ' Verzió sztring beállítása
        VersionString = VersionArray(0) + "." + VersionArray(1) + "." + VersionArray(2) + " (Build " + VersionArray(3) + ")"

        ' *** WMI LEKÉRDEZÉS: Win32_OperatingSystem -> Operációs rendszer információk, Futási idő ***
        ' Megjegyzés: Ez több paraméternek is függősége, így a lekérdezést már most meg kell tenni!
        objOS = New ManagementObjectSearcher("SELECT Version FROM Win32_OperatingSystem")

        ' Értékek definiálása
        Dim OSVersionArray() As String = Nothing                            ' OS tagolt verziószám tömbje

        ' Értékek beállítása
        For Each Me.objMgmt In objOS.Get
            OSVersionArray = Split(objMgmt("Version"), ".")
        Next

        ' OS fő- és alverzió beállítása
        ReDim Preserve OSVersionArray(0 To 1)
        OSMajorVersion = OSVersionArray(0)
        OSMinorVersion = OSVersionArray(1)

        ' Lista állapotának beállítása (Komponensek -> első elem: alaplap)
        SelectedHardware = DefaultHardware

        ' ----- REGISTRY LEKÉRDEZÉSEK ÉS LISTÁK FELTÖLTÉSE -----

        ' *** REGISTRY LEKÉRDEZÉS: Regisztrációs kulcs létrehozása, ha nem létezik (HKCU\Software) ***
        If RegPath Is Nothing Then
            RegPath = Registry.CurrentUser.CreateSubKey("Software\\" + MyName, RegistryKeyPermissionCheck.ReadWriteSubTree)
        End If

        ' *** REGISTRY LEKÉRDEZÉS: Utolsó beállított értékek lekérdezése ***
        ' Megjegyzés: a beállított számérték sztringként tér vissza, ha nem létezik, akkor üres lesz!
        Dim ReadLanguage As String = RegPath.GetValue("SelectedLanguage")               ' Nyelv beállítása
        Dim ReadSplash As String = RegPath.GetValue("DisableLoadSplash")                ' Splash Screen megjelenítése
        Dim ReadRefresh As String = RegPath.GetValue("SelectedRefreshIndex")            ' Frissítési időköz
        Dim ReadDownChart As String = RegPath.GetValue("ChartVisibleDownload")          ' Letöltési diagram
        Dim ReadUpChart As String = RegPath.GetValue("ChartVisibleUpload")              ' Feltöltési diagram
        Dim ReadTopMost As String = RegPath.GetValue("EnableTopMost")                   ' Láthatóság
        Dim ReadMinToTray As String = RegPath.GetValue("MinimizeToTaskbar")             ' Kicsinyítés állapota
        Dim ReadNoQuitAsk As String = RegPath.GetValue("DisableExitConfirmation")       ' Kilépési megerősítés

        ' *** REGISTRY ELEMZÉS: Nyelv kiválasztása (SelectedLanguage) ***
        If ReadLanguage Is Nothing Or ReadLanguage > UBound(Languages) Then
            SelectedLanguage = DefaultLaguage
        Else
            SelectedLanguage = ToInt32(ReadLanguage)
        End If

        ' *** LISTAFELTÖLTÉS: Nyelv kiválasztása ***
        Dim LanguageList(UBound(Languages)) As String           ' Nyelvlista
        Dim LanguageCount As Int32                              ' Nyelv sorszáma

        ' Nyelvi lista feltöltése
        For LanguageCount = 0 To UBound(Languages)
            LanguageList(LanguageCount) = ComboBox_LanguageList.Items.Add(Languages(LanguageCount))
        Next

        ' Kiválasztott listaelem állapotának beállítása
        ComboBox_LanguageList.SelectedIndex = LanguageList(SelectedLanguage)

        ' *** REGISTRY ELEMZÉS: Splash Screen elrejtése indításkor (DisableLoadSplash) ***
        If ReadSplash Is Nothing Or ToInt32(ReadSplash) = 0 Then
            CheckedSplashDisable = False
        Else
            CheckedSplashDisable = True
        End If

        ' XP esetén Splash letiltása (Felülírja a registry beállítást, mivel XP-n hibásan jelenik meg!)
        If OSMajorVersion < 6 Then
            CheckedSplashDisable = True
            MainContextMenuItem_DisableSplash.Enabled = False
            MainMenu_SettingsItem_DisableSplash.Enabled = False
        End If

        ' Menüelem állapotának beállítása
        MainMenu_SettingsItem_DisableSplash.Checked = CheckedSplashDisable
        MainContextMenuItem_DisableSplash.Checked = CheckedSplashDisable

        ' Splash Screen betöltése és státusz frissítése
        If Not CheckedSplashDisable Then
            LoadSplash.Visible = True
            LoadSplash.Splash_Status.Text = Str_Loading + ": " + Str_LoadReg + "..."
        End If

        ' *** REGISTRY LEKÉRDEZÉS: Frissítési időköz ***
        If ReadRefresh Is Nothing Or ToInt32(ReadRefresh) > UBound(RefreshInterval) Then
            SelectedRefresh = DefaultRefresh
        Else
            SelectedRefresh = ToInt32(ReadRefresh)
        End If

        ' *** LISTAFELTÖLTÉS: Frissítési időköz ***
        Dim RefreshList(UBound(RefreshInterval)) As String      ' Frissítési lista tömbje
        Dim RefreshItems As Int32                               ' Tömbelemek sorszáma

        ' Frissítési értékeket tartalmazó lista feltöltése
        For RefreshItems = 0 To UBound(RefreshInterval)
            RefreshList(RefreshItems) = ComboBox_UpdateList.Items.Add(RefreshInterval(RefreshItems).ToString)
        Next

        ' Lista állapotának beállítása
        ComboBox_UpdateList.SelectedIndex = RefreshList(SelectedRefresh)
        TraffGenCounter = ComboBox_UpdateList.SelectedIndex

        ' *** LISTAFELTÖLTÉS: Processzorok ***
        UpdateCPUList()

        ' *** LISTAFELTÖLTÉS: Interfészek ***
        UpdateInterfaceList()

        ' *** LISTAFELTÖLTÉS: Videokártyák ***
        UpdateVideoList()

        ' *** REGISTRY ELEMZÉS: Letöltési diagram engedélyezése ***
        If ReadDownChart Is Nothing Or ToInt32(ReadDownChart) <> 0 Then
            CheckedDownChart = True
        Else
            CheckedDownChart = False
        End If

        ' Checkbox és menüelemek állapotának beállítása
        CheckBoxChart_DownloadVisible.Checked = CheckedDownChart
        MainMenu_ChartItem_DownloadVisible.Checked = CheckedDownChart

        ' *** REGISTRY ELEMZÉS: Feltöltési diagram engedélyezése ***
        If ReadUpChart Is Nothing Or ToInt32(ReadUpChart) <> 0 Then
            CheckedUpChart = True
        Else
            CheckedUpChart = False
        End If

        ' Checkbox és menüelemek állapotának beállítása
        MainMenu_ChartItem_UploadVisible.Checked = CheckedUpChart
        CheckBoxChart_UploadVisible.Checked = CheckedUpChart

        ' *** REGISTRY ELEMZÉS: Állandó láthatóság ellenőrzése ***
        If ReadTopMost Is Nothing Or ToInt32(ReadTopMost) = 0 Then
            CheckedTopMost = False
            StatusLabel_TopMost.Image = My.Resources.Resources.Control_RedPin
        Else
            CheckedTopMost = True
            StatusLabel_TopMost.Image = My.Resources.Resources.Control_GreenPin
        End If

        ' Ablak láthatóságának beállítása
        Me.TopMost = CheckedTopMost

        ' Menüelemek állapotának beállítása
        MainMenu_SettingsItem_TopMost.Checked = CheckedTopMost
        MainContextMenuItem_TopMost.Checked = CheckedTopMost

        ' *** REGISTRY ELEMZÉS: Kicsinyítés a rendszerikonok közé ***
        If ReadMinToTray Is Nothing Or ToInt32(ReadMinToTray) = 0 Then
            CheckedMinToTray = False
        Else
            CheckedMinToTray = True
        End If

        ' Menüelemek állapotának beállítása
        MainMenu_SettingsItem_TaskbarMinimize.Checked = CheckedMinToTray
        MainContextMenuItem_TaskbarMinimize.Checked = CheckedMinToTray

        ' *** REGISTRY ELEMZÉS: Kilépési megerősítés kiírásának tiltása ***
        If ReadNoQuitAsk Is Nothing Or ToInt32(ReadNoQuitAsk) <> 1 Then
            CheckedNoQuitAsk = False
        Else
            CheckedNoQuitAsk = True
        End If

        ' Menüelem állapotának beállítása
        MainMenu_SettingsItem_DisableConfirm.Checked = CheckedNoQuitAsk
        MainContextMenuItem_DisableConfirm.Checked = CheckedNoQuitAsk

        ' ----- WMI LEKÉRDEZÉSEK -----

        ' Splash Screen státusz frissítése
        If Not CheckedSplashDisable Then
            LoadSplash.Splash_Status.Text = Str_Loading + ": " + Str_LoadWMI + "..."
        End If

        ' Hardverinformációk lekérdezése
        SelectedHardware = DefaultHardware
        SetHWInformation()
        ComboBox_HWList.SelectedIndex = SelectedHardware

        ' *** WMI LEKÉRDEZÉS: Win32_ComputerSystem" -> Hosztnév ***
        objCS = New ManagementObjectSearcher("SELECT Name FROM Win32_ComputerSystem")

        ' Értékek beállítása
        For Each Me.objMgmt In objCS.Get
            Hostname = objMgmt("Name").ToString
        Next

        ' Hosztnév beálítása az állapotsorban
        StatusLabel_Host.Text = Str_Hostname + ": " + Hostname

        ' *** WMI LEKÉRDEZÉS: Win32_OperatingSystem -> Operációs rendszer információk, Futási idő ***
        objOS = New ManagementObjectSearcher("SELECT Caption, BuildNumber, LocalDateTime, LastBootUptime FROM Win32_OperatingSystem")

        ' Értékek definiálása
        Dim OSName As String = Nothing                          ' OS neve
        Dim OSBuild As String = Nothing                         ' OS buildszáma
        Dim CurrentTime As String = Nothing                     ' Jelenlegi idő
        Dim SysUpTime As String = Nothing                       ' Indítás ideje

        ' Értékek beállítása
        For Each Me.objMgmt In objOS.Get
            OSName = objMgmt("Caption").ToString
            OSBuild = objMgmt("BuildNumber").ToString
            CurrentTime = objMgmt("LocalDateTime").ToString
            SysUpTime = objMgmt("LastBootUptime").ToString
        Next

        ' Kiírások értékének frissítése
        Value_OSName.Text = StringNormalize(OSName)
        Value_OSRelease.Text = OSRelase.ToString + "-bit"
        Value_OSVersion.Text = OSMajorVersion.ToString + "." + OSMinorVersion.ToString
        Value_OSBuild.Text = OSBuild.ToString

        ' *** KEZDŐÉRTÉK BEÁLLÍTÁS: Futásidő ***
        UptimeSeconds = DateDiff("s", DateTimeConv(SysUpTime), DateTimeConv(CurrentTime))
        SetUptime(UptimeSeconds, TimerSeconds)

        ' *** KEZDŐÉRTÉK BEÁLLÍTÁS: Memória információk ***
        SetMemoryInformation()

        ' *** LISTAFELTÖLTÉS: Lemez információk ***
        ' Megjegyzés: A lista feltöltése késleltetve van, mert az OS verzió függősége az egyes értékeknek!
        UpdateDiskList()

        ' ----- ZÁRÓ MŰVELETEK -----

        ' Időzítő indítása: EventTimer (1 másodperc)
        EventTimer.Enabled = True
        EventTimer.Interval = 1000

        ' Diagram frissítése (gyakorlatilag ez a kezdő reset, mert az óra már ketyeg, de betöltés előtt nem mér!)
        MakeChart(True)

        ' Splash ablak bezárása
        If Not CheckedSplashDisable Then
            LoadSplash.Close()
        End If

        ' Bezárás link megjelenítésének engedélyezése a Splash ablakon (Névjegy)
        SplashDefineAsAbout = True

        ' Főablak megjelenítve
        MainWindowDone = True

    End Sub

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVEÉNY: Hardver komponensek értékeinek beállítása ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function SetHWInformation()

        ' Értékek definiálása
        Dim Vendor As String = Nothing                      ' Gyártó
        Dim Model As String = Nothing                       ' Modell
        Dim Identifier As String = Nothing                  ' Azonosító

        ' *** WMI LEKÉRDEZÉS: Win32_Baseboard -> Alaplap információi ***
        objBB = New ManagementObjectSearcher("SELECT Manufacturer, Product, SerialNumber FROM Win32_Baseboard")

        ' Értékek beállítása
        For Each Me.objMgmt In objBB.Get
            Vendor = objMgmt("Manufacturer").ToString
            Model = objMgmt("Product").ToString
            Identifier = objMgmt("SerialNumber").ToString
        Next

        ' Értéktároló tömb frissítése -> Alaplap
        If Vendor = Nothing Or Vendor = "To be filled by O.E.M." Then
            HWVendor(0) = Nothing
        Else
            HWVendor(0) = Vendor
        End If

        If Model = Nothing Or Model = "To be filled by O.E.M." Then
            HWIdentifier(0) = Nothing
        ElseIf Identifier = Nothing Or Identifier = "To be filled by O.E.M." Then
            HWIdentifier(0) = Model
        Else
            HWIdentifier(0) = Model + ", S/N: " + Identifier
        End If

        ' *** WMI LEKÉRDEZÉS: Win32_ComputerSystem -> Számítógép információi ***
        objCS = New ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem")

        ' Értékek beállítása
        For Each Me.objMgmt In objCS.Get
            Vendor = objMgmt("Manufacturer").ToString
            Model = objMgmt("Model").ToString
        Next

        ' Értéktároló tömb frissítése -> Számítógép
        If Vendor = Nothing Or Vendor = "To be filled by O.E.M." Or Vendor = "System manufacturer" Then
            HWVendor(1) = Nothing
        Else
            HWVendor(1) = Vendor
        End If

        If Model = Nothing Or Model = "To be filled by O.E.M." Or Model = "System Product Name" Then
            HWIdentifier(1) = Nothing
        Else
            HWIdentifier(1) = Model
        End If

        ' *** WMI LEKÉRDEZÉS: Win32_BIOS -> BIOS információi ***
        objBS = New ManagementObjectSearcher("SELECT Manufacturer, SMBIOSBIOSVersion, ReleaseDate FROM Win32_BIOS")

        ' Értékek beállítása
        For Each Me.objMgmt In objBS.Get
            Vendor = objMgmt("Manufacturer").ToString
            Model = objMgmt("SMBIOSBIOSVersion").ToString
            Identifier = Format(DateTimeConv(objMgmt("ReleaseDate")), "yyyy-MM-dd").ToString
        Next

        ' Értéktároló tömb frissítése -> BIOS
        If Vendor = Nothing Then
            HWVendor(2) = Nothing
        Else
            HWVendor(2) = Vendor
        End If

        If Model = Nothing Then
            HWIdentifier(2) = Nothing
        Else
            HWIdentifier(2) = Model + " (" + Identifier + ")"
        End If

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVEÉNY: Processzor aktuális órajelének beállítása ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function SetCPUInformation()

        ' Értékek definiálása
        Dim CPUCoreNumber As Int32                              ' Magok száma
        Dim CPUThreadNumber As Int32                            ' Logikai szálak száma
        Dim CPUCurrentClock As Int32                            ' Jelenlegi CPU órajel
        Dim CPUMaximumClock As Int32                            ' Natív CPU órajel
        Dim CPUCount As Int32 = 0                               ' Processzor sorszáma

        ' WMI érték definiálása
        objPR = New ManagementObjectSearcher("SELECT NumberOfCores, NumberOfLogicalProcessors, CurrentClockSpeed, MaxClockSpeed FROM Win32_Processor")

        ' Értékek kinyerése a WMI-ből
        For Each Me.objMgmt In objPR.Get
            If CPUCount = SelectedCPU Then
                CPUCoreNumber = objMgmt("NumberOfCores")
                CPUThreadNumber = objMgmt("NumberOfLogicalProcessors")
                CPUCurrentClock = objMgmt("CurrentClockSpeed")
                CPUMaximumClock = objMgmt("MaxClockSpeed")
            End If
            CPUCount += 1
        Next

        ' Kiírások frissítése
        Value_CPUCore.Text = CPUCoreNumber.ToString + " / " + CPUThreadNumber.ToString
        Value_CPUClock.Text = CPUCurrentClock.ToString + " MHz"
        Value_CPUMaxClock.Text = CPUMaximumClock.ToString + " MHz"

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Memória információk beállítása ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function SetMemoryInformation()

        ' Értékek definiálása
        Dim PMemSize As String = Nothing ' Fizikai memória mérete
        Dim PMemFree As String = Nothing ' Szabad fizikai memória
        Dim PMemPerc As String = Nothing ' Fizikai memória kihasználtsága
        Dim VMemSize As String = Nothing ' Virtuális memória mérete
        Dim VMemFree As String = Nothing ' Szabad virtuális memória
        Dim VMemPerc As String = Nothing ' Virtuális memória kihasználtsága

        ' WMI érték definiálása
        objOS = New ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory, TotalVirtualMemorySize, FreeVirtualMemory FROM Win32_OperatingSystem")

        ' Értékek kinyerése a WMI-ből
        For Each Me.objMgmt In objOS.Get
            PMemSize = objMgmt("TotalVisibleMemorySize")
            PMemFree = objMgmt("FreePhysicalMemory")
            PMemPerc = Round(((PMemSize - PMemFree) / PMemSize) * 100)
            VMemSize = objMgmt("TotalVirtualMemorySize")
            VMemFree = objMgmt("FreeVirtualMemory")
            VMemPerc = Round(((VMemSize - VMemFree) / VMemSize) * 100)
        Next

        ' Hibakorrekció: Fizikai memória (100%-nál nagyobb kihasználtság. Nem fordulhat elő, de ha mégis, akkor 100-ra betonozva a plafon!)
        If PMemPerc > 100 Then
            PMemPerc = 100
        End If

        ' Hibakorrekció: Virtuális memória (Szintúgy...)
        If VMemPerc > 100 Then
            VMemPerc = 100
        End If

        ' Memóriaértékek formázása
        Dim PMemSizeConv(2), PMemFreeConv(2), VMemSizeConv(2), VMemFreeConv(2) As Double
        PMemSizeConv = DynByteConv(PMemSize * 1024, 2)
        PMemFreeConv = DynByteConv(PMemFree * 1024, 2)
        VMemSizeConv = DynByteConv(VMemSize * 1024, 2)
        VMemFreeConv = DynByteConv(VMemFree * 1024, 2)

        ' Kiírások formázása
        Value_PhysicalMemorySize.Text = FixDigitSeparator(PMemSizeConv(0), 2, True) + " " + PrefixTable(PMemSizeConv(1)) + "B"
        Value_PhysicalMemoryFree.Text = FixDigitSeparator(PMemFreeConv(0), 2, True).ToString + " " + PrefixTable(PMemFreeConv(1)) + "B"
        Value_PhysicalMemoryUsage.Text = PMemPerc.ToString + " %"
        Value_VirtualMemorySize.Text = FixDigitSeparator(VMemSizeConv(0), 2, True) + " " + PrefixTable(VMemSizeConv(1)) + "B"
        Value_VirtualMemoryFree.Text = FixDigitSeparator(VMemFreeConv(0), 2, True) + " " + PrefixTable(VMemFreeConv(1)) + "B"
        Value_VirtualMemoryUsage.Text = VMemPerc.ToString + " %"

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Lemezmeghajtó információk beállítása ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function SetDiskInformation()

        ' Értékek definiálása
        Dim DiskIndex As Int32 = 0                          ' Lemez sorszáma
        Dim Capacity As Double = 0                          ' Lemez kapacitása
        Dim FormattedCapacity(2) As Double                  ' Formázott kapacitás
        Dim Connector As String = Nothing                   ' Csatolófelület
        Dim SectorSize As Int32 = 0                         ' Szektorméret
        Dim Firmware As String = Nothing                    ' Firmware revízió
        Dim SmartInfo As String = Nothing                   ' S.M.A.R.T állapot
        Dim SerialNumber As String = Nothing                ' Sorozatszám

        ' WMI érték definiálása
        objDD = New ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE Index = '" + DiskList(SelectedDisk) + "'")

        ' Értékek kinyerése a WMI-ből
        For Each Me.objMgmt In objDD.Get
            Capacity = objMgmt("Size")
            Connector = objMgmt("InterfaceType")
            SectorSize = objMgmt("BytesPerSector")
            DiskIndex = objMgmt("Index")
            SmartInfo = objMgmt("Status")

            ' Windows XP alatt nem támogatott néhány érték lekérdezése
            If OSMajorVersion >= 6 Then
                SerialNumber = objMgmt("SerialNumber")
                Firmware = objMgmt("FirmwareRevision")
            Else
                SerialNumber = Nothing
                Firmware = Nothing
            End If
        Next

        ' OS Verzióellenőrzés: XP -> Ismeretlen, Vista és 7 -> Konverzió indítása
        If OSMajorVersion < 6 Then
            SerialNumber = Nothing
        ElseIf OSMajorVersion = 6 And OSMinorVersion <= 1 And SerialNumber <> Nothing Then
            SerialNumber = DiskSerialNumberConv(SerialNumber)
        End If

        ' Üres sorozatszám korrekció
        If SerialNumber = Nothing Then
            SerialNumber = "(" + Str_Unknown + ")"
        Else
            ' Felesleges szóközök eltávolítása
            While (InStr(SerialNumber, " "))
                SerialNumber = Replace(SerialNumber, " ", "")
            End While
        End If

        ' Üres firmware korrekció
        If Firmware = Nothing Then
            Firmware = "(" + Str_Unknown + ")"
        Else
            ' Felesleges szóközök eltávolítása
            While (InStr(Firmware, " "))
                Firmware = Replace(Firmware, " ", "")
            End While
        End If

        ' Kapacitás konvertálása 
        FormattedCapacity = DynByteConv(Capacity, 2)

        ' Nulla bájtos lemezméret korrekció -> Nincs lemez!
        If Capacity = 0 Then
            Value_DiskCapacity.Text = "(" + Str_NoDisk + ")"
        Else
            Value_DiskCapacity.Text = FixDigitSeparator(FormattedCapacity(0), 2, True) + " " + PrefixTable(FormattedCapacity(1)) + "B"
        End If

        ' Kiírások formázása
        Value_DiskIndex.Text = "# " + DiskIndex.ToString
        Value_DiskFirmware.Text = Firmware.ToString
        Value_DiskSmart.Text = SmartInfo.ToString
        Value_DiskSerial.Text = SerialNumber.ToString

        If Connector = "IDE" Then
            Value_DiskInterface.Text = "IDE / SATA"
        ElseIf Connector = "SCSI" Then
            Value_DiskInterface.Text = "SCSI / SAS"
        Else
            Value_DiskInterface.Text = Connector
        End If

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Videokártya információk beállítása ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function SetVideoInformation()

        ' WMI LEKÉRDEZÉS: Win32_VideoController -> Elsődleges videóvezérlő
        objVC = New ManagementObjectSearcher("SELECT * FROM Win32_VideoController")

        ' Értékek definiálása
        Dim VideoMemory As Int64                                ' Video memória
        Dim VideoResolution(3) As Int32                         ' Képernyőfelbontás
        Dim VideoCount As Int32 = 0                             ' Videokártya sorszáma

        ' Értékek beállítása
        For Each Me.objMgmt In objVC.Get
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
            Value_VideoMemory.Text = "(" + Str_Unknown + ")"
        Else
            ' Memóriaérték formázása
            Dim VideoMemoryConv(2) As Double
            VideoMemoryConv = DynByteConv(VideoMemory, 2)

            ' Kiírás értékének frissítése
            Value_VideoMemory.Text = FixDigitSeparator(VideoMemoryConv(0), 2, True) + " " + PrefixTable(VideoMemoryConv(1)) + "B"

            ' Ismeretlen felbontás (Pl.: 0 x 0, 0 bit)
            If VideoResolution(0) = 0 Or VideoResolution(1) = 0 Or VideoResolution(2) = 0 Then
                Value_VideoResolution.Text = Str_Inactive
            Else
                Value_VideoResolution.Text = VideoResolution(0).ToString + " x " + VideoResolution(1).ToString + " (" + VideoResolution(2).ToString + " bit)"
            End If
        End If

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Rendszer Futási idő beállítása ***
    ' Bemenet: UptimeSeconds -> Indítás óta eltelt másodpercek (Int32)
    '          TimerCount    -> Számláló állása (Int32)
    ' Kimenet: Boolean (False)
    Private Function SetUptime(ByVal UptimeSeconds As Int32, ByVal TimerCount As Int32)

        ' Értékek definiálása
        Dim Days, Hours, Minutes, Seconds As Int32 ' Időváltozók (nap, óra, perc, másodperc)
        Dim UptimeString As String = Nothing       ' Futásidő sztring

        ' Futásidő érték növelése (másodperc)
        UptimeSeconds += TimerCount

        ' Egységekre bontás
        Days = Int(UptimeSeconds / (24 * 3600))
        UptimeSeconds = UptimeSeconds - (Days * 24 * 3600)
        Hours = Int(UptimeSeconds / 3600)
        UptimeSeconds = UptimeSeconds - (Hours * 3600)
        Minutes = Int(UptimeSeconds / 60)
        UptimeSeconds = UptimeSeconds - (Minutes * 60)
        Seconds = Int(UptimeSeconds)

        ' Kiírási sztring beállítása
        UptimeString = Str_Uptime + ": " + Days.ToString + " " + Str_Days + ", " + Hours.ToString + " " + Str_Hours + ", " +
                       Minutes.ToString + " " + Str_Mins + " " + Str_And + " " + Seconds.ToString + " " + Str_Secs + "."

        ' Kiírás frissítése
        StatusLabel_Uptime.Text = UptimeString

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Karakterek javítása ***
    ' Bemenet: RawString -> Formázandó sztring (String)
    ' Kimenet: RawString -> Formázott sztring (String)
    Private Function StringNormalize(ByVal RawString As String)

        ' Értékek definiálása
        Dim str2char() As Char          ' Sztring-karakter konverzió tömbje
        Dim strTemp As String = Nothing ' Ideiglenes szting elemz

        ' Karaktertömbre bontás
        str2char = RawString.ToCharArray()

        ' Dupla szóközök eltávolítása (meg kell, hogy előzze a kezdőszóköz eltávolítását)
        While (InStr(RawString, "  "))
            RawString = Replace(RawString, "  ", " ")
        End While

        ' Kezdőszóköz eltávolítása
        If str2char(0) = " " Then
            For i As Int32 = 1 To UBound(str2char)
                strTemp += str2char(i)
            Next
            RawString = strTemp
        End If

        ' Szögletes zárójelek átalakítása
        While (InStr(RawString, "["))
            RawString = Replace(RawString, "[", "(")
        End While
        While (InStr(RawString, "]"))
            RawString = Replace(RawString, "]", ")")
        End While

        ' Packet Scheduler Miniport kiírás eltávolítása
        While (InStr(RawString, " - Packet Scheduler Miniport"))
            RawString = Replace(RawString, " - Packet Scheduler Miniport", "")
        End While

        ' FE/GbE (*/100*, */1000*) javítása
        While (InStr(RawString, "_100"))
            RawString = Replace(RawString, "_100", "/100")
        End While

        ' Atheros (*/AR*) nevek javítása - Felsorolás
        While (InStr(RawString, "_AR"))
            RawString = Replace(RawString, "_AR", "/AR")
        End While

        ' Intel (*/Wireless*, *PRO/*) nevek javítása - Mindenhol
        While (InStr(RawString, "_Wireless"))
            RawString = Replace(RawString, "_Wireless", "/Wireless")
        End While
        While (InStr(RawString, "PRO_"))
            RawString = Replace(RawString, "PRO_", "PRO/")
        End While

        ' Realtek (*/RTL*) nevek javítása - Felsorolás
        While (InStr(RawString, "_RTL"))
            RawString = Replace(RawString, "_RTL", "/RTL")
        End While

        ' Alsóvonások eltávolítása (főleg az adapter sorszámánál mutat rosszul)
        While (InStr(RawString, "_"))
            RawString = Replace(RawString, "_", "")
        End While

        ' Visszatérési érték beállítása
        Return RawString

    End Function

    ' *** FÜGGVÉNY: WMI alapú DateTime változó konverzió ***
    ' Bemenet: WMIDateTime -> WMI datetime formátumú változó (String)
    ' Kimenet: Converted   -> Hagyományos DateTime változó (DateTime)
    Private Function DateTimeConv(ByVal WMIDateTime As String)

        ' Értékek definiálása
        Dim Converted As DateTime                           ' DateTime változó
        Dim Year, Month, Day, Hour, Minute, Second As Int32 ' Időváltozók (év, hónap, nap, óra, perc, másodperc)

        ' Értékek részekre bontása
        Year = Mid(WMIDateTime, 1, 4)
        Month = Mid(WMIDateTime, 5, 2)
        Day = Mid(WMIDateTime, 7, 2)
        Hour = Mid(WMIDateTime, 9, 2)
        Minute = Mid(WMIDateTime, 11, 2)
        Second = Mid(WMIDateTime, 13, 2)

        ' Kimenet beállítása
        Converted = New DateTime(Year, Month, Day, Hour, Minute, Second)

        ' Visszatérési érték beállítása
        Return Converted

    End Function

    ' *** FÜGGVÉNY: Nyelvi dátum és időformátum konverzió ***
    ' Bemenet: InputDate -> Dátum és idő (DateTime)
    ' Kimenet: Converted -> Formázott sztring (String)
    Private Function GetLocalizedDate(ByVal InputDate As DateTime)

        ' Értékek definiálása
        Dim Converted As String = Str_DateFormat
        Dim ConvCount As Int32 = 0
        Dim SourceFormat() = {"yyyy", "MMMM", "dddd", "dd", "d", "H:mm:ss", "h:mm:ss", "tt"}
        Dim TargetFormat() = {Format(InputDate, "yyyy"), "###", "##", Format(InputDate, "dd"), Format(InputDate, "d"),
                              Format(InputDate, "H:mm:ss"), Format(InputDate, "h:mm:ss"), "#"}

        ' Ismert formátumok lecserélése valódi értékekre
        For i As Int32 = 0 To UBound(SourceFormat)
            While (InStr(Converted, SourceFormat(ConvCount)))
                Converted = Replace(Converted, SourceFormat(ConvCount), TargetFormat(ConvCount))
            End While
            ConvCount += 1
        Next

        ' Védett szöveges formátum cseréje: Hónap neve
        While (InStr(Converted, "###"))
            Converted = Replace(Converted, "###", Str_MonthName(InputDate.Month - 1))
        End While

        ' Védett szöveges formátum cseréje: Nap neve
        While (InStr(Converted, "##"))
            Converted = Replace(Converted, "##", Str_DayName(InputDate.DayOfWeek))
        End While

        ' Védett szöveges formátum cseréje: Napszak (AM/PM)
        While (InStr(Converted, "#"))
            Converted = Replace(Converted, "#", InputDate.ToString("tt", System.Globalization.CultureInfo.InvariantCulture))
        End While

        ' Visszatérési érték beállítása
        Return Converted

    End Function

    ' *** FÜGGVÉNY: Statikus bájtkonverzió ***
    ' Bemenet: Value -> bájt (Double)
    ' Kimenet: Formázott érték (String)
    Private Function StatByteConv(ByVal Value As Double, ByVal Prefix As Int32)

        ' Bemeneti érték formázása
        Value = Round(Value / (1024 ^ Prefix))

        ' Visszatérési érték beállítása
        Return Value.ToString + " " + PrefixTable(Prefix)

    End Function

    ' *** FÜGGVÉNY: Dinamikus bájtkonverzió ***
    ' Bemenet: Value -> bájt (Double)
    '          Digit -> Elválasztó utáni helyiértékek száma (Int32)
    ' Kimenet: ConvValue -> Formázott érték tömbje (Double): Kerekített érték, Prefixum sorszáma
    Private Function DynByteConv(ByVal Value As Double, ByVal Digit As Int32)

        ' Értékek definiálása
        Dim Prefix, ConvValue(2) As Double

        ' Prefixum görgetés indítása (Minden hurok eggyel feljebb tolja a tömbben lévő szorzó számát)
        Prefix = 0
        While (Value >= 1024)
            Prefix += 1
            Value = Round((Value / (1024)) * 10 ^ Digit) / (10 ^ Digit) ' A Microsoft nem SI alapján számol a bájtnál! (Valójában ez iB, kiB, MiB, stb. lenne.)
        End While

        ' Kimenet formázása
        ConvValue(0) = Round(Value, 2)
        ConvValue(1) = Prefix

        ' Visszatérési érték beállítása
        Return ConvValue

    End Function

    ' *** FÜGGVÉNY: Dinamikus bitkonverzió ***
    ' Bemenet: Value -> bit (Double)
    '          Digit -> Elválasztó utáni helyiértékek száma (Int32)
    ' Kimenet: ConvValue -> Formázott érték tömbje (Double): Kerekített érték, Prefixum sorszáma
    Private Function DynBitConv(ByVal Value As Double, ByVal Digit As Int32)

        ' Értékek definiálása
        Dim Prefix, ConvValue(2) As Double

        ' Prefixum görgetés indítása (Minden hurok eggyel feljebb tolja a tömbben lévő szorzó számát)
        Prefix = 0
        While (Value >= 1000)
            Prefix += 1
            Value = Round((Value / (1000)) * 10 ^ Digit) / (10 ^ Digit) ' A Microsoft SI alapján számol a bitnél!
        End While

        ' Kimenet formázása
        ConvValue(0) = Round(Value, 2)
        ConvValue(1) = Prefix

        ' Visszatérési érték beállítása
        Return ConvValue

    End Function

    ' *** FÜGGVÉNY: Tizedes elválaszó javítása (területi beállítás felülbírálása) ***
    ' Bemenet: Value      -> Módosítatlan tizedestört érték (Double)
    '          Digit      -> Elválasztó utáni helyiértékek száma (Int32)
    '          FloatFract -> Lebegő (True) vagy statikus (False) törtformátum (Boolean)
    ' Kimenet: ConvString -> Formázott tizedestört (String)
    Private Function FixDigitSeparator(ByVal Value As Double, ByVal Digit As Int32, ByVal FloatFract As Boolean)

        ' Értékek definiálása
        Dim IntString, ConvString As String
        Dim ConvFract As Double
        Dim FractString As String = Nothing

        ' Értékek számítása
        IntString = Fix(Value).ToString

        ' Tizedes elválasztó ellenőrzése -> Ha nincs helyiérték, akkor nem kell!
        If Digit <> 0 Then
            FractString = Str_DigitSeparator
        End If

        ' Törtrész generálása -> Lebegő (pl.: 0,1 -> 0,1) vagy statikus tört (pl. 0,1 -> 0,10)
        If FloatFract Then

            ' Tört kerekítése és meghagyása törtnek
            ConvFract = Round(Value - Fix(Value), Digit)

            ' Tizedesrész generálása - Lebegő, végződő 0-k nélkül (Ha a string 3-nál rövidebb, akkor egész szám, ilyenkor a törtrész üres, és nincs tizedes elválasztó sem!)
            If ConvFract.ToString.Length < 3 Then
                FractString = Nothing
            Else
                FractString = FractString + ConvFract.ToString.Substring(2, ConvFract.ToString.Length - 2)
            End If

        Else

            ' Törtrész konvetálása egésszé, valamint az elejére egy 1-es számjegy
            ConvFract = (Round(Value - Fix(Value), Digit) * 10 ^ Digit) + 10 ^ Digit

            ' Tizedesrész generálása - Statikus, a végződő 0-k is kiírva maradnak (Az elejéről csak az egyest kell eltüntetni!)
            FractString = FractString + ConvFract.ToString.Substring(1, ConvFract.ToString.Length - 1)

        End If

        ' Kimenet formázása
        ConvString = IntString + FractString

        ' Visszatérési érték beállítása
        Return ConvString

    End Function

    ' *** FÜGGVÉNY: Lemez sorozatszám korrekció (Windows 7) ***
    ' Bemenet: Value -> Sorozatszám (String)
    ' Kimenet: ConvValue -> Formázott érték (String)
    Private Function DiskSerialNumberConv(ByVal Value As String)

        ' Értékek definiálása
        Dim HexArr() As Char = Value.ToCharArray
        Dim TempHex As String
        Dim ConvValue As String = Nothing
        Dim TempChar As Int32
        Dim CharNum As Int32 = UBound(HexArr) + 1
        Dim CharArr((CharNum / 2) - 1) As String

        ' Érvénytelen sorozatszám ellenőrzés (Ha nem pont 40 karakter, akkor konvertálás nélkül írja ki!)
        If Value.Length <> 40 Then
            ConvValue = Value
        Else
            ' Konverzió
            For Position As Int32 = 0 To (CharNum / 2) - 1
                TempHex = HexArr(Position * 2) + HexArr((Position * 2) + 1)
                TempChar = Convert.ToInt32(TempHex, 16)

                ' Bájtsorrend javítása (Páros-páratlan csere)
                If Position Mod 2 <> 0 Then
                    CharArr(Position - 1) = Chr(TempChar).ToString
                Else
                    CharArr(Position + 1) = Chr(TempChar).ToString
                End If
            Next

            ' Konvertált sztring összefűzése
            For i As Int32 = 0 To UBound(CharArr)
                ConvValue = ConvValue + CharArr(i)
            Next

        End If

        ' Visszatérési érték beállítása
        Return ConvValue

    End Function

    ' *** FÜGGVÉNY: Sebesség értékek frissítése a statisztikához (folyamatos) ***
    ' Bemenet: TraffReset -> Forgalom nullázása (Boolean)
    ' Kimenet: Boolean (False)
    Private Function UpdateSpeedStatistics(ByVal TraffReset As Boolean)

        ' Értékek definiálása
        Dim MaxBandwidth(2) As Double                           ' Maximális sávszélesség (érték, prefixum sorszáma)
        Dim MaxRelativeSpeed As Double                          ' Legnagyobb felvehető sebesség érték
        Dim CurrentDownload, CurrentUpload As Int64             ' Jelenlegi le- és feltöltött bájtok száma
        Dim DownloadSpeed(2), UploadSpeed(2) As Double          ' Jelenlegi sebesség értékek (2 dimenziós: érték, prefixum sorszáma)
        Dim CurrentUsage As Int32                               ' Jelenlegi kihasználtság
        Dim UsageValue As Double                                ' Kihasználtsági érték

        ' WMI érték definiálása
        objNI = New ManagementObjectSearcher("SELECT CurrentBandwidth, BytesReceivedPersec, BytesSentPersec FROM Win32_PerfRawData_Tcpip_NetworkInterface WHERE Name = '" + InterfaceList(SelectedInterface) + "'")

        ' Lekérdezett értékek feldolgozása
        For Each Me.objMgmt In objNI.Get

            ' Maximálisan felvehető sebességérték (Sávszélesség / 8)
            If objMgmt("CurrentBandwidth") <> 0 Then
                MaxRelativeSpeed = objMgmt("CurrentBandwidth") / 8
            Else
                MaxRelativeSpeed = 1 ' Nullával való osztás elkerülésére
            End If

            ' Aktuális érték az első helyre (UInt64 korrekció) -> Letöltés (NT6.0-tól, XP-nél még uint32 volt!)
            If objMgmt("BytesReceivedPersec") > (2 ^ 63 - 1) Then
                CurrentDownload = objMgmt("BytesReceivedPersec") - (2 ^ 63)
            Else
                CurrentDownload = objMgmt("BytesReceivedPersec")
            End If

            ' Aktuális érték az első helyre (UInt64 korrekció) -> Feltöltés (NT6.0-tól, XP-nél még uint32 volt!)
            If objMgmt("BytesSentPersec") > (2 ^ 63 - 1) Then
                CurrentUpload = objMgmt("BytesSentPersec") - (2 ^ 63)
            Else
                CurrentUpload = objMgmt("BytesSentPersec")
            End If
        Next

        ' Sávszélesség kiírás formázása
        MaxBandwidth = DynBitConv(objMgmt("CurrentBandwidth"), 2)
        Value_Bandwidth.Text = FixDigitSeparator(MaxBandwidth(0), 2, True)
        Value_BandwidthUnit.Text = PrefixTable(MaxBandwidth(1)) + "bps"

        ' Forgalomtörlés ellenőrzése
        If TraffReset Then

            ' Forgalomtörlés esetére hibakorrekció
            LatestDownload = CurrentDownload
            LatestUpload = CurrentUpload

            ' Interfész kihasználtság nullázása
            CurrentUsage = 0

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
            CurrentUsage = Round(Abs(UsageValue / (MaxRelativeSpeed) * 100))

        End If

        ' Sebesség értékek kiszámítása és konvertálása
        DownloadSpeed = DynByteConv(Abs(CurrentDownload - LatestDownload), 2)
        UploadSpeed = DynByteConv(Abs(CurrentUpload - LatestUpload), 2)

        ' Kiírási értékek frissítése
        Value_DownloadSpeed.Text = FixDigitSeparator(DownloadSpeed(0), 2, False)
        Value_DownloadSpeedUnit.Text = PrefixTable(DownloadSpeed(1)) + "B/s"
        Value_UploadSpeed.Text = FixDigitSeparator(UploadSpeed(0), 2, False)
        Value_UploadSpeedUnit.Text = PrefixTable(UploadSpeed(1)) + "B/s"

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
    ' Bemenet: TraffReset -> Forgalom nullázása (Boolean)
    ' Kimenet: Boolean (False)
    Private Function UpdateTraffArray(ByVal TraffReset As Boolean)

        ' Értékek definiálása
        Dim Download(UBound(TraffDownArray)) As Int64   ' Letöltött bájtok tömbje
        Dim Upload(UBound(TraffUpArray)) As Int64       ' Feltöltött bájtok tömbje

        ' WMI érték definiálása
        objNI = New ManagementObjectSearcher("SELECT * FROM Win32_PerfRawData_Tcpip_NetworkInterface WHERE Name = '" + InterfaceList(SelectedInterface) + "'")

        ' Lekérdezett értékek feldolgozása
        For Each Me.objMgmt In objNI.Get

            ' Aktuális letöltési érték az első helyre (UInt64 korrekció -> NT6.0-tól, XP-nél még UInt32 volt!)
            If objMgmt("BytesReceivedPersec") >= 2 ^ 63 Then
                Download(0) = objMgmt("BytesReceivedPersec") - (2 ^ 63)
            Else
                Download(0) = objMgmt("BytesReceivedPersec")
            End If

            ' Aktuális feltöltési érték az első helyre (UInt64 korrekció -> NT6.0-tól, XP-nél még UInt32 volt!)
            If objMgmt("BytesSentPersec") >= 2 ^ 63 Then
                Upload(0) = objMgmt("BytesSentPersec") - (2 ^ 63)
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

    ' *** FÜGGVÉNY: Processzor lista újratöltése ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function UpdateCPUList()

        ' Lista kiürítése
        ComboBox_CPUList.Items.Clear()

        ' WMI lekérdezés: Win32_Processor -> Processzor információk
        objPR = New ManagementObjectSearcher("SELECT Name, AddressWidth FROM Win32_Processor")

        ' Értékek definiálása
        Dim CPUCount As Int32 = 0                               ' Processzor sorszáma
        Dim CPUString As String = Nothing                       ' Processzor neve
        Dim CPUAddressWidth As Int32 = 0                        ' Processzor címbusz szélessége

        ' Értékek beállítása
        For Each Me.objMgmt In objPR.Get
            CPUString = objMgmt("Name")
            CPUAddressWidth = ToInt32(objMgmt("AddressWidth"))
            CPUName(CPUCount) = ComboBox_CPUList.Items.Add("CPU #" + CPUCount.ToString + " - " + StringNormalize(CPUString))
            CPUCount += 1
        Next

        ' Alapértelmezett kiválasztás visszaállítása (Legelső lemez!)
        SelectedCPU = 0
        ComboBox_CPUList.SelectedIndex = SelectedCPU

        ' OS kiadás korrekció
        OSRelase = CPUAddressWidth

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Lemezlista újratöltése ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function UpdateDiskList()

        ' Lista kiürítése
        ComboBox_DiskList.Items.Clear()

        ' WMI lekérdezés: Win32_DiskDrive -> Lemezmeghajtók
        objDD = New ManagementObjectSearcher("SELECT Index, Model FROM Win32_DiskDrive")

        ' Számláló beállítása
        Dim DiskCount As Int32 = 0                              ' Lemezek sorszáma
        Dim SortCount, ListCount As Int32                       ' Sorbarendezési sorszámok

        For Each Me.objMgmt In objDD.Get
            DiskList(DiskCount) = ToInt32(objMgmt("Index"))
            DiskName(DiskList(DiskCount)) = objMgmt("Model").ToString
            DiskCount += 1
        Next

        ' Sorbarendezés index alapján
        Dim DiskSort(DiskCount - 1) As Int32                    ' Lemezek sorrendje
        For SortCount = 0 To DiskCount - 1
            DiskSort(SortCount) = DiskList(SortCount)
        Next
        Array.Sort(DiskSort)

        ' Lemezlista feltöltése
        For ListCount = 0 To DiskCount - 1
            DiskName(ListCount) = ComboBox_DiskList.Items.Add(DiskName(DiskSort(ListCount)))
            DiskList(ListCount) = DiskSort(ListCount)
        Next

        ' Alapértelmezett kiválasztás visszaállítása (Legelső lemez!)
        SelectedDisk = 0
        ComboBox_DiskList.SelectedIndex = SelectedDisk

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Videokártya lista újratöltése ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function UpdateVideoList()

        ' Lista kiürítése
        ComboBox_VideoList.Items.Clear()

        ' WMI lekérdezés: Win32_DiskDrive -> Lemezmeghajtók
        objVC = New ManagementObjectSearcher("SELECT Caption FROM Win32_VideoController")

        ' Értékek definiálása
        Dim VideoCount As Int32 = 0                             ' Kártya sorszáma

        ' Értékek beállítása
        For Each Me.objMgmt In objVC.Get
            VideoName(VideoCount) = ComboBox_VideoList.Items.Add(objMgmt("Caption").ToString)
            VideoCount += 1
        Next

        ' Alapértelmezett kiválasztás visszaállítása (Legelső lemez!)
        SelectedVideo = 0
        ComboBox_VideoList.SelectedIndex = SelectedVideo

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Interfész lista újratöltése ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function UpdateInterfaceList()

        ' Lista kiürítése
        ComboBox_InterfaceList.Items.Clear()

        ' WMI lekérdezés: Win32_PerfRawData_Tcpip_NetworkInterface -> Interfészek
        objNI = New ManagementObjectSearcher("SELECT Name FROM Win32_PerfRawData_Tcpip_NetworkInterface")

        ' Számláló beállítása
        Dim InterfaceCount As Int32 = 0                         ' Interfészek sorszáma

        ' Interfészlista feltöltése
        For Each Me.objMgmt In objNI.Get
            InterfaceList(InterfaceCount) = objMgmt("Name")
            InterfaceName(InterfaceCount) = StringNormalize(objMgmt("Name"))
            ComboBox_InterfaceList.Items.Add(InterfaceName(InterfaceCount))
            InterfaceCount += 1
        Next

        ' Alapértelmezett kiválasztás visszaállítása (Legelső interfész!)
        SelectedInterface = 0
        ComboBox_InterfaceList.SelectedIndex = SelectedInterface

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Forgalmi diagram készítése ***
    ' Bemenet: TraffReset -> Forgalom nullázása (Boolean)
    ' Kimenet: Boolean (False)
    Private Function MakeChart(ByVal TraffReset As Boolean)

        ' Értékek definiálása
        Dim DrawOffset(2) As Int32                              ' Rajzolási koordináta eltolási értékek: kézi eltolás (grafikai jellegű)
        Dim TextOffset(2) As Int32                              ' Szöveges koordináta letolásiértékek: relatív eltolás (pl.: sorugrás vagy távolság ugrás)
        Dim StartCorrection As Int32                            ' Koordináta eltilási értékek hibakorrekciónál, pl. kerekítési hiba kiküszöbölése (matematikai jellegű)
        Dim SignFont As New Font("Arial", 8)                    ' Betűtípus és méret beállítása a rajzon lévő szövegekhez
        Dim SignLine(1) As Point                                ' Rajzolási koordináták (X0, X1, Y0, Y1)
        Dim InterfaceReset As Boolean = False                   ' Hibakorrekció uint változó átbillenésére (előjelcsere)
        Dim ResetCount As Int32 = 0                             ' Hibakorrekció számlálója (lépések száma az átbillenáés után)

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
            EventTimer.Enabled = False
            EventTimer.Enabled = True

            ' Generálási intervallum újraindítása (EventTimer által)
            TraffGenCounter = RefreshInterval(SelectedRefresh) - 1

            ' Forgalmi adatok frissítése
            UpdateTraffArray(True)

            ' Diagram rajzolásának kezdő állapotba hozása az állapotsorban
            StatusLabel_ChartStatus.Image = My.Resources.Resources.Control_Check
            StatusLabel_ChartStatus.Text = Str_ChartDone + " " + (RefreshInterval(SelectedRefresh).ToString) + " " + Str_ChartCount + "..."

        End If

        ' Sebességtömb feltöltése
        For i As Int32 = 0 To TraffResolution
            ChartDownNumbers(i) = (TraffDownArray(i) - TraffDownArray(i + 1)) / RefreshInterval(SelectedRefresh)
            ChartUpNumbers(i) = (TraffUpArray(i) - TraffUpArray(i + 1)) / RefreshInterval(SelectedRefresh)

            ' Hibakorrekció túlcsordulás ellen -> Letöltés
            If TraffDownArray(i) - TraffDownArray(i + 1) < 0 Then
                ChartDownNumbers(i) = (TraffDownArray(i + 1) - TraffDownArray(i + 2)) / RefreshInterval(SelectedRefresh)
                If CheckedDownChart Then
                    InterfaceReset = True
                    ResetCount = (TraffResolution) - i
                End If
            End If

            ' Hibakorrekció túlcsordulás ellen -> Feltöltés
            If TraffUpArray(i) - TraffUpArray(i + 1) < 0 Then
                ChartUpNumbers(i) = (TraffUpArray(i + 1) - TraffUpArray(i + 2)) / RefreshInterval(SelectedRefresh)
                If CheckedUpChart Then
                    InterfaceReset = True
                    ResetCount = (TraffResolution) - i
                End If
            End If

            ' Maximum érték keresés -> Letöltés
            If CheckedDownChart Then
                If ChartDownNumbers(i) > DownPeak Then
                    DownPeak = ChartDownNumbers(i)
                End If

                If ChartDownNumbers(i) > Amplitude Then
                    Amplitude = ChartDownNumbers(i)
                End If
            End If

            ' Maximum érték keresés -> Feltöltés
            If CheckedUpChart Then
                If ChartUpNumbers(i) > UpPeak Then
                    UpPeak = ChartUpNumbers(i)
                End If

                If ChartUpNumbers(i) > Amplitude Then
                    Amplitude = ChartUpNumbers(i)
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
        If TraffReset Then

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
        ScaleMax = DynByteConv(Amplitude, 2)

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
        UpPeakConv = DynByteConv(UpPeak, TraffDigit)

        ' Koordinátacsúszás beállítása: vízszintesen változatlan, függőlegesen kettővel felfelé -> Mindig a csúcsérték felett látszódik, az meg már el van tolva eggyel!
        DrawOffset = {0, -2}

        ' Függőleges koordináta számítása
        If UpPeak <> 0 Then
            UpPeakLine = ChartCanvas(1) - Round(((UpPeakConv(0) * (1000 ^ UpPeakConv(1))) / (PeakDiv * (1000 ^ PeakExp))) * (ChartCanvas(1) - ChartBegining(1)))
        Else
            UpPeakLine = ChartCanvas(1)
        End If

        ' Feltöltési segédegyenes megrajzolása
        If CheckedUpChart Then
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
        DownPeakConv = DynByteConv(DownPeak, TraffDigit)

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
        If CheckedDownChart Then
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
                UpByteDiffCurrent = DynByteConv(ChartUpNumbers(TraffResolution - Count), 2)
                UpByteDiffLast = DynByteConv(ChartUpNumbers(TraffResolution - (Count + 1)), 2)

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
                DownByteDiffCurrent = DynByteConv(ChartDownNumbers(TraffResolution - Count), 2)
                DownByteDiffLast = DynByteConv(ChartDownNumbers(TraffResolution - (Count + 1)), 2)

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
        Chart.DrawString(Str_Interface + ": " + InterfaceName(SelectedInterface), SignFont, Brushes.DeepSkyBlue, TextOffset(0), TextOffset(1))

        ' *** SZÖVEGES KIÍRÁSOK: Intervallumok (2.sor) ***

        ' Koordináták eltolása: sorugrás
        TextOffset(1) += 15

        ' Egységre állítás (Csak, ha maradék nélkül osztható!)
        IntervalValue = TraffResolution * RefreshInterval(SelectedRefresh)
        If IntervalValue >= 3600 And (IntervalValue Mod 3600) = 0 Then
            ' Másodperc -> Óra
            IntervalTag = (IntervalValue / 3600).ToString + " " + Str_Hours
        ElseIf IntervalValue >= 60 And (IntervalValue Mod 60) = 0 Then
            ' Másodperc -> Perc
            IntervalTag = (IntervalValue / 60).ToString + " " + Str_Mins
        Else
            ' Másodperc
            IntervalTag = IntervalValue.ToString + " " + Str_Secs
        End If

        ' Intervallum sor kiírása
        Chart.DrawString(Str_Interval + " - " + Str_Traffic + ": " + PeakDiv.ToString + " " + PrefixTable(PeakExp) + "B / " +
                         Str_Time + ": " + IntervalTag + " (" + Str_Update + ": " + RefreshInterval(SelectedRefresh).ToString + " " + Str_Secs + ")",
                         SignFont, Brushes.DeepSkyBlue, TextOffset(0), TextOffset(1))

        ' *** SZÖVEGES KIÍRÁSOK: Diagram készítési ideje (3.sor) ***

        ' Koordináták eltolása: sorugrás
        TextOffset(1) += 15

        ' Készítési idő kiírása
        Chart.DrawString(Str_ChartTime + " " + GetLocalizedDate(ChartCreationTime) + ".", SignFont, Brushes.DeepSkyBlue, TextOffset(0), TextOffset(1))

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
        Chart.DrawString(Str_ChartDown, SignFont, Brushes.Lime, TextOffset(0) + 25, TextOffset(1))

        ' Jelenlegi és csúcssebesség kiírása (vagy, ha ki van kapcsolva, akkor az erre vonatkozó szöveg)
        DownCurrentConv = DynByteConv(ChartDownNumbers(0), TraffDigit)

        If CheckedDownChart Then
            Chart.DrawString(Str_Current + ": " + FixDigitSeparator(DownCurrentConv(0), TraffDigit, True) + " " +
                             PrefixTable(DownCurrentConv(1)).ToString + "B/s", SignFont, Brushes.DarkGreen, TextOffset(0) + TextSpacing(0), TextOffset(1))
            Chart.DrawString(Str_Peak + ": " + FixDigitSeparator(DownPeakConv(0), TraffDigit, True) + " " +
                             PrefixTable(DownPeakConv(1)).ToString + "B/s", SignFont, Brushes.DarkGreen, TextOffset(0) + TextSpacing(1), TextOffset(1))
        Else
            Chart.DrawString(Str_ChartHide, SignFont, Brushes.Gray, TextOffset(0) + TextSpacing(0), TextOffset(1))
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
        Chart.DrawString(Str_ChartUp, SignFont, Brushes.Red, TextOffset(0) + 25, TextOffset(1))

        ' Jelenlegi és csúcssebesség kiírása (vagy, ha ki van kapcsolva, akkor az erre vonatkozó szöveg)
        UpCurrentConv = DynByteConv(ChartUpNumbers(0), TraffDigit)

        If CheckedUpChart Then
            Chart.DrawString(Str_Current + ": " + FixDigitSeparator(UpCurrentConv(0), TraffDigit, True) + " " +
                             PrefixTable(UpPeakConv(1)).ToString + "B/s", SignFont, Brushes.DarkRed, TextOffset(0) + TextSpacing(0), TextOffset(1))
            Chart.DrawString(Str_Peak + ": " + FixDigitSeparator(UpPeakConv(0), TraffDigit, True) + " " +
                             PrefixTable(UpPeakConv(1)).ToString + "B/s", SignFont, Brushes.DarkRed, TextOffset(0) + TextSpacing(1), TextOffset(1))
        Else
            Chart.DrawString(Str_ChartHide, SignFont, Brushes.Gray, TextOffset(0) + TextSpacing(0), TextOffset(1))
        End If

        ' ----- KÉPMŰVELETEK -----

        ' Elkészült kép kirajzolása
        PictureBox_TrafficChart.Image = Picture

        Return False

    End Function

    ' *** FÜGGVÉNY: Közvetett időzítő ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function TraffRefresh()

        ' Diagram ToolTip beállítása (másodpercek kiírásával)
        EventToolTip.SetToolTip(PictureBox_TrafficChart, Tip_Chart + " (" + RefreshInterval(SelectedRefresh).ToString + " " + Tip_Average + ")")

        ' Forgalmi adatok frissítése
        UpdateTraffArray(False)

        ' Rácsfrissítés engedélyezése
        GridUpdate = True

        ' Diagram frissítése
        MakeChart(False)

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' ----- ELJÁRÁSOK -----

    ' *** ELJÁRÁS: Közvetlen időzítő ***
    ' Eseményvezérelt: EventTimer.Tick -> Óra ugrása
    Private Sub EventTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EventTimer.Tick

        ' Eseményszámláló növelése
        TimerSeconds += 1

        ' Processzorórajel frissítése
        SetCPUInformation()

        ' Memóriakihasználtság frissítése
        SetMemoryInformation()

        ' Uptime frissítése
        SetUptime(UptimeSeconds, TimerSeconds)

        ' Interfész statisztika frissítése
        UpdateSpeedStatistics(False)

        ' Diagramgenerálás időközének beálltása és kiírása az állapotsorban
        If TraffGenCounter <> 0 Then
            StatusLabel_ChartStatus.Text = Str_ChartRedraw + " " + TraffGenCounter.ToString + " " + Str_ChartCount + "..."
            StatusLabel_ChartStatus.Image = My.Resources.Resources.Control_Load
            TraffGenCounter = TraffGenCounter - 1
        Else
            TraffRefresh()
            TraffGenCounter = RefreshInterval(SelectedRefresh) - 1
            StatusLabel_ChartStatus.Image = My.Resources.Resources.Control_Check
            StatusLabel_ChartStatus.Text = Str_ChartDone + " " + (TraffGenCounter + 1).ToString + " " + Str_ChartCount + "..."
        End If

    End Sub

    ' *** ELJÁRÁS: Nyelvek betöltése *** 
    ' Eseményvezérelt: ComboBox_LanguageList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub LanguageList_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_LanguageList.SelectedIndexChanged

        ' Változás beállítása
        SelectedLanguage = ComboBox_LanguageList.SelectedIndex

        ' ----- NYELVEK BETÖLTÉSE -----
        ' Megjegyzés: 0 = angol (alapértelmezett), 1 = magyar (Jelenleg statikus!)
        Select Case ComboBox_LanguageList.SelectedIndex

            Case 0 ' *** NYELV: Angol kiírások -> Alapértelmezett (0) ***

                ' Sztringek
                Str_Title = "System Information and Network Monitor"
                Str_Comment = "This software is open source and portable."
                Str_Version = "Version"
                Str_Loading = "Loading"
                Str_LoadReg = "Registry settings"
                Str_LoadWMI = "WMI database"
                Str_DigitSeparator = "."
                Str_Interval = "Intervals"
                Str_Traffic = "Traffic"
                Str_Motherboard = "Motherboard"
                Str_System = "Computer"
                Str_BIOS = "BIOS"
                Str_Interface = "Interface"
                Str_ChartTime = "Chart created on"
                Str_Time = "Time"
                Str_Update = "Update"
                Str_Current = "Current"
                Str_Peak = "Peak"
                Str_DayName = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}
                Str_MonthName = {"January", "February", "March", "April", "May", "June", "July",
                                 "August", "September", "October", "November", "December"}
                Str_DateFormat = "dddd, dd MMMM yyyy, h:mm:ss tt"
                Str_ChartDown = "Download speed"
                Str_ChartUp = "Upload speed"
                Str_ChartHide = "Chart rendering disabled."
                Str_ChartDone = "Status: Chart done, next is in"
                Str_ChartRedraw = "Status: Chart redraw in"
                Str_ChartCount = "seconds"
                Str_Hostname = "Hostname"
                Str_Uptime = "Uptime"
                Str_Days = "days"
                Str_Hours = "hours"
                Str_Mins = "minutes"
                Str_Secs = "seconds"
                Str_And = "and"
                Str_QuitAsk = "Do you really want to quit?"
                Str_QuitTitle = "Exit confirmation"
                Str_Note = "Notification"
                Str_Taskbar = "The process is still running in the background."
                Str_ImageSaved = "Image saved"
                Str_NoDisk = "No disk"
                Str_Unknown = "Not available"
                Str_Invalid = "Invalid"
                Str_Inactive = "Inactive"
                Str_Close = "Close"

                ' ToolTip sztringek
                Tip_Language = "Language selection"
                Tip_CPU = "Processor selection"
                Tip_Disk = "Disk selection"
                Tip_Video = "Graphics card selection"
                Tip_Interface = "Network adapter selection"
                Tip_Reload = "Reload list"
                Tip_Chart = "Traffic history chart"
                Tip_Average = "seconds averages"
                Tip_ChartDown = "Enable/disable download chart rendering (Alt + D)"
                Tip_ChartUp = "Enable/disable upload chart rendering (Alt + U)"
                Tip_Refresh = "Refresh interval selection (seconds)"
                Tip_Exit = "Exit (Alt + X)"
                Tip_LinkBottom = "About..."
                Tip_Hostname = "Host information"
                Tip_Uptime = "System uptime"
                Tip_Status = "Status of the chart creation process"
                Tip_TopMost = "Always on top (Green = enabled, Red = disabled)"
                Tip_Screenshot = "Take screenshot"

                ' Feliratok
                Name_HWList.Text = "Component:"
                Name_HWVendor.Text = "Vendor:"
                Name_HWIdentifier.Text = "Identifier:"
                Name_CPUList.Text = "Processor:"
                Name_CPUCore.Text = "Cores / Threads:"
                Name_CPUClock.Text = "Clock:"
                Name_CPUMaxClock.Text = "Maximum:"
                Name_PhysicalMemory.Text = "Physical memory:"
                Name_PhysicalMemoryUsage.Text = "Usage:"
                Name_PhysicalMemoryFree.Text = "Free:"
                Name_VirtualMemory.Text = "Virtual memory:"
                Name_VirtualMemoryUsage.Text = "Usage:"
                Name_VirtualMemoryFree.Text = "Free:"
                Name_OSName.Text = "Product name:"
                Name_OSVersion.Text = "Version number:"
                Name_OSBuild.Text = "Build:"
                Name_OSRelease.Text = "Release:"
                Name_DiskList.Text = "Disk drive:"
                Name_DiskIndex.Text = "Disk index:"
                Name_DiskCapacity.Text = "Capacity:"
                Name_DiskInterface.Text = "Interface:"
                Name_DiskFirmware.Text = "Firmware:"
                Name_DiskSmart.Text = "S.M.A.R.T status:"
                Name_DiskSerial.Text = "Serial number:"
                Name_VideoList.Text = "Graphics card:"
                Name_VideoMemory.Text = "Video memory:"
                Name_VideoResolution.Text = "Resolution:"
                Name_InterfaceList.Text = "Interface:"
                Name_Bandwidth.Text = "Bandwidth:"
                Name_InterfaceUsage.Text = "Interface usage:"
                Name_DownloadSpeed.Text = "Download speed:"
                Name_UploadSpeed.Text = "Upload speed:"
                Name_ChartVisible.Text = "Chart visibility:"
                Name_UpdateList.Text = "Update interval:"
                Name_UpdateUnit.Text = "s"

                ' Csoportfeliratok
                GroupBox_HWInfo.Text = "Computer system information"
                GroupBox_CPUInfo.Text = "Processor information"
                GroupBox_MemoryInfo.Text = "Memory information"
                GroupBox_OSInfo.Text = "Operating system information"
                GroupBox_DiskInfo.Text = "Disk information"
                GroupBox_VideoInfo.Text = "Display controller information"
                GroupBox_Network.Text = "Network interface statistics"

                ' Menüelemek
                MainMenuItem_Settings.Text = "&Settings"
                MainMenuItem_Chart.Text = "&Chart"
                MainMenuItem_Information.Text = "&Action"
                MainMenu_SettingsItem_TopMost.Text = "Always on &top"
                MainMenu_SettingsItem_TaskbarMinimize.Text = "&Minimize to taskbar"
                MainMenu_SettingsItem_DisableConfirm.Text = "Disable exit &confirmation"
                MainMenu_SettingsItem_DisableSplash.Text = "Disable loading &screen"
                MainMenu_ActionItem_UpdateCheck.Text = "Check for u&pdates..."
                MainMenu_ActionItem_About.Text = "A&bout..."
                MainMenu_ActionItem_Exit.Text = "E&xit"
                MainMenu_ChartItem_DownloadVisible.Text = "Show &download chart"
                MainMenu_ChartItem_UploadVisible.Text = "Show &upload chart"
                MainMenu_ChartItem_ClearChart.Text = "&Clear chart"

                ' Checkbox feliratok
                CheckBoxChart_DownloadVisible.Text = "&Download"
                CheckBoxChart_UploadVisible.Text = "&Upload"

                ' Gombfeliratok
                Button_Exit.Text = "E&xit"

            Case 1 ' *** NYELV: Magyar kiírások (1) ***

                ' Sztringek
                Str_Title = "Rendszerinformációk és hálózatfigyelés"
                Str_Comment = "Ez a szoftver nyílt forrású és hordozható."
                Str_Version = "Verziószám"
                Str_Loading = "Betöltés"
                Str_LoadReg = "Registry beállítások"
                Str_LoadWMI = "WMI adatbázis"
                Str_DigitSeparator = ","
                Str_Interval = "Intervallumok"
                Str_Traffic = "Forgalom"
                Str_Motherboard = "Alaplap"
                Str_System = "Számítógép"
                Str_BIOS = "BIOS"
                Str_Interface = "Interfész"
                Str_ChartTime = "Diagram elkészítve:"
                Str_Time = "Idő"
                Str_Update = "Frissítés"
                Str_Current = "Jelenlegi"
                Str_Peak = "Csúcs"
                Str_DayName = {"vasárnap", "hétfő", "kedd", "szerda", "csütörtök", "péntek", "szombat"}
                Str_MonthName = {"január", "február", "március", "április", "május", "június", "július",
                                 "augusztus", "szeptember", "október", "november", "december"}
                Str_DateFormat = "yyyy. MMMM dd. dddd, H:mm:ss"
                Str_ChartDown = "Letöltési sebesség"
                Str_ChartUp = "Feltöltési sebesség"
                Str_ChartHide = "A diagram leképezés ki van kapcsolva."
                Str_ChartDone = "Állapot: Diagram kész, következő"
                Str_ChartRedraw = "Állapot: Diagram újrarajzolása"
                Str_ChartCount = "másodperc múlva"
                Str_Hostname = "Hosztnév"
                Str_Uptime = "Futási idő"
                Str_Days = "nap"
                Str_Hours = "óra"
                Str_Mins = "perc"
                Str_Secs = "másodperc"
                Str_And = "és"
                Str_QuitAsk = "Valóban ki szeretne lépni?"
                Str_QuitTitle = "Kilépés megerősítése"
                Str_Note = "Értesítés"
                Str_Taskbar = "A folyamat továbbra is fut a háttérben."
                Str_ImageSaved = "Kép elmentve"
                Str_NoDisk = "Nincs lemez"
                Str_Unknown = "Nem elérhető"
                Str_Invalid = "Érvénytelen"
                Str_Inactive = "Inaktív"
                Str_Close = "Bezárás"

                ' ToolTip sztringek
                Tip_Language = "Nyelv kiválasztása"
                Tip_CPU = "Processzor kiválasztása"
                Tip_Disk = "Lemez kiválasztása"
                Tip_Video = "Graphics card selection"
                Tip_Interface = "Hálózati adapter kiválasztása"
                Tip_Reload = "Lista újratöltése"
                Tip_Chart = "Adatforgalmi előzmények diagramja"
                Tip_Average = "másodperces átlagok"
                Tip_ChartDown = "Letöltési diagram leképezésének engedélyezése/tiltása (Alt + L)"
                Tip_ChartUp = "Feltöltési diagram leképezésének engedélyezése/tiltása (Alt + F)"
                Tip_Refresh = "Frissítési időköz kiválasztása (másodperc)"
                Tip_Exit = "Kilépés (Alt + K)"
                Tip_LinkBottom = "Névjegy..."
                Tip_Hostname = "Hoszt információk"
                Tip_Uptime = "Rendszer futási ideje"
                Tip_Status = "A diagram jelenlegi állapota"
                Tip_TopMost = "Mindig látható (zöld = engedélyezve, piros = tiltva)"
                Tip_Screenshot = "Képernyőkép készítése"

                ' Feliratok
                Name_HWList.Text = "Komponens:"
                Name_HWVendor.Text = "Gyártó:"
                Name_HWIdentifier.Text = "Azonosító:"
                Name_CPUList.Text = "Processzor:"
                Name_CPUCore.Text = "Magok / Szálak:"
                Name_CPUClock.Text = "Órajel:"
                Name_CPUMaxClock.Text = "Maximum:"
                Name_PhysicalMemory.Text = "Fizikai memória:"
                Name_PhysicalMemoryUsage.Text = "Kihasználtság:"
                Name_PhysicalMemoryFree.Text = "Szabad:"
                Name_VirtualMemory.Text = "Virtuális memória:"
                Name_VirtualMemoryUsage.Text = "Kihasználtság:"
                Name_VirtualMemoryFree.Text = "Szabad:"
                Name_OSName.Text = "Termék neve:"
                Name_OSVersion.Text = "Verziószám:"
                Name_OSBuild.Text = "Build:"
                Name_OSRelease.Text = "Kiadás:"
                Name_DiskList.Text = "Meghajtó:"
                Name_DiskIndex.Text = "Sorszám:"
                Name_DiskCapacity.Text = "Tárterület:"
                Name_DiskInterface.Text = "Interfész:"
                Name_DiskFirmware.Text = "Firmware:"
                Name_DiskSmart.Text = "S.M.A.R.T állapot:"
                Name_DiskSerial.Text = "Sorozatszám:"
                Name_VideoList.Text = "Videokártya:"
                Name_VideoMemory.Text = "Videomemória:"
                Name_VideoResolution.Text = "Felbontás:"
                Name_InterfaceList.Text = "Interfész:"
                Name_Bandwidth.Text = "Sávszélesség:"
                Name_InterfaceUsage.Text = "Interfész kihasználtsága:"
                Name_DownloadSpeed.Text = "Letöltési sebesség:"
                Name_UploadSpeed.Text = "Feltöltés sebesség:"
                Name_ChartVisible.Text = "Diagramok:"
                Name_UpdateList.Text = "Frissítési időköz:"
                Name_UpdateUnit.Text = "s"

                ' Csoportfeliratok
                GroupBox_HWInfo.Text = "Számítógép rendszer információk"
                GroupBox_CPUInfo.Text = "Processzor információk"
                GroupBox_MemoryInfo.Text = "Memória információk"
                GroupBox_OSInfo.Text = "Operációs rendszer információk"
                GroupBox_DiskInfo.Text = "Meghajtó információk"
                GroupBox_VideoInfo.Text = "Videovezérlő információk"
                GroupBox_Network.Text = "Hálózati interfész statisztika"

                ' Menüelemek
                MainMenuItem_Settings.Text = "&Beállítások"
                MainMenuItem_Chart.Text = "&Diagramok"
                MainMenuItem_Information.Text = "&Műveletek"
                MainMenu_SettingsItem_TopMost.Text = "Mi&ndig látható"
                MainMenu_SettingsItem_TaskbarMinimize.Text = "Kicsinyítés a &rendszerikonok közé"
                MainMenu_SettingsItem_DisableConfirm.Text = "&Kilépési megerősítés kikapcsolása"
                MainMenu_SettingsItem_DisableSplash.Text = "&Betöltő képernyő elrejtése"
                MainMenu_ActionItem_UpdateCheck.Text = "&Frissítések keresése..."
                MainMenu_ActionItem_About.Text = "&Névjegy..."
                MainMenu_ActionItem_Exit.Text = "&Kilépés"
                MainMenu_ChartItem_DownloadVisible.Text = "&Letöltési diagram mutatása"
                MainMenu_ChartItem_UploadVisible.Text = "&Feltöltési diagram mutatása"
                MainMenu_ChartItem_ClearChart.Text = "Diagram &törlése"

                ' Checkbox feliratok
                CheckBoxChart_DownloadVisible.Text = "&Letöltés"
                CheckBoxChart_UploadVisible.Text = "&Feltöltés"

                ' Gombfeliratok
                Button_Exit.Text = "&Kilépés"

        End Select

        ' Egyező menüelemek feltöltése (Egyező néven fut, csak másik menüben!)
        MainContextMenuItem_TopMost.Text = MainMenu_SettingsItem_TopMost.Text
        MainContextMenuItem_TaskbarMinimize.Text = MainMenu_SettingsItem_TaskbarMinimize.Text
        MainContextMenuItem_DisableConfirm.Text = MainMenu_SettingsItem_DisableConfirm.Text
        MainContextMenuItem_DisableSplash.Text = MainMenu_SettingsItem_DisableSplash.Text
        MainContextMenuItem_UpdateCheck.Text = MainMenu_ActionItem_UpdateCheck.Text
        MainContextMenuItem_About.Text = MainMenu_ActionItem_About.Text
        MainContextMenuItem_Exit.Text = MainMenu_ActionItem_Exit.Text

        ' Hosztnév beálítása az állapotsorban
        StatusLabel_Host.Text = Str_Hostname + ": " + Hostname

        ' Az ablak címének beállítása
        Me.Text = MyName + " - " + Str_Title + ", " + Str_Version + " " + VersionString

        ' Taskbar ikon nevének beállítása
        MainNotifyIcon.Text = MyName + " - " + Str_Title

        ' Alsó link szövegének beállítása
        Link_Bottom.Text = Str_Title + " - " + Str_Comment

        ' Kis méretű buborék visszaállítása (Nyelvváltás után a választott nyelven is jelenjen meg adott esetben!)
        DisableBalloon = False

        ' Checkbox és Combobox ToolTip értékek beállítása
        EventToolTip.SetToolTip(ComboBox_LanguageList, Tip_Language)
        EventToolTip.SetToolTip(ComboBox_CPUList, Tip_CPU)
        EventToolTip.SetToolTip(ComboBox_DiskList, Tip_Disk)
        EventToolTip.SetToolTip(ComboBox_VideoList, Tip_Video)
        EventToolTip.SetToolTip(ComboBox_InterfaceList, Tip_Interface)
        EventToolTip.SetToolTip(Button_CPUListReload, Tip_Reload)
        EventToolTip.SetToolTip(Button_DiskListReload, Tip_Reload)
        EventToolTip.SetToolTip(Button_VideoListReload, Tip_Reload)
        EventToolTip.SetToolTip(Button_InterfaceListReload, Tip_Reload)
        EventToolTip.SetToolTip(PictureBox_TrafficChart, Tip_Chart + " (" + RefreshInterval(SelectedRefresh).ToString + " " + Tip_Average + ")") ' Az átlagértékek is hozzáadva!
        EventToolTip.SetToolTip(CheckBoxChart_DownloadVisible, Tip_ChartDown)
        EventToolTip.SetToolTip(CheckBoxChart_UploadVisible, Tip_ChartUp)
        EventToolTip.SetToolTip(ComboBox_UpdateList, Tip_Refresh)
        EventToolTip.SetToolTip(Button_Exit, Tip_Exit)
        EventToolTip.SetToolTip(Link_Bottom, Tip_LinkBottom)

        ' Állapot- és menüsori ToolTip kiírások beállítása
        StatusLabel_Host.ToolTipText = Tip_Hostname
        StatusLabel_Uptime.ToolTipText = Tip_Uptime
        StatusLabel_ChartStatus.ToolTipText = Tip_Status
        StatusLabel_TopMost.ToolTipText = Tip_TopMost
        ScreenshotToolStripMenuItem.ToolTipText = Tip_Screenshot

        ' ComboBox elemek
        ComboBox_HWList.Items.Clear()
        ComboBox_HWList.Items.AddRange(New Object() {Str_Motherboard, Str_System, Str_BIOS})
        If MainWindowDone Then
            ComboBox_HWList.SelectedIndex = SelectedHardware
        End If

        ' Lemez és videokártya információk újbóli lekérdezése a kiválasztott lemezhez (A nyelvi formázások miatt fontos!)
        If MainWindowDone Then
            SetDiskInformation()
            SetVideoInformation()
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
        EventToolTip.SetToolTip(PictureBox_TrafficChart, Tip_Chart + " (" + RefreshInterval(SelectedRefresh).ToString + " " + Tip_Average + ")")

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
            Value_HWVendor.Text = "(" + Str_Unknown + ")"
        Else
            Value_HWVendor.Text = StringNormalize(HWVendor(SelectedHardware))
        End If

        If HWIdentifier(SelectedHardware) = Nothing Then
            Value_HWIdentifier.Text = "(" + Str_Unknown + ")"
        Else
            Value_HWIdentifier.Text = StringNormalize(HWIdentifier(SelectedHardware))
        End If

    End Sub

    ' *** ELJÁRÁS: Processzor kiválasztása ***
    ' Eseményvezérelt: ComboBox_CPUList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub ComboBox_CPUList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_CPUList.SelectedIndexChanged

        ' Változás beállítása
        SelectedCPU = ComboBox_CPUList.SelectedIndex

        ' Videokártya információk lekérdezése
        SetCPUInformation()

    End Sub

    ' *** ELJÁRÁS: Meghajtó kiválasztása ***
    ' Eseményvezérelt: ComboBox_DiskList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub DiskList_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_DiskList.SelectedIndexChanged

        ' Változás beállítása
        SelectedDisk = ComboBox_DiskList.SelectedIndex

        ' Értékek lekérdezése
        SetDiskInformation()

    End Sub

    ' *** ELJÁRÁS: Videokártya kiválasztása ***
    ' Eseményvezérelt: ComboBox_VideoList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub ComboBox_VideoList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_VideoList.SelectedIndexChanged

        ' Változás beállítása
        SelectedVideo = ComboBox_VideoList.SelectedIndex

        ' Videokártya információk lekérdezése
        SetVideoInformation()

    End Sub

    ' *** ELJÁRÁS: Interfész kiválasztása ***
    ' Eseményvezérelt: ComboBox_InterfaceList.SelectedIndexChanged -> Listaelem kiválasztása
    Private Sub InterfaceList_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_InterfaceList.SelectedIndexChanged

        ' Változás beállítása
        SelectedInterface = ComboBox_InterfaceList.SelectedIndex

        ' Statisztika nullázása
        UpdateSpeedStatistics(True)

        ' Diagram frissítése
        MakeChart(True)

    End Sub

    ' *** ELJÁRÁS: Processzor lista újratöltése ***
    ' Eseményvezérelt: Button_CPUListReload.Click -> Gomb megnyomása
    Private Sub CPUList_Reload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_CPUListReload.Click

        ' Processzor lista újratöltése -> függvény
        UpdateCPUList()

    End Sub

    ' *** ELJÁRÁS: Meghajtólista újratöltése ***
    ' Eseményvezérelt: Button_DiskListReload.Click -> Gomb megnyomása
    Private Sub DiskList_Reload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_DiskListReload.Click

        ' Lemezlista újratöltése -> függvény
        UpdateDiskList()

    End Sub

    ' *** ELJÁRÁS: Videokártya lista újratöltése ***
    ' Eseményvezérelt: Button_VideoListReload.Click -> Gomb megnyomása
    Private Sub VideoList_Reload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_VideoListReload.Click

        ' Videokártya lista újratöltése -> függvény
        UpdateVideoList()

    End Sub

    ' *** ELJÁRÁS: Interfész lista újratöltése ***
    ' Eseményvezérelt: Button_InterfaceListReload.Click -> Gomb megnyomása
    Private Sub InterfaceList_Reload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_InterfaceListReload.Click

        ' Interfész lista újratöltése -> függvény
        UpdateInterfaceList()

    End Sub

    ' *** ELJÁRÁS: Letöltési diagram generálásának ellenőrzése ***
    ' Eseményvezérelt: MainMenu_ChartItem_DownloadVisible.Click, ChartMenuItem_DownloadVisible.Click, CheckBoxChart_DownloadVisible.Click -> Klikk (Menüelem, Checkbox)
    Private Sub DownloadChartVisible_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_ChartItem_DownloadVisible.Click, CheckBoxChart_DownloadVisible.Click

        ' Változás ellenőrzése és állapot invertálása
        If CheckedDownChart Then
            CheckedDownChart = False
        Else
            CheckedDownChart = True
        End If

        ' Checkbox állapotának beállítása
        CheckBoxChart_DownloadVisible.Checked = CheckedDownChart

        ' Menüelem állapotának beállítása
        MainMenu_ChartItem_DownloadVisible.Checked = CheckedDownChart

        ' Diagram frissítése
        MakeChart(False)

    End Sub

    ' *** ELJÁRÁS: Feltöltési diagram generálásának ellenőrzése ***
    ' Eseményvezérelt: MainMenu_ChartItem_UploadVisible.Click, ChartMenuItem_UploadVisible.Click, CheckBoxChart_UploadVisible.Click -> Klikk (Menüelem, Checkbox)
    Private Sub UploadChartVisible_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_ChartItem_UploadVisible.Click, CheckBoxChart_UploadVisible.Click

        ' Változás ellenőrzése és állapot invertálása
        If CheckedUpChart Then
            CheckedUpChart = False
        Else
            CheckedUpChart = True
        End If

        ' Checkbox állapotának beállítása
        CheckBoxChart_UploadVisible.Checked = CheckedUpChart

        ' Menüelem állapotának beállítása
        MainMenu_ChartItem_UploadVisible.Checked = CheckedUpChart

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

        ' Beállításjegyzék értékeinek mentése (Ha a főablak hiba nélkül betöltött)
        If MainWindowDone Then
            RegPath.SetValue("DisableLoadSplash", ToInt32(CheckedSplashDisable), RegistryValueKind.DWord)
            RegPath.SetValue("SelectedLanguage", SelectedLanguage, RegistryValueKind.DWord)
            RegPath.SetValue("SelectedRefreshIndex", SelectedRefresh, RegistryValueKind.DWord)
            RegPath.SetValue("SelectedInterface", SelectedInterface, RegistryValueKind.DWord)
            RegPath.SetValue("ChartVisibleDownload", ToInt32(CheckedDownChart), RegistryValueKind.DWord)
            RegPath.SetValue("ChartVisibleUpload", ToInt32(CheckedUpChart), RegistryValueKind.DWord)
            RegPath.SetValue("EnableTopMost", ToInt32(CheckedTopMost), RegistryValueKind.DWord)
            RegPath.SetValue("DisableExitConfirmation", ToInt32(CheckedNoQuitAsk), RegistryValueKind.DWord)
            RegPath.SetValue("MinimizeToTaskbar", ToInt32(CheckedMinToTray), RegistryValueKind.DWord)
        End If

        ' Kilépési megerősítés
        If CheckedNoQuitAsk = False Then

            ' Folyamtos láthatóság kikapcsolása (FONTOS! Ha ez nincs, akkor nem látszik a kilépési megerősítő ablak!!!)
            Me.TopMost = False

            ' Megerősítőablak megjelenítése (Igen -> Kilépés, Nem -> Mégse)
            e.Cancel = MsgBox(Str_QuitAsk, vbQuestion + vbYesNo + vbMsgBoxSetForeground, Str_QuitTitle) = MsgBoxResult.No

            ' Folyamatos láthatósag visszaállítása
            Me.TopMost = CheckedTopMost

        End If

    End Sub

    ' *** ELJÁRÁS: Kis méret gomb megnyomása ***
    ' Eseményvezérelt: Me.Resize -> Ablak átméretezése
    Private Sub MainWindow_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        ' Splash ablak bezárása
        LoadSplash.Close()

        ' Kis méret állapotának beállítása
        If Me.WindowState = FormWindowState.Minimized And CheckedMinToTray Then
            Me.Visible = False

            ' Buboréküzenet megjelenítése, majd későbbi kikapcsolása (Csak az első tálcára tételkor jelenik meg!)
            If Not DisableBalloon Then
                MainNotifyIcon.ShowBalloonTip(3000, MyName + " - " + Str_Note, Str_Taskbar, ToolTipIcon.Info)
                DisableBalloon = True
            End If
        End If

    End Sub

    ' *** ELJÁRÁS: Tálcaikon duplaklikk kezelése (Kis méret: oda-vissza) ***
    ' Eseményvezérelt: MainNotifyIcon.MouseDoubleClick -> Dupla klikk (Taskbar ikon)
    Private Sub MainNotifyIcon_DoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainNotifyIcon.MouseDoubleClick

        ' Splash ablak bezárása
        LoadSplash.Close()

        ' Változás ellenőrzése és állapot invertálása
        If Me.WindowState = FormWindowState.Normal Then
            If CheckedMinToTray Then
                Me.Visible = False
                If Not DisableBalloon Then
                    MainNotifyIcon.ShowBalloonTip(3000, MyName, Str_Taskbar, ToolTipIcon.Info)
                    DisableBalloon = True
                End If
            End If
            Me.WindowState = FormWindowState.Minimized
        Else
            Me.Visible = True
            Me.WindowState = FormWindowState.Normal
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

        ' Splash ablak bezárása
        LoadSplash.Close()

        ' Változás ellenőrzése és állapot invertálása
        If Me.TopMost Then
            CheckedTopMost = False
            StatusLabel_TopMost.Image = My.Resources.Resources.Control_RedPin
        Else
            CheckedTopMost = True
            StatusLabel_TopMost.Image = My.Resources.Resources.Control_GreenPin
        End If

        ' Láthatóság beállítása
        Me.TopMost = CheckedTopMost

        ' Menüelem állapotának beállítása
        MainMenu_SettingsItem_TopMost.Checked = CheckedTopMost
        MainContextMenuItem_TopMost.Checked = CheckedTopMost

    End Sub

    ' *** ELJÁRÁS: Forgalmi diagram törlése ***
    ' Eseményvezérelt: MainMenu_ChartItem_ClearChart.Click, ChartMenuItem_ClearChart.Click -> Állapotváltozás (Menüelem)
    Private Sub Chart_Clear(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainMenu_ChartItem_ClearChart.Click

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

        ' Splash ablak bezárása
        LoadSplash.Close()

        ' Splash időzítő újbóli elindítása és ablak megjelenítése
        LoadSplash.SplashTimer.Enabled = False
        LoadSplash.Visible = True

    End Sub

    ' *** ELJÁRÁS: Képernyőkép mentése ***
    ' Eseményvezérelt: ScreenshotToolStripMenuItem.Click -> Klikk (Menüelem)
    Private Sub Screenshot_Save(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScreenshotToolStripMenuItem.Click

        ' Mentési kép létrhozása és feltöltése
        Dim SaveImage As New Bitmap(Me.Width, Me.Height, Imaging.PixelFormat.Format24bppRgb)
        DrawToBitmap(SaveImage, New Rectangle(0, 0, Me.Width, Me.Height))

        ' Kép készítési idejének frissítése
        Dim ImageCreationTime As DateTime = DateTime.Now

        ' Fájnév generálása
        Dim FileName As String = MyName + "_" + Hostname + "_" + Format(ImageCreationTime, "yyyyMMdd-HHmmss") + ".png"

        ' Elérési út beállítása (Desktop)
        Dim DesktopPath As String = My.Computer.FileSystem.SpecialDirectories.Desktop
        SavePath = DesktopPath + "\" + FileName

        ' Kép mentése (PNG)
        SaveImage.Save(SavePath, Imaging.ImageFormat.Png)

        ' Buboréküzenet állapotának beállítása (fájl megnyitása) és üzenet megjelenítése
        OpenFile = True
        MainNotifyIcon.ShowBalloonTip(5000, MyName + " - " + Str_Note, Str_ImageSaved + ": '" + SavePath + "'", ToolTipIcon.Info)

    End Sub

    ' *** ELJÁRÁS: Buboréküzenetre kattintás kezelése ***
    ' Eseményvezérelt: MainNotifyIcon.BalloonTipClicked -> Klikk (Taskbar ikon buboréküzenet)
    Private Sub MainNotifyIcon_BalloonTipClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainNotifyIcon.BalloonTipClicked

        ' Kép megnyitása, a buboréközenet fájl mentésére vonatkozik
        If OpenFile Then
            Process.Start(SavePath)

            ' Buboréküzenet állapot visszaállítása
            OpenFile = False
        End If

    End Sub

End Class