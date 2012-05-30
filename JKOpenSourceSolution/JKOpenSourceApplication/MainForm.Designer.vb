<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JKDeskTopSearch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JKDeskTopSearch))
        Me.Button1 = New System.Windows.Forms.Button
        Me.TmpTextBox = New System.Windows.Forms.TextBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.検索ボタンを表示ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.終了ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.JKウェブToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.スタイルの変更ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ClassicToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AeroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Classic検索窓ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Aero検索窓ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.タスクバーに格納ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.使いかたヘルプToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.バージョン情報ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(20, 9)
        Me.Button1.Margin = New System.Windows.Forms.Padding(0)
        Me.Button1.MaximumSize = New System.Drawing.Size(122, 50)
        Me.Button1.MinimumSize = New System.Drawing.Size(122, 50)
        Me.Button1.Name = "Button1"
        Me.Button1.Padding = New System.Windows.Forms.Padding(2, 1, 2, 1)
        Me.Button1.Size = New System.Drawing.Size(122, 50)
        Me.Button1.TabIndex = 1
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TmpTextBox
        '
        Me.TmpTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.TmpTextBox.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TmpTextBox.Location = New System.Drawing.Point(-3, 88)
        Me.TmpTextBox.Name = "TmpTextBox"
        Me.TmpTextBox.Size = New System.Drawing.Size(155, 22)
        Me.TmpTextBox.TabIndex = 2
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "ジャパンナレッジ検索"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.検索ボタンを表示ToolStripMenuItem, Me.終了ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(173, 48)
        '
        '検索ボタンを表示ToolStripMenuItem
        '
        Me.検索ボタンを表示ToolStripMenuItem.Name = "検索ボタンを表示ToolStripMenuItem"
        Me.検索ボタンを表示ToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.検索ボタンを表示ToolStripMenuItem.Text = "検索ボタンを表示"
        '
        '終了ToolStripMenuItem
        '
        Me.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem"
        Me.終了ToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.終了ToolStripMenuItem.Text = "終了"
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 1000
        Me.ToolTip1.AutoPopDelay = 3000
        Me.ToolTip1.InitialDelay = 1000
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ReshowDelay = 200
        Me.ToolTip1.ToolTipTitle = "「いつでもジャパンナレッジ」使用法"
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.JKウェブToolStripMenuItem, Me.スタイルの変更ToolStripMenuItem, Me.タスクバーに格納ToolStripMenuItem, Me.使いかたヘルプToolStripMenuItem, Me.バージョン情報ToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(257, 114)
        '
        'JKウェブToolStripMenuItem
        '
        Me.JKウェブToolStripMenuItem.Name = "JKウェブToolStripMenuItem"
        Me.JKウェブToolStripMenuItem.Size = New System.Drawing.Size(256, 22)
        Me.JKウェブToolStripMenuItem.Text = "JapanKnowledge（本家サイト）"
        '
        'スタイルの変更ToolStripMenuItem
        '
        Me.スタイルの変更ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClassicToolStripMenuItem, Me.AeroToolStripMenuItem, Me.Classic検索窓ToolStripMenuItem, Me.Aero検索窓ToolStripMenuItem})
        Me.スタイルの変更ToolStripMenuItem.Name = "スタイルの変更ToolStripMenuItem"
        Me.スタイルの変更ToolStripMenuItem.Size = New System.Drawing.Size(256, 22)
        Me.スタイルの変更ToolStripMenuItem.Text = "スタイルの変更"
        '
        'ClassicToolStripMenuItem
        '
        Me.ClassicToolStripMenuItem.Checked = True
        Me.ClassicToolStripMenuItem.CheckOnClick = True
        Me.ClassicToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ClassicToolStripMenuItem.Name = "ClassicToolStripMenuItem"
        Me.ClassicToolStripMenuItem.Size = New System.Drawing.Size(291, 22)
        Me.ClassicToolStripMenuItem.Text = "Classic"
        '
        'AeroToolStripMenuItem
        '
        Me.AeroToolStripMenuItem.CheckOnClick = True
        Me.AeroToolStripMenuItem.Name = "AeroToolStripMenuItem"
        Me.AeroToolStripMenuItem.Size = New System.Drawing.Size(291, 22)
        Me.AeroToolStripMenuItem.Text = "Aero(Aero Theme使用時有効)"
        '
        'Classic検索窓ToolStripMenuItem
        '
        Me.Classic検索窓ToolStripMenuItem.Name = "Classic検索窓ToolStripMenuItem"
        Me.Classic検索窓ToolStripMenuItem.Size = New System.Drawing.Size(291, 22)
        Me.Classic検索窓ToolStripMenuItem.Text = "Classic+検索窓"
        '
        'Aero検索窓ToolStripMenuItem
        '
        Me.Aero検索窓ToolStripMenuItem.Name = "Aero検索窓ToolStripMenuItem"
        Me.Aero検索窓ToolStripMenuItem.Size = New System.Drawing.Size(291, 22)
        Me.Aero検索窓ToolStripMenuItem.Text = "Aero+検索窓(Aero Theme使用時有効)"
        '
        'タスクバーに格納ToolStripMenuItem
        '
        Me.タスクバーに格納ToolStripMenuItem.Name = "タスクバーに格納ToolStripMenuItem"
        Me.タスクバーに格納ToolStripMenuItem.Size = New System.Drawing.Size(256, 22)
        Me.タスクバーに格納ToolStripMenuItem.Text = "タスクバーに格納"
        '
        '使いかたヘルプToolStripMenuItem
        '
        Me.使いかたヘルプToolStripMenuItem.Name = "使いかたヘルプToolStripMenuItem"
        Me.使いかたヘルプToolStripMenuItem.Size = New System.Drawing.Size(256, 22)
        Me.使いかたヘルプToolStripMenuItem.Text = "使いかたヘルプ"
        '
        'バージョン情報ToolStripMenuItem
        '
        Me.バージョン情報ToolStripMenuItem.Name = "バージョン情報ToolStripMenuItem"
        Me.バージョン情報ToolStripMenuItem.Size = New System.Drawing.Size(256, 22)
        Me.バージョン情報ToolStripMenuItem.Text = "バージョン情報"
        '
        'Timer1
        '
        '
        'JKDeskTopSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ClientSize = New System.Drawing.Size(154, 113)
        Me.ContextMenuStrip = Me.ContextMenuStrip2
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TmpTextBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(170, 150)
        Me.MinimumSize = New System.Drawing.Size(170, 150)
        Me.Name = "JKDeskTopSearch"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.TopMost = True
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 検索ボタンを表示ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 終了ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents JKウェブToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents タスクバーに格納ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents スタイルの変更ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClassicToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AeroToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Classic検索窓ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Aero検索窓ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents TmpTextBox As System.Windows.Forms.TextBox
    Friend WithEvents 使いかたヘルプToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents バージョン情報ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
