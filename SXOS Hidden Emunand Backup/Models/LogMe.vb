Imports System.IO

Public Class LogMe
    ReadOnly strFile As String = "logger.txt"
    ReadOnly fileExists As Boolean = File.Exists(strFile)

    Friend Sub SetupLogFile()
        Dim Message As String = "SXOS Loggine Output File for SXOS Hidden Emunand Backup"

        If Not fileExists Then
            Using sw As New StreamWriter(File.Open(strFile, FileMode.OpenOrCreate))
                sw.WriteLine(Message)
            End Using
        Else
            Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
                sw.WriteLine(Message)
            End Using
        End If
    End Sub

    Friend Sub Log(ByRef message As String, Optional ex As Exception = Nothing, Optional showUser As Boolean = False, Optional severity As MessageBoxIcon = MessageBoxIcon.None)
        Console.WriteLine(message)

        If Not fileExists Then
            Using sw As New StreamWriter(File.Open(strFile, FileMode.OpenOrCreate))
                sw.WriteLine(message)
            End Using
        Else
            Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
                sw.WriteLine(message)
            End Using
        End If

        If showUser Then
            MessageBox.Show(message, "ALERT - MESSAGE", MessageBoxButtons.OK, severity)
        End If
    End Sub
End Class
