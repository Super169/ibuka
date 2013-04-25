Public Class frmComicInfo

    Public Sub SetComic(ByRef comic As BukaComicInfo)
        lblName.Text = comic.name
        lblAuthor.Text = comic.author
        txtIntro.Text = comic.intro

    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class