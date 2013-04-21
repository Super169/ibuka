Option Explicit On
Imports System.IO

Public Class frmReader

    Private idxSigature As Byte() = System.Text.Encoding.ASCII.GetBytes("index2.dat")
    Dim buffer(10240) As Byte ' Assume the header must be within 10K
    Dim bufferSize As Integer

    Const rightPanel = 80
    Const initBookSize = 100
    Const extBookSize = 20
    Dim bukaBook(initBookSize) As bukaPage
    Dim bookLogo As bukaPage
    Dim chaporder As bukaPage
    Dim pageCnt As Integer = 0
    Dim pageIdx As Integer = -1

    Dim fixTop As Boolean = False

    Dim fs As FileStream
    Dim viewMode As DisplayMode
    Dim imageLoaded As Boolean = False


    Private Enum DisplayMode
        ActualSize = 0
        FitWidth = 1
        FitHeight = 2
        AutoFit = 3
    End Enum

    Private Sub frmReader_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub


    Private Sub btnGo_Click(sender As System.Object, e As System.EventArgs) Handles btnGo.Click
        Dim fn As String = txtFilename.Text.Trim
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
        readPage(0)

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

    Private Class SortBukaPage
        Implements IComparer

        Public Function Compare( _
         ByVal x As Object, ByVal y As Object) As Integer _
         Implements System.Collections.IComparer.Compare
            Dim p1 As bukaPage = CType(x, bukaPage)
            Dim p2 As bukaPage = CType(y, bukaPage)

            Return p1.name.CompareTo(p2.name)
        End Function
    End Class

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

    Private Sub btnActual_Click(sender As System.Object, e As System.EventArgs) Handles btnActual.Click
        viewMode = DisplayMode.ActualSize
        showPage()
    End Sub

    Private Sub btnAuto_Click(sender As System.Object, e As System.EventArgs) Handles btnAuto.Click
        viewMode = DisplayMode.AutoFit
        showPage()
    End Sub

    Private Sub btnFitWidth_Click(sender As System.Object, e As System.EventArgs) Handles btnFitWidth.Click
        viewMode = DisplayMode.FitWidth
        showPage()
    End Sub


    Private Sub btnFitHeight_Click(sender As System.Object, e As System.EventArgs) Handles btnFitHeight.Click
        viewMode = DisplayMode.FitHeight
        showPage()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If fs IsNot Nothing Then
            fs.Close()
        End If
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
        pbImage.Size = New Size(w, h)
        pbImage.SizeMode = PictureBoxSizeMode.Zoom
        pbImage.Focus()
    End Sub


    Private Sub frmReader_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.PageUp, Keys.Subtract, Keys.Up
                readPage(pageIdx - 1)
            Case Keys.PageDown, Keys.Add, Keys.Down
                readPage(pageIdx + 1)
            Case Keys.Home, Keys.Left
                readPage(0)
            Case Keys.End, Keys.Right
                readPage(pageCnt - 1)
        End Select
        ' disable all keys
        e.Handled = True
    End Sub

    Private Sub Form1_Move(sender As Object, e As System.EventArgs) Handles Me.Move
        If (fixTop And Me.Top <> 0) Then Me.Top = 0
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
        If Not imageLoaded Then Return
        showPage()
    End Sub

    Private Sub btnWindowsFit_Click(sender As System.Object, e As System.EventArgs) Handles btnWindowsFit.Click
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

    Private Sub lvPages_DrawColumnHeader(sender As Object, e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs) Handles lvPages.DrawColumnHeader
        e.DrawDefault = True
    End Sub

    Private Sub lvPages_DrawItem(sender As Object, e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvPages.DrawItem
        Dim clrSelectedText As Color = Color.Black
        Dim clrHighlight As Color = Color.Orange
        If (e.Item.Selected) Then
            e.Graphics.FillRectangle(New SolidBrush(clrHighlight), e.Bounds)
            e.Graphics.DrawString(lvPages.Items.Item(e.ItemIndex).Text, e.Item.Font, New SolidBrush(clrSelectedText), e.Bounds) 'Draw the text for the item
        Else
            e.DrawBackground()
            e.Graphics.DrawString(lvPages.Items.Item(e.ItemIndex).Text, e.Item.Font, Brushes.Black, e.Bounds) 'Draw the item text in its regular(color)
        End If
    End Sub

    Private Sub lvPages_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvPages.KeyDown
        e.Handled = True
    End Sub

    Private Sub lvPages_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles lvPages.KeyPress
        e.Handled = True
    End Sub

    Private Sub lvPages_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvPages.KeyUp
        e.Handled = True
    End Sub

    Private Sub lvPages_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvPages.SelectedIndexChanged
        If lvPages.SelectedIndices.Count() > 0 Then
            readPage(lvPages.SelectedIndices(0))
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        ofd.Filter = "Buka File (*.buka) | *.buka"
        If (ofd.ShowDialog = Windows.Forms.DialogResult.OK) Then
            txtFilename.Text = ofd.FileName
        End If
    End Sub


End Class