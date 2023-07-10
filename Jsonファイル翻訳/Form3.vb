Imports System.Reflection.Emit

Public Class Form3
    Private endpoint As String = Start.endpoint
    Private key As String = Start.key
    Private location As String = Start.location
    Private DeepLKey As String = Start.DeepLKey

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '自分自身の存在するフォルダ
        Dim strPath As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        'INIファイルクラスの生成
        Dim Ini As New ClsIni(strPath & "\マイクラMOD翻訳ソフト.ini")
        'INIファイルへの書込みテスト
        Ini.WriteProfileString("Settings", "Key", TextBox1.Text)
        Ini.WriteProfileString("Settings", "Endpoint", TextBox2.Text)
        Ini.WriteProfileString("Settings", "location", TextBox3.Text)
        Ini.WriteProfileString("Settings", "DeepLKey", TextBox4.Text)

        'INIファイルからの取得テスト
        Dim str As String
        str = Ini.GetProfileString("Settings", "Key", "")
        key = str.ToString
        str = Ini.GetProfileString("Settings", "Endpoint", "")
        endpoint = str.ToString
        str = Ini.GetProfileString("Settings", "location", "")
        location = str.ToString
        str = Ini.GetProfileString("Settings", "DeepLKey", "")
        DeepLKey = str.ToString
        Me.Close()
        If DeepLKey <> "" And key <> "" And endpoint <> "" And location <> "" Then
            Start.Label4.Text = "無料版DeepL・Azure翻訳 有効"
            Start.Label4.Height = 32
        ElseIf DeepLKey = "" And key <> "" And endpoint <> "" And location <> "" Then
            Start.Label4.Text = "Azure翻訳 有効"
            Start.Label4.Height = 32
        ElseIf DeepLKey <> "" And key = "" And endpoint <> "" And location <> "" Then
            Start.Label4.Text = "無料版DeepL有効"
            Start.Label4.Height = 32
        ElseIf DeepLKey <> "" And key <> "" And endpoint = "" And location <> "" Then
            Start.Label4.Text = "無料版DeepL有効"
            Start.Label4.Height = 32
        ElseIf DeepLKey <> "" And key <> "" And endpoint <> "" And location = "" Then
            Start.Label4.Text = "無料版DeepL有効"
            Start.Label4.Height = 32
        ElseIf DeepLKey <> "" And key = "" And endpoint = "" And location <> "" Then
            Start.Label4.Text = "無料版DeepL有効"
            Start.Label4.Height = 32
        ElseIf DeepLKey <> "" And key = "" And endpoint <> "" And location = "" Then
            Start.Label4.Text = "無料版DeepL有効"
            Start.Label4.Height = 32
        ElseIf DeepLKey <> "" And key <> "" And endpoint = "" And location = "" Then
            Start.Label4.Text = "無料版DeepL有効"
            Start.Label4.Height = 32
        ElseIf DeepLKey <> "" And key = "" And endpoint = "" And location = "" Then
            Start.Label4.Text = "無料版DeepL有効"
            Start.Label4.Height = 32
        Else
            Start.Label4.Text = "無料版DeepL APIもしくはAzure翻訳APIを入力してください"
            Start.Label4.Height = 161
        End If
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strPath As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        'INIファイルクラスの生成
        Dim Ini As New ClsIni(strPath & "\マイクラMOD翻訳ソフト.ini")

        'INIファイルからの取得テスト
        Dim str As String
        str = Ini.GetProfileString("Settings", "Key", "")
        TextBox1.Text = str.ToString
        str = Ini.GetProfileString("Settings", "Endpoint", "")
        TextBox2.Text = str.ToString
        str = Ini.GetProfileString("Settings", "location", "")
        TextBox3.Text = str.ToString
        str = Ini.GetProfileString("Settings", "DeepLKey", "")
        TextBox4.Text = str.ToString
        endpoint = TextBox2.Text
        key = TextBox1.Text
        location = TextBox3.Text
        DeepLKey = TextBox4.Text
    End Sub
End Class