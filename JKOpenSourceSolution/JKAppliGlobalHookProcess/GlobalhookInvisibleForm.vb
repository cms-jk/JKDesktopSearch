'**************************************
'"JKDeskTopSearch" Powered by Bukkyo University Library
'本プログラムについてJapanKnowledge会員、および佛教大学図書館が許可する業者以外への公開を禁じます。
'***************************************

Imports System.IO
Imports System.Net
Imports System.Windows.Forms.Form
Imports System.Deployment.Application
Imports System.Diagnostics.Process

'共有クラスからWin32API関数・定数のインポート
Imports JKWin32HookAndCom.Win32API

#Region "プロセス間通信用のインポート"

'参照タブでSystem.Runtime.Remotingを追加すること
'今回は同一端末内での通信のためIpcプロトコルを利用する。ポートは8181を指定
'Httpプロトコルを使用する場合は、Ipc→Httpに置き換えれば利用可能。
'ただしUrlはHttp://localhostで開始すること

Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Ipc
'オブジェクトのリース期間を無期限に設定する必要あり
Imports System.Runtime.Remoting.Lifetime
'プロジェクトの共有クラスを追加すること
Imports JKWin32HookAndCom

#End Region

Public Class JKAppliGlobalHookProcess
    Inherits System.Windows.Forms.Form

    Private WithEvents GlobalHooks As New GlobalHook

    'Win32 APIとしてSetForegroundWindow,keybd_event,GetWindowRect,WindowFromPoint,GetAncestor,GetClassNameを使用

#Region "フォームのロードとクローズ"

    Private MousePressPara As Boolean
    Private MouseX As Integer
    Private MouseUpPara As Boolean
    Private MakeMeWithTextBox As Boolean

    '*****起動フォームを非表示にするためのパラメータ変更
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        <System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, _
            Flags:=System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)> _
        Get
            Const WS_EX_TOOLWINDOW As Int32 = &H80
            Const WS_POPUP As Int32 = &H80000000
            Const WS_VISIBLE As Int32 = &H10000000
            Const WS_SYSMENU As Int32 = &H80000
            Const WS_MAXIMIZEBOX As Int32 = &H10000

            Dim cp As System.Windows.Forms.CreateParams
            cp = MyBase.CreateParams
            cp.ExStyle = WS_EX_TOOLWINDOW
            cp.Style = WS_POPUP Or WS_VISIBLE Or WS_SYSMENU Or WS_MAXIMIZEBOX
            cp.Height = 0
            cp.Width = 0
            Return cp
        End Get
    End Property

    '*****起動フォームをロード
    Private Sub GlobalhookInvisibleForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Debug.WriteLine("form load")
        '電源監視（スリープモードから復帰した時に通信を再開させるため）
        AddHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf SystemEvents_PowerModeChanged

        '----プロセス間通信
        '①通信用オブジェクトのリース期限を無期限にする
        Lifetime.LifetimeServices.LeaseTime = TimeSpan.Zero

        '②メインプロセスへの送信準備
        'IPCチャネルを用意する(送信ポートは9191)
        Try
            Dim SendChannel As New IpcChannel(9191)
            ChannelServices.RegisterChannel(SendChannel, False)

            '③メインプロセスへの接続準備（受信ポートは8181）
            RemotingConfiguration.RegisterWellKnownClientType(GetType(HookClientToMainServerEvent), "IPC://8181/SendToMain_URI")
            '-----------
        Catch ex As Exception
            Debug.Print("JK GlobalHook IPC error:" & ex.Message)
            End
        End Try

        'グローバルフックを組み込む
        GlobalHooks.InstallHooks()
    End Sub

    '******フォームのロード終了を待ちプロセスIDを取得する
    Private Sub GlobalhookInvisibleForm_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown

        'プロセスIDを取得する（MainformのProcess1は対象の再起動時に消滅するため）
        Dim ProcessId As Integer
        ProcessId = GetCurrentProcess().Id()

        'プロセスIDをメインプロセス（サーバ）へ通知
        Dim SendToMain As HookClientToMainServerEvent = New HookClientToMainServerEvent
        SendToMain.RaiseServerEvent("ProcessID:anThAjkJG23bsjhiQPj:" & ProcessId.ToString)


    End Sub

    '******Windows7のHKEY_CURRENT_USER\Control Panel\Desktop\LowLevelHooksTimeoutの設定問題に対応
    Private Sub SystemEvents_PowerModeChanged(ByVal sender As Object, ByVal e As Microsoft.Win32.PowerModeChangedEventArgs)

        'スリープもしくは休止モードから復活した場合再起動
        If e.Mode = Microsoft.Win32.PowerModes.Resume Then
            Application.Restart()
        End If
    End Sub

    '******フォームのクローズイベント
    Private Sub GlobalhookInvisibleForm_Closed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        Try
            '電源監視イベントの解放
            RemoveHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf SystemEvents_PowerModeChanged
        Catch ex As Exception
            Debug.Print("RemoveHandler error" + ex.Message)

            Throw ex
        End Try

        Try
            GlobalHooks.RemoveHooks()
        Catch ex As Exception
            Debug.Print("RemoveHooks error" + ex.Message)

            Me.Close()
            Exit Sub
        End Try
    End Sub
