<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SmartWindow
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SmartWindow))
        Me.SMART_Table = New System.Windows.Forms.ListView()
        Me.Dummy = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Number = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Record = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Treshold = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Value = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Worst = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Data = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox_Table = New System.Windows.Forms.GroupBox()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.GroupBox_Table.SuspendLayout()
        Me.SuspendLayout()
        '
        'SMART_Table
        '
        Me.SMART_Table.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Dummy, Me.Number, Me.Record, Me.Treshold, Me.Value, Me.Worst, Me.Data})
        Me.SMART_Table.FullRowSelect = True
        Me.SMART_Table.GridLines = True
        Me.SMART_Table.Location = New System.Drawing.Point(10, 23)
        Me.SMART_Table.Name = "SMART_Table"
        Me.SMART_Table.ShowGroups = False
        Me.SMART_Table.Size = New System.Drawing.Size(696, 300)
        Me.SMART_Table.TabIndex = 0
        Me.SMART_Table.UseCompatibleStateImageBehavior = False
        Me.SMART_Table.View = System.Windows.Forms.View.Details
        '
        'Dummy
        '
        Me.Dummy.DisplayIndex = 6
        Me.Dummy.Tag = "Dummy"
        Me.Dummy.Text = ""
        Me.Dummy.Width = 0
        '
        'Number
        '
        Me.Number.DisplayIndex = 0
        Me.Number.Tag = "Number"
        Me.Number.Text = "#"
        Me.Number.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Number.Width = 35
        '
        'Record
        '
        Me.Record.DisplayIndex = 1
        Me.Record.Tag = "Record"
        Me.Record.Text = "Record"
        Me.Record.Width = 250
        '
        'Treshold
        '
        Me.Treshold.DisplayIndex = 2
        Me.Treshold.Tag = "Treshold"
        Me.Treshold.Text = "Treshold"
        Me.Treshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Treshold.Width = 80
        '
        'Value
        '
        Me.Value.DisplayIndex = 3
        Me.Value.Tag = "Value"
        Me.Value.Text = "Value"
        Me.Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Value.Width = 80
        '
        'Worst
        '
        Me.Worst.DisplayIndex = 4
        Me.Worst.Tag = "Worst"
        Me.Worst.Text = "Worst"
        Me.Worst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Worst.Width = 80
        '
        'Data
        '
        Me.Data.DisplayIndex = 5
        Me.Data.Tag = "Data"
        Me.Data.Text = "Data"
        Me.Data.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Data.Width = 150
        '
        'GroupBox_Table
        '
        Me.GroupBox_Table.Controls.Add(Me.SMART_Table)
        Me.GroupBox_Table.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_Table.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Table.Name = "GroupBox_Table"
        Me.GroupBox_Table.Size = New System.Drawing.Size(716, 334)
        Me.GroupBox_Table.TabIndex = 1
        Me.GroupBox_Table.TabStop = False
        Me.GroupBox_Table.Text = "S.M.A.R.T - Selected disk name"
        '
        'Button_Close
        '
        Me.Button_Close.Location = New System.Drawing.Point(653, 356)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 0
        Me.Button_Close.Text = "&Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'SmartWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(740, 389)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.GroupBox_Table)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SmartWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "S.M.A.R.T information"
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
    Friend WithEvents Dummy As System.Windows.Forms.ColumnHeader
End Class
