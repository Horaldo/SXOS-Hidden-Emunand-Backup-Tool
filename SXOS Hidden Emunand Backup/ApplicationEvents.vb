Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown
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
    End Class
End Namespace
