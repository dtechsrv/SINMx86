Imports System.Math
Imports System.Convert
Imports System.Management
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Imports SINMx86.Localization

' Függvények osztálya
Public Class Functions

    ' Alkalmazás adatai
    Public Shared MyVersion As String = Application.ProductVersion                          ' Saját verziószám
    Public Shared MyName As String = My.Application.Info.Title                              ' Program neve
    Public Shared MyLink As String = My.Application.Info.Description + "/releases/latest"   ' Támogatási link
    Public Shared ReleaseStatus As String = Nothing                                         ' Kiadás állapota ('BETA', 'RC', vagy stabil verzió esetén üres)
    Public Shared VersionString As String = SetMainVersion()                                ' Formázott verziószám

    ' További változók
    Public Shared SIPrefix() As String = {"", "k", "M", "G", "T", "P", "E"}                 ' SI prefixumok tömbje (nagyságrend váltás: 10^3 = 1000)
    Public Shared BytePrefix() As String = {"", "ki", "Mi", "Gi", "Ti", "Pi", "Ei"}         ' Bináris prefixumok tömbje (nagyságrend váltás: 2^10 = 1024)
    Public Shared RefreshInterval() As Int32 = {1, 2, 3, 4, 5, 10, 15, 30, 60}              ' Frissítési intervallumok
    Public Shared MainWindowDone As Boolean                                                 ' A főablak betöltési állapota
    Public Shared OSVersion() As Int32 = GetOSVersion()                                     ' Operációs rendszer verziószám
    Public Shared DiskSmart(32) As String                                                   ' Meghajtó S.M.A.R.T azonosítója (ha van, egyékbént üres)

    ' Beállításjegyzék változói
    Public Shared RegPath As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\\" + MyName, True)

    ' Checkboxok és menüelemek változói
    Public Shared CheckedSplashDisable, CheckedTopMost, CheckedNoQuitAsk, CheckedMinToTray As Boolean

    ' Listák kiválasztott elemeinek sorszáma
    Public Shared SelectedRefresh, SelectedHardware, SelectedCPU, SelectedDisk, SelectedPartition, SelectedVideo, SelectedInterface As Int32

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVÉNY: Betöltési állapot beállítása ***
    ' Bemenet: Stage -> Betöltés állapota (String)
    ' Kimenet: *     -> hamis érték (Boolean)
    Public Shared Function DebugLoadStage(ByVal Stage As String)

        ' Betöltési üzenet beállítása a Splash ablakon és a Debug változóban a főablakban.
        LoadSplash.Splash_Status.Text = GetLoc("SplashLoad") + ": " + Stage + "..."
        MainWindow.Value_Debug.Text = GetLoc("LoadDebug") + ": " + Stage

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Registry beállítások lekérdezése ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Public Shared Function GetRegValues()

        ' Alapértelmezett értékek beállítása (Ismeretlen érték vagy üres registryváltozó esetén)
        Dim DefaultRefresh As Int32 = 2                                             ' Frissítési időköz: 3 másodperc

        ' *** REGISTRY LEKÉRDEZÉS: Regisztrációs kulcs létrehozása, ha nem létezik (HKCU\Software) ***
        If RegPath Is Nothing Then
            RegPath = Registry.CurrentUser.CreateSubKey("Software\\" + MyName, RegistryKeyPermissionCheck.ReadWriteSubTree)
        End If

        ' Registry lekérdezés: Utolsó beállított értékek lekérdezése
        ' Megjegyzés: a beállított számérték sztringként tér vissza, ha nem létezik, akkor üres lesz!
        Dim ReadLanguage As String = RegPath.GetValue("SelectedLanguage")           ' Nyelv beállítása
        Dim ReadSplash As String = RegPath.GetValue("DisableLoadSplash")            ' Splash Screen megjelenítése
        Dim ReadRefresh As String = RegPath.GetValue("SelectedRefreshIndex")        ' Frissítési időköz
        Dim ReadTopMost As String = RegPath.GetValue("EnableTopMost")               ' Láthatóság
        Dim ReadMinToTray As String = RegPath.GetValue("MinimizeToTaskbar")         ' Kicsinyítés állapota
        Dim ReadNoQuitAsk As String = RegPath.GetValue("DisableExitConfirmation")   ' Kilépési megerősítés

        ' Függő változó beállítása: Nyelv kiválasztása (SelectedLanguage)
        If ReadLanguage <> Nothing And ReadLanguage <= UBound(Languages) Then
            SelectedLanguage = ToInt32(ReadLanguage)
        Else
            SelectedLanguage = 0
        End If

        ' Függő változó beállítása: Splash Screen elrejtése indításkor
        If ReadSplash Is Nothing Or ToInt32(ReadSplash) = 0 Then
            CheckedSplashDisable = False
        Else
            CheckedSplashDisable = True
        End If

        ' Függő változó beállítása: Frissítési időköz
        If ReadRefresh Is Nothing Or ToInt32(ReadRefresh) > UBound(RefreshInterval) Then
            SelectedRefresh = DefaultRefresh
        Else
            SelectedRefresh = ToInt32(ReadRefresh)
        End If

        ' Függő változó beállítása: Állandó láthatóság ellenőrzése
        If ReadTopMost Is Nothing Or ToInt32(ReadTopMost) = 0 Then
            CheckedTopMost = False
        Else
            CheckedTopMost = True
        End If

        ' Függő változó beállítása: Kicsinyítés a rendszerikonok közé
        If ReadMinToTray Is Nothing Or ToInt32(ReadMinToTray) = 0 Then
            CheckedMinToTray = False
        Else
            CheckedMinToTray = True
        End If

        ' Függő változó beállítása: Kilépési megerősítés kiírásának tiltása
        If ReadNoQuitAsk Is Nothing Or ToInt32(ReadNoQuitAsk) <> 1 Then
            CheckedNoQuitAsk = False
        Else
            CheckedNoQuitAsk = True
        End If

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Registry beállítások mentése ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> hamis érték (Boolean)
    Public Shared Function SetRegValues()

        ' Registry értékek mentése
        RegPath.SetValue("DisableLoadSplash", ToInt32(CheckedSplashDisable), RegistryValueKind.DWord)
        RegPath.SetValue("SelectedLanguage", SelectedLanguage, RegistryValueKind.DWord)
        RegPath.SetValue("SelectedRefreshIndex", SelectedRefresh, RegistryValueKind.DWord)
        RegPath.SetValue("EnableTopMost", ToInt32(CheckedTopMost), RegistryValueKind.DWord)
        RegPath.SetValue("DisableExitConfirmation", ToInt32(CheckedNoQuitAsk), RegistryValueKind.DWord)
        RegPath.SetValue("MinimizeToTaskbar", ToInt32(CheckedMinToTray), RegistryValueKind.DWord)

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Saját verziószám beállítása ***
    ' Bemenet: * -> üres (Void)
    ' Kimenet: * -> formázott érték (String): 'Major.Minor.Sub Release-status (Build)'
    Public Shared Function SetMainVersion()

        ' Veriószám felbontása (Build-del)
        Dim VersionArray() As String = Split(MyVersion, ".")                        ' Verziószámok elemeinek tömbje (Major, Minor, Sub, Build)
        ReDim Preserve VersionArray(0 To 3)

        ' Alverzió ellenőrzés: BETA (900+) és RC (500+) tagek ellenőrzése
        Dim SubVersion As Int32 = ToInt32(VersionArray(2))                          ' Alverzió

        ' Alverzió és kiadási állapot módosítása (Pl. 901 = 'vx.y.1 BETA', 502 = 'vx.y.1 RC')
        If SubVersion >= 900 And SubVersion < 1000 Then
            ReleaseStatus = "BETA"
            VersionArray(2) = (SubVersion Mod 900).ToString + " " + ReleaseStatus
        ElseIf SubVersion >= 500 Then
            ReleaseStatus = "RC"
            VersionArray(2) = (SubVersion Mod 500).ToString + " " + ReleaseStatus
        End If

        ' Visszatérési érték beállítása
        Return VersionArray(0) + "." + VersionArray(1) + "." + VersionArray(2) + " (Build " + VersionArray(3) + ")"

    End Function

    ' Bemenet: *            -> üres (Void)
    ' Kimenet: OSVerArray() -> formázott érték (In32): Főverzió, Alverzió, Build
    Public Shared Function GetOSVersion()

        ' Értékek definiálása
        Dim objOS As ManagementObjectSearcher
        Dim objMgmt As ManagementObject

        ' WMI értékek lekérdezése: Win32_ComputerSystem -> Operációs rendszer információi
        objOS = New ManagementObjectSearcher("SELECT Version FROM Win32_OperatingSystem")

        ' Értékek definiálása
        Dim VersionString() As String                                               ' Verzió sztring tagolt tömbje
        Dim VersionCount As Int32                                                   ' Verziószám tagolási sorszám

        ' Értékek beállítása -> Számítógép: OS verziószám
        For Each objMgmt In objOS.Get()
            VersionString = Split(objMgmt("Version"), ".")
        Next

        ' OS fő- és alverzió beállítása
        ReDim Preserve VersionString(0 To 2)

        ' Függő változók definiálása
        Dim OSVerArray(UBound(VersionString)) As Int32                               ' Verziószám tagolt tömbje

        ' Konvertálás integerré
        For VersionCount = 0 To UBound(VersionString)
            OSVerArray(VersionCount) = ToInt32(VersionString(VersionCount))
        Next

        ' Visszatérési érték beállítása
        Return OSVerArray

    End Function

    ' *** FÜGGVÉNY: Felesleges szóközök eltávolítása ***
    ' Bemenet: RawString -> formázandó sztring (String)
    ' Kimenet: RawString -> formázott sztring (String)
    Public Shared Function RemoveSpaces(ByVal RawString As String)

        ' Értékek definiálása
        Dim Str2Char() As Char                                  ' Sztring-karakter konverzió tömbje
        Dim TempString As String = Nothing                      ' Ideiglenes sztring az elemzéshez

        ' Dupla szóközök eltávolítása, addig amíg szerepel benne! (Ezért kell a While!)
        While (InStr(RawString, "  "))
            RawString = Replace(RawString, "  ", " ")
        End While

        ' Kezdőszóköz eltávolítása
        If RawString <> Nothing Then

            ' Karaktertömbre bontás
            Str2Char = RawString.ToCharArray()

            ' Kezdőszóköz eltávolítása
            If Str2Char(0) = " " Then
                For i As Int32 = 1 To UBound(Str2Char)
                    TempString += Str2Char(i)
                Next
                RawString = TempString
            End If

            ' Zárószóköz eltávolítása
            If Str2Char(UBound(Str2Char)) = " " And RawString <> Nothing Then

                ' Ideiglenes sztring kiürítése
                TempString = Nothing

                ' Karaktertömbre bontás (újra)
                Str2Char = RawString.ToCharArray()

                For i As Int32 = 0 To (UBound(Str2Char) - 1)
                    TempString += Str2Char(i)
                Next
                RawString = TempString

            End If
        End If

        ' Visszatérési érték beállítása
        Return RawString

    End Function

    ' *** FÜGGVÉNY: Nem elfogadható karakterek eltávolítása ***
    ' Bemenet: RawString -> formázandó sztring (String)
    ' Kimenet: RawString -> formázott érték (String)
    Public Shared Function RemoveInvalidChars(ByVal RawString As String)

        ' Értékek definiálása
        Dim Str2Char() As Char                                  ' Sztring-karakter konverzió tömbje
        Dim TempString As String = Nothing                      ' Ideiglenes sztring az összefűzéshez

        ' Bemeneti érték formázása
        If RawString <> Nothing Then

            ' Karaktertömbre bontás
            Str2Char = RawString.ToCharArray()

            ' Elemzés: az ismert és elfogadható karakterek kivételével minden egyéb eltávolítása
            For i As Int32 = 0 To UBound(Str2Char)
                TempString += Regex.Replace(Str2Char(i), "[^a-zA-Z0-9 \(\)\[\]\\/\-_~\*?$#&'%+=!|<>{},.:;@]", "")
            Next

            RawString = TempString
        End If

        ' Visszatérési érték beállítása
        Return RawString

    End Function

    ' *** FÜGGVÉNY: Sztring egyezés keresése tömbből ***
    ' Bemenet: RawString -> keresési forrás sztring (String)
    '          ChkArr()  -> keresett elemek tömbje (String)
    '          CSense    -> case sensitive összehasonlítás (Boolean)
    ' Kimenet: *         -> egyezés állapota (Boolean)
    Public Shared Function CheckStrMatch(ByVal RawString As String, ByVal ChkArr() As String, ByVal CSense As Boolean)

        ' Értékek definiálása
        Dim Match As Boolean = False                            ' Egyezés állapota

        ' Egyezés keresés a tömb elemei között
        For i As Int32 = 0 To UBound(ChkArr)

            ' Case insensitive átalakítás
            If CSense = False Then
                If LCase(RawString) = LCase(ChkArr(i)) And Match = False Then
                    Match = True
                End If
            Else
                If RawString = ChkArr(i) And Match = False Then
                    Match = True
                End If
            End If
        Next

        ' Visszatérési érték beállítása
        Return Match

    End Function

    ' *** FÜGGVÉNY: Sztring részlet keresése tömbből ***
    ' Bemenet: RawString -> keresési forrás sztring (String)
    '          ChkArr()  -> keresett elemek tömbje (String)
    '          CSense    -> case sensitive összehasonlítás (Boolean)
    ' Kimenet: *         -> egyezés állapota (Boolean)
    Public Shared Function CheckStrContain(ByVal RawString As String, ByVal ChkArr() As String, ByVal CSense As Boolean)

        ' Értékek definiálása
        Dim Contain As Boolean = False                          ' Egyezés állapota

        ' Egyezés keresés a tömb elemei között
        For i As Int32 = 0 To UBound(ChkArr)

            ' Case insensitive átalakítás
            If CSense = False Then
                If LCase(InStr(RawString, ChkArr(i))) And Contain = False Then
                    Contain = True
                End If
            Else
                If InStr(RawString, ChkArr(i)) And Contain = False Then
                    Contain = True
                End If
            End If
        Next

        ' Visszatérési érték beállítása
        Return Contain

    End Function

    ' *** FÜGGVÉNY: WMI alapú DateTime változó konverzió ***
    ' Bemenet: WMIDateTime -> WMI datetime formátumú változó (String)
    ' Kimenet: Converted   -> hagyományos DateTime változó (DateTime)
    Public Shared Function DateTimeConv(ByVal WMIDateTime As String)

        ' Értékek definiálása
        Dim Converted As DateTime                               ' DateTime változó
        Dim Year, Month, Day, Hour, Minute, Second As Int32     ' Időváltozók (év, hónap, nap, óra, perc, másodperc)

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

    ' *** FÜGGVÉNY: Dinamikus nagyságrendi konverzió ***
    ' Bemenet: Value       -> bájt (Double)
    '          Digit       -> elválasztó utáni helyiértékek száma (Int32)
    '          ByteValue   -> bájt konverzió (Booelan)
    ' Kimenet: ConvValue() -> formázott érték tömbje (Double): Kerekített érték, Prefixum sorszáma
    Public Shared Function ScaleConversion(ByVal Value As Double, ByVal Digit As Int32, ByVal ByteValue As Boolean)

        ' Értékek definiálása
        Dim Prefix As Int32 = 0                                 ' Prefixum sorszáma
        Dim ConvValue(2) As Double                              ' Kovertált érték: érték, prefixum
        Dim Base As Int32                                       ' Nagyságrendi szorzó: SI -> 1000, Byte -> 1024

        ' Bájt vagy sima nagyságrendi konverzió -> A Microsoft nem SI alapján számol a bájtnál! (Valójában ez iB, kiB, MiB, stb. lenne.)
        If ByteValue Then
            Base = 1024
        Else
            Base = 1000
        End If

        ' Prefixum görgetés indítása (Minden hurok eggyel feljebb tolja a tömbben lévő szorzó számát)
        While (Value >= Base)
            Prefix += 1
            Value = Round((Value / Base) * (10 ^ Digit)) / (10 ^ Digit)
        End While

        ' Kimenet formázása
        ConvValue(0) = Round(Value, 2)
        ConvValue(1) = Prefix

        ' Visszatérési érték beállítása
        Return ConvValue

    End Function

End Class