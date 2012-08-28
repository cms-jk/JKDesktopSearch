'**************
'"JKDeskTopSearch" Powered by Bukkyo University Library
'本プログラムについてJapanKnowledge会員、および佛教大学図書館が許可する業者以外への公開を禁じます。
'**************

Imports System.Windows.Forms
Public Class OrgWebForm
    Inherits System.Windows.Forms.Form

#Region "ブラウザの機能拡張のための変数定義等"
    '
    'イベントで使用するためWithEventsにて定義
    Public WithEvents WebBrowser1 As ExWebBrowser

    'WebBrowserでマウスイベント取得のための定義
    Private WithEvents WebBrowserMouseClick As BrowserMouseClick

    'ブラウザをタブブラウザ化させるための定義
    Private TabControl1 As New TabControl
    Private TabPage1 As TabPage

    'ボタン２度押し防止のための定義
    Private _IsEventProcessing As Boolean
#End Region

#Region "フォームのロードとクローズ"


    Private Sub OrgWebForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '位置の記録
        'フォームデザインの記録
        Regkey.SetValue("Design", CStr(FormDesign))

        '位置の記録
        Regkey.SetValue("LeftPosition", CStr(JKDeskTopSearch.Left))
        Regkey.SetValue("TopPosition", CStr(JKDeskTopSearch.Top))

        '検索ボタンを隠す（ブラウザのボタンへのスイッチ）
        JKDeskTopSearch.Hide()

        '一番上に表示
        Me.TopMost = True

        '表示位置
        Me.Top = 0
        Me.Left = DisplayWidth - OrgWebFormWidth

        '大きさ
        Me.Width = OrgWebFormWidth
        Me.Height = DisplayHeight

        '垂直方向にのみサイズ変更を可能（横は現在の値）
        Me.MaximumSize = New System.Drawing.Size(Me.Size.Width, Short.MaxValue)
        Me.MinimumSize = New System.Drawing.Size(Me.Size.Width, 0)
        Me.MaximizeBox = False

        'WebBrowser1
        Me.WebBrowser1 = New ExWebBrowser
        Me.WebBrowser1.Dock = DockStyle.Fill
        Me.WebBrowser1.ScriptErrorsSuppressed = True
        AddHandler WebBrowser1.NewWindow2, AddressOf WebBrowser_NewWindow2

        'タブを介さずに直接フォーム上にブラウザを追加
        Me.Controls.Add(WebBrowser1)

        'Button1OnBrowserのプロパティ指定
        Me.Button1OnBrowser.Location = New System.Drawing.Point(OrgWebFormWidth - MainFormRight - JKDeskTopSearch.Width + JKDeskTopSearch.Button1.Location.X + 15, MainFormTop + JKDeskTopSearch.Button1.Location.Y - 20)
        Dim ButtonColor As Color = ColorTranslator.FromOle(RGB(247, 141, 29))
        Me.Button1OnBrowser.BackColor = ButtonColor
        Me.Button1OnBrowser.ForeColor = Color.White

        'TmpTextBoxOnBrowの大きさ位置
        Me.TmpTextBoxOnBrow.Width = 170
        Me.TmpTextBoxOnBrow.Location = New System.Drawing.Point(OrgWebFormWidth - MainFormRight - JKDeskTopSearch.Width + JKDeskTopSearch.Button1.Location.X - Me.TmpTextBoxOnBrow.Width, MainFormTop + JKDeskTopSearch.Button1.Location.Y + Button1OnBrowser.Height - TmpTextBoxOnBrow.Height - 20)

        'panel1の大きさ位置
        Me.Panel1.Width = 297
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Height = 60

        'Panel1の色指定
        Dim OrgBackColor As Color = ColorTranslator.FromOle(RGB(85, 85, 85))
        Me.Panel1.BackColor = OrgBackColor

        'フォームの見出し
        Me.Text = "JapanKnowlede検索一覧"

        'マウスイベント取得のための宣言
        Me.WebBrowserMouseClick = New BrowserMouseClick(Me.WebBrowser1)
        Me.WebBrowser1.IsWebBrowserContextMenuEnabled = False
    End Sub

    Private Sub OrgWebForm_Closed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        'メインフォームを再表示
        JKDeskTopSearch.Show()

        'メインフォームの位置をもとに戻す（移動前）
        JKDeskTopSearch.Top = CInt(Regkey.GetValue("TopPosition", "0"))
        JKDeskTopSearch.Left = CInt(Regkey.GetValue("LeftPosition", "0"))

        'とりあえずメインフォームをトップに
        JKDeskTopSearch.TopMost = True
        JKDeskTopSearch.Activate()
        JKDeskTopSearch.Select()
        JKDeskTopSearch.TmpTextBox.Focus()

    End Sub

#End Region

