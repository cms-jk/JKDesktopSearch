'**************************************
'"JKDeskTopSearch" Powered by Bukkyo University Library
'本プログラムについてJapanKnowledge会員、および佛教大学図書館が許可する業者以外への公開を禁じます。
'***************************************

Imports System.Windows.Forms
Imports System.Deployment.Application
Imports System.Diagnostics
Imports System.Diagnostics.Process

#Region "プロセス間通信用のインポート"

Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
'参照タブでSystem.Runtime.Remotingを追加すること
Imports System.Runtime.Remoting.Channels.Ipc
'オブジェクトのリース期間を無期限に設定する必要あり
Imports System.Runtime.Remoting.Lifetime
'プロジェクトの共有クラスを追加すること
Imports JKWin32HookAndCom
'共有クラスからWin32API関数・定数のインポート
Imports JKWin32HookAndCom.Win32API
Imports JKWin32HookAndCom.Win32API.WM
Imports JKWin32HookAndCom.Win32API.HT
Imports JKWin32HookAndCom.Win32API.DWM

#End Region

Public Class JKDeskTopSearch
    Inherits System.Windows.Forms.Form

#Region "Win32API関係"


    '*****Win32APIの利用定義
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

        '①タイトルバーの上でも右クリックでのメニュー表示を有効にする
        If m.Msg = WM_NCRBUTTONDOWN And m.WParam.ToInt32() = HTCAPTION Then
            Me.ContextMenuStrip2.Show(Me, Me.PointToClient(Cursor.Position))
            Return
        End If

        '②ウインドウ境界部でカーソルの矢印を普通に（拡大・縮小のカーソルを表示させない）
        If m.Msg = WM_NCMOUSEMOVE Or m.Msg = WM_NCMOUSEHOVER Then
            If HTLEFT <= m.WParam.ToInt32() <= HTBOTTOMRIGHT Then Cursor.Current = Cursors.Default
            Return
        End If

        '③スタイルがAeroのときは「上部を含む」ウインドウ境界部をクリックした時もカーソル矢印を普通に
        If FormDesign = 1 And m.Msg = WM_NCLBUTTONDOWN Then
            If HTLEFT <= m.WParam.ToInt32() <= HTBOTTOMRIGHT Then Cursor.Current = Cursors.Default
            Return
        End If

        MyBase.WndProc(m)
    End Sub

    'タイトルバーを表示させる場合
    '「閉じるボタン」を使えなくする
    Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
        Get
            Const CS_NOCLOSE As Short = &H200
            Dim createParams1 As System.Windows.Forms.CreateParams = MyBase.CreateParams
            createParams1.ClassStyle = createParams1.ClassStyle Or CS_NOCLOSE

            Return createParams1
        End Get
    End Property

#End Region

