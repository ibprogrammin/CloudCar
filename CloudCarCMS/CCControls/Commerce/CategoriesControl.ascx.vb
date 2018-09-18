Imports CloudCar.CCFramework.Commerce

Namespace CCControls.Commerce

    Public Class CategoriesControl
        Inherits UserControl

        Protected Overrides Sub OnLoad(ByVal Args As EventArgs)
            If Not Page.IsPostBack Then
                LoadCategories()
            End If
        End Sub

        Private Sub LoadCategories()
            Select Case OrderMode
                Case CategoryOrderMode.None
                    CategoryRepeater.DataSource = New CategoryController().GetCategoriesWithProducts()
                Case CategoryOrderMode.Name
                    CategoryRepeater.DataSource = New CategoryController().GetCategoriesWithProducts().OrderBy(Function(Category) Category.Name)
                Case CategoryOrderMode.Priority
                    'TODO Implement priority for category display
                Case CategoryOrderMode.Sales
                    CategoryRepeater.DataSource = New CategoryController().GetCategoriesWithProducts().OrderByDescending(Function(Category) Category.Products.Sum(Function(Product) Product.OrderItems.Where(Function(OrderItem) OrderItem.Order.ApprovalState = 1).Sum(Function(OrderItem) OrderItem.Quantity * OrderItem.Price)))
                Case Else
                    CategoryRepeater.DataSource = New CategoryController().GetCategoriesWithProducts()
            End Select

            CategoryRepeater.DataBind()
        End Sub

        Public Property OrderMode As CategoryOrderMode
            Get
                If Not ViewState("OrderMode") Is Nothing Then
                    Return CType(ViewState("OrderMode"), CategoryOrderMode)
                Else
                    Return CategoryOrderMode.None
                End If
            End Get
            Set(Value As CategoryOrderMode)
                If ViewState("OrderMode") Is Nothing Then
                    ViewState.Add("OrderMode", Value)
                Else
                    ViewState("OrderMode") = Value
                End If
            End Set
        End Property

    End Class

    Public Enum CategoryOrderMode
        None = 0
        Name = 1
        Priority = 2
        Sales = 3
    End Enum

End Namespace