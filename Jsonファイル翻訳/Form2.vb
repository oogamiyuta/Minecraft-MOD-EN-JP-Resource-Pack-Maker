Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.IO.Compression
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public Class Form2
    Private endpoint As String = Start.endpoint
    Private key As String = Start.key
    Private location As String = Start.location
    Private DeepLKey As String = Start.DeepLKey

    Private modid As String
    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) _
        Handles MyBase.FormClosing
        Application.Exit()
    End Sub
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add（"リリースVer.1.13～1.14.4 プレVer.17w48a～19w46b"）
        ComboBox1.Items.Add（"リリースVer.1.15～1.16.1 プレVer.1.15-pre1～1.16.2-pre3"）
        ComboBox1.Items.Add（"リリースVer.1.16.2～1.16.5 プレVer.1.16.2-rc1～1.16.5"）
        ComboBox1.Items.Add（"リリースVer.1.17～1.17.1 プレVer.20w45a～21w38a"）
        ComboBox1.Items.Add（"リリースVer.1.18～1.18.2 プレVer.21w39a～1.18.2"）
        ComboBox1.Items.Add（"リリースVer.1.19～1.19.2 プレVer.22w11a～1.19.2"）
        ComboBox1.Items.Add（"リリースVer.欠番 プレVer.欠番"）
        ComboBox1.Items.Add（"プレVer.22w42a～22w44a"）
        ComboBox1.Items.Add（"リリースVer.1.19.3 プレVer.22w45a～23w07a"）
        ComboBox1.Items.Add（"リリースVer.1.19.4 プレVer.1.19.4-pre1～23w13a"）
        ComboBox1.Items.Add（"プレVer.23w14a～23w16a"）
        ComboBox1.Items.Add（"リリースVer.1.20～1.20.1 プレVer.23w17a～1.20.1"）
        ComboBox1.Items.Add（"プレVer.23w31a"）
        ComboBox1.Items.Add（"プレVer.23w32a～1.20.2-pre1"）
        ComboBox1.Items.Add（"リリースVer.1.20.2 プレVer.1.20.2-pre2～23w41a"）
        ComboBox1.Items.Add（"プレVer.23w42a"）
        ComboBox1.Items.Add（"プレVer.23w43a～23w44a"）
        ComboBox1.Items.Add（"プレVer.23w45a～23w46a"）
        ComboBox1.Items.Add（"リリースVer.1.20.3～1.20.4 プレVer.1.20.3-pre1～23w51b"）
        ComboBox1.Items.Add（"リリースVer.欠番 プレVer.欠番"）
        ComboBox1.Items.Add（"プレVer.24w03a～24w04a"）
        ComboBox1.Items.Add（"プレVer.24w05a～24w05b"）
        ComboBox1.Items.Add（"プレVer.24w06a～24w07"）
        ComboBox1.Items.Add（"リリースVer.欠番 プレVer.欠番"）
        ComboBox1.Items.Add（"プレVer.24w09a～24w10a"）
        ComboBox1.Items.Add（"プレVer.24w11a"）
        ComboBox1.Items.Add（"プレVer.24w12a"）
        ComboBox1.Items.Add（"プレVer.24w13a～1.20.5-pre3"）
        ComboBox1.Items.Add（"リリースVer.1.20.5～ プレVer.1.20.5-pre4～"）
        ComboBox1.Items.Add("手動設定")
        ComboBox1.SelectedIndex = 28
        pack_format.Text = ComboBox1.SelectedIndex + 4
        If ComboBox1.SelectedText = "手動設定" Then
            pack_format.Enabled = True
        Else
            pack_format.Enabled = False
        End If

        If Start.Label4.Text = "Azure翻訳 有効" Then
            Button2.Enabled = False
            TranslateButton.Enabled = True
        ElseIf Start.Label4.Text = "無料版DeepL有効" Then
            Button2.Enabled = True
            TranslateButton.Enabled = False
        ElseIf Start.Label4.Text = "無料版DeepL・Azure翻訳 有効" Then
            Button2.Enabled = True
            TranslateButton.Enabled = True
        End If
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        'リンクラベルのURLに移動する
        System.Diagnostics.Process.Start("https://minecraft.fandom.com/wiki/Pack_format")
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        pack_format.Text = ComboBox1.SelectedIndex + 4
        If ComboBox1.SelectedText = "手動設定" Then
            pack_format.Enabled = True
        Else
            pack_format.Enabled = False
        End If
    End Sub

    Private Sub OpenFileButton_Click(sender As Object, e As EventArgs) Handles OpenFileButton.Click
        OpenFileDialog1.Filter = "MOD Files|*.jar"
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            ' Zipファイルを開く
            Dim zipFilePath As System.IO.Compression.ZipArchive = System.IO.Compression.ZipFile.OpenRead(OpenFileDialog1.FileName)
            ' Zipファイルに含まれるファイルを指定
            Dim zipInFile As System.IO.Compression.ZipArchiveEntry = Nothing
            For Each entry As ZipArchiveEntry In zipFilePath.Entries
                If entry.FullName.Contains("assets/") AndAlso entry.FullName.Contains("/lang/en_us.json") Then
                    Dim startIndex As Integer = entry.FullName.IndexOf("/assets/") + "/assets/".Length
                    Dim endIndex As Integer = entry.FullName.IndexOf("/lang/en_us.json")
                    modid = entry.FullName.Substring(startIndex, endIndex - startIndex)
                    zipInFile = entry
                    Exit For
                End If
            Next

            If zipInFile IsNot Nothing Then
                ' JSONファイルを開く
                Using reader As New StreamReader(zipInFile.Open())
                    Dim fileContent As String = reader.ReadToEnd()
                    Dim json As JObject = JObject.Parse(fileContent)
                    Dim dataTable As New DataTable()
                    dataTable.Columns.Add("Key", GetType(String))
                    dataTable.Columns.Add("Value", GetType(String))
                    For Each propertyItem As JProperty In json.Properties()
                        dataTable.Rows.Add(propertyItem.Name, propertyItem.Value.ToString())
                    Next
                    DataGridView1.DataSource = dataTable
                End Using
            End If
        End If
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    End Sub


    Private Sub TranslateButton_Click(sender As Object, e As EventArgs) Handles TranslateButton.Click
        ProgressBar1.Maximum = DataGridView1.Rows.Count
        ProgressBar1.Value = 0
        ProgressBar1.Step = DataGridView1.Rows.Count
        翻訳中.ProgressBar1.Maximum = DataGridView1.Rows.Count
        翻訳中.ProgressBar1.Value = 0
        翻訳中.Label2.Text = ProgressBar1.Value & "/" & ProgressBar1.Step & "翻訳完了"
        翻訳中.Show()
        '翻訳対象のテキストを抽出する
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            Dim text As String = DataGridView1.Rows(i).Cells("Value").Value.ToString()

            '翻訳する
            Dim fromLanguage As String = "en"
            Dim toLanguages As String = "ja"

            Dim route As String = String.Format("/translate?api-version=3.0&from={0}&to={1}", fromLanguage, String.Join("&to=", toLanguages))

            Dim body() As Object = {New With {Key .Text = text}}
            Dim requestBody As String = JsonConvert.SerializeObject(body)

            Using client As New HttpClient()
                Using request As New HttpRequestMessage()

                    request.Method = HttpMethod.Post
                    request.RequestUri = New Uri(endpoint + route)
                    request.Content = New StringContent(requestBody, Encoding.UTF8, "application/json")
                    request.Headers.Add("Ocp-Apim-Subscription-Key", key)
                    request.Headers.Add("Ocp-Apim-Subscription-Region", Location)

                    Dim response As HttpResponseMessage = client.SendAsync(request).GetAwaiter().GetResult()
                    Dim result As String = response.Content.ReadAsStringAsync().GetAwaiter().GetResult()

                    Dim translations As JArray = JArray.Parse(result).First()("translations")

                    '翻訳結果を反映する

                    DataGridView1.Rows(i).Cells("Value").Value = translations.First()("text")
                    ProgressBar1.Value = ProgressBar1.Value + 1
                    翻訳中.Label2.Text = ProgressBar1.Value & "/" & ProgressBar1.Step & "翻訳完了"


                End Using
            End Using
        Next
        翻訳中.Close()
        MsgBox("翻訳終了")
    End Sub

    Private Class TranslationResult
        <JsonProperty("translations")>
        Public Property Translations As List(Of Translation)
    End Class

    Private Class Translation
        <JsonProperty("text")>
        Public Property Text As String
        <JsonProperty("to")>
        Public Property ToLanguage As String
    End Class

    Private Sub SaveFileButton_Click(sender As Object, e As EventArgs) Handles SaveFileButton.Click
        'Dim dialog As New FolderBrowserDialog()
        'dialog.Description = "リソースパック保存場所選択"
        Dim dialog As New SaveFileDialog()
        dialog.Filter = "すべてのファイル(*.*)|*.*"
        dialog.FileName = "dummy.txt"
        dialog.ValidateNames = False
        dialog.CheckFileExists = False
        dialog.CheckPathExists = True
        dialog.FileName = "リソースパック保存場所選択"
        If dialog.ShowDialog() = DialogResult.OK Then
            dialog.FileName = Path.GetDirectoryName(dialog.FileName)
            'If dialog.ShowDialog() = DialogResult.OK Then
            Dim selectedFolder As String = dialog.FileName
            '翻訳結果をJSONファイルとして保存する
            Dim json As New JObject()
            For Each row As DataGridViewRow In DataGridView1.Rows
                json(row.Cells("Key").Value.ToString()) = row.Cells("Value").Value.ToString()
            Next
            If Not Directory.Exists(dialog.FileName & "\" & modid & "リソースパック\assets\" & modid & "\lang") Then
                Directory.CreateDirectory(dialog.FileName & "\" & modid & "リソースパック\assets\" & modid & "\lang”)
            End If
            File.WriteAllText(dialog.FileName & "\" & modid & "リソースパック\assets\" & modid & "\lang\ja_jp.json", json.ToString())

            Dim outputPath As String = dialog.FileName & "\" & modid & "リソースパック\"

            ' pack.mcmeta の内容を作成する
            Dim packData As New JObject()
            packData("pack") = JObject.FromObject(New With {
                .pack_format = New JRaw(pack_format.Text),
                .description = modid & "の日本語化"
            })

            ' pack.mcmeta を保存する
            Dim packFilePath As String = Path.Combine(outputPath, "pack.mcmeta")
            If Not Directory.Exists(Path.GetDirectoryName(packFilePath)) Then
                Directory.CreateDirectory(Path.GetDirectoryName(packFilePath))
            End If

            File.WriteAllText(packFilePath, packData.ToString())

            ' パッケージ画像を作成する
            Dim packPath As String = dialog.FileName & "\" & modid & "リソースパック\pack.png"

            ' パッケージ画像を作成
            Dim bmp As New Bitmap(512, 512)
            Dim g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Color.White)

            ' 文字列を描画
            Dim font As New Font("Yu Gothic UI", 50)
            Dim brush As New SolidBrush(Color.Black)
            Dim format As New StringFormat()
            format.Alignment = StringAlignment.Center
            format.LineAlignment = StringAlignment.Center
            g.DrawString(modid & "の日本語化", font, brush, New RectangleF(0, 0, 512, 512), format)

            ' パッケージ画像を保存
            bmp.Save(packPath, ImageFormat.Png)


        End If






    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.Exit()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ProgressBar1.Maximum = DataGridView1.Rows.Count
        ProgressBar1.Value = 0
        ProgressBar1.Step = DataGridView1.Rows.Count
        翻訳中.ProgressBar1.Maximum = DataGridView1.Rows.Count
        翻訳中.ProgressBar1.Value = 0
        翻訳中.Label2.Text = ProgressBar1.Value & "/" & ProgressBar1.Step & "翻訳完了"
        翻訳中.Show()
        '翻訳対象のテキストを抽出する
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            Dim text As String = DataGridView1.Rows(i).Cells("Value").Value.ToString()

            '翻訳する
            Dim apiKey As String = DeepLKey
            Dim endpoint As String = "https://api-free.deepl.com/v2/translate"
            Dim fromLanguage As String = "EN"
            Dim toLanguage As String = "JA"

            Dim url As String = $"{endpoint}?text={Uri.EscapeDataString(text)}&source_lang={fromLanguage}&target_lang={toLanguage}&auth_key={apiKey}"
            Dim client As New HttpClient()
            Dim response As HttpResponseMessage = client.GetAsync(url).Result
            Dim responseContent As String = response.Content.ReadAsStringAsync().Result
            Dim result As JObject = JObject.Parse(responseContent)

            '翻訳結果を反映する
            Dim translation As String = result("translations")(0)("text").ToString()
            DataGridView1.Rows(i).Cells("Value").Value = translation
            翻訳中.ProgressBar1.Value = 翻訳中.ProgressBar1.Value + 1
            ProgressBar1.Value = ProgressBar1.Value + 1
            翻訳中.Label2.Text = ProgressBar1.Value & "/" & ProgressBar1.Step & "翻訳完了"

        Next
        翻訳中.Close()
        MsgBox("翻訳終了")
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class