#End Region

#Region "右コントロールキーによるダイレクト検索モードの実行"

    'アクティブなウインドウのハンドルを取得
    Private BeforeMouseMoveWin As RECT
    Private AfterMouseMoveWin As RECT
    Private UnderMousePoint As Point
    Private UnderMousePointHandle As Integer


    Private Sub GlobalHooks_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GlobalHooks.MouseDown

        Try

            Debug.WriteLine("A-マウスダウンイベント発生")

            'パラメータの初期化
            MousePressPara = False
            UnderMousePoint.X = e.X
            UnderMousePoint.Y = e.Y

            '押した時点でのポインタ下のウインドウのハンドルを取得
            UnderMousePointHandle = WindowFromPoint(UnderMousePoint)

            '①ポインタ下のウインドウのルートオーナーが自分自身のクラス名であった場合、処理を中止する
            'ウインドウクラス名を取得し、そこに文字列「WindowsForms10」で始まらないかを調査する
            Const GA_ROOTOWNER As Short = 3
            Const BUFFER_SIZE As Integer = 36
            Dim ClassName As String = Space(BUFFER_SIZE)

            GetClassName(GetAncestor(UnderMousePointHandle, GA_ROOTOWNER), ClassName, BUFFER_SIZE)
            Debug.WriteLine(ClassName)


            '②グローバルフックの対象であれば、押した時点での該当ハンドルのウインドウの四隅の座標を取得する
            'この座標が移動するようであれば、ALTを使っての検索はさせられない
            GetWindowRect(UnderMousePointHandle, BeforeMouseMoveWin)

            '左クリックであれば「MousePressPara」にTrueを代入
            If e.Button.ToString = "Left" Then
                MousePressPara = True
                MouseX = e.X
                Debug.WriteLine("B-マウスダウンパラメータTrue変更成功")
            End If

        Catch ex As Exception
            Application.Restart()

        End Try

    End Sub


    Private Sub GlobalHooks_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GlobalHooks.MouseUp

        Try
            'パラメータの初期化
            MouseUpPara = False

            Debug.WriteLine("C-マウスアップイベント発生")

            '①前提条件は「左クリックされたままの状態」であること
            If MousePressPara = False Then Exit Sub

            '②マウス直下のウインドウの移動を感知する。もしウインドウが動いていたらなら処理中止
            'アクティブなウインドウが固定された状態で、５ポイント以上動いたのであれば、MousePressPara=true

            GetWindowRect(UnderMousePointHandle, AfterMouseMoveWin)

            If AfterMouseMoveWin.top = BeforeMouseMoveWin.top And AfterMouseMoveWin.left = BeforeMouseMoveWin.left And AfterMouseMoveWin.right = BeforeMouseMoveWin.right And AfterMouseMoveWin.bottom = BeforeMouseMoveWin.bottom Then

                If System.Math.Abs(e.X - MouseX) >= 5 Then

                    MouseUpPara = True
                    Debug.WriteLine("D-マウスアップパラメータTrue変更成功")

                    'マウスダウンの初期化
                    MousePressPara = False
                End If
            End If
        Catch ex As Exception
            Application.Restart()

        End Try

    End Sub

    Private Sub GlobalHooks_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GlobalHooks.KeyUp

        Try
            Debug.WriteLine(e.KeyCode.ToString)
            Debug.WriteLine("E-キーアップイベント発生")

            'Keydownイベントは押している間はずっと発生してしまうので、KeyUpイベントを用いる
            'Controlキーを押すと転送開始
            'ALTキーはメニューを起動するため、フォーカスがずれるので使えない
            If e.KeyCode.ToString = "RControlKey" And MouseUpPara Then
                Debug.WriteLine("F-事前コピールーチン起動成功")
                'マウスアップの初期化
                MouseUpPara = False
                Call CopyAtOnceOnClient()

            End If
        Catch ex As Exception
            Debug.Print("exception" & ex.Message)
            Application.Restart()

        End Try

    End Sub
    '***********事前に選択領域を検索ボタンのテキストボックスにコピーしておく
    Private Sub CopyAtOnceOnClient()

        Try
            '初期化
            MouseUpPara = False
            'MousePressMovePara = False
            MousePressPara = False

            'TmpTextBoxの値をクリア
            TmpTextBoxGH.Text = ""

            'マウスポインタの直下のウインドウをアクティブにする
            SetForegroundWindow(UnderMousePointHandle)

            '「CTRL+C」で反転領域をクリップボードにコピー
            '500ミリ秒以上でエクセルのセルからのコピーを認識（時間調整）
            Application.DoEvents()
            Threading.Thread.Sleep(500)
            Const KEYEVENTF_KEYUP = &H2
            Const vbKeyControl = 17
            Const vbKeyC = &H43

            '3回クリップボードへのコピーにトライ（Excelのセルなど1度では失敗するため）
            Dim i As Short
            For i = 1 To 3
                Call keybd_event(vbKeyControl, 0, 0, 0)
                Call keybd_event(vbKeyC, 0, 0, 0)
                Call keybd_event(vbKeyC, 0, KEYEVENTF_KEYUP, 0)
                Call keybd_event(vbKeyControl, 0, KEYEVENTF_KEYUP, 0)

                '時間調整
                Application.DoEvents()
                Threading.Thread.Sleep(50)

            Next

            Me.WindowState = FormWindowState.Normal
            Me.TopLevel = True
            Me.Activate()
            TmpTextBoxGH.Clear()
            TmpTextBoxGH.Focus()


            '書式を抜いて貼り付け
            Const vbKeyV = &H56
            Call keybd_event(vbKeyControl, 0, 0, 0)
            Call keybd_event(vbKeyV, 0, 0, 0)
            Call keybd_event(vbKeyV, 0, KEYEVENTF_KEYUP, 0)
            Call keybd_event(vbKeyControl, 0, KEYEVENTF_KEYUP, 0)


            '時間調整
            Application.DoEvents()
            Threading.Thread.Sleep(200)

            Debug.WriteLine("GH.text:" & TmpTextBoxGH.Text)

            'メインプロセス（サーバ）への通知
            Dim SendToMain As HookClientToMainServerEvent = New HookClientToMainServerEvent
            SendToMain.RaiseServerEvent(TmpTextBoxGH.Text)

            '再起動（Windows7のGlobalHookハングアップ対策）

            Debug.WriteLine("Let's restart")
            Application.Restart()

        Catch ex As Exception
            Debug.Print("CAOO excp:" & ex.Message)
            Application.Restart()

        End Try
    End Sub

