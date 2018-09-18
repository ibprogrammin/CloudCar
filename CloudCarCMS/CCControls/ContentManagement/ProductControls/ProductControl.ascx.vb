Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCControls.ContentManagement.ProductControls

    Partial Public Class ProductControl
        Inherits UserControl

        Private NoColorID As Integer = Integer.Parse(ConfigurationManager.AppSettings("NoColorID"))
        Private NoSizeID As Integer = Integer.Parse(ConfigurationManager.AppSettings("NoSizeID"))
        Private NoImageUrl As String = ConfigurationManager.AppSettings("NoImageUrl")

        Private Sub LoadProduct()
            Dim product As Product = New ProductController().GetElement(ProductID)

            Dim Columns As Integer = 3 'product.Category.Columns

            Select Case Columns
                Case 1
                    pnlProduct.Attributes.CssStyle.Add("width", "620px")
                    pnlProduct.Attributes.CssStyle.Add("height", "235px")
                    pnlProduct.Attributes.CssStyle.Add("margin-top", "20px")
                Case 2
                    pnlProduct.Attributes.CssStyle.Add("width", "280px")
                    pnlProduct.Attributes.CssStyle.Add("height", "235px")
                    pnlProduct.Attributes.CssStyle.Add("margin-left", "30px")
                    pnlProduct.Attributes.CssStyle.Add("margin-top", "20px")
                Case 3
                    pnlProduct.Attributes.CssStyle.Add("width", "160px")
                    pnlProduct.Attributes.CssStyle.Add("height", "200px")
                    pnlProduct.Attributes.CssStyle.Add("margin-left", "30px")
                    pnlProduct.Attributes.CssStyle.Add("margin-right", "30px")
                    pnlProduct.Attributes.CssStyle.Add("margin-top", "20px")
                Case Else
                    pnlProduct.Attributes.CssStyle.Add("width", "160px")
                    pnlProduct.Attributes.CssStyle.Add("height", "200px")
                    pnlProduct.Attributes.CssStyle.Add("margin-left", "30px")
                    pnlProduct.Attributes.CssStyle.Add("margin-right", "30px")
                    pnlProduct.Attributes.CssStyle.Add("margin-top", "20px")
            End Select

            hlProductName.Text = CCFramework.Core.ApplicationFunctions.StripShortString(product.Name, 34)
            hlProductName.ToolTip = product.Name
            hlProductName.NavigateUrl = "/Home/Products/" & product.Category.Permalink & "/" & product.Permalink & ".html"

            If Not product.DefaultImageID = Nothing Then
                imgProduct.Src = "/images/db/" & product.DefaultImageID & "/293/" & product.Permalink & ".jpg"
                imgProduct.Alt = product.Name & " made by " & product.Brand.Name
            Else
                imgProduct.Src = NoImageUrl
            End If
        End Sub

        Public Property ProductID() As Integer
            Get
                If Not ViewState("ProductID") Is Nothing Then
                    Return CInt(ViewState("ProductID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Integer)
                If ViewState("ProductID") Is Nothing Then
                    ViewState.Add("ProductID", value)
                Else
                    ViewState("ProductID") = value '_productID = value
                End If

                LoadProduct()
            End Set
        End Property

    End Class

End Namespace