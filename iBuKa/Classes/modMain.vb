
Module modMain

    Sub main()
        myConfig.ReadKey()
        If (myConfig.lastFile > "") And System.IO.File.Exists(myConfig.lastFile) Then
            frmReader.setStartUpFile(myConfig.lastFile, myConfig.lastPage)
            frmReader.Show()
        Else
            frmStart.Show()
        End If
        Application.Run()
    End Sub

End Module

