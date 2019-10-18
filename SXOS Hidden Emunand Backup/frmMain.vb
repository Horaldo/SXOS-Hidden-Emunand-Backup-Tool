Imports System.IO

Public Class frmMain
    Dim Production As Boolean = False

    Dim drive As New SXOSDrive
    ReadOnly inspector As String = "secinspect.exe"
    Dim SelectedFolder As String = ""

    Dim secins = My.Resources.secinspect

    Dim logger As New LogMe(Production)

    Enum BorR
        Backup
        Restore
    End Enum

    ReadOnly appPath As String = Application.StartupPath()
    ReadOnly BinaryName As String = IIf(BOOT0.Checked, "BOOT0.BIN", IIf(BOOT1.Checked, "BOOT1.BIN", IIf(RAWNAND.Checked, "RAWNAND.BIN", "")))
    Dim BinaryFileSize As Long
    Dim CanceledOperation As Boolean

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            logger.Log("Main Form Loading", Not Production)
            Me.lvDriveInfo.View = View.Details
            GetDriveInfo()
        Catch ex As Exception
            logger.Log("Main Form Load error", ex)
        End Try
    End Sub

    Private Sub FrmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        logger.Log("Form Resizing.", Not Production)

        Try
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
        Catch ex As Exception
            logger.Log("Resizing Form Failed!", ex)
        End Try
    End Sub

    Private Sub MyApplication_FormClosing(sender As Object, e As EventArgs) Handles Me.FormClosing
        logger.Log("Form Closing.", Not Production)
        Try
            KillProcess()

            Dim FileToDelete As String = My.Application.Info.DirectoryPath & "\" & inspector
            If System.IO.File.Exists(FileToDelete) Then
                Threading.Thread.Sleep(1500)
                System.IO.File.Delete(FileToDelete)
            End If
        Catch ex As Exception
            logger.Log("Failed to Close Form.", ex)
        End Try
    End Sub

    Private Sub RefreshDrives_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshDrives.Click
        logger.Log("Refresh Drives Clicked.", Not Production)
        GetDriveInfo()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Try
            logger.Log("Browse Button Clicked.", Not Production)
            SetSelectedFolder()

            Return 'should make this go back to where it came from
        Catch ex As Exception
            logger.Log("Failed at Browse Button", ex)
        End Try
    End Sub

    Private Function SetSelectedFolder() As DialogResult
        Dim setfolder As New FolderBrowserDialog()
        Try
            logger.Log("Setting SelectedFolder.", Not Production)
            Dim result = setfolder.ShowDialog()
            If result = DialogResult.OK Then
                SelectedFolder = IIf(Not setfolder.SelectedPath.EndsWith("\"), setfolder.SelectedPath & "\", setfolder.SelectedPath)
                BackupLocationPathTextbox.Text = SelectedFolder
            End If
        Catch ex As Exception
            logger.Log("Failed to set SelectedFolder", ex)
        End Try
    End Function

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        logger.Log("Restore Button Clicked.", Not Production)
        ValidateBackupRestore(BorR.Restore)
    End Sub

    Private Sub Backup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
        logger.Log("Backup Button Clicked.", Not Production)
        ValidateBackupRestore(BorR.Backup)
    End Sub

    Public Sub ValidateBackupRestore(ByRef _borr As BorR)
        logger.Log("ValidateBackupRestore starting.", Not Production)
        If drive.DriveLetter = Nothing Then
            logger.Log("Please select the SXOS drive to restore to", Nothing, True, MessageBoxIcon.Exclamation)
        Else
            Try
                If drive.DriveVolumeName = "TXNAND" Then
                    If SelectedFolder Is Nothing Then
                        Dim result As DialogResult = SetSelectedFolder()
                        If Not result = DialogResult.OK Then
                            logger.Log("No Location was selected." & vbCrLf &
                                       "You must make a selection.",
                                       Nothing,
                                       True,
                                       MessageBoxIcon.Information)
                        Else
                            If _borr = BorR.Backup Then
                                OverwriteCheck()
                            ElseIf _borr = BorR.Restore Then
                                RestoreCheck()
                            Else
                                Throw New Exception("Somehow we are Validating Backup and Restore without either selected? This should be impossible.")
                            End If
                        End If
                    End If
                Else
                    logger.Log("The selected drive does not contain SXOS Emunand!", Nothing, True, MessageBoxIcon.Hand)
                    Exit Sub
                End If
            Catch ex As Exception
                logger.Log("Failed at Validata Backup Restore.", ex)
            End Try
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            logger.Log("Button Cancel Clicked.", Not Production)
            KillProcess()
            'What are you trying to do here? Cancel is not exiting the program. Is it supposed to?
            'If so, it should really make sure that the process is killed first. Good thread control btw.
            'To exit uncomment:
            'Me.Close()
            ProgressBar1.Value = 0
            FileSizeTextBox.Text = " "
            CanceledOperation = True
            btnCancel.Enabled = False
            btnRestore.Enabled = True
            btnBackup.Enabled = True
            btnBrowse.Enabled = True
            btnRefreshDrives.Enabled = True
            lvDriveInfo.Enabled = True
            BOOT0.Enabled = True
            BOOT1.Enabled = True
            RAWNAND.Enabled = True
            Percent.Text = " "
        Catch ex As Exception
            logger.Log("Failed at Button Cancel clicked.", ex)
        End Try
    End Sub

    Private Sub LvDriveInfo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvDriveInfo.SelectedIndexChanged
        ' The drive information for the drive selected in the ListView is synchronized each time the selection changes
        Try
            logger.Log("At ListView selected index changed.", Not Production)
            With Me.lvDriveInfo
                Dim selectedDriveIndex = .SelectedIndices(0)
                drive.DriveLetter = .Items(selectedDriveIndex).SubItems(1).Text
                drive.DrivePhysicalName = lvDriveInfo.Items(selectedDriveIndex).SubItems(2).Text.Replace(" ", "")
                drive.DriveType = lvDriveInfo.Items(selectedDriveIndex).SubItems(3).Text
                drive.DriveVolumeName = lvDriveInfo.Items(selectedDriveIndex).SubItems(4).Text
                drive.DriveFileSystem = lvDriveInfo.Items(selectedDriveIndex).SubItems(5).Text
            End With

            'Update that textbox (why is it a textbox and not a label?)
            LocationTextBox.Text = drive.DriveVolumeName
        Catch ex As Exception
            logger.Log("Failed at selected Index Changed.", ex)
        End Try

    End Sub

    Private Sub GetDriveInfo()
        logger.Log("Trying to get Drive Info.", Not Production)
        Try
            Dim count As Int32 = 0
            Dim driveInfoEx As New clsDiskInfoEx
            Dim multipleParents() As String = Nothing
            Dim parentDrives As String = Nothing

            lvDriveInfo.Items.Clear()

            For Each drive As System.IO.DriveInfo In System.IO.DriveInfo.GetDrives
                'Since we are only concerned with USB devices and not fixed disks, we are checking for removeable devices            
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
        Catch ex As Exception
            logger.Log("Failed to get Drive Info.", ex)
        End Try
    End Sub

    Private Sub OverwriteCheck()
        If My.Computer.FileSystem.FileExists(SelectedFolder & BinaryName) Then
            Select Case MsgBox(BinaryName & " already exists. Overwrite?", MsgBoxStyle.YesNo, "FILE ALREADY EXISTS")
                Case MsgBoxResult.Yes
                    DoBackup(CreateCommand(BorR.Backup))
                Case MsgBoxResult.No
                    Exit Sub
            End Select
        Else
            DoBackup(CreateCommand(BorR.Backup))
        End If

    End Sub

    Private Sub RestoreCheck()
        If My.Computer.FileSystem.FileExists(SelectedFolder & BinaryName) Then
            Select Case MsgBox("Are you sure you want to restore " & BinaryName & " to " & drive.DriveVolumeName & "?", MsgBoxStyle.YesNo, "CONFIRM RESTORE")
                Case MsgBoxResult.Yes
                    DoRestore(CreateCommand(BorR.Restore))
                Case MsgBoxResult.No
                    Exit Sub
            End Select
        Else
            MsgBox(BinaryName & " not found in " & SelectedFolder)
        End If
    End Sub

    Private Sub DoBackup(cmd As String)
        logger.Log("Doing backup.", Not Production)
        Try
            Dim myProcess As New Process()

            With myProcess.StartInfo
                .FileName = inspector
                .Arguments = cmd
                '.Verb = "runas"
                .CreateNoWindow = True
                .UseShellExecute = False
            End With

            logger.Log("## Attempting to backup " & BinaryName &
                       " to " & SelectedFolder & vbCrLf &
                       "Command Is: " & cmd)

            myProcess.Start()

            For Each ln As String In myProcess.StandardOutput.ReadToEnd.Split(CChar(vbLf))
                logger.Log(ln)
            Next

            Fileprogress()
        Catch ex As Exception
            logger.Log("Failed at doing backup.", ex)
        End Try
    End Sub

    Private Sub DoRestore(cmd As String)
        logger.Log("Doing Restore.", Not Production)
        Try
            Dim myProcess As New Process()

            With myProcess.StartInfo
                .FileName = inspector
                .Arguments = cmd
                .Verb = "runas"
                .CreateNoWindow = False
                .UseShellExecute = True
                .RedirectStandardOutput = True
            End With

            logger.Log("## Attempting to restore " & BinaryName &
                           " from " & SelectedFolder &
                           " to " & SelectedFolder & vbCrLf &
                           "Command Is: " & cmd)

            myProcess.Start()

            For Each ln As String In myProcess.StandardOutput.ReadToEnd.Split(CChar(vbLf))
                logger.Log(ln)
            Next

            MyRestoreProgress()
        Catch ex As Exception
            logger.Log("Failed to Do Restore.", ex)
        End Try
    End Sub

    Private Function CreateCommand(BorR As BorR) As String
        logger.Log("Creating Command.", Not Production)
        Try
            Dim cmd = String.Format("-{0} {1} ""{2}"" {3} {4}",
                        BorR.ToString,
                        drive.DrivePhysicalName,
                        SelectedFolder & BinaryName,
                        IIf(BOOT0.Checked, "2", IIf(BOOT1.Checked, "8194", IIf(RAWNAND.Checked, "16386", ""))),
                        IIf(BorR = BorR.Backup, IIf(BOOT0.Checked, "8192", IIf(BOOT1.Checked, "8192", IIf(RAWNAND.Checked, "61071360", ""))), ""))

            logger.Log("Command Is: " & cmd)
            Return cmd
        Catch ex As Exception
            logger.Log("Failed to Create command.", ex)
        End Try
        Return Nothing
    End Function

    Public Function FormatBytes(ByVal BytesCaller As ULong) As String
        Try
            Dim DoubleBytes As Double
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
        Catch ex As Exception 'TODO: How do you want to handle if this messes up?
            Return ""
            Console.WriteLine(ex)
        End Try
    End Function

    Private Sub Fileprogress()
        logger.Log("In File Progress Sub.", Not Production)
        Try
            Threading.Thread.Sleep(1000)
            Application.DoEvents()

            If RAWNAND.Checked = True Then
                btnCancel.Enabled = True
                btnRestore.Enabled = False
                btnBackup.Enabled = False
                btnBrowse.Enabled = False
                btnRefreshDrives.Enabled = False
                lvDriveInfo.Enabled = False
                BOOT0.Enabled = False
                BOOT1.Enabled = False
                RAWNAND.Enabled = False
            Else
                btnCancel.Enabled = False
            End If

            ProgressBar1.Maximum = BinaryFileSize / 1024

            Dim flength = FileLen(SelectedFolder & BinaryName)

            Dim percentDone As Double

            Do Until flength = BinaryFileSize
                flength = FileLen(SelectedFolder & BinaryName)
                'TextBox5.Text = flength.ToString
                FileSizeTextBox.Text = FormatBytes(flength)
                ProgressBar1.Value = flength / 1024
                Application.DoEvents()
                percentDone = CDec(Math.Round(flength / BinaryFileSize, 4) * 100)
                Percent.Text = percentDone.ToString + " % Complete"

                If CanceledOperation Then
                    CanceledOperation = False
                    Exit Sub
                End If
            Loop

            FileSizeTextBox.Text = FormatBytes(flength)
            ProgressBar1.Value = flength / 1024
            logger.Log("Backup of " & BinaryName & " now complete", Nothing, True, MessageBoxIcon.Information)

            ProgressBar1.Value = 0
            FileSizeTextBox.Text = " "
            Percent.Text = " "
            btnCancel.Enabled = False
            btnRestore.Enabled = True
            btnBackup.Enabled = True
            btnBrowse.Enabled = True
            btnRefreshDrives.Enabled = True
            lvDriveInfo.Enabled = True
            BOOT0.Enabled = True
            BOOT1.Enabled = True
            RAWNAND.Enabled = True

        Catch ex As Exception
            logger.Log("Failed to Fileprogress.", ex)
        End Try

    End Sub

    Private Sub KillProcess()
        logger.Log("Trying to Kill Process.", Not Production)
        Try
            For Each proc As Process In Process.GetProcessesByName("secinspect")
                proc.Kill()
            Next
        Catch ex As Exception
            logger.Log("Failed to kill all processes!", ex)
        End Try
    End Sub

    Public Sub MyRestoreProgress()
        logger.Log("Restore progress", Not Production)
        Try
            Dim Finished As Integer
            Dim pName As String = "secinspect"

            If RAWNAND.Checked Then
                btnCancel.Enabled = True
                btnRestore.Enabled = False
                btnBackup.Enabled = False
                btnBrowse.Enabled = False
                btnRefreshDrives.Enabled = False
                lvDriveInfo.Enabled = False
                BOOT0.Enabled = False
                BOOT1.Enabled = False
                RAWNAND.Enabled = False
            Else
                btnCancel.Enabled = False
            End If

            Finished = 0
            ProgressBar1.Maximum = 100
            ProgressBar1.Style = ProgressBarStyle.Marquee

            While Process.GetProcessesByName("secinspect").Length > 0
                UseWaitCursor = True
            End While
            UseWaitCursor = False
            ProgressBar1.Style = ProgressBarStyle.Continuous
            ProgressBar1.Value = 100

            FileSizeTextBox.Text = " "
            Percent.Text = " "
            btnCancel.Enabled = False
            btnRestore.Enabled = True
            btnBackup.Enabled = True
            btnBrowse.Enabled = True
            btnRefreshDrives.Enabled = True
            lvDriveInfo.Enabled = True
            BOOT0.Enabled = True
            BOOT1.Enabled = True
            RAWNAND.Enabled = True
            logger.Log("Restore now complete!", Nothing, True, MessageBoxIcon.Information)
        Catch ex As Exception
            logger.Log("Failed on restore progress!", ex)
        End Try
    End Sub
End Class