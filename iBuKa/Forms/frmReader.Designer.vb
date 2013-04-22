<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReader
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReader))
        Me.panImage = New System.Windows.Forms.Panel()
        Me.pbImage = New System.Windows.Forms.PictureBox()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.lvPages = New System.Windows.Forms.ListView()
        Me.ofd = New System.Windows.Forms.OpenFileDialog()
        Me.lvBooks = New System.Windows.Forms.ListView()
        Me.cboViewMode = New System.Windows.Forms.ComboBox()
        Me.btnAbout = New System.Windows.Forms.Button()
        Me.panImage.SuspendLayout()
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panImage
        '
        Me.panImage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panImage.AutoScroll = True
        Me.panImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panImage.Controls.Add(Me.pbImage)
        Me.panImage.Location = New System.Drawing.Point(0, 0)
        Me.panImage.Margin = New System.Windows.Forms.Padding(0)
        Me.panImage.Name = "panImage"
        Me.panImage.Size = New System.Drawing.Size(723, 520)
        Me.panImage.TabIndex = 4
        '
        'pbImage
        '
        Me.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbImage.Location = New System.Drawing.Point(0, 0)
        Me.pbImage.Name = "pbImage"
        Me.pbImage.Size = New System.Drawing.Size(719, 516)
        Me.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbImage.TabIndex = 0
        Me.pbImage.TabStop = False
        '
        'btnOpen
        '
        Me.btnOpen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpen.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnOpen.BackgroundImage = CType(resources.GetObject("btnOpen.BackgroundImage"), System.Drawing.Image)
        Me.btnOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnOpen.Location = New System.Drawing.Point(726, 43)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(56, 36)
        Me.btnOpen.TabIndex = 19
        Me.btnOpen.UseVisualStyleBackColor = False
        '
        'lvPages
        '
        Me.lvPages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvPages.FullRowSelect = True
        Me.lvPages.Location = New System.Drawing.Point(726, 250)
        Me.lvPages.MultiSelect = False
        Me.lvPages.Name = "lvPages"
        Me.lvPages.OwnerDraw = True
        Me.lvPages.Size = New System.Drawing.Size(56, 267)
        Me.lvPages.TabIndex = 15
        Me.lvPages.UseCompatibleStateImageBehavior = False
        Me.lvPages.View = System.Windows.Forms.View.Details
        '
        'ofd
        '
        Me.ofd.FileName = "OpenFileDialog1"
        '
        'lvBooks
        '
        Me.lvBooks.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvBooks.FullRowSelect = True
        Me.lvBooks.Location = New System.Drawing.Point(726, 112)
        Me.lvBooks.MultiSelect = False
        Me.lvBooks.Name = "lvBooks"
        Me.lvBooks.OwnerDraw = True
        Me.lvBooks.Size = New System.Drawing.Size(56, 132)
        Me.lvBooks.TabIndex = 20
        Me.lvBooks.UseCompatibleStateImageBehavior = False
        Me.lvBooks.View = System.Windows.Forms.View.Details
        '
        'cboViewMode
        '
        Me.cboViewMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboViewMode.FormattingEnabled = True
        Me.cboViewMode.Items.AddRange(New Object() {"Actual", "AutoFit", "Width", "Height", "WinFit"})
        Me.cboViewMode.Location = New System.Drawing.Point(726, 85)
        Me.cboViewMode.Name = "cboViewMode"
        Me.cboViewMode.Size = New System.Drawing.Size(56, 21)
        Me.cboViewMode.TabIndex = 1
        '
        'btnAbout
        '
        Me.btnAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbout.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnAbout.BackgroundImage = CType(resources.GetObject("btnAbout.BackgroundImage"), System.Drawing.Image)
        Me.btnAbout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnAbout.Location = New System.Drawing.Point(726, 1)
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(56, 36)
        Me.btnAbout.TabIndex = 21
        Me.btnAbout.UseVisualStyleBackColor = False
        '
        'frmReader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(784, 520)
        Me.Controls.Add(Me.btnAbout)
        Me.Controls.Add(Me.cboViewMode)
        Me.Controls.Add(Me.lvBooks)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.panImage)
        Me.Controls.Add(Me.lvPages)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmReader"
        Me.Text = "iBuKa Reader"
        Me.panImage.ResumeLayout(False)
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents panImage As System.Windows.Forms.Panel
    Friend WithEvents pbImage As System.Windows.Forms.PictureBox
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents lvPages As System.Windows.Forms.ListView
    Friend WithEvents ofd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lvBooks As System.Windows.Forms.ListView
    Friend WithEvents cboViewMode As System.Windows.Forms.ComboBox
    Friend WithEvents btnAbout As System.Windows.Forms.Button
End Class
