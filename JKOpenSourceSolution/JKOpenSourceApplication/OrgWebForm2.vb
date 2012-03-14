'**************
'"JKDeskTopSearch" Powered by Bukkyo University Library
'本プログラムについてJapanKnowledge会員、および佛教大学図書館が許可する業者以外への公開を禁じます。
'**************

Imports System.Windows.Forms

Public Class OrgWebForm2
    Inherits System.Windows.Forms.Form

    'オリジナルブラウザ内でタブを利用してURLを開くための宣言
    '複数のイベントで使用するためWithEventsにて表示
    Public WithEvents WebBrowser1 As ExWebBrowser
    Public WithEvents TabControl1 As New System.Windows.Forms.CustomTabControl
    Public TabPage1 As TabPage

#Region "フォームのロードとクローズ"
    '*****フォームのロード
    Private Sub OrgWebForm2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        '一番上に表示
        Me.TopMost = True

        '表示位置
        Me.Top = 0
        '左寄せ(最初の見出しブラウザの横に表示)
        Me.Left = DisplayWidth - OrgWebFormWidth - OrgWebForm2Width

        '大きさ
        Me.Width = OrgWebForm2Width
        Me.Height = DisplayHeight

        'フォームの見出し
        Me.Text = "JapanKnowledge"

        '垂直方向にのみサイズ変更を可能（横は現在の値）
        Me.MaximumSize = New System.Drawing.Size(Me.Size.Width, Integer.MaxValue)
        Me.MinimumSize = New System.Drawing.Size(Me.Size.Width, 0)
        Me.MaximizeBox = False

        'WebBrowser1の定義
        Me.WebBrowser1 = New ExWebBrowser
        Me.WebBrowser1.Dock = DockStyle.Fill
        AddHandler WebBrowser1.NewWindow2, AddressOf WebBrowser_NewWindow2

        ' Tabに閉じるボタン
        'Me.TabControl1.Appearance = TabAppearance.Buttons
        Me.TabControl1.DisplayStyleProvider.ShowTabCloser = True

        'TabPage1を生成し、その上にWebBrowser1を追加
        Me.TabPage1 = New TabPage
        Me.TabPage1.Controls.Add(WebBrowser1)

        'TabControl1（つまむ部分）にTabPage1を追加
        Me.TabControl1.Dock = DockStyle.Fill
        Me.TabControl1.TabPages.Add(TabPage1)

        '開かれたタブが一番上になる
        Me.TabControl1.BringToFront()

        'TabControl1をフォームに追加
        Me.Controls.Add(Me.TabControl1)

        '右クリック抑制
        WebBrowser1.IsWebBrowserContextMenuEnabled = False

        '解除
        Me.TopMost = False
    End Sub

    '*****フォームのクローズ
    Private Sub OrgWebForm2_Closed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        'グローバルフック
        'globalHooks.InstallHooks()

        'OrgWebFormが存在していれば、それを最大化してTopに
        If Application.OpenForms("OrgWebForm") IsNot Nothing Then

            '大きさを既定サイズに戻して表示
            OrgWebForm.WindowState = FormWindowState.Normal

            '表示位置
            OrgWebForm.Top = 0
            OrgWebForm.Left = DisplayWidth - OrgWebFormWidth

            '大きさ
            OrgWebForm.Width = OrgWebFormWidth
            OrgWebForm.Height = DisplayHeight

            'OrgWebFormを一番上
            OrgWebForm.TopMost = True

        Else

            'OrgWebFormが存在していなければ、メインフォームをトップに
            JKDeskTopSearch.TopMost = True
            JKDeskTopSearch.Select()
            JKDeskTopSearch.TmpTextBox.Focus()
        End If
    End Sub
#End Region