#Region "マルチプロセス&プロセス間通信設定"
    'グローバルフックに関して別プロセスで起動させる
    '本来は単一プロセスで可能なはずだが、以下の要件によりマルチプロセスが望ましい
    '
    '①Windows7より「LowLevelHooksTimeout」パラメータにより、その閾値を超えてタイムアウトしたフックが、
    '10回以上記録された場合、そのプロセスからのグローバルフックの起動ができなくなる。
    'このため安定した運用のためには、1度検索を行ったらプロセス自体を再起動するような処理が望ましい。
    '
    '②ハンドルウインドウが、自身のプロセス（クラス）からの派生であった場合、クリップボードからの
    'テキストボックスへの転送フォーカスが効かない。このため同一プロセス上の場合には、
    'OrgWebFormとOrgWebForm2からのGlobalHookを利用した値の取得・検索ができない

    '******フックプロセス起動用パスの取得
    Public Shared Function GetAppPath() As String
        Return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
    End Function

    '******IPCプロトコルによるプロセス間通信
    '（※）Httpを使う場合にはIPCをHttpにセットしなおす
    'ただしHttpは他の端末との通信が可能になるため、セキュリティ的に危険なので、
    'ウイルスチェッカーやファイヤーウォールでも引っかかる可能性大。

    '①受信用定義
    Protected IPC_MainSideChannel As IpcChannel
    Public WithEvents SendToMain As New JKWin32HookAndCom.HookClientToMainServerEvent

    '②受信待機
    Private Sub MainSideReciever()

        'IPC受信用チャネルを用意(ポートは8181を利用)
        Try
            IPC_MainSideChannel = New IpcChannel(8181)
        Catch re As RemotingException
            MsgBox("別ユーザで当プログラムが起動しています。この状態では検索窓に直接単語を入力しての検索のみ行えます。", MsgBoxStyle.OkOnly)
            Exit Sub
        End Try

        ChannelServices.RegisterChannel(IPC_MainSideChannel, False)

        'SendToMainをSendToMain_URIで参照できるように設定
        Dim ref As ObjRef = RemotingServices.Marshal(SendToMain, "SendToMain_URI")

        '受信準備
        IPC_MainSideChannel.StartListening(Nothing)

    End Sub

    '③受信後の処理（１）
    'クライアントから受信したメッセージを処理
    'クロススレッド（マルチスレッド）処理のため、Delegateを利用
    Delegate Sub SetTmpTextBoxDelegate(ByVal Value As String)
    Private TmpTextBoxDelegate As New SetTmpTextBoxDelegate(AddressOf RecieveText)

    '④受信後の処理（２）
    'メッセージ受信によるイベントの発生とDelegateの処理
    Private Sub MainSide_RaiseClientEvent(ByVal Message As String) Handles SendToMain.RaiseClientEvent
        Invoke(TmpTextBoxDelegate, New Object() {Message})
    End Sub

    '⑤受信後の処理（３）
    Public HookProcessID As Integer

    Private Sub RecieveText(ByVal Message As String)

        If message.StartsWith("ProcessID:anThAjkJG23bsjhiQPj:") Then

            'プロセスIDの場合内部的に保持し、TmpTextBoxには送らない
            HookProcessID = Replace(Message, "ProcessID:anThAjkJG23bsjhiQPj:", "")
        Else

            '検索語の場合

            If TmpTextBox.Text <> "" Or TmpTextBox.Text <> Message Then
                TmpTextBox.Text = Message
                Call MainModule.Button_Click(Me, TmpTextBox)
            End If
        End If
    End Sub


    '*********電源状態変化時の通信用サーバ再起動
    Private Sub SystemEvents_PowerModeChanged(ByVal sender As Object, ByVal e As Microsoft.Win32.PowerModeChangedEventArgs)

        'スリープもしくは休止モードから復活した場合
        If e.Mode = Microsoft.Win32.PowerModes.Resume Then

            '通信用オブジェクトのリース期限を無期限にする
            Lifetime.LifetimeServices.LeaseTime = TimeSpan.Zero

            Call MainSideReciever()

        End If
    End Sub


#End Region

