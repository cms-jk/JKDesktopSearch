<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OrgWebForm
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OrgWebForm))
        Me.Button1OnBrowser = New System.Windows.Forms.Button
        Me.TmpTextBoxOnBrow = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.使い方ヘルプToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.バージョン情報ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadingProgressBar = New System.Windows.Forms.ProgressBar
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1OnBrowser
        '
        Me.Button1OnBrowser.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1OnBrowser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Button1OnBrowser.Location = New System.Drawing.Point(81, 86)
        Me.Button1OnBrowser.Margin = New System.Windows.Forms.Padding(0)
        Me.Button1OnBrowser.MaximumSize = New System.Drawing.Size(75, 23)
        Me.Button1OnBrowser.MinimumSize = New System.Drawing.Size(75, 23)
        Me.Button1OnBrowser.Name = "Button1OnBrowser"
        Me.Button1OnBrowser.Size = New System.Drawing.Size(75, 23)
        Me.Button1OnBrowser.TabIndex = 3
        Me.Button1OnBrowser.Text = "検索"
        Me.Button1OnBrowser.UseVisualStyleBackColor = True
        Me.Button1OnBrowser.Visible = False
        '
        'TmpTextBoxOnBrow
        '
        Me.TmpTextBoxOnBrow.Font = New System.Drawing.Font("MS UI Gothic", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TmpTextBoxOnBrow.Location = New System.Drawing.Point(81, 158)
        Me.TmpTextBoxOnBrow.Name = "TmpTextBoxOnBrow"
        Me.TmpTextBoxOnBrow.Size = New System.Drawing.Size(160, 24)
        Me.TmpTextBoxOnBrow.TabIndex = 4
        Me.TmpTextBoxOnBrow.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(1, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(366, 80)
        Me.Panel1.TabIndex = 5
        Me.Panel1.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.使い方ヘルプToolStripMenuItem, Me.バージョン情報ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(161, 48)
        '
        '使い方ヘルプToolStripMenuItem
        '
        Me.使い方ヘルプToolStripMenuItem.Name = "使い方ヘルプToolStripMenuItem"
        Me.使い方ヘルプToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.使い方ヘルプToolStripMenuItem.Text = "使い方ヘルプ"
        '
        'バージョン情報ToolStripMenuItem
        '
        Me.バージョン情報ToolStripMenuItem.Name = "バージョン情報ToolStripMenuItem"
        Me.バージョン情報ToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.バージョン情報ToolStripMenuItem.Text = "バージョン情報"
        '
        'LoadingProgressBar
        '
        Me.LoadingProgressBar.Dock = System.Windows.Forms.DockStyle.Top
        Me.LoadingProgressBar.Location = New System.Drawing.Point(0, 0)
        Me.LoadingProgressBar.Name = "LoadingProgressBar"
        Me.LoadingProgressBar.Size = New System.Drawing.Size(284, 10)
        Me.LoadingProgressBar.TabIndex = 6
        '
        'OrgWebForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.LoadingProgressBar)
        Me.Controls.Add(Me.TmpTextBoxOnBrow)
        Me.Controls.Add(Me.Button1OnBrowser)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "OrgWebForm"
        Me.Text = "Form3"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1OnBrowser As System.Windows.Forms.Button
    Public WithEvents TmpTextBoxOnBrow As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 使い方ヘルプToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadingProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents バージョン情報ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
