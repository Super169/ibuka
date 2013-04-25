Option Explicit On
Imports System.IO
Imports System.Text.RegularExpressions

Public Class frmReader

    Private idxSignature As Byte() = System.Text.Encoding.ASCII.GetBytes("index2.dat")
    Dim buffer(10240) As Byte ' Assume the header must be within 10K
    Dim bufferSize As Integer

    Const rightPanel = 100
    Const initBookSize = 100
    Const extBookSize = 20

    Dim fileIdx As Integer


    Dim comic As BukaComicInfo

    Dim bukaBook(initBookSize) As bukaPage

    ' Dim bookLogo As bukaPage
    ' Dim chaporderBuka As bukaPage
    Dim pageCnt As Integer = 0
    Dim pageIdx As Integer = -1

    Dim fixTop As Boolean = False

    Dim fs As FileStream
    Dim viewMode As DisplayMode = DisplayMode.AutoFit
    Dim imageLoaded As Boolean = False
    Dim startUpFile As String
    Dim startUpPage As Integer
    ' Dim comicName As String = ""


    Public Sub setStartUpFile(ByVal fn As String, Optional ByVal sp As Integer = 0)
        startUpFile = fn
        startUpPage = sp
    End Sub

    Private Sub frmReader_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Size = New Size(myConfig.winWidth, myConfig.winHeight)
        Me.Location = New Point(myConfig.winX, myConfig.winY)
        Me.cboViewMode.SelectedIndex = myConfig.viewMode
        Me.SetDirectory(startUpFile)
    End Sub


    Private Function getBukaPage(ByRef idx As Integer) As bukaPage
        Dim iStart, iLength As Integer
        Dim fname As String
        iStart = getInt(idx)
        iLength = getInt(idx + 4)
        fname = getString(idx + 8)
        idx = idx + 8 + fname.Length + 1
        Return New bukaPage(fname, iStart, iLength)
    End Function

    Private Function getInt(ByVal idx As Integer) As Integer
        Return (buffer(idx) + (buffer(idx + 1) * &H100) + (buffer(idx + 2) * &H10000) + (buffer(idx + 3) * &H1000000))
    End Function

    Private Function getString(ByVal idx As Integer) As String
        Dim sReturn As String = ""
        Dim i As Integer = idx
        Do While (i < bufferSize)
            If (buffer(i) = 0) Then
                Exit Do
            End If
            sReturn = sReturn & Chr(buffer(i))
            i = i + 1
        Loop
        Return sReturn
    End Function

    Private Sub Addpage(ByVal page As bukaPage)
        If pageCnt >= bukaBook.Length Then
            ReDim Preserve bukaBook(bukaBook.Length + extBookSize)
        End If
        bukaBook(pageCnt) = page
        pageCnt = pageCnt + 1
    End Sub


    Private Sub frmReader_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.PageUp, Keys.Subtract, Keys.Up
                If (pageIdx > 0) Then
                    readPage(pageIdx - 1)
                Else
                    goPrevBook()
                    readPage(pageCnt - 1)
                End If
            Case Keys.PageDown, Keys.Add, Keys.Down
                If (pageIdx < pageCnt - 1) Then
                    readPage(pageIdx + 1)
                Else
                    goNextBook()
                End If
            Case Keys.Home
                readPage(0)
            Case Keys.End
                readPage(pageCnt - 1)
            Case Keys.Insert, Keys.Left
                goPrevBook()
            Case Keys.Delete, Keys.Right
                goNextBook()
        End Select
        ' disable all keys
        e.Handled = True
    End Sub

    Private Sub goNextBook()
        If (fileIdx < comic.books.Length - 1) Then
            fileIdx = fileIdx + 1
            SetFile()
        End If
    End Sub

    Private Sub goPrevBook()
        If (fileIdx > 0) Then
            fileIdx = fileIdx - 1
            SetFile()
        End If
    End Sub

    Private Sub Form1_Move(sender As Object, e As System.EventArgs) Handles Me.Move
        If (fixTop And Me.Top <> 0) Then Me.Top = 0
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
        If Not imageLoaded Then Return
        showPage()
    End Sub

    Private Sub WindowsFit()
        If Not imageLoaded Then Return
        Dim w, h As Integer
        Dim imgWH, scrWH As Double
        Dim scrW As Integer = Screen.PrimaryScreen.WorkingArea.Width
        Dim scrH As Integer = Screen.PrimaryScreen.WorkingArea.Height
        scrWH = (scrW - rightPanel) / scrH
        imgWH = pbImage.Image.Width / pbImage.Image.Height
        If (imgWH > scrWH) Then
            viewMode = DisplayMode.FitWidth
            w = scrW
            h = scrH  ' use full screen in this case
        Else
            viewMode = DisplayMode.FitHeight
            h = scrH
            w = Math.Min(h * imgWH + rightPanel, scrW)
        End If
        Me.WindowState = FormWindowState.Normal
        Me.Size = New Size(w, h)
        If (w < scrW) Then
            Me.Location = New Point((scrW - w) / 2, 0)
        End If

    End Sub