#Region "フォームのロードとクローズ"

    'OSバージョン
    Private OSVer As String
    Private MajorVer As Char

    '*****メインフォームのロード
    Private Sub JKDeskTopSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '電源監視
        'スリープモードから復帰した時に通信を再開させるため
        AddHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf SystemEvents_PowerModeChanged

        '----プロセス間通信用サーバー起動
        '通信用オブジェクトのリース期限を無期限にする
        Lifetime.LifetimeServices.LeaseTime = TimeSpan.Zero

        '受信準備
        Call MainSideReciever()

        '時間調整
        Application.DoEvents()
        Threading.Thread.Sleep(200)

        'フックプロセスの起動（再起動を挟むと再利用できないのでDimで定義）
        Dim StartingProcess As New Process

        '起動するファイルを指定する
        StartingProcess.StartInfo.FileName = GetAppPath() & "\JKAppliGlobalHookProcess.exe"

        '起動する
        StartingProcess.Start()
        '--------

        '時間調整
        'Application.DoEvents()
        'Threading.Thread.Sleep(200)

        '本ソフトウェアがClickOnceでインストールされている場合
        'スタートアップ登録の有無を確認し、ない場合は登録
        'パブリックデスクトップのショートカットの有無も確認
        If ApplicationDeployment.IsNetworkDeployed Then
            Call CheckForShortcut()
        End If


        'XPのときはAero非設定
        'デザイン選択画面は非表示
        AeroToolStripMenuItem.Visible = False
        Aero検索窓ToolStripMenuItem.Visible = False

        'OSの種類取得
        OSVer = My.Computer.Info.OSVersion

        '１文字目がメジャーバージョン
        MajorVer = OSVer.Chars(0)

        'MajorVerが6以上でAeroOK。ToolStripMenuItemメニュー上に表示
        If Val(MajorVer) >= 6 Then
            AeroToolStripMenuItem.Visible = True
            Aero検索窓ToolStripMenuItem.Visible = True
        End If

        'デフォルトのデザインをレジストリから取得
        FormDesign = CInt(Regkey.GetValue("Design", "0"))
        If FormDesign = 0 Then

            'Vista以降か否かを判断
            'XPより前ではDwmIsCompositionEnabled関数は呼び出せない
            If Val(MajorVer) >= 6 Then

                'Vista以降でアエロ環境か否か
                If DwmIsCompositionEnabled() Then
                    Call ChangeToAero()

                    '画面リフレッシュ用タイマー起動（Aeroの背景の黒抜き防止）
                    Me.Timer1.Interval = 1000
                    Me.Timer1.Start()
                Else

                    '画面リフレッシュ用タイマーの停止
                    Me.Timer1.Stop()
                    Call ChangeToClassic()
                End If

                'XP以前の場合
            Else
                '画面リフレッシュ用タイマーの停止
                Me.Timer1.Stop()
                Call ChangeToClassic()
            End If

        ElseIf FormDesign = 1 Then
            Call ChangeToAero()

            '画面リフレッシュ用タイマー起動（Aeroの背景の黒抜き防止）
            Me.Timer1.Interval = 1000
            Me.Timer1.Start()

        ElseIf FormDesign = 5 Then
            Call ChangeToClassicWithTextBox()

            '画面リフレッシュ用タイマーの停止
            Me.Timer1.Stop()

        ElseIf FormDesign = 6 Then
            Call ChangeToAeroWithTextBox()

            '画面リフレッシュ用タイマー起動（Aeroの背景の黒抜き防止）
            Me.Timer1.Interval = 1000
            Me.Timer1.Start()
        End If

        '終了位置を読み込み
        Me.Top = CInt(Regkey.GetValue("TopPosition", "0"))
        Me.Left = CInt(Regkey.GetValue("LeftPosition", "0"))

        'スクリーン外なら補正
        Dim rect As Rectangle = Screen.PrimaryScreen.WorkingArea
        If Me.Top + Me.Height > rect.Height Then
            Me.Top = rect.Height - Me.Height
        End If
        If Me.Left + Me.Width > rect.Width Then
            Me.Left = rect.Width - Me.Width
        End If

        'アクティブウインドウにしておく
        Me.Activate()

    End Sub

    Private MakeMeClassic As Integer
    '******Timerのインタバール経過後の処理
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        'Aeroクラスの利用ができるか否か
        'まずVista以降である
        If Val(MajorVer) >= 6 Then

            If DwmIsCompositionEnabled() Then
                Me.Refresh()

                If MakeMeClassic = 1 Then
                    Call ChangeToAero()
                    MakeMeClassic = 0
                    Exit Sub

                ElseIf MakeMeClassic = 6 Then
                    Call ChangeToAeroWithTextBox()
                    MakeMeClassic = 0
                    Exit Sub
                End If
            Else
                If FormDesign = 1 Then
                    'クラッシク画面へ
                    Call ChangeToClassic()

                    'タイマーでのクラッシク画面への変更なので、変更が終わった段階で、もしMsdnMag.DwmApi.DwmIsCompositionEnabledがTrueなのであれば、
                    'もう一度Aeroモードにもどす
                    MakeMeClassic = 1
                    Exit Sub

                ElseIf FormDesign = 6 Then
                    Call ChangeToClassicWithTextBox()
                    MakeMeClassic = 6
                    Exit Sub
                End If
            End If
        End If
    End Sub

    '*****ボタンをクリックしたときの動作
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.TopMost = False
        Me.Button1.Enabled = False
        Call MainModule.Button_Click(Me, TmpTextBox)
        Me.Button1.Enabled = True
        Me.TopMost = True
    End Sub

    '******メインフォームのテキストボックスでリターンキーを打つと検索開始
    Private Sub TmpTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TmpTextBox.KeyPress

        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then

            'TextValueの値はTmpTextBoxの値で設定
            TextValue = Me.TmpTextBox.Text

            '空でなければ実際の検索
            If TextValue <> "" Then
                'ベースURLを設定する
                Call BaseURL_Setting(Me)

                '検索を実行する
                Call Search_Execute(Me, TmpTextBox)
            End If
        End If
    End Sub

    '*****メインフォームのクローズ
    Private Sub Form1_Closed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        'ダイレクト検索を利用したか否か
        If MakeMeWithTextBox And FormDesign = 5 Then FormDesign = 0
        If MakeMeWithTextBox And FormDesign = 6 Then FormDesign = 1

        'フォームデザインの記録
        Regkey.SetValue("Design", CStr(FormDesign))

        '位置の記録
        Regkey.SetValue("LeftPosition", CStr(Me.Left))
        Regkey.SetValue("TopPosition", CStr(Me.Top))

        '電源監視イベントの解放
        RemoveHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf SystemEvents_PowerModeChanged
    End Sub
