Option Explicit On
Imports System.IO

Public Class BukaBookInfo
    Dim m_title As String
    Dim m_idx As Integer
    Dim m_type As Integer
    Dim m_cid As Integer
    Dim m_size As Integer
    Dim m_fileName As String
    Dim m_fileExists As Boolean

    Public ReadOnly Property SortKey As Integer
        Get
            ' should not have over 999999 books for one comic
            Return m_type * 1000000 + m_idx
        End Get
    End Property

    Public ReadOnly Property FileName As String
        Get
            Return m_fileName
        End Get
    End Property

    Public Sub New(ByVal filePath As String, ByVal title As String, ByVal idx As Integer, ByVal type As Integer, ByVal cid As Integer, ByVal size As Integer)
        m_title = title
        m_idx = idx
        m_type = type
        m_cid = cid
        m_size = size
        m_fileName = Path.Combine(filePath, cid.ToString.Trim & ".buka")
        m_fileExists = File.Exists(m_fileName)
    End Sub

    Public ReadOnly Property BookInfo As String
        Get
            Dim sInfo As String = ""
            Select Case m_type
                Case 0
                    sInfo = m_idx.ToString & " 話"
                Case 1
                    sInfo = m_idx.ToString & " 卷"
                Case 2
                    sInfo = m_title
                Case Else
                    If (m_title > "") Then
                        sInfo = m_title
                    Else
                        sInfo = m_idx.ToString
                    End If
            End Select
            Return sInfo
        End Get
    End Property

    Public ReadOnly Property FileInfo As String
        Get
            If m_fileExists Then
                Return "[" & Me.BookInfo & "]"
            Else
                Return " " & Me.BookInfo & " "
            End If
        End Get
    End Property

End Class
