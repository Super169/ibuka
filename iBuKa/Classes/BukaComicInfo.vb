Option Explicit On
Imports System.IO
Imports Newtonsoft.Json

Public Class BukaComicInfo

    Dim m_ready As Boolean = False
    Dim m_datFile As Boolean = False
    Dim m_filePath As String
    Dim m_books() As BukaBookInfo
    Dim m_name As String
    Dim m_author As String
    Dim m_intro As String
    Dim m_lastup As String
    Dim m_lastuptime As String
    Dim m_upno As Integer
    Dim m_lastupcid As Integer
    Dim m_logo As String

    Public Sub New(Optional ByVal fn As String = "")
        ReadChaporder(fn)
    End Sub

    Public Sub Reset()
        m_ready = False
        If m_books IsNot Nothing Then
            For Each o In m_books
                o = Nothing
            Next
        End If
        m_books = Nothing
        m_name = ""
        m_author = ""
        m_intro = ""
        m_lastup = ""
        m_lastuptime = ""
        m_upno = -1
        m_lastupcid = -1
        m_logo = -1
    End Sub

#Region "Properties"

    Public ReadOnly Property ready As Boolean
        Get
            Return m_ready
        End Get
    End Property

    Public ReadOnly Property fromDAT As Boolean
        Get
            Return m_datFile
        End Get
    End Property

    Public Property name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Public Property author As String
        Get
            Return m_author
        End Get
        Set(value As String)
            m_author = value
        End Set
    End Property

    Public Property intro As String
        Get
            Return m_intro
        End Get
        Set(value As String)
            m_intro = value
        End Set
    End Property

    Public ReadOnly Property books As BukaBookInfo()
        Get
            Return m_books
        End Get
    End Property

#End Region


    Public Sub addBook(ByVal book As BukaBookInfo)
        If m_books Is Nothing Then
            ReDim m_books(0)
        Else
            ReDim Preserve m_books(m_books.Length)
        End If
        m_books(m_books.Length - 1) = book
    End Sub

    Public Function ReadChaporder(ByVal filename As String) As Boolean

        Me.Reset()

        If Not File.Exists(filename) Then Return False
        Dim fi As FileInfo = New FileInfo(filename)
        m_filePath = fi.DirectoryName

        Dim reader As StreamReader
        Dim content As String
        Dim fn As String = Path.Combine(m_filePath, "chaporder.dat")

        Me.Reset()
        If (fn.Trim = "") Then Return False
        If Not File.Exists(fn) Then Return ReadChaporderFromFolder(filename)


        Try
            reader = New StreamReader(fn)
            content = reader.ReadToEnd
            reader.Close()

            If Not ReadChaporderFromJString(content, True) Then Return False

        Catch ex As Exception
            Return False
        End Try

        m_datFile = True
        m_ready = True
        Return True

    End Function

    Private Function ReadChaporderFromFolder(ByVal filename As String) As Boolean
        Dim fileSignature As Byte() = System.Text.Encoding.ASCII.GetBytes("chaporder.dat")

        If Not File.Exists(filename) Then Return False
        Dim fi As FileInfo = New FileInfo(filename)
        Dim fs As FileStream
        Dim buffer(1024) As Byte
        Dim bufferRead As Integer
        Dim iStart, iLength As Integer

        fs = New FileStream(filename, FileMode.Open, FileAccess.Read)

        fs.Seek(0, SeekOrigin.Begin)
        bufferRead = fs.Read(buffer, 0, 1024)
        Dim vol As Integer
        vol = buffer(17) * 256 + buffer(16)

        Dim idxPos = 0
        For idx = 0 To 1024
            If (buffer(idx) = fileSignature(0)) Then
                Dim match As Boolean = True
                For i = 1 To fileSignature.Length - 1
                    If (buffer(idx + i) <> fileSignature(i)) Then
                        match = False
                        Exit For
                    End If
                Next
                If match Then
                    idxPos = idx
                    Exit For
                End If
            End If
        Next

        If idxPos = 0 Then
            Return False
        End If
        idxPos = idxPos - 8
        iStart = myUtil.getInt(buffer, idxPos)
        iLength = myUtil.getInt(buffer, idxPos + 4)
        fs.Seek(iStart, SeekOrigin.Begin)
        ReDim buffer(iLength)
        fs.Read(buffer, 0, iLength)
        Dim s As String = (New System.Text.ASCIIEncoding()).GetString(buffer)
        fs.Close()

        If Not ReadChaporderFromJString(s, False) Then Return False

        Dim fa As FileInfo() = fi.Directory.GetFiles("*.buka")
        Array.Sort(fa, New SortBukaFiles)
        For idx = 0 To fa.Length - 1
            Dim o As FileInfo = fa(idx)
            addBook(New BukaBookInfo(m_filePath, "", ReadVol(o.FullName), 999, o.Name.Replace(".buka", ""), o.Length))
        Next

        m_datFile = False
        m_ready = True
        Return True

    End Function

    Private Function ReadVol(ByVal fn As String) As Integer
        Dim fs = New FileStream(fn, FileMode.Open, FileAccess.Read)
        Dim data(2) As Byte
        fs.Seek(16, SeekOrigin.Begin)
        fs.Read(data, 0, 2)
        fs.Close()
        Dim vol As Integer
        vol = data(1) * 256 + data(0)
        Return vol
    End Function

    Private Function ReadChaporderFromJString(ByRef content As String, ByVal readBooks As Boolean) As Boolean

        Dim JChaporder As Linq.JObject
        Dim Links As Linq.JArray


        Try

            JChaporder = Newtonsoft.Json.JsonConvert.DeserializeObject(content)
            If JChaporder Is Nothing Then Return False

            m_name = JChaporder.Item("name")
            m_author = JChaporder.Item("author")
            m_intro = JChaporder.Item("intro")
            m_lastup = JChaporder.Item("lastup")
            m_lastuptime = JChaporder.Item("lastuptime")
            m_upno = JChaporder.Item("upno")
            m_lastupcid = JChaporder.Item("lastupcid")
            m_logo = JChaporder.Item("logo")
            Links = JChaporder.Item("links")

            If readBooks Then
                For Each o In Links
                    addBook(New BukaBookInfo(m_filePath, o.Item("title"), o.Item("idx"), o.Item("type"), o.Item("cid"), o.Item("size")))
                Next
                Array.Sort(m_books, New SortBukaBooks)
            End If


        Catch ex As Exception
            Return False
        End Try

        m_ready = True
        Return True

    End Function


End Class
