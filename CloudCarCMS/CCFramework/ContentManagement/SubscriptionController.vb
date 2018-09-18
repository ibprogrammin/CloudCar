Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class SubscriptionController
        Implements IDisposable

        Public Shared Sub Add(ByVal SubscriberEmail As Net.Mail.MailAddress)
            Dim db As New ContentDataContext

            If (From s In db.Subscriptions Where s.Email = SubscriberEmail.Address).Count = 0 Then
                Dim subs As New Subscription

                subs.DateAdded = Now
                subs.Email = SubscriberEmail.Address

                db.Subscriptions.InsertOnSubmit(subs)
                db.SubmitChanges()

                subs = Nothing
            Else
                Throw New Exception("Address already exists")
            End If

            db = Nothing
        End Sub

        Public Shared Sub Remove(ByVal SubscriberEmail As System.Net.Mail.MailAddress)
            Dim db As New ContentDataContext

            For Each subs In (From s In db.Subscriptions Where s.Email = SubscriberEmail.Address)
                db.Subscriptions.DeleteOnSubmit(subs)
            Next
            db.SubmitChanges()

            db = Nothing
        End Sub

        Public Shared Sub OptOut(ByVal SubscriberEmail As System.Net.Mail.MailAddress)
            Dim db As New ContentDataContext

            For Each subs In (From s In db.Subscriptions Where s.Email = SubscriberEmail.Address)
                subs.OptOut = True
            Next

            db.SubmitChanges()

            db = Nothing
        End Sub

        Public Shared Function GetSubscribers() As List(Of Subscription)
            Dim CurrentDataContext As New ContentDataContext

            GetSubscribers = (From s In CurrentDataContext.Subscriptions Select s).ToList

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetOptInSubscribers() As List(Of Subscription)
            Dim CurrentDataContext As New ContentDataContext

            GetOptInSubscribers = (From s In CurrentDataContext.Subscriptions Where s.OptOut = False Select s).ToList

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetSubscriberCsv() As String
            Dim CustomerCsvFile As New Core.CSVGenerator()

            CustomerCsvFile.AddColumn("Email")
            CustomerCsvFile.AddColumn("Name")
            CustomerCsvFile.AddColumn("Date Added")

            For Each SubscriberItem As Subscription In GetOptInSubscribers()
                Dim CsvRow As DataRow = CustomerCsvFile.CSVData.NewRow

                CsvRow.Item(0) = SubscriberItem.Email
                CsvRow.Item(1) = SubscriberItem.Name
                CsvRow.Item(2) = SubscriberItem.DateAdded

                CustomerCsvFile.AddRow(CsvRow)
            Next

            GetSubscriberCsv = CustomerCsvFile.GenerateCSVString()
        End Function


#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then

                End If
            End If
            Me.disposedValue = True
        End Sub
#End Region

    End Class

End Namespace