Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce
Imports System.Web
Imports CloudCar.CCFramework.Commerce.ShoppingCart
Imports CloudCar.Resources

Namespace CCCommerce

    Partial Public Class Details
        Inherits RoutablePage

        Private Const _ShippingDays As Integer = 2

        Protected Property CurrentProductDisplayHelper As ProductShoppingCartMediator
            Get
                Return CType(Session("CurrentProductDisplayHelper"), ProductShoppingCartMediator)
            End Get
            Set(Value As ProductShoppingCartMediator)
                If Session("CurrentProductDisplayHelper") Is Nothing Then
                    Session.Add("CurrentProductDisplayHelper", Value)
                Else
                    Session("CurrentProductDisplayHelper") = Value
                End If
            End Set
        End Property

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Settings.StoreEnabled Then
                Response.Redirect(Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                CurrentProductDisplayHelper = New ProductShoppingCartMediator(Session("SessionId").ToString)

                Dim CurrentPermalink As String = (From v In RequestContext.RouteData.Values Where v.Key = "product" Select New With {.id = v.Value}).SingleOrDefault.id.ToString
                If Not CurrentPermalink = Nothing Then
                    LoadProduct(CurrentPermalink)
                ElseIf Not Request("ProductID") = Nothing Then
                    Dim CurrentProductId As Integer = Integer.Parse(Request("ProductID"))

                    LoadProduct(CurrentProductId)
                End If
            End If
        End Sub

        Private Sub LoadProduct(ByVal CurrentPermalink As String)
            Dim CurrentProductController As New ProductController

            Dim CurrentProduct As Product = CurrentProductController.GetElement(CurrentPermalink)

            ProductColorDropDownList.DataSource = CurrentProductController.GetColors(CurrentProduct.ID)
            ProductSizeDropDownList.DataSource = CurrentProductController.GetSizes(CurrentProduct.ID)
            ReviewsRepeater.DataSource = CurrentProduct.ProductReviews

            ProductColorDropDownList.DataBind()
            ProductSizeDropDownList.DataBind()
            ReviewsRepeater.DataBind()

            ReviewsLinkLiteral.Text = String.Format("{0} Reviews", CurrentProduct.ProductReviews.Count)

            CurrentProductDisplayHelper.SelectedProductId = CurrentProduct.ID

            Dim CurrentProductUrl As String
            Dim CurrentProductImageUrl As String
            Dim CurrentRelativeProductImageUrl As String = String.Format("/images/db/{0}/full/{1}", CurrentProduct.DefaultImageID, PictureController.GetPictureFilename(CurrentProduct.DefaultImageID))
            Dim CurrentSmallRelativeProductImageUrl As String = String.Format("/images/db/{0}/420/{1}", CurrentProduct.DefaultImageID, PictureController.GetPictureFilename(CurrentProduct.DefaultImageID))

            If Settings.EnableSSL = True And Settings.FullSSL Then
                CurrentProductUrl = String.Format("https://{0}/Shop/{1}/{2}.html", Settings.HostName, New CategoryController().GetElement(CurrentProduct.CategoryID).Permalink, CurrentProduct.Permalink)
                CurrentProductImageUrl = String.Format("https://{0}/images/db/{1}/full/{2}", Settings.HostName, CurrentProduct.DefaultImageID, PictureController.GetPictureFilename(CurrentProduct.DefaultImageID))
            Else
                CurrentProductUrl = String.Format("http://{0}/Shop/{1}/{2}.html", Settings.HostName, New CategoryController().GetElement(CurrentProduct.CategoryID).Permalink, CurrentProduct.Permalink)
                CurrentProductImageUrl = String.Format("http://{0}/images/db/{1}/full/{2}", Settings.HostName, CurrentProduct.DefaultImageID, PictureController.GetPictureFilename(CurrentProduct.DefaultImageID))
            End If

            DefaultImageHiddenField.Value = CurrentRelativeProductImageUrl
            DefaultSmallImageHiddenField.Value = CurrentSmallRelativeProductImageUrl
            DefaultImageDescriptionHiddenField.Value = CurrentProduct.Name

            'MainProductImage.Src = CurrentRelativeProductImageUrl
            'MainProductImage.Alt = CurrentProduct.ImageAlt

            'MainImageAnchor.HRef = CurrentRelativeProductImageUrl
            'MainImageAnchor.Title = CurrentProduct.Name

            LoadProductImages()

            PintrestAnchor.HRef = String.Format("http://pinterest.com/pin/create/button/?url={0}&media={1}&description={2}", CurrentProductUrl, CurrentProductImageUrl, CurrentProduct.Name)

            AverageProductRating.CurrentRating = ProductReviewController.GetProductRating(CurrentProduct.ID)

            ProductBrandLiteral.Text = CurrentProduct.Brand.Name
            ProductListPrice.Text = CurrentProduct.ListPrice.ToString("C")
            ProductCodeLiteral.Text = CurrentProduct.SKU
            ProductTitleLiteral.Text = CurrentProduct.Name
            ProductDescriptionLiteral.Text = CurrentProduct.Details
            DetailsRecomendedProductControl.ProductId = CurrentProduct.ID

            If Not Security.Membership.GetUser Is Nothing Then
                Dim CurrentUserName As String = Security.Membership.GetUser().UserName

                ProductPriceLiteral.Text = String.Format("{0:$0.00} {1}", ProductController.GetPrice(RegisteredUserController.GetUserPriceLevel(CurrentUserName), CurrentProduct), CurrentProduct.PricingUnit)
            Else
                ProductPriceLiteral.Text = String.Format("{0:$0.00} {1}", CurrentProduct.Price, CurrentProduct.PricingUnit)
            End If

            PageBreadCrumbControl.ProductId = CurrentProduct.ID

            'MoreImagesHyperLink.HRef = String.Format("~/CCCommerce/ProductImages.aspx?Product={0}", CurrentProduct.ID)
            'MoreImagesHyperLink.Title = String.Format("Click here for more images of {0}", CurrentProduct.Name)

            If ProductColorDropDownList.Items.Count < 1 Then
                ProductColorDropDownList.Visible = False
            End If

            If ProductSizeDropDownList.Items.Count < 1 Then
                ProductSizeDropDownList.Visible = False
            End If

            If CurrentProduct.TrackInventory Then
                Dim CurrentInventory As Integer

                Dim CurrentInventoryController As New InventoryController
                CurrentInventory = CurrentInventoryController.GetInventoryQuantity(CurrentProduct.ID)
                CurrentInventoryController.Dispose()

                If CurrentInventory > 0 Then
                    AvailabilityLiteral.Text = String.Format(CommerceResources.AvailabilityLabel1, CurrentInventory, CurrentProduct.PricingUnit, _ShippingDays)
                Else
                    AvailabilityLiteral.Text = CommerceResources.AvailabilityLabel2
                End If
            Else
                AvailabilityLiteral.Text = String.Format(CommerceResources.AvailabilityLabel3, _ShippingDays)
            End If

            Title = String.Format("{0}{1}", CurrentProduct.BrowserTitle, Settings.SiteTitle)

            PageKeywordsMeta.Attributes("content") = CurrentProduct.Keywords
            PageDescriptionMeta.Attributes("content") = CurrentProduct.Description
            PageCanonicalMeta.Attributes("href") = CurrentProductUrl

            prcReview.ProductId = CurrentProduct.ID

            CurrentProductDisplayHelper.ItemIsMembership = CurrentProduct.Membership

            CurrentProductController.Dispose()
        End Sub

        Private Sub LoadProduct(ByVal CurrentProductId As Integer)
            Dim CurrentProductController As New ProductController

            Dim CurrentProduct As Product = CurrentProductController.GetElement(CurrentProductId)

            ProductColorDropDownList.DataSource = CurrentProductController.GetColors(CurrentProduct.ID)
            ProductSizeDropDownList.DataSource = CurrentProductController.GetSizes(CurrentProduct.ID)
            ReviewsRepeater.DataSource = CurrentProduct.ProductReviews

            ProductColorDropDownList.DataBind()
            ProductSizeDropDownList.DataBind()
            ReviewsRepeater.DataBind()

            CurrentProductController.Dispose()

            CurrentProductDisplayHelper.SelectedProductId = CurrentProduct.ID

            'MainProductImage.Src = String.Format("/images/db/{0}/420/{1}.jpg", CurrentProduct.HeaderImageID, CurrentProduct.Permalink)
            'MainProductImage.Alt = CurrentProduct.ImageAlt

            LoadProductImages()

            AverageProductRating.CurrentRating = ProductReviewController.GetProductRating(CurrentProduct.ID)

            ProductTitleLiteral.Text = CurrentProduct.Name
            ProductDescriptionLiteral.Text = CurrentProduct.Details

            If Not Security.Membership.GetUser Is Nothing Then
                Dim CurrentUserName As String = Security.Membership.GetUser().UserName

                ProductPriceLiteral.Text = String.Format("{0:$0.00}", ProductController.GetPrice(RegisteredUserController.GetUserPriceLevel(CurrentUserName), CurrentProduct))
            Else
                ProductPriceLiteral.Text = CurrentProduct.Price.ToString("C")
            End If

            PageBreadCrumbControl.ProductId = CurrentProduct.ID

            'MoreImagesHyperLink.HRef = String.Format("~/CCCommerce/ProductImages.aspx?Product={0}", CurrentProduct.ID)
            'MoreImagesHyperLink.Title = String.Format("Click here for more images of {0}", CurrentProduct.Name)

            If ProductColorDropDownList.Items.Count < 1 Then
                ProductColorDropDownList.Visible = False
            End If

            If ProductSizeDropDownList.Items.Count < 1 Then
                ProductSizeDropDownList.Visible = False
            End If

            Me.Title = String.Format("{0}{1}", CurrentProduct.BrowserTitle, Settings.SiteTitle)

            PageKeywordsMeta.Attributes("content") = CurrentProduct.Keywords
            PageDescriptionMeta.Attributes("content") = CurrentProduct.Description
            
            If Settings.EnableSSL = True And Settings.FullSSL Then
                PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/Shop/{1}/{2}.html", Settings.HostName, New CategoryController().GetElement(CurrentProduct.CategoryID).Permalink, CurrentProduct.Permalink)
            Else
                PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/Shop/{1}/{2}.html", Settings.HostName, New CategoryController().GetElement(CurrentProduct.CategoryID).Permalink, CurrentProduct.Permalink)
            End If

            prcReview.ProductId = CurrentProduct.ID

            CurrentProductDisplayHelper.ItemIsMembership = CurrentProduct.Membership
        End Sub

        Private Sub LoadProductImages()
            Dim CurrentImageTable As New DataTable("ProductImages")

            CurrentImageTable.Columns.Add("ImageId")
            CurrentImageTable.Columns.Add("Description")

            Dim CurrentProductController As New ProductController
            Dim CurrentProduct As Product = CurrentProductController.GetElement(CurrentProductDisplayHelper.SelectedProductId)
            CurrentProductController.Dispose()

            If Not CurrentProduct Is Nothing Then
                Dim CurrentDataRow As DataRow = CurrentImageTable.NewRow

                CurrentDataRow.Item("ImageId") = CurrentProduct.DefaultImageID
                CurrentDataRow.Item("Description") = CurrentProduct.Name

                CurrentImageTable.Rows.Add(CurrentDataRow)

                Dim dr2 As DataRow = CurrentImageTable.NewRow

                dr2.Item("ImageId") = CurrentProduct.HeaderImageID
                dr2.Item("Description") = CurrentProduct.Name

                CurrentImageTable.Rows.Add(dr2)

                'MainProductImage.Src = "/images/db/" & CurrentProduct.HeaderImageID & "/640/image" & CurrentProduct.HeaderImageID & ".jpg"
                'MainProductImage.Alt = CurrentProduct.ImageAlt
            End If

            Dim CurrentProductColorController As New ProductColourController
            Dim CurrentColorImages As List(Of ProductColor) = CurrentProductColorController.GetColors(CurrentProductDisplayHelper.SelectedProductId).ToList
            CurrentProductColorController.Dispose()

            If CurrentColorImages.Count > 0 Then
                For Each item In CurrentColorImages
                    If item.ImageID.HasValue And item.ImageID > 0 Then
                        Dim CurrentDataRow As DataRow = CurrentImageTable.NewRow

                        CurrentDataRow.Item("ImageId") = item.ImageID
                        CurrentDataRow.Item("Description") = String.Format("{0} {1}", item.Color.Name, CurrentProduct.Name)

                        CurrentImageTable.Rows.Add(CurrentDataRow)
                    End If
                Next
            End If

            Dim CurrentDataContext As New CommerceDataContext
            Dim AdditionalImages As List(Of ProductImage) = (From pi In CurrentDataContext.ProductImages Where pi.ProductID = CurrentProductDisplayHelper.SelectedProductId Select pi).ToList

            If AdditionalImages.Count > 0 Then
                For Each item In AdditionalImages
                    Dim CurrentDataRow As DataRow = CurrentImageTable.NewRow

                    CurrentDataRow.Item("ImageId") = item.ImageID
                    CurrentDataRow.Item("Description") = item.Description

                    CurrentImageTable.Rows.Add(CurrentDataRow)
                Next
            End If

            ImagesThumbnailRepeater.DataSource = CurrentImageTable
            ImagesThumbnailRepeater.DataBind()

            CurrentDataContext.Dispose()
        End Sub

        Protected Sub AddToCartButtonClick(ByVal Sender As Object, ByVal Args As EventArgs)
            If ProductColorDropDownList.Items.Count < 1 OrElse ProductColorDropDownList.SelectedValue = String.Empty Then
                CurrentProductDisplayHelper.SelectedColor = Settings.NoColorID
            Else
                CurrentProductDisplayHelper.SelectedColor = Integer.Parse(ProductColorDropDownList.SelectedValue)
            End If

            If ProductSizeDropDownList.Items.Count < 1 OrElse ProductSizeDropDownList.SelectedValue = String.Empty Then
                CurrentProductDisplayHelper.SelectedSize = Settings.NoSizeID
            Else
                CurrentProductDisplayHelper.SelectedSize = Integer.Parse(ProductSizeDropDownList.SelectedValue)
            End If

            If Not Integer.TryParse(ProductQuantityTextBox.Text, CurrentProductDisplayHelper.SelectedQuantity) Then
                CurrentProductDisplayHelper.SelectedQuantity = 1
            End If

            CurrentProductDisplayHelper.ProductAddingToCart(Sender, New EventArgs())

            Select Case CurrentProductDisplayHelper.CurrentAddShoppingCartItemState
                Case AddShoppingCartItemState.AddItem
                    Response.RedirectToRoute("RouteShoppingCartA")
                Case AddShoppingCartItemState.AddMembership
                    Response.Redirect(String.Format("~/CCCommerce/Membership/MemberApp.aspx?ProductID={0}", CurrentProductDisplayHelper.SelectedProductId))
                Case Else
                    StatusMessageLabel.InnerText = String.Format("{0}", CurrentProductDisplayHelper.StatusMessage)
                    StatusMessageLabel.Visible = True
            End Select
        End Sub

        'Public Sub DetailsAddToCart(ByVal Sender As Object, ByVal Args As ProductControlEventArgs) Handles Me.AddToCart
        '    Dim CurrentShoppingCartController As New ShoppingCartController()
        '    Dim CurrentRegisteredUserController As New RegisteredUserController
        '    Dim CurrentRegisteredUser As RegisteredUser
        '    Dim CurrentUserName As String
        '    Dim CurrentSessionId As String = Session("SessionId").ToString

        '    If IsMembership Then
        '        If Args.IsRegisteredUser Then

        '            CurrentUserName = Security.Membership.GetUser.UserName
        '            CurrentRegisteredUser = CurrentRegisteredUserController.GetByUserName(CurrentUserName)

        '            If RegisteredUserController.UserHasMembership(CurrentRegisteredUser.UserName) Or ShoppingCartController.CartHasMembership(CurrentRegisteredUser.UserID) Then
        '                RaiseEvent AddMembership(Sender, New ProductControlEventArgs(True, True, Args.ProductID, Args.SizeID, Args.ColorID, Args.Quantity, Args.Inventory))
        '            Else
        '                RaiseEvent AddMembership(Sender, New ProductControlEventArgs(True, False, Args.ProductID, Args.SizeID, Args.ColorID, Args.Quantity, Args.Inventory))
        '            End If
        '        Else
        '            If ShoppingCartController.CartHasMembership(CurrentSessionId) Then
        '                RaiseEvent AddMembership(Sender, New ProductControlEventArgs(False, True, Args.ProductID, Args.SizeID, Args.ColorID, Args.Quantity, Args.Inventory))
        '            Else
        '                RaiseEvent AddMembership(Sender, New ProductControlEventArgs(False, False, Args.ProductID, Args.SizeID, Args.ColorID, Args.Quantity, Args.Inventory))
        '            End If
        '        End If
        '    Else
        '        Dim CurrentCartHasMembership As Boolean

        '        If Not Security.Membership.GetUser Is Nothing Then
        '            CurrentUserName = Security.Membership.GetUser.UserName
        '            CurrentRegisteredUser = CurrentRegisteredUserController.GetByUserName(CurrentUserName)

        '            CurrentCartHasMembership = ShoppingCartController.CartHasMembership(CurrentRegisteredUser.UserID)

        '            If Not CurrentCartHasMembership Then
        '                CurrentShoppingCartController.Create(CurrentSessionId, CurrentRegisteredUser.UserID, Args.ProductID, Args.ColorID, Args.SizeID, Args.Quantity)
        '            End If
        '        Else
        '            CurrentCartHasMembership = ShoppingCartController.CartHasMembership(CurrentSessionId)

        '            If Not CurrentCartHasMembership Then
        '                CurrentShoppingCartController.Create(CurrentSessionId, -1, Args.ProductID, Args.ColorID, Args.SizeID, Args.Quantity)
        '            End If
        '        End If

        '        If Not CurrentCartHasMembership Then
        '            RaiseEvent CartItemAdded(Sender, New EventArgs())
        '        End If
        '    End If

        '    CurrentRegisteredUserController.Dispose()
        '    CurrentShoppingCartController.Dispose()
        'End Sub

        'Protected Sub DetailsCartItemAdded(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.CartItemAdded
        '    Response.RedirectToRoute("RouteShoppingCartA")
        'End Sub

        'Protected Sub ProductOutOfStock(ByVal Sender As Object, ByVal Args As ProductControlEventArgs) Handles Me.OutOfStock
        '    If Args.Inventory = 0 Then
        '        StatusMessageLabel.Text = String.Format("<p style=""color: red;"">Sorry! We currently have none of those items in stock.</p>")
        '    Else
        '        StatusMessageLabel.Text = String.Format("<p style=""color: red;"">Sorry! We currently only have ( {0} ) items in stock.</p>", Args.Inventory)
        '    End If

        '    StatusMessageLabel.Visible = True
        'End Sub

        'Protected Sub DetailsAddMembership(ByVal Sender As Object, ByVal Args As ProductControlEventArgs) Handles Me.AddMembership
        '    'Dim CurrentShoppingDetailsControl As ShoppingDetailsControl = CType(Me.Master.FindControl("MasterShoppingDetailsControl"), ShoppingDetailsControl)

        '    'CurrentShoppingDetailsControl.LoadDetails()

        '    StatusMessageLabel.Text = ""

        '    If Args.IsRegisteredUser Then
        '        If Not Args.HasMembership Then
        '            Response.Redirect("~/CCCommerce/Membership/MemberApp.aspx?ProductID=" & Args.ProductID)
        '        Else
        '            StatusMessageLabel.Text = "You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account."
        '            StatusMessageLabel.Visible = True
        '        End If
        '    Else
        '        If Not Args.HasMembership Then
        '            Response.Redirect("~/CCCommerce/Membership/MemberApp.aspx?ProductID=" & Args.ProductID)
        '        Else
        '            StatusMessageLabel.Text = "You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account."
        '            StatusMessageLabel.Visible = True
        '        End If

        '    End If

        'End Sub

        'Public Property ProductId() As Integer
        '    Get
        '        If Not ViewState("ProductID") Is Nothing Then
        '            Return ViewState("ProductID") ' _productId
        '        Else
        '            Return -1
        '        End If
        '    End Get
        '    Set(ByVal Value As Integer)
        '        If ViewState("ProductID") Is Nothing Then
        '            ViewState.Add("ProductID", Value)
        '        Else
        '            ViewState("ProductID") = Value '_productID = value
        '        End If
        '    End Set
        'End Property

        'Public Property IsMembership() As Boolean
        '    Get
        '        If Not ViewState("IsMembership") Is Nothing Then
        '            Return ViewState("IsMembership") ' _productId
        '        Else
        '            Return -1
        '        End If
        '    End Get
        '    Set(ByVal Value As Boolean)
        '        If ViewState("IsMembership") Is Nothing Then
        '            ViewState.Add("IsMembership", Value)
        '        Else
        '            ViewState("IsMembership") = Value '_productID = value
        '        End If
        '    End Set
        'End Property

    End Class

End Namespace