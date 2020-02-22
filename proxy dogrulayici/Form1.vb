Imports System.Threading
Imports System.IO
Imports System.Net
Public Class Form1
    Dim proxies As New List(Of String)
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim threads As Integer = NumericUpDown1.Value
        ThreadPool.SetMaxThreads(threads, threads)
        ThreadPool.SetMinThreads((threads / 2), (threads / 2))
        For Each proxy In proxies
            ThreadPool.QueueUserWorkItem(AddressOf check)
        Next
    End Sub
    Public Sub check()
        Dim myProxy As WebProxy
        For Each proxy As String In proxies
            Try
                myProxy = New WebProxy(proxy)
                Dim r As HttpWebRequest = WebRequest.Create("http://www.google.com/")
                r.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.2 Safari/537.36"
                r.Timeout = 3000
                r.Proxy = myProxy
                Dim re As HttpWebResponse = r.GetResponse()
                ListBox1.Items.Add("Working Proxy: " & proxy)
            Catch ex As Exception
                'ListBox1.Items.Add("Unresponsive Proxy: " & proxy)
            End Try
        Next
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim fo As New OpenFileDialog
        fo.RestoreDirectory = True
        fo.Multiselect = False
        fo.Filter = "txt files (*.txt)|*.txt"
        fo.FilterIndex = 1
        fo.ShowDialog()
        If (Not fo.FileName = Nothing) Then
            Using sr As New StreamReader(fo.FileName)
                While sr.Peek <> -1
                    proxies.Add(sr.ReadLine())
                End While
            End Using
        End If
    End Sub

End Class
