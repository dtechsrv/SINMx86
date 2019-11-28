Imports SINMx86.Functions
Imports SINMx86.Localization

Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Partial Friend Class MyApplication

        ' Második példány indítása esetén végrhajtandó
        Private Sub MyApplication_StartupNextInstance(ByVal sender As System.Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance

            ' Ha a főablak be van töltve
            If MainWindowDone Then

                ' Buboréküzenet megjelenítése
                MainWindow.MainNotifyIcon.ShowBalloonTip(3000, MyName + " - " + GetLoc("Note"), GetLoc("AlreadyRun"), ToolTipIcon.Info)

                ' Főablak megjelenítésének engedélyezése
                If MainWindow.Visible = False Then
                    MainWindow.Visible = True
                End If

                ' Visszaállítás normálra, ha kis méretű
                If MainWindow.WindowState = FormWindowState.Minimized Then
                    MainWindow.WindowState = FormWindowState.Normal
                End If

                ' Előtérbe hozás
                MainWindow.BringToFront()

            End If

        End Sub

    End Class

End Namespace

