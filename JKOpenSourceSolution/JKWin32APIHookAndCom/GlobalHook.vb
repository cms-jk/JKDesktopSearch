'**************
'"JKDeskTopSearch" Powered by Bukkyo University Library
'本モジュールについてJapanKnowledge会員、および佛教大学図書館が許可する業者以外への公開を禁じます。
'**************
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Class GlobalHook
    Implements IDisposable


#Region "パブリックイベントの定義"

    Public Event MouseDown As MouseEventHandler
    Public Event MouseMove As MouseEventHandler
    Public Event MouseUp As MouseEventHandler
    Public Event MouseWheel As MouseEventHandler

    Public Event KeyDown As KeyEventHandler
    Public Event KeyPress As KeyPressEventHandler
    Public Event KeyUp As KeyEventHandler

#End Region

#Region "アンマネージ リソースの解放"

    '解放検出パラメータ
    Private disposed As Boolean = False

    ' *****IDisposable(アンマネージ リソースの解放)
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then
            If disposing Then

            End If

            'グローバルフックの停止
            Me.RemoveHooks()
        End If
        Me.disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements System.IDisposable.Dispose
        Me.Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "イベントのトリガー"

    Protected Overridable Sub OnMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        If Not MouseDownEvent Is Nothing Then
            RaiseEvent MouseDown(sender, e)
        End If
    End Sub

    Protected Overridable Sub OnMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
        If Not MouseMoveEvent Is Nothing Then
            RaiseEvent MouseMove(sender, e)
        End If
    End Sub

    Protected Overridable Sub OnMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        If Not MouseUpEvent Is Nothing Then
            RaiseEvent MouseUp(sender, e)
        End If
    End Sub

    Protected Overridable Sub OnMouseWheel(ByVal sender As Object, ByVal e As MouseEventArgs)
        If Not MouseWheelEvent Is Nothing Then
            RaiseEvent MouseWheel(sender, e)
        End If
    End Sub

    Protected Overridable Sub OnKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If Not KeyDownEvent Is Nothing Then
            For Each handler As KeyEventHandler In KeyDownEvent.GetInvocationList
                handler.Invoke(Me, e)
                If e.Handled Then
                    Exit For
                End If
            Next
        End If
    End Sub

    Protected Overridable Sub OnKeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
        If Not KeyUpEvent Is Nothing Then
            For Each handler As KeyEventHandler In KeyUpEvent.GetInvocationList
                handler.Invoke(Me, e)
                If e.Handled Then
                    Exit For
                End If
            Next
        End If
    End Sub

    Protected Overridable Sub OnKeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If Not KeyPressEvent Is Nothing Then
            For Each handler As KeyPressEventHandler In KeyPressEvent.GetInvocationList
                handler.Invoke(Me, e)
                If e.Handled Then
                    Exit For
                End If
            Next
        End If
    End Sub

#End Region

#Region "変数定義"

    Private Shared hMouseHook As Integer = 0
    Private Shared hKeyboardHook As Integer = 0

    Private MouseHookProcedure As Win32API.HookProc
    Private KeyboardHookProcedure As Win32API.HookProc

#End Region

#Region "グローバルフックのインストールとアンインストール"

    Public Sub InstallHooks()
        If hMouseHook = 0 Then
            MouseHookProcedure = New Win32API.HookProc(AddressOf MouseHookProc)

            hMouseHook = Win32API.SetWindowsHookEx( _
                Win32API.WH.WH_MOUSE_LL, _
                MouseHookProcedure, _
                Marshal.GetHINSTANCE(Reflection.Assembly.GetExecutingAssembly().GetModules()(0)), _
                0)

            If hMouseHook = 0 Then
                RemoveHooks()
                Throw New Exception("SetWindowsHookExの実行に失敗しました")
            End If
        End If

        If hKeyboardHook = 0 Then ' install Keyboard hook 
            KeyboardHookProcedure = New Win32API.HookProc(AddressOf KeyboardHookProc)
            hKeyboardHook = Win32API.SetWindowsHookEx( _
                Win32API.WH.WH_KEYBOARD_LL, _
                KeyboardHookProcedure, _
                Marshal.GetHINSTANCE(Reflection.Assembly.GetExecutingAssembly().GetModules()(0)), _
                0)

            If (hKeyboardHook = 0) Then 'SetWindowsHookEx failed
                RemoveHooks()
                Throw New Exception("SetWindowsHookExの実行に失敗しました")
            End If
        End If
    End Sub

    Public Sub RemoveHooks()
        Dim mouseResult As Boolean = True
        Dim keyboardResult As Boolean = True

        If hMouseHook <> 0 Then
            mouseResult = Win32API.UnhookWindowsHookEx(hMouseHook)
            hMouseHook = 0
        End If

        If hKeyboardHook <> 0 Then
            keyboardResult = Win32API.UnhookWindowsHookEx(hKeyboardHook)
            hKeyboardHook = 0
        End If

        If Not (mouseResult AndAlso keyboardResult) Then
            Throw New Exception("UnhookWindowsHookExの実行に失敗しました")
        End If
    End Sub