#End Region

#Region "クリックワンスによるアプリ配布設定"

    '*********スタートアップへのClickOnceショートカット作成
    Private Sub CheckForShortcut()

        '現在の起動ファイルを定義
        'System.Deployment.ApplicationのImport必要
        Dim ad As ApplicationDeployment
        ad = ApplicationDeployment.CurrentDeployment

        '最初の起動のとき
        If (ad.IsFirstRun) Then
            Dim code As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()

            'ClickOnceでのスタートメニューに作成されるショートカットのフォルダ名（アセンブリの発行者名）
            Dim company As String = String.Empty

            'ClickOnceでのスタートメニューに作成されるショートカット名（アセンブリの製品名）
            Dim description As String = String.Empty

            'ショートカットのフォルダ名（アセンブリの発行者名）の取得
            If (Attribute.IsDefined(code, GetType(System.Reflection.AssemblyCompanyAttribute))) Then
                Dim ascompany As System.Reflection.AssemblyCompanyAttribute
                ascompany = Attribute.GetCustomAttribute(code, GetType(System.Reflection.AssemblyCompanyAttribute))
                company = ascompany.Company
            End If

            'ショートカット名（アセンブリの製品名）の取得
            If (Attribute.IsDefined(code, GetType(System.Reflection.AssemblyTitleAttribute))) Then
                Dim asdescription As System.Reflection.AssemblyTitleAttribute
                asdescription = Attribute.GetCustomAttribute(code, GetType(System.Reflection.AssemblyTitleAttribute))
                description = asdescription.Title
            End If

            '値が空でないとき
            If (company <> String.Empty And description <> String.Empty) Then

                '元のスタートメニューのショートカットのパス
                Dim ShortCutName As String = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "\", company, "\", description, ".appref-ms")

                'ユーザスタートアップのコピー先のパス
                Dim StartUpPath As String = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "\", description, ".appref-ms")


                System.IO.File.Copy(ShortCutName, StartUpPath, True)

            End If

        End If
    End Sub
#End Region

#Region "通知領域"
    '****最小化ボタンを押したとき、通知領域に格納する
    Private Sub Form1_ClientSizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ClientSizeChanged
        Call Form_Minimaize()
    End Sub

    '****通知領域のアイコンを左クリックすると復活する
    Private Sub NotifyIcon1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseClick
        If e.Button = MouseButtons.Left Then
            Me.Visible = True
            Me.ShowInTaskbar = False
        End If
    End Sub
#End Region

