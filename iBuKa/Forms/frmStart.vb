Option Explicit On
Public Class frmStart

    Private Sub btnQuite_Click(sender As System.Object, e As System.EventArgs) Handles btnQuit.Click
        Application.Exit()
    End Sub

    Private Sub frmStart_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lblVersion.Text = "(Version " + My.Application.Info.Version.ToString() + ")"
    End Sub

    Private Sub btnOpen_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenFile.Click
        ofd.Filter = "Buka File (*.buka) | *.buka"
        If (ofd.ShowDialog = Windows.Forms.DialogResult.OK) Then
            frmReader.setStartUpFile(ofd.FileName)
            frmReader.Show()
            Me.Hide()
        End If
    End Sub

End Class