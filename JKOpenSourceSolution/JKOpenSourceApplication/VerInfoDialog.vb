'**************
'"JKDeskTopSearch" Powered by Bukkyo University Library
'本プログラムについてJapanKnowledge会員、および佛教大学図書館が許可する業者以外への公開を禁じます。
'**************

Imports System.Reflection
Imports System.Runtime.InteropServices

Public Class VerInfoDialog
    Inherits System.Windows.Forms.Form

#Region " Windows フォーム デザイナで生成されたコード "

    Public Sub New()
        MyBase.New()

        ' この呼び出しは Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後に初期化を追加します。

    End Sub



    ' Windows フォーム デザイナで必要です。


    ' メモ : 以下のプロシージャは、Windows フォーム デザイナで必要です。
    ' Windows フォーム デザイナを使って変更してください。  
    ' コード エディタは使用しないでください。
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents button1 As System.Windows.Forms.Button
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents pictureBox1 As System.Windows.Forms.PictureBox
    

#End Region

#Region "フォームのロードとクローズ"

    Public Sub VerInfoDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'OrgWebFormが存在していれば、それを一時的に隠す
        If Application.OpenForms("OrgWebForm") IsNot Nothing Then OrgWebForm.Hide()

        '表示位置
        Me.Left = DisplayWidth - OrgWebFormWidth

        'バージョン名（AssemblyInformationalVersionAttribute属性）を取得
        Dim AppVersion As String = Application.ProductVersion

        '製品名（AssemblyProductAttribute属性）を取得
        Dim AppProductName As String = Application.ProductName

        '会社・学校名（AssemblyCompanyAttribute属性）を取得
        Dim AppCompanyName As String = Application.CompanyName

        'アセンブリから直接取得
        Dim MainAssembly As [Assembly] = [Assembly].GetEntryAssembly()

        'コピーライト情報を取得
        Dim AppCopyright As String = "-"
        Dim CopyrightArray() As Object = MainAssembly.GetCustomAttributes(GetType(AssemblyCopyrightAttribute), False)
        If Not (CopyrightArray Is Nothing) AndAlso (CopyrightArray.Length > 0) Then
            AppCopyright = (CType(CopyrightArray(0), AssemblyCopyrightAttribute)).Copyright
        End If

        '詳細情報を取得
        Dim AppDescription As String = "-"
        Dim DescriptionArray() As Object = MainAssembly.GetCustomAttributes(GetType(AssemblyDescriptionAttribute), False)
        If Not (DescriptionArray Is Nothing) AndAlso (DescriptionArray.Length > 0) Then
            AppDescription = (CType(DescriptionArray(0), AssemblyDescriptionAttribute)).Description
        End If

        'JKアイコンについてはEXEファイルから直接取得（Win32API使用） 
        '本学アイコンについては組み込み済み
        'アプリケーション・アイコンを取得
        Dim AppIcon As Icon
        Dim Shinfo As New SHFILEINFO()
        Dim HSuccess As IntPtr = SHGetFileInfo(MainAssembly.Location, 0, Shinfo, Marshal.SizeOf(Shinfo), SHGFI_ICON Or SHGFI_LARGEICON)
        If HSuccess.Equals(IntPtr.Zero) = False Then
            AppIcon = Icon.FromHandle(Shinfo.hIcon)
        Else
            AppIcon = SystemIcons.Application
        End If

        pictureBox1.Image = AppIcon.ToBitmap()

        'ラベルにバージョン情報をセット
        Text = AppProductName & " のバージョン情報"
        label1.Text = AppCompanyName & " " & AppProductName
        label2.Text = "Version " & AppVersion
        label3.Text = AppCopyright
        label4.Text = AppDescription
    End Sub

    '*****クローズボタンを押したとき
    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click

        'OrgWebFormがHide状態で存在していれば、それを再び表示
        If Application.OpenForms("OrgWebForm") IsNot Nothing Then OrgWebForm.Show()

        Me.Close()
    End Sub
#End Region

#Region "Win32 API関係"

    ' SHGetFileInfo関数
    Private Declare Ansi Function SHGetFileInfo Lib "shell32.dll" (ByVal pszPath As String, ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As IntPtr

    ' SHGetFileInfo関数で使用するフラグ
    Private Const SHGFI_ICON As Integer = &H100 ' アイコン・リソースの取得
    Private Const SHGFI_LARGEICON As Integer = &H0 ' 大きいアイコン
    Private Const SHGFI_SMALLICON As Integer = &H1 ' 小さいアイコン

    ' SHGetFileInfo関数で使用する構造体
    Private Structure SHFILEINFO
        Public hIcon As IntPtr
        Public iIcon As IntPtr
        Public dwAttributes As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> _
        Public szDisplayName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)> _
        Public szTypeName As String
    End Structure

#End Region

End Class