#Region "クライアント領域でのマウス設定"
    'フォーム上（クライアント領域）でもマウスでつまんで移動できるようにする
    'マウスのクリック位置を記憶するパラメータ
    Private MousePoint As Point

    '*****マウスのボタンが押されたとき
    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        If (e.Button And MouseButtons.Left) = MouseButtons.Left Then
            '位置を記憶する
            MousePoint = New Point(e.X, e.Y)
        End If
    End Sub

    '*****マウスが動いたとき
    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        If (e.Button And MouseButtons.Left) = MouseButtons.Left Then
            Me.Left += e.X - MousePoint.X
            Me.Top += e.Y - MousePoint.Y
        End If
    End Sub

    '*****マウスのボタンが押されたとき
    Private Sub Form1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseClick
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
            Me.ShowInTaskbar = False
            Me.Visible = False
        End If
    End Sub

    '*****マウスのボタンがクライアント領域上でダブルクリックされたとき（入力用フォームを出現・消失させる）
    Private Sub Form1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDoubleClick
        If FormDesign = 0 Then

            Call ChangeToClassicWithTextBox()
        ElseIf FormDesign = 1 Then

            Call ChangeToAeroWithTextBox()
        ElseIf FormDesign = 5 Then

            Call ChangeToClassic()
        ElseIf FormDesign = 6 Then

            Call ChangeToAero()
        End If
    End Sub
#End Region

#Region "ToolStripMenuItemでの動作"
    Private Sub 検索ボタンを表示ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 検索ボタンを表示ToolStripMenuItem.Click
        Me.Show()
    End Sub

    'ToolStripを通してのプロセス終了パラメータ（再起動ではない）
    Private ManualClose As Boolean = False

    Private Sub 終了ToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 終了ToolStripMenuItem.Click

        'プロセスが動いていたら、グローバルフック用のプロセス停止(停止後Exitedによりメインプロセスは終了)
        If Process.GetProcessesByName("JKAppliGlobalHookProcess").Length > 0 Then

            GetProcessById(HookProcessID).CloseMainWindow()
        End If

        Me.Close()

    End Sub

    Private Sub タスクバーに格納ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles タスクバーに格納ToolStripMenuItem.Click
        Call Form_Minimaize()
    End Sub

    Private Sub ClassicToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClassicToolStripMenuItem.Click
        Call ChangeToClassic()
    End Sub

    Private Sub AeroToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AeroToolStripMenuItem.Click
        Call ChangeToAero()
    End Sub

    Private Sub Classic検索窓ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Classic検索窓ToolStripMenuItem.Click
        Call ChangeToClassicWithTextBox()
    End Sub

    Private Sub Aero検索窓ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Aero検索窓ToolStripMenuItem.Click
        Call ChangeToAeroWithTextBox()
    End Sub
    Private Sub 使いかたヘルプToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 使いかたヘルプToolStripMenuItem.Click

        'ヘルプ画面をオリジナルブラウザで表示させる
        'オリジナルブラウザが存在していれば、それを最大化してTopに
        If Application.OpenForms("OrgWebForm") IsNot Nothing Then

            '大きさを既定サイズに戻して表示
            OrgWebForm.WindowState = FormWindowState.Normal

            '表示位置
            OrgWebForm.Top = 0
            OrgWebForm.Left = DisplayWidth - OrgWebFormWidth

            '大きさ
            OrgWebForm.Width = OrgWebFormWidth
            OrgWebForm.Height = DisplayHeight

            '一度一番上にする
            OrgWebForm.TopMost = True

            'それを解除してメインフォームを一番上
            OrgWebForm.TopMost = False
        End If

        '小さい見出し一覧用ブラウザを起動
        OrgWebForm.Show()

        'ヘルプを表示
        OrgWebForm.WebBrowser1.Navigate(Regkey.GetValue("proxyUrl"))
    End Sub

    Private Sub MF検索ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'MainFormに由来するToolStripMenuを右クリック
        Me.TopMost = False
        Call Button_Click(Me, TmpTextBox)
        Me.TopMost = True
    End Sub

    Private Sub バージョン情報ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles バージョン情報ToolStripMenuItem.Click
        VerInfoDialog.ShowDialog()
    End Sub

#End Region

