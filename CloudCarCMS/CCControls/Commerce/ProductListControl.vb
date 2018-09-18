Imports CloudCar.CCControls.Commerce.ProductControls
Imports CloudCar.CCFramework.Model
Imports System.ComponentModel
Imports CloudCar.CCFramework.Commerce.ShoppingCart
Imports CloudCar.CCFramework.Core

Namespace CCControls.Commerce

    Public MustInherit Class ProductListControl
        Inherits UserControl

        Protected WithEvents StatusMessage As Label
        Protected WithEvents LargeProductRepeater As Repeater
        Protected WithEvents SmallProductRepeater As Repeater

        Protected MustOverride Sub LoadProducts()

        Protected ProductListControlProperties As New ProductListControlProperties
        Protected ProductListDataContext As New CommerceDataContext
        Protected ListControlProducts As List(Of Product)

        <Bindable(True)> _
        <Category("Attributes")> _
        <DefaultValue("")> _
        <Localizable(True)> _
        Public Property Count As Integer
            Get
                Return ProductListControlProperties.Count
            End Get
            Set(ByVal Value As Integer)
                ProductListControlProperties.Count = Value
                SaveControlState()
            End Set
        End Property

        <Bindable(True)> _
        <Category("Attributes")> _
        <DefaultValue("")> _
        <Localizable(True)> _
        Public Property DisplaySize As ProductDisplaySize
            Get
                Return ProductListControlProperties.DisplaySize
            End Get
            Set(ByVal Value As ProductDisplaySize)
                ProductListControlProperties.DisplaySize = Value
                SaveControlState()
            End Set
        End Property

        Protected Overrides Sub OnLoad(ByVal Args As EventArgs)
            If Not Page.IsPostBack Then
                LoadProducts()
                BindData()
            End If
        End Sub

        Protected Sub BindData()
            If ListControlProducts.Count > 0 Then
                Select Case DisplaySize
                    Case ProductDisplaySize.Large
                        LargeProductRepeater.DataSource = ListControlProducts
                        LargeProductRepeater.DataBind()
                        LargeProductRepeater.Visible = True
                    Case Else
                        SmallProductRepeater.DataSource = ListControlProducts
                        SmallProductRepeater.DataBind()
                        SmallProductRepeater.Visible = True
                End Select
            End If
        End Sub

        Protected Overrides Sub OnInit(ByVal Args As EventArgs)
            Page.RegisterRequiresControlState(Me)
            MyBase.OnInit(Args)
        End Sub

        Protected Overrides Sub LoadControlState(ByVal SavedState As Object)
            Dim Value As ProductListControlProperties = CType(SavedState, ProductListControlProperties)

            Count = Value.Count
            DisplaySize = Value.DisplaySize
        End Sub

        Protected Overrides Function SaveControlState() As Object
            Return ProductListControlProperties
        End Function

        Protected Sub ProductControlOutOfStock(ByVal Sender As Object, ByVal Args As ProductControlEventArgs)
            If Args.Inventory = 0 Then
                StatusMessage.Text = String.Format("<p style=""color: red;"">Sorry! We currently have none of those items in stock.</p>")
            Else
                StatusMessage.Text = String.Format("<p style=""color: red;"">Sorry! We currently only have ( {0} ) items in stock.</p>", Args.Inventory)
            End If

            StatusMessage.Visible = True
        End Sub

        Protected Sub ProductControlAddMembership(ByVal Sender As Object, ByVal Args As ProductControlEventArgs)
            If Args.IsRegisteredUser Then
                If Not Args.HasMembership Then
                    Response.Redirect(String.Format("~/CCCommerce/Membership/MemberApp.aspx?ProductID={0}", Args.ProductID))
                Else
                    StatusMessage.Text = String.Format("You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account.")
                End If
            Else
                If Not Args.HasMembership Then
                    Response.Redirect(String.Format("~/CCCommerce/Membership/MemberApp.aspx?ProductID={0}", Args.ProductID))
                Else
                    StatusMessage.Text = String.Format("You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account.")
                End If

            End If

        End Sub

        Protected Overrides Sub OnUnload(Args As EventArgs)
            ProductListDataContext.Dispose()
        End Sub

        Protected Sub AddToCartButtonClick(ByVal Sender As Object, ByVal Args As EventArgs)
            Dim CurrentProductShoppingCartMediator As New ProductShoppingCartMediator(Session("SessionId").ToString)

            CurrentProductShoppingCartMediator.SelectedProductId = CInt(Sender.Attributes("ProductId"))
            CurrentProductShoppingCartMediator.SelectedColor = Settings.NoColorID
            CurrentProductShoppingCartMediator.SelectedSize = Settings.NoSizeID
            CurrentProductShoppingCartMediator.SelectedQuantity = 1

            CurrentProductShoppingCartMediator.ProductAddingToCart(Sender, New EventArgs())

            Select Case CurrentProductShoppingCartMediator.CurrentAddShoppingCartItemState
                Case AddShoppingCartItemState.AddItem
                    Response.RedirectToRoute("RouteShoppingCartA")
                Case AddShoppingCartItemState.AddMembership
                    Response.Redirect(String.Format("~/CCCommerce/Membership/MemberApp.aspx?ProductID={0}", CurrentProductShoppingCartMediator.SelectedProductId))
                Case Else
                    'StatusMessageLabel.Text = String.Format("<h4>{0}</h4>", _CurrentProductShoppingCartMediator.StatusMessage)
                    'StatusMessageLabel.Visible = True
            End Select
        End Sub


    End Class

    <Serializable()> _
    Public Structure ProductListControlProperties
        Public Count As Integer
        Public DisplaySize As ProductDisplaySize
    End Structure

    Public Enum ProductDisplaySize
        None = 0
        Small = 1
        Medium = 2
        Large = 3
    End Enum

End Namespace