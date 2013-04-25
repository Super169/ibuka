Option Explicit On
Imports System.IO

Public Class SortBukaFiles

    Implements IComparer

    Public Function Compare( _
     ByVal x As Object, ByVal y As Object) As Integer _
     Implements System.Collections.IComparer.Compare
        Dim file1 As FileInfo = CType(x, FileInfo)
        Dim file2 As FileInfo = CType(y, FileInfo)
        Dim i1 As Integer = CType(file1.Name.Replace(".buka", ""), Integer)
        Dim i2 As Integer = CType(file2.Name.Replace(".buka", ""), Integer)
        Return i1.CompareTo(i2)
    End Function
End Class
