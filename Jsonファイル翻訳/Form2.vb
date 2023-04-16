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
    Private location As String = Start.Location

    Private modid As String
    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) _
        Handles MyBase.FormClosing
        Application.Exit()
    End Sub
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add（"1.13-1.14.4"）
        ComboBox1.Items.Add（"1.15-1.16.1"）
        ComboBox1.Items.Add（"1.16.2-1.16.5"）
        ComboBox1.Items.Add（"1.17.17.1"）
        ComboBox1.Items.Add（"1.18-1.18.2"）
        ComboBox1.Items.Add（"1.19-19.2"）
        ComboBox1.Items.Add（"22w42a-22w44a"）
        ComboBox1.Items.Add（"22w45a-1.19.3"）
        ComboBox1.Items.Add（"1.19.4-23w13a"）
        ComboBox1.Items.Add（"23w14a"）
        ComboBox1.Items.Add("その他")
        ComboBox1.SelectedIndex = 5
        pack_format.Text = ComboBox1.SelectedIndex + 4
        If ComboBox1.SelectedIndex = 10 Then
            pack_format.Enabled = True
        Else
            pack_format.Enabled = False
        End If
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        'リンクラベルのURLに移動する
        System.Diagnostics.Process.Start("https://minecraft.fandom.com/wiki/Pack_format")
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        pack_format.Text = ComboBox1.SelectedIndex + 4
        If ComboBox1.SelectedIndex = 10 Then
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

                End Using
            End Using
        Next
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

End Class
