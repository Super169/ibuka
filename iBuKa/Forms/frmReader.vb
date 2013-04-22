Option Explicit On
Imports System.IO
Imports System.Text.RegularExpressions

Public Class frmReader

    Private idxSigature As Byte() = System.Text.Encoding.ASCII.GetBytes("index2.dat")
    Dim buffer(10240) As Byte ' Assume the header must be within 10K
    Dim bufferSize As Integer

    Const rightPanel = 80
    Const initBookSize = 100
    Const extBookSize = 20
    Dim bukaDir() As String
    Dim fileIdx As Integer
    Dim bukaBook(initBookSize) As bukaPage
    Dim bookLogo As bukaPage
    Dim chaporder As bukaPage
    Dim pageCnt As Integer = 0
    Dim pageIdx As Integer = -1

    Dim fixTop As Boolean = False

    Dim fs As FileStream
    Dim viewMode As DisplayMode = DisplayMode.AutoFit
    Dim imageLoaded As Boolean = False
    Dim startUpFile As String
    Dim startUpPage As Integer
    Dim comicName As String = ""

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
        If (fileIdx < bukaDir.Length - 1) Then
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
        Dim fi As FileInfo = New FileInfo(filename)
        Dim fa As FileInfo() = fi.Directory.GetFiles("*.buka")
        Array.Sort(fa, New SortFiles)

        lvBooks.Clear()
        lvBooks.Columns.Clear()
        lvBooks.Columns.Add("", 0)
        lvBooks.Columns.Add("Vol", 30, HorizontalAlignment.Right)
        lvBooks.Columns.RemoveAt(0)

        ReDim bukaDir(fa.Length - 1)
        For idx = 0 To fa.Length - 1
            bukaDir(idx) = fa(idx).FullName
            lvBooks.Items.Add(New ListViewItem(ReadVol(fa(idx).FullName)))
        Next
        fileIdx = Array.IndexOf(bukaDir, filename)
        SetFile()
    End Sub

    Private Sub SetFile()
        lvBooks.Items(fileIdx).Selected = True
        lvBooks.EnsureVisible(fileIdx)
        lvBooks.Invalidate()
        ReadFile()
    End Sub


    Private Sub ReadFile()
        Dim fn As String = bukaDir(fileIdx)
        If Not File.Exists(fn) Then
            MsgBox("File not found: " & fn)
            Return
        End If

        Dim MyFile As New FileInfo(fn)

        If (fs IsNot Nothing) Then fs.Close()

        fs = New FileStream(fn, FileMode.Open, FileAccess.Read)
        lvPages.Clear()
        lvPages.Columns.Clear()
        lvPages.Columns.Add("", 0)
        lvPages.Columns.Add("P#.", 30, HorizontalAlignment.Right)
        lvPages.Columns.RemoveAt(0)

        pageCnt = 0
        pageIdx = -1

        fs.Seek(0, SeekOrigin.Begin)
        bufferSize = fs.Read(buffer, 0, 10240)
        Dim vol As Integer
        vol = buffer(17) * 256 + buffer(16)

        Dim idxPos = 0
        For idx = 0 To 10240
            If (buffer(idx) = idxSigature(0)) Then
                Dim match As Boolean = True
                For i = 1 To idxSigature.Length - 1
                    If (buffer(idx + i) <> idxSigature(i)) Then
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
                bookLogo = o
            ElseIf o.name = "chaporder.dat" Then
                chaporder = o
                Exit Do
            Else
                Addpage(o)
            End If
        Loop

        Array.Sort(bukaBook, 0, pageCnt, New SortBukaPage)
        For idx = 1 To pageCnt
            lvPages.Items.Add(New ListViewItem({idx}))
        Next

        comicName = ReadName()

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
        Me.Text = "iBuka@Super169    " & comicName & "  (Vol. " & lvBooks.Items(fileIdx).Text & ")   [page " & (pageIdx + 1) & "/" & pageCnt & "]"
    End Sub


    Private Sub cboViewMode_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboViewMode.SelectedIndexChanged
        viewMode = cboViewMode.SelectedIndex
        If viewMode = DisplayMode.WinFit Then
            WindowsFit()
        End If
        showPage()
    End Sub

    Private Function ReadVol(ByVal fn As String) As Integer
        Dim fs = New FileStream(fn, FileMode.Open, FileAccess.Read)
        Dim data(2) As Byte
        fs.Seek(16, SeekOrigin.Begin)
        fs.Read(data, 0, 2)
        fs.Close()
        Dim vol As Integer
        vol = data(1) * 256 + data(0)
        Return vol
    End Function

    Private Function ReadName() As String
        Dim data(500) As Byte
        fs.Seek(chaporder.startPos, SeekOrigin.Begin)
        fs.Read(data, 0, 500)
        Dim s As String = (New System.Text.ASCIIEncoding()).GetString(data)
        Dim i = s.IndexOf("""name"":")
        Dim j = s.IndexOf(""",", i + 6)
        Dim name = s.Substring(i + 8, j - i - 8)
        Dim idx = 0
        Dim DName As String = ""
        Do While True
            If idx <= name.Length - 6 Then
                DName = DName + ChrW(CInt("&H" & name.Substring(idx + 2, 4)))
                idx = idx + 6
            Else
                Exit Do
            End If
        Loop

        Return DName
    End Function


    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If fs IsNot Nothing Then
            fs.Close()
        End If
        myConfig.winX = Me.Left
        myConfig.winY = Me.Top
        myConfig.winWidth = Me.Width
        myConfig.winHeight = Me.Height
        myConfig.viewMode = viewMode
        myConfig.lastFile = bukaDir(fileIdx)
        myConfig.lastPage = pageIdx
        myConfig.SaveKey()
    End Sub


    Private Sub frmReader_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub


    Private Sub btnAbout_Click(sender As System.Object, e As System.EventArgs) Handles btnAbout.Click
        frmAbout.Show()
    End Sub
End Class