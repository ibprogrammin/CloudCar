Namespace CCFramework.Core

    Public Class CSVGenerator

        Private Filename As String
        Private Path As String
        Private Data As DataTable

        Public Sub New()
            Filename = "Default.csv"
            Path = ""

            Data = New DataTable("CSV Data Table")
        End Sub

        Public Sub New(ByVal CSVFilename As String, ByVal CSVPath As String)
            Filename = CSVFilename
            Path = CSVPath

            Data = New DataTable("CSV Data Table")
        End Sub


        Public Sub AddColumn(ByVal ColumnName As String)
            Data.Columns.Add(New DataColumn(ColumnName))
        End Sub

        Public Sub AddRow(ByVal CSVRow As DataRow)
            Data.Rows.Add(CSVRow)
        End Sub

        Public Function GenerateCSVString() As String
            Dim CSVStringBuilder As New StringBuilder()

            Dim ColumnCount As Integer = Data.Columns.Count

            'Generate Column Headings
            For i As Integer = 0 To ColumnCount - 1
                If i < ColumnCount And i > 0 Then
                    CSVStringBuilder.Append(", ")
                End If

                CSVStringBuilder.Append(Data.Columns(i).ColumnName)
            Next

            CSVStringBuilder.Append(vbNewLine)

            'Generate Rows
            For Each row As DataRow In Data.Rows
                For i As Integer = 0 To ColumnCount - 1
                    If i < ColumnCount And i > 0 Then
                        CSVStringBuilder.Append(", ")
                    End If

                    CSVStringBuilder.Append(row(i).ToString)
                Next

                CSVStringBuilder.Append(vbNewLine)
            Next

            Return CSVStringBuilder.ToString
        End Function


        Public ReadOnly Property CSVData() As DataTable
            Get
                Return Data
            End Get
        End Property

        Public Property CSVFilename() As String
            Get
                Return Filename
            End Get
            Set(ByVal value As String)
                Filename = value
            End Set
        End Property

        Public Property CSVPath() As String
            Get
                Return Path
            End Get
            Set(ByVal value As String)
                Path = value
            End Set
        End Property

    End Class

End Namespace