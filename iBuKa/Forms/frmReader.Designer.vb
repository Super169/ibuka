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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnAuto = New System.Windows.Forms.Button()
        Me.btnActual = New System.Windows.Forms.Button()
        Me.btnWindowsFit = New System.Windows.Forms.Button()
        Me.lvPages = New System.Windows.Forms.ListView()
        Me.btnFitHeight = New System.Windows.Forms.Button()
        Me.btnFitWidth = New System.Windows.Forms.Button()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.txtFilename = New System.Windows.Forms.TextBox()
        Me.ofd = New System.Windows.Forms.OpenFileDialog()
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
        Me.panImage.Size = New System.Drawing.Size(923, 731)
        Me.panImage.TabIndex = 4
        '
        'pbImage
        '
        Me.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbImage.Location = New System.Drawing.Point(0, 0)
        Me.pbImage.Name = "pbImage"
        Me.pbImage.Size = New System.Drawing.Size(918, 719)
        Me.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbImage.TabIndex = 0
        Me.pbImage.TabStop = False
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(961, 1)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(21, 23)
        Me.Button1.TabIndex = 19
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnAuto
        '
        Me.btnAuto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAuto.Location = New System.Drawing.Point(926, 610)
        Me.btnAuto.Name = "btnAuto"
        Me.btnAuto.Size = New System.Drawing.Size(56, 23)
        Me.btnAuto.TabIndex = 18
        Me.btnAuto.Text = "AutoFit"
        Me.btnAuto.UseVisualStyleBackColor = True
        '
        'btnActual
        '
        Me.btnActual.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnActual.Location = New System.Drawing.Point(926, 581)
        Me.btnActual.Name = "btnActual"
        Me.btnActual.Size = New System.Drawing.Size(56, 23)
        Me.btnActual.TabIndex = 17
        Me.btnActual.Text = "Actual"
        Me.btnActual.UseVisualStyleBackColor = True
        '
        'btnWindowsFit
        '
        Me.btnWindowsFit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWindowsFit.Location = New System.Drawing.Point(926, 697)
        Me.btnWindowsFit.Name = "btnWindowsFit"
        Me.btnWindowsFit.Size = New System.Drawing.Size(56, 23)
        Me.btnWindowsFit.TabIndex = 16
        Me.btnWindowsFit.Text = "Win Fit"
        Me.btnWindowsFit.UseVisualStyleBackColor = True
        '
        'lvPages
        '
        Me.lvPages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvPages.FullRowSelect = True
        Me.lvPages.Location = New System.Drawing.Point(926, 27)
        Me.lvPages.MultiSelect = False
        Me.lvPages.Name = "lvPages"
        Me.lvPages.OwnerDraw = True
        Me.lvPages.Size = New System.Drawing.Size(56, 519)
        Me.lvPages.TabIndex = 15
        Me.lvPages.UseCompatibleStateImageBehavior = False
        Me.lvPages.View = System.Windows.Forms.View.Details
        '
        'btnFitHeight
        '
        Me.btnFitHeight.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFitHeight.Location = New System.Drawing.Point(926, 668)
        Me.btnFitHeight.Name = "btnFitHeight"
        Me.btnFitHeight.Size = New System.Drawing.Size(56, 23)
        Me.btnFitHeight.TabIndex = 14
        Me.btnFitHeight.Text = "Height"
        Me.btnFitHeight.UseVisualStyleBackColor = True
        '
        'btnFitWidth
        '
        Me.btnFitWidth.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFitWidth.Location = New System.Drawing.Point(926, 639)
        Me.btnFitWidth.Name = "btnFitWidth"
        Me.btnFitWidth.Size = New System.Drawing.Size(56, 23)
        Me.btnFitWidth.TabIndex = 13
        Me.btnFitWidth.Text = " Width"
        Me.btnFitWidth.UseVisualStyleBackColor = True
        '
        'btnGo
        '
        Me.btnGo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGo.Location = New System.Drawing.Point(926, 552)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(56, 23)
        Me.btnGo.TabIndex = 12
        Me.btnGo.Text = "Read"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'txtFilename
        '
        Me.txtFilename.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilename.Location = New System.Drawing.Point(927, 4)
        Me.txtFilename.Name = "txtFilename"
        Me.txtFilename.Size = New System.Drawing.Size(28, 20)
        Me.txtFilename.TabIndex = 11
        Me.txtFilename.Text = "e:\buka\Working\65538.buka"
        '
        'ofd
        '
        Me.ofd.FileName = "OpenFileDialog1"
        '
        'frmReader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 731)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.panImage)
        Me.Controls.Add(Me.btnAuto)
        Me.Controls.Add(Me.txtFilename)
        Me.Controls.Add(Me.btnActual)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.btnWindowsFit)
        Me.Controls.Add(Me.btnFitWidth)
        Me.Controls.Add(Me.lvPages)
        Me.Controls.Add(Me.btnFitHeight)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmReader"
        Me.Text = "BuKa Reader"
        Me.panImage.ResumeLayout(False)
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents panImage As System.Windows.Forms.Panel
    Friend WithEvents pbImage As System.Windows.Forms.PictureBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnAuto As System.Windows.Forms.Button
    Friend WithEvents btnActual As System.Windows.Forms.Button
    Friend WithEvents btnWindowsFit As System.Windows.Forms.Button
    Friend WithEvents lvPages As System.Windows.Forms.ListView
    Friend WithEvents btnFitHeight As System.Windows.Forms.Button
    Friend WithEvents btnFitWidth As System.Windows.Forms.Button
    Friend WithEvents btnGo As System.Windows.Forms.Button
    Friend WithEvents txtFilename As System.Windows.Forms.TextBox
    Friend WithEvents ofd As System.Windows.Forms.OpenFileDialog
End Class
