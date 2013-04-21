Public Class bukaPage
    Dim m_name As String
    Dim m_startPos As Integer
    Dim m_length As Integer

    Public Sub New(ByVal name As String, ByVal start As Integer, ByVal length As Integer)
        m_name = name
        m_startPos = start
        m_length = length
    End Sub

    Public ReadOnly Property name As String
        Get
            Return m_name
        End Get
    End Property

    Public ReadOnly Property startPos As Integer
        Get
            Return m_startPos
        End Get
    End Property

    Public ReadOnly Property length As Integer
        Get
            Return m_length
        End Get
    End Property

End Class