#Region "ブラウザでの動作"
    '*****別ウインドウURLに飛ぶものをフックする
    Private Sub WebBrowser_NewWindow2(ByVal sender As Object, ByVal e As WebBrowserNewWindow2EventArgs)

        '一番上に表示
        Me.TopMost = True

        'WebBrowser1の定義
        Me.WebBrowser1 = New ExWebBrowser
        Me.WebBrowser1.Dock = DockStyle.Fill
        AddHandler Me.WebBrowser1.NewWindow2, AddressOf WebBrowser_NewWindow2

        'TabPage1を生成し、その上にWebBrowser1を追加
        Me.TabPage1 = New TabPage
        Me.TabPage1.Controls.Add(Me.WebBrowser1)

        'TabControl1（つまむ部分）にTabPage1を追加
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.SelectedTab = Me.TabPage1

        '開かれたタブが一番上になる
        Me.TabControl1.BringToFront()


        '新しいウィンドウが開くのを抑制
        e.ppDisp = Me.WebBrowser1.Application
        Me.WebBrowser1.RegisterAsBrowser = True

        '一番上に表示を停止
        Me.TopMost = False

    End Sub

    '******ブラウザ表示内容の編集
    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted

        '水平スクロールバーを非表示/縦スクロールバーのカラーのセット
        Me.WebBrowser1.Document.Body.Style = "overflow-x:hidden;scrollbar-base-color: #DA7C0C;scrollbar-face-color: #F78D1D;scrollbar-highlight-color: #fff;scrollbar-arrow-color: #fff;scrollbar-darkshadow-color:#666;"

        '新しく開くタブに名前を付ける
        Dim Document As HtmlDocument = Me.WebBrowser1.Document
        Dim HtmlElements As HtmlElementCollection = Document.GetElementsByTagName("title")

        'HtmlElementsは複数あっても一番最初のものをタブのタイトルとして取得し表示
        'ただし「 ：ジャパンナレッジ」は不要
        Me.TabPage1.Text = HtmlElements.Item(0).InnerText.Replace(" ：ジャパンナレッジ", "") & "　"


        'id=closeAreaを本学の「技術提供」表示に置き換えます
        '******************************************************************************************************
        '★★重要★★
        '下記の「技術提供：佛教大学図書館」という文字列は、本ソフトウェアおよび二次著作物の利用許諾契約に基づき、
        '検索結果の画面上に明示的に表記される必要があります。従いまして文言の変更、消去はご遠慮ください。
        'ただし「Powered by BUKKYO UNIVERSITY LIBRALY」という文字列への変更は可能です
        '******************************************************************************************************
        Dim idElement As HtmlElement = WebBrowser1.Document.GetElementById("closeArea")
        If idElement IsNot Nothing Then
            idElement.InnerText = "技術提供：佛教大学図書館"
            idElement.Style = "font-size: 75%;"
        End If

        'id=naviMarginFrameを非表示
        Dim idElement2 As HtmlElement = WebBrowser1.Document.GetElementById("naviMarginFrame")
        If idElement2 IsNot Nothing Then idElement2.InnerText = ""

        ''id=naviContainBaseを非表示
        Dim idElement3 As HtmlElement = WebBrowser1.Document.GetElementById("naviContainBase")
        If idElement3 IsNot Nothing Then idElement3.InnerText = ""

        ''id=navSideAreaを非表示
        Dim idElement4 As HtmlElement = WebBrowser1.Document.GetElementById("naviSideArea")
        If idElement4 IsNot Nothing Then idElement4.InnerText = ""

        'id=headerFieldを非表示
        Dim idElement5 As HtmlElement = WebBrowser1.Document.GetElementById("headerField")
        If idElement5 IsNot Nothing Then idElement5.InnerText = ""

        '*****日本歴史地名体系の場合
        'クラス名で指定td class="leftSideArea
        Dim AllElements As HtmlElementCollection = WebBrowser1.Document.GetElementsByTagName("td")

        'tdの数が0以上でないとエラーがでる
        If AllElements.Count > 0 Then

            Dim AllElement As HtmlElement
            For Each AllElement In AllElements

                'NameStrを各エレメントで取得する
                Dim NameStr As String = AllElement.GetAttribute("className")
                If ((NameStr IsNot Nothing) And (NameStr.Length <> 0)) Then

                    'OuterHtmlがエラーを生じさせるのでInnertextで削除
                    If NameStr.ToLower().Equals("rightsidearea") Or NameStr.ToLower().Equals("leftsidearea") Then
                        AllElement.InnerText = ""
                    End If

                End If

            Next
        End If


        '右クリックでのToolStripMenuItemメニュー内容の定義
        '読み込んだ後に履歴があるかを判定
        '履歴に前のページがある場合
        If Me.WebBrowser1.CanGoBack Then

            '前のページに戻るを表示
            Me.戻るToolStripMenuItem.Enabled = True
        Else

            '前のページに戻るを非表示
            Me.戻るToolStripMenuItem.Enabled = False
        End If

        '履歴に次のページがある場合
        If Me.WebBrowser1.CanGoForward Then

            '次のページに進むを表示
            Me.進むToolStripMenuItem.Enabled = True
        Else

            '次のページに進むを非表示
            Me.進むToolStripMenuItem.Enabled = False
        End If
    End Sub



