Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce.Product

Namespace CCAdmin.Commerce

    Partial Public Class ProductDetails
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()

                StoredColors = Nothing
                StoredSizes = Nothing

                hlAddBrand.NavigateUrl = hlAddBrand.NavigateUrl & "?Product=NEW"
                hlAddCategory.NavigateUrl = hlAddCategory.NavigateUrl & "?Product=NEW"
                hlAddColor.NavigateUrl = hlAddColor.NavigateUrl & "?Product=NEW"
                hlAddSize.NavigateUrl = hlAddSize.NavigateUrl & "?Product=NEW"

                Dim _productID As Integer

                If Integer.TryParse(Request.QueryString("Product"), _productID) Then
                    LoadProduct(_productID)
                End If
            End If

            lblMessage.Visible = False
        End Sub

        Private Sub LoadProduct(ByVal ProductId As Integer)
            Dim CurrentProduct As Product = New ProductController().GetElement(ProductId)

            litProductID.Text = CurrentProduct.ID.ToString
            hfProductID.Value = CurrentProduct.ID.ToString

            hfPDIID.Value = CurrentProduct.DefaultImageID.ToString
            hfPHeaderImageID.Value = CurrentProduct.HeaderImageID.ToString
            hfDigitalMediaFile.Value = CurrentProduct.Filename

            If Not CurrentProduct.HeaderImageID = 0 Then
                Try
                    lblHeaderImageLocation.Text = String.Format("/images/db/{0}/full/{1}", CurrentProduct.HeaderImageID, PictureController.GetPicture(CurrentProduct.HeaderImageID).PictureFileName)
                    lblHeaderImageLocation.Visible = True

                    imgPHI.Src = String.Format("/images/db/{0}/220/{1}", CurrentProduct.HeaderImageID, PictureController.GetPicture(CurrentProduct.HeaderImageID).PictureFileName)
                    imgPHI.Visible = True
                Catch ex As Exception

                End Try
            End If

            If Not CurrentProduct.DefaultImageID = 0 Then
                Try
                    lblDefaultImageLocation.Text = String.Format("/images/db/{0}/full/{1}", CurrentProduct.DefaultImageID, PictureController.GetPicture(CurrentProduct.DefaultImageID).PictureFileName)
                    lblDefaultImageLocation.Visible = True

                    imgPDI.Src = String.Format("/images/db/{0}/220/{1}", CurrentProduct.DefaultImageID, PictureController.GetPicture(CurrentProduct.DefaultImageID).PictureFileName)
                    imgPDI.Visible = True
                Catch Ex As Exception

                End Try
            End If

            If Not CurrentProduct.Filename = String.Empty Then
                lblDigitalFileLocation.Text = CCFramework.Core.Settings.ProductUploadPath & CurrentProduct.Filename
                lblDefaultImageLocation.Visible = True
            End If

            hlAddBrand.NavigateUrl = hlAddBrand.NavigateUrl & "?Product=" & CurrentProduct.ID
            hlAddCategory.NavigateUrl = hlAddCategory.NavigateUrl & "?Product=" & CurrentProduct.ID
            hlAddColor.NavigateUrl = hlAddColor.NavigateUrl & "?Product=" & CurrentProduct.ID
            hlAddSize.NavigateUrl = hlAddSize.NavigateUrl & "?Product=" & CurrentProduct.ID

            If CurrentProduct.Active Then
                ddlPActive.Style.Item("background-color") = "#E6FFD2"
            Else
                ddlPActive.Style.Item("background-color") = "none"
            End If

            ddlPActive.SelectedValue = CurrentProduct.Active.ToString
            ddlPBrand.SelectedValue = CurrentProduct.BrandID.ToString
            ddlPCategory.SelectedValue = CurrentProduct.CategoryID.ToString
            ddlTopSeller.SelectedValue = CurrentProduct.TopSeller.ToString
            ddlClearance.SelectedValue = CurrentProduct.Clearance.ToString
            ddlIsMembership.SelectedValue = CurrentProduct.Membership.ToString
            ddlIsDigitalMedia.SelectedValue = CurrentProduct.IsDigitalMedia.ToString
            ddlTrackInventory.SelectedValue = CurrentProduct.TrackInventory.ToString
            'ddlProductAvailability.SelectedValue = product.AreaRestricted

            ProductNameTextBox.Text = CurrentProduct.Name
            txtPSKU.Text = CurrentProduct.SKU
            txtPermalink.Text = Server.UrlDecode(CurrentProduct.Permalink)
            txtPDescription.Text = CurrentProduct.Description
            txtPKeywords.Text = CurrentProduct.Keywords
            DetailsTextArea.InnerText = CurrentProduct.Details

            PricingUnitTextBox.Text = CurrentProduct.PricingUnit
            txtPCost.Text = CurrentProduct.Cost.ToString
            ListPriceTextBox.Text = CurrentProduct.ListPrice.ToString
            txtPPrice.Text = CurrentProduct.Price.ToString
            txtPWarehousePrice.Text = CurrentProduct.PriceB.ToString
            txtPAssociatePrice.Text = CurrentProduct.PriceC.ToString
            DiscountTextBox.Text = CurrentProduct.Discount.ToString

            txtPWeight.Text = CurrentProduct.Weight.ToString
            txtPLength.Text = CurrentProduct.Length.ToString
            txtPWidth.Text = CurrentProduct.Width.ToString
            txtPHeight.Text = CurrentProduct.Height.ToString

            txtBrowserTitle.Text = CurrentProduct.BrowserTitle
            txtAlternateTag.Text = CurrentProduct.ImageAlt

            Dim pColors As List(Of ProductColor) = New ProductColourController().GetColors(CurrentProduct.ID)

            If Not pColors Is Nothing Then
                For Each c As ProductColor In pColors
                    RemoveColor(c.ColorID)
                Next
            End If

            Dim pSizes As List(Of Size) = New ProductController().GetSizes(CurrentProduct.ID)

            If Not pSizes Is Nothing Then
                For Each s As Size In pSizes
                    RemoveSize(s.ID)
                Next
            End If

            Dim pRecomendations As List(Of ProductRecomendation) = New ProductRecomendationController().GetElements(CurrentProduct.ID)

            If Not pRecomendations Is Nothing Then
                For Each r As ProductRecomendation In pRecomendations
                    RemoveRecomendedProduct(r.RecomendedProductId)
                Next
            End If

            StoredColors = Nothing
            StoredSizes = Nothing
            StoredRecomendations = Nothing

            dtgPAddingColors.DataSource = Nothing
            dtgPAddingColors.DataBind()

            dtgPAddingSize.DataSource = Nothing
            dtgPAddingSize.DataBind()

            AddingRecomendedProductDataGrid.DataSource = Nothing
            AddingRecomendedProductDataGrid.DataBind()

            dtgPSelectedColors.DataSource = pColors
            dtgPSelectedColors.DataBind()

            dtgSelectedSizes.DataSource = pSizes
            dtgSelectedSizes.DataBind()

            SelectingRecomendedProductDataGrid.DataSource = pRecomendations
            SelectingRecomendedProductDataGrid.DataBind()

            If Not ProductController.ProductHasOrderItems(CurrentProduct.ID) Then
                btnDeleteProduct.Enabled = True
            Else
                btnDeleteProduct.Enabled = False
            End If

            'tblProductAvailability.Visible = True

            'dgAvailableAreas.DataSource = SMCommerce.ProductAvailabilityController.GetProductAvailabilityByProductID(product.ID)
            'dgAvailableAreas.DataBind()

        End Sub

        Private Function SaveProduct() As Integer
            Dim CurrentProductId As Integer

            Dim Name As String = ProductNameTextBox.Text
            Dim ProductSku As String = txtPSKU.Text
            Dim Details As String = DetailsTextArea.InnerText
            Dim Permalink As String = Server.UrlEncode(txtPermalink.Text)
            Dim Description As String = txtPDescription.Text
            Dim Keywords As String = txtPKeywords.Text
            Dim BrowserTitle As String = txtBrowserTitle.Text
            Dim AlternateTag As String = txtAlternateTag.Text

            Dim PricingUnit As String = PricingUnitTextBox.Text

            Dim Cost As Decimal
            If Decimal.TryParse(txtPCost.Text, Cost) = False Then
                Cost = 0D
            End If
            Dim ListPrice As Decimal
            If Decimal.TryParse(ListPriceTextBox.Text, ListPrice) = False Then
                ListPrice = 0D
            End If
            Dim Price As Decimal
            If Decimal.TryParse(txtPPrice.Text, Price) = False Then
                Price = 0D
            End If
            Dim WarehousePrice As Decimal
            If Decimal.TryParse(txtPWarehousePrice.Text, WarehousePrice) = False Then
                WarehousePrice = 0D
            End If
            Dim AssociatePrice As Decimal
            If Decimal.TryParse(txtPAssociatePrice.Text, AssociatePrice) = False Then
                AssociatePrice = 0D
            End If
            Dim Discount As Decimal
            If Decimal.TryParse(DiscountTextBox.Text, Discount) = False Then
                Discount = 0D
            End If
            Dim Weight As Decimal
            If Decimal.TryParse(txtPWeight.Text, Weight) = False Then
                Weight = 0D
            End If
            Dim Length As Decimal
            If Decimal.TryParse(txtPLength.Text, Length) = False Then
                Length = 0D
            End If
            Dim Width As Decimal
            If Decimal.TryParse(txtPWidth.Text, Width) = False Then
                Width = 0D
            End If
            Dim Height As Decimal
            If Decimal.TryParse(txtPHeight.Text, Height) = False Then
                Height = 0D
            End If

            Dim Active As Boolean = CBool(ddlPActive.SelectedValue)
            Dim BrandId As Integer = CInt(ddlPBrand.SelectedValue)
            Dim CategoryId As Integer = CInt(ddlPCategory.SelectedValue)
            Dim TopSeller As Boolean = CBool(ddlTopSeller.SelectedValue)
            Dim Clearance As Boolean = CBool(ddlClearance.SelectedValue)
            Dim Membership As Boolean = CBool(ddlIsMembership.SelectedValue)
            Dim TrackInventory As Boolean = CBool(ddlTrackInventory.SelectedValue)
            Dim IsDigitalMedia As Boolean = CBool(ddlIsDigitalMedia.SelectedValue)
            'Dim RestrictProductAvailability As Boolean = ddlProductAvailability.SelectedValue

            Dim DigitalMediaFilename As String = ""
            If Not fuDigitalMediaFile.PostedFile.FileName = String.Empty Then
                DigitalMediaFilename = fuDigitalMediaFile.PostedFile.FileName
                fuDigitalMediaFile.PostedFile.SaveAs(MapPath(CCFramework.Core.Settings.ProductUploadPath & DigitalMediaFilename))
            End If

            'Dim files As OboutInc.FileUpload.OboutFileCollection = fupProgress.Files

            'For i As Integer = 1 To files.Count
            '    Dim file As OboutInc.FileUpload.OboutPostedFile = files(i)
            '    Dim filename As String = System.IO.Path.GetFileName(file.FileName)

            '    file.SaveAs(MapPath(SMCore.Settings.ProductUploadPath & filename))
            'Next

            Dim DefaultImageId As Integer
            If fuPDefaultImage.HasFile Then
                DefaultImageId = CCFramework.Core.ImageFunctions.UploadImage(fuPDefaultImage)
            Else
                DefaultImageId = 0
            End If

            Dim HeaderImageId As Integer
            If fuPHeaderImage.HasFile Then
                HeaderImageId = CCFramework.Core.ImageFunctions.UploadImage(fuPHeaderImage)
            Else
                HeaderImageId = 0
            End If

            Dim CurrentProductController As New ProductController()

            If Not hfProductID.Value Is Nothing And Integer.TryParse(hfProductID.Value, CurrentProductId) Then
                CurrentProductController.Update(CurrentProductId, ProductSku, Name, Details, Description, Keywords, BrowserTitle, AlternateTag, PricingUnit, Cost, ListPrice, Price, WarehousePrice, AssociatePrice, Discount, Weight, Length, Width, Height, Active, BrandId, CategoryId, DefaultImageId, HeaderImageId, TopSeller, Clearance, Membership, Permalink, TrackInventory, IsDigitalMedia, DigitalMediaFilename, 0)
            Else
                CurrentProductId = CurrentProductController.Create(ProductSku, Name, Details, Description, Keywords, BrowserTitle, AlternateTag, PricingUnit, Cost, ListPrice, Price, WarehousePrice, AssociatePrice, Discount, Weight, Length, Width, Height, Active, BrandId, CategoryId, DefaultImageId, HeaderImageId, TopSeller, Clearance, Membership, Permalink, TrackInventory, IsDigitalMedia, DigitalMediaFilename, 0)
            End If

            For Each item In StoredColors
                CurrentProductController.AddColor(CurrentProductId, item.ColorID, item.ImageID)
            Next

            For Each item In StoredSizes
                CurrentProductController.AddSize(CurrentProductId, item.SizeID)
            Next

            Dim CurrentProductRecomendationController As New ProductRecomendationController()
            For Each item In StoredRecomendations
                CurrentProductRecomendationController.Create(CurrentProductId, item.RecomendedProductId)
            Next

            lblMessage.Text = String.Format("Product saved successfully! Product ID ({0}) was commited to the server.", CurrentProductId)
            lblMessage.Visible = True

            Return CurrentProductId
        End Function

        Private Sub DeleteProduct()
            Dim ProductID As Integer

            If Not hfProductID.Value Is Nothing And Integer.TryParse(hfProductID.Value, ProductID) Then
                Dim product As New ProductController

                Dim Success As Boolean = product.Delete(ProductID)

                If Success Then
                    lblMessage.Text = "The Product was successfully deleted!"
                    lblMessage.Visible = True

                    ClearControls()
                Else
                    lblMessage.Text = "There was an error deleting the product! This product may be associated with an order, you may want to consider setting the product to inactive instead."
                    lblMessage.Visible = True
                End If
            End If
        End Sub

        Private Sub RemoveColor(ByVal ColorId As Integer)
            ddlPAddColour.Items.Remove(ddlPAddColour.Items.FindByValue(ColorId.ToString))
        End Sub

        Private Sub RemoveSize(ByVal SizeID As Integer)
            ddlPAddSize.Items.Remove(ddlPAddSize.Items.FindByValue(SizeID.ToString))
        End Sub

        Private Sub RemoveRecomendedProduct(ByVal RecomendedProductId As Integer)
            RecomendedProductDropDown.Items.Remove(RecomendedProductDropDown.Items.FindByValue(RecomendedProductId.ToString))
        End Sub

        'Private Sub RemoveProductOption(ByVal ProductOptionId As Integer)
        '    ProductOptionsDropDown.Items.Remove(ProductOptionsDropDown.Items.FindByValue(ProductOptionId.ToString))
        'End Sub

        Private Sub RefreshDataSources()
            ddlPBrand.Items.Clear()
            'ddlPBrand.Items.Add(New ListItem("Brand", ""))
            ddlPBrand.AppendDataBoundItems = True
            ddlPBrand.DataSource = New BrandController().GetElements()
            ddlPBrand.DataBind()

            ddlPCategory.Items.Clear()
            'ddlPCategory.Items.Add(New ListItem("Category", ""))
            ddlPCategory.AppendDataBoundItems = True
            ddlPCategory.DataSource = New CategoryController().GetElements()
            ddlPCategory.DataBind()

            ddlPAddColour.Items.Clear()
            ddlPAddColour.Items.Add(New ListItem("Colour", ""))
            ddlPAddColour.AppendDataBoundItems = True
            ddlPAddColour.DataSource = New ColourController().GetElements()
            ddlPAddColour.DataBind()

            ddlPAddSize.Items.Clear()
            ddlPAddSize.Items.Add(New ListItem("Size", ""))
            ddlPAddSize.AppendDataBoundItems = True
            ddlPAddSize.DataSource = New SizeController().GetElements()
            ddlPAddSize.DataBind()

            RecomendedProductDropDown.Items.Clear()
            RecomendedProductDropDown.Items.Add(New ListItem("Recomendation", ""))
            RecomendedProductDropDown.AppendDataBoundItems = True
            RecomendedProductDropDown.DataSource = New ProductController().GetElements
            RecomendedProductDropDown.DataBind()

            'ProductOptionsDropDown.Items.Clear()
            'ProductOptionsDropDown.Items.Add(New ListItem("Options", ""))
            'ProductOptionsDropDown.AppendDataBoundItems = True
            'ProductOptionsDropDown.DataSource = New ProductController().GetElements
            'ProductOptionsDropDown.DataBind()
        End Sub

        Private Sub ClearControls()
            hlAddBrand.NavigateUrl = hlAddBrand.NavigateUrl & "?Product=NEW"
            hlAddCategory.NavigateUrl = hlAddCategory.NavigateUrl & "?Product=NEW"
            hlAddColor.NavigateUrl = hlAddColor.NavigateUrl & "?Product=NEW"
            hlAddSize.NavigateUrl = hlAddSize.NavigateUrl & "?Product=NEW"

            litProductID.Text = "NEW"
            hfProductID.Value = ""
            hfPDIID.Value = ""
            hfDigitalMediaFile.Value = ""

            ddlPActive.SelectedValue = "False"
            ddlPBrand.SelectedIndex = 0
            ddlPCategory.SelectedIndex = 0
            ddlIsMembership.SelectedIndex = 0
            ddlTrackInventory.SelectedValue = "0"
            ddlIsDigitalMedia.SelectedValue = ""
            'ddlProductAvailability.SelectedValue = ""

            DetailsTextArea.InnerText = ""

            lblDefaultImageLocation.Text = ""
            lblHeaderImageLocation.Text = ""
            lblDigitalFileLocation.Text = ""

            lblDefaultImageLocation.Visible = False
            lblHeaderImageLocation.Visible = False
            lblDigitalFileLocation.Visible = False

            ProductNameTextBox.Text = ""
            txtPSKU.Text = ""
            DetailsTextArea.InnerText = ""
            txtPermalink.Text = ""
            txtPDescription.Text = ""
            txtPKeywords.Text = ""

            PricingUnitTextBox.Text = ""
            txtPCost.Text = ""
            ListPriceTextBox.Text = ""
            txtPPrice.Text = ""
            txtPWarehousePrice.Text = ""
            txtPAssociatePrice.Text = ""
            DiscountTextBox.Text = ""

            txtPWeight.Text = ""
            txtPLength.Text = ""
            txtPWidth.Text = ""
            txtPHeight.Text = ""

            'txtAreaDescription.Text = ""
            'txtAreaPrefixHigh.Text = ""
            'txtAreaPrefixLow.Text = ""
        End Sub

        Protected Function GetColorName(ByVal ColorID As Integer) As String
            Dim color As Color = New ColourController().GetElement(ColorID)

            Return color.Name
        End Function

        Protected Function GetColorImage(ByVal ColorID As Integer) As String
            Dim productID As Integer = Integer.Parse(hfProductID.Value)

            Return New ProductController().GetProductColorImage(productID, ColorID).ToString

        End Function

        Protected Function GetSizeName(ByVal SizeID As Integer) As String
            Dim size As Size = New SizeController().GetElement(SizeID)

            Return size.Name
        End Function

        Protected Function GetProductName(ByVal ProductId As Integer) As String
            Dim CurrentProduct As Product = New ProductController().GetElement(ProductId)

            Return CurrentProduct.Name
        End Function

        Private Property StoredColors() As List(Of SessionColor)
            Get
                If Not Session("StoredColors") Is Nothing Then
                    Return CType(Session("StoredColors"), List(Of SessionColor))
                Else
                    Dim _storedColors As New List(Of SessionColor)

                    Session.Add("StoredColors", _storedColors)

                    Return CType(Session("StoredColors"), List(Of SessionColor))
                End If
            End Get
            Set(ByVal value As List(Of SessionColor))
                Session("StoredColors") = value
            End Set
        End Property

        Private Property StoredSizes() As List(Of SessionSize)
            Get
                If Not Session("StoredSizes") Is Nothing Then
                    Return CType(Session("StoredSizes"), List(Of SessionSize))
                Else
                    Dim _storedSizes As New List(Of SessionSize)

                    Session.Add("StoredSizes", _storedSizes)

                    Return CType(Session("StoredSizes"), List(Of SessionSize))
                End If
            End Get
            Set(ByVal value As List(Of SessionSize))
                Session("StoredSizes") = value
            End Set
        End Property

        Private Property StoredRecomendations() As List(Of SessionRecomendation)
            Get
                If Not Session("StoredRecomendations") Is Nothing Then
                    Return CType(Session("StoredRecomendations"), List(Of SessionRecomendation))
                Else
                    Dim _storedRecomendations As New List(Of SessionRecomendation)

                    Session.Add("StoredRecomendations", _storedRecomendations)

                    Return CType(Session("StoredRecomendations"), List(Of SessionRecomendation))
                End If
            End Get
            Set(ByVal Value As List(Of SessionRecomendation))
                Session("StoredRecomendations") = Value
            End Set
        End Property

        Private Property StoredProductOptions() As List(Of SessionProductOption)
            Get
                If Not Session("StoredProductOptions") Is Nothing Then
                    Return CType(Session("StoredProductOptions"), List(Of SessionProductOption))
                Else
                    Dim CurrentStoredProductOptions As New List(Of SessionProductOption)

                    Session.Add("StoredProductOptions", CurrentStoredProductOptions)

                    Return CType(Session("StoredProductOptions"), List(Of SessionProductOption))
                End If
            End Get
            Set(ByVal Value As List(Of SessionProductOption))
                Session("StoredProductOptions") = Value
            End Set
        End Property

        Private Sub btnAddColor_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddColor.Command
            Dim colorID As Integer

            Dim NewColor As SessionColor

            If Integer.TryParse(ddlPAddColour.SelectedValue, colorID) Then
                Dim sc = From ssc In StoredColors Where ssc.ColorID = colorID Select ssc

                If sc.Count = 0 Then
                    Dim imageID As Integer = 0

                    If fuPAddColorImage.HasFile Then
                        imageID = CCFramework.Core.ImageFunctions.UploadImage(fuPAddColorImage.PostedFile)
                        'imageID = SMCommerce.ImageFunctions.UploadImage(fuPAddColorImage)
                    End If

                    NewColor = New SessionColor(colorID, imageID)

                    If Not hfProductID.Value Is Nothing And Not hfProductID.Value = String.Empty Then
                        Dim productID As Integer = Integer.Parse(hfProductID.Value)

                        Dim ProductColor As New ProductColourController()
                        ProductColor.AddColor(productID, colorID, imageID)

                        dtgPSelectedColors.DataSource = ProductColor.GetColors(productID)
                        dtgPSelectedColors.DataBind()
                        dtgPSelectedColors.Visible = True

                        dtgPAddingColors.Visible = False
                    Else
                        StoredColors.Add(NewColor)

                        dtgPAddingColors.DataSource = StoredColors
                        dtgPAddingColors.DataBind()
                        dtgPAddingColors.Visible = True

                        dtgPSelectedColors.Visible = False
                    End If
                End If
            Else
                lblMessage.Text = "There was an error trying to add the color."
                lblMessage.Visible = True
            End If

            RemoveColor(colorID)
        End Sub

        Private Sub btnAddSize_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddSize.Command
            Dim sizeID As Integer

            Dim NewSize As SessionSize

            If Integer.TryParse(ddlPAddSize.SelectedValue, sizeID) Then
                Dim ss = From sss In StoredSizes Where sss.SizeID = sizeID Select sss

                If ss.Count = 0 Then
                    NewSize = New SessionSize(sizeID)

                    If Not hfProductID.Value Is Nothing And Not hfProductID.Value = String.Empty Then
                        Dim productID As Integer = Integer.Parse(hfProductID.Value)

                        Dim product As New ProductController()
                        product.AddSize(productID, sizeID)

                        dtgSelectedSizes.DataSource = product.GetSizes(productID)
                        dtgSelectedSizes.DataBind()
                        dtgSelectedSizes.Visible = True

                        dtgPAddingSize.Visible = False
                    Else
                        StoredSizes.Add(NewSize)

                        dtgPAddingSize.DataSource = StoredSizes
                        dtgPAddingSize.DataBind()
                        dtgPAddingSize.Visible = True

                        dtgSelectedSizes.Visible = False
                    End If


                End If
            Else
                lblMessage.Text = "There was an error trying to add the size."
                lblMessage.Visible = True
            End If

            RemoveSize(sizeID)

            dtgPAddingSize.DataSource = StoredSizes
            dtgPAddingSize.DataBind()
        End Sub

        Private Sub AddRecomendedProductButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles AddRecomendedProductButton.Click
            Dim RecomendedProductId As Integer

            Dim NewRecomendation As SessionRecomendation

            If Integer.TryParse(RecomendedProductDropDown.SelectedValue, RecomendedProductId) Then
                Dim sr = From spr In StoredRecomendations Where spr.RecomendedProductId = RecomendedProductId Select spr

                If sr.Count = 0 Then
                    NewRecomendation = New SessionRecomendation(RecomendedProductId)

                    If Not hfProductID.Value Is Nothing And Not hfProductID.Value = String.Empty Then
                        Dim ProductId As Integer = Integer.Parse(hfProductID.Value)

                        Dim CurrentProductRecomendationController As New ProductRecomendationController()
                        CurrentProductRecomendationController.Create(ProductId, RecomendedProductId)

                        SelectingRecomendedProductDataGrid.DataSource = CurrentProductRecomendationController.GetElements(ProductId)
                        SelectingRecomendedProductDataGrid.DataBind()
                        SelectingRecomendedProductDataGrid.Visible = True

                        AddingRecomendedProductDataGrid.Visible = False
                    Else
                        StoredRecomendations.Add(NewRecomendation)

                        AddingRecomendedProductDataGrid.DataSource = StoredRecomendations
                        AddingRecomendedProductDataGrid.DataBind()
                        AddingRecomendedProductDataGrid.Visible = True

                        SelectingRecomendedProductDataGrid.Visible = False
                    End If
                End If
            Else
                lblMessage.Text = "There was an error trying to add the size."
                lblMessage.Visible = True
            End If

            RemoveRecomendedProduct(RecomendedProductId)

            AddingRecomendedProductDataGrid.DataSource = StoredRecomendations
            AddingRecomendedProductDataGrid.DataBind()
        End Sub

        Public Class SessionColor
            Private _colorID As Integer
            Private _imageID As Integer

            Public Sub New(ByVal ColorID As Integer, ByVal ImageID As Integer)
                _colorID = ColorID
                _imageID = ImageID
            End Sub

            Public Property ColorID() As Integer
                Get
                    Return _colorID
                End Get
                Set(ByVal value As Integer)
                    _colorID = value
                End Set
            End Property

            Public Property ImageID() As Integer
                Get
                    Return _imageID
                End Get
                Set(ByVal value As Integer)
                    _imageID = value
                End Set
            End Property

        End Class

        Public Class SessionSize
            Private _sizeID As Integer

            Public Sub New(ByVal SizeID As Integer)
                _sizeID = SizeID
            End Sub

            Public Property SizeID() As Integer
                Get
                    Return _sizeID
                End Get
                Set(ByVal value As Integer)
                    _sizeID = value
                End Set
            End Property

        End Class

        Public Class SessionRecomendation
            Private _RecomendedProductId As Integer

            Public Sub New(ByVal RecomendedProductId As Integer)
                _RecomendedProductId = RecomendedProductId
            End Sub

            Public Property RecomendedProductId() As Integer
                Get
                    Return _RecomendedProductId
                End Get
                Set(ByVal value As Integer)
                    _RecomendedProductId = value
                End Set
            End Property

        End Class

        Public Class SessionProductOption
            Private _ProductOptionId As Integer
            Private _Title As String
            Private _Description As String
            Private _ImageId As Integer
            Private _PriceA As Decimal
            Private _PriceB As Decimal
            Private _PriceC As Decimal
            Private _Group As String

            Public Sub New(ByVal ProductOptionId As Integer, Title As String, Description As String, ImageId As Integer, PriceA As Decimal, PriceB As Decimal, PriceC As Decimal, Group As String)
                _ProductOptionId = ProductOptionId
                _Title = Title
                _Description = Description
                _ImageId = ImageId
                _PriceA = PriceA
                _PriceB = PriceB
                _PriceC = PriceC
                _Group = Group
            End Sub

            Public Property ProductOptionId() As Integer
                Get
                    Return _ProductOptionId
                End Get
                Set(ByVal Value As Integer)
                    _ProductOptionId = value
                End Set
            End Property

            Public Property Title As String
                Get
                    Return _Title
                End Get
                Set(Value As String)
                    _Title = value
                End Set
            End Property

            Public Property Description As String
                Get
                    Return _Description
                End Get
                Set(Value As String)
                    _Description = value
                End Set
            End Property

            Private Property ImageId As Integer
                Get
                    Return _ImageId
                End Get
                Set(Value As Integer)
                    _ImageId = Value
                End Set
            End Property

            Public Property PriceA As Decimal
                Get
                    Return _PriceA
                End Get
                Set(Value As Decimal)
                    _PriceA = Value
                End Set
            End Property

            Public Property PriceB As Decimal
                Get
                    Return _PriceB
                End Get
                Set(Value As Decimal)
                    _PriceB = Value
                End Set
            End Property

            Public Property PriceC As Decimal
                Get
                    Return _PriceC
                End Get
                Set(Value As Decimal)
                    _PriceC = Value
                End Set
            End Property

            Public Property [Group] As String
                Get
                    Return _Group
                End Get
                Set(Value As String)
                    _Group = Value
                End Set
            End Property

        End Class

        Public Sub dtgPAddingColors_Delete(ByVal Sender As Object, ByVal E As CommandEventArgs)
            Dim colorID As Integer = Integer.Parse(E.CommandArgument.ToString)
            Dim color As SessionColor = (From sc In StoredColors Where sc.ColorID = colorID Select sc).FirstOrDefault

            ddlPAddColour.Items.Add(New ListItem(GetColorName(color.ColorID), color.ColorID.ToString))

            StoredColors.Remove(color)

            dtgPAddingColors.DataSource = StoredColors
            dtgPAddingColors.DataBind()
        End Sub

        Public Sub dtgPSelectedColors_Delete(ByVal source As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
            Dim colorID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim pID As Integer = Integer.Parse(hfProductID.Value)

            Dim product As Boolean = New ProductController().RemoveColor(pID, colorID)

            ddlPAddColour.Items.Add(New ListItem(GetColorName(colorID), colorID.ToString))

            Dim pColors As List(Of ProductColor) = New ProductColourController().GetColors(pID)

            For Each c As ProductColor In pColors
                RemoveColor(c.ColorID)
            Next

            dtgPSelectedColors.DataSource = pColors
            dtgPSelectedColors.DataBind()
        End Sub

        Public Sub dtgPAddingSize_Delete(ByVal source As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
            Dim sizeID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim size = (From ss In StoredSizes Where ss.SizeID = sizeID Select ss).FirstOrDefault

            ddlPAddSize.Items.Add(New ListItem(GetSizeName(size.SizeID), size.SizeID.ToString))

            StoredSizes.Remove(size)

            dtgPAddingSize.DataSource = StoredSizes
            dtgPAddingSize.DataBind()
        End Sub

        Public Sub dtgPSelectedSizes_Delete(ByVal source As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
            Dim sizeID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim pID As Integer = Integer.Parse(hfProductID.Value)

            Dim product As Boolean = New ProductController().RemoveSize(pID, sizeID)

            ddlPAddSize.Items.Add(New ListItem(GetSizeName(sizeID), sizeID.ToString))

            Dim pSizes As List(Of Size) = New ProductController().GetSizes(pID)

            For Each s As Size In pSizes
                RemoveSize(s.ID)
            Next

            dtgSelectedSizes.DataSource = pSizes
            dtgSelectedSizes.DataBind()
        End Sub

        Public Sub AddingRecomendedProductDataGridDelete(ByVal Sender As Object, ByVal Args As CommandEventArgs)
            Dim RecomendedProductId As Integer = Integer.Parse(Args.CommandArgument.ToString)
            Dim CurrentRecomendation As SessionRecomendation = (From sr In StoredRecomendations Where sr.RecomendedProductId = RecomendedProductId Select sr).FirstOrDefault

            RecomendedProductDropDown.Items.Add(New ListItem(GetProductName(CurrentRecomendation.RecomendedProductId), CurrentRecomendation.RecomendedProductId.ToString))

            StoredRecomendations.Remove(CurrentRecomendation)

            AddingRecomendedProductDataGrid.DataSource = StoredRecomendations
            AddingRecomendedProductDataGrid.DataBind()
        End Sub

        Public Sub SelectingRecomendedProductDataGridDelete(ByVal Sender As Object, ByVal Args As CommandEventArgs)
            Dim RecomendedProductId As Integer = Integer.Parse(Args.CommandArgument.ToString)
            Dim ProductId As Integer = Integer.Parse(hfProductID.Value)

            Dim CurrentStatus As Boolean = New ProductRecomendationController().Delete(ProductId, RecomendedProductId)

            RecomendedProductDropDown.Items.Add(New ListItem(GetProductName(RecomendedProductId), RecomendedProductId.ToString))

            Dim CurrentRecomendations As List(Of ProductRecomendation) = New ProductRecomendationController().GetElements(ProductId)

            For Each CurrentRecomendation As ProductRecomendation In CurrentRecomendations
                RemoveRecomendedProduct(CurrentRecomendation.RecomendedProductId)
            Next

            SelectingRecomendedProductDataGrid.DataSource = CurrentRecomendations
            SelectingRecomendedProductDataGrid.DataBind()
        End Sub

        'Public Sub AddingProductOptionsDataGridDelete(ByVal Sender As Object, ByVal Args As CommandEventArgs)
        '    Dim ProductOptionId As Integer = Integer.Parse(Args.CommandArgument.ToString)
        '    Dim CurrentProductOption As SessionProductOption = (From sr In StoredProductOptions Where sr.ProductOptionId = ProductOptionId Select sr).FirstOrDefault

        '    ProductOptionsDropDown.Items.Add(New ListItem(GetProductName(CurrentProductOption.ProductOptionId), CurrentProductOption.ProductOptionId.ToString))

        '    StoredProductOptions.Remove(CurrentProductOption)

        '    AddingProductOptionsDataGrid.DataSource = StoredRecomendations
        '    AddingProductOptionsDataGrid.DataBind()
        'End Sub

        'Public Sub SelectingProductOptionsDataGridDelete(ByVal Sender As Object, ByVal Args As CommandEventArgs)
        '    Dim ProductOptionId As Integer = Integer.Parse(Args.CommandArgument.ToString)
        '    Dim ProductId As Integer = Integer.Parse(hfProductID.Value)

        '    Dim CurrentStatus As Boolean = New ProductOptionController().Delete(ProductOptionId)

        '    ProductOptionsDropDown.Items.Add(New ListItem(GetProductName(ProductOptionId), ProductOptionId.ToString))

        '    Dim CurrentProductOptions As List(Of ProductOption) = New ProductOptionController().GetElements(ProductId)

        '    For Each CurrentProductOption As ProductRecomendation In CurrentProductOptions
        '        RemoveProductOption(CurrentProductOption.RecomendedProductId)
        '    Next

        '    SelectingProductOptionsDataGrid.DataSource = CurrentProductOptions
        '    SelectingProductOptionsDataGrid.DataBind()
        'End Sub

        Private Sub btnSavePBottom_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnSavePBottom.Command
            Dim pID As Integer = SaveProduct()

            LoadProduct(pID)
        End Sub

        Private Sub btnSavePTop_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnSavePTop.Command
            Dim pID As Integer = SaveProduct()

            LoadProduct(pID)
        End Sub

        Private Sub btnDeleteProduct_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnDeleteProduct.Command
            DeleteProduct()
        End Sub

        Private Sub btnAddArea_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddArea.Command
            'Dim _ProductID As Integer = Integer.Parse(hfProductID.Value)
            'Dim Details As String = txtAreaDescription.Text
            'Dim PrefixLow As String = txtAreaPrefixLow.Text
            'Dim PrefixHigh As String = txtAreaPrefixHigh.Text

            'If Not Details = String.Empty And Not PrefixLow = String.Empty And Not PrefixHigh = String.Empty Then
            '    SMCommerce.ProductAvailabilityController.Create(_ProductID, PrefixLow, PrefixHigh, Details)
            'End If

            'dgAvailableAreas.DataSource = SMCommerce.ProductAvailabilityController.GetProductAvailabilityByProductID(_ProductID)
            'dgAvailableAreas.DataBind()

            'txtAreaDescription.Text = ""
            'txtAreaPrefixLow.Text = ""
            'txtAreaPrefixHigh.Text = ""
        End Sub

        Public Sub dgAvailableAreas_Delete(ByVal source As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
            'Dim AreaID As Integer = Integer.Parse(e.CommandArgument)

            'Dim deleted As Boolean = SMCommerce.ProductAvailabilityController.Delete(AreaID)

            'If deleted Then
            '    Dim _ProductID As Integer = Integer.Parse(hfProductID.Value)

            '    dgAvailableAreas.DataSource = SMCommerce.ProductAvailabilityController.GetProductAvailabilityByProductID(_ProductID)
            '    dgAvailableAreas.DataBind()
            'End If
        End Sub

    End Class

End Namespace