#End Region

#Region "マウスクリックを取得するユーザ定義関数"

    '*****マウスボタンを押したときにフラグを立てる
    Private Shared Function GetMouseButtonFlag(ByVal wParam As Integer, ByVal hiWord As Integer) As MouseButtons

        Select Case (wParam)

            Case _
                Win32API.WM.WM_LBUTTONDOWN, Win32API.WM.WM_LBUTTONUP, Win32API.WM.WM_LBUTTONDBLCLK
                Return MouseButtons.Left

            Case _
                Win32API.WM.WM_MBUTTONDOWN, Win32API.WM.WM_MBUTTONUP, Win32API.WM.WM_MBUTTONDBLCLK
                Return MouseButtons.Middle

            Case _
                Win32API.WM.WM_RBUTTONDOWN, Win32API.WM.WM_RBUTTONUP, Win32API.WM.WM_RBUTTONDBLCLK
                Return MouseButtons.Right

            Case _
                Win32API.WM.WM_XBUTTONDOWN, Win32API.WM.WM_XBUTTONUP, Win32API.WM.WM_XBUTTONDBLCLK, Win32API.WM.WM_NCXBUTTONDOWN, _
                Win32API.WM.WM_NCXBUTTONUP,Win32API.WM.WM_NCXBUTTONDBLCLK

                If hiWord = 1 Then
                    Return MouseButtons.XButton1
                ElseIf hiWord = 2 Then
                    Return MouseButtons.XButton2
                End If
        End Select
        Return MouseButtons.None

    End Function

    '******スクロール情報を取得する
    Private Shared Function GetDelta(ByVal wParam As Integer, ByVal hiWord As Integer) As Integer

        If wParam = Win32API.WM.WM_MOUSEWHEEL Then
            Return hiWord
        Else
            Return 0
        End If
    End Function

    '*******ダブルクリックかシングルクリックかを区別する
    Private Shared Function GetClickCount(ByVal wParam As Integer, ByVal buttons As MouseButtons) As Integer

        If buttons <> MouseButtons.None Then
            Select Case wParam
                Case _
                    Win32API.WM.WM_LBUTTONDBLCLK, Win32API.WM.WM_MBUTTONDBLCLK, Win32API.WM.WM_RBUTTONDBLCLK,Win32API.WM.WM_XBUTTONDBLCLK
                    Return 2
                Case Else
                    Return 1
            End Select
        End If
        Return 0

    End Function

    '*******マウスクリックイベント（基本クラス）の作成
    Private Shared Function CreateMouseEventArgs(ByVal wParam As Integer, ByVal lParam As IntPtr) As MouseEventArgs

        Dim ClickCount As Integer
        Dim Buttons As MouseButtons
        Dim Mhs As New Win32API.MSLLHOOKSTRUCT
        Dim HiWord As Integer
        Dim Delta As Integer

        Marshal.PtrToStructure(lParam, Mhs)
        HiWord = Win32API.HIWORD(Mhs.mouseData)
        Buttons = GetMouseButtonFlag(wParam, HiWord)
        Delta = GetDelta(wParam, HiWord)
        ClickCount = GetClickCount(wParam, Buttons)

        Return New MouseEventArgs(Buttons, ClickCount, Mhs.pt.X, Mhs.pt.Y, Delta)

    End Function

    '********マウスの動作をフックする関数
    Private Function MouseHookProc(ByVal nCode As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As Integer
        If nCode >= 0 Then
            Select Case wParam
                Case _
                    Win32API.WM.WM_LBUTTONDOWN, Win32API.WM.WM_LBUTTONDBLCLK, Win32API.WM.WM_MBUTTONDOWN, Win32API.WM.WM_MBUTTONDBLCLK, _
                    Win32API.WM.WM_RBUTTONDOWN, Win32API.WM.WM_RBUTTONDBLCLK, Win32API.WM.WM_XBUTTONDOWN, Win32API.WM.WM_XBUTTONDBLCLK

                    'マウスイベントを取得できないとき
                    If Not MouseDownEvent Is Nothing Then
                        Dim e As MouseEventArgs = CreateMouseEventArgs(wParam, lParam)
                        OnMouseDown(Me, e)
                    End If

                Case _
                    Win32API.WM.WM_LBUTTONUP, Win32API.WM.WM_MBUTTONUP, Win32API.WM.WM_RBUTTONUP, Win32API.WM.WM_XBUTTONUP

                    'マウスイベントを取得できないとき
                    If Not MouseUpEvent Is Nothing Then
                        Dim e As MouseEventArgs = CreateMouseEventArgs(wParam, lParam)
                        OnMouseUp(Me, e)
                    End If

                Case Win32API.WM.WM_MOUSEWHEEL

                    'マウスイベントを取得できないとき
                    If Not MouseWheelEvent Is Nothing Then
                        Dim e As MouseEventArgs = CreateMouseEventArgs(wParam, lParam)
                        OnMouseWheel(Me, e)
                    End If

                Case Win32API.WM.WM_MOUSEMOVE

                    'マウスイベントを取得できないとき
                    If Not MouseMoveEvent Is Nothing Then
                        Dim e As MouseEventArgs = CreateMouseEventArgs(wParam, lParam)
                        OnMouseMove(Me, e)
                    End If

            End Select
        End If

        Return Win32API.CallNextHookEx(hMouseHook, nCode, wParam, lParam)
    End Function

#End Region

#Region "キーの動作を取得するユーザ定義関数"

    '*******キーイベント（基本クラス）の作成
    Private Function CreateKeyEventArgs(ByVal khs As Win32API.KeyboardHookStruct) As KeyEventArgs
        Dim ctrl As Keys
        If Win32API.IsKeyDown(Keys.ControlKey) Then
            ctrl = Keys.Control
        End If
        Dim alt As Keys
        If Win32API.IsKeyDown(Keys.Menu) Then
            alt = Keys.Alt
        End If
        Dim shift As Keys
        If Win32API.IsKeyDown(Keys.ShiftKey) Then
            shift = Keys.Shift
        End If

        Dim keyCode As Keys = CType(khs.vkCode, Keys)
        Dim e As New KeyEventArgs(keyCode Or ctrl Or alt Or shift)
        Return e
    End Function

    '*******キープレスイベントの取得
    Private Function FireKeyPress(ByVal wParam As Integer, ByVal khs As Win32API.KeyboardHookStruct, ByVal e As KeyEventArgs) As Boolean
        If wParam = Win32API.WM.WM_SYSKEYDOWN Then
            Return False
        Else
            Dim inBuffer(2) As Byte
            Dim keyState(256) As Byte
            Win32API.GetKeyboardState(keyState)

            If Win32API.ToAscii(khs.vkCode, khs.scanCode, keyState, inBuffer, khs.flags) > 0 Then
                Dim args As New KeyPressEventArgs(BitConverter.ToChar(inBuffer, 0))
                OnKeyPress(Me, args)
                Return e.Handled
            End If
        End If
        Return False
    End Function

    '*******キーダウンイベントの取得
    Private Sub CheckForKeyDown(ByVal wParam As Integer, ByVal lParam As IntPtr, ByRef handled As Boolean, ByRef khs As Win32API.KeyboardHookStruct)
        If Not KeyDownEvent Is Nothing Then
            khs = New Win32API.KeyboardHookStruct
            Marshal.PtrToStructure(lParam, khs)

            Dim e As KeyEventArgs = CreateKeyEventArgs(khs)
            Me.OnKeyDown(Me, e)

            handled = e.Handled

            If Not handled Then
                handled = FireKeyPress(wParam, khs, e)
            End If
        End If
    End Sub

    '*******キーアップイベントの取得
    Private Sub CheckForKeyUp(ByVal wParam As Integer, ByVal lParam As IntPtr, ByRef handled As Boolean, ByVal khs As Win32API.KeyboardHookStruct)
        If Not KeyUpEvent Is Nothing Then
            If khs Is Nothing Then
                khs = New Win32API.KeyboardHookStruct
                Marshal.PtrToStructure(lParam, khs)
            End If

            Dim keyData As Keys = CType(khs.vkCode, Keys)
            Dim e As New KeyEventArgs(keyData)
            OnKeyUp(Me, e)
            handled = e.Handled
        End If
    End Sub

    '******キー動作のフック
    Private Function KeyboardHookProc(ByVal nCode As Integer, ByVal wParam As Integer,ByVal lParam As IntPtr) As Integer

        Dim handled As Boolean = False

        If nCode >= 0 Then
            'Nullに注意
            Dim khs As Win32API.KeyboardHookStruct

            If wParam = Win32API.WM.WM_KEYDOWN OrElse wParam = Win32API.WM.WM_SYSKEYDOWN Then
                CheckForKeyDown(wParam, lParam, handled, khs)
            ElseIf wParam = Win32API.WM.WM_KEYUP OrElse wParam = Win32API.WM.WM_SYSKEYUP Then
                CheckForKeyUp(wParam, lParam, handled, khs)
            End If
        End If

        If handled Then
            Return -1
        Else
            Return Win32API.CallNextHookEx(hKeyboardHook, nCode, wParam, lParam)
        End If
    End Function
#End Region

End Class
