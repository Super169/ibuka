Public Class myUtil

    Public Shared Function getInt(ByRef buffer() As Byte, ByVal idx As Integer) As Integer
        Return (buffer(idx) + (buffer(idx + 1) * &H100) + (buffer(idx + 2) * &H10000) + (buffer(idx + 3) * &H1000000))
    End Function

End Class
