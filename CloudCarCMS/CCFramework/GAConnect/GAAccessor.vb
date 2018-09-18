Imports System.Reflection

Namespace CCFramework.GAConnect

    Public Class GAAccessor
        Public Email As String = "email@domain.com"
        Public Password As String = "password"

        Private ReadOnly _SelectedDimensions As List(Of GADimension)
        Private ReadOnly _SelectedMetrics As List(Of GAMetric)

        Private _AnalyticsDataTable As DataTable

        Public Sub New()
            _SelectedDimensions = New List(Of GADimension)
            _SelectedMetrics = New List(Of GAMetric)
        End Sub

        Public Function GetProfiles() As List(Of GAProfile)
            Try
                Dim CurrentGaDataFetcher As New GADataFetcher(Email, Password)

                GetProfiles = CurrentGaDataFetcher.GetUserProfiles().ToList

            Catch Ex As Exception
                GetProfiles = Nothing
            End Try
        End Function

        Public Function GetAnalyticsInfo(ProfileId As String, FromDate As DateTime, ToDate As DateTime, MaxRecords As Integer) As DataTable
            Dim CurrentGaDataFetcher As New GADataFetcher(Email, Password)

            Dim CurrentData As IEnumerable(Of GAData) = CurrentGaDataFetcher.GetAnalytics(ProfileId, FromDate, ToDate, MaxRecords, _SelectedDimensions, _SelectedMetrics, _SelectedMetrics(0), GASortDirection.Descending)

            Dim CurrentDataTable As DataTable = ToDataTable(Of GAData)(CurrentData)
            Dim CurrentDataCaption As String
            Dim CurrentUnwantedColumns As New List(Of DataColumn)()

            For Each CurrentDataColumn As DataColumn In CurrentDataTable.Columns
                CurrentDataCaption = CurrentDataColumn.ToString().Substring(0, 1).ToLower() + CurrentDataColumn.ToString().Substring(1)
                If [Enum].IsDefined(GetType(GADimension), CurrentDataCaption) Then
                    If Not _SelectedDimensions.Contains(DirectCast([Enum].Parse(GetType(GADimension), CurrentDataCaption), GADimension)) Then
                        CurrentUnwantedColumns.Add(CurrentDataColumn)
                    End If
                ElseIf [Enum].IsDefined(GetType(GAMetric), CurrentDataCaption) Then
                    If Not _SelectedMetrics.Contains(DirectCast([Enum].Parse(GetType(GAMetric), CurrentDataCaption), GAMetric)) Then
                        CurrentUnwantedColumns.Add(CurrentDataColumn)
                    End If
                End If
            Next

            For Each CurrentDataColumn As DataColumn In CurrentUnwantedColumns
                CurrentDataTable.Columns.Remove(CurrentDataColumn)
            Next

            Dim CurrentDataView As DataView = CurrentDataTable.DefaultView
            CurrentDataView.Sort = "Date asc"
            CurrentDataTable = CurrentDataView.ToTable()

            _AnalyticsDataTable = CurrentDataTable

            Return _AnalyticsDataTable
        End Function

        Public Sub AddDimension(CurrentDimension As GADimension)
            _SelectedDimensions.Add(CurrentDimension)
        End Sub

        Public Sub AddMetric(CurrentMetric As GAMetric)
            _SelectedMetrics.Add(CurrentMetric)
        End Sub

        Public Shared Function ToDataTable(Of T)(DataItems As IEnumerable(Of T)) As DataTable
            Dim CurrentDataTable As New DataTable(GetType(T).Name)
            Dim CurrentProps As PropertyInfo() = GetType(T).GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)

            For Each CurrentProp As PropertyInfo In CurrentProps
                CurrentDataTable.Columns.Add(CurrentProp.Name, CurrentProp.PropertyType)
            Next

            For Each CurrentItem In DataItems
                Dim CurrentValues As Object() = New Object(CurrentProps.Length - 1) {}

                For CurrentIndex As Integer = 0 To CurrentProps.Length - 1
                    CurrentValues(CurrentIndex) = CurrentProps(CurrentIndex).GetValue(CurrentItem, Nothing)
                Next

                CurrentDataTable.Rows.Add(CurrentValues)
            Next

            Return CurrentDataTable
        End Function
        
        Public Shared Function ParseAnalyticsDate(ParseDate As String) As DateTime
            Dim CurrentDate As DateTime

            CurrentDate = New DateTime(Integer.Parse(ParseDate.Substring(0, 4)), Integer.Parse(ParseDate.Substring(4, 2)), Integer.Parse(ParseDate.Substring(6, 2)))

            Return CurrentDate
        End Function

        Public Function GetJsChartData() As String
            If Not _AnalyticsDataTable Is Nothing Then
                Dim CurrentData As String = "[["

                _AnalyticsDataTable.Columns("Date").SetOrdinal(0)

                For CurrentColumnIndex As Integer = 0 To _AnalyticsDataTable.Columns.Count - 1
                    If CurrentColumnIndex = _AnalyticsDataTable.Columns.Count - 1 Then
                        CurrentData &= String.Format("'{0}'],", _AnalyticsDataTable.Columns(CurrentColumnIndex).ColumnName)
                    Else
                        CurrentData &= String.Format("'{0}',", _AnalyticsDataTable.Columns(CurrentColumnIndex).ColumnName)
                    End If
                Next

                For CurrentRowIndex As Integer = 0 To _AnalyticsDataTable.Rows.Count - 1
                    CurrentData &= "["

                    For CurrentColumnIndex As Integer = 0 To _AnalyticsDataTable.Columns.Count - 1
                        If _AnalyticsDataTable.Columns(CurrentColumnIndex).ColumnName = "Date" Then
                            CurrentData &= String.Format("'{0:MMM dd yyyy}'", ParseAnalyticsDate(_AnalyticsDataTable.Rows(CurrentRowIndex)(CurrentColumnIndex).ToString()))
                        Else
                            CurrentData &= String.Format("{0}", _AnalyticsDataTable.Rows(CurrentRowIndex)(CurrentColumnIndex).ToString())
                        End If

                        If CurrentColumnIndex = _AnalyticsDataTable.Columns.Count - 1 Then
                            CurrentData &= "]"
                        Else
                            CurrentData &= ","
                        End If
                    Next

                    If CurrentRowIndex = _AnalyticsDataTable.Rows.Count - 1 Then
                        CurrentData &= "]"
                    Else
                        CurrentData &= ","
                    End If
                Next

                Return CurrentData
            Else
                Return "[]"
            End If
        End Function

    End Class

End Namespace