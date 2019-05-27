Imports System.Convert
Imports Microsoft.Win32

Public NotInheritable Class LoadSplash

    ' Külső változók
    Public LoadLanguage As Int32 = 0 ' Betöltési nyelv alapértelmezése
    Public CurrentImage As Int32 = 0 ' Véletlen kép kezdő sorszáma

    ' *** FŐ ELJÁRÁS: Splash ablak betöltése (Me.Load -> LoadSplash) ***
    ' Eseményvezérelt: Betöltőképernyó vagy névjegy meghívása
    Public Sub LoadSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Mindig látható (Fontos, főleg, ha a főablak is az!)
        Me.TopMost = True

        ' Névjegy időzítője (betöltéskor nincs jelentősége)
        SplashTimer.Enabled = True
        SplashTimer.Interval = 10000 ' Betöltés: 10 másodperc (Érték: 10000)

        ' Alkalmazás adatai
        Dim MyVersion As String = Application.ProductVersion   ' Saját verziószám
        Dim MyName As String = My.Application.Info.Title       ' Program neve

        ' Háttérkép generálása
        RandomImage()

        ' Címsor feltöltése
        If MainWindow.ReleaseStatus = Nothing Then
            Splash_Title.Text = MyName
        Else
            Splash_Title.Text = MyName + " " + MainWindow.ReleaseStatus
        End If

        ' Megjegyzés feltöltése
        Splash_Comment.Text = MainWindow.Str_Title + ChrW(13) + ChrW(10) + MainWindow.Str_Version + " " + MainWindow.VersionString

        ' Állapotellenőrzés: Névjegy vagy betöltőképernyő?
        If MainWindow.SplashDefineAsAbout Then
            Link_SplashClose.Text = MainWindow.Str_Close
            Splash_Status.Text = My.Application.Info.Copyright

            If MainWindow.TopMost Then
                Me.TopMost = True
            Else
                Me.TopMost = False
            End If
        Else
            Link_SplashClose.Text = Nothing
            Splash_Status.Text = MainWindow.Str_Loading + "..." ' Ez csak kezdőérték, az aktuális műveletet a főablak állítja be.

            Me.TopMost = True
        End If

    End Sub

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVEÉNY: Képkiválasztás véletlenszám generálással ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function RandomImage()

        ' Értékek definiálása
        Dim ImageNum As Int32 = 0 ' Kezdőérték

        ' Véletlenszámgenerátor inicializálása (enélkül is megy, de hivatalosan kell!)
        Randomize()

        ' Kezdő érték ellenőrzése (Ha 0, akkor egyszerűen generál, ha eltér, akkor ellenőrzi a korábbival való egyezést!)
        If CurrentImage = 0 Then
            ImageNum = CInt(Int((5 * Rnd()) + 1))
        Else
            ImageNum = CurrentImage

            ' Ha egyezik, akkor újra generál (Addig, amíg eltérő nem lesz: kétszer egymás után nem lehet ugyanaz!)
            While (ImageNum = CurrentImage)
                ImageNum = CInt(Int((5 * Rnd()) + 1))
            End While
        End If

        ' Háttérkép beállítása
        BackgroundImage = My.Resources.ResourceManager.GetObject("Splash_" + ImageNum.ToString)

        ' Külső változó beállítása (utolső kép)
        CurrentImage = ImageNum

        Return False

    End Function

    ' ----- ELJÁRÁSOK -----

    ' *** ELJÁRÁS: Ablak bezárása, ha az időzítő elérte a kijelölt értéket ***
    ' Eseményvezérelt: SplashTimer.Tick -> Óra ugrása
    Private Sub SplashTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplashTimer.Tick

        ' Időzítő engedélyezése
        SplashTimer.Enabled = False

        ' Slpash ablak bezárása
        Me.Close()

    End Sub

    ' *** ELJÁRÁS: Kép lecserélése ***
    ' Eseményvezérelt: *.DoubleClick -> Dupla klikk (bárhol az ablakon, kivéve a linket)
    Private Sub LoadSplash_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.DoubleClick, Splash_Title.DoubleClick, Splash_Comment.DoubleClick, Splash_Status.DoubleClick

        ' Új háttérkép generálása
        RandomImage()

    End Sub

    ' *** ELJÁRÁS: Bezárás link ***
    ' Eseményvezérelt: Link_SplashClose.LinkClicked -> Klikk (Link)
    Private Sub Link_SplashClose_Click(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles Link_SplashClose.LinkClicked

        ' Időzítő leállítása
        SplashTimer.Enabled = False

        ' Ablak bezárása
        Me.Close()

    End Sub

End Class