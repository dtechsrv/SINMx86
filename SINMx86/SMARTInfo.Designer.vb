<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SMARTInfo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SMARTInfo))
        Me.SMART_Table = New System.Windows.Forms.ListView()
        Me.Image = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Number = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Record = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Value = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Treshold = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Worst = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Status = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Data = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SMART_Status = New System.Windows.Forms.ImageList(Me.components)
        Me.GroupBox_Table = New System.Windows.Forms.GroupBox()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.GroupBox_Table.SuspendLayout()
        Me.SuspendLayout()
        '
        'SMART_Table
        '
        Me.SMART_Table.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Image, Me.Number, Me.Record, Me.Treshold, Me.Value, Me.Worst, Me.Status, Me.Data})
        Me.SMART_Table.FullRowSelect = True
        Me.SMART_Table.GridLines = True
        Me.SMART_Table.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.SMART_Table.Location = New System.Drawing.Point(10, 23)
        Me.SMART_Table.Name = "SMART_Table"
        Me.SMART_Table.Size = New System.Drawing.Size(821, 300)
        Me.SMART_Table.StateImageList = Me.SMART_Status
        Me.SMART_Table.TabIndex = 0
        Me.SMART_Table.UseCompatibleStateImageBehavior = False
        Me.SMART_Table.View = System.Windows.Forms.View.Details
        '
        'Image
        '
        Me.Image.Tag = "Image"
        Me.Image.Text = ""
        Me.Image.Width = 22
        '
        'Number
        '
        Me.Number.Tag = "Number"
        Me.Number.Text = "#"
        Me.Number.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Number.Width = 35
        '
        'Record
        '
        Me.Record.Tag = "Record"
        Me.Record.Text = "Record"
        Me.Record.Width = 260
        '
        'Value
        '
        Me.Value.Tag = "Value"
        Me.Value.Text = "Value"
        Me.Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Value.Width = 75
        '
        'Treshold
        '
        Me.Treshold.Tag = "Treshold"
        Me.Treshold.Text = "Treshold"
        Me.Treshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Treshold.Width = 75
        '
        'Worst
        '
        Me.Worst.Tag = "Worst"
        Me.Worst.Text = "Worst"
        Me.Worst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Worst.Width = 75
        '
        'Status
        '
        Me.Status.Tag = "Status"
        Me.Status.Text = "Status"
        Me.Status.Width = 100
        '
        'Data
        '
        Me.Data.Tag = "Data"
        Me.Data.Text = "Data"
        Me.Data.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Data.Width = 175
        '
        'SMART_Status
        '
        Me.SMART_Status.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.SMART_Status.ImageSize = New System.Drawing.Size(16, 16)
        Me.SMART_Status.TransparentColor = System.Drawing.Color.Transparent
        '
        'GroupBox_Table
        '
        Me.GroupBox_Table.Controls.Add(Me.SMART_Table)
        Me.GroupBox_Table.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_Table.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Table.Name = "GroupBox_Table"
        Me.GroupBox_Table.Size = New System.Drawing.Size(841, 334)
        Me.GroupBox_Table.TabIndex = 0
        Me.GroupBox_Table.TabStop = False
        Me.GroupBox_Table.Text = "S.M.A.R.T - Selected disk name"
        '
        'Button_Close
        '
        Me.Button_Close.Location = New System.Drawing.Point(778, 356)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 1
        Me.Button_Close.Text = "&Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'SMARTInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(865, 389)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.GroupBox_Table)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SMARTInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SINMx86 - S.M.A.R.T table"
        Me.TopMost = True
        Me.GroupBox_Table.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SMART_Table As System.Windows.Forms.ListView
    Friend WithEvents Number As System.Windows.Forms.ColumnHeader
    Friend WithEvents Record As System.Windows.Forms.ColumnHeader
    Friend WithEvents Treshold As System.Windows.Forms.ColumnHeader
    Friend WithEvents Value As System.Windows.Forms.ColumnHeader
    Friend WithEvents Worst As System.Windows.Forms.ColumnHeader
    Friend WithEvents Data As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox_Table As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents Status As System.Windows.Forms.ColumnHeader
    Friend WithEvents SMART_Status As System.Windows.Forms.ImageList
    Friend WithEvents Image As System.Windows.Forms.ColumnHeader
End Class