<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OrgWebForm2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OrgWebForm2))
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.タブを閉じるToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.戻るToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.進むToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.タブを閉じるToolStripMenuItem, Me.戻るToolStripMenuItem, Me.進むToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(173, 92)
        '
        'タブを閉じるToolStripMenuItem
        '
        Me.タブを閉じるToolStripMenuItem.Name = "タブを閉じるToolStripMenuItem"
        Me.タブを閉じるToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.タブを閉じるToolStripMenuItem.Text = "タブを閉じる"
        '
        '戻るToolStripMenuItem
        '
        Me.戻るToolStripMenuItem.Enabled = False
        Me.戻るToolStripMenuItem.Name = "戻るToolStripMenuItem"
        Me.戻るToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.戻るToolStripMenuItem.Text = "前のページへ戻る"
        '
        '進むToolStripMenuItem
        '
        Me.進むToolStripMenuItem.Enabled = False
        Me.進むToolStripMenuItem.Name = "進むToolStripMenuItem"
        Me.進むToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.進むToolStripMenuItem.Text = "次のページへ進む"
        '
        'OrgWebForm2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "OrgWebForm2"
        Me.Text = "JapanKnowledge"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents タブを閉じるToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 戻るToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 進むToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