#Region "フォームデザインの変更・最小化"

    '*****検索用TextBoxつきのエアロの設定
    Public Sub ChangeToAeroWithTextBox()

        If Val(MajorVer) >= 6 Then


            '基本的にXPより前ではこの関数は呼び出せない
            'AeroModeのチェックと適用
            'マージンを調整し、アエロはテキストボックスのエリアでは有効にしない（字が見えないため）
            If DwmIsCompositionEnabled() Then
                DwmExtendFrameIntoClientArea(Me.Handle, New MARGINS(23, 2 + Button1.Height + 6, 5, 5))
            Else


                'Aeroモードになっていない場合、タイマーを止める
                Timer1.Stop()

                Dim returnValue As DialogResult
                returnValue = MessageBox.Show("「いつでもジャパンナレッジ」で透明感のあるAeroフォームを使用する際には、「コントロールパネル」の「個人設定」から「Aeroテーマ」を選んでください。Classicフォームに変更します", "設定変更のお願い", MessageBoxButtons.OK, MessageBoxIcon.Error)

                'Classicモードへ
                Call ChangeToClassicWithTextBox()

                Exit Sub
            End If

        End If
        'ツールチップの文言設定
        ToolTip1.SetToolTip(Button1, "検索したい語をマウスで選択・反転させるか、検索窓に検索したい語を入力してから、このアイコンをクリックしてください")

        'パラメータ設定
        FormDesign = 6

        'メニューに「チェック」
        AeroToolStripMenuItem.Checked = False
        ClassicToolStripMenuItem.Checked = False
        Aero検索窓ToolStripMenuItem.Checked = True
        Classic検索窓ToolStripMenuItem.Checked = False

        '一旦非表示
        Me.Visible = False

        'サーチボックスを空に
        Me.TmpTextBox.Text = ""

        'サーチ用textboxの大きさ設定
        Me.TmpTextBox.Height = 20
        Me.TmpTextBox.Width = 122
        'Me.TmpTextBox.Width = 144

        'テキストボックスの色の設定
        Me.TmpTextBox.BackColor = Color.White
        Me.TmpTextBox.ForeColor = Color.Black

        'aero用にFormの大きさを決定
        'トップのタイトルバーを非表示
        Me.ControlBox = False

        '背景を黒に
        Me.BackColor = Color.Black

        '大きさは固定(テキストボックスとマージン分縦に広げる)
        Me.MinimumSize = New System.Drawing.Size(163, 72 + TmpTextBox.Height + 3)
        Me.MaximumSize = New System.Drawing.Size(163, 72 + TmpTextBox.Height + 3)

        'ボタンの位置
        Me.Button1.Location = New Point(22, 2)

        'TextBoxの位置
        Me.TmpTextBox.Location = New Point(22, 2 + Button1.Height + 5)

        '再度表示
        Me.Visible = True
        Me.ShowInTaskbar = False

        'フォーカスを充てる（カーソル点滅）
        Me.TmpTextBox.Focus()

    End Sub

    '******TextBoxつきのクラシックの設定
    Public Sub ChangeToClassicWithTextBox()

        'Aeroクラスの利用ができるか否か
        'まずVista以降である
        If Val(MajorVer) >= 6 Then

            'Aeroが有効になっていたら、テキストボックスの背後は不透過に変更
            If DwmIsCompositionEnabled() Then
                DwmExtendFrameIntoClientArea(Me.Handle, New MARGINS(2, 2 + Button1.Height + 6, 2, 2))
            End If

        End If

        'ツールチップの文言設定
        ToolTip1.SetToolTip(Button1, "検索したい語をマウスで選択・反転させるか、検索窓に検索したい語を入力してから、このアイコンをクリックしてください")

        'パラメータ設定
        FormDesign = 5

        'メニューに「チェック」
        AeroToolStripMenuItem.Checked = False
        ClassicToolStripMenuItem.Checked = False
        Aero検索窓ToolStripMenuItem.Checked = False
        Classic検索窓ToolStripMenuItem.Checked = True

        '一旦非表示
        Me.Visible = False

        'XPおよび非管理者権限ユーザ用
        'トップのタイトルバーを表示
        Me.ControlBox = True

        'サーチボックスを空に
        Me.TmpTextBox.Text = ""

        'サーチ用textboxの大きさ設定
        Me.TmpTextBox.Height = 20
        Me.TmpTextBox.Width = 122

        'テキストボックスの色・フォントの設定
        Me.TmpTextBox.BackColor = Color.White
        Me.TmpTextBox.ForeColor = Color.Black
        'Me.TmpTextBox.Font = New Font("Arial", 10, FontStyle.Bold)

        '背景色は標準（control）
        Me.BackColor = System.Drawing.SystemColors.ControlLight

        '大きさは固定
        Me.MinimumSize = New System.Drawing.Size(143, 92 + TmpTextBox.Height + 3)
        Me.MaximumSize = New System.Drawing.Size(143, 92 + TmpTextBox.Height + 3)

        'ボタンの位置
        Me.Button1.Location = New Point(2, 2)

        'TextBoxの位置
        Me.TmpTextBox.Location = New Point(2, 2 + Button1.Height + 5)

        '再度表示
        Me.Visible = True
        Me.ShowInTaskbar = False

        'フォーカスを充てる（カーソル点滅）
        Me.TmpTextBox.Focus()
    End Sub

    Private Sub ChangeToAero()

        If Val(MajorVer) >= 6 Then

            '基本的にXPより前ではこの関数は呼び出せない
            'AeroModeのチェックと適用
            If DwmIsCompositionEnabled() Then
                DwmExtendFrameIntoClientArea(Me.Handle, New MARGINS(-1, 0, 0, 0))
            Else


                'Aeroモードになっていない場合、タイマーを止める
                Timer1.Stop()

                Dim returnValue As DialogResult
                returnValue = MessageBox.Show("「いつでもジャパンナレッジ」で透明感のあるAeroフォームを使用する際には、「コントロールパネル」の「個人設定」から「Aeroテーマ」を選んでください。Classicフォームに変更します", "設定変更のお願い", MessageBoxButtons.OK, MessageBoxIcon.Error)

                'Classicモードへ
                Call ChangeToClassic()
                Exit Sub
            End If
        End If
        'ツールチップの文言設定
        ToolTip1.SetToolTip(Button1, "検索したい語をマウスで選択・反転させてから、このアイコンをクリックしてください")

        'パラメータ設定
        FormDesign = 1

        'メニューに「チェック」
        AeroToolStripMenuItem.Checked = True
        ClassicToolStripMenuItem.Checked = False
        Aero検索窓ToolStripMenuItem.Checked = False
        Classic検索窓ToolStripMenuItem.Checked = False

        '一旦非表示
        Me.Visible = False

        'aero用にFormの大きさを決定
        'トップのタイトルバーを非表示
        Me.ControlBox = False

        '背景を黒に
        Me.BackColor = Color.Black

        '大きさは固定
        Me.MinimumSize = New System.Drawing.Size(163, 72)
        Me.MaximumSize = New System.Drawing.Size(163, 72)

        'ボタンの位置
        Me.Button1.Location = New Point(22, 2)



        '再度表示
        Me.Visible = True
        Me.ShowInTaskbar = False


    End Sub

    Private Sub ChangeToClassic()

        'ツールチップの文言設定
        ToolTip1.SetToolTip(Button1, "検索したい語をマウスで選択・反転させてから、このアイコンをクリックしてください")

        'パラメータ設定
        FormDesign = 0

        'メニューに「チェック」
        AeroToolStripMenuItem.Checked = False
        ClassicToolStripMenuItem.Checked = True
        Aero検索窓ToolStripMenuItem.Checked = False
        Classic検索窓ToolStripMenuItem.Checked = False

        '一旦非表示
        Me.Visible = False

        'XPおよび非管理者権限ユーザ用
        'トップのタイトルバーを表示
        Me.ControlBox = True

        '背景色は標準（control）
        Me.BackColor = System.Drawing.SystemColors.ControlLight

        '大きさは固定
        Me.MinimumSize = New System.Drawing.Size(143, 91)
        Me.MaximumSize = New System.Drawing.Size(143, 91)

        'ボタンの位置
        Me.Button1.Location = New Point(2, 2)

        'TextBoxの位置(見えない位置へ)
        Me.TmpTextBox.Location = New Point(22, 2 + Button1.Height + 10)

        '再度表示
        Me.Visible = True
        Me.ShowInTaskbar = False
    End Sub

    '*****フォームの最小化
    Private Sub Form_Minimaize()

        '最小化して普通の動作
        Me.WindowState = FormWindowState.Maximized
        Me.WindowState = FormWindowState.Normal
        Me.ShowInTaskbar = False
        Me.Visible = False
    End Sub
#End Region

End Class
