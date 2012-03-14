'**************
'"JKDeskTopSearch" Powered by Bukkyo University Library
'本モジュールについてJapanKnowledge会員、および佛教大学図書館が許可する業者以外への公開を禁じます。
'**************

'プロセス間通信のための共有クラス
Public Class HookClientToMainServerEvent
    '二つのプロセス間で共有
    Inherits MarshalByRefObject

    'クライアントからの文字列を受信し、サーバ側にイベントを発生させる
    Public Sub RaiseServerEvent(ByVal Message As String)
        RaiseEvent RaiseClientEvent(Message)
    End Sub
    Public Event RaiseClientEvent(ByVal Messsage As String)
End Class


