<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JKAppliGlobalHookProcess
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
        Me.TmpTextBoxGH = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'TmpTextBoxGH
        '
        Me.TmpTextBoxGH.Location = New System.Drawing.Point(32, 205)
        Me.TmpTextBoxGH.Name = "TmpTextBoxGH"
        Me.TmpTextBoxGH.Size = New System.Drawing.Size(100, 19)
        Me.TmpTextBoxGH.TabIndex = 0
        '
        'JKAppliGlobalHookProcess
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.TmpTextBoxGH)
        Me.Name = "JKAppliGlobalHookProcess"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TmpTextBoxGH As System.Windows.Forms.TextBox
End Class
