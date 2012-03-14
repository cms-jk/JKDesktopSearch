'**************
'"JKDeskTopSearch" Powered by Bukkyo University Library
'本プログラムについてJapanKnowledge会員、および佛教大学図書館が許可する業者以外への公開を禁じます。
'**************

Imports System.IO
Imports System.Net
Imports System.Windows.Forms.Form
Imports System.Runtime.InteropServices
'Win32API関数の共有
Imports JKWin32HookAndCom.Win32API


Module MainModule

    '本モジュールではWin32APIとしてGetWindow、IsWindowVisible、SetForegroundWindow、keybd_eventを利用

#Region "各種変数定義"
    'レジストリ位置の設定
    Public Regkey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\Bulib\JKOpenSourceApplication")
    Public HookRegkey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Control Panel\Desktop")

    'メインフォームのデザイン決定用パラメータ
    Public FormDesign As Short

    'フォームの幅設定
    Public OrgWebFormWidth As Short = 360
    Public OrgWebForm2Width As Short = 640

    'メインフォームの検索後移動先（上、右）
    Public MainFormTop As Short = 55
    Public MainFormRight As Short = 25

    'テキストボックスの文字列を格納（履歴としても利用）
    Public TextValue As String

    'ディスプレイの作業領域
    Public DisplayHeight As Short = System.Windows.Forms.Screen.GetWorkingArea(JKDeskTopSearch).Height
    Public DisplayWidth As Short = System.Windows.Forms.Screen.GetWorkingArea(JKDeskTopSearch).Width

    '検索用BaseURL
    Public BaseURLStr As String
    Public BaseURLStrRireki As String

    '外部のウィンドウのハンドルをクリックで取得した時に値格納
    Public hWndTarget As Integer

    '---Ver.1.5.0 IE9 グローバルフック
    Public MousePressPara As Boolean
    'Public MousePressMovePara As Boolean
    Public MouseX As Integer
    Public MouseUpPara As Boolean
    'Public CopyatOncePara As Boolean
    Public MakeMeWithTextBox As Boolean

#End Region

#Region "検索（ボタンクリック時）分岐のまとめ"
    '*******検索方法のまとめ******

    '(A)検索のBaseURLが履歴と変わった場合→全てテキストボックスの中を検索
    'ただし起動して初回の検索履歴がなくて異なる場合は除く

    '(B)検索のBaseURLに変更がない場合

    '選択領域か、直接テキストボックスのいずれを検索対象にするのかを、検索履歴（TextValue）とテキストボックス(TmpTextBox)で判断
    '①検索履歴×、テキストボックス×→選択領域（先にクリップボードへデータ送信）
    '②検索履歴×、テキストボックス○→直接テキストボックス
    '③検索履歴○、テキストボックス×→選択領域（先にクリップボードへデータ送信）
    '④検索履歴○、テキストボックス○→以下の条件で実行

    '⑤検索履歴＝テキストボックス→選択領域（先にクリップボードへデータ送信）
    '⑥検索履歴<>テキストボックス→直接テキストボックス

    '以上の(A)-(B),①‐⑥の条件をシンプルに書くと

    '１、検索履歴がない場合と検索のBaseURLに変更がなく、かつテキストボックスに値がないか、検索履歴と同じときは「選択領域」
    '２、それ以外は直接テキストボックス検索
    '****************************
#End Region

