Imports System.IO
Imports System.Linq

Public Class frmMain
    Dim SXOSDriveLetter As String
    Dim SXOSDrivePhysicalName As String
    Dim SXOSDriveType As String
    Dim SXOSDriveVolumeName As String
    Dim SXOSDriveFileSystem As String

    Dim Backupcommand As String
    Dim FolderLocation As String
    Dim FolderString As String
    Dim appPath As String = Application.StartupPath()
    Dim BinaryName As String
    Dim BinaryFileSize As Long
    Dim CanceledOperation As Integer


    Private Sub GetDriveInfo()

        Dim count As Int32 = 0
        Dim driveInfoEx As New clsDiskInfoEx
        Dim multipleParents() As String = Nothing
        Dim parentDrives As String = Nothing

        lvDriveInfo.Items.Clear()

        For Each drive As System.IO.DriveInfo In System.IO.DriveInfo.GetDrives
            'If drive.DriveType = IO.DriveType.Fixed Then


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

            'End If
        Next
    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

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

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        ' Trigger form.resize()
        Me.Width += 1
        Me.Refresh()

        GetDriveInfo()
        CalculateFileSelection()
    End Sub

    Private Sub RefreshDrives_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshDrives.Click
        GetDriveInfo()
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.Text = appPath
    End Sub

    Private Sub lvDriveInfo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvDriveInfo.SelectedIndexChanged
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

        If SXOSDriveLetter = Nothing Then
            MsgBox("Please Select Your Drive To Backup")
            Exit Sub
        Else

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

        End If

    End Sub

    Private Sub BOOT0_CheckedChanged(sender As Object, e As EventArgs) Handles BOOT0.CheckedChanged
        CalculateFileSelection()
    End Sub

    Private Sub BOOT1_CheckedChanged(sender As Object, e As EventArgs) Handles BOOT1.CheckedChanged
        CalculateFileSelection()
    End Sub
    Private Sub RAWNAND_CheckedChanged(sender As Object, e As EventArgs) Handles RAWNAND.CheckedChanged
        CalculateFileSelection()
    End Sub
    Private Sub Browse_Click(sender As Object, e As EventArgs) Handles Browse.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            FolderLocation = FolderBrowserDialog1.SelectedPath
            FolderString = FolderLocation
            If FolderString.EndsWith("\") = False Then
                FolderString = FolderLocation & "\"
            End If
            BackupLocationPathTextbox.Text = FolderString
            CalculateFileSelection()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'CalculateFileSelection()
        'Test
        'TextBox1.Text = FolderString & BinaryName
    End Sub

    Private Sub CalculateFileSelection()
        If BackupLocationPathTextbox.Text = "" Then
            BOOT0.Enabled = False
            BOOT1.Enabled = False
            RAWNAND.Enabled = False
            Backup.Enabled = False
        Else
            BOOT0.Enabled = True
            BOOT0.Checked = True
            BOOT1.Enabled = True
            RAWNAND.Enabled = True
            Backup.Enabled = True
        End If
        If BOOT0.Checked = True Then
            Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + FolderString + "BOOT0.BIN 2 8192"
            'TextBox1.Text = Backupcommand
            BinaryName = "BOOT0.BIN"
            BinaryFileSize = 4194304
        End If

        If BOOT1.Checked = True Then
            Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + FolderString + "BOOT1.BIN 8194 8192"
            'TextBox1.Text = Backupcommand
            BinaryName = "BOOT1.BIN"
            BinaryFileSize = 4194304
        End If

        If RAWNAND.Checked = True Then
            Backupcommand = "-backup " + SXOSDrivePhysicalName + " " + FolderString + "RAWNAND.BIN 16386 61071360"
            'TextBox1.Text = Backupcommand
            BinaryName = "RAWNAND.BIN"
            BinaryFileSize = 31268536320
        End If


    End Sub

    Dim DoubleBytes As Double
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
        Catch
            Return ""
        End Try

    End Function

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click

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

        ProgressBar1.Value = 0
        FileSizeTextBox.Text = " "
        CanceledOperation = 1

    End Sub

    Private Sub Fileprogress()
        Threading.Thread.Sleep(1000)
        Application.DoEvents()

        Dim ConvertedBinaryFileSize As Long
        ConvertedBinaryFileSize = CLng(BinaryFileSize / 1024)
        ProgressBar1.Maximum = CInt(ConvertedBinaryFileSize)

        Dim flength As Long
        flength = FileLen(FolderString & BinaryName)

        Do Until flength = BinaryFileSize
            flength = FileLen(FolderString & BinaryName)
            'TextBox5.Text = flength.ToString
            FileSizeTextBox.Text = (FormatBytes(CULng(flength.ToString)))
            ProgressBar1.Value = CInt(flength / 1024)
            Application.DoEvents()

            If CanceledOperation = 1 Then
                CanceledOperation = 0
                Exit Sub
            End If
        Loop
        'TextBox5.Text = flength.ToString
        FileSizeTextBox.Text = (FormatBytes(CULng(flength.ToString)))
        ProgressBar1.Value = CInt(flength / 1024)
    End Sub


End Class
