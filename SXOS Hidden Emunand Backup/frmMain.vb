Imports System.IO
Imports System.Linq
Imports SXOSHiddenEmunandBackupTool.SXOSDrive


Public Class frmMain

    Dim bsrc As New BindingSource
    Dim dset As New DataSet
    Dim drive As New SXOSDrive
    ReadOnly inspector As String = "secinspect.exe"
    Dim SelectedFolder As String = ""

    Enum BorR
        Backup
        Restore
    End Enum

    Dim Backupcommand As String
    Dim RestoreCommand As String
    Dim appPath As String = Application.StartupPath()
    Dim BinaryName As String
    Dim BinaryFileSize As Long
    Dim CanceledOperation As Integer

    Dim ToggleDebug As Boolean
    Dim p As Process()


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

    Private Sub RefreshDrives_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshDrives.Click
        GetDriveInfo()
    End Sub

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.lvDriveInfo.View = View.Details

        Dim SetupPath As String = appPath & "\" & inspector

        If System.IO.File.Exists(SetupPath) Then
            Using sCreateMSIFile As New FileStream(SetupPath, FileMode.Create)
                sCreateMSIFile.Write(My.Resources.secinspect, 0, My.Resources.secinspect.Length)
            End Using
        End If

        GetDriveInfo()
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
                        drive.DriveLetter = myString
                    Case 2
                        drive.DrivePhysicalName = myString.Replace(" ", "")
                    Case 3
                        drive.DriveType = myString
                    Case 4
                        drive.DriveVolumeName = myString
                    Case 5
                        drive.DriveFileSystem = myString
                End Select
                innercounter += 1
            Next
        End With
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
                        CalculateFileSelection(BorR.Backup)
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


    Private Sub OverwriteCheck()
        If BOOT0.Checked Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathTextbox.Text + "BOOT0.BIN") Then
                Select Case MsgBox("BOOT0.BIN already exists. Overwrite?", MsgBoxStyle.YesNo, "FILE ALREADY EXISTS")
                    Case MsgBoxResult.Yes
                        DoBackup()
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                DoBackup()
            End If
        End If

        If BOOT1.Checked Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathTextbox.Text + "BOOT1.BIN") Then
                Select Case MsgBox("BOOT1.BIN already exists. Overwrite?", MsgBoxStyle.YesNo, "FILE ALREADY EXISTS")
                    Case MsgBoxResult.Yes
                        DoBackup()
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                DoBackup()
            End If
        End If

        If RAWNAND.Checked Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathTextbox.Text + "RAWNAND.BIN") Then
                Select Case MsgBox("RAWNAND.BIN already exists. Overwrite?", MsgBoxStyle.YesNo, "FILE ALREADY EXISTS")
                    Case MsgBoxResult.Yes
                        DoBackup()
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                DoBackup()
            End If
        End If
    End Sub

    Private Sub DoBackup()
        CalculateFileSelection(BorR.Backup)

        Dim myProcessStartInfo As New ProcessStartInfo()

        With myProcessStartInfo
            .FileName = inspector
            .Arguments = Backupcommand
            '.Verb = "runas"

            .CreateNoWindow = True
            .UseShellExecute = False
        End With

        Process.Start(myProcessStartInfo)

        Fileprogress()
    End Sub

    Private Sub RestoreCheck()
        If BOOT0.Checked Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathTextbox.Text + "BOOT0.BIN") Then
                Select Case MsgBox("Are you sure you want to restore BOOT0.BIN to " & LocationTextBox.Text & "?", MsgBoxStyle.YesNo, "CONFIRM RESTORE")
                    Case MsgBoxResult.Yes
                        DoRestore()
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                MsgBox("BOOT0.BIN not found in " & BackupLocationPathTextbox.Text)
            End If
        End If

        If BOOT1.Checked Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathTextbox.Text + "BOOT1.BIN") Then
                Select Case MsgBox("Are you sure you want to restore BOOT1.BIN to " & LocationTextBox.Text & "?", MsgBoxStyle.YesNo, "CONFIRM RESTORE")
                    Case MsgBoxResult.Yes
                        DoRestore()
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                MsgBox("BOOT1.BIN not found in " & BackupLocationPathTextbox.Text)
            End If
        End If

        If RAWNAND.Checked Then
            If My.Computer.FileSystem.FileExists(BackupLocationPathTextbox.Text + "RAWNAND.BIN") Then
                Select Case MsgBox("Are you sure you want to restore RAWNAND.BIN to " & LocationTextBox.Text & "?", MsgBoxStyle.YesNo, "CONFIRM RESTORE")
                    Case MsgBoxResult.Yes
                        DoRestore()
                    Case MsgBoxResult.No
                        Exit Sub
                End Select
            Else
                MsgBox("RAWNAND.BIN not found in " & BackupLocationPathTextbox.Text)
            End If
        End If
    End Sub

    Private Sub DoRestore()
        CalculateFileSelection(BorR.Restore)

        Dim myProcessStartInfo As New ProcessStartInfo()

        With myProcessStartInfo
            .FileName = inspector
            .Arguments = RestoreCommand
            '.Verb = "runas"

            .CreateNoWindow = False
            .UseShellExecute = True

            .RedirectStandardOutput = True

        End With

        Process.Start(myProcessStartInfo)

        MyRestoreProgress()
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
                            CalculateFileSelection(BorR.Restore)
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


    Private Sub CalculateFileSelection(BorR As BorR)
        LocationTextBox.Text = drive.DriveVolumeName
        BinaryName = IIf(BOOT0.Checked, "BOOT0.BIN", IIf(BOOT1.Checked, "BOOT1.BIN", IIf(RAWNAND.Checked, "RAWNAND.BIN", "")))

        Dim cmd = String.Format("-{0} {1} ""{2}"" {3} {4}", BorR.ToString,
                                                            drive.DrivePhysicalName,
                                                            SelectedFolder & BinaryName,
                                                            IIf(BOOT0.Checked, "2", IIf(BOOT1.Checked, "8194", IIf(RAWNAND.Checked, "16386", ""))),
                                                            IIf(BorR = BorR.Backup, IIf(BOOT0.Checked, "8192", IIf(BOOT1.Checked, "8192", IIf(RAWNAND.Checked, "61071360", ""))), ""))

        'If BOOT0.Checked = True Then
        '    Backupcommand = "-backup " + drive.DrivePhysicalName + " " + """" + SelectedFolder + "BOOT0.BIN" + """ " + "2 8192"
        '    RestoreCommand = "-restore " + drive.DrivePhysicalName + " " + """" + SelectedFolder + "BOOT0.BIN" + """ " + "2"
        '    BinaryName = "BOOT0.BIN"
        '    BinaryFileSize = 4194304
        'End If

        'If BOOT1.Checked = True Then
        '    Backupcommand = "-backup " + drive.DrivePhysicalName + " " + """" + SelectedFolder + "BOOT1.BIN" + """ " + "8194 8192"
        '    RestoreCommand = "-restore " + drive.DrivePhysicalName + " " + """" + SelectedFolder + "BOOT1.BIN" + """ " + "8194"
        '    BinaryName = "BOOT1.BIN"
        '    BinaryFileSize = 4194304
        'End If

        'If RAWNAND.Checked = True Then
        '    Backupcommand = "-backup " + drive.DrivePhysicalName + " " + """" + SelectedFolder + "RAWNAND.BIN" + """ " + "16386 61071360"
        '    RestoreCommand = "-restore " + drive.DrivePhysicalName + " " + """" + SelectedFolder + "RAWNAND.BIN" + """ " + "16386"
        '    BinaryName = "RAWNAND.BIN"
        '    BinaryFileSize = 31268536320
        'End If

        'Console.WriteLine("Backup Command: " & Backupcommand)
        'Console.WriteLine("Restore Command: " & RestoreCommand)

        Console.WriteLine(cmd)
    End Sub

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

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        KillProcess()
        ProgressBar1.Value = 0
        FileSizeTextBox.Text = " "
        CanceledOperation = 1
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


            If CanceledOperation = 1 Then
                CanceledOperation = 0
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