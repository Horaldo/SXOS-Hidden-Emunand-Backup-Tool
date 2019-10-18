Imports System.IO

Public Class LogMe
    ReadOnly strFile As String = "logger.txt"
    ReadOnly fileExists As Boolean = File.Exists(strFile)

    Public Sub New()
        Dim Message As String = "SXOS Loggine Output File for SXOS Hidden Emunand Backup" & vbCrLf

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

    Public Sub Log(ByRef message As String, Optional ex As Exception = Nothing, Optional showUser As Boolean = False, Optional severity As MessageBoxIcon = MessageBoxIcon.None)
        If ex IsNot Nothing Then
            message += vbCrLf
            message += vbCrLf
            message += ex.Message
            message += vbCrLf
            message += vbCrLf
            message += ex.StackTrace
            If Not ex.InnerException Is Nothing Then
                message += vbCrLf
                message += vbCrLf
                message += ex.InnerException.ToString
            End If
        End If

        If showUser Then
            MessageBox.Show(message, "ALERT - MESSAGE", MessageBoxButtons.OK, severity)
        End If

        Console.WriteLine(message)

        If fileExists Then
            Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
                sw.WriteLine(message)
            End Using
        End If
    End Sub
End Class
