Public Class SXOSDrive
    Private _DriveLetter As String
    Private _DrivePhysicalName As String
    Private _DriveType As String
    Private _DriveVolumeName As String
    Private _DriveFileSystem As String

    Public Property DriveLetter As String
        Get
            Return _DriveLetter
        End Get
        Set
            _DriveLetter = Value
        End Set
    End Property

    Public Property DrivePhysicalName As String
        Get
            Return _DrivePhysicalName
        End Get
        Set
            _DrivePhysicalName = Value
        End Set
    End Property

    Public Property DriveType As String
        Get
            Return _DriveType
        End Get
        Set
            _DriveType = Value
        End Set
    End Property

    Public Property DriveVolumeName As String
        Get
            Return _DriveVolumeName
        End Get
        Set
            _DriveVolumeName = Value
        End Set
    End Property

    Public Property DriveFileSystem As String
        Get
            Return _DriveFileSystem
        End Get
        Set
            _DriveFileSystem = Value
        End Set
    End Property
End Class
