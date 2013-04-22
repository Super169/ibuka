Option Explicit On

Public Class SortBukaPage
    Implements IComparer

    Public Function Compare( _
     ByVal x As Object, ByVal y As Object) As Integer _
     Implements System.Collections.IComparer.Compare
        Dim p1 As bukaPage = CType(x, bukaPage)
        Dim p2 As bukaPage = CType(y, bukaPage)

        Return p1.name.CompareTo(p2.name)
    End Function
End Class