#End Region

#Region "ToolStripMenuItem"

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged

        'globalHooks.InstallHooks()

        'タブをクリックするたびにTabPage1,WebBrowser1を選択されたタブ内に変更
        Dim SelectTabIndex As Short = Me.TabControl1.SelectedIndex

        If SelectTabIndex < 0 Then
            Exit Sub
        End If

        'Tabpage1の再設定
        Me.TabPage1 = Me.TabControl1.TabPages.Item(SelectTabIndex)

        'WebBrowserの再設定
        Me.WebBrowser1 = Me.TabPage1.Controls(0)

        '読み込んだ後に履歴があるかを判定
        '履歴に前のページがある場合
        If Me.WebBrowser1.CanGoBack Then

            '前のページに戻るを表示
            Me.戻るToolStripMenuItem.Enabled = True
        Else

            '前のページに戻るを非表示
            Me.戻るToolStripMenuItem.Enabled = False
        End If

        '履歴に次のページがある場合
        If Me.WebBrowser1.CanGoForward Then

            '次のページに進むを表示
            Me.進むToolStripMenuItem.Enabled = True
        Else

            '次のページに進むを非表示
            Me.進むToolStripMenuItem.Enabled = False
        End If
    End Sub

    Private Sub タブを閉じるToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles タブを閉じるToolStripMenuItem.Click

        If Me.TabControl1.TabPages.Count <= 1 Then

            'タブページが1枚しかないとき
            Me.Close()
        Else

        End If
    End Sub

    Private Sub 前のページに戻るToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 戻るToolStripMenuItem.Click

        '履歴に前のページがある場合
        If Me.WebBrowser1.CanGoBack Then

            '前のページに戻る
            Me.WebBrowser1.GoBack()
        End If
    End Sub

    Private Sub 次のページへ進むToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 進むToolStripMenuItem.Click

        '履歴に次のページがある場合
        If Me.WebBrowser1.CanGoForward Then

            '次のページに戻る
            Me.WebBrowser1.GoForward()
        End If
    End Sub
#End Region

    Private Sub TabControl1_TabClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TabControl1.TabClosing

        If Me.TabControl1.TabPages.Count <= 1 Then

            'タブページが1枚しかないとき
            Me.Close()
        Else
            Dim DelTabIndex As Short = e.TabPageIndex

            'TabPage1が削除によりロストしたので再設定
            '削除したタブが一番最初に開いたもののとき(DelTabIndexが0のとき)
            If DelTabIndex = 0 Then

                'Deltabindexが0の時は新たな0となるページをTabPage1に設定
                Me.TabPage1 = Me.TabControl1.TabPages.Item(DelTabIndex)
            Else

                'DelTabIndexが0でないときは、ひとつ前のページをTabPage1に設定
                Me.TabPage1 = Me.TabControl1.TabPages.Item(DelTabIndex - 1)
            End If

            'WebBrowser1が削除によりロストしたので再設定
            Me.WebBrowser1 = Me.TabPage1.Controls(0)

            '右クリック抑制
            Me.WebBrowser1.IsWebBrowserContextMenuEnabled = False

            'TabPage1を一番上に表示
            Me.TabControl1.SelectedTab = TabPage1
        End If

    End Sub
End Class

