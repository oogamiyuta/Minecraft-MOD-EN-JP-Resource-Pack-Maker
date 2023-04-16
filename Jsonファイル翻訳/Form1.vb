Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Form1
    Private endpoint As String = Start.endpoint
    Private key As String = Start.key
    Private location As String = Start.Location
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) _
        Handles MyBase.FormClosing
        Application.Exit()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.Exit()
    End Sub
    Private Sub OpenFileButton_Click(sender As Object, e As EventArgs) Handles OpenFileButton.Click

        OpenFileDialog1.FileName = "en_us.json"
        OpenFileDialog1.Filter = "英語 JSON Files|en_us.json|JSON Files|*.json"
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            '選択したJSONファイルをDataGridView1に表示する
            Dim fileContent As String = File.ReadAllText(OpenFileDialog1.FileName)
            Dim json As JObject = JObject.Parse(fileContent)
            Dim dataTable As New DataTable()
            dataTable.Columns.Add("Key", GetType(String))
            dataTable.Columns.Add("Value", GetType(String))
            For Each propertyItem As JProperty In json.Properties()
                dataTable.Rows.Add(propertyItem.Name, propertyItem.Value)
            Next
            DataGridView1.DataSource = dataTable
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
        SaveFileDialog1.FileName = "ja_jp.json"
        SaveFileDialog1.Filter = "日本語 JSON Files|ja_jp.json|JSON Files|*.json"
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            '翻訳結果をJSONファイルとして保存する
            Dim json As New JObject()
            For Each row As DataGridViewRow In DataGridView1.Rows
                json(row.Cells("Key").Value.ToString()) = row.Cells("Value").Value.ToString()
            Next
            File.WriteAllText(SaveFileDialog1.FileName, json.ToString())
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
