Public Class frmAbout

    Private Sub frmAbout_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lblVersion.Text = "(Version " + My.Application.Info.Version.ToString() + ")"
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class