#End Region

    Public Sub New()

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

        ' ThreadExceptionイベント・ハンドラを登録する
        AddHandler Application.ThreadException, AddressOf Application_ThreadException
        ' UnhandledExceptionイベント・ハンドラを登録する
        AddHandler System.Threading.Thread.GetDomain().UnhandledException, AddressOf Application_UnhandledException

    End Sub

    ''' <summary>
    ''' 未処理例外をキャッチするイベントハンドラ。
    ''' メインスレッド用。(WindowsForm専用)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Sub Application_ThreadException(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        'メッセージボックス表示
        'MessageBox.Show(e.Exception.Message)
        Debug.Print(e.Exception.Message)

        'イベントログ出力
        OutPutLogErr(e.Exception)
        '予期せぬ例外時は強制終了
        Environment.Exit(-1)

    End Sub

    ''' <summary>
    ''' 未処理例外をキャッチするイベントハンドラ。
    ''' 別スレッド用。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Sub Application_UnhandledException(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        Dim ex As Exception = CType(e.ExceptionObject, Exception)
        If Not ex Is Nothing Then
            'メッセージボックス表示
            ' MessageBox.Show(ex.Message)
            Debug.Print(ex.Message)

            'イベントログ出力
            OutPutLogErr(ex)
            '予期せぬ例外時は強制終了
            Environment.Exit(-1)
        End If
    End Sub

    '' <summary>
    ''' エラーログをイベントビュアーに出力
    ''' </summary>
    ''' <param name="e">Exceptionオブジェクト</param>
    ''' <remarks></remarks>
    Private Shared Sub OutPutLogErr(ByVal e As Exception)
        Try
            'ソース
            Dim sourceName As String = "JKAppliClobalHookProcess"
            'ソースが存在していない時は、作成する
            If Not System.Diagnostics.EventLog.SourceExists(sourceName) Then
                'ログ名を空白にすると、"Application"となる
                System.Diagnostics.EventLog.CreateEventSource(sourceName, "")
            End If
            'テスト用にイベントログエントリに付加するデータを適当に作る
            Dim myData() As Byte = {}
            Dim msg As String = "例外：" & vbNewLine & e.Message & vbNewLine & "例外スタックトレース" & vbNewLine & e.StackTrace & vbNewLine
            If e.InnerException IsNot Nothing Then
                msg = msg & "InnerException:" & vbNewLine & e.InnerException.Message & vbNewLine & "InnerExceptionスタックトレース:" & vbNewLine & e.InnerException.StackTrace
            End If
            'イベントログにエントリを書き込む
            'ここではエントリの種類をエラー、イベントIDを1、分類を1000とする
            System.Diagnostics.EventLog.WriteEntry(sourceName, msg, System.Diagnostics.EventLogEntryType.Error, 1, 1000, myData)
        Catch ex As Exception
        End Try
    End Sub
End Class