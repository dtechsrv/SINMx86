<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RAMInfo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RAMInfo))
        Me.RAM_Table = New System.Windows.Forms.ListView()
        Me.Dummy = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Description = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Value = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox_Table = New System.Windows.Forms.GroupBox()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.GroupBox_Table.SuspendLayout()
        Me.SuspendLayout()
        '
        'RAM_Table
        '
        Me.RAM_Table.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Dummy, Me.Description, Me.Value})
        Me.RAM_Table.FullRowSelect = True
        Me.RAM_Table.GridLines = True
        Me.RAM_Table.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.RAM_Table.Location = New System.Drawing.Point(10, 23)
        Me.RAM_Table.Name = "RAM_Table"
        Me.RAM_Table.ShowGroups = False
        Me.RAM_Table.Size = New System.Drawing.Size(454, 249)
        Me.RAM_Table.TabIndex = 0
        Me.RAM_Table.UseCompatibleStateImageBehavior = False
        Me.RAM_Table.View = System.Windows.Forms.View.Details
        '
        'Dummy
        '
        Me.Dummy.DisplayIndex = 2
        Me.Dummy.Tag = "Dummy"
        Me.Dummy.Text = ""
        Me.Dummy.Width = 0
        '
        'Description
        '
        Me.Description.DisplayIndex = 0
        Me.Description.Tag = "Description"
        Me.Description.Text = "Description"
        Me.Description.Width = 150
        '
        'Value
        '
        Me.Value.DisplayIndex = 1
        Me.Value.Tag = "Value"
        Me.Value.Text = "Value"
        Me.Value.Width = 300
        '
        'GroupBox_Table
        '
        Me.GroupBox_Table.Controls.Add(Me.RAM_Table)
        Me.GroupBox_Table.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox_Table.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Table.Name = "GroupBox_Table"
        Me.GroupBox_Table.Size = New System.Drawing.Size(474, 283)
        Me.GroupBox_Table.TabIndex = 0
        Me.GroupBox_Table.TabStop = False
        Me.GroupBox_Table.Text = "Memory module - Selected memory module name"
        '
        'Button_Close
        '
        Me.Button_Close.Location = New System.Drawing.Point(411, 305)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 1
        Me.Button_Close.Text = "&Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'RAMInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(498, 338)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.GroupBox_Table)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RAMInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SINMx86 - Memory module details"
        Me.TopMost = True
        Me.GroupBox_Table.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RAM_Table As System.Windows.Forms.ListView
    Friend WithEvents Description As System.Windows.Forms.ColumnHeader
    Friend WithEvents Value As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox_Table As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents Dummy As System.Windows.Forms.ColumnHeader
End Class
