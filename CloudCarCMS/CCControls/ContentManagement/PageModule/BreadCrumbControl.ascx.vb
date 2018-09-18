Imports CloudCar.CCFramework.ContentManagement
Imports CloudCar.CCFramework.ContentManagement.EventsModule
Imports CloudCar.CCFramework.ContentManagement.NewsModule
Imports CloudCar.CCFramework.Commerce

Namespace CCControls.ContentManagement.PageModule

    Public Class BreadCrumbControl
        Inherits UserControl

        Private _PageIdSet As Boolean
        Private _BreadCrumbTypeSet As Boolean

        Public Event PageIdSet()
        Public Event BreadBrumbTypeSet()

        Private Const _BackBreadCrumb As String = "&laquo; <a onclick=""history.go(-1);return true;"" style=""cursor: pointer;"">Back to results</a> | "

        Public Sub New()
            _PageIdSet = False
            _BreadCrumbTypeSet = False
        End Sub

        Public Property PageId() As Integer
            Get
                Return CInt(ViewState("PageId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageId") = Value

                RaiseEvent PageIdSet()
            End Set
        End Property

        Public Property ProductId() As Integer
            Get
                Return CInt(ViewState("ProductId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("ProductId") = Value

                RaiseEvent PageIdSet()
            End Set
        End Property

        Public Property CategoryId() As Integer
            Get
                Return CInt(ViewState("CategoryId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("CategoryId") = Value

                RaiseEvent PageIdSet()
            End Set
        End Property

        Public Property ColorId() As Integer
            Get
                Return CInt(ViewState("ColorId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("ColorId") = Value

                RaiseEvent PageIdSet()
            End Set
        End Property

        Public Property BreadCrumbType() As BreadCrumbType
            Get
                Return CType(ViewState("BreadCrumbType"), BreadCrumbType)
            End Get
            Set(ByVal Value As BreadCrumbType)
                ViewState("BreadCrumbType") = Value

                RaiseEvent BreadBrumbTypeSet()
            End Set
        End Property

        Private Sub LoadBreadCrumb()
            Select Case BreadCrumbType
                Case BreadCrumbType.ContentPage
                    BreadCrumbLiteral.Text = ContentPageController.GetBreadCrumb(PageId, "Home")
                Case BreadCrumbType.EventPage
                    BreadCrumbLiteral.Text = EventsController.GetBreadCrumb(PageId)
                Case BreadCrumbType.NewsPage
                    BreadCrumbLiteral.Text = NewsController.GetBreadCrumb(PageId)
                Case BreadCrumbType.StoreProductPage
                    BreadCrumbLiteral.Text = _BackBreadCrumb
                    BreadCrumbLiteral.Text &= ProductController.GetBreadCrumb(ProductId)
                Case BreadCrumbType.StoreClearancePage
                    BreadCrumbLiteral.Text = ProductController.GetClearanceBreadCrumb("Clearance")
                Case BreadCrumbType.StoreTopSellerPage
                    BreadCrumbLiteral.Text = ProductController.GetTopSellerBreadCrumb("Top Sellers")
                Case BreadCrumbType.StoreCategoryPage
                    BreadCrumbLiteral.Text = CategoryController.GetBreadCrumb(CategoryId)
                Case BreadCrumbType.StoreColorPage
                    BreadCrumbLiteral.Text = ColourController.GetBreadCrumb(ColorId)
                Case Else
                    BreadCrumbLiteral.Text = ContentPageController.GetBreadCrumb(PageId, "Home")
            End Select
        End Sub

        Private Sub PageBreadBrumbTypeSet() Handles Me.BreadBrumbTypeSet
            _BreadCrumbTypeSet = True

            If _PageIdSet Then
                LoadBreadCrumb()
            End If
        End Sub

        Private Sub PagePageIdSet() Handles Me.PageIdSet
            _PageIdSet = True

            LoadBreadCrumb()
        End Sub

    End Class

    Public Enum BreadCrumbType
        ContentPage = 0
        EventPage = 1
        NewsPage = 2
        StoreCategoryPage = 3
        StoreBrandPage = 4
        StoreProductPage = 5
        StoreTopSellerPage = 6
        StoreClearancePage = 7
        StoreColorPage = 8
    End Enum

End Namespace