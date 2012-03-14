'**************
'"JKDeskTopSearch" Powered by Bukkyo University Library
'本プログラムについてJapanKnowledge会員、および佛教大学図書館が許可する業者以外への公開を禁じます。
'**************

'Win32API.vbからユーザ定義関数と定数をインポート
Imports JKWin32HookAndCom.Win32API
Imports JKWin32HookAndCom.Win32API.WM

'*****WebBrowser上でマウスのクリックイベントを取得するためのオリジナルクラス
Public Class BrowserMouseClick
    Inherits NativeWindow

    Public Event MouseDown As MouseEventHandler

    Sub New(ByVal WebBrowser As WebBrowser)
        AddHandler WebBrowser.HandleCreated, AddressOf OnHandleCreated
        AddHandler WebBrowser.HandleDestroyed, AddressOf OnHandleDestroyed
        Me.AssignHandle(WebBrowser.Handle)
    End Sub

    Private Sub OnHandleCreated(ByVal sender As Object, ByVal e As EventArgs)
        Me.AssignHandle(DirectCast(sender, WebBrowser).Handle)
    End Sub

    Private Sub OnHandleDestroyed(ByVal sender As Object, ByVal e As EventArgs)
        Me.ReleaseHandle()
    End Sub

    Overridable Sub OnMouseDown(ByVal e As MouseEventArgs)
        RaiseEvent MouseDown(Me, e)
    End Sub


    '*****以下、右ボタン・左ボタンなどで区別をつける場合と座標を取得する場合に使用
    '16進数の値変換
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

        If m.Msg = WM_PARENTNOTIFY Then
            Select Case LOWORD(m.WParam.ToInt32)
                Case WM_LBUTTONDOWN
                    Dim e As New MouseEventArgs(MouseButtons.Left, 1, LOWORD(m.LParam.ToInt32), HIWORD(m.LParam.ToInt32), 0)
                    OnMouseDown(e)
                Case WM_RBUTTONDOWN
                    Dim e As New MouseEventArgs(MouseButtons.Right, 1, LOWORD(m.LParam.ToInt32), HIWORD(m.LParam.ToInt32), 0)
                    OnMouseDown(e)
                Case WM_MBUTTONDOWN
                    Dim e As New MouseEventArgs(MouseButtons.Middle, 1, LOWORD(m.LParam.ToInt32), HIWORD(m.LParam.ToInt32), 0)
                    OnMouseDown(e)
                Case WM_XBUTTONDOWN
                    Select Case HIWORD(m.WParam.ToInt32)
                        Case XBUTTON.XBUTTON1
                            Dim e As New MouseEventArgs(MouseButtons.XButton1, 1, LOWORD(m.LParam.ToInt32), HIWORD(m.LParam.ToInt32), 0)
                            OnMouseDown(e)
                        Case XBUTTON.XBUTTON2
                            Dim e As New MouseEventArgs(MouseButtons.XButton2, 1, LOWORD(m.LParam.ToInt32), HIWORD(m.LParam.ToInt32), 0)
                            OnMouseDown(e)
                    End Select
            End Select
        End If
        MyBase.WndProc(m)
    End Sub

    Private Enum XBUTTON As Integer
        XBUTTON1 = &H1
        XBUTTON2 = &H2
    End Enum

End Class
