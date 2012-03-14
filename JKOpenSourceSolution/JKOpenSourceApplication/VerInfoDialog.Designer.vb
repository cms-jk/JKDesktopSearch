<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VerInfoDialog
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VerInfoDialog))
        Me.label4 = New System.Windows.Forms.Label
        Me.button1 = New System.Windows.Forms.Button
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.pictureBox1 = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'label4
        '
        Me.label4.Location = New System.Drawing.Point(72, 123)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(262, 39)
        Me.label4.TabIndex = 11
        Me.label4.Text = "label4"
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(231, 165)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(75, 23)
        Me.button1.TabIndex = 10
        Me.button1.Text = "OK"
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(72, 80)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(262, 43)
        Me.label3.TabIndex = 9
        Me.label3.Text = "label3"
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(72, 56)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(262, 23)
        Me.label2.TabIndex = 8
        Me.label2.Text = "label2"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(72, 32)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(262, 23)
        Me.label1.TabIndex = 7
        Me.label1.Text = "label1"
        '
        'pictureBox1
        '
        Me.pictureBox1.Location = New System.Drawing.Point(24, 32)
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.pictureBox1.TabIndex = 6
        Me.pictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.InitialImage = Nothing
        Me.PictureBox2.Location = New System.Drawing.Point(24, 80)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox2.TabIndex = 12
        Me.PictureBox2.TabStop = False
        '
        'VerInfoDialog
        '
        Me.AcceptButton = Me.button1
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.ClientSize = New System.Drawing.Size(341, 200)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.button1)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.pictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "VerInfoDialog"
        Me.ShowInTaskbar = False
        Me.Text = "バージョン情報"
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox

End Class
