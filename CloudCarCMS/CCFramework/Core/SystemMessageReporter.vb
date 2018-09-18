Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement

Namespace CCFramework.Core

    Public Interface IMessageReporter

        Sub LoadMessages()
        Function GetMessages() As List(Of SystemMessage)

    End Interface

    Public Class SystemMessageReporter
        Implements IMessageReporter

        Private _CurrentSystemMessages As List(Of SystemMessage)

        Public Sub New()
            _CurrentSystemMessages = New List(Of SystemMessage)

            LoadMessages()
        End Sub

        Public Sub LoadMessages() Implements IMessageReporter.LoadMessages
            If Settings.StoreEnabled Then
                GetOrderMessages()
            End If

            GetSalesLeadMessages()
            GetMailinglistMessages()

            _CurrentSystemMessages = _CurrentSystemMessages.OrderByDescending(Function(m) m.ActivityDate).ToList
        End Sub

        Public Function GetMessages() As List(Of SystemMessage) Implements IMessageReporter.GetMessages
            Return _CurrentSystemMessages
        End Function

        Private Sub GetOrderMessages()
            Dim CurrentOrderController As New OrderController

            _CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, String.Format("You currently have <b>{0}</b> order(s) waiting to be shipped", OrderController.GetUnshippedOrderCount)))

            Dim CurrentUnshippedOrders As List(Of Order) = CurrentOrderController.GetElements.Where(Function(o) o.ShipDate Is Nothing).ToList

            For Each Item As Order In CurrentUnshippedOrders
                Dim CurrentActivityDate As DateTime = Item.OrderDate
                Dim CurrentActivityMessage As String = String.Format("{0} {1} is waiting for Order <b>{2}</b> to be shipped", Item.SimpleUser.FirstName, Item.SimpleUser.LastName, Item.ID)

                _CurrentSystemMessages.Add(New SystemMessage(CurrentActivityDate, CurrentActivityMessage))
            Next

            CurrentOrderController.Dispose()
        End Sub

        Private Sub GetSalesLeadMessages()
            _CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, String.Format("You currently have <b>{0}</b> new lead(s)", SalesInquiryController.GetNewInquiriesCount)))

            Dim CurrentUncheckedSalesInquiry As List(Of SalesInquiry) = SalesInquiryController.GetElement.Where(Function(o) o.checked = False).ToList

            For Each Item As SalesInquiry In CurrentUncheckedSalesInquiry
                Dim CurrentActivityDate As DateTime = Item.datesent
                Dim CurrentActivityMessage As String = String.Format("<b>{0}</b> has made an enquiry on your web site", Item.name)

                _CurrentSystemMessages.Add(New SystemMessage(CurrentActivityDate, CurrentActivityMessage))
            Next
        End Sub

        Private Sub GetMailinglistMessages()
            _CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, String.Format("You currently have <b>{0}</b> subscriber(s) to your mailinglist", SubscriptionController.GetSubscribers.Count)))

            Dim CurrentUnshippedOrders As List(Of Subscription) = SubscriptionController.GetSubscribers.ToList

            For Each Item As Subscription In CurrentUnshippedOrders
                Dim CurrentActivityDate As DateTime = Item.DateAdded
                Dim CurrentActivityMessage As String = String.Format("<b>{0}</b> subscribed to your mailinglist", Item.Email)

                _CurrentSystemMessages.Add(New SystemMessage(CurrentActivityDate, CurrentActivityMessage))
            Next
        End Sub

    End Class

    Public Class TipMessageReporter
        Implements IMessageReporter

        Private _CurrentSystemMessages As List(Of SystemMessage)

        Public Sub New()
            _CurrentSystemMessages = New List(Of SystemMessage)

            LoadMessages()
        End Sub

        Public Sub LoadMessages() Implements IMessageReporter.LoadMessages
            If Settings.StoreEnabled Then
                GetStoreTipMessages()
            End If

            GetContentTipMessages()
            GetBlogTipMessages()

            _CurrentSystemMessages = _CurrentSystemMessages.OrderByDescending(Function(m) m.ActivityDate).ToList
        End Sub

        Private Sub GetStoreTipMessages()
            _CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, "To make a new product available on your store, click products on the dashboard or from the Cloud Car Store menu, and click the green ""New"" button"))
            '_CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, ""))
            '_CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, ""))
            '_CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, ""))
            '_CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, ""))
        End Sub

        Private Sub GetContentTipMessages()
            _CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, "If you would like to add more content to your site, click the pages button on the dashboard or from the Cloud Car Content menu, and click the green ""New"" button"))
            '_CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, ""))
            '_CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, ""))
            '_CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, ""))
        End Sub

        Private Sub GetBlogTipMessages()
            _CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, "Add blog post's to increase traffic and add engaging content to your site"))
            '_CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, ""))
            '_CurrentSystemMessages.Add(New SystemMessage(DateTime.Now, ""))
        End Sub

        Public Function GetMessages() As List(Of SystemMessage) Implements IMessageReporter.GetMessages
            Return _CurrentSystemMessages
        End Function

    End Class

    Public Class SystemMessage
        Private _ActivityDate As DateTime
        Private _ActivityMessage As String

        Public Sub New(ActivityDate As DateTime, ActivityMessage As String)
            _ActivityDate = ActivityDate
            _ActivityMessage = ActivityMessage
        End Sub

        Public Property ActivityDate() As DateTime
            Get
                Return _ActivityDate
            End Get
            Set(Value As DateTime)
                _ActivityDate = Value
            End Set
        End Property

        Public Property ActivityMessage As String
            Get
                Return _ActivityMessage
            End Get
            Set(Value As String)
                _ActivityMessage = Value
            End Set
        End Property

    End Class

    'The obejective of this class will be to display messages on the dashboard about recent events.
    'The following events should be shown:
    '   -New orders
    '   -New Sales Leads
    '   -New Mailinglist Subscribers
    '   -New Product Reviews
    '   -New Blog Comments

End Namespace