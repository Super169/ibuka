Option Explicit On

Public Class SortBukaBooks
    Implements IComparer

    Public Function Compare( _
     ByVal x As Object, ByVal y As Object) As Integer _
     Implements System.Collections.IComparer.Compare

        Dim b1 As BukaBookInfo = CType(x, BukaBookInfo)
        Dim b2 As BukaBookInfo = CType(y, BukaBookInfo)
        
        Return b1.SortKey.CompareTo(b2.SortKey)
    End Function
End Class
