<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadSplash
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoadSplash))
        Me.Splash_Title = New System.Windows.Forms.Label()
        Me.Splash_Comment = New System.Windows.Forms.Label()
        Me.Splash_Status = New System.Windows.Forms.Label()
        Me.SplashTimer = New System.Windows.Forms.Timer(Me.components)
        Me.Link_SplashClose = New System.Windows.Forms.LinkLabel()
        Me.SuspendLayout()
        '
        'Splash_Title
        '
        Me.Splash_Title.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Splash_Title.BackColor = System.Drawing.Color.Transparent
        Me.Splash_Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Splash_Title.ForeColor = System.Drawing.SystemColors.Highlight
        Me.Splash_Title.Location = New System.Drawing.Point(8, 10)
        Me.Splash_Title.Name = "Splash_Title"
        Me.Splash_Title.Size = New System.Drawing.Size(213, 28)
        Me.Splash_Title.TabIndex = 4
        Me.Splash_Title.Text = "SINMx86"
        '
        'Splash_Comment
        '
        Me.Splash_Comment.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Splash_Comment.BackColor = System.Drawing.Color.Transparent
        Me.Splash_Comment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Splash_Comment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Splash_Comment.Location = New System.Drawing.Point(10, 42)
        Me.Splash_Comment.Name = "Splash_Comment"
        Me.Splash_Comment.Size = New System.Drawing.Size(200, 28)
        Me.Splash_Comment.TabIndex = 6
        Me.Splash_Comment.Text = "System Information and Network Monitor" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Version 1.0.0 (Build 1000)"
        '
        'Splash_Status
        '
        Me.Splash_Status.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Splash_Status.BackColor = System.Drawing.Color.Transparent
        Me.Splash_Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Splash_Status.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Splash_Status.Location = New System.Drawing.Point(10, 77)
        Me.Splash_Status.Name = "Splash_Status"
        Me.Splash_Status.Size = New System.Drawing.Size(200, 14)
        Me.Splash_Status.TabIndex = 2
        Me.Splash_Status.Text = "Copyright (C) dtech(.hu)"
        '
        'SplashTimer
        '
        '
        'Link_SplashClose
        '
        Me.Link_SplashClose.ActiveLinkColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Link_SplashClose.BackColor = System.Drawing.Color.Transparent
        Me.Link_SplashClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Link_SplashClose.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.Link_SplashClose.LinkColor = System.Drawing.SystemColors.Highlight
        Me.Link_SplashClose.Location = New System.Drawing.Point(242, 80)
        Me.Link_SplashClose.Name = "Link_SplashClose"
        Me.Link_SplashClose.Size = New System.Drawing.Size(92, 13)
        Me.Link_SplashClose.TabIndex = 18
        Me.Link_SplashClose.TabStop = True
        Me.Link_SplashClose.Text = "Close"
        Me.Link_SplashClose.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.Link_SplashClose.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        '
        'LoadSplash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.SINMx86.My.Resources.Resources.Splash_1
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(340, 100)
        Me.ControlBox = False
        Me.Controls.Add(Me.Link_SplashClose)
        Me.Controls.Add(Me.Splash_Status)
        Me.Controls.Add(Me.Splash_Comment)
        Me.Controls.Add(Me.Splash_Title)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoadSplash"
        Me.Opacity = 0.85R
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Splash_Title As System.Windows.Forms.Label
    Friend WithEvents Splash_Comment As System.Windows.Forms.Label
    Friend WithEvents Splash_Status As System.Windows.Forms.Label
    Friend WithEvents SplashTimer As System.Windows.Forms.Timer
    Friend WithEvents Link_SplashClose As System.Windows.Forms.LinkLabel

End Class
