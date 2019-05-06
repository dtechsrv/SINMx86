<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainWindow
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainWindow))
        Me.GroupBox_HWInfo = New System.Windows.Forms.GroupBox()
        Me.Value_HWIdentifier = New System.Windows.Forms.Label()
        Me.Name_HWIdentifier = New System.Windows.Forms.Label()
        Me.Name_HWList = New System.Windows.Forms.Label()
        Me.Value_HWVendor = New System.Windows.Forms.Label()
        Me.Name_HWVendor = New System.Windows.Forms.Label()
        Me.GroupBox_OSInfo = New System.Windows.Forms.GroupBox()
        Me.Value_OSLang = New System.Windows.Forms.Label()
        Me.Name_OSLang = New System.Windows.Forms.Label()
        Me.Name_OSRelease = New System.Windows.Forms.Label()
        Me.Value_OSVersion = New System.Windows.Forms.Label()
        Me.Name_OSVersion = New System.Windows.Forms.Label()
        Me.Value_OSRelease = New System.Windows.Forms.Label()
        Me.Value_OSName = New System.Windows.Forms.Label()
        Me.Name_OSName = New System.Windows.Forms.Label()
        Me.GroupBox_CPUInfo = New System.Windows.Forms.GroupBox()
        Me.Value_CPUMaxClock = New System.Windows.Forms.Label()
        Me.Name_CPUMaxClock = New System.Windows.Forms.Label()
        Me.Value_CPUCore = New System.Windows.Forms.Label()
        Me.Name_CPUCore = New System.Windows.Forms.Label()
        Me.Value_CPUClock = New System.Windows.Forms.Label()
        Me.Name_CPUClock = New System.Windows.Forms.Label()
        Me.Name_CPUList = New System.Windows.Forms.Label()
        Me.GroupBox_MemoryInfo = New System.Windows.Forms.GroupBox()
        Me.Value_PhysicalMemoryFree = New System.Windows.Forms.Label()
        Me.Value_VirtualMemoryFree = New System.Windows.Forms.Label()
        Me.Name_VirtualMemoryFree = New System.Windows.Forms.Label()
        Me.Name_PhysicalMemoryUsage = New System.Windows.Forms.Label()
        Me.Name_PhysicalMemoryFree = New System.Windows.Forms.Label()
        Me.Value_VirtualMemoryUsage = New System.Windows.Forms.Label()
        Me.Value_PhysicalMemoryUsage = New System.Windows.Forms.Label()
        Me.Name_VirtualMemoryUsage = New System.Windows.Forms.Label()
        Me.Value_VirtualMemorySize = New System.Windows.Forms.Label()
        Me.Value_PhysicalMemorySize = New System.Windows.Forms.Label()
        Me.Name_VirtualMemory = New System.Windows.Forms.Label()
        Me.Name_PhysicalMemory = New System.Windows.Forms.Label()
        Me.GroupBox_VideoInfo = New System.Windows.Forms.GroupBox()
        Me.Button_VideoListReload = New System.Windows.Forms.Button()
        Me.Value_VideoResolution = New System.Windows.Forms.Label()
        Me.Value_VideoMemory = New System.Windows.Forms.Label()
        Me.Name_VideoResolution = New System.Windows.Forms.Label()
        Me.Name_VideoMemory = New System.Windows.Forms.Label()
        Me.Name_VideoList = New System.Windows.Forms.Label()
        Me.Button_Exit = New System.Windows.Forms.Button()
        Me.Button_InterfaceListReload = New System.Windows.Forms.Button()
        Me.Name_InterfaceList = New System.Windows.Forms.Label()
        Me.Name_Bandwidth = New System.Windows.Forms.Label()
        Me.Name_DownloadSpeed = New System.Windows.Forms.Label()
        Me.Name_UploadSpeed = New System.Windows.Forms.Label()
        Me.GroupBox_Network = New System.Windows.Forms.GroupBox()
        Me.Value_UploadSpeedUnit = New System.Windows.Forms.Label()
        Me.Value_BandwidthUnit = New System.Windows.Forms.Label()
        Me.Value_DownloadSpeedUnit = New System.Windows.Forms.Label()
        Me.Value_UploadSpeed = New System.Windows.Forms.Label()
        Me.Value_InterfaceUsage = New System.Windows.Forms.Label()
        Me.Value_DownloadSpeed = New System.Windows.Forms.Label()
        Me.Value_Bandwidth = New System.Windows.Forms.Label()
        Me.Name_UpdateUnit = New System.Windows.Forms.Label()
        Me.Name_UpdateList = New System.Windows.Forms.Label()
        Me.Name_ChartVisible = New System.Windows.Forms.Label()
        Me.CheckBoxChart_UploadVisible = New System.Windows.Forms.CheckBox()
        Me.CheckBoxChart_DownloadVisible = New System.Windows.Forms.CheckBox()
        Me.Name_InterfaceUsage = New System.Windows.Forms.Label()
        Me.PictureBox_TrafficChart = New System.Windows.Forms.PictureBox()
        Me.EventTimer = New System.Windows.Forms.Timer(Me.components)
        Me.Name_LanguageList = New System.Windows.Forms.Label()
        Me.EventToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.Link_Bottom = New System.Windows.Forms.LinkLabel()
        Me.MainStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.MainContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MainContextMenuItem_TopMost = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainContextMenuItem_TaskbarMinimize = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainContextMenuItem_DisableConfirm = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainContextMenuItem_DisableSplash = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainContextMenuItem_Separator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MainContextMenuItem_UpdateCheck = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainContextMenuItem_About = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainContextMenuItem_Separator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.MainContextMenuItem_Exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusLabel_Host = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusLabel_Uptime = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusLabel_ChartStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusLabel_TopMost = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MainNotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Value_Debug = New System.Windows.Forms.Label()
        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.MainMenuItem_Settings = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_SettingsItem_TopMost = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_SettingsItem_TaskbarMinimize = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_SettingsItem_DisableConfirm = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_SettingsItem_DisableSplash = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenuItem_Chart = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_ChartItem_DownloadVisible = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_ChartItem_UploadVisible = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_ChartItem_Separator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MainMenu_ChartItem_ClearChart = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenuItem_Information = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_ActionItem_UpdateCheck = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_ActionItem_About = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu_ActionItem_Separator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MainMenu_ActionItem_Exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScreenshotToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox_DiskInfo = New System.Windows.Forms.GroupBox()
        Me.Value_PartLabel = New System.Windows.Forms.Label()
        Me.Value_PartInfo = New System.Windows.Forms.Label()
        Me.Button_DiskListReload = New System.Windows.Forms.Button()
        Me.Value_DiskSerial = New System.Windows.Forms.Label()
        Me.Name_DiskSerial = New System.Windows.Forms.Label()
        Me.Value_DiskFirmware = New System.Windows.Forms.Label()
        Me.Name_PartList = New System.Windows.Forms.Label()
        Me.Name_DiskFirmware = New System.Windows.Forms.Label()
        Me.Value_DiskInterface = New System.Windows.Forms.Label()
        Me.Value_DiskCapacity = New System.Windows.Forms.Label()
        Me.Name_DiskInterface = New System.Windows.Forms.Label()
        Me.Name_DiskCapacity = New System.Windows.Forms.Label()
        Me.Name_DiskList = New System.Windows.Forms.Label()
        Me.ComboBox_PartList = New SINMx86.LeftComboBox()
        Me.ComboBox_DiskList = New SINMx86.LeftComboBox()
        Me.ComboBox_LanguageList = New SINMx86.LeftComboBox()
        Me.ComboBox_InterfaceList = New SINMx86.LeftComboBox()
        Me.ComboBox_UpdateList = New SINMx86.RightComboBox()
        Me.ComboBox_VideoList = New SINMx86.LeftComboBox()
        Me.ComboBox_CPUList = New SINMx86.LeftComboBox()
        Me.ComboBox_HWList = New SINMx86.LeftComboBox()
        Me.GroupBox_HWInfo.SuspendLayout()
        Me.GroupBox_OSInfo.SuspendLayout()
        Me.GroupBox_CPUInfo.SuspendLayout()
        Me.GroupBox_MemoryInfo.SuspendLayout()
        Me.GroupBox_VideoInfo.SuspendLayout()
        Me.GroupBox_Network.SuspendLayout()
        CType(Me.PictureBox_TrafficChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainStatusStrip.SuspendLayout()
        Me.MainContextMenu.SuspendLayout()
        Me.MainMenu.SuspendLayout()
        Me.GroupBox_DiskInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_HWInfo
        '
        Me.GroupBox_HWInfo.Controls.Add(Me.Value_HWIdentifier)
        Me.GroupBox_HWInfo.Controls.Add(Me.Name_HWIdentifier)
        Me.GroupBox_HWInfo.Controls.Add(Me.ComboBox_HWList)
        Me.GroupBox_HWInfo.Controls.Add(Me.Name_HWList)
        Me.GroupBox_HWInfo.Controls.Add(Me.Value_HWVendor)
        Me.GroupBox_HWInfo.Controls.Add(Me.Name_HWVendor)
        Me.GroupBox_HWInfo.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_HWInfo.Location = New System.Drawing.Point(12, 30)
        Me.GroupBox_HWInfo.Name = "GroupBox_HWInfo"
        Me.GroupBox_HWInfo.Size = New System.Drawing.Size(455, 99)
        Me.GroupBox_HWInfo.TabIndex = 6
        Me.GroupBox_HWInfo.TabStop = False
        Me.GroupBox_HWInfo.Text = "Computer system information"
        '
        'Value_HWIdentifier
        '
        Me.Value_HWIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_HWIdentifier.Location = New System.Drawing.Point(114, 71)
        Me.Value_HWIdentifier.Name = "Value_HWIdentifier"
        Me.Value_HWIdentifier.Size = New System.Drawing.Size(323, 13)
        Me.Value_HWIdentifier.TabIndex = 5
        Me.Value_HWIdentifier.Text = "To Be Filled By O.E.M."
        '
        'Name_HWIdentifier
        '
        Me.Name_HWIdentifier.AutoSize = True
        Me.Name_HWIdentifier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_HWIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_HWIdentifier.Location = New System.Drawing.Point(15, 71)
        Me.Name_HWIdentifier.Name = "Name_HWIdentifier"
        Me.Name_HWIdentifier.Size = New System.Drawing.Size(61, 13)
        Me.Name_HWIdentifier.TabIndex = 4
        Me.Name_HWIdentifier.Text = "Identifier:"
        '
        'Name_HWList
        '
        Me.Name_HWList.AutoSize = True
        Me.Name_HWList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_HWList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_HWList.Location = New System.Drawing.Point(15, 25)
        Me.Name_HWList.Name = "Name_HWList"
        Me.Name_HWList.Size = New System.Drawing.Size(74, 13)
        Me.Name_HWList.TabIndex = 0
        Me.Name_HWList.Text = "Component:"
        '
        'Value_HWVendor
        '
        Me.Value_HWVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_HWVendor.Location = New System.Drawing.Point(114, 48)
        Me.Value_HWVendor.Name = "Value_HWVendor"
        Me.Value_HWVendor.Size = New System.Drawing.Size(323, 13)
        Me.Value_HWVendor.TabIndex = 3
        Me.Value_HWVendor.Text = "To Be Filled By O.E.M."
        '
        'Name_HWVendor
        '
        Me.Name_HWVendor.AutoSize = True
        Me.Name_HWVendor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_HWVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_HWVendor.Location = New System.Drawing.Point(15, 48)
        Me.Name_HWVendor.Name = "Name_HWVendor"
        Me.Name_HWVendor.Size = New System.Drawing.Size(51, 13)
        Me.Name_HWVendor.TabIndex = 2
        Me.Name_HWVendor.Text = "Vendor:"
        '
        'GroupBox_OSInfo
        '
        Me.GroupBox_OSInfo.Controls.Add(Me.Value_OSLang)
        Me.GroupBox_OSInfo.Controls.Add(Me.Name_OSLang)
        Me.GroupBox_OSInfo.Controls.Add(Me.Name_OSRelease)
        Me.GroupBox_OSInfo.Controls.Add(Me.Value_OSVersion)
        Me.GroupBox_OSInfo.Controls.Add(Me.Name_OSVersion)
        Me.GroupBox_OSInfo.Controls.Add(Me.Value_OSRelease)
        Me.GroupBox_OSInfo.Controls.Add(Me.Value_OSName)
        Me.GroupBox_OSInfo.Controls.Add(Me.Name_OSName)
        Me.GroupBox_OSInfo.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_OSInfo.Location = New System.Drawing.Point(12, 299)
        Me.GroupBox_OSInfo.Name = "GroupBox_OSInfo"
        Me.GroupBox_OSInfo.Size = New System.Drawing.Size(455, 76)
        Me.GroupBox_OSInfo.TabIndex = 8
        Me.GroupBox_OSInfo.TabStop = False
        Me.GroupBox_OSInfo.Text = "Operating system information"
        '
        'Value_OSLang
        '
        Me.Value_OSLang.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_OSLang.Location = New System.Drawing.Point(381, 48)
        Me.Value_OSLang.Name = "Value_OSLang"
        Me.Value_OSLang.Size = New System.Drawing.Size(56, 13)
        Me.Value_OSLang.TabIndex = 7
        Me.Value_OSLang.Text = "Unknown"
        Me.Value_OSLang.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_OSLang
        '
        Me.Name_OSLang.AutoSize = True
        Me.Name_OSLang.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_OSLang.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_OSLang.Location = New System.Drawing.Point(318, 48)
        Me.Name_OSLang.Name = "Name_OSLang"
        Me.Name_OSLang.Size = New System.Drawing.Size(67, 13)
        Me.Name_OSLang.TabIndex = 6
        Me.Name_OSLang.Text = "Language:"
        '
        'Name_OSRelease
        '
        Me.Name_OSRelease.AutoSize = True
        Me.Name_OSRelease.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_OSRelease.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_OSRelease.Location = New System.Drawing.Point(199, 48)
        Me.Name_OSRelease.Name = "Name_OSRelease"
        Me.Name_OSRelease.Size = New System.Drawing.Size(57, 13)
        Me.Name_OSRelease.TabIndex = 4
        Me.Name_OSRelease.Text = "Release:"
        '
        'Value_OSVersion
        '
        Me.Value_OSVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_OSVersion.Location = New System.Drawing.Point(114, 48)
        Me.Value_OSVersion.Name = "Value_OSVersion"
        Me.Value_OSVersion.Size = New System.Drawing.Size(81, 13)
        Me.Value_OSVersion.TabIndex = 3
        Me.Value_OSVersion.Text = "1.0"
        '
        'Name_OSVersion
        '
        Me.Name_OSVersion.AutoSize = True
        Me.Name_OSVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_OSVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_OSVersion.Location = New System.Drawing.Point(15, 48)
        Me.Name_OSVersion.Name = "Name_OSVersion"
        Me.Name_OSVersion.Size = New System.Drawing.Size(53, 13)
        Me.Name_OSVersion.TabIndex = 2
        Me.Name_OSVersion.Text = "Version:"
        '
        'Value_OSRelease
        '
        Me.Value_OSRelease.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_OSRelease.Location = New System.Drawing.Point(271, 48)
        Me.Value_OSRelease.Name = "Value_OSRelease"
        Me.Value_OSRelease.Size = New System.Drawing.Size(41, 13)
        Me.Value_OSRelease.TabIndex = 5
        Me.Value_OSRelease.Text = "32-bit"
        Me.Value_OSRelease.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_OSName
        '
        Me.Value_OSName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_OSName.Location = New System.Drawing.Point(114, 25)
        Me.Value_OSName.Name = "Value_OSName"
        Me.Value_OSName.Size = New System.Drawing.Size(323, 13)
        Me.Value_OSName.TabIndex = 1
        Me.Value_OSName.Text = "Unknown Operating System, Service Pack 99"
        '
        'Name_OSName
        '
        Me.Name_OSName.AutoSize = True
        Me.Name_OSName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_OSName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_OSName.Location = New System.Drawing.Point(15, 25)
        Me.Name_OSName.Name = "Name_OSName"
        Me.Name_OSName.Size = New System.Drawing.Size(55, 13)
        Me.Name_OSName.TabIndex = 0
        Me.Name_OSName.Text = "Product:"
        '
        'GroupBox_CPUInfo
        '
        Me.GroupBox_CPUInfo.Controls.Add(Me.ComboBox_CPUList)
        Me.GroupBox_CPUInfo.Controls.Add(Me.Value_CPUMaxClock)
        Me.GroupBox_CPUInfo.Controls.Add(Me.Name_CPUMaxClock)
        Me.GroupBox_CPUInfo.Controls.Add(Me.Value_CPUCore)
        Me.GroupBox_CPUInfo.Controls.Add(Me.Name_CPUCore)
        Me.GroupBox_CPUInfo.Controls.Add(Me.Value_CPUClock)
        Me.GroupBox_CPUInfo.Controls.Add(Me.Name_CPUClock)
        Me.GroupBox_CPUInfo.Controls.Add(Me.Name_CPUList)
        Me.GroupBox_CPUInfo.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_CPUInfo.Location = New System.Drawing.Point(12, 135)
        Me.GroupBox_CPUInfo.Name = "GroupBox_CPUInfo"
        Me.GroupBox_CPUInfo.Size = New System.Drawing.Size(455, 76)
        Me.GroupBox_CPUInfo.TabIndex = 7
        Me.GroupBox_CPUInfo.TabStop = False
        Me.GroupBox_CPUInfo.Text = "Processor information"
        '
        'Value_CPUMaxClock
        '
        Me.Value_CPUMaxClock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_CPUMaxClock.Location = New System.Drawing.Point(376, 48)
        Me.Value_CPUMaxClock.Name = "Value_CPUMaxClock"
        Me.Value_CPUMaxClock.Size = New System.Drawing.Size(61, 13)
        Me.Value_CPUMaxClock.TabIndex = 7
        Me.Value_CPUMaxClock.Text = "9999 MHz"
        Me.Value_CPUMaxClock.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_CPUMaxClock
        '
        Me.Name_CPUMaxClock.AutoSize = True
        Me.Name_CPUMaxClock.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_CPUMaxClock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_CPUMaxClock.Location = New System.Drawing.Point(318, 48)
        Me.Name_CPUMaxClock.Name = "Name_CPUMaxClock"
        Me.Name_CPUMaxClock.Size = New System.Drawing.Size(48, 13)
        Me.Name_CPUMaxClock.TabIndex = 6
        Me.Name_CPUMaxClock.Text = "Native:"
        '
        'Value_CPUCore
        '
        Me.Value_CPUCore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_CPUCore.Location = New System.Drawing.Point(124, 48)
        Me.Value_CPUCore.Name = "Value_CPUCore"
        Me.Value_CPUCore.Size = New System.Drawing.Size(69, 13)
        Me.Value_CPUCore.TabIndex = 3
        Me.Value_CPUCore.Text = "99 / 99"
        Me.Value_CPUCore.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_CPUCore
        '
        Me.Name_CPUCore.AutoSize = True
        Me.Name_CPUCore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_CPUCore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_CPUCore.Location = New System.Drawing.Point(15, 48)
        Me.Name_CPUCore.Name = "Name_CPUCore"
        Me.Name_CPUCore.Size = New System.Drawing.Size(103, 13)
        Me.Name_CPUCore.TabIndex = 2
        Me.Name_CPUCore.Text = "Cores / Threads:"
        '
        'Value_CPUClock
        '
        Me.Value_CPUClock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_CPUClock.Location = New System.Drawing.Point(252, 48)
        Me.Value_CPUClock.Name = "Value_CPUClock"
        Me.Value_CPUClock.Size = New System.Drawing.Size(60, 13)
        Me.Value_CPUClock.TabIndex = 5
        Me.Value_CPUClock.Text = "9999 MHz"
        Me.Value_CPUClock.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_CPUClock
        '
        Me.Name_CPUClock.AutoSize = True
        Me.Name_CPUClock.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_CPUClock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_CPUClock.Location = New System.Drawing.Point(199, 48)
        Me.Name_CPUClock.Name = "Name_CPUClock"
        Me.Name_CPUClock.Size = New System.Drawing.Size(43, 13)
        Me.Name_CPUClock.TabIndex = 4
        Me.Name_CPUClock.Text = "Clock:"
        '
        'Name_CPUList
        '
        Me.Name_CPUList.AutoSize = True
        Me.Name_CPUList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_CPUList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_CPUList.Location = New System.Drawing.Point(15, 25)
        Me.Name_CPUList.Name = "Name_CPUList"
        Me.Name_CPUList.Size = New System.Drawing.Size(67, 13)
        Me.Name_CPUList.TabIndex = 0
        Me.Name_CPUList.Text = "Processor:"
        '
        'GroupBox_MemoryInfo
        '
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Value_PhysicalMemoryFree)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Value_VirtualMemoryFree)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Name_VirtualMemoryFree)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Name_PhysicalMemoryUsage)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Name_PhysicalMemoryFree)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Value_VirtualMemoryUsage)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Value_PhysicalMemoryUsage)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Name_VirtualMemoryUsage)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Value_VirtualMemorySize)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Value_PhysicalMemorySize)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Name_VirtualMemory)
        Me.GroupBox_MemoryInfo.Controls.Add(Me.Name_PhysicalMemory)
        Me.GroupBox_MemoryInfo.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_MemoryInfo.Location = New System.Drawing.Point(12, 217)
        Me.GroupBox_MemoryInfo.Name = "GroupBox_MemoryInfo"
        Me.GroupBox_MemoryInfo.Size = New System.Drawing.Size(455, 76)
        Me.GroupBox_MemoryInfo.TabIndex = 3
        Me.GroupBox_MemoryInfo.TabStop = False
        Me.GroupBox_MemoryInfo.Text = "Memory information"
        '
        'Value_PhysicalMemoryFree
        '
        Me.Value_PhysicalMemoryFree.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_PhysicalMemoryFree.Location = New System.Drawing.Point(246, 25)
        Me.Value_PhysicalMemoryFree.Name = "Value_PhysicalMemoryFree"
        Me.Value_PhysicalMemoryFree.Size = New System.Drawing.Size(66, 13)
        Me.Value_PhysicalMemoryFree.TabIndex = 3
        Me.Value_PhysicalMemoryFree.Text = "1000.00 MB"
        Me.Value_PhysicalMemoryFree.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_VirtualMemoryFree
        '
        Me.Value_VirtualMemoryFree.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_VirtualMemoryFree.Location = New System.Drawing.Point(246, 48)
        Me.Value_VirtualMemoryFree.Name = "Value_VirtualMemoryFree"
        Me.Value_VirtualMemoryFree.Size = New System.Drawing.Size(66, 13)
        Me.Value_VirtualMemoryFree.TabIndex = 9
        Me.Value_VirtualMemoryFree.Text = "1000.00 MB"
        Me.Value_VirtualMemoryFree.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_VirtualMemoryFree
        '
        Me.Name_VirtualMemoryFree.AutoSize = True
        Me.Name_VirtualMemoryFree.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_VirtualMemoryFree.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_VirtualMemoryFree.Location = New System.Drawing.Point(199, 48)
        Me.Name_VirtualMemoryFree.Name = "Name_VirtualMemoryFree"
        Me.Name_VirtualMemoryFree.Size = New System.Drawing.Size(36, 13)
        Me.Name_VirtualMemoryFree.TabIndex = 8
        Me.Name_VirtualMemoryFree.Text = "Free:"
        '
        'Name_PhysicalMemoryUsage
        '
        Me.Name_PhysicalMemoryUsage.AutoSize = True
        Me.Name_PhysicalMemoryUsage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_PhysicalMemoryUsage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_PhysicalMemoryUsage.Location = New System.Drawing.Point(318, 25)
        Me.Name_PhysicalMemoryUsage.Name = "Name_PhysicalMemoryUsage"
        Me.Name_PhysicalMemoryUsage.Size = New System.Drawing.Size(47, 13)
        Me.Name_PhysicalMemoryUsage.TabIndex = 4
        Me.Name_PhysicalMemoryUsage.Text = "Usage:"
        '
        'Name_PhysicalMemoryFree
        '
        Me.Name_PhysicalMemoryFree.AutoSize = True
        Me.Name_PhysicalMemoryFree.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_PhysicalMemoryFree.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_PhysicalMemoryFree.Location = New System.Drawing.Point(199, 25)
        Me.Name_PhysicalMemoryFree.Name = "Name_PhysicalMemoryFree"
        Me.Name_PhysicalMemoryFree.Size = New System.Drawing.Size(36, 13)
        Me.Name_PhysicalMemoryFree.TabIndex = 2
        Me.Name_PhysicalMemoryFree.Text = "Free:"
        '
        'Value_VirtualMemoryUsage
        '
        Me.Value_VirtualMemoryUsage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_VirtualMemoryUsage.Location = New System.Drawing.Point(393, 48)
        Me.Value_VirtualMemoryUsage.Name = "Value_VirtualMemoryUsage"
        Me.Value_VirtualMemoryUsage.Size = New System.Drawing.Size(44, 13)
        Me.Value_VirtualMemoryUsage.TabIndex = 11
        Me.Value_VirtualMemoryUsage.Text = "100 %"
        Me.Value_VirtualMemoryUsage.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_PhysicalMemoryUsage
        '
        Me.Value_PhysicalMemoryUsage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_PhysicalMemoryUsage.Location = New System.Drawing.Point(393, 25)
        Me.Value_PhysicalMemoryUsage.Name = "Value_PhysicalMemoryUsage"
        Me.Value_PhysicalMemoryUsage.Size = New System.Drawing.Size(44, 13)
        Me.Value_PhysicalMemoryUsage.TabIndex = 5
        Me.Value_PhysicalMemoryUsage.Text = "100 %"
        Me.Value_PhysicalMemoryUsage.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_VirtualMemoryUsage
        '
        Me.Name_VirtualMemoryUsage.AutoSize = True
        Me.Name_VirtualMemoryUsage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_VirtualMemoryUsage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_VirtualMemoryUsage.Location = New System.Drawing.Point(318, 48)
        Me.Name_VirtualMemoryUsage.Name = "Name_VirtualMemoryUsage"
        Me.Name_VirtualMemoryUsage.Size = New System.Drawing.Size(47, 13)
        Me.Name_VirtualMemoryUsage.TabIndex = 10
        Me.Name_VirtualMemoryUsage.Text = "Usage:"
        '
        'Value_VirtualMemorySize
        '
        Me.Value_VirtualMemorySize.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_VirtualMemorySize.Location = New System.Drawing.Point(127, 48)
        Me.Value_VirtualMemorySize.Name = "Value_VirtualMemorySize"
        Me.Value_VirtualMemorySize.Size = New System.Drawing.Size(66, 13)
        Me.Value_VirtualMemorySize.TabIndex = 7
        Me.Value_VirtualMemorySize.Text = "1000.00 MB"
        Me.Value_VirtualMemorySize.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_PhysicalMemorySize
        '
        Me.Value_PhysicalMemorySize.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_PhysicalMemorySize.Location = New System.Drawing.Point(127, 25)
        Me.Value_PhysicalMemorySize.Name = "Value_PhysicalMemorySize"
        Me.Value_PhysicalMemorySize.Size = New System.Drawing.Size(66, 13)
        Me.Value_PhysicalMemorySize.TabIndex = 1
        Me.Value_PhysicalMemorySize.Text = "1000.00 MB"
        Me.Value_PhysicalMemorySize.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_VirtualMemory
        '
        Me.Name_VirtualMemory.AutoSize = True
        Me.Name_VirtualMemory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_VirtualMemory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_VirtualMemory.Location = New System.Drawing.Point(15, 48)
        Me.Name_VirtualMemory.Name = "Name_VirtualMemory"
        Me.Name_VirtualMemory.Size = New System.Drawing.Size(93, 13)
        Me.Name_VirtualMemory.TabIndex = 6
        Me.Name_VirtualMemory.Text = "Virtual memory:"
        '
        'Name_PhysicalMemory
        '
        Me.Name_PhysicalMemory.AutoSize = True
        Me.Name_PhysicalMemory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_PhysicalMemory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_PhysicalMemory.Location = New System.Drawing.Point(15, 25)
        Me.Name_PhysicalMemory.Name = "Name_PhysicalMemory"
        Me.Name_PhysicalMemory.Size = New System.Drawing.Size(104, 13)
        Me.Name_PhysicalMemory.TabIndex = 0
        Me.Name_PhysicalMemory.Text = "Physical memory:"
        '
        'GroupBox_VideoInfo
        '
        Me.GroupBox_VideoInfo.Controls.Add(Me.Button_VideoListReload)
        Me.GroupBox_VideoInfo.Controls.Add(Me.ComboBox_VideoList)
        Me.GroupBox_VideoInfo.Controls.Add(Me.Value_VideoResolution)
        Me.GroupBox_VideoInfo.Controls.Add(Me.Value_VideoMemory)
        Me.GroupBox_VideoInfo.Controls.Add(Me.Name_VideoResolution)
        Me.GroupBox_VideoInfo.Controls.Add(Me.Name_VideoMemory)
        Me.GroupBox_VideoInfo.Controls.Add(Me.Name_VideoList)
        Me.GroupBox_VideoInfo.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_VideoInfo.Location = New System.Drawing.Point(477, 30)
        Me.GroupBox_VideoInfo.Name = "GroupBox_VideoInfo"
        Me.GroupBox_VideoInfo.Size = New System.Drawing.Size(455, 76)
        Me.GroupBox_VideoInfo.TabIndex = 10
        Me.GroupBox_VideoInfo.TabStop = False
        Me.GroupBox_VideoInfo.Text = "Display controller information"
        '
        'Button_VideoListReload
        '
        Me.Button_VideoListReload.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_VideoListReload.Image = Global.SINMx86.My.Resources.Resources.Control_Refresh
        Me.Button_VideoListReload.Location = New System.Drawing.Point(414, 21)
        Me.Button_VideoListReload.Name = "Button_VideoListReload"
        Me.Button_VideoListReload.Size = New System.Drawing.Size(23, 23)
        Me.Button_VideoListReload.TabIndex = 2
        Me.Button_VideoListReload.UseVisualStyleBackColor = True
        '
        'Value_VideoResolution
        '
        Me.Value_VideoResolution.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_VideoResolution.Location = New System.Drawing.Point(317, 48)
        Me.Value_VideoResolution.Name = "Value_VideoResolution"
        Me.Value_VideoResolution.Size = New System.Drawing.Size(118, 13)
        Me.Value_VideoResolution.TabIndex = 6
        Me.Value_VideoResolution.Text = "1920 x 1080 (32 bit)"
        Me.Value_VideoResolution.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_VideoMemory
        '
        Me.Value_VideoMemory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_VideoMemory.Location = New System.Drawing.Point(111, 48)
        Me.Value_VideoMemory.Name = "Value_VideoMemory"
        Me.Value_VideoMemory.Size = New System.Drawing.Size(111, 13)
        Me.Value_VideoMemory.TabIndex = 4
        Me.Value_VideoMemory.Text = "512 MB"
        Me.Value_VideoMemory.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_VideoResolution
        '
        Me.Name_VideoResolution.AutoSize = True
        Me.Name_VideoResolution.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_VideoResolution.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_VideoResolution.Location = New System.Drawing.Point(228, 48)
        Me.Name_VideoResolution.Name = "Name_VideoResolution"
        Me.Name_VideoResolution.Size = New System.Drawing.Size(71, 13)
        Me.Name_VideoResolution.TabIndex = 5
        Me.Name_VideoResolution.Text = "Resolution:"
        '
        'Name_VideoMemory
        '
        Me.Name_VideoMemory.AutoSize = True
        Me.Name_VideoMemory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_VideoMemory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_VideoMemory.Location = New System.Drawing.Point(15, 48)
        Me.Name_VideoMemory.Name = "Name_VideoMemory"
        Me.Name_VideoMemory.Size = New System.Drawing.Size(89, 13)
        Me.Name_VideoMemory.TabIndex = 3
        Me.Name_VideoMemory.Text = "Video memory:"
        '
        'Name_VideoList
        '
        Me.Name_VideoList.AutoSize = True
        Me.Name_VideoList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_VideoList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_VideoList.Location = New System.Drawing.Point(15, 25)
        Me.Name_VideoList.Name = "Name_VideoList"
        Me.Name_VideoList.Size = New System.Drawing.Size(90, 13)
        Me.Name_VideoList.TabIndex = 0
        Me.Name_VideoList.Text = "Graphics card:"
        '
        'Button_Exit
        '
        Me.Button_Exit.Location = New System.Drawing.Point(857, 513)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.Size = New System.Drawing.Size(75, 23)
        Me.Button_Exit.TabIndex = 0
        Me.Button_Exit.Text = "E&xit"
        Me.Button_Exit.UseVisualStyleBackColor = True
        '
        'Button_InterfaceListReload
        '
        Me.Button_InterfaceListReload.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_InterfaceListReload.Image = Global.SINMx86.My.Resources.Resources.Control_Refresh
        Me.Button_InterfaceListReload.Location = New System.Drawing.Point(414, 21)
        Me.Button_InterfaceListReload.Name = "Button_InterfaceListReload"
        Me.Button_InterfaceListReload.Size = New System.Drawing.Size(23, 23)
        Me.Button_InterfaceListReload.TabIndex = 2
        Me.Button_InterfaceListReload.UseVisualStyleBackColor = True
        '
        'Name_InterfaceList
        '
        Me.Name_InterfaceList.AutoSize = True
        Me.Name_InterfaceList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_InterfaceList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_InterfaceList.Location = New System.Drawing.Point(15, 25)
        Me.Name_InterfaceList.Name = "Name_InterfaceList"
        Me.Name_InterfaceList.Size = New System.Drawing.Size(62, 13)
        Me.Name_InterfaceList.TabIndex = 0
        Me.Name_InterfaceList.Text = "Interface:"
        '
        'Name_Bandwidth
        '
        Me.Name_Bandwidth.AutoSize = True
        Me.Name_Bandwidth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_Bandwidth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_Bandwidth.Location = New System.Drawing.Point(15, 48)
        Me.Name_Bandwidth.Name = "Name_Bandwidth"
        Me.Name_Bandwidth.Size = New System.Drawing.Size(70, 13)
        Me.Name_Bandwidth.TabIndex = 3
        Me.Name_Bandwidth.Text = "Bandwidth:"
        '
        'Name_DownloadSpeed
        '
        Me.Name_DownloadSpeed.AutoSize = True
        Me.Name_DownloadSpeed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_DownloadSpeed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_DownloadSpeed.Location = New System.Drawing.Point(15, 71)
        Me.Name_DownloadSpeed.Name = "Name_DownloadSpeed"
        Me.Name_DownloadSpeed.Size = New System.Drawing.Size(105, 13)
        Me.Name_DownloadSpeed.TabIndex = 8
        Me.Name_DownloadSpeed.Text = "Download speed:"
        '
        'Name_UploadSpeed
        '
        Me.Name_UploadSpeed.AutoSize = True
        Me.Name_UploadSpeed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_UploadSpeed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_UploadSpeed.Location = New System.Drawing.Point(228, 71)
        Me.Name_UploadSpeed.Name = "Name_UploadSpeed"
        Me.Name_UploadSpeed.Size = New System.Drawing.Size(89, 13)
        Me.Name_UploadSpeed.TabIndex = 11
        Me.Name_UploadSpeed.Text = "Upload speed:"
        '
        'GroupBox_Network
        '
        Me.GroupBox_Network.Controls.Add(Me.Name_InterfaceList)
        Me.GroupBox_Network.Controls.Add(Me.Button_InterfaceListReload)
        Me.GroupBox_Network.Controls.Add(Me.Value_UploadSpeedUnit)
        Me.GroupBox_Network.Controls.Add(Me.ComboBox_InterfaceList)
        Me.GroupBox_Network.Controls.Add(Me.Value_BandwidthUnit)
        Me.GroupBox_Network.Controls.Add(Me.Value_DownloadSpeedUnit)
        Me.GroupBox_Network.Controls.Add(Me.Value_UploadSpeed)
        Me.GroupBox_Network.Controls.Add(Me.Value_InterfaceUsage)
        Me.GroupBox_Network.Controls.Add(Me.Value_DownloadSpeed)
        Me.GroupBox_Network.Controls.Add(Me.Value_Bandwidth)
        Me.GroupBox_Network.Controls.Add(Me.Name_UpdateUnit)
        Me.GroupBox_Network.Controls.Add(Me.ComboBox_UpdateList)
        Me.GroupBox_Network.Controls.Add(Me.Name_UpdateList)
        Me.GroupBox_Network.Controls.Add(Me.Name_ChartVisible)
        Me.GroupBox_Network.Controls.Add(Me.CheckBoxChart_UploadVisible)
        Me.GroupBox_Network.Controls.Add(Me.CheckBoxChart_DownloadVisible)
        Me.GroupBox_Network.Controls.Add(Me.Name_InterfaceUsage)
        Me.GroupBox_Network.Controls.Add(Me.PictureBox_TrafficChart)
        Me.GroupBox_Network.Controls.Add(Me.Name_DownloadSpeed)
        Me.GroupBox_Network.Controls.Add(Me.Name_Bandwidth)
        Me.GroupBox_Network.Controls.Add(Me.Name_UploadSpeed)
        Me.GroupBox_Network.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_Network.Location = New System.Drawing.Point(477, 112)
        Me.GroupBox_Network.Name = "GroupBox_Network"
        Me.GroupBox_Network.Size = New System.Drawing.Size(455, 391)
        Me.GroupBox_Network.TabIndex = 11
        Me.GroupBox_Network.TabStop = False
        Me.GroupBox_Network.Text = "Network interface statistics"
        '
        'Value_UploadSpeedUnit
        '
        Me.Value_UploadSpeedUnit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_UploadSpeedUnit.Location = New System.Drawing.Point(399, 71)
        Me.Value_UploadSpeedUnit.Name = "Value_UploadSpeedUnit"
        Me.Value_UploadSpeedUnit.Size = New System.Drawing.Size(36, 13)
        Me.Value_UploadSpeedUnit.TabIndex = 13
        Me.Value_UploadSpeedUnit.Text = "MB/s"
        Me.Value_UploadSpeedUnit.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_BandwidthUnit
        '
        Me.Value_BandwidthUnit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_BandwidthUnit.Location = New System.Drawing.Point(186, 48)
        Me.Value_BandwidthUnit.Name = "Value_BandwidthUnit"
        Me.Value_BandwidthUnit.Size = New System.Drawing.Size(36, 13)
        Me.Value_BandwidthUnit.TabIndex = 5
        Me.Value_BandwidthUnit.Text = "Mbps"
        Me.Value_BandwidthUnit.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_DownloadSpeedUnit
        '
        Me.Value_DownloadSpeedUnit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_DownloadSpeedUnit.Location = New System.Drawing.Point(186, 71)
        Me.Value_DownloadSpeedUnit.Name = "Value_DownloadSpeedUnit"
        Me.Value_DownloadSpeedUnit.Size = New System.Drawing.Size(36, 13)
        Me.Value_DownloadSpeedUnit.TabIndex = 10
        Me.Value_DownloadSpeedUnit.Text = "MB/s"
        Me.Value_DownloadSpeedUnit.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_UploadSpeed
        '
        Me.Value_UploadSpeed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_UploadSpeed.Location = New System.Drawing.Point(347, 71)
        Me.Value_UploadSpeed.Name = "Value_UploadSpeed"
        Me.Value_UploadSpeed.Size = New System.Drawing.Size(54, 13)
        Me.Value_UploadSpeed.TabIndex = 12
        Me.Value_UploadSpeed.Text = "1.00"
        Me.Value_UploadSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_InterfaceUsage
        '
        Me.Value_InterfaceUsage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_InterfaceUsage.Location = New System.Drawing.Point(379, 48)
        Me.Value_InterfaceUsage.Name = "Value_InterfaceUsage"
        Me.Value_InterfaceUsage.Size = New System.Drawing.Size(56, 13)
        Me.Value_InterfaceUsage.TabIndex = 7
        Me.Value_InterfaceUsage.Text = "100 %"
        Me.Value_InterfaceUsage.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_DownloadSpeed
        '
        Me.Value_DownloadSpeed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_DownloadSpeed.Location = New System.Drawing.Point(134, 71)
        Me.Value_DownloadSpeed.Name = "Value_DownloadSpeed"
        Me.Value_DownloadSpeed.Size = New System.Drawing.Size(54, 13)
        Me.Value_DownloadSpeed.TabIndex = 9
        Me.Value_DownloadSpeed.Text = "1.00"
        Me.Value_DownloadSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_Bandwidth
        '
        Me.Value_Bandwidth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_Bandwidth.Location = New System.Drawing.Point(137, 48)
        Me.Value_Bandwidth.Name = "Value_Bandwidth"
        Me.Value_Bandwidth.Size = New System.Drawing.Size(51, 13)
        Me.Value_Bandwidth.TabIndex = 4
        Me.Value_Bandwidth.Text = "100.00"
        Me.Value_Bandwidth.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_UpdateUnit
        '
        Me.Name_UpdateUnit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_UpdateUnit.Location = New System.Drawing.Point(428, 363)
        Me.Name_UpdateUnit.Name = "Name_UpdateUnit"
        Me.Name_UpdateUnit.Size = New System.Drawing.Size(11, 13)
        Me.Name_UpdateUnit.TabIndex = 19
        Me.Name_UpdateUnit.Text = "s"
        '
        'Name_UpdateList
        '
        Me.Name_UpdateList.AutoSize = True
        Me.Name_UpdateList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_UpdateList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_UpdateList.Location = New System.Drawing.Point(275, 363)
        Me.Name_UpdateList.Name = "Name_UpdateList"
        Me.Name_UpdateList.Size = New System.Drawing.Size(98, 13)
        Me.Name_UpdateList.TabIndex = 17
        Me.Name_UpdateList.Text = "Update interval:"
        '
        'Name_ChartVisible
        '
        Me.Name_ChartVisible.AutoSize = True
        Me.Name_ChartVisible.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_ChartVisible.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_ChartVisible.Location = New System.Drawing.Point(14, 363)
        Me.Name_ChartVisible.Name = "Name_ChartVisible"
        Me.Name_ChartVisible.Size = New System.Drawing.Size(90, 13)
        Me.Name_ChartVisible.TabIndex = 14
        Me.Name_ChartVisible.Text = "Chart visibility:"
        '
        'CheckBoxChart_UploadVisible
        '
        Me.CheckBoxChart_UploadVisible.AutoSize = True
        Me.CheckBoxChart_UploadVisible.Checked = True
        Me.CheckBoxChart_UploadVisible.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxChart_UploadVisible.ForeColor = System.Drawing.Color.Red
        Me.CheckBoxChart_UploadVisible.Location = New System.Drawing.Point(206, 362)
        Me.CheckBoxChart_UploadVisible.Name = "CheckBoxChart_UploadVisible"
        Me.CheckBoxChart_UploadVisible.Size = New System.Drawing.Size(60, 17)
        Me.CheckBoxChart_UploadVisible.TabIndex = 16
        Me.CheckBoxChart_UploadVisible.Text = "Upload"
        Me.CheckBoxChart_UploadVisible.UseVisualStyleBackColor = True
        '
        'CheckBoxChart_DownloadVisible
        '
        Me.CheckBoxChart_DownloadVisible.AutoSize = True
        Me.CheckBoxChart_DownloadVisible.Checked = True
        Me.CheckBoxChart_DownloadVisible.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxChart_DownloadVisible.ForeColor = System.Drawing.Color.Green
        Me.CheckBoxChart_DownloadVisible.Location = New System.Drawing.Point(121, 362)
        Me.CheckBoxChart_DownloadVisible.Name = "CheckBoxChart_DownloadVisible"
        Me.CheckBoxChart_DownloadVisible.Size = New System.Drawing.Size(74, 17)
        Me.CheckBoxChart_DownloadVisible.TabIndex = 15
        Me.CheckBoxChart_DownloadVisible.Text = "Download"
        Me.CheckBoxChart_DownloadVisible.UseVisualStyleBackColor = True
        '
        'Name_InterfaceUsage
        '
        Me.Name_InterfaceUsage.AutoSize = True
        Me.Name_InterfaceUsage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_InterfaceUsage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_InterfaceUsage.Location = New System.Drawing.Point(228, 48)
        Me.Name_InterfaceUsage.Name = "Name_InterfaceUsage"
        Me.Name_InterfaceUsage.Size = New System.Drawing.Size(100, 13)
        Me.Name_InterfaceUsage.TabIndex = 6
        Me.Name_InterfaceUsage.Text = "Interface usage:"
        '
        'PictureBox_TrafficChart
        '
        Me.PictureBox_TrafficChart.BackColor = System.Drawing.Color.Gray
        Me.PictureBox_TrafficChart.Location = New System.Drawing.Point(17, 96)
        Me.PictureBox_TrafficChart.Name = "PictureBox_TrafficChart"
        Me.PictureBox_TrafficChart.Size = New System.Drawing.Size(420, 256)
        Me.PictureBox_TrafficChart.TabIndex = 18
        Me.PictureBox_TrafficChart.TabStop = False
        '
        'EventTimer
        '
        Me.EventTimer.Interval = 1000
        '
        'Name_LanguageList
        '
        Me.Name_LanguageList.AutoSize = True
        Me.Name_LanguageList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_LanguageList.Location = New System.Drawing.Point(9, 518)
        Me.Name_LanguageList.Name = "Name_LanguageList"
        Me.Name_LanguageList.Size = New System.Drawing.Size(67, 13)
        Me.Name_LanguageList.TabIndex = 2
        Me.Name_LanguageList.Text = "Language:"
        '
        'EventToolTip
        '
        Me.EventToolTip.AutomaticDelay = 1000
        '
        'Link_Bottom
        '
        Me.Link_Bottom.ActiveLinkColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Link_Bottom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Link_Bottom.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.Link_Bottom.LinkColor = System.Drawing.SystemColors.Highlight
        Me.Link_Bottom.Location = New System.Drawing.Point(219, 518)
        Me.Link_Bottom.Name = "Link_Bottom"
        Me.Link_Bottom.Size = New System.Drawing.Size(507, 13)
        Me.Link_Bottom.TabIndex = 4
        Me.Link_Bottom.TabStop = True
        Me.Link_Bottom.Text = "BOTTOM_TEXT"
        Me.Link_Bottom.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Link_Bottom.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        '
        'MainStatusStrip
        '
        Me.MainStatusStrip.AllowDrop = True
        Me.MainStatusStrip.AutoSize = False
        Me.MainStatusStrip.ContextMenuStrip = Me.MainContextMenu
        Me.MainStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel_Host, Me.StatusLabel_Uptime, Me.StatusLabel_ChartStatus, Me.StatusLabel_TopMost})
        Me.MainStatusStrip.Location = New System.Drawing.Point(0, 545)
        Me.MainStatusStrip.Name = "MainStatusStrip"
        Me.MainStatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode
        Me.MainStatusStrip.ShowItemToolTips = True
        Me.MainStatusStrip.Size = New System.Drawing.Size(944, 22)
        Me.MainStatusStrip.SizingGrip = False
        Me.MainStatusStrip.TabIndex = 13
        Me.MainStatusStrip.Text = "StatusStrip"
        '
        'MainContextMenu
        '
        Me.MainContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MainContextMenuItem_TopMost, Me.MainContextMenuItem_TaskbarMinimize, Me.MainContextMenuItem_DisableConfirm, Me.MainContextMenuItem_DisableSplash, Me.MainContextMenuItem_Separator1, Me.MainContextMenuItem_UpdateCheck, Me.MainContextMenuItem_About, Me.MainContextMenuItem_Separator2, Me.MainContextMenuItem_Exit})
        Me.MainContextMenu.Name = "NotifyMenu"
        Me.MainContextMenu.Size = New System.Drawing.Size(206, 170)
        Me.MainContextMenu.Text = "SINMx86"
        '
        'MainContextMenuItem_TopMost
        '
        Me.MainContextMenuItem_TopMost.Name = "MainContextMenuItem_TopMost"
        Me.MainContextMenuItem_TopMost.Size = New System.Drawing.Size(205, 22)
        Me.MainContextMenuItem_TopMost.Text = "Always on Top"
        '
        'MainContextMenuItem_TaskbarMinimize
        '
        Me.MainContextMenuItem_TaskbarMinimize.Name = "MainContextMenuItem_TaskbarMinimize"
        Me.MainContextMenuItem_TaskbarMinimize.Size = New System.Drawing.Size(205, 22)
        Me.MainContextMenuItem_TaskbarMinimize.Text = "Minimize to taskbar"
        '
        'MainContextMenuItem_DisableConfirm
        '
        Me.MainContextMenuItem_DisableConfirm.Name = "MainContextMenuItem_DisableConfirm"
        Me.MainContextMenuItem_DisableConfirm.Size = New System.Drawing.Size(205, 22)
        Me.MainContextMenuItem_DisableConfirm.Text = "Disable exit confirmation"
        '
        'MainContextMenuItem_DisableSplash
        '
        Me.MainContextMenuItem_DisableSplash.Name = "MainContextMenuItem_DisableSplash"
        Me.MainContextMenuItem_DisableSplash.Size = New System.Drawing.Size(205, 22)
        Me.MainContextMenuItem_DisableSplash.Text = "Disable loading screen"
        '
        'MainContextMenuItem_Separator1
        '
        Me.MainContextMenuItem_Separator1.Name = "MainContextMenuItem_Separator1"
        Me.MainContextMenuItem_Separator1.Size = New System.Drawing.Size(202, 6)
        '
        'MainContextMenuItem_UpdateCheck
        '
        Me.MainContextMenuItem_UpdateCheck.Name = "MainContextMenuItem_UpdateCheck"
        Me.MainContextMenuItem_UpdateCheck.Size = New System.Drawing.Size(205, 22)
        Me.MainContextMenuItem_UpdateCheck.Text = "Search for updates..."
        '
        'MainContextMenuItem_About
        '
        Me.MainContextMenuItem_About.Name = "MainContextMenuItem_About"
        Me.MainContextMenuItem_About.Size = New System.Drawing.Size(205, 22)
        Me.MainContextMenuItem_About.Text = "About..."
        '
        'MainContextMenuItem_Separator2
        '
        Me.MainContextMenuItem_Separator2.Name = "MainContextMenuItem_Separator2"
        Me.MainContextMenuItem_Separator2.Size = New System.Drawing.Size(202, 6)
        '
        'MainContextMenuItem_Exit
        '
        Me.MainContextMenuItem_Exit.Name = "MainContextMenuItem_Exit"
        Me.MainContextMenuItem_Exit.Size = New System.Drawing.Size(205, 22)
        Me.MainContextMenuItem_Exit.Text = "Exit"
        '
        'StatusLabel_Host
        '
        Me.StatusLabel_Host.AutoSize = False
        Me.StatusLabel_Host.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.StatusLabel_Host.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.StatusLabel_Host.Image = Global.SINMx86.My.Resources.Resources.Control_Info
        Me.StatusLabel_Host.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.StatusLabel_Host.Margin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.StatusLabel_Host.Name = "StatusLabel_Host"
        Me.StatusLabel_Host.Size = New System.Drawing.Size(240, 19)
        Me.StatusLabel_Host.Text = "Hostname: Unknown"
        Me.StatusLabel_Host.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'StatusLabel_Uptime
        '
        Me.StatusLabel_Uptime.AutoSize = False
        Me.StatusLabel_Uptime.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.StatusLabel_Uptime.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.StatusLabel_Uptime.Image = Global.SINMx86.My.Resources.Resources.Control_Uptime
        Me.StatusLabel_Uptime.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.StatusLabel_Uptime.Margin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.StatusLabel_Uptime.Name = "StatusLabel_Uptime"
        Me.StatusLabel_Uptime.Size = New System.Drawing.Size(340, 19)
        Me.StatusLabel_Uptime.Text = "Uptime: Unknown"
        Me.StatusLabel_Uptime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'StatusLabel_ChartStatus
        '
        Me.StatusLabel_ChartStatus.AutoSize = False
        Me.StatusLabel_ChartStatus.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.StatusLabel_ChartStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.StatusLabel_ChartStatus.Image = Global.SINMx86.My.Resources.Resources.Control_Check
        Me.StatusLabel_ChartStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.StatusLabel_ChartStatus.Margin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.StatusLabel_ChartStatus.Name = "StatusLabel_ChartStatus"
        Me.StatusLabel_ChartStatus.Size = New System.Drawing.Size(343, 19)
        Me.StatusLabel_ChartStatus.Text = "Status: Chart creation failed."
        Me.StatusLabel_ChartStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'StatusLabel_TopMost
        '
        Me.StatusLabel_TopMost.AutoSize = False
        Me.StatusLabel_TopMost.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.StatusLabel_TopMost.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.StatusLabel_TopMost.Image = Global.SINMx86.My.Resources.Resources.Control_RedPin
        Me.StatusLabel_TopMost.Margin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.StatusLabel_TopMost.Name = "StatusLabel_TopMost"
        Me.StatusLabel_TopMost.Size = New System.Drawing.Size(19, 19)
        Me.StatusLabel_TopMost.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MainNotifyIcon
        '
        Me.MainNotifyIcon.ContextMenuStrip = Me.MainContextMenu
        Me.MainNotifyIcon.Icon = CType(resources.GetObject("MainNotifyIcon.Icon"), System.Drawing.Icon)
        Me.MainNotifyIcon.Text = "SINMx86"
        Me.MainNotifyIcon.Visible = True
        '
        'Value_Debug
        '
        Me.Value_Debug.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_Debug.Location = New System.Drawing.Point(732, 518)
        Me.Value_Debug.Name = "Value_Debug"
        Me.Value_Debug.Size = New System.Drawing.Size(118, 13)
        Me.Value_Debug.TabIndex = 5
        Me.Value_Debug.Text = "DEBUG_TEXT"
        Me.Value_Debug.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'MainMenu
        '
        Me.MainMenu.AutoSize = False
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MainMenuItem_Settings, Me.MainMenuItem_Chart, Me.MainMenuItem_Information, Me.ScreenshotToolStripMenuItem})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Margin = New System.Windows.Forms.Padding(0, 0, 0, 3)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Padding = New System.Windows.Forms.Padding(2)
        Me.MainMenu.ShowItemToolTips = True
        Me.MainMenu.Size = New System.Drawing.Size(944, 24)
        Me.MainMenu.TabIndex = 1
        Me.MainMenu.Text = "MainMenu"
        '
        'MainMenuItem_Settings
        '
        Me.MainMenuItem_Settings.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MainMenu_SettingsItem_TopMost, Me.MainMenu_SettingsItem_TaskbarMinimize, Me.MainMenu_SettingsItem_DisableConfirm, Me.MainMenu_SettingsItem_DisableSplash})
        Me.MainMenuItem_Settings.Name = "MainMenuItem_Settings"
        Me.MainMenuItem_Settings.Size = New System.Drawing.Size(61, 20)
        Me.MainMenuItem_Settings.Text = "&Settings"
        '
        'MainMenu_SettingsItem_TopMost
        '
        Me.MainMenu_SettingsItem_TopMost.Name = "MainMenu_SettingsItem_TopMost"
        Me.MainMenu_SettingsItem_TopMost.Size = New System.Drawing.Size(205, 22)
        Me.MainMenu_SettingsItem_TopMost.Text = "Always on &Top"
        '
        'MainMenu_SettingsItem_TaskbarMinimize
        '
        Me.MainMenu_SettingsItem_TaskbarMinimize.Name = "MainMenu_SettingsItem_TaskbarMinimize"
        Me.MainMenu_SettingsItem_TaskbarMinimize.Size = New System.Drawing.Size(205, 22)
        Me.MainMenu_SettingsItem_TaskbarMinimize.Text = "&Minimize to taskbar"
        '
        'MainMenu_SettingsItem_DisableConfirm
        '
        Me.MainMenu_SettingsItem_DisableConfirm.Name = "MainMenu_SettingsItem_DisableConfirm"
        Me.MainMenu_SettingsItem_DisableConfirm.Size = New System.Drawing.Size(205, 22)
        Me.MainMenu_SettingsItem_DisableConfirm.Text = "Disable exit &confirmation"
        '
        'MainMenu_SettingsItem_DisableSplash
        '
        Me.MainMenu_SettingsItem_DisableSplash.Name = "MainMenu_SettingsItem_DisableSplash"
        Me.MainMenu_SettingsItem_DisableSplash.Size = New System.Drawing.Size(205, 22)
        Me.MainMenu_SettingsItem_DisableSplash.Text = "Disable loading &screen"
        '
        'MainMenuItem_Chart
        '
        Me.MainMenuItem_Chart.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MainMenu_ChartItem_DownloadVisible, Me.MainMenu_ChartItem_UploadVisible, Me.MainMenu_ChartItem_Separator1, Me.MainMenu_ChartItem_ClearChart})
        Me.MainMenuItem_Chart.Name = "MainMenuItem_Chart"
        Me.MainMenuItem_Chart.Size = New System.Drawing.Size(48, 20)
        Me.MainMenuItem_Chart.Text = "&Chart"
        '
        'MainMenu_ChartItem_DownloadVisible
        '
        Me.MainMenu_ChartItem_DownloadVisible.Name = "MainMenu_ChartItem_DownloadVisible"
        Me.MainMenu_ChartItem_DownloadVisible.Size = New System.Drawing.Size(189, 22)
        Me.MainMenu_ChartItem_DownloadVisible.Text = "Show &download chart"
        '
        'MainMenu_ChartItem_UploadVisible
        '
        Me.MainMenu_ChartItem_UploadVisible.Name = "MainMenu_ChartItem_UploadVisible"
        Me.MainMenu_ChartItem_UploadVisible.Size = New System.Drawing.Size(189, 22)
        Me.MainMenu_ChartItem_UploadVisible.Text = "Show &upload chart"
        '
        'MainMenu_ChartItem_Separator1
        '
        Me.MainMenu_ChartItem_Separator1.Name = "MainMenu_ChartItem_Separator1"
        Me.MainMenu_ChartItem_Separator1.Size = New System.Drawing.Size(186, 6)
        '
        'MainMenu_ChartItem_ClearChart
        '
        Me.MainMenu_ChartItem_ClearChart.Name = "MainMenu_ChartItem_ClearChart"
        Me.MainMenu_ChartItem_ClearChart.Size = New System.Drawing.Size(189, 22)
        Me.MainMenu_ChartItem_ClearChart.Text = "&Clear chart"
        '
        'MainMenuItem_Information
        '
        Me.MainMenuItem_Information.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MainMenu_ActionItem_UpdateCheck, Me.MainMenu_ActionItem_About, Me.MainMenu_ActionItem_Separator1, Me.MainMenu_ActionItem_Exit})
        Me.MainMenuItem_Information.Name = "MainMenuItem_Information"
        Me.MainMenuItem_Information.Size = New System.Drawing.Size(54, 20)
        Me.MainMenuItem_Information.Text = "A&ction"
        '
        'MainMenu_ActionItem_UpdateCheck
        '
        Me.MainMenu_ActionItem_UpdateCheck.Name = "MainMenu_ActionItem_UpdateCheck"
        Me.MainMenu_ActionItem_UpdateCheck.Size = New System.Drawing.Size(181, 22)
        Me.MainMenu_ActionItem_UpdateCheck.Text = "Search for u&pdates..."
        '
        'MainMenu_ActionItem_About
        '
        Me.MainMenu_ActionItem_About.Name = "MainMenu_ActionItem_About"
        Me.MainMenu_ActionItem_About.Size = New System.Drawing.Size(181, 22)
        Me.MainMenu_ActionItem_About.Text = "A&bout..."
        '
        'MainMenu_ActionItem_Separator1
        '
        Me.MainMenu_ActionItem_Separator1.Name = "MainMenu_ActionItem_Separator1"
        Me.MainMenu_ActionItem_Separator1.Size = New System.Drawing.Size(178, 6)
        '
        'MainMenu_ActionItem_Exit
        '
        Me.MainMenu_ActionItem_Exit.Name = "MainMenu_ActionItem_Exit"
        Me.MainMenu_ActionItem_Exit.Size = New System.Drawing.Size(181, 22)
        Me.MainMenu_ActionItem_Exit.Text = "E&xit"
        '
        'ScreenshotToolStripMenuItem
        '
        Me.ScreenshotToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ScreenshotToolStripMenuItem.Image = Global.SINMx86.My.Resources.Resources.Control_Screenshot
        Me.ScreenshotToolStripMenuItem.Name = "ScreenshotToolStripMenuItem"
        Me.ScreenshotToolStripMenuItem.Size = New System.Drawing.Size(28, 20)
        '
        'GroupBox_DiskInfo
        '
        Me.GroupBox_DiskInfo.Controls.Add(Me.Value_PartLabel)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Value_PartInfo)
        Me.GroupBox_DiskInfo.Controls.Add(Me.ComboBox_PartList)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Button_DiskListReload)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Value_DiskSerial)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Name_DiskSerial)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Value_DiskFirmware)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Name_PartList)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Name_DiskFirmware)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Value_DiskInterface)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Value_DiskCapacity)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Name_DiskInterface)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Name_DiskCapacity)
        Me.GroupBox_DiskInfo.Controls.Add(Me.ComboBox_DiskList)
        Me.GroupBox_DiskInfo.Controls.Add(Me.Name_DiskList)
        Me.GroupBox_DiskInfo.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_DiskInfo.Location = New System.Drawing.Point(12, 381)
        Me.GroupBox_DiskInfo.Name = "GroupBox_DiskInfo"
        Me.GroupBox_DiskInfo.Size = New System.Drawing.Size(455, 122)
        Me.GroupBox_DiskInfo.TabIndex = 9
        Me.GroupBox_DiskInfo.TabStop = False
        Me.GroupBox_DiskInfo.Text = "Disk information"
        '
        'Value_PartLabel
        '
        Me.Value_PartLabel.AutoSize = True
        Me.Value_PartLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Value_PartLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_PartLabel.Location = New System.Drawing.Point(231, 94)
        Me.Value_PartLabel.Name = "Value_PartLabel"
        Me.Value_PartLabel.Size = New System.Drawing.Size(30, 13)
        Me.Value_PartLabel.TabIndex = 13
        Me.Value_PartLabel.Text = "N/A"
        '
        'Value_PartInfo
        '
        Me.Value_PartInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_PartInfo.Location = New System.Drawing.Point(267, 94)
        Me.Value_PartInfo.Name = "Value_PartInfo"
        Me.Value_PartInfo.Size = New System.Drawing.Size(170, 13)
        Me.Value_PartInfo.TabIndex = 14
        Me.Value_PartInfo.Text = "Unknown"
        Me.Value_PartInfo.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Button_DiskListReload
        '
        Me.Button_DiskListReload.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_DiskListReload.Image = Global.SINMx86.My.Resources.Resources.Control_Refresh
        Me.Button_DiskListReload.Location = New System.Drawing.Point(414, 21)
        Me.Button_DiskListReload.Name = "Button_DiskListReload"
        Me.Button_DiskListReload.Size = New System.Drawing.Size(23, 23)
        Me.Button_DiskListReload.TabIndex = 2
        Me.Button_DiskListReload.UseVisualStyleBackColor = True
        '
        'Value_DiskSerial
        '
        Me.Value_DiskSerial.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_DiskSerial.Location = New System.Drawing.Point(315, 71)
        Me.Value_DiskSerial.Name = "Value_DiskSerial"
        Me.Value_DiskSerial.Size = New System.Drawing.Size(122, 13)
        Me.Value_DiskSerial.TabIndex = 10
        Me.Value_DiskSerial.Text = "1234567890ABCDEF"
        Me.Value_DiskSerial.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_DiskSerial
        '
        Me.Name_DiskSerial.AutoSize = True
        Me.Name_DiskSerial.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_DiskSerial.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_DiskSerial.Location = New System.Drawing.Point(231, 71)
        Me.Name_DiskSerial.Name = "Name_DiskSerial"
        Me.Name_DiskSerial.Size = New System.Drawing.Size(88, 13)
        Me.Name_DiskSerial.TabIndex = 9
        Me.Name_DiskSerial.Text = "Serial number:"
        '
        'Value_DiskFirmware
        '
        Me.Value_DiskFirmware.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_DiskFirmware.Location = New System.Drawing.Point(111, 71)
        Me.Value_DiskFirmware.Name = "Value_DiskFirmware"
        Me.Value_DiskFirmware.Size = New System.Drawing.Size(114, 13)
        Me.Value_DiskFirmware.TabIndex = 8
        Me.Value_DiskFirmware.Text = "1000"
        Me.Value_DiskFirmware.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_PartList
        '
        Me.Name_PartList.AutoSize = True
        Me.Name_PartList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_PartList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_PartList.Location = New System.Drawing.Point(15, 94)
        Me.Name_PartList.Name = "Name_PartList"
        Me.Name_PartList.Size = New System.Drawing.Size(52, 13)
        Me.Name_PartList.TabIndex = 11
        Me.Name_PartList.Text = "Volume:"
        '
        'Name_DiskFirmware
        '
        Me.Name_DiskFirmware.AutoSize = True
        Me.Name_DiskFirmware.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_DiskFirmware.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_DiskFirmware.Location = New System.Drawing.Point(15, 71)
        Me.Name_DiskFirmware.Name = "Name_DiskFirmware"
        Me.Name_DiskFirmware.Size = New System.Drawing.Size(61, 13)
        Me.Name_DiskFirmware.TabIndex = 7
        Me.Name_DiskFirmware.Text = "Firmware:"
        '
        'Value_DiskInterface
        '
        Me.Value_DiskInterface.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_DiskInterface.Location = New System.Drawing.Point(111, 48)
        Me.Value_DiskInterface.Name = "Value_DiskInterface"
        Me.Value_DiskInterface.Size = New System.Drawing.Size(114, 13)
        Me.Value_DiskInterface.TabIndex = 4
        Me.Value_DiskInterface.Text = "SCSI / SAS"
        Me.Value_DiskInterface.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Value_DiskCapacity
        '
        Me.Value_DiskCapacity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Value_DiskCapacity.Location = New System.Drawing.Point(329, 48)
        Me.Value_DiskCapacity.Name = "Value_DiskCapacity"
        Me.Value_DiskCapacity.Size = New System.Drawing.Size(108, 13)
        Me.Value_DiskCapacity.TabIndex = 6
        Me.Value_DiskCapacity.Text = "999 GB"
        Me.Value_DiskCapacity.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Name_DiskInterface
        '
        Me.Name_DiskInterface.AutoSize = True
        Me.Name_DiskInterface.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_DiskInterface.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_DiskInterface.Location = New System.Drawing.Point(15, 48)
        Me.Name_DiskInterface.Name = "Name_DiskInterface"
        Me.Name_DiskInterface.Size = New System.Drawing.Size(62, 13)
        Me.Name_DiskInterface.TabIndex = 3
        Me.Name_DiskInterface.Text = "Interface:"
        '
        'Name_DiskCapacity
        '
        Me.Name_DiskCapacity.AutoSize = True
        Me.Name_DiskCapacity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_DiskCapacity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_DiskCapacity.Location = New System.Drawing.Point(231, 48)
        Me.Name_DiskCapacity.Name = "Name_DiskCapacity"
        Me.Name_DiskCapacity.Size = New System.Drawing.Size(60, 13)
        Me.Name_DiskCapacity.TabIndex = 5
        Me.Name_DiskCapacity.Text = "Capacity:"
        '
        'Name_DiskList
        '
        Me.Name_DiskList.AutoSize = True
        Me.Name_DiskList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name_DiskList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name_DiskList.Location = New System.Drawing.Point(15, 25)
        Me.Name_DiskList.Name = "Name_DiskList"
        Me.Name_DiskList.Size = New System.Drawing.Size(68, 13)
        Me.Name_DiskList.TabIndex = 0
        Me.Name_DiskList.Text = "Disk drive:"
        '
        'ComboBox_PartList
        '
        Me.ComboBox_PartList.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBox_PartList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBox_PartList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_PartList.FormattingEnabled = True
        Me.ComboBox_PartList.ItemHeight = 15
        Me.ComboBox_PartList.Location = New System.Drawing.Point(111, 91)
        Me.ComboBox_PartList.Name = "ComboBox_PartList"
        Me.ComboBox_PartList.Size = New System.Drawing.Size(114, 21)
        Me.ComboBox_PartList.TabIndex = 12
        '
        'ComboBox_DiskList
        '
        Me.ComboBox_DiskList.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBox_DiskList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBox_DiskList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_DiskList.FormattingEnabled = True
        Me.ComboBox_DiskList.ItemHeight = 15
        Me.ComboBox_DiskList.Location = New System.Drawing.Point(111, 22)
        Me.ComboBox_DiskList.Name = "ComboBox_DiskList"
        Me.ComboBox_DiskList.Size = New System.Drawing.Size(297, 21)
        Me.ComboBox_DiskList.TabIndex = 1
        '
        'ComboBox_LanguageList
        '
        Me.ComboBox_LanguageList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBox_LanguageList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_LanguageList.FormattingEnabled = True
        Me.ComboBox_LanguageList.ItemHeight = 15
        Me.ComboBox_LanguageList.Location = New System.Drawing.Point(82, 515)
        Me.ComboBox_LanguageList.Name = "ComboBox_LanguageList"
        Me.ComboBox_LanguageList.Size = New System.Drawing.Size(93, 21)
        Me.ComboBox_LanguageList.TabIndex = 3
        '
        'ComboBox_InterfaceList
        '
        Me.ComboBox_InterfaceList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBox_InterfaceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_InterfaceList.FormattingEnabled = True
        Me.ComboBox_InterfaceList.Location = New System.Drawing.Point(111, 22)
        Me.ComboBox_InterfaceList.Name = "ComboBox_InterfaceList"
        Me.ComboBox_InterfaceList.Size = New System.Drawing.Size(297, 21)
        Me.ComboBox_InterfaceList.TabIndex = 1
        '
        'ComboBox_UpdateList
        '
        Me.ComboBox_UpdateList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBox_UpdateList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_UpdateList.DropDownWidth = 46
        Me.ComboBox_UpdateList.FormattingEnabled = True
        Me.ComboBox_UpdateList.Location = New System.Drawing.Point(381, 360)
        Me.ComboBox_UpdateList.Name = "ComboBox_UpdateList"
        Me.ComboBox_UpdateList.Size = New System.Drawing.Size(41, 21)
        Me.ComboBox_UpdateList.TabIndex = 18
        '
        'ComboBox_VideoList
        '
        Me.ComboBox_VideoList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBox_VideoList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_VideoList.FormattingEnabled = True
        Me.ComboBox_VideoList.Location = New System.Drawing.Point(111, 22)
        Me.ComboBox_VideoList.Name = "ComboBox_VideoList"
        Me.ComboBox_VideoList.Size = New System.Drawing.Size(297, 21)
        Me.ComboBox_VideoList.TabIndex = 1
        '
        'ComboBox_CPUList
        '
        Me.ComboBox_CPUList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBox_CPUList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_CPUList.FormattingEnabled = True
        Me.ComboBox_CPUList.Location = New System.Drawing.Point(111, 22)
        Me.ComboBox_CPUList.Name = "ComboBox_CPUList"
        Me.ComboBox_CPUList.Size = New System.Drawing.Size(326, 21)
        Me.ComboBox_CPUList.TabIndex = 1
        '
        'ComboBox_HWList
        '
        Me.ComboBox_HWList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBox_HWList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_HWList.FormattingEnabled = True
        Me.ComboBox_HWList.Items.AddRange(New Object() {"1", "2", "3"})
        Me.ComboBox_HWList.Location = New System.Drawing.Point(111, 22)
        Me.ComboBox_HWList.Name = "ComboBox_HWList"
        Me.ComboBox_HWList.Size = New System.Drawing.Size(326, 21)
        Me.ComboBox_HWList.TabIndex = 1
        '
        'MainWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(944, 567)
        Me.ContextMenuStrip = Me.MainContextMenu
        Me.Controls.Add(Me.GroupBox_DiskInfo)
        Me.Controls.Add(Me.Value_Debug)
        Me.Controls.Add(Me.MainStatusStrip)
        Me.Controls.Add(Me.MainMenu)
        Me.Controls.Add(Me.Link_Bottom)
        Me.Controls.Add(Me.Name_LanguageList)
        Me.Controls.Add(Me.ComboBox_LanguageList)
        Me.Controls.Add(Me.GroupBox_Network)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.GroupBox_VideoInfo)
        Me.Controls.Add(Me.GroupBox_MemoryInfo)
        Me.Controls.Add(Me.GroupBox_CPUInfo)
        Me.Controls.Add(Me.GroupBox_OSInfo)
        Me.Controls.Add(Me.GroupBox_HWInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MainMenu
        Me.MaximizeBox = False
        Me.Name = "MainWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SINMx86 - System Information and Network Monitor"
        Me.GroupBox_HWInfo.ResumeLayout(False)
        Me.GroupBox_HWInfo.PerformLayout()
        Me.GroupBox_OSInfo.ResumeLayout(False)
        Me.GroupBox_OSInfo.PerformLayout()
        Me.GroupBox_CPUInfo.ResumeLayout(False)
        Me.GroupBox_CPUInfo.PerformLayout()
        Me.GroupBox_MemoryInfo.ResumeLayout(False)
        Me.GroupBox_MemoryInfo.PerformLayout()
        Me.GroupBox_VideoInfo.ResumeLayout(False)
        Me.GroupBox_VideoInfo.PerformLayout()
        Me.GroupBox_Network.ResumeLayout(False)
        Me.GroupBox_Network.PerformLayout()
        CType(Me.PictureBox_TrafficChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainStatusStrip.ResumeLayout(False)
        Me.MainStatusStrip.PerformLayout()
        Me.MainContextMenu.ResumeLayout(False)
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.GroupBox_DiskInfo.ResumeLayout(False)
        Me.GroupBox_DiskInfo.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox_HWInfo As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_OSInfo As System.Windows.Forms.GroupBox
    Friend WithEvents Value_OSRelease As System.Windows.Forms.Label
    Friend WithEvents Value_OSName As System.Windows.Forms.Label
    Friend WithEvents Name_OSRelease As System.Windows.Forms.Label
    Friend WithEvents Name_OSName As System.Windows.Forms.Label
    Friend WithEvents GroupBox_CPUInfo As System.Windows.Forms.GroupBox
    Friend WithEvents Value_CPUClock As System.Windows.Forms.Label
    Friend WithEvents Name_CPUClock As System.Windows.Forms.Label
    Friend WithEvents Name_CPUList As System.Windows.Forms.Label
    Friend WithEvents GroupBox_MemoryInfo As System.Windows.Forms.GroupBox
    Friend WithEvents Value_VirtualMemorySize As System.Windows.Forms.Label
    Friend WithEvents Value_PhysicalMemorySize As System.Windows.Forms.Label
    Friend WithEvents Name_VirtualMemory As System.Windows.Forms.Label
    Friend WithEvents Name_PhysicalMemory As System.Windows.Forms.Label
    Friend WithEvents GroupBox_VideoInfo As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Exit As System.Windows.Forms.Button
    Friend WithEvents Name_InterfaceList As System.Windows.Forms.Label
    Friend WithEvents ComboBox_InterfaceList As LeftComboBox
    Friend WithEvents Name_Bandwidth As System.Windows.Forms.Label
    Friend WithEvents Name_DownloadSpeed As System.Windows.Forms.Label
    Friend WithEvents Name_UploadSpeed As System.Windows.Forms.Label
    Friend WithEvents GroupBox_Network As System.Windows.Forms.GroupBox
    Public WithEvents EventTimer As System.Windows.Forms.Timer
    Friend WithEvents Name_InterfaceUsage As System.Windows.Forms.Label
    Friend WithEvents ComboBox_LanguageList As LeftComboBox
    Friend WithEvents Name_LanguageList As System.Windows.Forms.Label
    Friend WithEvents Name_UpdateList As System.Windows.Forms.Label
    Friend WithEvents Name_ChartVisible As System.Windows.Forms.Label
    Friend WithEvents CheckBoxChart_UploadVisible As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxChart_DownloadVisible As System.Windows.Forms.CheckBox
    Friend WithEvents EventToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents Link_Bottom As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox_TrafficChart As System.Windows.Forms.PictureBox
    Friend WithEvents Value_HWVendor As System.Windows.Forms.Label
    Friend WithEvents Name_HWVendor As System.Windows.Forms.Label
    Friend WithEvents MainStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLabel_Uptime As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusLabel_Host As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Button_InterfaceListReload As System.Windows.Forms.Button
    Friend WithEvents StatusLabel_ChartStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusLabel_TopMost As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MainNotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents MainContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MainContextMenuItem_TopMost As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainContextMenuItem_DisableConfirm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainContextMenuItem_Separator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MainContextMenuItem_UpdateCheck As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Value_OSVersion As System.Windows.Forms.Label
    Friend WithEvents Name_OSVersion As System.Windows.Forms.Label
    Friend WithEvents Value_CPUCore As System.Windows.Forms.Label
    Friend WithEvents Name_CPUCore As System.Windows.Forms.Label
    Friend WithEvents MainContextMenuItem_Exit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Value_Debug As System.Windows.Forms.Label
    Friend WithEvents Value_VirtualMemoryUsage As System.Windows.Forms.Label
    Friend WithEvents Value_PhysicalMemoryUsage As System.Windows.Forms.Label
    Friend WithEvents Name_VirtualMemoryUsage As System.Windows.Forms.Label
    Friend WithEvents Name_PhysicalMemoryUsage As System.Windows.Forms.Label
    Friend WithEvents Name_UpdateUnit As System.Windows.Forms.Label
    Friend WithEvents ComboBox_UpdateList As RightComboBox
    Friend WithEvents MainContextMenuItem_TaskbarMinimize As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainContextMenuItem_DisableSplash As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainContextMenuItem_Separator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MainContextMenuItem_About As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Name_VirtualMemoryFree As System.Windows.Forms.Label
    Friend WithEvents Name_PhysicalMemoryFree As System.Windows.Forms.Label
    Friend WithEvents Name_OSLang As System.Windows.Forms.Label
    Friend WithEvents Value_OSLang As System.Windows.Forms.Label
    Friend WithEvents Value_PhysicalMemoryFree As System.Windows.Forms.Label
    Friend WithEvents Value_VirtualMemoryFree As System.Windows.Forms.Label
    Friend WithEvents Value_CPUMaxClock As System.Windows.Forms.Label
    Friend WithEvents Name_CPUMaxClock As System.Windows.Forms.Label
    Friend WithEvents Value_DownloadSpeed As System.Windows.Forms.Label
    Friend WithEvents Value_Bandwidth As System.Windows.Forms.Label
    Friend WithEvents Value_UploadSpeed As System.Windows.Forms.Label
    Friend WithEvents Value_InterfaceUsage As System.Windows.Forms.Label
    Friend WithEvents Value_DownloadSpeedUnit As System.Windows.Forms.Label
    Friend WithEvents Value_BandwidthUnit As System.Windows.Forms.Label
    Friend WithEvents Value_UploadSpeedUnit As System.Windows.Forms.Label
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents MainMenuItem_Settings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenuItem_Chart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenuItem_Information As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_SettingsItem_TopMost As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_SettingsItem_TaskbarMinimize As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_SettingsItem_DisableConfirm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_SettingsItem_DisableSplash As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_ChartItem_DownloadVisible As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_ChartItem_UploadVisible As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_ChartItem_ClearChart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_ActionItem_UpdateCheck As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_ActionItem_About As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu_ActionItem_Separator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MainMenu_ActionItem_Exit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Name_VideoList As System.Windows.Forms.Label
    Friend WithEvents GroupBox_DiskInfo As System.Windows.Forms.GroupBox
    Friend WithEvents Name_DiskList As System.Windows.Forms.Label
    Friend WithEvents Value_VideoResolution As System.Windows.Forms.Label
    Friend WithEvents Value_VideoMemory As System.Windows.Forms.Label
    Friend WithEvents Name_VideoResolution As System.Windows.Forms.Label
    Friend WithEvents Name_VideoMemory As System.Windows.Forms.Label
    Friend WithEvents ComboBox_DiskList As LeftComboBox
    Friend WithEvents Value_DiskSerial As System.Windows.Forms.Label
    Friend WithEvents Name_DiskSerial As System.Windows.Forms.Label
    Friend WithEvents Value_DiskFirmware As System.Windows.Forms.Label
    Friend WithEvents Name_PartList As System.Windows.Forms.Label
    Friend WithEvents Name_DiskFirmware As System.Windows.Forms.Label
    Friend WithEvents Value_DiskInterface As System.Windows.Forms.Label
    Friend WithEvents Value_DiskCapacity As System.Windows.Forms.Label
    Friend WithEvents Name_DiskInterface As System.Windows.Forms.Label
    Friend WithEvents Name_DiskCapacity As System.Windows.Forms.Label
    Friend WithEvents Button_DiskListReload As System.Windows.Forms.Button
    Friend WithEvents ScreenshotToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComboBox_VideoList As LeftComboBox
    Friend WithEvents Button_VideoListReload As System.Windows.Forms.Button
    Friend WithEvents ComboBox_CPUList As LeftComboBox
    Friend WithEvents MainMenu_ChartItem_Separator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Name_HWList As System.Windows.Forms.Label
    Friend WithEvents ComboBox_HWList As SINMx86.LeftComboBox
    Friend WithEvents Name_HWIdentifier As System.Windows.Forms.Label
    Friend WithEvents Value_HWIdentifier As System.Windows.Forms.Label
    Friend WithEvents ComboBox_PartList As SINMx86.LeftComboBox
    Friend WithEvents Value_PartInfo As System.Windows.Forms.Label
    Friend WithEvents Value_PartLabel As System.Windows.Forms.Label

End Class

Public Class LeftComboBox
    Inherits ComboBox
    Sub New()
        Me.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        Me.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub CenteredComboBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()
        Dim txt As String = ""
        If e.Index >= 0 Then txt = Me.Items(e.Index).ToString
        TextRenderer.DrawText(e.Graphics, txt, e.Font, e.Bounds, e.ForeColor, TextFormatFlags.Left)
        e.DrawFocusRectangle()
    End Sub
End Class

Public Class RightComboBox
    Inherits ComboBox
    Sub New()
        Me.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        Me.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub CenteredComboBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()
        Dim txt As String = ""
        If e.Index >= 0 Then txt = Me.Items(e.Index).ToString
        TextRenderer.DrawText(e.Graphics, txt, e.Font, e.Bounds, e.ForeColor, TextFormatFlags.Right)
        e.DrawFocusRectangle()
    End Sub
End Class