<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btRefresh = New System.Windows.Forms.Button()
        Me.lvDriveInfo = New System.Windows.Forms.ListView()
        Me._hidden = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Partition = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ParentDrive = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.DriveType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VolumeName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.FileSystem = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.DriveSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.FreeSpace = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.BOOT0 = New System.Windows.Forms.RadioButton()
        Me.BOOT1 = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BackupLocationPathTextbox = New System.Windows.Forms.TextBox()
        Me.Browse = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RAWNAND = New System.Windows.Forms.RadioButton()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btRefresh
        '
        Me.btRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btRefresh.Location = New System.Drawing.Point(676, 274)
        Me.btRefresh.Margin = New System.Windows.Forms.Padding(2)
        Me.btRefresh.Name = "btRefresh"
        Me.btRefresh.Size = New System.Drawing.Size(58, 27)
        Me.btRefresh.TabIndex = 1
        Me.btRefresh.Text = "Refresh"
        Me.btRefresh.UseVisualStyleBackColor = True
        '
        'lvDriveInfo
        '
        Me.lvDriveInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvDriveInfo.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._hidden, Me.Partition, Me.ParentDrive, Me.DriveType, Me.VolumeName, Me.FileSystem, Me.DriveSize, Me.FreeSpace})
        Me.lvDriveInfo.FullRowSelect = True
        Me.lvDriveInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvDriveInfo.Location = New System.Drawing.Point(8, 31)
        Me.lvDriveInfo.Margin = New System.Windows.Forms.Padding(2)
        Me.lvDriveInfo.MultiSelect = False
        Me.lvDriveInfo.Name = "lvDriveInfo"
        Me.lvDriveInfo.Size = New System.Drawing.Size(726, 239)
        Me.lvDriveInfo.TabIndex = 2
        Me.lvDriveInfo.UseCompatibleStateImageBehavior = False
        Me.lvDriveInfo.View = System.Windows.Forms.View.Details
        '
        '_hidden
        '
        Me._hidden.Width = 0
        '
        'Partition
        '
        Me.Partition.Text = "Partition"
        Me.Partition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.Partition.Width = 50
        '
        'ParentDrive
        '
        Me.ParentDrive.Text = "Parent Drive"
        Me.ParentDrive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ParentDrive.Width = 80
        '
        'DriveType
        '
        Me.DriveType.Text = "Drive Type"
        Me.DriveType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.DriveType.Width = 80
        '
        'VolumeName
        '
        Me.VolumeName.Text = "Volume Name"
        Me.VolumeName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.VolumeName.Width = 80
        '
        'FileSystem
        '
        Me.FileSystem.Text = "Filesystem"
        Me.FileSystem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.FileSystem.Width = 88
        '
        'DriveSize
        '
        Me.DriveSize.Text = "Drive Size"
        Me.DriveSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.DriveSize.Width = 82
        '
        'FreeSpace
        '
        Me.FreeSpace.Text = "Free Space"
        Me.FreeSpace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.FreeSpace.Width = 105
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(24, 485)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox1.Size = New System.Drawing.Size(493, 20)
        Me.TextBox1.TabIndex = 3
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(139, 378)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 30)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Backup"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(188, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Please Select SXOS Drive To Backup"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(186, 513)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox2.Size = New System.Drawing.Size(242, 20)
        Me.TextBox2.TabIndex = 6
        '
        'BOOT0
        '
        Me.BOOT0.AutoSize = True
        Me.BOOT0.Location = New System.Drawing.Point(28, 20)
        Me.BOOT0.Name = "BOOT0"
        Me.BOOT0.Size = New System.Drawing.Size(61, 17)
        Me.BOOT0.TabIndex = 7
        Me.BOOT0.TabStop = True
        Me.BOOT0.Text = "BOOT0"
        Me.BOOT0.UseVisualStyleBackColor = True
        '
        'BOOT1
        '
        Me.BOOT1.AutoSize = True
        Me.BOOT1.Location = New System.Drawing.Point(135, 20)
        Me.BOOT1.Name = "BOOT1"
        Me.BOOT1.Size = New System.Drawing.Size(61, 17)
        Me.BOOT1.TabIndex = 8
        Me.BOOT1.TabStop = True
        Me.BOOT1.Text = "BOOT1"
        Me.BOOT1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(39, 278)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Backup Location"
        '
        'BackupLocationPathTextbox
        '
        Me.BackupLocationPathTextbox.Location = New System.Drawing.Point(133, 275)
        Me.BackupLocationPathTextbox.Name = "BackupLocationPathTextbox"
        Me.BackupLocationPathTextbox.ReadOnly = True
        Me.BackupLocationPathTextbox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BackupLocationPathTextbox.Size = New System.Drawing.Size(459, 20)
        Me.BackupLocationPathTextbox.TabIndex = 11
        '
        'Browse
        '
        Me.Browse.Location = New System.Drawing.Point(598, 275)
        Me.Browse.Name = "Browse"
        Me.Browse.Size = New System.Drawing.Size(75, 23)
        Me.Browse.TabIndex = 12
        Me.Browse.Text = "Browse"
        Me.Browse.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(656, 482)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 13
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RAWNAND)
        Me.GroupBox1.Controls.Add(Me.BOOT1)
        Me.GroupBox1.Controls.Add(Me.BOOT0)
        Me.GroupBox1.Location = New System.Drawing.Point(225, 301)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(336, 71)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Please Select Option"
        '
        'RAWNAND
        '
        Me.RAWNAND.AutoSize = True
        Me.RAWNAND.Location = New System.Drawing.Point(227, 20)
        Me.RAWNAND.Name = "RAWNAND"
        Me.RAWNAND.Size = New System.Drawing.Size(82, 17)
        Me.RAWNAND.TabIndex = 9
        Me.RAWNAND.TabStop = True
        Me.RAWNAND.Text = "RAWNAND"
        Me.RAWNAND.UseVisualStyleBackColor = True
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(245, 407)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox3.Size = New System.Drawing.Size(252, 20)
        Me.TextBox3.TabIndex = 16
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(245, 404)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(252, 23)
        Me.ProgressBar1.TabIndex = 17
        '
        'Cancel
        '
        Me.Cancel.Location = New System.Drawing.Point(593, 425)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(138, 24)
        Me.Cancel.TabIndex = 20
        Me.Cancel.Text = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(503, 378)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(89, 30)
        Me.Button6.TabIndex = 15
        Me.Button6.Text = "Restore"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(245, 433)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox5.Size = New System.Drawing.Size(252, 20)
        Me.TextBox5.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(187, 414)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 13)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "Progress"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(187, 436)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Filesize"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(743, 543)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Browse)
        Me.Controls.Add(Me.BackupLocationPathTextbox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.lvDriveInfo)
        Me.Controls.Add(Me.btRefresh)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmMain"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SXOS Hidden Emunand Backup Tool 2.0"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btRefresh As System.Windows.Forms.Button
    Friend WithEvents lvDriveInfo As System.Windows.Forms.ListView
    Friend WithEvents _hidden As System.Windows.Forms.ColumnHeader
    Friend WithEvents ParentDrive As System.Windows.Forms.ColumnHeader
    Friend WithEvents Partition As System.Windows.Forms.ColumnHeader
    Friend WithEvents DriveType As System.Windows.Forms.ColumnHeader
    Friend WithEvents DriveSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents FreeSpace As System.Windows.Forms.ColumnHeader
    Friend WithEvents FileSystem As System.Windows.Forms.ColumnHeader
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents VolumeName As ColumnHeader
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents BOOT0 As RadioButton
    Friend WithEvents BOOT1 As RadioButton
    Friend WithEvents Label2 As Label
    Friend WithEvents BackupLocationPathTextbox As TextBox
    Friend WithEvents Browse As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents Button2 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents RAWNAND As RadioButton
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Cancel As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
End Class
