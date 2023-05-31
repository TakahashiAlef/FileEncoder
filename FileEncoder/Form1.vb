Public Class Form1


#Region "メソッド"

#Region "エンコード"
    ''' <summary>
    ''' base64にエンコードしてテキストファイル作成
    ''' </summary>
    Private Sub Encode()
        Dim cnt As Integer = 0

        For Each li As ListViewItem In Me.ListView1.Items
            If IO.File.Exists(li.Tag.ToString) = False Then
                Continue For
            End If

            Dim FilePath As String = li.Tag.ToString
            Dim Base64String As String = Base64Encode(FilePath)
            Dim OutDir As String = System.Environment.CurrentDirectory & "\"
            Dim FileName As String = li.Text & ".txt"

            Using ws As IO.StreamWriter = New IO.StreamWriter(OutDir & FileName, False)
                ws.Write(Base64String)
            End Using
            cnt += 1
        Next

        If cnt > 0 Then MessageBox.Show("エンコード成功")

    End Sub

    ''' <summary>
    ''' base64にエンコード
    ''' </summary>
    ''' <param name="strFileFullPath"></param>
    ''' <returns></returns>
    Private Function Base64Encode(ByVal strFileFullPath As String) As String
        'Base64で文字列に変換するファイル
        Dim inFile As System.IO.FileStream
        Dim bs() As Byte

        'ファイルをbyte型配列としてすべて読み込む
        inFile = New IO.FileStream(strFileFullPath, IO.FileMode.Open, IO.FileAccess.Read)
        ReDim bs(inFile.Length - 1)
        Dim readBytes As Long = inFile.Read(bs, 0, inFile.Length)
        inFile.Close()
        inFile.Dispose()

        Dim base64String As String
        base64String = System.Convert.ToBase64String(bs)

        Return base64String

    End Function

#End Region

#Region "デコード"

    ''' <summary>
    ''' テキストファイルの内容をbase64デコードしファイルを作成
    ''' </summary>
    Private Sub Decode()
        Dim cnt As Integer = 0

        For Each li As ListViewItem In Me.ListView1.Items
            If IO.File.Exists(li.Tag.ToString) = False Then
                Continue For
            End If
            If IO.Path.GetExtension(li.Tag.ToString) <> ".txt" Then
                MessageBox.Show("txt以外のファイルを検出しました。処理を中断します。")
                Return
            End If

            Dim FilePath As String = li.Tag.ToString
            Dim Base64String As String
            Using sr As New IO.StreamReader(FilePath)
                Base64String = sr.ReadToEnd
            End Using

            Dim bs As Byte() = System.Convert.FromBase64String(Base64String)
            Dim OutDir As String = System.Environment.CurrentDirectory & "\"
            Dim FileName As String = IO.Path.GetFileNameWithoutExtension(FilePath)

            Using outFile As IO.FileStream = New System.IO.FileStream(OutDir & FileName, IO.FileMode.Create, IO.FileAccess.Write)
                outFile.Write(bs, 0, bs.Length)
                outFile.Close()
            End Using
            cnt += 1
        Next

        If cnt > 0 Then MessageBox.Show("デコード成功")

    End Sub

    ''' <summary>
    ''' base64デコード
    ''' </summary>
    ''' <param name="strBase64"></param>
    ''' <param name="strFullPath"></param>
    Private Sub Base64Decode(ByVal strBase64 As String, ByVal strFullPath As String)

        If strBase64 <> "" Then
            'バイト型配列に戻す
            Dim bs As Byte() = System.Convert.FromBase64String(strBase64)
            'ファイルに書き込む

            Using outFile As IO.FileStream = New System.IO.FileStream(strFullPath, IO.FileMode.Create, IO.FileAccess.Write)
                outFile.Write(bs, 0, bs.Length)
                outFile.Close()
            End Using

        End If
    End Sub


#End Region

#End Region


#Region "イベント"

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        OpenFileDialog1.Multiselect = True
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            For Each strFilePath As String In OpenFileDialog1.FileNames
                Dim strFileName As String = IO.Path.GetFileName(strFilePath)
                Dim li As ListViewItem = Me.ListView1.Items.Add(strFileName)
                li.Tag = strFilePath ' フルパスをタグにセット
            Next
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.ListView1.Items.Clear()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.ListView1.Items.Clear()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Encode()
    End Sub

    Private Sub 使用方法ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 使用方法ToolStripMenuItem.Click
        Using f2 As New Form2
            f2.ShowDialog()
        End Using
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Decode()
    End Sub

#End Region

End Class
