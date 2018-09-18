Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce

    Public Class ProductDownloadController
        Inherits DataControllerClass

        Public Shared GetAllProductDownloadsFunc As Func(Of CommerceDataContext, IQueryable(Of ProductDownload)) = _
                CompiledQuery.Compile(Function(db As CommerceDataContext) From pd In db.ProductDownloads Select pd)

        Public Shared GetProductDownloadFromGUIDFunc As Func(Of CommerceDataContext, Guid, ProductDownload) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, guid As Guid) (From pd In db.ProductDownloads Where pd.GUID = guid Select pd).FirstOrDefault)

        Public Shared GetProductDownloadFromOrderItemIDFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductDownload)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, ItemID As Integer) From pd In db.ProductDownloads Where pd.OrderItemID = ItemID Select pd)

        Public Shared GetProductDownloadDetailsByOrderID As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductDownloadDetail)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, OrderID As Integer) _
                                    From pd In db.ProductDownloads _
                                    Join oi In db.OrderItems On pd.OrderItemID Equals oi.ID _
                                    Join o In db.Orders On oi.OrderID Equals o.ID _
                                    Join p In db.Products On oi.ProductID Equals p.ID _
                                    Where o.ID = OrderID _
                                    Select New ProductDownloadDetail With _
                                    {.ID = pd.GUID, .Name = p.Name, .OrderID = o.ID, .UserID = o.UserID, .Filename = pd.Filename, .Downloads = pd.Downloads})

        Public Shared GetProductDownloadsByOrderID As Func(Of CommerceDataContext, Integer, IQueryable(Of SimpleProductDownloadDetail)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, OrderID As Integer) _
                                        From oi In db.OrderItems _
                                        Join p In db.Products On oi.ProductID Equals p.ID _
                                        Where p.IsDigitalMedia = True And oi.OrderID = OrderID _
                                        Select New SimpleProductDownloadDetail With {.Name = p.Name, .ItemID = oi.ID, .Filename = p.Filename})

        Public Shared GetProductDownloadDetailsByUserID As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductDownloadDetail)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, UserID As Integer) _
                                    From pd In db.ProductDownloads _
                                    Join oi In db.OrderItems On pd.OrderItemID Equals oi.ID _
                                    Join o In db.Orders On oi.OrderID Equals o.ID _
                                    Join p In db.Products On oi.ProductID Equals p.ID _
                                    Where o.UserID = UserID _
                                    Select New ProductDownloadDetail With _
                                    {.ID = pd.GUID, .Name = p.Name, .OrderID = o.ID, .UserID = o.UserID, .Filename = pd.Filename, .Downloads = pd.Downloads})


        Public Overloads Function Create(ByVal OrderItemID As Integer, ByVal Filename As String) As Guid
            Dim item As New ProductDownload
            Dim itemID As Guid = Guid.NewGuid

            item.GUID = itemID
            item.OrderItemID = OrderItemID
            item.Filename = Filename
            item.Downloads = 0

            db.ProductDownloads.InsertOnSubmit(item)
            db.SubmitChanges()

            item = Nothing

            Return itemID
        End Function

        Public Overloads Function Delete(ByVal ID As Guid) As Boolean
            Try
                Dim item = GetProductDownloadFromGUIDFunc(db, ID)

                db.ProductDownloads.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal GUID As Guid, ByVal OrderItemID As Integer, ByVal Filename As String, ByVal Downloads As Integer) As Boolean
            Dim item As ProductDownload = GetProductDownloadFromGUIDFunc(db, GUID)

            If item Is Nothing Then
                Throw New Exception("Product Download " & GUID.ToString & " does not exist.")
            Else
                item.OrderItemID = OrderItemID
                item.Filename = Filename
                item.Downloads = Downloads

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal GUID As Guid) As ProductDownload
            Dim item As ProductDownload = GetProductDownloadFromGUIDFunc(db, GUID)

            If item Is Nothing Then
                Throw New Exception("Product Download " & GUID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of ProductDownload)
            Dim items = GetAllProductDownloadsFunc(db)

            If Not items Is Nothing Then
                Return items.ToList
            Else
                Throw New Exception("There are no Product Downloads")
            End If
        End Function

        Public Shared Sub CreateDownloadableOrderItems(ByVal OrderId As Integer)
            Dim db As New CommerceDataContext
            Dim downloadDetails As List(Of SimpleProductDownloadDetail) = GetProductDownloadsByOrderID(db, OrderID).ToList

            If downloadDetails.Count > 0 Then
                Dim DownloadableProduct As New ProductDownloadController
                For Each item As SimpleProductDownloadDetail In downloadDetails
                    DownloadableProduct.Create(item.ItemID, item.Filename)
                Next
            End If
        End Sub

        Public Shared Function SendDownloadLinks(ByVal OrderId As Integer) As Boolean
            Dim db As New CommerceDataContext
            Dim downloadDetails As List(Of ProductDownloadDetail) = GetProductDownloadDetailsByOrderID(db, OrderID).ToList

            Dim DomainName As String

            If Core.Settings.EnableSSL And Core.Settings.FullSSL Then
                DomainName = "https://" & Core.Settings.HostName
            Else
                DomainName = "http://" & Core.Settings.HostName
            End If

            Dim linkString As New StringBuilder

            linkString.Append("The following files are now available for you to download at " & DomainName & ": " & vbNewLine & vbNewLine)

            For Each item As ProductDownloadDetail In downloadDetails
                linkString.Append(DomainName & item.Link)
                linkString.Append(vbNewLine)
            Next

            If downloadDetails.Count > 0 Then
                Dim user As SimpleUser = New SimpleUserController().GetElement(downloadDetails.First.UserID)
                Dim ToAddress As New Net.Mail.MailAddress(user.Email, user.FirstName & user.LastName)

                SendEmails.Send(ToAddress, "Your files are ready for download from " & DomainName, linkString.ToString)

                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Sub DeleteDownloadsByOrderItemID(ByVal OrderItemId As Integer)
            Dim db As New CommerceDataContext
            Dim downloads = GetProductDownloadFromOrderItemIDFunc(db, OrderItemId)

            db.ProductDownloads.DeleteAllOnSubmit(downloads)
            db.SubmitChanges()
        End Sub

    End Class

    Public Class ProductDownloadDetail
        Private _id As Guid
        Private _name As String
        Private _orderID As Integer
        Private _userID As Integer
        Private _filename As String
        Private _downloads As Integer

        Public Property ID() As Guid
            Get
                Return _id
            End Get
            Set(ByVal value As Guid)
                _id = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property OrderID() As Integer
            Get
                Return _orderID
            End Get
            Set(ByVal value As Integer)
                _orderID = value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal value As Integer)
                _userID = value
            End Set
        End Property

        Public Property Filename() As String
            Get
                Return _filename
            End Get
            Set(ByVal value As String)
                _filename = value
            End Set
        End Property

        Public Property Downloads() As Integer
            Get
                Return _downloads
            End Get
            Set(ByVal value As Integer)
                _downloads = value
            End Set
        End Property

        Public ReadOnly Property Link() As String
            Get
                Return String.Format(HttpUtility.HtmlDecode(Core.Settings.LinkFormatString), _id.ToString, _filename)
            End Get
        End Property

    End Class

    Public Class SimpleProductDownloadDetail
        Private _name As String
        Private _itemID As Integer
        Private _filename As String

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property ItemID() As Integer
            Get
                Return _itemID
            End Get
            Set(ByVal value As Integer)
                _itemID = value
            End Set
        End Property

        Public Property Filename() As String
            Get
                Return _filename
            End Get
            Set(ByVal value As String)
                _filename = value
            End Set
        End Property

    End Class

End Namespace