Imports System.Reflection.Emit

Public Class Form3
    Private endpoint As String = Start.endpoint
    Private key As String = Start.key
    Private location As String = Start.location

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '自分自身の存在するフォルダ
        Dim strPath As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        'INIファイルクラスの生成
        Dim Ini As New ClsIni(strPath & "\マイクラMOD翻訳ソフト.ini")
        'INIファイルへの書込みテスト
        Ini.WriteProfileString("Settings", "Key", TextBox1.Text)
        Ini.WriteProfileString("Settings", "Endpoint", TextBox2.Text)
        Ini.WriteProfileString("Settings", "location", TextBox3.Text)

        'INIファイルからの取得テスト
        Dim str As String
        str = Ini.GetProfileString("Settings", "Key", "")
        key = str.ToString
        str = Ini.GetProfileString("Settings", "Endpoint", "")
        endpoint = str.ToString
        str = Ini.GetProfileString("Settings", "location", "")
        location = str.ToString
        Me.Close()
        If key = "" AndAlso endpoint = "" AndAlso location = "" Then
            MsgBox（"設定を開いてAzure 翻訳APIと、 そのエンドポイント及び場所を" & "入力してください。" & vbCrLf & "無料版もあります。詳しくはご自分で調べてください。"）
            Start.Label4.Height = 161
            Start.Label4.Text = "設定を開いてAzure 翻訳APIと、そのエンドポイント及び場所を入力してください。" & vbCrLf & "無料版もあります。詳しくはご自分で調べてください。"
        ElseIf key = "" Or endpoint = "" Or location = "" Then
            MsgBox（"設定を開いてAzure 翻訳APIと、そのエンドポイント及び場所を入力してください。" & vbCrLf & "すべて入力されていないと使用できません。"）
            Start.Label4.Height = 161
            Start.Label4.Text = "設定を開いてAzure 翻訳APIと、そのエンドポイント及び場所を入力してください。" & vbCrLf & "すべて入力されていないと使用できません。"
        Else
            Start.Label4.Text = "APIなどが正しいものであれば使用できます。"
            Start.Label4.Height = 32
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
        endpoint = TextBox2.Text
        key = TextBox1.Text
        location = TextBox3.Text
    End Sub
End Class