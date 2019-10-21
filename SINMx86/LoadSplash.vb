Imports System.Convert

Imports SINMx86.Functions
Imports SINMx86.Localization

' Betöltőképernyő osztálya
Public NotInheritable Class LoadSplash

    ' *** FŐ ELJÁRÁS: Splash ablak betöltése (Me.Load -> LoadSplash) ***
    ' Eseményvezérelt: Betöltőképernyó vagy névjegy meghívása
    Public Sub LoadSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Névjegy időzítője (betöltéskor nincs elindítva)
        SplashTimer.Interval = 10000                                ' Bezárás ennyi idő múlva: 10 másodperc (Érték: ezredmásodperc)

        ' Alkalmazás adatai
        Dim MyVersion As String = Application.ProductVersion        ' Saját verziószám
        Dim MyName As String = My.Application.Info.Title            ' Program neve

        ' Ablak láthatóságának beállítása (Beöltéskor felül, About ablak esetén megegyezik a főablakkal!
        If MainWindowDone Then
            Me.TopMost = MainWindow.TopMost
        Else
            Me.TopMost = True
        End If

        ' Billentyűk figyelése
        Me.KeyPreview = True

        ' Háttérkép generálása
        RandomImage()

        ' Címsor feltöltése
        If ReleaseStatus = Nothing Then
            Splash_Title.Text = MyName
        Else
            Splash_Title.Text = MyName + " " + ReleaseStatus
        End If

        ' Megjegyzés feltöltése
        Splash_Comment.Text = GetLoc("Title") + ChrW(13) + ChrW(10) + GetLoc("Version") + " " + VersionString

        ' Állapotellenőrzés: Névjegy vagy betöltőképernyő?
        If MainWindowDone Then

            ' Időzítő indítása
            SplashTimer.Enabled = True

            ' Bezárás gomb és copyright üzenet
            Link_SplashClose.Text = GetLoc("SplashClose")
            Splash_Status.Text = My.Application.Info.Copyright
        Else

            ' Betöltési állapot kitöltése
            Splash_Status.Text = MainWindow.Value_Debug.Text
        End If

    End Sub

    ' ----- FÜGGVÉNYEK -----

    ' *** FÜGGVEÉNY: Képkiválasztás véletlenszám generálással ***
    ' Bemenet: Void
    ' Kimenet: Boolean (False)
    Private Function RandomImage()

        ' Értékek definiálása
        Dim ImageNum As Int32 = 0                                   ' Kezdőérték (legeslő)
        Dim CurrentImage As Int32 = ImageNum                        ' Jelenlegi beállítása (alapból a kezdőértékre)

        ' Véletlenszámgenerátor inicializálása (enélkül is megy, de hivatalosan kell!)
        Randomize()

        ' Kezdő érték ellenőrzése (Ha 0, akkor egyszerűen generál, ha eltér, akkor ellenőrzi a korábbival való egyezést!)
        If CurrentImage = 0 Then
            ImageNum = ToInt32(Int((5 * Rnd()) + 1))
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

        ' Visszatérési érték beállítása
        Return False

    End Function

    ' ----- ELJÁRÁSOK -----

    ' *** ELJÁRÁS: Kilépési procedúra megindítása (közvetett) ***
    ' Eseményvezérelt: Me.KeyDown -> ESC (Fizikai gomb lenyomása)
    Private Sub KeyDown_Escape_Close(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        ' Ablak bezárása ESC lenyomására
        If e.KeyCode = Keys.Escape And MainWindowDone Then
            Me.Close()
        End If

    End Sub

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