#Region "ブラウザでの動作"
    '*******別ウインドウURLに飛ぶものを中止させフックするルーチン
    Private Sub WebBrowser_NewWindow2(ByVal sender As Object, ByVal e As WebBrowserNewWindow2EventArgs)

        'ディスプレイの作業領域の高さ
        Dim DisplayHeight As Short = System.Windows.Forms.Screen.GetWorkingArea(Me).Height

        'ディスプレイの作業領域の幅
        Dim DisplayWidth As Short = System.Windows.Forms.Screen.GetWorkingArea(Me).Width

        'オリジナルブラウザ「OrgWebForm2」が存在していれば、それを最大化してTopに
        If Application.OpenForms("OrgWebForm2") IsNot Nothing Then

            '大きさを既定サイズに戻して表示
            OrgWebForm2.WindowState = FormWindowState.Normal

            '表示位置
            OrgWebForm2.Top = 0
            OrgWebForm2.Left = DisplayWidth - OrgWebFormWidth - OrgWebForm2Width

            '大きさ
            OrgWebForm2.Width = OrgWebForm2Width
            OrgWebForm2.Height = DisplayHeight

            '一度一番上にする
            OrgWebForm2.TopMost = True

            'Windowの再利用をせずにタブで開く場合----（WebBrowser1の再設定が必要）
            'WebBrowser1の定義
            OrgWebForm2.WebBrowser1 = New ExWebBrowser
            OrgWebForm2.WebBrowser1.ScriptErrorsSuppressed = True
            OrgWebForm2.WebBrowser1.Dock = DockStyle.Fill
            AddHandler OrgWebForm2.WebBrowser1.NewWindow2, AddressOf WebBrowser_NewWindow2

            'TabPage1を生成し、その上にWebBrowser1を追加
            OrgWebForm2.TabPage1 = New TabPage
            OrgWebForm2.TabPage1.Controls.Add(OrgWebForm2.WebBrowser1)

            'TabControl1（つまむ部分）にTabPage1を追加
            OrgWebForm2.TabControl1.Dock = DockStyle.Fill
            OrgWebForm2.TabControl1.TabPages.Add(OrgWebForm2.TabPage1)

            'TabControl1をフォームに追加
            OrgWebForm2.Controls.Add(OrgWebForm2.TabControl1)

            'TabPage1を一番上に表示
            OrgWebForm2.TabControl1.SelectedTab = OrgWebForm2.TabPage1

            'トップに来るのをを解除してメインフォームを一番上
            OrgWebForm2.TopMost = False
            Me.TopMost = True
        Else

            '存在していない場合オリジナルブラウザの２を表示
            OrgWebForm2.Show()
        End If

        '新しいウィンドウの展開を中止しフックする。ブラウザ２へスイッチ
        e.ppDisp = OrgWebForm2.WebBrowser1.Application
        OrgWebForm2.WebBrowser1.RegisterAsBrowser = True
    End Sub

    '******ドキュメントを読み終えた時
    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted

        'ドキュメントを読み終えたら、ボタンと検索窓を表示し、前面に
        Me.Button1OnBrowser.Visible = True
        Me.TmpTextBoxOnBrow.Visible = True
        Me.Panel1.Visible = True

        'ブラウザ本体は後ろへ
        WebBrowser1.SendToBack()
        Me.Button1OnBrowser.BringToFront()
        Me.TmpTextBoxOnBrow.BringToFront()

        '少し待った後でプログレスバーをリセット
        Application.DoEvents()
        Threading.Thread.Sleep(300)
        Me.LoadingProgressBar.Value = 0
    End Sub

    '*****プログレスバー
    Private Sub WebBrowser1_ProgressChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserProgressChangedEventArgs) Handles WebBrowser1.ProgressChanged

        Dim ProgressStep As Long
        ProgressStep = e.CurrentProgress

        '初回例外、および範囲外エラーの回避（このエラーがあるとグローバルフックが停止する）
        If (ProgressStep > 0) And (ProgressStep <= e.MaximumProgress) Then
            Me.LoadingProgressBar.Value = Integer.Parse(System.Math.Floor((ProgressStep / e.MaximumProgress) * (Me.LoadingProgressBar.Maximum - Me.LoadingProgressBar.Minimum) + Me.LoadingProgressBar.Minimum).ToString)
        End If
    End Sub

    '*****WebBrowser上でのマウスイベントを取得（オリジナルクラスを利用）
    Private Sub WebBrowserMouseClick_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles WebBrowserMouseClick.MouseDown
        'WebBrowser上では左・右・中央いずれのボタンに対しても反応
        'マウスによる座標取得(クリックしたボタンまで取得可能)
        '画面上で右クリックをしたときに、ContextMenuStripを立ち上げる
        If String.Format(e.Button.ToString) = "Right" Then

            '右クリックでのメニュー立ち上げ
            WebBrowser1.ContextMenuStrip = ContextMenuStrip1
        End If
    End Sub

    ''*****ボタンをクリックしたときの動作
    Private Sub Button1OnBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1OnBrowser.Click
        Me.TopMost = False
        Me.Button1OnBrowser.Enabled = False
        Call MainModule.Button_Click(Me, TmpTextBoxOnBrow)
        Me.Button1OnBrowser.Enabled = True
        Me.TopMost = True
    End Sub

    '****テキストボックスでリターンキーを打つと検索開始
    Private Sub TmpTextBoxOnBrow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TmpTextBoxOnBrow.KeyPress
        Me.TopMost = False
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then

            'TextValueの値はTmpTextBoxの値で設定
            TextValue = Me.TmpTextBoxOnBrow.Text

            '空でなければ実際の検索
            If TextValue <> "" Then

                'ベースURLを設定する
                Call BaseURL_Setting(Me)
                Call MainModule.Search_Execute(Me, TmpTextBoxOnBrow)

            End If
        End If

        Me.TopMost = True
    End Sub

#End Region

#Region "ToolStripMenuItem"
    Private Sub 使い方ヘルプToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 使い方ヘルプToolStripMenuItem.Click
        WebBrowser1.Navigate(Regkey.GetValue("proxyUrl"))
    End Sub

    Private Sub バージョン情報ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles バージョン情報ToolStripMenuItem.Click
        VerInfoDialog.ShowDialog()
    End Sub
#End Region

End Class

