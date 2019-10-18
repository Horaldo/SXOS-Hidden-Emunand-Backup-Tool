Imports System.IO

Public Class frmMain

    Dim drive As New SXOSDrive
    ReadOnly inspector As String = "secinspect.exe"
    Dim SelectedFolder As String = ""

    Dim logger As New LogMe

    Enum BorR
        Backup
        Restore
    End Enum

    ReadOnly appPath As String = Application.StartupPath()
    ReadOnly BinaryName As String = IIf(BOOT0.Checked, "BOOT0.BIN", IIf(BOOT1.Checked, "BOOT1.BIN", IIf(RAWNAND.Checked, "RAWNAND.BIN", "")))
    Dim BinaryFileSize As Long
    Dim CanceledOperation As Boolean

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.lvDriveInfo.View = View.Details

        Dim SetupPath As String = appPath & "\" & inspector

        Dim sec = My.Resources.secinspect

        If System.IO.File.Exists(SetupPath) Then
            Using sCreateMSIFile As New FileStream(SetupPath, FileMode.Create)
                sCreateMSIFile.Write(My.Resources.secinspect, 0, My.Resources.secinspect.Length)
            End Using
        End If

        GetDriveInfo()
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

    Private Sub MyApplication_FormClosing(sender As Object, e As EventArgs) Handles Me.FormClosing
        KillProcess()

        Dim FileToDelete As String = My.Application.Info.DirectoryPath & "\" & inspector
        If System.IO.File.Exists(FileToDelete) Then
            Threading.Thread.Sleep(1500)
            System.IO.File.Delete(FileToDelete)
        End If
        End

    End Sub

    Private Sub RefreshDrives_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshDrives.Click
        GetDriveInfo()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            SelectedFolder = IIf(Not FolderBrowserDialog1.SelectedPath.EndsWith("\"), FolderBrowserDialog1.SelectedPath & "\", FolderBrowserDialog1.SelectedPath)
            BackupLocationPathTextbox.Text = SelectedFolder
        End If
    End Sub

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        If drive.DriveLetter = Nothing Then
            MsgBox("Please select the SXOS drive to restore to")
            Exit Sub
        Else
            Try
                If LocationTextBox.Text = "TXNAND" Then
                    If BackupLocationPathTextbox.Text = "" Then
                        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            SelectedFolder = IIf(Not FolderBrowserDialog1.SelectedPath.EndsWith("\"), FolderBrowserDialog1.SelectedPath & "\", FolderBrowserDialog1.SelectedPath)
                            BackupLocationPathTextbox.Text = SelectedFolder
                            CreateCommand(BorR.Restore)
                        Else
                            logger.Log("No Location was selected." & vbCrLf &
                                       "You must select the location of the file to restore.",
                                       Nothing,
                                       True,
                                       MessageBoxIcon.Information)
                            Exit Sub
                        End If

                        If BackupLocationPathTextbox.Text = "" Then
                            Exit Sub
                        Else
                            RestoreCheck()
                        End If
                    Else
                        RestoreCheck()
                    End If
                Else
                    MsgBox("The selected drive is not SXOS Emunand ready!")
                    Exit Sub
                End If
            Catch ex As Exception
                Console.Write(ex)
            End Try
        End If
    End Sub

    Private Sub Backup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click

        If drive.DriveLetter = Nothing Then
            MsgBox("Please select the SXOS drive to backup!", MsgBoxStyle.Exclamation)
            Exit Sub
        Else
            If LocationTextBox.Text = "TXNAND" Then
                If BackupLocationPathTextbox.Text = "" Then
                    If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        SelectedFolder = IIf(Not FolderBrowserDialog1.SelectedPath.EndsWith("\"), FolderBrowserDialog1.SelectedPath & "\", FolderBrowserDialog1.SelectedPath)
                        BackupLocationPathTextbox.Text = SelectedFolder
                        CreateCommand(BorR.Backup)
                    End If

                    If BackupLocationPathTextbox.Text = "" Then
                        Exit Sub
                    Else
                        OverwriteCheck()
                    End If
                Else
                    OverwriteCheck()
                End If
            Else
                MsgBox("The selected drive does not contain an SXOS Emunand")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        KillProcess()
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
    End Sub

    Private Sub LvDriveInfo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvDriveInfo.SelectedIndexChanged
        ' The drive information for the drive selected in the ListView is synchronized each time the selection changes
        With Me.lvDriveInfo
            Dim selectedDriveIndex = .SelectedIndices(0)
            drive.DriveLetter = .Items(selectedDriveIndex).SubItems(1).Text
            drive.DrivePhysicalName = lvDriveInfo.Items(selectedDriveIndex).SubItems(2).Text.Replace(" ", "")
            drive.DriveType = lvDriveInfo.Items(selectedDriveIndex).SubItems(3).Text
            drive.DriveVolumeName = lvDriveInfo.Items(selectedDriveIndex).SubItems(4).Text
            drive.DriveFileSystem = lvDriveInfo.Items(selectedDriveIndex).SubItems(5).Text
        End With

        'Update that textbox (why is it a textbox and not a label?
        LocationTextBox.Text = drive.DriveVolumeName

    End Sub

    Private Sub GetDriveInfo()

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
    End Sub

    Private Sub OverwriteCheck()
        If My.Computer.FileSystem.FileExists(BackupLocationPathTextbox.Text & BinaryName) Then
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

    Private Sub DoBackup(cmd As String)
        Dim myProcess As New Process()

        With myProcess.StartInfo
            .FileName = inspector
            .Arguments = cmd
            '.Verb = "runas"
            .CreateNoWindow = True
            .UseShellExecute = False
        End With

        myProcess.Start()

        Fileprogress()
    End Sub

    Private Sub RestoreCheck()
        If My.Computer.FileSystem.FileExists(BackupLocationPathTextbox.Text & BinaryName) Then
            Select Case MsgBox("Are you sure you want to restore " & BinaryName & " to " & LocationTextBox.Text & "?", MsgBoxStyle.YesNo, "CONFIRM RESTORE")
                Case MsgBoxResult.Yes
                    DoRestore(CreateCommand(BorR.Restore))
                Case MsgBoxResult.No
                    Exit Sub
            End Select
        Else
            MsgBox(BinaryName & " not found in " & BackupLocationPathTextbox.Text)
        End If
    End Sub

    Private Sub DoRestore(cmd As String)
        Dim myProcess As New Process()

        With myProcess.StartInfo
            .FileName = inspector
            .Arguments = cmd
            .Verb = "runas"
            .CreateNoWindow = False
            .UseShellExecute = True
            .RedirectStandardOutput = True
        End With

        myProcess.Start()

        For Each ln As String In myProcess.StandardOutput.ReadToEnd.Split(CChar(vbLf))
            logger.Log(ln)
        Next

        MyRestoreProgress()
    End Sub

    Private Function CreateCommand(BorR As BorR) As String
        Dim cmd = String.Format("-{0} {1} ""{2}"" {3} {4}",
            BorR.ToString,
            drive.DrivePhysicalName,
            SelectedFolder & BinaryName,
            IIf(BOOT0.Checked, "2", IIf(BOOT1.Checked, "8194", IIf(RAWNAND.Checked, "16386", ""))),
            IIf(BorR = BorR.Backup, IIf(BOOT0.Checked, "8192", IIf(BOOT1.Checked, "8192", IIf(RAWNAND.Checked, "61071360", ""))), ""))

        Console.WriteLine(cmd)
        Return cmd
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

        Dim ConvertedBinaryFileSize As Long
        ConvertedBinaryFileSize = CLng(BinaryFileSize / 1024)
        ProgressBar1.Maximum = CInt(ConvertedBinaryFileSize)

        Dim flength As Long
        flength = FileLen(SelectedFolder & BinaryName)

        Dim percentDone As Double

        Do Until flength = BinaryFileSize
            flength = FileLen(SelectedFolder & BinaryName)
            'TextBox5.Text = flength.ToString
            FileSizeTextBox.Text = (FormatBytes(CULng(flength.ToString)))
            ProgressBar1.Value = CInt(flength / 1024)
            Application.DoEvents()
            percentDone = CDec(CDbl(CType(Math.Round(flength / BinaryFileSize, 4) * 100, String)))
            Percent.Text = CType(percentDone, String) + " % Complete"


            If CanceledOperation Then
                CanceledOperation = False
                Exit Sub
            End If
        Loop
        'TextBox5.Text = flength.ToString
        FileSizeTextBox.Text = (FormatBytes(CULng(flength.ToString)))
        ProgressBar1.Value = CInt(flength / 1024)
        MsgBox("Backup of " & BinaryName & " now complete")

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

    End Sub

    Private Sub KillProcess()
        Dim arrayListInfo As New System.Collections.ArrayList()
        Dim currentProcess As Process = Process.GetCurrentProcess()
        Dim localAll As Process() = Process.GetProcesses()
        Dim localByName As Process() = Process.GetProcessesByName("secinspect")
        Dim proc As Process

        Dim A As Integer
        Dim B As Integer

        For Each proc In localByName
            arrayListInfo.Add(proc.Id)
            B = B + 1
        Next

        For A = 0 To B - 1
            'TextBox4.Text = TextBox4.Text + arrayListInfo(A).ToString & vbCrLf
            Process.GetProcessById(CInt(arrayListInfo(A))).Kill()
        Next
    End Sub

    Public Sub MyRestoreProgress()

        Dim Finished As Integer
        Dim pName As String = "secinspect"
        'Dim psList() As Process


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
        ProgressBar1.Style = ProgressBarStyle.Marquee



        'Do Until Finished = 1
        '    p = Process.GetProcessesByName("secinspect")
        '    If p.Count > 0 Then
        '        ' Process is running
        '        Finished = 0
        '    Else
        '        ' Process is not running
        '        Finished = 1
        '    End If
        'Loop


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
        MsgBox("Restore now complete")
        ProgressBar1.Style = ProgressBarStyle.Blocks

    End Sub
End Class