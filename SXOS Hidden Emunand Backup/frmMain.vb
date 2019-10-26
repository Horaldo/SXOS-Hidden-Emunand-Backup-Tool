Imports System.IO
Imports System.Linq


Public Class frmMain
    Dim SXOSDriveLetter As String
    Dim SXOSDrivePhysicalName As String
    Dim SXOSDriveType As String
    Dim SXOSDriveVolumeName As String
    Dim SXOSDriveFileSystem As String

    Dim Backupcommand As String

    Dim SplitFileNum As Integer

    Dim BackupcommandSplit0 As String
    Dim BackupcommandSplit1 As String
    Dim BackupcommandSplit2 As String
    Dim BackupcommandSplit3 As String
    Dim BackupcommandSplit4 As String
    Dim BackupcommandSplit5 As String
    Dim BackupcommandSplit6 As String
    Dim BackupcommandSplit7 As String

    Dim RestoreCommand As String
    Dim FolderLocation As String
    Dim FolderString As String
    Dim appPath As String = Application.StartupPath()
    Dim BinaryName As String
    Dim BinaryFileSize As Long
    Dim TotalFileSize As Int64
    Dim CanceledOperation As Integer
    Dim CanceledRawsplitBackup As Integer
    Dim CanceledRestoreOperation As Integer
    Dim deleteFileName As String

    Dim Restoring As Boolean
    Dim ToggleDebug As Boolean
    Dim AppClosing As Boolean
    Dim DeletingFile As Boolean
    Dim p As Process()


    Private WithEvents Timer1 As New Timer With {.Interval = 50}
    Private stp As Integer = 0


    Private Sub GetDriveInfo()

        Dim count As Int32 = 0
        Dim driveInfoEx As New clsDiskInfoEx
        Dim multipleParents() As String = Nothing
        Dim parentDrives As String = Nothing

        lvDriveInfo.Items.Clear()

        For Each drive As System.IO.DriveInfo In System.IO.DriveInfo.GetDrives
            If drive.DriveType = IO.DriveType.Removable Then


                parentDrives = driveInfoEx.GetPhysicalDiskParentFor(drive.RootDirectory.ToString)
                If parentDrives.Contains(", ") Then
                    ' We have multiple parent drives:
                    multipleParents = Split(parentDrives, ", ")
                    ' Enumerate them backwards so the lowest numbered drives are reported 1st
                    For more As Int32 = (multipleParents.Length - 1) To 0 Step -1
                        If more = (multipleParents.Length - 1) Then
                            lvDriveInfo.Items.Add("")
                            lvDriveInfo.Items(count).SubItems.Add(drive.RootDirectory.ToString.Replace("\", ""))
                            lvDriveInfo.Items(count).SubItems.Add(multipleParents(more)) ' Parent drive
                            lvDriveInfo.Items(count).SubItems.Add(drive.DriveType.ToString)

                            If drive.IsReady Then
                                lvDriveInfo.Items(count).SubItems.Add(drive.DriveFormat)
                                lvDriveInfo.Items(count).SubItems.Add((drive.TotalSize / 1000000).ToString("N0") & " MB")
                                lvDriveInfo.Items(count).SubItems.Add((drive.AvailableFreeSpace / 1000000).ToString("N0") & " MB")

                            Else
                                lvDriveInfo.Items(count).SubItems.Add("No Disc")
                                lvDriveInfo.Items(count).SubItems.Add("-")
                                lvDriveInfo.Items(count).SubItems.Add("-")
                            End If
                        Else
                            count += 1
                            lvDriveInfo.Items.Add("")
                            lvDriveInfo.Items(count).SubItems.Add(drive.RootDirectory.ToString.Replace("\", ""))
                            lvDriveInfo.Items(count).SubItems.Add(multipleParents(more)) ' Parent drive
                            lvDriveInfo.Items(count).SubItems.Add("Mirror or Span")
                            If drive.IsReady Then
                                lvDriveInfo.Items(count).SubItems.Add(drive.DriveFormat)
                            Else
                                lvDriveInfo.Items(count).SubItems.Add("")
                            End If
                            lvDriveInfo.Items(count).SubItems.Add("-")
                            lvDriveInfo.Items(count).SubItems.Add("-")
                        End If
                    Next
                    count += 1
                Else
                    ' Just one parent drive for this partition.
                    lvDriveInfo.Items.Add("")
                    lvDriveInfo.Items(count).SubItems.Add(drive.RootDirectory.ToString.Replace("\", ""))
                    lvDriveInfo.Items(count).SubItems.Add(parentDrives) ' Parent drive
                    lvDriveInfo.Items(count).SubItems.Add(drive.DriveType.ToString)

                    If drive.IsReady Then
                        lvDriveInfo.Items(count).SubItems.Add(drive.VolumeLabel.ToString)
                        lvDriveInfo.Items(count).SubItems.Add(drive.DriveFormat)
                        lvDriveInfo.Items(count).SubItems.Add((drive.TotalSize / 1000000).ToString("N0") & " MB")
                        lvDriveInfo.Items(count).SubItems.Add((drive.AvailableFreeSpace / 1000000).ToString("N0") & " MB")

                    Else
                        lvDriveInfo.Items(count).SubItems.Add("No Disc")
                        lvDriveInfo.Items(count).SubItems.Add("-")
                        lvDriveInfo.Items(count).SubItems.Add("-")
                    End If
                    count += 1
                End If

            End If
        Next
    End Sub

    Private Sub FrmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Dim cellWidth As Integer = 0
        For count As UShort = 1 To 7
            If count = 1 Then
                cellWidth = CInt(lvDriveInfo.Width / 7)
                cellWidth = CInt(cellWidth * 0.65)
            ElseIf count = 2 Then
                cellWidth = CInt(lvDriveInfo.Width / 7)
                cellWidth = CInt(cellWidth * 1.0)
            ElseIf count = 4 Then
                cellWidth = CInt(lvDriveInfo.Width / 7)
                cellWidth = CInt(cellWidth * 0.85)
            ElseIf count = 6 Then
                cellWidth = CInt(lvDriveInfo.Width / 7)
                If (cellWidth - 5) > 0 Then
                    cellWidth = cellWidth - 7
                End If
            Else
                cellWidth = CInt(lvDriveInfo.Width / 7)
            End If

            lvDriveInfo.Columns(count).Width = cellWidth
        Next

    End Sub

    Private Sub FrmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        ' Trigger form.resize()
        Me.Width += 1
        Me.Refresh()

        GetDriveInfo()
        CalculateFileSelection()
    End Sub

    Private Sub RefreshDrives_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshDrives.Click
        GetDriveInfo()
    End Sub

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TotalProgressBar.ForeColor = Color.Green
        'FileProgressBar.ForeColor = Color.Black
        Control.CheckForIllegalCrossThreadCalls = False
        AppClosing = False


        Me.Size = New Size(600, 510)
        Me.MaximumSize = New Size(1075, 510)
        Me.MinimumSize = Me.Size

        Dim SetupPath As String = Application.StartupPath & "\secinspect.exe"

        If System.IO.File.Exists(SetupPath) <> True Then

            Using sCreateMSIFile As New FileStream(SetupPath, FileMode.Create)
                sCreateMSIFile.Write(My.Resources.secinspect, 0, My.Resources.secinspect.Length)
                IO.File.SetAttributes(SetupPath, IO.FileAttributes.Hidden)

            End Using
        End If

    End Sub

    Private Sub MyApplication_FormClosing(sender As Object, e As EventArgs) Handles Me.FormClosing

        AppClosing = True
        KillProcess()

        Dim FileToDelete As String = My.Application.Info.DirectoryPath + "\secinspect.exe"

        Dim ProcessRunning As Boolean
        ProcessRunning = True
        Do Until ProcessRunning = False
            Dim listProc() As System.Diagnostics.Process
            listProc = Process.GetProcessesByName("secinspect")
            If listProc.Length > 0 Then
                ProcessRunning = True
            Else
                ProcessRunning = False
            End If
        Loop

        If System.IO.File.Exists(FileToDelete) = True Then
            System.IO.File.Delete(FileToDelete)
        End If
        End

    End Sub

    Private Sub LvDriveInfo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvDriveInfo.SelectedIndexChanged
        With Me.lvDriveInfo
            Dim i As Integer
            For Each item As ListViewItem In lvDriveInfo.SelectedItems
                i = item.Index
            Next

            Dim innercounter As Integer = 0
            For Each subItem As ListViewItem.ListViewSubItem In lvDriveInfo.Items(i).SubItems
                Dim myString As String = lvDriveInfo.Items(i).SubItems(innercounter).Text

                'Select case to select which colunm of data you want to store in variables drom listview.
                Select Case innercounter
                    Case 1
                        SXOSDriveLetter = myString

                    Case 2

                        SXOSDrivePhysicalName = myString.Replace(" ", "")

                    Case 3
                        SXOSDriveType = myString

                    Case 4
                        SXOSDriveVolumeName = myString

                    Case 5
                        SXOSDriveFileSystem = myString

                End Select

                innercounter += 1
            Next
        End With
        CalculateFileSelection()
    End Sub

    Private Sub Backup_Click(sender As Object, e As EventArgs) Handles Backup.Click
        Restoring = False
        If SXOSDriveLetter = Nothing Then
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("Please select the SXOS drive to backup")
            Exit Sub
        Else
            If LocationTextBox.Text = "TXNAND" Then

                If BackupLocationPathLabel.Text = "" Then
                    My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                    If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        FolderLocation = FolderBrowserDialog1.SelectedPath
                        FolderString = FolderLocation
                        If FolderString.EndsWith("\") = False Then
                            FolderString = FolderLocation & "\"
                        End If
                        BackupLocationPathLabel.Text = FolderString
                        CalculateFileSelection()
                    End If

                    If BackupLocationPathLabel.Text = "" Then
                        Exit Sub
                    Else
                        OverwriteCheck()
                    End If

                Else

                    OverwriteCheck()

                End If
            Else
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                MsgBox("The selected drive does not contain an SXOS Emunand")
                Exit Sub

            End If

        End If

    End Sub


    Private Sub OverwriteCheck()

        Dim ProcessRunning As Boolean
        ProcessRunning = True

        If BOOT0.Checked = True Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "BOOT0.BIN") Then
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                Select Case MsgBox("BOOT0.BIN already exists. Overwrite?", MsgBoxStyle.YesNoCancel, "FILE ALREADY EXISTS")
                    Case MsgBoxResult.Yes
                        If System.IO.File.Exists(FolderLocation + "\BOOT0.BIN") = True Then

                            Do Until ProcessRunning = False
                                Dim listProc() As System.Diagnostics.Process
                                listProc = Process.GetProcessesByName("secinspect")
                                If listProc.Length > 0 Then
                                    ProcessRunning = True
                                Else
                                    ProcessRunning = False
                                End If
                            Loop

                            System.IO.File.Delete(FolderLocation + "\BOOT0.BIN")
                        End If
                        DoBackup()
                    Case MsgBoxResult.Cancel
                        Exit Sub
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                DoBackup()
            End If
        End If

        If BOOT1.Checked = True Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "BOOT1.BIN") Then
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                Select Case MsgBox("BOOT1.BIN already exists. Overwrite?", MsgBoxStyle.YesNoCancel, "FILE ALREADY EXISTS")
                    Case MsgBoxResult.Yes
                        If System.IO.File.Exists(FolderLocation + "\BOOT1.BIN") = True Then

                            Do Until ProcessRunning = False
                                Dim listProc() As System.Diagnostics.Process
                                listProc = Process.GetProcessesByName("secinspect")
                                If listProc.Length > 0 Then
                                    ProcessRunning = True
                                Else
                                    ProcessRunning = False
                                End If
                            Loop

                            System.IO.File.Delete(FolderLocation + "\BOOT1.BIN")
                        End If
                        DoBackup()
                    Case MsgBoxResult.Cancel
                        Exit Sub
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                DoBackup()
            End If
        End If

        If RAWNAND.Checked = True Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "RAWNAND.BIN") Then
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                Select Case MsgBox("RAWNAND.BIN already exists. Overwrite?", MsgBoxStyle.YesNoCancel, "FILE ALREADY EXISTS")
                    Case MsgBoxResult.Yes
                        If System.IO.File.Exists(FolderLocation + "\RAWNAND.BIN") = True Then

                            Do Until ProcessRunning = False
                                Dim listProc() As System.Diagnostics.Process
                                listProc = Process.GetProcessesByName("secinspect")
                                If listProc.Length > 0 Then
                                    ProcessRunning = True
                                Else
                                    ProcessRunning = False
                                End If
                            Loop

                            System.IO.File.Delete(FolderLocation + "\RAWNAND.BIN")
                        End If
                        DoBackup()
                    Case MsgBoxResult.Cancel
                        Exit Sub
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                DoBackup()
            End If
        End If


        If RAWNANDSPLIT.Checked = True Then
            CanceledRawsplitBackup = 0
            If My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "full.00.BIN") Or My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "full.01.BIN") Or My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "full.02.BIN") Or My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "full.03.BIN") Or My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "full.04.BIN") Or My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "full.05.BIN") Or My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "full.06.BIN") Or My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "full.07.BIN") Then
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                Select Case MsgBox("Split RAWNAND dump files already exists. Overwrite?", MsgBoxStyle.YesNoCancel, "FILE ALREADY EXISTS")
                    Case MsgBoxResult.Yes
                        Percent.Text = "0 %"
                        Cancel.Enabled = True
                        Restore.Enabled = False
                        Backup.Enabled = False
                        Browse.Enabled = False
                        RefreshDrives.Enabled = False
                        lvDriveInfo.Enabled = False
                        HEADER.Enabled = False
                        BOOT0.Enabled = False
                        BOOT1.Enabled = False
                        RAWNAND.Enabled = False
                        RAWNANDSPLIT.Enabled = False
                        BackgroundDeleteSplitFiles.RunWorkerAsync()
                        ' DoBackup()     -----   DoBackup()  is called when BackgroundDelete completes
                    Case MsgBoxResult.Cancel
                        Exit Sub
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                DoBackup()
            End If
        End If
    End Sub

    Private Sub DoBackup()


        CalculateFileSelection()

        Dim myProcessStartInfo As New ProcessStartInfo()

        With myProcessStartInfo
            .FileName = "secinspect.exe"
            .Arguments = Backupcommand
            '.Verb = "runas"

            .CreateNoWindow = True
            .UseShellExecute = False
        End With

        Process.Start(myProcessStartInfo)



        Fileprogress()


    End Sub

    Private Sub RestoreCheck()
        If BOOT0.Checked = True Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "BOOT0.BIN") Then
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                Select Case MsgBox("Are you sure you want to restore BOOT0.BIN to " & LocationTextBox.Text & "?", MsgBoxStyle.YesNoCancel, "CONFIRM RESTORE")
                    Case MsgBoxResult.Yes
                        DoRestore()
                    Case MsgBoxResult.Cancel
                        Exit Sub
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                MsgBox("BOOT0.BIN not found in " & BackupLocationPathLabel.Text)
            End If
        End If

        If BOOT1.Checked = True Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "BOOT1.BIN") Then
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                Select Case MsgBox("Are you sure you want to restore BOOT1.BIN to " & LocationTextBox.Text & "?", MsgBoxStyle.YesNoCancel, "CONFIRM RESTORE")
                    Case MsgBoxResult.Yes
                        DoRestore()
                    Case MsgBoxResult.Cancel
                        Exit Sub
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                MsgBox("BOOT1.BIN not found in " & BackupLocationPathLabel.Text)
            End If
        End If

        '*********************************************************************************************************************
        'This Code To Be Modified so it Can split the RAWNAND.BIN into SPlit files to support Restoring.
        'Alternatively Do we only allow split file dumping with an option to merge once all parts are dumped?
        'Merge of file  can be done in cmd prompt with this command: copy /b full.*.bin RAWNAND.bin
        'We Need a to prompt user if they are restoring to existing hidden SXOS Emunand SD Card Or Using a New SD Card
        'If New SD CARD is used then they must also restore Header + Boot0 + Boot 1
        'If user has made changes to only rawnand using hackdiskmount then Split Rawnand  & restore back the split files only (EG Inserted Online Account Manually
        'Restore creates a .DSK file for each setcor your restoring. Final Size should be same as .bin been restored
        'You should be able to use that to calculate Progress Bar on Restore,
        'Potentially Delete .DSK files upon successfull restore to free up Space, Total Space Consumed during restore 30GB
        'Can market .DSK files as a feature. " Screwed your restore? dont panic, Just restore the DSK file and changes will be reverted"
        'I Will need a couple dumps of header files from Different SD Formats example FAT32. EXFAT Both With Hidden EMunand & also Files EMunand as created by SXOS
        'This is required so i can see what changes are made to switch from a hidden emunand / files emunand without format Hopefullly
        '*********************************************************************************************************************

        If RAWNAND.Checked = True Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "RAWNAND.BIN") Then
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                Select Case MsgBox("Are you sure you want to restore RAWNAND.BIN to " & LocationTextBox.Text & "?", MsgBoxStyle.YesNoCancel, "CONFIRM RESTORE")
                    Case MsgBoxResult.Yes
                        DoRestore()
                    Case MsgBoxResult.Cancel
                        Exit Sub
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                MsgBox("RAWNAND.BIN not found in " & BackupLocationPathLabel.Text)
            End If
        End If
        If RAWNANDSPLIT.Checked = True Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathLabel.Text + "full.00.BIN") Then
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                Select Case MsgBox("Split RAWNAND restore is not currently supported", MsgBoxStyle.OkOnly, "Feature missing")
                    Case MsgBoxResult.Ok
                        Exit Sub
                End Select
            Else
                DoBackup()
            End If
        End If
    End Sub


    Private Sub DoRestore()
        CalculateFileSelection()
        MessageLabel1.Text = "Restoring... Please Wait."
        TotalProgressBar.Style = ProgressBarStyle.Marquee

        BackgroundRestore.RunWorkerAsync()

        MyRestoreProgress()
    End Sub



    Private Sub BOOT0_CheckedChanged(sender As Object, e As EventArgs) Handles BOOT0.CheckedChanged
        CalculateFileSelection()
    End Sub

    Private Sub BOOT1_CheckedChanged(sender As Object, e As EventArgs) Handles BOOT1.CheckedChanged
        CalculateFileSelection()
    End Sub
    Private Sub RAWNAND_CheckedChanged(sender As Object, e As EventArgs) Handles RAWNAND.CheckedChanged
        CalculateFileSelection()
        SplitFileNum = 0
    End Sub
    Private Sub Browse_Click(sender As Object, e As EventArgs) Handles Browse.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            FolderLocation = FolderBrowserDialog1.SelectedPath
            FolderString = FolderLocation
            If FolderString.EndsWith("\") = False Then
                FolderString = FolderLocation & "\"
            End If
            BackupLocationPathLabel.Text = FolderString
            CalculateFileSelection()
        End If
    End Sub

    Private Sub Restore_Click(sender As Object, e As EventArgs) Handles Restore.Click

        MsgBox("Restoring is not supported yet")
        Exit Sub

        Restoring = True
        If SXOSDriveLetter = Nothing Then
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("Please select the SXOS drive to restore to")
            Exit Sub
        Else
            If LocationTextBox.Text = "TXNAND" Then

                If BackupLocationPathLabel.Text = "" Then
                    My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                    If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        FolderLocation = FolderBrowserDialog1.SelectedPath
                        FolderString = FolderLocation
                        If FolderString.EndsWith("\") = False Then
                            FolderString = FolderLocation & "\"
                        End If
                        BackupLocationPathLabel.Text = FolderString
                        CalculateFileSelection()
                    End If

                    If BackupLocationPathLabel.Text = "" Then
                        Exit Sub
                    Else
                        RestoreCheck()
                    End If

                Else

                    RestoreCheck()

                End If
            Else
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                MsgBox("The selected drive is not SXOS Emunand ready!")
                Exit Sub

            End If

        End If

    End Sub


    Private Sub CalculateFileSelection()
        LocationTextBox.Text = SXOSDriveVolumeName


        If HEADER.Checked = True Then
            Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "HEADER.BIN" + """ " + "0 2"
            RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "HEADER.BIN" + """ " + "0 CONFIRM"
            BinaryName = "HEADER.BIN"
            BinaryFileSize = 1024
        End If

        If BOOT0.Checked = True Then
            Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "BOOT0.BIN" + """ " + "2 8192"
            RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "BOOT0.BIN" + """ " + "2 CONFIRM"
            BinaryName = "BOOT0.BIN"
            BinaryFileSize = 4194304
        End If

        If BOOT1.Checked = True Then
            Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "BOOT1.BIN" + """ " + "8194 8192"
            RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "BOOT1.BIN" + """ " + "8194 CONFIRM"
            BinaryName = "BOOT1.BIN"
            BinaryFileSize = 4194304
        End If

        If RAWNAND.Checked = True Then
            Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "RAWNAND.BIN" + """ " + "16386 61071360"
            RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "RAWNAND.BIN" + """ " + "16386 CONFIRM"
            BinaryName = "RAWNAND.BIN"
            BinaryFileSize = 31268536320
        End If

        If RAWNAND.Checked = True Then
            Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "RAWNAND.BIN" + """ " + "16386 61071360"
            RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "RAWNAND.BIN" + """ " + "16386 CONFIRM"
            BinaryName = "RAWNAND.BIN"
            BinaryFileSize = 31268536320
        End If

        If RAWNANDSPLIT.Checked = True Then
            CalculateSplitFiles()

        End If

        BackupPathDebug.Text = Backupcommand
        RestorePathDebug.Text = RestoreCommand

    End Sub

    Dim DoubleBytes As Double
    Private this As Object

    Public Sub CalculateSplitFiles()


        Select Case SplitFileNum
            Case 0
                Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.00.bin" + """ " + "16386 8388352"
                RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.00.bin" + """ " + "16386 CONFIRM"
                BinaryName = "full.00.bin"
                BinaryFileSize = 4294836224
            Case 1
                Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.01.bin" + """ " + "8404738 8388352"
                RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.01.bin" + """ " + "8404738 CONFIRM"
                BinaryName = "full.01.bin"
                BinaryFileSize = 4294836224
            Case 2
                Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.02.bin" + """ " + "16793090 8388352"
                RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.02.bin" + """ " + "16793090 CONFIRM"
                BinaryName = "full.02.bin"
                BinaryFileSize = 4294836224
            Case 3
                Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.03.bin" + """ " + "25181442 8388352"
                RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.03.bin" + """ " + "25181442 CONFIRM"
                BinaryName = "full.03.bin"
                BinaryFileSize = 4294836224
            Case 4
                Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.04.bin" + """ " + "33569794 8388352"
                RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.04.bin" + """ " + "33569794 CONFIRM"
                BinaryName = "full.04.bin"
                BinaryFileSize = 4294836224
            Case 5
                Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.05.bin" + """ " + "41958146 8388352"
                RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.05.bin" + """ " + "41958146 CONFIRM"
                BinaryName = "full.05.bin"
                BinaryFileSize = 4294836224
            Case 6

                Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.06.bin" + """ " + "50346498 8388352"
                RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.06.bin" + """ " + "50346498 CONFIRM"
                BinaryName = "full.06.bin"
                BinaryFileSize = 4294836224
            Case 7
                Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.07.bin" + """ " + "58734850 2352896"
                RestoreCommand = "-restore " + SXOSDrivePhysicalName + " " + """" + FolderString + "full.07.bin" + """ " + "58734850 CONFIRM"
                BinaryName = "full.07.bin"
                BinaryFileSize = 1204682758
            Case 8
                Exit Sub
        End Select



    End Sub


    Public Function FormatBytes(ByVal BytesCaller As ULong) As String

        Try
            Select Case BytesCaller
                Case Is >= 1099511627776
                    DoubleBytes = CDbl(BytesCaller / 1099511627776) 'TB
                    Return FormatNumber(DoubleBytes, 2) & " TB"
                Case 1073741824 To 1099511627775
                    DoubleBytes = CDbl(BytesCaller / 1073741824) 'GB
                    Return FormatNumber(DoubleBytes, 2) & " GB"
                Case 1048576 To 1073741823
                    DoubleBytes = CDbl(BytesCaller / 1048576) 'MB
                    Return FormatNumber(DoubleBytes, 2) & " MB"
                Case 1024 To 1048575
                    DoubleBytes = CDbl(BytesCaller / 1024) 'KB
                    Return FormatNumber(DoubleBytes, 2) & " KB"
                Case 0 To 1023
                    DoubleBytes = BytesCaller ' bytes
                    Return FormatNumber(DoubleBytes, 2) & " bytes"
                Case Else
                    Return ""
            End Select
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
        Select Case MsgBox("WARNING: CANCELING A BACKUP/RESTORE MAY CAUSE LOSS OF DATA!  CONTINUE?", MsgBoxStyle.YesNoCancel, "WARNING!")
            Case MsgBoxResult.Yes
                DoCancel()
            Case MsgBoxResult.Cancel
                Exit Sub
            Case MsgBoxResult.No
                Exit Sub
        End Select
    End Sub

    Private Sub DoCancel()

        CanceledOperation = 1
        KillProcess()
        TotalProgressBar.Value = 0
        FileProgressBar.Value = 0
        MessageLabel1.Text = ""
        Cancel.Enabled = False
        Restore.Enabled = True
        Backup.Enabled = True
        Browse.Enabled = True
        RefreshDrives.Enabled = True
        lvDriveInfo.Enabled = True
        HEADER.Enabled = True
        BOOT0.Enabled = True
        BOOT1.Enabled = True
        RAWNAND.Enabled = True
        RAWNANDSPLIT.Enabled = True
        Percent.Text = " "
        SplitFileNum = 0


    End Sub

    Private Sub Fileprogress()
        'Threading.Thread.Sleep(500)
        Application.DoEvents()

        TotalFileSize = 31268536320

        If RAWNAND.Checked = True Or RAWNANDSPLIT.Checked = True Then
            Cancel.Enabled = True
            Restore.Enabled = False
            Backup.Enabled = False
            Browse.Enabled = False
            RefreshDrives.Enabled = False
            lvDriveInfo.Enabled = False
            HEADER.Enabled = False
            BOOT0.Enabled = False
            BOOT1.Enabled = False
            RAWNAND.Enabled = False
            RAWNANDSPLIT.Enabled = False

        Else
            Cancel.Enabled = False
        End If

        Dim ConvertedBinaryFileSize As Long
        ConvertedBinaryFileSize = CLng(BinaryFileSize / 1024)

        Dim totalProgressMax As Int32
        TotalProgressBar.Maximum = totalProgressMax
        FileProgressBar.Maximum = CInt(ConvertedBinaryFileSize)

        Dim flength As Long

        Dim totalSizeDone As Long
        Dim totalSizeDoneLoop As Long
        totalSizeDone = 0

        Dim percentDone As Double


        Do Until System.IO.File.Exists(FolderLocation + "\" + BinaryName)
            Threading.Thread.Sleep(10)
        Loop

        flength = FileLen(FolderString & BinaryName)

        If RAWNANDSPLIT.Checked = False Then
            totalProgressMax = CInt(ConvertedBinaryFileSize)
            TotalProgressBar.Maximum = totalProgressMax
            Do Until flength = BinaryFileSize Or CanceledOperation = 1


                flength = FileLen(FolderString & BinaryName)
                'TextBox5.Text = flength.ToString
                'MessageLabel1.Text = (FormatBytes(CULng(flength.ToString)))

                FileProgressBar.Value = CInt(flength / 1024)
                TotalProgressBar.Value = FileProgressBar.Value
                MessageLabel1.Text = "Creating " + BinaryName + " - " + (FormatBytes(CULng(flength.ToString))) + " / " + (FormatBytes(CULng(BinaryFileSize.ToString)))
                FileProgressBar.Value = CInt(flength / 1024)
                Application.DoEvents()
                percentDone = CDec(CDbl(CType(Math.Round(flength / BinaryFileSize, 4) * 100, String)))
                Percent.Text = CType(percentDone, String) + " %"

                If CanceledOperation = 1 Then
                    CanceledOperation = 0
                    Percent.Text = " "
                    Exit Sub
                End If
            Loop
        Else
            totalProgressMax = 100
            TotalProgressBar.Maximum = totalProgressMax

            Do Until flength = BinaryFileSize Or CanceledOperation = 1 Or totalSizeDone = TotalFileSize
                flength = FileLen(FolderString & BinaryName)
                totalSizeDoneLoop = 0
                Try
                    If System.IO.File.Exists(FolderLocation + "\full.00.bin") Then
                        totalSizeDoneLoop += FileLen(FolderLocation + "\full.00.bin")
                    End If
                    If System.IO.File.Exists(FolderLocation + "\full.01.bin") Then
                        totalSizeDoneLoop += FileLen(FolderLocation + "\full.01.bin")
                    End If
                    If System.IO.File.Exists(FolderLocation + "\full.02.bin") Then
                        totalSizeDoneLoop += FileLen(FolderLocation + "\full.02.bin")
                    End If
                    If System.IO.File.Exists(FolderLocation + "\full.03.bin") Then
                        totalSizeDoneLoop += FileLen(FolderLocation + "\full.03.bin")
                    End If
                    If System.IO.File.Exists(FolderLocation + "\full.04.bin") Then
                        totalSizeDoneLoop += FileLen(FolderLocation + "\full.04.bin")
                    End If
                    If System.IO.File.Exists(FolderLocation + "\full.05.bin") Then
                        totalSizeDoneLoop += FileLen(FolderLocation + "\full.05.bin")
                    End If
                    If System.IO.File.Exists(FolderLocation + "\full.06.bin") Then
                        totalSizeDoneLoop += FileLen(FolderLocation + "\full.06.bin")
                    End If
                    If System.IO.File.Exists(FolderLocation + "\full.07.bin") Then
                        totalSizeDoneLoop += FileLen(FolderLocation + "\full.07.bin")
                    End If
                Catch ex As Exception
                    MsgBox("Error reading file " + FolderLocation + "\full.00.bin! Progress Bar will not be accurate!" + Environment.NewLine + ex.Message)
                End Try

                totalSizeDone = totalSizeDoneLoop
                Dim percentsplit As Int64
                percentsplit = CLng((totalSizeDone / TotalFileSize))



                MessageLabel1.Text = "Creating " + BinaryName + " - " + (FormatBytes(CULng(flength.ToString))) + " / " + (FormatBytes(CULng(BinaryFileSize.ToString)))


                TotalProgressBar.Value = CInt(totalSizeDone / TotalFileSize * 100)
                FileProgressBar.Value = CInt(flength / 1024)
                Application.DoEvents()
                percentDone = Math.Round((totalSizeDone / TotalFileSize * 100), 2)
                Percent.Text = CType(percentDone, String) + " %"

                If totalSizeDone = TotalFileSize Then
                    Exit Do
                End If

                If CanceledOperation = 1 Then
                    CanceledOperation = 0
                    Percent.Text = " "
                    Exit Sub
                End If
        Loop
        End If

        If RAWNANDSPLIT.Checked = True And SplitFileNum <> 7 Then
            SplitFileNum = SplitFileNum + 1
            DoBackup()
        ElseIf RAWNAND.Checked = True Then
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("RAWNAND backup complete!")
        ElseIf HEADER.Checked = True Then
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("HEADER backup complete!")
        ElseIf BOOT0.Checked = True Then
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("BOOT0 backup complete!")
        ElseIf BOOT1.Checked = True Then
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("BOOT1 backup complete!")
        ElseIf RAWNANDSPLIT.Checked = True Then
            Threading.Thread.Sleep(2000)
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("RAWNAND SPLIT backup complete!")
        End If

            'TextBox5.Text = flength.ToString
            'MessageLabel1.Text = (FormatBytes(CULng(flength.ToString)))
            'FileProgressBar.Value = CInt(flength / 1024)
            'If RAWNANDSPLIT.Checked = True = False Then
            'MsgBox("Backup of " & BinaryName & " now complete")
            'ElseIf RAWNANDSPLIT.Checked = True Then
            'MsgBox("RAWNAND SPLIT backup  complete!")
            'End If


            TotalProgressBar.Value = 0
        FileProgressBar.Value = 0
        MessageLabel1.Text = ""
        Percent.Text = ""
        Cancel.Enabled = False
        Restore.Enabled = True
        Backup.Enabled = True
        Browse.Enabled = True
        RefreshDrives.Enabled = True
        lvDriveInfo.Enabled = True
        HEADER.Enabled = True
        BOOT0.Enabled = True
        BOOT1.Enabled = True
        RAWNAND.Enabled = True
        RAWNANDSPLIT.Enabled = True

    End Sub

    Private Sub KillProcess()


        Dim arrayListInfo As New System.Collections.ArrayList()
        Dim currentProcess As Process = Process.GetCurrentProcess()
        Dim localAll As Process() = Process.GetProcesses()
        Dim localByName As Process() = Process.GetProcessesByName("secinspect")
        Dim proc As Process

        Dim A As Integer
        Dim B As Integer

        If BackgroundRestore.IsBusy Then
            If BackgroundRestore.WorkerSupportsCancellation Then
                CanceledRestoreOperation = 1
                BackgroundRestore.CancelAsync()
            End If
        End If

        If BackGroundRetoreProgress.IsBusy Then
            If BackGroundRetoreProgress.WorkerSupportsCancellation Then
                CanceledRestoreOperation = 1
                BackGroundRetoreProgress.CancelAsync()
            End If
        End If

        If BackgroundDeleteSplitFiles.IsBusy Then
            If BackgroundDeleteSplitFiles.WorkerSupportsCancellation Then
                BackgroundDeleteSplitFiles.CancelAsync()
                MessageLabel1.Text = ""

            End If
        End If


        For Each proc In localByName
            arrayListInfo.Add(proc.Id)
            B = B + 1
        Next

        For A = 0 To B - 1
            'TextBox4.Text = TextBox4.Text + arrayListInfo(A).ToString & vbCrLf
            Process.GetProcessById(CInt(arrayListInfo(A))).Kill()
        Next

        If RAWNAND.Checked = True And Restoring = False And AppClosing = False Then
            MessageLabel1.Text = "Deleting RAWNAND.BIN"

            Dim ProcessRunning As Boolean
            ProcessRunning = True
            Do Until ProcessRunning = False
                Dim listProc() As System.Diagnostics.Process
                listProc = Process.GetProcessesByName("secinspect")
                If listProc.Length > 0 Then
                    ProcessRunning = True
                Else
                    ProcessRunning = False
                End If
            Loop

            Dim RawNandFileToDelete As String = BackupLocationPathLabel.Text + "\RAWNAND.BIN"

            Try

                If System.IO.File.Exists(RawNandFileToDelete) = True Then
                    'Threading.Thread.Sleep(2000)
                    System.IO.File.Delete(RawNandFileToDelete)
                End If
            Catch ex As Exception
                My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
                MsgBox("Error deleting" + RawNandFileToDelete + "from" + BackupLocationPathLabel.Text + Environment.NewLine + ex.Message)
            End Try
        End If

        If RAWNANDSPLIT.Checked = True And Restoring = False And AppClosing = False Then
            CanceledRawsplitBackup = 1
            'Threading.Thread.Sleep(2000)
            BackgroundDeleteSplitFiles.RunWorkerAsync()

        End If


    End Sub


    Public Sub MyRestoreProgress()

        Dim pName As String = "secinspect"
        'Dim psList() As Process


        If RAWNAND.Checked = True Then
            Cancel.Enabled = True
            Restore.Enabled = False
            Backup.Enabled = False
            Browse.Enabled = False
            RefreshDrives.Enabled = False
            lvDriveInfo.Enabled = False
            HEADER.Enabled = False
            BOOT0.Enabled = False
            BOOT1.Enabled = False
            RAWNAND.Enabled = False
            RAWNANDSPLIT.Enabled = False
        Else
            Cancel.Enabled = False
        End If


        BackGroundRetoreProgress.RunWorkerAsync()


    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles DebugButton.Click

        If ToggleDebug = False Then

            ToggleDebug = True
            ' Old Method of snapping window size wider
            'Me.Width = Me.Width + 600


            ' New Method  animates window resize
            DebugButton.Enabled = False
            If Not Timer1.Enabled Then
                stp = 60
                Timer1.Start()
            End If

            'DebugButton.Text = "<<< Debug"
        Else
            ToggleDebug = False

            ' Old Method of snapping window size wider
            'Me.Width = Me.Width - 600


            ' New Method  animates window resize
            DebugButton.Enabled = False
            If Not Timer1.Enabled Then
                stp = -60
                Timer1.Start()
            End If


            ' DebugButton.Text = "Debug >>>"
        End If

    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If (stp > 0 And Me.Width < Me.MaximumSize.Width) Or (stp < 0 And Me.Width > Me.MinimumSize.Width) Then
            Me.Width += stp

        Else
            Timer1.Stop()
            DebugButton.Enabled = True
            If ToggleDebug = False Then
                DebugButton.Text = ">"
            Else
                DebugButton.Text = "<"
            End If

        End If
    End Sub

    Private Sub BackGroundRetoreWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackGroundRetoreProgress.DoWork


        Dim Finished As Integer
        Finished = 0


        Do Until Finished = 1

            p = Process.GetProcessesByName("secinspect")
            If p.Count > 0 Then
                ' Process is running
                Finished = 0
            Else
                ' Process is not running
                Finished = 1

            End If

        Loop



    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        ConsoleTextBox.Text = ""
    End Sub

    Private Sub BackgroundRestoreProgress_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundRestore.DoWork

        Dim myProcessStartInfo As New Process()

        With myProcessStartInfo
            .StartInfo.FileName = "secinspect.exe"
            .StartInfo.Arguments = RestoreCommand
            .StartInfo.Verb = "runas"
            .StartInfo.RedirectStandardOutput = True
            .StartInfo.CreateNoWindow = True
            .StartInfo.UseShellExecute = False

        End With
        myProcessStartInfo.Start()

        'Process.StartInfo.Start(myProcessStartInfo)
        Dim output() As String = myProcessStartInfo.StandardOutput.ReadToEnd.Split(CChar(vbLf))
        For Each ln As String In output
            ConsoleTextBox.AppendText(ln & vbNewLine)
        Next


    End Sub

    Private Sub BackGroundRetoreProgress_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackGroundRetoreProgress.ProgressChanged
        TotalProgressBar.Value = e.ProgressPercentage
    End Sub

    Private Sub BackGroundRetoreProgress_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackGroundRetoreProgress.RunWorkerCompleted
        TotalProgressBar.Value = 0
        FileProgressBar.Value = 0
        'Percent.Text = " "
        MessageLabel1.Text = "Restoring... Please Wait."
        Cancel.Enabled = False
        Restore.Enabled = True
        Backup.Enabled = True
        Browse.Enabled = True
        RefreshDrives.Enabled = True
        lvDriveInfo.Enabled = True
        HEADER.Enabled = True
        BOOT0.Enabled = True
        BOOT1.Enabled = True
        RAWNAND.Enabled = True
        RAWNANDSPLIT.Enabled = True
        TotalProgressBar.Style = ProgressBarStyle.Continuous
        FileProgressBar.Style = ProgressBarStyle.Continuous

        If CanceledRestoreOperation = 1 Then
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("Restore has been canceled")

            CanceledRestoreOperation = 0
        Else
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("Restore now complete")
        End If
    End Sub

    Private Sub BackgroundDeleteSplitFiles_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundDeleteSplitFiles.DoWork

        Dim CheckDeletes As Int64
        CheckDeletes = 0
        deleteFileName = ""


        Dim ProcessRunning As Boolean
        ProcessRunning = True
        Do Until ProcessRunning = False
            Dim listProc() As System.Diagnostics.Process
            listProc = Process.GetProcessesByName("secinspect")
            If listProc.Length > 0 Then
                ProcessRunning = True
            Else
                ProcessRunning = False
            End If
        Loop


        Do Until CheckDeletes = 8

            CheckDeletes = 0

            If System.IO.File.Exists(FolderLocation + "\full.00.BIN") = True Then
                deleteFileName = "full.00.bin"
                MessageLabel1.Text = "Deleting full.00.bin"
                Threading.Thread.Sleep(500)
                System.IO.File.Delete(FolderLocation + "\full.00.BIN")
            Else
                CheckDeletes += 1
            End If
            If System.IO.File.Exists(FolderLocation + "\full.01.BIN") = True Then
                deleteFileName = "full.01.bin"
                MessageLabel1.Text = "Deleting full.01.bin"
                Threading.Thread.Sleep(200)
                System.IO.File.Delete(FolderLocation + "\full.01.BIN")
            Else
                CheckDeletes += 1
            End If
            If System.IO.File.Exists(FolderLocation + "\full.02.BIN") = True Then
                deleteFileName = "full.02.bin"
                MessageLabel1.Text = "Deleting full.02.bin"
                Threading.Thread.Sleep(200)
                System.IO.File.Delete(FolderLocation + "\full.02.BIN")
            Else
                CheckDeletes += 1
            End If
            If System.IO.File.Exists(FolderLocation + "\full.03.BIN") = True Then
                deleteFileName = "full.03.bin"
                MessageLabel1.Text = "Deleting full.03.bin"
                Threading.Thread.Sleep(200)
                System.IO.File.Delete(FolderLocation + "\full.03.BIN")
            Else
                CheckDeletes += 1
            End If
            If System.IO.File.Exists(FolderLocation + "\full.04.BIN") = True Then
                deleteFileName = "full.04.bin"
                MessageLabel1.Text = "Deleting full.04.bin"
                Threading.Thread.Sleep(200)
                System.IO.File.Delete(FolderLocation + "\full.04.BIN")
            Else
                CheckDeletes += 1
            End If
            If System.IO.File.Exists(FolderLocation + "\full.05.BIN") = True Then
                deleteFileName = "full.05.bin"
                MessageLabel1.Text = "Deleting full.05.bin"
                Threading.Thread.Sleep(200)
                System.IO.File.Delete(FolderLocation + "\full.05.BIN")
            Else
                CheckDeletes += 1
            End If
            If System.IO.File.Exists(FolderLocation + "\full.06.BIN") = True Then
                deleteFileName = "full.06.bin"
                MessageLabel1.Text = "Deleting full.06.bin"
                Threading.Thread.Sleep(200)
                System.IO.File.Delete(FolderLocation + "\full.06.BIN")
            Else
                CheckDeletes += 1
            End If
            If System.IO.File.Exists(FolderLocation + "\full.07.BIN") = True Then
                deleteFileName = "full.07.bin"
                MessageLabel1.Text = "Deleting full.07.bin"
                Threading.Thread.Sleep(200)
                System.IO.File.Delete(FolderLocation + "\full.07.BIN")
            Else
                CheckDeletes += 1
            End If

        Loop



    End Sub

    Private Sub BackgroundDeleteSplitFiles_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundDeleteSplitFiles.RunWorkerCompleted
        MessageLabel1.Text = ""
        If e.Error IsNot Nothing Then
            My.Computer.Audio.Play(My.Resources.blip, AudioPlayMode.Background)
            MsgBox("The file " + deleteFileName + "could not be deleted. Please delete " + deleteFileName + " manually from " + FolderLocation + "and try again.")

        Else
            'Threading.Thread.Sleep(1000)

            If CanceledRawsplitBackup = 0 Then
                DoBackup()
            End If

        End If

    End Sub


End Class
