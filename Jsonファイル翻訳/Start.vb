
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Newtonsoft.Json

Imports System.Text.Json
Public Class Start
    Public endpoint As String = ""
    Public key As String = ""
    Public location As String = ""
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form2.Show()
        Me.Hide()
    End Sub






    Private Sub Start_Load(sender As Object, e As EventArgs) Handles Me.Load
        Console.WriteLine（Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)）
        Dim strPath As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        'INIファイルクラスの生成
        Dim Ini As New ClsIni(strPath & "\マイクラMOD翻訳ソフト.ini")

        'INIファイルからの取得テスト
        Dim str As String
        str = Ini.GetProfileString("Settings", "Key", "")
        key = str.ToString
        str = Ini.GetProfileString("Settings", "Endpoint", "")
        endpoint = str.ToString
        str = Ini.GetProfileString("Settings", "location", "")
        location = str.ToString

        If key = "" AndAlso endpoint = "" AndAlso location = "" Then
            MsgBox（"設定を開いてAzure 翻訳APIと、 そのエンドポイント及び場所を" & "入力してください。" & vbCrLf & "無料版もあります。詳しくはご自分で調べてください。"）
            Label4.Height = 161
            Label4.Text = "設定を開いてAzure 翻訳APIと、そのエンドポイント及び場所を入力してください。" & vbCrLf & "無料版もあります。詳しくはご自分で調べてください。"
        ElseIf key = "" Or endpoint = "" Or location = "" Then
            MsgBox（"設定を開いてAzure 翻訳APIと、そのエンドポイント及び場所を入力してください。" & vbCrLf & "すべて入力されていないと使用できません。"）
            Label4.Height = 161
            Label4.Text = "設定を開いてAzure 翻訳APIと、そのエンドポイント及び場所を入力してください。" & vbCrLf & "すべて入力されていないと使用できません。"
        Else
            Label4.Text = "APIなどが正しいものであれば使用できます。"
            Label4.Height = 32
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form3.Show()
    End Sub

    Private Sub Start_Activated(sender As Object, e As EventArgs) Handles Me.Activated

    End Sub
End Class



Public Class ClsIni
    'プロファイル文字列取得
    'Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" ( _
    '   ByVal lpApplicationName As String, _
    '   ByVal lpKeyName As String, _
    '   ByVal lpDefault As String, _
    '   ByVal lpReturnedString As System.Text.StringBuilder, _
    '   ByVal nSize As UInt32, _
    '   ByVal lpFileName As String) As UInt32
    '宣言修正
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (
        <MarshalAs(UnmanagedType.LPStr)> ByVal lpApplicationName As String,
        <MarshalAs(UnmanagedType.LPStr)> ByVal lpKeyName As String,
        <MarshalAs(UnmanagedType.LPStr)> ByVal lpDefault As String,
        <MarshalAs(UnmanagedType.LPStr)> ByVal lpReturnedString As StringBuilder,
        ByVal nSize As UInt32,
        <MarshalAs(UnmanagedType.LPStr)> ByVal lpFileName As String) As UInt32

    'プロファイル文字列書込み
    'Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" ( _
    '    ByVal lpAppName As String, _
    '    ByVal lpKeyName As String, _
    '    ByVal lpString As String, _
    '    ByVal lpFileName As String) As Integer
    '宣言修正
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (
        <MarshalAs(UnmanagedType.LPStr)> ByVal lpAppName As String,
        <MarshalAs(UnmanagedType.LPStr)> ByVal lpKeyName As String,
        <MarshalAs(UnmanagedType.LPStr)> ByVal lpString As String,
        <MarshalAs(UnmanagedType.LPStr)> ByVal lpFileName As String) As Integer

    Private strIniFileName As String = ""

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="strIniFile">INIファイル名(フルパス)</param>
    Sub New(ByVal strIniFile As String)
        Me.strIniFileName = strIniFile  'ファイル名退避
    End Sub

    ''' <summary>
    ''' プロファイル文字列取得
    ''' </summary>
    ''' <param name="strAppName">アプリケーション文字列</param>
    ''' <param name="strKeyName">キー文字列</param>
    ''' <param name="strDefault">デフォルト文字列</param>
    ''' <returns>プロファイル文字列</returns>
    Public Function GetProfileString(ByVal strAppName As String,
                                     ByVal strKeyName As String,
                                     ByVal strDefault As String) As String
        Try
            Dim strWork As System.Text.StringBuilder = New System.Text.StringBuilder(1024)
            Dim intRet As Integer = GetPrivateProfileString(strAppName, strKeyName,
                                                                       strDefault, strWork,
                                                                       strWork.Capacity - 1, strIniFileName)
            If intRet > 0 Then
                'エスケープ文字を解除して返す
                Return ResetEscape(strWork.ToString())
            Else
                Return strDefault
            End If
        Catch ex As Exception
            Return strDefault
        End Try
    End Function

    ''' <summary>
    ''' プロファイル文字列設定
    ''' </summary>
    ''' <param name="strAppName">アプリケーション文字列</param>
    ''' <param name="strKeyName">キー文字列</param>
    ''' <param name="strSet">設定文字列</param>
    ''' <returns>True:正常, False:エラー</returns>
    Public Function WriteProfileString(ByVal strAppName As String,
                                       ByVal strKeyName As String,
                                       ByVal strSet As String) As Boolean
        Try
            'エスケープ文字変換
            Dim strCnv As String = SetEscape(strSet)
            Dim intRet As Integer = WritePrivateProfileString(strAppName, strKeyName, strCnv, strIniFileName)
            If intRet > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' エスケープ文字変換
    ''' </summary>
    ''' <param name="strSet">設定文字列</param>
    ''' <returns>変換後文字列</returns>
    Private Function SetEscape(ByVal strSet As String) As String
        Dim strEscape As String = ";#=:"
        Dim strRet As String = strSet
        Try
            For i = 0 To strEscape.Length - 1
                Dim str As String = strEscape.Substring(i, 1)
                strRet = strRet.Replace(str, "\" & str)
            Next
            Return strRet
        Catch ex As Exception
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' エスケープ文字解除
    ''' </summary>
    ''' <param name="strSet">設定文字列</param>
    ''' <returns>変換後文字列</returns>
    Private Function ResetEscape(ByVal strSet As String) As String
        Dim strEscape As String = ";#=:"
        Dim strRet As String = strSet
        Try
            For i = 0 To strEscape.Length - 1
                Dim str As String = strEscape.Substring(i, 1)
                strRet = strRet.Replace("\" & str, str)
            Next
            Return strRet
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class