#Region "lvPage Handler"

    Private Sub lv_DrawColumnHeader(sender As Object, e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs) Handles lvPages.DrawColumnHeader, lvBooks.DrawColumnHeader
        e.DrawDefault = True
    End Sub

    Private Sub lv_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvPages.KeyDown, lvBooks.KeyDown
        e.Handled = True
    End Sub

    Private Sub lv_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles lvPages.KeyPress, lvBooks.KeyPress
        e.Handled = True
    End Sub

    Private Sub lv_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvPages.KeyUp, lvBooks.KeyUp
        e.Handled = True
    End Sub

    Private Sub lv_DrawItem(sender As Object, e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvPages.DrawItem, lvBooks.DrawItem
        Dim lv As ListView = sender
        Dim clrSelectedText As Color = Color.Black
        Dim clrHighlight As Color = Color.Orange
        If (e.Item.Selected) Then
            e.Graphics.FillRectangle(New SolidBrush(clrHighlight), e.Bounds)
            e.Graphics.DrawString(lv.Items.Item(e.ItemIndex).Text, e.Item.Font, New SolidBrush(clrSelectedText), e.Bounds) 'Draw the text for the item
        Else
            e.DrawBackground()
            e.Graphics.DrawString(lv.Items.Item(e.ItemIndex).Text, e.Item.Font, Brushes.Black, e.Bounds) 'Draw the item text in its regular(color)
        End If
    End Sub

    Private Sub lvBooks_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvBooks.SelectedIndexChanged
        Dim lv As ListView = sender
        If lv.SelectedIndices.Count() > 0 Then
            If (fileIdx = lv.SelectedIndices(0)) Then Return
            fileIdx = lv.SelectedIndices(0)
            SetFile()
        End If
    End Sub

    Private Sub lvPages_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvPages.SelectedIndexChanged
        Dim lv As ListView = sender
        If lv.SelectedIndices.Count() > 0 Then
            readPage(lv.SelectedIndices(0))
        End If
    End Sub

#End Region

    Private Sub btnOpenFile_Click(sender As System.Object, e As System.EventArgs) Handles btnOpen.Click
        ofd.Filter = "Buka File (*.buka) | *.buka"
        If (ofd.ShowDialog = Windows.Forms.DialogResult.OK) Then
            If (ofd.FileName = myConfig.lastFile) Then Return
            SetDirectory(ofd.FileName)
        End If
    End Sub


    Public Sub SetDirectory(ByVal filename As String)
        If Not File.Exists(filename) Then Return

        comic = New BukaComicInfo(filename)

        If Not comic.ready Then Return

        lvBooks.Clear()
        lvBooks.Columns.Clear()
        lvBooks.Columns.Add("", 0)
        If comic.fromDAT Then
            lvBooks.Columns.Add("DAT", 55, HorizontalAlignment.Left)
        Else
            lvBooks.Columns.Add("File", 55, HorizontalAlignment.Left)
        End If
        lvBooks.Columns.RemoveAt(0)

        fileIdx = -1
        For idx = 0 To comic.books.Length - 1
            lvBooks.Items.Add(New ListViewItem(comic.books(idx).FileInfo))
            If (comic.books(idx).FileName = filename) Then
                fileIdx = idx
            End If
        Next

        SetFile()
    End Sub

    Private Sub SetFile()
        lvBooks.Items(fileIdx).Selected = True
        lvBooks.EnsureVisible(fileIdx)
        lvBooks.Invalidate()
        ReadFile()
    End Sub


    Private Sub ReadFile()
        Dim fn As String = comic.books(fileIdx).FileName

        If Not File.Exists(fn) Then
            lvPages.Items.Clear()
            pageCnt = -1
            pageIdx = -1
            pbImage.Image = Nothing
            pbImage.Size = New Size(0, 0)
            'pbImage.Width = panImage.Width - 25
            'pbImage.Height = panImage.Height - 25
            lblMessage.Width = panImage.Width
            lblMessage.Text = "請先到布卡漫畫下載 " & comic.name & " " & comic.books(fileIdx).BookInfo
            lblMessage.Visible = True
            lblMessage.BringToFront()
            Return
        End If
        lblMessage.Visible = False

        Dim MyFile As New FileInfo(fn)

        If (fs IsNot Nothing) Then fs.Close()

        fs = New FileStream(fn, FileMode.Open, FileAccess.Read)
        lvPages.Clear()
        lvPages.Columns.Clear()
        lvPages.Columns.Add("", 0)
        lvPages.Columns.Add("Page", 55, HorizontalAlignment.Left)
        lvPages.Columns.RemoveAt(0)

        pageCnt = 0
        pageIdx = -1

        fs.Seek(0, SeekOrigin.Begin)
        bufferSize = fs.Read(buffer, 0, 10240)
        Dim vol As Integer
        vol = buffer(17) * 256 + buffer(16)

        Dim idxPos = 0
        For idx = 0 To 10240
            If (buffer(idx) = idxSignature(0)) Then
                Dim match As Boolean = True
                For i = 1 To idxSignature.Length - 1
                    If (buffer(idx + i) <> idxSignature(i)) Then
                        match = False
                        Exit For
                    End If
                Next
                If match Then
                    idxPos = idx
                    Exit For
                End If
            End If
        Next

        If idxPos = 0 Then
            Return
        End If
        idxPos = idxPos + 11

        Do While True
            Dim o As bukaPage = getBukaPage(idxPos)
            If o.name = "logo" Then
                ' no action required
            ElseIf o.name = "chaporder.dat" Then
                ' no action required
                Exit Do
            Else
                Addpage(o)
            End If
        Loop

        Array.Sort(bukaBook, 0, pageCnt, New SortBukaPage)
        For idx = 1 To pageCnt
            lvPages.Items.Add(New ListViewItem({idx}))
        Next

        ' comicName = ReadName()

        If startUpPage > 0 Then
            readPage(startUpPage)
            startUpPage = 0
        Else
            readPage(0)
        End If

    End Sub

    Private Sub readPage(ByVal page As Integer)
        If (page < 0) Then Return
        If (page >= pageCnt) Then Return
        If (pageIdx = page) Then Return
        Dim o As bukaPage = bukaBook(page)

        fs.Seek(o.startPos, SeekOrigin.Begin)
        Dim jpgBuffer(o.length - 1) As Byte
        fs.Read(jpgBuffer, 0, o.length)

        Dim ms As System.IO.MemoryStream
        ms = New System.IO.MemoryStream(jpgBuffer)
        pbImage.Image = System.Drawing.Image.FromStream(ms)
        imageLoaded = True
        pageIdx = page
        lvPages.Items(page).EnsureVisible()
        lvPages.Items(page).Selected = True
        lvPages.Invalidate()

        showPage()
    End Sub

    Public Sub showPage()
        If Not imageLoaded Then Return
        Dim w, h As Integer
        Dim imgWH, panWH As Double
        Dim mode As DisplayMode
        imgWH = pbImage.Image.Width / pbImage.Image.Height
        panWH = panImage.Width / panImage.Height
        If viewMode = DisplayMode.AutoFit Then
            If (imgWH >= panWH) Then
                mode = DisplayMode.FitWidth
            Else
                mode = DisplayMode.FitHeight
            End If
        Else
            mode = viewMode
        End If
        Select Case mode
            Case DisplayMode.ActualSize
                w = pbImage.Image.Width
                h = pbImage.Image.Height
            Case DisplayMode.FitHeight
                If (imgWH <= panWH) Then
                    ' No scroll bar required
                    h = panImage.Height - 2
                Else
                    h = panImage.Height - 25
                End If
                w = pbImage.Image.Width * h / pbImage.Image.Height
            Case DisplayMode.FitWidth
                If (panWH <= imgWH) Then
                    w = panImage.Width - 2
                Else
                    w = panImage.Width - 25

                End If
                h = pbImage.Image.Height * w / pbImage.Image.Width
        End Select
        ShowHeader()
        pbImage.SizeMode = PictureBoxSizeMode.Zoom
        ' A bug in VB, must resize to disable the scrollbar then redraw, otherwise, the scroll bar may displayed even fully displayed
        pbImage.Size = New Size(0, 0)
        pbImage.Size = New Size(w, h)
        pbImage.Focus()
    End Sub

    Private Sub ShowHeader()
        Me.Text = "iBuka@Super169    " & comic.name & "  (" & comic.books(fileIdx).BookInfo & ")   [page " & (pageIdx + 1) & "/" & pageCnt & "]"
    End Sub


    Private Sub cboViewMode_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboViewMode.SelectedIndexChanged
        viewMode = cboViewMode.SelectedIndex
        If viewMode = DisplayMode.WinFit Then
            WindowsFit()
        End If
        showPage()
    End Sub



    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If fs IsNot Nothing Then
            fs.Close()
        End If
        myConfig.winX = Me.Left
        myConfig.winY = Me.Top
        myConfig.winWidth = Me.Width
        myConfig.winHeight = Me.Height
        myConfig.viewMode = viewMode
        If comic.ready Then
            myConfig.lastFile = comic.books(fileIdx).FileName
            myConfig.lastPage = pageIdx
        Else
            myConfig.lastFile = ""
            myConfig.lastPage = -1
        End If
        myConfig.SaveKey()
    End Sub


    Private Sub frmReader_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub


    Private Sub btnAbout_Click(sender As System.Object, e As System.EventArgs) Handles btnAbout.Click
        frmAbout.Show()
    End Sub

    Private Sub btnInfo_Click(sender As System.Object, e As System.EventArgs) Handles btnInfo.Click
        frmComicInfo.SetComic(comic)
        frmComicInfo.ShowDialog()
    End Sub
End Class