#Region "検索ルーチン"
    '*****ボタンをクリックしたときの動作
    Public Sub Button_Click(ByVal FormClassName As Form, ByVal TmpTextBox As TextBox)

        'ベースURLを設定する
        Call BaseURL_Setting(FormClassName)

        Debug.Print("urlR:" & BaseURLStrRireki)
        Debug.Print("urlN:" & BaseURLStr)
        Debug.Print("box:" & TmpTextBox.Text)
        Debug.Print("val:" & TextValue)


        '「検索方法1」の条件のとき、クリップボードを利用 
        If BaseURLStrRireki = Nothing Or BaseURLStrRireki = BaseURLStr Then
            If TmpTextBox.Text = "" Or TmpTextBox.Text = TextValue Then Call GetClipboardData(FormClassName, TmpTextBox)
        End If

        'テキストボックスの値の検索
        Call Search_Execute(FormClassName, TmpTextBox)
    End Sub

    '*****クリップボードのデータを取得しTextValueに値をセットするルーチン
    Public Sub GetClipboardData(ByVal FormClassName As Form, ByVal TmpTextBox As TextBox)

        'クリップボードの内容をクリア
        System.Windows.Forms.Clipboard.Clear()

        'TmpTextBoxの値を退避
        Dim tmpText = TmpTextBox.Text

        'TmpTextBoxの値をクリア
        TmpTextBox.Text = ""

        '反転している文字列をもつオブジェクトを取得する（ボタンを押す直前にアクティブだったオブジェクト）
        '一時的に最前面を解除
        FormClassName.TopMost = False

        '最前面であるFormClassNameのハンドルを取得
        Dim hd As System.IntPtr
        Dim tmp As Boolean
        Const GW_HWNDNEXT = 2
        hd = FormClassName.Handle

        '本当の最前面のひとつ前を検索しハンドルを取得
        'ひとつ前は必ず可視ウィンドウでなくてはならない
        Do Until IsWindowVisible(hd) And hd <> FormClassName.Handle
            hd = GetWindow(hd, GW_HWNDNEXT)
        Loop

        '取得したハンドルのウインドウをアクティブ化
        Dim hdp As System.IntPtr
        hdp = hd
        tmp = SetForegroundWindow(hdp)

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

        'クリップボードにテキストデータがあるかを確認。ない場合はhdpをhWndTargetに変えてリトライ
        Dim data As IDataObject = Clipboard.GetDataObject()
        If data.GetDataPresent(DataFormats.Text) = Nothing Then
            tmp = SetForegroundWindow(hWndTarget)

            '「CTRL+C」で反転領域をクリップボードにコピー
            '500ミリ秒以上でエクセルのセルからのコピーを認識（時間調整）
            Application.DoEvents()
            Threading.Thread.Sleep(500)

            '3回クリップボードへのコピーにトライ（Excelのセルなど1度では失敗するため）
            For i = 1 To 3
                Call keybd_event(vbKeyControl, 0, 0, 0)
                Call keybd_event(vbKeyC, 0, 0, 0)
                Call keybd_event(vbKeyC, 0, KEYEVENTF_KEYUP, 0)
                Call keybd_event(vbKeyControl, 0, KEYEVENTF_KEYUP, 0)

                '時間調整
                Application.DoEvents()
                Threading.Thread.Sleep(50)
            Next
        End If

        '最前面に復活
        FormClassName.TopMost = True

        'Clipboardデータから書式なしデータの作成
        'FormClassNameをアクティブ化
        tmp = SetForegroundWindow(FormClassName.Handle)
        TmpTextBox.Clear()
        TmpTextBox.Focus()

        '時間調整
        Application.DoEvents()
        Threading.Thread.Sleep(200)

        '書式を抜いて貼り付け
        Const vbKeyV = &H56
        Call keybd_event(vbKeyControl, 0, 0, 0)
        Call keybd_event(vbKeyV, 0, 0, 0)
        Call keybd_event(vbKeyV, 0, KEYEVENTF_KEYUP, 0)
        Call keybd_event(vbKeyControl, 0, KEYEVENTF_KEYUP, 0)

        '時間調整
        Application.DoEvents()
        Threading.Thread.Sleep(500)

        'テキストがクリアされてしまったら元の値に戻す(連続で同じ検索語の右CTRLへの対策
        If TmpTextBox.Text = "" Then
            TmpTextBox.Text = tmpText
        End If

    End Sub

    '*****検索情報をAPIにおくるサブルーチン
    Public Sub Search_Execute(ByVal FormClassName As Form, ByVal TmpTextBox As TextBox)

        'TmpTextBoxに入った値を利用して検索
        TextValue = TmpTextBox.Text
        If TextValue <> "" Then
            Dim UTF8text As String = Uri.EscapeUriString(TextValue)
            Dim URL As String = BaseURLStr & UTF8text

            '一時的に最上位を解除
            FormClassName.TopMost = False

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

                '一番上にする
                OrgWebForm.TopMost = True
            End If

            '小さい見出し一覧用ブラウザを起動
            OrgWebForm.Show()
            OrgWebForm.WebBrowser1.Navigate(URL)

            '再設定
            FormClassName.TopMost = True

            '検索履歴（TextValue）の値をTmpTextBoxとTmpTextBoxOnBrowのいずれにも表示
            JKDeskTopSearch.TmpTextBox.Text = TextValue
            OrgWebForm.TmpTextBoxOnBrow.Text = TextValue
        Else
            Dim returnValue As DialogResult
            returnValue = MessageBox.Show("単語や文字が選択・入力されていないか、選択文字の取得に失敗しました。IE9のテキストを選択する場合には選択後、Ctrlキーを「1度」押してください", "検索エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        '値の解放
        FormClassName = Nothing
        TmpTextBox = Nothing
    End Sub

    '*****ジャパンナレッジAPIによるベースURLの作成
    Public Sub BaseURL_Setting(ByVal FormClassName As Form)

        '検索に利用したBaseURLStrを履歴に格納
        BaseURLStrRireki = BaseURLStr

        '値の初期化
        BaseURLStr = ""

        'レジストリから取得
        Dim proxyUrl As String = Regkey.GetValue("proxyUrl")

        'メインフォームから検索する場合（固定）
        If FormClassName.Name = "JKDeskTopSearch" Then
            BaseURLStr = proxyUrl & "?currentPage=1&contentsSelect=all&flowMode=1&searchWord="
        Else

            Try
                'オリジナルブラウザの窓から検索する場合
                '検索Web画面の検索ボタンを押したとき,検索対象コンテンツの種類を示す、Radioボタンの状況を取得する
                Dim SelectValue As Short
                If OrgWebForm.WebBrowser1.Document.GetElementById("flowMode1").GetAttribute("checked") Then SelectValue = 1
                If OrgWebForm.WebBrowser1.Document.GetElementById("flowMode2").GetAttribute("checked") Then SelectValue = 2
                If OrgWebForm.WebBrowser1.Document.GetElementById("flowMode3").GetAttribute("checked") Then SelectValue = 3

                '検索Web画面の「詳細」で検索コンテンツを指定した場合、チェックが入っているtitleidの値リストを取得する
                'nameが「titleid」について作業
                Dim BrowserAllElements As HtmlElementCollection = OrgWebForm.WebBrowser1.Document.GetElementsByTagName("*")

                '作成されるIdリストを初期化
                Dim TitleIdList As String = ""
                '作成されるIdリストの数をカウント
                Dim TitleIdListCount As Short = 0

                'タグの数が0以上でないとエラーがでる
                If BrowserAllElements.Count > 0 Then
                    Dim BrowserAllElement As HtmlElement
                    For Each BrowserAllElement In BrowserAllElements

                        'NameStr（nameの要素)を各エレメントで取得する
                        'CheckedStr（classnameの要素）を各エレメントで取得する
                        Dim NameStr As String = BrowserAllElement.GetAttribute("name")
                        Dim CheckedStr As String = BrowserAllElement.GetAttribute("classname")

                        'NameStr, CheckedStrともに存在していない、長さ0の場合はだめ
                        If ((NameStr IsNot Nothing) And (NameStr.Length <> 0)) And ((CheckedStr IsNot Nothing) And (CheckedStr.Length <> 0)) Then
                            If NameStr.ToLower().Equals("titleid") And CheckedStr.ToLower().Equals("title_chkbox checked") Then
                                If TitleIdListCount = 0 Then

                                    '一つ目のチェックの場合
                                    TitleIdList = BrowserAllElement.GetAttribute("value")
                                Else

                                    '二つ目以降のチェックの場合
                                    TitleIdList = TitleIdList & "," & BrowserAllElement.GetAttribute("value")
                                End If
                                TitleIdListCount = TitleIdListCount + 1
                            End If
                        End If
                    Next
                End If

                'もしTitleIdListが空であれば、"all"をセットする
                If TitleIdList = "" Then TitleIdList = "all"

                'Baseとなる検索用の作成
                BaseURLStr = proxyUrl & "?currentPage=1&contentsSelect=" & TitleIdList & "&flowMode=" & SelectValue & "&searchWord="
            Catch ex As Exception
                Debug.Print("baseUrl_Setting error:" & ex.Message)
                BaseURLStr = proxyUrl & "?currentPage=1&contentsSelect=all&flowMode=1&searchWord="
            End Try
        End If
    End Sub

#End Region
    
#Region "メインフォームの制御"

    '*****MainFormの位置の記録
    Public Sub MainForm_Setting()

        '移動先の位置と現在位置が同じ場所のときは記録しない。位置が規定の場所と同じ時のみ記録
        If JKDeskTopSearch.Top <> MainFormTop Or JKDeskTopSearch.Left <> DisplayWidth - JKDeskTopSearch.Width - MainFormRight Then
            Regkey.SetValue("LeftPosition", CStr(JKDeskTopSearch.Left))
            Regkey.SetValue("TopPosition", CStr(JKDeskTopSearch.Top))
        End If

        'MainFormを邪魔にならない場所へ移動させる
        JKDeskTopSearch.Top = MainFormTop
        JKDeskTopSearch.Left = DisplayWidth - JKDeskTopSearch.Width - MainFormRight
    End Sub

#End Region


End Module
