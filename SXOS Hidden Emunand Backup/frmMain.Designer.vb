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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.RefreshDrives = New System.Windows.Forms.Button()
        Me.lvDriveInfo = New System.Windows.Forms.ListView()
        Me._hidden = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Partition = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ParentDrive = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.DriveType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VolumeName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.FileSystem = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.DriveSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.FreeSpace = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Backup = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BOOT0 = New System.Windows.Forms.RadioButton()
        Me.BOOT1 = New System.Windows.Forms.RadioButton()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RAWNANDSPLIT = New System.Windows.Forms.RadioButton()
        Me.LocationTextBox = New System.Windows.Forms.TextBox()
        Me.RAWNAND = New System.Windows.Forms.RadioButton()
        Me.TotalProgressBar = New System.Windows.Forms.ProgressBar()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.Restore = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Browse = New System.Windows.Forms.Button()
        Me.SaveLocLabel = New System.Windows.Forms.Label()
        Me.Percent = New System.Windows.Forms.Label()
        Me.RestorePathDebug = New System.Windows.Forms.TextBox()
        Me.DebugButton = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BackupPathDebug = New System.Windows.Forms.TextBox()
        Me.ConsoleTextBox = New System.Windows.Forms.TextBox()
        Me.BackGroundRetoreProgress = New System.ComponentModel.BackgroundWorker()
        Me.BackgroundRestore = New System.ComponentModel.BackgroundWorker()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.FileProgressBar = New System.Windows.Forms.ProgressBar()
        Me.MessageLabel1 = New System.Windows.Forms.Label()
        Me.BackgroundDeleteSplitFiles = New System.ComponentModel.BackgroundWorker()
        Me.BackupLocationPathLabel = New System.Windows.Forms.Label()
        Me.HEADER = New System.Windows.Forms.RadioButton()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RefreshDrives
        '
        Me.RefreshDrives.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RefreshDrives.Location = New System.Drawing.Point(7, 203)
        Me.RefreshDrives.Margin = New System.Windows.Forms.Padding(2)
        Me.RefreshDrives.Name = "RefreshDrives"
        Me.RefreshDrives.Size = New System.Drawing.Size(77, 20)
        Me.RefreshDrives.TabIndex = 1
        Me.RefreshDrives.Text = "Refresh List"
        Me.RefreshDrives.UseVisualStyleBackColor = True
        '
        'lvDriveInfo
        '
        Me.lvDriveInfo.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._hidden, Me.Partition, Me.ParentDrive, Me.DriveType, Me.VolumeName, Me.FileSystem, Me.DriveSize, Me.FreeSpace})
        Me.lvDriveInfo.FullRowSelect = True
        Me.lvDriveInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvDriveInfo.Location = New System.Drawing.Point(7, 38)
        Me.lvDriveInfo.Margin = New System.Windows.Forms.Padding(2)
        Me.lvDriveInfo.MultiSelect = False
        Me.lvDriveInfo.Name = "lvDriveInfo"
        Me.lvDriveInfo.Size = New System.Drawing.Size(571, 160)
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
        'Backup
        '
        Me.Backup.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Backup.Location = New System.Drawing.Point(243, 312)
        Me.Backup.Name = "Backup"
        Me.Backup.Size = New System.Drawing.Size(61, 30)
        Me.Backup.TabIndex = 4
        Me.Backup.Text = "Backup"
        Me.Backup.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(136, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(339, 25)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "PLEASE SELECT SXOS DRIVE"
        '
        'BOOT0
        '
        Me.BOOT0.AutoSize = True
        Me.BOOT0.Checked = True
        Me.BOOT0.Location = New System.Drawing.Point(93, 45)
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
        Me.BOOT1.Location = New System.Drawing.Point(160, 45)
        Me.BOOT1.Name = "BOOT1"
        Me.BOOT1.Size = New System.Drawing.Size(61, 17)
        Me.BOOT1.TabIndex = 8
        Me.BOOT1.Text = "BOOT1"
        Me.BOOT1.UseVisualStyleBackColor = True
        '
        'FolderBrowserDialog1
        '
        Me.FolderBrowserDialog1.Description = "Select your backup location."
        Me.FolderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.HEADER)
        Me.GroupBox1.Controls.Add(Me.RAWNANDSPLIT)
        Me.GroupBox1.Controls.Add(Me.LocationTextBox)
        Me.GroupBox1.Controls.Add(Me.RAWNAND)
        Me.GroupBox1.Controls.Add(Me.BOOT1)
        Me.GroupBox1.Controls.Add(Me.BOOT0)
        Me.GroupBox1.Location = New System.Drawing.Point(67, 228)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(465, 71)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Please Select Option"
        '
        'RAWNANDSPLIT
        '
        Me.RAWNANDSPLIT.AutoSize = True
        Me.RAWNANDSPLIT.Location = New System.Drawing.Point(344, 46)
        Me.RAWNANDSPLIT.Name = "RAWNANDSPLIT"
        Me.RAWNANDSPLIT.Size = New System.Drawing.Size(115, 17)
        Me.RAWNANDSPLIT.TabIndex = 12
        Me.RAWNANDSPLIT.TabStop = True
        Me.RAWNANDSPLIT.Text = "RAWNAND SPLIT"
        Me.RAWNANDSPLIT.UseVisualStyleBackColor = True
        '
        'LocationTextBox
        '
        Me.LocationTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LocationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LocationTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LocationTextBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.LocationTextBox.Location = New System.Drawing.Point(166, 19)
        Me.LocationTextBox.Name = "LocationTextBox"
        Me.LocationTextBox.ReadOnly = True
        Me.LocationTextBox.Size = New System.Drawing.Size(159, 20)
        Me.LocationTextBox.TabIndex = 11
        Me.LocationTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RAWNAND
        '
        Me.RAWNAND.AutoSize = True
        Me.RAWNAND.Location = New System.Drawing.Point(227, 45)
        Me.RAWNAND.Name = "RAWNAND"
        Me.RAWNAND.Size = New System.Drawing.Size(111, 17)
        Me.RAWNAND.TabIndex = 9
        Me.RAWNAND.Text = "RAWNAND FULL"
        Me.RAWNAND.UseVisualStyleBackColor = True
        '
        'TotalProgressBar
        '
        Me.TotalProgressBar.BackColor = System.Drawing.Color.White
        Me.TotalProgressBar.Location = New System.Drawing.Point(243, 351)
        Me.TotalProgressBar.Name = "TotalProgressBar"
        Me.TotalProgressBar.Size = New System.Drawing.Size(252, 23)
        Me.TotalProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.TotalProgressBar.TabIndex = 17
        '
        'Cancel
        '
        Me.Cancel.Enabled = False
        Me.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel.Location = New System.Drawing.Point(437, 312)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(58, 30)
        Me.Cancel.TabIndex = 20
        Me.Cancel.Text = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = True
        '
        'Restore
        '
        Me.Restore.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Restore.Location = New System.Drawing.Point(344, 312)
        Me.Restore.Name = "Restore"
        Me.Restore.Size = New System.Drawing.Size(56, 30)
        Me.Restore.TabIndex = 15
        Me.Restore.Text = "Restore"
        Me.Restore.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(191, 358)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "Progress:"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(7, 305)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(176, 186)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 23
        Me.PictureBox1.TabStop = False
        '
        'Browse
        '
        Me.Browse.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Browse.Location = New System.Drawing.Point(194, 440)
        Me.Browse.Name = "Browse"
        Me.Browse.Size = New System.Drawing.Size(56, 20)
        Me.Browse.TabIndex = 12
        Me.Browse.Text = "Browse"
        Me.Browse.UseVisualStyleBackColor = True
        '
        'SaveLocLabel
        '
        Me.SaveLocLabel.AutoSize = True
        Me.SaveLocLabel.Location = New System.Drawing.Point(191, 424)
        Me.SaveLocLabel.Name = "SaveLocLabel"
        Me.SaveLocLabel.Size = New System.Drawing.Size(82, 13)
        Me.SaveLocLabel.TabIndex = 25
        Me.SaveLocLabel.Text = "Save Location: "
        '
        'Percent
        '
        Me.Percent.AutoSize = True
        Me.Percent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Percent.Location = New System.Drawing.Point(499, 358)
        Me.Percent.Name = "Percent"
        Me.Percent.Size = New System.Drawing.Size(0, 13)
        Me.Percent.TabIndex = 26
        '
        'RestorePathDebug
        '
        Me.RestorePathDebug.Location = New System.Drawing.Point(695, 61)
        Me.RestorePathDebug.Name = "RestorePathDebug"
        Me.RestorePathDebug.ReadOnly = True
        Me.RestorePathDebug.Size = New System.Drawing.Size(347, 20)
        Me.RestorePathDebug.TabIndex = 28
        '
        'DebugButton
        '
        Me.DebugButton.BackColor = System.Drawing.SystemColors.Desktop
        Me.DebugButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DebugButton.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.DebugButton.Location = New System.Drawing.Point(551, 203)
        Me.DebugButton.Name = "DebugButton"
        Me.DebugButton.Size = New System.Drawing.Size(27, 32)
        Me.DebugButton.TabIndex = 29
        Me.DebugButton.Text = ">"
        Me.DebugButton.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(601, 64)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(94, 13)
        Me.Label5.TabIndex = 31
        Me.Label5.Text = "Restore Command"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(601, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 33
        Me.Label2.Text = "Backup Command"
        '
        'BackupPathDebug
        '
        Me.BackupPathDebug.Location = New System.Drawing.Point(695, 35)
        Me.BackupPathDebug.Name = "BackupPathDebug"
        Me.BackupPathDebug.ReadOnly = True
        Me.BackupPathDebug.Size = New System.Drawing.Size(347, 20)
        Me.BackupPathDebug.TabIndex = 32
        '
        'ConsoleTextBox
        '
        Me.ConsoleTextBox.BackColor = System.Drawing.SystemColors.Desktop
        Me.ConsoleTextBox.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ConsoleTextBox.Location = New System.Drawing.Point(606, 119)
        Me.ConsoleTextBox.Multiline = True
        Me.ConsoleTextBox.Name = "ConsoleTextBox"
        Me.ConsoleTextBox.ReadOnly = True
        Me.ConsoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ConsoleTextBox.Size = New System.Drawing.Size(436, 341)
        Me.ConsoleTextBox.TabIndex = 34
        '
        'BackGroundRetoreProgress
        '
        Me.BackGroundRetoreProgress.WorkerReportsProgress = True
        Me.BackGroundRetoreProgress.WorkerSupportsCancellation = True
        '
        'BackgroundRestore
        '
        Me.BackgroundRestore.WorkerReportsProgress = True
        Me.BackgroundRestore.WorkerSupportsCancellation = True
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Button1.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Button1.Location = New System.Drawing.Point(995, 96)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(47, 23)
        Me.Button1.TabIndex = 35
        Me.Button1.Text = "clear"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'FileProgressBar
        '
        Me.FileProgressBar.BackColor = System.Drawing.Color.White
        Me.FileProgressBar.Location = New System.Drawing.Point(243, 379)
        Me.FileProgressBar.Name = "FileProgressBar"
        Me.FileProgressBar.Size = New System.Drawing.Size(252, 10)
        Me.FileProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.FileProgressBar.TabIndex = 36
        '
        'MessageLabel1
        '
        Me.MessageLabel1.AutoSize = True
        Me.MessageLabel1.Location = New System.Drawing.Point(243, 397)
        Me.MessageLabel1.Name = "MessageLabel1"
        Me.MessageLabel1.Size = New System.Drawing.Size(0, 13)
        Me.MessageLabel1.TabIndex = 37
        '
        'BackgroundDeleteSplitFiles
        '
        Me.BackgroundDeleteSplitFiles.WorkerReportsProgress = True
        Me.BackgroundDeleteSplitFiles.WorkerSupportsCancellation = True
        '
        'BackupLocationPathLabel
        '
        Me.BackupLocationPathLabel.AutoSize = True
        Me.BackupLocationPathLabel.Location = New System.Drawing.Point(270, 424)
        Me.BackupLocationPathLabel.Name = "BackupLocationPathLabel"
        Me.BackupLocationPathLabel.Size = New System.Drawing.Size(0, 13)
        Me.BackupLocationPathLabel.TabIndex = 38
        '
        'HEADER
        '
        Me.HEADER.AutoSize = True
        Me.HEADER.Location = New System.Drawing.Point(6, 45)
        Me.HEADER.Name = "HEADER"
        Me.HEADER.Size = New System.Drawing.Size(70, 17)
        Me.HEADER.TabIndex = 13
        Me.HEADER.TabStop = True
        Me.HEADER.Text = "HEADER"
        Me.HEADER.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(1059, 532)
        Me.Controls.Add(Me.BackupLocationPathLabel)
        Me.Controls.Add(Me.MessageLabel1)
        Me.Controls.Add(Me.FileProgressBar)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ConsoleTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BackupPathDebug)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.DebugButton)
        Me.Controls.Add(Me.RestorePathDebug)
        Me.Controls.Add(Me.Percent)
        Me.Controls.Add(Me.SaveLocLabel)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.TotalProgressBar)
        Me.Controls.Add(Me.Restore)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Browse)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Backup)
        Me.Controls.Add(Me.lvDriveInfo)
        Me.Controls.Add(Me.RefreshDrives)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmMain"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SXOS Hidden Emunand Backup Tool 2.0"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RefreshDrives As System.Windows.Forms.Button
    Friend WithEvents lvDriveInfo As System.Windows.Forms.ListView
    Friend WithEvents _hidden As System.Windows.Forms.ColumnHeader
    Friend WithEvents ParentDrive As System.Windows.Forms.ColumnHeader
    Friend WithEvents Partition As System.Windows.Forms.ColumnHeader
    Friend WithEvents DriveType As System.Windows.Forms.ColumnHeader
    Friend WithEvents DriveSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents FreeSpace As System.Windows.Forms.ColumnHeader
    Friend WithEvents FileSystem As System.Windows.Forms.ColumnHeader
    Friend WithEvents Backup As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents VolumeName As ColumnHeader
    Friend WithEvents BOOT0 As RadioButton
    Friend WithEvents BOOT1 As RadioButton
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents RAWNAND As RadioButton
    Friend WithEvents TotalProgressBar As ProgressBar
    Friend WithEvents Cancel As Button
    Friend WithEvents Restore As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents LocationTextBox As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Browse As Button
    Friend WithEvents SaveLocLabel As Label
    Friend WithEvents Percent As Label
    Friend WithEvents RestorePathDebug As TextBox
    Friend WithEvents DebugButton As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents BackupPathDebug As TextBox
    Friend WithEvents ConsoleTextBox As TextBox
    Friend WithEvents RAWNANDSPLIT As RadioButton
    Friend WithEvents BackGroundRetoreProgress As System.ComponentModel.BackgroundWorker
    Friend WithEvents BackgroundRestore As System.ComponentModel.BackgroundWorker
    Friend WithEvents Button1 As Button
    Friend WithEvents FileProgressBar As ProgressBar
    Friend WithEvents MessageLabel1 As Label
    Friend WithEvents BackgroundDeleteSplitFiles As System.ComponentModel.BackgroundWorker
    Friend WithEvents BackupLocationPathLabel As Label
    Friend WithEvents HEADER As RadioButton
End Class
