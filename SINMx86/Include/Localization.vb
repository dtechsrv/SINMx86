Imports System.Math

Imports SINMx86.Functions

' Nyelvi osztály
Public Class Localization

    ' Alapbeállítások
    Public Shared Languages() As String = {"English (EN)", "Magyar (HU)"}   ' Nyelvek (statikus)
    Public Shared SelectedLanguage As Int32                                 ' Kiválasztott nyelv

    ' Nyelvi változók csoportosítása
    Public Shared LocRef(0) As String                                       ' Nyelvi változók neveinek tömbje
    Public Shared LocStr(0) As String                                       ' Nyelvi változók értékeinek tömbje

    ' Önálló nyelvi változók
    Public Shared LocDayName(7), LocMonthName(12) As String                 ' Napok és hónapok nevei

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVÉNY: Nyelvek betöltése ***
    ' Bemenet: LangID -> kiválasztott nyelv sorszáma (Int32)
    ' Kimenet: *      -> hamis érték (Boolean)
    Public Shared Function LoadLocalization(ByVal LangID As Int32)

        ' Választási lista
        Select Case LangID

            Case 0 ' *** NYELV: Angol kiírások -> Alapértelmezett (0) ***

                ' Napok és hónapok nevei
                LocDayName = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}
                LocMonthName = {"January", "February", "March", "April", "May", "June", "July",
                                "August", "September", "October", "November", "December"}

                ' Főablak nyelvi változói
                AddLoc("Title", "System Information and Network Monitor")
                AddLoc("Comment", "This software is open source and portable.")
                AddLoc("Version", "Version")
                AddLoc("DigitSeparator", ".")
                AddLoc("ScaleSeparator", ",")
                AddLoc("Interval", "Intervals")
                AddLoc("Traffic", "Traffic")
                AddLoc("Motherboard", "Motherboard")
                AddLoc("System", "Computer")
                AddLoc("BIOS", "BIOS")
                AddLoc("Battery", "Battery")
                AddLoc("Serial", "Serial number")
                AddLoc("Volt", "Nominal voltage")
                AddLoc("SvcPack", "Service Pack")
                AddLoc("Interface", "Interface")
                AddLoc("ChartTime", "Chart created on")
                AddLoc("Time", "Time")
                AddLoc("Update", "Update")
                AddLoc("Current", "Current")
                AddLoc("Peak", "Peak")
                AddLoc("DateFormat", "dddd, dd MMMM yyyy, h:mm:ss tt")
                AddLoc("ChartDown", "Download speed")
                AddLoc("ChartUp", "Upload speed")
                AddLoc("ChartHide", "Chart rendering disabled.")
                AddLoc("ChartStop", "Status: Chart creation is currently stopped.")
                AddLoc("ChartReset", "Status: Chart has been reset.")
                AddLoc("ChartDone", "Status: Chart done, next is in")
                AddLoc("ChartRedraw", "Status: Chart redraw in")
                AddLoc("ChartCount", "seconds")
                AddLoc("Hostname", "Hostname")
                AddLoc("Uptime", "Uptime")
                AddLoc("Date", "Date")
                AddLoc("Days", "days")
                AddLoc("Hours", "hours")
                AddLoc("Mins", "minutes")
                AddLoc("Secs", "seconds")
                AddLoc("And", "and")
                AddLoc("Note", "Notification")
                AddLoc("Taskbar", "The process is still running in the background.")
                AddLoc("ImageSaved", "Image saved")
                AddLoc("UnknownDisk", "Unknown disk drive")
                AddLoc("NoDisk", "No disk")
                AddLoc("NoName", "No name")
                AddLoc("NotAvailable", "Not available")
                AddLoc("NotInstalled", "Not installed")
                AddLoc("Unknown", "Unknown")
                AddLoc("None", "None")
                AddLoc("Invalid", "Invalid")
                AddLoc("Inactive", "Inactive")
                AddLoc("Enabled", "Enabled")
                AddLoc("Disabled", "Disabled")

                ' Üzenetdoboz változók
                AddLoc("MsgInterfaceTitle", "Interface is missing")
                AddLoc("MsgInterfaceText", "The selected interface has been removed or disabled, the interface list will be updated!")
                AddLoc("MsgDiskTitle", "Disk is missing")
                AddLoc("MsgDiskText", "The selected disk has been removed, the disk list will be updated!")
                AddLoc("MSgQuitTitle", "Exit confirmation")
                AddLoc("MsgQuitText", "Do you really want to quit?")

                ' Betöltési változók
                AddLoc("LoadDebug", "Load failure")
                AddLoc("LoadRegistry", "Registry")
                AddLoc("LoadLanguage", "Language")
                AddLoc("LoadDefaults", "Defaults")
                AddLoc("LoadHostname", "Hostname")
                AddLoc("LoadUptime", "Uptime")
                AddLoc("LoadHardware", "Hardware")
                AddLoc("LoadProcessor", "Processor")
                AddLoc("LoadDisk", "Disk")
                AddLoc("LoadVideo", "Video card")
                AddLoc("LoadNetwork", "Network")

                ' Külső nyelvi sztingek -> Splash ablak
                AddLoc("SplashLoad", "Loading")
                AddLoc("SplashClose", "Close")

                ' Külső nyelvi sztingek -> S.M.A.R.T ablak
                AddLoc("SmartTitle", "S.M.A.R.T information")
                AddLoc("SmartStatus", "Status")
                AddLoc("SmartNumber", "#")
                AddLoc("SmartRaw", "Raw data")
                AddLoc("SmartConvert", "Converted")
                AddLoc("SmartOK", "OK")
                AddLoc("SmartWarning", "Warning")
                AddLoc("SmartCritical", "Critical")
                AddLoc("SmartTable", "Disk")
                AddLoc("SmartRecord", "Record")
                AddLoc("SmartTreshold", "Treshold")
                AddLoc("SmartValue", "Value")
                AddLoc("SmartWorst", "Worst")
                AddLoc("SmartData", "Data")

                ' Külső nyelvi sztingek -> CPU-infó ablak
                AddLoc("CPUTitle", "Processor information")
                AddLoc("CPUTable", "CPU")
                AddLoc("CPUDescription", "Description")
                AddLoc("CPUValue", "Value")
                AddLoc("CPUVendor", "Vendor")
                AddLoc("CPUName", "Name")
                AddLoc("CPUIdent", "Identifier")
                AddLoc("CPUCores", "Number of cores")
                AddLoc("CPUThreads", "Number of threads")
                AddLoc("CPUSocket", "Socket")
                AddLoc("CPUVoltage", "Core voltage")
                AddLoc("CPUArchitect", "Architecture")
                AddLoc("CPUCurrentSpeed", "Current clock")
                AddLoc("CPUMaxSpeed", "Maximum clock")
                AddLoc("CPUBusClock", "Bus clock")
                AddLoc("CPUL2", "Size of L2 cache")
                AddLoc("CPUL3", "Size of L3 cache")

                ' Külső nyelvi sztingek -> IP-infó ablak
                AddLoc("IPTitle", "Interface configuration")
                AddLoc("IPTable", "Interface")
                AddLoc("IPDescription", "Description")
                AddLoc("IPValue", "Value")
                AddLoc("IPAdapter", "Adapter")
                AddLoc("IPMACAddr", "MAC address")
                AddLoc("IPDHCPClient", "DHCP client")
                AddLoc("IPDHCPServer", "DHCP server")
                AddLoc("IPDHCPStart", "DHCP lease start")
                AddLoc("IPDHCPEnd", "DHCP lease end")
                AddLoc("IPDHCPInt", "DHCP lease interval")
                AddLoc("IPAddress", "IP address")
                AddLoc("IPGateway", "Gateway")
                AddLoc("IPMetric", "Metric")
                AddLoc("IPDNS", "DNS server")
                AddLoc("IPDomain", "DNS domain")
                AddLoc("IPWINS", "WINS server")
                AddLoc("IPNetBIOS", "NetBIOS over TCP/IP")

                ' ToolTip sztringek
                AddLoc("Tip_Language", "Language selection")
                AddLoc("Tip_HW", "Component selection")
                AddLoc("Tip_CPUInfo", "Details...")
                AddLoc("Tip_Smart", "Open S.M.A.R.T table...")
                AddLoc("Tip_IPInfo", "Open configuration...")
                AddLoc("Tip_Reload", "Reload list")
                AddLoc("Tip_Chart", "Traffic history chart")
                AddLoc("Tip_Average", "seconds averages")
                AddLoc("Tip_ChartDown", "Enable/disable download chart rendering (Alt + D)")
                AddLoc("Tip_ChartUp", "Enable/disable upload chart rendering (Alt + U)")
                AddLoc("Tip_Refresh", "Refresh interval selection (seconds)")
                AddLoc("Tip_Exit", "Exit (Alt + X)")
                AddLoc("Tip_LinkBottom", "About...")
                AddLoc("Tip_Hostname", "Computer name")
                AddLoc("Tip_Uptime", "System uptime")
                AddLoc("Tip_Status", "Status of the chart creation process")
                AddLoc("Tip_TopMost", "Always on top (Green = enabled, Red = disabled)")
                AddLoc("Tip_Screenshot", "Take screenshot")

                ' Kiírások
                AddLoc("HWList", "Component:")
                AddLoc("HWVendor", "Vendor:")
                AddLoc("HWIdent", "Identifier:")
                AddLoc("CPUList", "Processor:")
                AddLoc("CPUCore", "Cores / Threads:")
                AddLoc("CPUClock", "Clock:")
                AddLoc("CPUMaximum", "Maximum:")
                AddLoc("PhyMemSize", "Physical memory:")
                AddLoc("PhyMemUsed", "Usage:")
                AddLoc("PhyMemFree", "Free:")
                AddLoc("VirtMemSize", "Virtual memory:")
                AddLoc("VirtMemUsed", "Usage:")
                AddLoc("VirtMemFree", "Free:")
                AddLoc("OSName", "Product:")
                AddLoc("OSVersion", "Version:")
                AddLoc("OSLang", "Language:")
                AddLoc("OSRelease", "Release:")
                AddLoc("DiskList", "Disk drive:")
                AddLoc("MediaType", "Media type:")
                AddLoc("DiskInterface", "Interface:")
                AddLoc("DiskFirmware", "Firmware:")
                AddLoc("PartList", "Volume:")
                AddLoc("DiskSerial", "Serial number:")
                AddLoc("VideoList", "Graphics card:")
                AddLoc("VideoMemory", "Video memory:")
                AddLoc("VideoResolution", "Resolution:")
                AddLoc("InterfaceList", "Interface:")
                AddLoc("Bandwidth", "Bandwidth:")
                AddLoc("InterfaceUsage", "Interface usage:")
                AddLoc("DownloadSpeed", "Download speed:")
                AddLoc("UploadSpeed", "Upload speed:")
                AddLoc("ChartVisible", "Chart visibility:")
                AddLoc("UpdateList", "Update interval:")
                AddLoc("UpdateUnit", "s")

                ' Csoportfeliratok
                AddLoc("GB_HWInfo", "Computer system information")
                AddLoc("GB_CPUInfo", "Processor information")
                AddLoc("GB_MemoryInfo", "Memory information")
                AddLoc("GB_OSInfo", "Operating system information")
                AddLoc("GB_DiskInfo", "Disk information")
                AddLoc("GB_VideoInfo", "Display controller information")
                AddLoc("GB_Network", "Network interface statistics")

                ' Menüelemek
                AddLoc("Menu_Settings", "&Settings")
                AddLoc("Menu_Chart", "&Chart")
                AddLoc("Menu_Information", "&Action")
                AddLoc("Menu_TopMost", "Always on &top")
                AddLoc("Menu_TaskbarMinimize", "&Minimize to taskbar")
                AddLoc("Menu_DisableConfirm", "Disable exit &confirmation")
                AddLoc("Menu_DisableSplash", "Disable loading &screen")
                AddLoc("Menu_UpdateCheck", "Check for u&pdates...")
                AddLoc("Menu_About", "A&bout...")
                AddLoc("Menu_Exit", "E&xit")
                AddLoc("Menu_DownloadVisible", "Show &download chart")
                AddLoc("Menu_UploadVisible", "Show &upload chart")
                AddLoc("Menu_SaveChart", "&Save chart image to desktop")
                AddLoc("Menu_ClearChart", "&Clear chart")

                ' Checkbox feliratok
                AddLoc("CB_DownloadVisible", "&Download")
                AddLoc("CB_UploadVisible", "&Upload")

                ' Gombfeliratok
                AddLoc("Button_Exit", "E&xit")
                AddLoc("Button_Close", "&Close")

            Case 1 ' *** NYELV: Magyar kiírások (1) ***

                LocDayName = {"vasárnap", "hétfő", "kedd", "szerda", "csütörtök", "péntek", "szombat"}
                LocMonthName = {"január", "február", "március", "április", "május", "június", "július",
                                "augusztus", "szeptember", "október", "november", "december"}

                ' Főablak nyelvi változói
                AddLoc("Title", "Rendszerinformációk és hálózatfigyelés")
                AddLoc("Comment", "Ez a szoftver nyílt forrású és hordozható.")
                AddLoc("Version", "Verziószám")
                AddLoc("DigitSeparator", ",")
                AddLoc("ScaleSeparator", " ")
                AddLoc("Interval", "Intervallumok")
                AddLoc("Traffic", "Forgalom")
                AddLoc("Motherboard", "Alaplap")
                AddLoc("System", "Számítógép")
                AddLoc("BIOS", "BIOS")
                AddLoc("Battery", "Akkumulátor")
                AddLoc("Serial", "Sorozatszám")
                AddLoc("Volt", "Névleges feszültség")
                AddLoc("SvcPack", "Szervizcsomag")
                AddLoc("Interface", "Interfész")
                AddLoc("ChartTime", "Diagram elkészítve:")
                AddLoc("Time", "Idő")
                AddLoc("Update", "Frissítés")
                AddLoc("Current", "Jelenlegi")
                AddLoc("Peak", "Csúcs")
                AddLoc("DateFormat", "yyyy. MMMM d. dddd, H:mm:ss")
                AddLoc("ChartDown", "Letöltési sebesség")
                AddLoc("ChartUp", "Feltöltési sebesség")
                AddLoc("ChartHide", "A diagram leképezés ki van kapcsolva.")
                AddLoc("ChartStop", "Állapot: A diagram leképezés jelenleg le van állítva.")
                AddLoc("ChartReset", "Állapot: A diagram alaphelyzetbe lett állítva.")
                AddLoc("ChartDone", "Állapot: Diagram kész, következő")
                AddLoc("ChartRedraw", "Állapot: Diagram újrarajzolása")
                AddLoc("ChartCount", "másodperc múlva")
                AddLoc("Hostname", "Hosztnév")
                AddLoc("Uptime", "Futási idő")
                AddLoc("Date", "Dátum")
                AddLoc("Days", "nap")
                AddLoc("Hours", "óra")
                AddLoc("Mins", "perc")
                AddLoc("Secs", "másodperc")
                AddLoc("And", "és")
                AddLoc("Note", "Értesítés")
                AddLoc("Taskbar", "A folyamat továbbra is fut a háttérben.")
                AddLoc("ImageSaved", "Kép elmentve")
                AddLoc("UnknownDisk", "Ismeretlen lemezmeghajtó")
                AddLoc("NoDisk", "Nincs lemez")
                AddLoc("NoName", "Névtelen")
                AddLoc("NotAvailable", "Nem elérhető")
                AddLoc("NotInstalled", "Nincs telepítve")
                AddLoc("Unknown", "Ismeretlen")
                AddLoc("None", "Nincs")
                AddLoc("Invalid", "Érvénytelen")
                AddLoc("Inactive", "Inaktív")
                AddLoc("Enabled", "Engedélyezve")
                AddLoc("Disabled", "Letiltva")

                ' Üzenetdoboz változók
                AddLoc("MsgInterfaceTitle", "Hiányzó interfész")
                AddLoc("MsgInterfaceText", "A kiválasztott interfészt eltávolították vagy le lett tiltva, az interfész lista frissítve lesz!")
                AddLoc("MsgDiskTitle", "Hiányzó lemez")
                AddLoc("MsgDiskText", "A kiválasztott lemezt eltávolították, a lemezlista frissítve lesz!")
                AddLoc("MsgQuitTitle", "Kilépés megerősítése")
                AddLoc("MsgQuitText", "Valóban ki szeretne lépni?")

                ' Betöltési változók
                AddLoc("LoadDebug", "Betöltési hiba")
                AddLoc("LoadRegistry", "Registry")
                AddLoc("LoadLanguage", "Nyelv")
                AddLoc("LoadDefaults", "Alapértékek")
                AddLoc("LoadHostname", "Hosztnév")
                AddLoc("LoadUptime", "Futásidő")
                AddLoc("LoadHardware", "Hardver")
                AddLoc("LoadProcessor", "Processzor")
                AddLoc("LoadDisk", "Lemez")
                AddLoc("LoadVideo", "Videokártya")
                AddLoc("LoadNetwork", "Hálózat")

                ' Külső nyelvi sztingek -> Splash ablak
                AddLoc("SplashLoad", "Betöltés")
                AddLoc("SplashClose", "Bezárás")

                ' Külső nyelvi sztingek -> S.M.A.R.T ablak
                AddLoc("SmartTitle", "S.M.A.R.T információk")
                AddLoc("SmartStatus", "Állapot")
                AddLoc("SmartNumber", "#")
                AddLoc("SmartRaw", "Nyers adat")
                AddLoc("SmartConvert", "Konvertálva")
                AddLoc("SmartOK", "Rendben")
                AddLoc("SmartWarning", "Figyelmeztetés")
                AddLoc("SmartCritical", "Kritikus")
                AddLoc("SmartTable", "Lemez")
                AddLoc("SmartRecord", "Rekord")
                AddLoc("SmartTreshold", "Küszöb")
                AddLoc("SmartValue", "Érték")
                AddLoc("SmartWorst", "Legrosszabb")
                AddLoc("SmartData", "Adat")
                AddLoc("SmartClose", "&Bezárás")

                ' Külső nyelvi sztingek -> CPU-infó ablak
                AddLoc("CPUTitle", "Processzor információk")
                AddLoc("CPUTable", "CPU")
                AddLoc("CPUDescription", "Megnevezés")
                AddLoc("CPUValue", "Érték")
                AddLoc("CPUVendor", "Gyártó")
                AddLoc("CPUName", "Név")
                AddLoc("CPUIdent", "Azonosító")
                AddLoc("CPUCores", "Magok száma")
                AddLoc("CPUThreads", "Logikai szálak száma")
                AddLoc("CPUSocket", "Tokozás")
                AddLoc("CPUVoltage", "Magfeszültség")
                AddLoc("CPUArchitect", "Architectúra")
                AddLoc("CPUCurrentSpeed", "Jelenlegi órajel")
                AddLoc("CPUMaxSpeed", "Maximális órajel")
                AddLoc("CPUBusClock", "Busz órajel")
                AddLoc("CPUL2", "L2 gyorsítótár mérete")
                AddLoc("CPUL3", "L3 gyorsítótár mérete")

                ' Külső nyelvi sztingek -> IP-infó ablak
                AddLoc("IPTitle", "Interfész beállítások")
                AddLoc("IPTable", "Interfész")
                AddLoc("IPDescription", "Megnevezés")
                AddLoc("IPValue", "Érték")
                AddLoc("IPAdapter", "Adapter")
                AddLoc("IPMACAddr", "Fizikai cím")
                AddLoc("IPDHCPClient", "DHCP ügyfél")
                AddLoc("IPDHCPServer", "DHCP kiszolgáló")
                AddLoc("IPDHCPStart", "DHCP bérlet kezdete")
                AddLoc("IPDHCPEnd", "DHCP bérlet vége")
                AddLoc("IPAddress", "IP-cím")
                AddLoc("IPGateway", "Átjáró")
                AddLoc("IPMetric", "Metrika")
                AddLoc("IPDNS", "DNS kiszolgáló")
                AddLoc("IPDomain", "DNS tartomány")
                AddLoc("IPWINS", "WINS kiszolgáló")
                AddLoc("IPNetBIOS", "NetBIOS TCP/IP felett")

                ' ToolTip sztringek
                AddLoc("Tip_Language", "Nyelv kiválasztása")
                AddLoc("Tip_HW", "Komponens kiválasztása")
                AddLoc("Tip_CPUInfo", "Részletek...")
                AddLoc("Tip_Smart", "S.M.A.R.T tábla megnyitása...")
                AddLoc("Tip_IPInfo", "Beállítások megnyitása...")
                AddLoc("Tip_Reload", "Lista újratöltése")
                AddLoc("Tip_Chart", "Adatforgalmi előzmények diagramja")
                AddLoc("Tip_Average", "másodperces átlagok")
                AddLoc("Tip_ChartDown", "Letöltési diagram leképezésének engedélyezése/tiltása (Alt + L)")
                AddLoc("Tip_ChartUp", "Feltöltési diagram leképezésének engedélyezése/tiltása (Alt + F)")
                AddLoc("Tip_Refresh", "Frissítési időköz kiválasztása (másodperc)")
                AddLoc("Tip_Exit", "Kilépés (Alt + K)")
                AddLoc("Tip_LinkBottom", "Névjegy...")
                AddLoc("Tip_Hostname", "Számítógépnév")
                AddLoc("Tip_Uptime", "Rendszer futási ideje")
                AddLoc("Tip_Status", "A diagram jelenlegi állapota")
                AddLoc("Tip_TopMost", "Mindig látható (zöld = engedélyezve, piros = tiltva)")
                AddLoc("Tip_Screenshot", "Képernyőkép készítése")

                ' Kiírások
                AddLoc("HWList", "Komponens:")
                AddLoc("HWVendor", "Gyártó:")
                AddLoc("HWIdent", "Azonosító:")
                AddLoc("CPUList", "Processzor:")
                AddLoc("CPUCore", "Magok / Szálak:")
                AddLoc("CPUClock", "Órajel:")
                AddLoc("CPUMaximum", "Maximum:")
                AddLoc("PhyMemSize", "Fizikai memória:")
                AddLoc("PhyMemUsed", "Foglaltság:")
                AddLoc("PhyMemFree", "Szabad:")
                AddLoc("VirtMemSize", "Virtuális memória:")
                AddLoc("VirtMemUsed", "Foglaltság:")
                AddLoc("VirtMemFree", "Szabad:")
                AddLoc("OSName", "Termék:")
                AddLoc("OSVersion", "Verzió:")
                AddLoc("OSLang", "Nyelv:")
                AddLoc("OSRelease", "Kiadás:")
                AddLoc("DiskList", "Meghajtó:")
                AddLoc("MediaType", "Lemez típusa:")
                AddLoc("DiskInterface", "Interfész:")
                AddLoc("DiskFirmware", "Firmware:")
                AddLoc("PartList", "Kötet:")
                AddLoc("DiskSerial", "Sorozatszám:")
                AddLoc("VideoList", "Videokártya:")
                AddLoc("VideoMemory", "Videomemória:")
                AddLoc("VideoResolution", "Felbontás:")
                AddLoc("InterfaceList", "Interfész:")
                AddLoc("Bandwidth", "Sávszélesség:")
                AddLoc("InterfaceUsage", "Interfész kihasználtsága:")
                AddLoc("DownloadSpeed", "Letöltési sebesség:")
                AddLoc("UploadSpeed", "Feltöltés sebesség:")
                AddLoc("ChartVisible", "Diagramok:")
                AddLoc("UpdateList", "Frissítési időköz:")
                AddLoc("UpdateUnit", "s")

                ' Csoportfeliratok
                AddLoc("GB_HWInfo", "Számítógép rendszer információk")
                AddLoc("GB_CPUInfo", "Processzor információk")
                AddLoc("GB_MemoryInfo", "Memória információk")
                AddLoc("GB_OSInfo", "Operációs rendszer információk")
                AddLoc("GB_DiskInfo", "Meghajtó információk")
                AddLoc("GB_VideoInfo", "Videovezérlő információk")
                AddLoc("GB_Network", "Hálózati interfész statisztika")

                ' Menüelemek
                AddLoc("Menu_Settings", "&Beállítások")
                AddLoc("Menu_Chart", "&Diagram")
                AddLoc("Menu_Information", "&Műveletek")
                AddLoc("Menu_TopMost", "Mi&ndig látható")
                AddLoc("Menu_TaskbarMinimize", "Kicsinyítés a &rendszerikonok közé")
                AddLoc("Menu_DisableConfirm", "&Kilépési megerősítés kikapcsolása")
                AddLoc("Menu_DisableSplash", "&Betöltő képernyő elrejtése")
                AddLoc("Menu_UpdateCheck", "&Frissítések keresése...")
                AddLoc("Menu_About", "&Névjegy...")
                AddLoc("Menu_Exit", "&Kilépés")
                AddLoc("Menu_DownloadVisible", "&Letöltési diagram mutatása")
                AddLoc("Menu_UploadVisible", "&Feltöltési diagram mutatása")
                AddLoc("Menu_SaveChart", "Diagram &mentése az asztalra")
                AddLoc("Menu_ClearChart", "Diagram &törlése")

                ' Checkbox feliratok
                AddLoc("CB_DownloadVisible", "&Letöltés")
                AddLoc("CB_UploadVisible", "&Feltöltés")

                ' Gombfeliratok
                AddLoc("Button_Exit", "&Kilépés")
                AddLoc("Button_Close", "&Bezárás")

        End Select

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Nyelvi változók tömbjének feltölése ***
    ' Bemenet: Name  -> nyelvi változó neve (String)
    '          Value -> nyelvi változó értéke (String)
    ' Kimenet: *     -> hamis érték (Boolean)
    Public Shared Function AddLoc(ByVal Name As String, ByVal Value As String)

        ' Értékek definiálása
        Dim LocCount As Int32 = UBound(LocRef)                              ' Nyelvi tömbök elemszáma
        Dim LocPosition As Int32 = 0                                        ' Keresési ciklus sorszáma
        Dim LocFind As Boolean = False                                      ' Keresés eredménye

        ' Tömb ellenőrzése
        If LocCount = 0 Then

            ' Üres tömb első értékének hozzáadása
            Array.Resize(LocRef, LocRef.Length + 1)
            Array.Resize(LocStr, LocStr.Length + 1)
            LocRef(LocPosition) = Name
            LocStr(LocPosition) = Value
        Else

            ' A tömb végnézése
            For LocPosition = 0 To LocCount - 1

                ' Ha van egyező nevű, akkor felülírja (és megszakítja a ciklust)
                If LocRef(LocPosition) = Name Then
                    LocFind = True
                    LocStr(LocPosition) = Value
                    LocPosition = LocCount - 1
                End If
            Next

            ' Ha nincs egyezés, akkor hozzáad egy új értéket
            If LocFind = False Then
                Array.Resize(LocRef, LocRef.Length + 1)
                Array.Resize(LocStr, LocStr.Length + 1)
                LocRef(LocCount) = Name
                LocStr(LocCount) = Value
            End If
        End If

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Nyelvi változó értékének lekérdezése ***
    ' Bemenet: Name  -> nyelvi változó neve (String)
    ' Kimenet: Value -> nyelvi változó értéke (String)
    Public Shared Function GetLoc(ByVal Name As String)

        ' Értékek definiálása
        Dim Value As String = Nothing                                       ' Nyelvi változó értéke
        Dim LocCount As Int32 = UBound(LocRef)                              ' Nyelvi tömbök elemszáma
        Dim LocPosition As Int32 = 0                                        ' Keresési ciklus sorszáma

        ' A tömb végnézése
        For LocPosition = 0 To LocCount

            ' Ha van egyezés, akkor beállítja az értéket
            If LocRef(LocPosition) = Name Then
                Value = LocStr(LocPosition)
            End If
        Next

        ' Visszatérési érték beállítása
        Return Value

    End Function

    ' *** FÜGGVÉNY: Nyelvi dátum és időformátum konverzió ***
    ' Bemenet: InputDate -> dátum és idő (DateTime)
    ' Kimenet: Converted -> formázott sztring (String)
    Public Shared Function GetLocalizedDate(ByVal InputDate As DateTime)

        ' Értékek definiálása
        Dim Converted As String = GetLoc("DateFormat")
        Dim ConvCount As Int32
        Dim SourceFormat() = {"yyyy", "MMMM", "dddd", "dd", "d", "H:mm:ss", "h:mm:ss", "tt"}
        ' Megjegyzés: A nap neve védett formátum, mivel a 'd' önmagában más formátumot takar. Ezért kell bele egy szóköz, amelyet utólag el kell távolítani!
        Dim TargetFormat() = {Format(InputDate, "yyyy"), "###", "##", Format(InputDate, "dd"), RemoveSpaces(Format(InputDate, " d")),
                              Format(InputDate, "H:mm:ss"), Format(InputDate, "h:mm:ss"), "#"}

        ' Ismert formátumok lecserélése valódi értékekre
        For ConvCount = 0 To UBound(SourceFormat)
            Converted = Replace(Converted, SourceFormat(ConvCount), TargetFormat(ConvCount))
        Next

        ' Védett szöveges formátum cseréje: Hónap neve, nap neve, napszak (AM/PM)
        Converted = Replace(Converted, "###", LocMonthName(InputDate.Month - 1))
        Converted = Replace(Converted, "##", LocDayName(InputDate.DayOfWeek))
        Converted = Replace(Converted, "#", InputDate.ToString("tt", System.Globalization.CultureInfo.InvariantCulture))

        ' Visszatérési érték beállítása
        Return Converted

    End Function

    ' *** FÜGGVÉNY: Számformátumra alakítás: 1000-es elválaszó és tizedesvessző javítása (területi beállítás felülbírálása) ***
    ' Bemenet: Value      -> módosítatlan tizedestört érték (Double)
    '          Digit      -> elválasztó utáni helyiértékek száma (Int32)
    '          FloatFract -> lebegő (True) vagy statikus (False) törtformátum (Boolean)
    ' Kimenet: ConvString -> formázott tizedestört (String)
    Public Shared Function FixNumberFormat(ByVal Value As Double, ByVal Digit As Int32, ByVal FloatFract As Boolean)

        ' Értékek definiálása
        Dim IntValue, ConvString As String
        Dim IntString As String = Nothing
        Dim IntDigits() As Char
        Dim IntLength, IntCount As Int32
        Dim ConvFract As Double
        Dim FractString As String = Nothing

        ' Egész érték beállítása
        IntValue = Fix(Value).ToString

        ' Helyiértékek számának meghatározása
        IntLength = IntValue.Length

        ' Egész érték karakterekre bontása
        IntDigits = IntValue.ToCharArray

        ' Egész értékek összefűzése karakterenként
        For IntCount = 0 To IntLength - 1
            If (IntLength - IntCount) Mod 3 = 0 And IntCount <> 0 Then
                IntString += GetLoc("ScaleSeparator") + IntDigits(IntCount)
            Else
                IntString += IntDigits(IntCount)
            End If
        Next

        ' Tizedes elválasztó ellenőrzése -> Ha nincs helyiérték, akkor nem kell!
        If Digit <> 0 Then
            FractString = GetLoc("DigitSeparator")
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

End Class