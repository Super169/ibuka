Option Explicit On
Imports System.IO

Public Class SortFiles
    Implements IComparer

    Public Function Compare( _
     ByVal x As Object, ByVal y As Object) As Integer _
     Implements System.Collections.IComparer.Compare
        Dim file1 As FileInfo = CType(x, FileInfo)
        Dim file2 As FileInfo = CType(y, FileInfo)

        Return file1.Name.CompareTo(file2.Name)
    End Function
End Class
