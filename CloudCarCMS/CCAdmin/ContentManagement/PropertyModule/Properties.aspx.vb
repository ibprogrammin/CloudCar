Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement.PropertyModule

    Partial Public Class Properties
        Inherits Page

        Protected Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadProperties()
                LoadCountries()
            End If
        End Sub

        Private Sub LoadProperty(ByVal PropertyId As Integer)
            Try
                Dim SelectedProperty As [Property] = CCFramework.ContentManagement.PropertyModule.PropertyController.GetElement(PropertyId)

                hfPropertyId.Value = SelectedProperty.Id.ToString
                hfGalleryId.Value = SelectedProperty.ImageGalleryId.ToString
                hfAddressId.Value = SelectedProperty.AddressId.ToString

                txtTitle.Text = SelectedProperty.Title
                DetailsTextArea.InnerText = SelectedProperty.Details
                txtListingNumber.Text = SelectedProperty.ListingId
                TestimonialTextArea.InnerText = SelectedProperty.Testimonial

                txtPrice.Text = SelectedProperty.Price.ToString
                txtBedrooms.Text = SelectedProperty.Bedrooms.ToString
                txtBathrooms.Text = SelectedProperty.Bathrooms.ToString
                ckbActive.Checked = SelectedProperty.Active
                ckbVacancy.Checked = SelectedProperty.Vacancy
                txtListingLink.Text = SelectedProperty.ListingLink

                txtBrowserTitle.Text = SelectedProperty.PageTitle
                pvPermalink.ItemID = SelectedProperty.Id
                txtKeywords.Text = SelectedProperty.Keywords
                txtDescription.Text = SelectedProperty.Description
                hfPermalink.Value = SelectedProperty.Permalink
                txtPermalink.Text = SelectedProperty.Permalink

                txtAddress.text = SelectedProperty.Address.Address
                txtCity.Text = SelectedProperty.Address.City
                txtPostalCode.Text = SelectedProperty.Address.PCZIP

                LoadProvinces(SelectedProperty.Address.Province.CountryID)

                ddlCountry.SelectedValue = SelectedProperty.Address.Province.CountryID.ToString
                ddlProvince.SelectedValue = SelectedProperty.Address.ProvStateID.ToString

                LoadPropertyFeatures(SelectedProperty.Id)
                LoadPropertyImages(SelectedProperty.ImageGalleryId.Value)

                phAddtionalDetails.Visible = True

                litPropertyLink.Visible = True
                litPropertyLink.Text = "<label>Current Property</label><a href=""/Property/" & SelectedProperty.Permalink & ".html"" target=""_blank"" style=""background-color: #FFF8C2; position:relative; top: 10px;"">/Property/" & SelectedProperty.Permalink & ".html</a><br style=""clear: both;"" /><br /><br />"

                ImageGalleryDropDown.DataSource = CCFramework.ContentManagement.ImageGalleryController.GetElements()
                ImageGalleryDropDown.DataBind()

                ImageGalleryDropDown.SelectedValue = SelectedProperty.ImageGalleryId.Value.ToString
            Catch ex As Exception
                lblStatus.Text = ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub LoadPropertyFeatures(ByVal PropertyId As Integer)
            Dim PropertyFeatures As List(Of PropertyFeature) = CCFramework.ContentManagement.PropertyModule.PropertyFeatureController.GetPropertyFeaturesByProperty(PropertyId)

            ddlFeatures.Items.Clear()
            ddlFeatures.Items.Add(New ListItem("Please select...", ""))
            ddlFeatures.AppendDataBoundItems = True
            ddlFeatures.DataSource = CCFramework.ContentManagement.FeatureController.GetElements
            ddlFeatures.DataBind()

            For Each item As PropertyFeature In PropertyFeatures
                ddlFeatures.Items.Remove(ddlFeatures.Items.FindByValue(item.FeatureId.ToString))
            Next

            dgPropertyFeatures.DataSource = PropertyFeatures
            dgPropertyFeatures.DataBind()
        End Sub

        Private Sub btnSave_Click(ByVal Sender As Object, ByVal Args As EventArgs) Handles btnSave.Click
            Dim PropertyId As Integer

            If Page.IsValid Then
                Dim Title As String = txtTitle.Text
                Dim Details As String = DetailsTextArea.InnerText
                Dim Testimonial As String = TestimonialTextArea.InnerText
                Dim ListingNumber As String = txtListingNumber.Text

                Dim Price As Decimal
                If Not Decimal.TryParse(txtPrice.Text, Price) Then
                    Price = 0
                End If
                Dim Bedrooms As Integer
                If Not Integer.TryParse(txtBedrooms.Text, Bedrooms) Then
                    Bedrooms = 0
                End If
                Dim Bathrooms As Integer
                If Not Integer.TryParse(txtBathrooms.Text, Bathrooms) Then
                    Bathrooms = 0
                End If

                Dim ListingLink As String = txtListingLink.Text
                Dim Active As Boolean = ckbActive.Checked
                Dim Vacancy As Boolean = ckbVacancy.Checked

                Dim BrowserTitle As String = txtBrowserTitle.Text
                Dim Permalink As String = txtPermalink.Text
                Dim Keywords As String = txtKeywords.Text
                Dim Description As String = txtDescription.Text

                Dim Sitemap As New CCFramework.Generic.SiteMapProvider

                Dim AddressId As Integer
                Dim Address As String = txtAddress.Text
                Dim City As String = txtCity.Text
                Dim CountryId As Integer = Integer.Parse(ddlCountry.SelectedValue)
                Dim ProvinceId As Integer = Integer.Parse(ddlProvince.SelectedValue)
                Dim PostalCode As String = txtPostalCode.Text

                Dim ImageGalleryId As Integer

                If Integer.TryParse(hfPropertyId.Value, PropertyId) Then
                    ImageGalleryId = Integer.Parse(ImageGalleryDropDown.SelectedValue)
                    'SMContentManagement.ImageGalleryController.Update(ImageGalleryId, Title, Details)

                    AddressId = Integer.Parse(hfAddressId.Value)
                    Dim AddressUpdated As Boolean = New AddressController().Update(AddressId, Address, City, PostalCode, ProvinceId)

                    CCFramework.ContentManagement.PropertyModule.PropertyController.Update(PropertyId, Title, Details, ListingNumber, AddressId, ImageGalleryId, Testimonial, Price, Bedrooms, Bathrooms, BrowserTitle, Permalink, Keywords, Description, ListingLink, Active, Vacancy)

                    Sitemap.UpdatePage("/Property/" & hfPermalink.Value & ".html", "/Property/" & txtPermalink.Text & ".html", "", 0.7, MyBase.Context)

                    lblStatus.Text = "Your property has been saved!"
                    lblStatus.Visible = True

                    LoadProperty(PropertyId)
                Else
                    ImageGalleryId = CCFramework.ContentManagement.ImageGalleryController.Create(Title, Details)
                    AddressId = New AddressController().Create(Address, City, PostalCode, ProvinceId)

                    PropertyId = CCFramework.ContentManagement.PropertyModule.PropertyController.Create(Title, Details, ListingNumber, AddressId, ImageGalleryId, Testimonial, Price, Bedrooms, Bathrooms, BrowserTitle, Permalink, Keywords, Description, ListingLink, Active, Vacancy)

                    Sitemap.AddNewPage("/Property/" & txtPermalink.Text & ".html", "", 0.7, MyBase.Context)

                    lblStatus.Text = "Your property has been created!"
                    lblStatus.Visible = True

                    LoadProperty(PropertyId)

                    LoadProperties()
                End If
            End If
        End Sub

        Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
            ClearControls()

            phPropertyList.Visible = False
            phPropertyDetails.Visible = True
        End Sub

        Private Sub LoadProperties()
            'lbProperties.Items.Clear()
            'lbProperties.Items.Add(New ListItem("Select a property from this list", ""))
            'lbProperties.AppendDataBoundItems = True
            'lbProperties.DataSource = SMContentManagement.PropertyController.GetElements
            'lbProperties.DataBind()

            dgProperties.DataSource = CCFramework.ContentManagement.PropertyModule.PropertyController.GetElements
            dgProperties.DataBind()

            phPropertyList.Visible = True
        End Sub

        Private Sub ClearControls()
            lbProperties.SelectedValue = Nothing

            hfPropertyId.Value = Nothing
            hfPermalink.Value = Nothing
            pvPermalink.ItemID = Nothing

            txtTitle.Text = ""
            DetailsTextArea.InnerText = ""
            txtListingNumber.Text = ""
            TestimonialTextArea.InnerText = ""

            txtBrowserTitle.Text = ""
            txtKeywords.Text = ""
            txtDescription.Text = ""
            txtPermalink.Text = ""
            txtListingLink.Text = ""

            pvPermalink.ItemID = Nothing

            txtPrice.Text = ""
            txtBedrooms.Text = ""
            txtBathrooms.Text = ""
            ckbActive.Checked = False
            ckbVacancy.Checked = False

            txtAddress.Text = ""
            txtCity.Text = ""
            txtPostalCode.Text = ""
            ddlCountry.SelectedValue = Nothing
            ddlProvince.SelectedValue = Nothing

            litPropertyLink.Visible = False
            litPropertyLink.Text = ""

            hfGalleryId.Value = Nothing
            hfAddressId.Value = Nothing

            ImageGalleryDropDown.SelectedValue = Nothing

            ClearPropertyImageForm()

            dgPropertyImages.DataSource = Nothing
            dgPropertyImages.DataBind()

            dgPropertyFeatures.DataSource = Nothing
            dgPropertyFeatures.DataBind()

            phAddtionalDetails.Visible = False
        End Sub

        Public Sub dgProperties_Select(ByVal source As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
            Dim PropertyId As Integer = Integer.Parse(e.CommandArgument.ToString)

            LoadProperty(PropertyId)

            phPropertyList.Visible = False
            phPropertyDetails.Visible = True
        End Sub

        Private Sub lbProperties_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProperties.SelectedIndexChanged
            If Not lbProperties.SelectedValue = Nothing Then
                Dim PropertyId As Integer = Integer.Parse(lbProperties.SelectedValue)

                LoadProperty(PropertyId)
            End If
        End Sub

        Private Sub btnDelete_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnDelete.Command
            If Not hfPropertyId.Value = Nothing Then
                Dim PropertyId As Integer = Integer.Parse(hfPropertyId.Value)

                If CCFramework.ContentManagement.PropertyModule.PropertyController.Delete(PropertyId) = True Then
                    lblStatus.Text = "The selected property has been deleted permanently!"
                    lblStatus.Visible = True

                    LoadProperties()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current property."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have a property selected."
                lblStatus.Visible = True
            End If
        End Sub

        Protected Function GetFeatureName(ByVal FeatureId As Integer) As String
            Dim SelectedFeature As Feature = CCFramework.ContentManagement.FeatureController.GetElement(FeatureId)

            Return SelectedFeature.Name
        End Function

        Private Sub btnAddFeature_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddFeature.Command
            Dim FeatureId As Integer

            If Integer.TryParse(ddlFeatures.SelectedValue, FeatureId) Then
                If Not hfPropertyId.Value Is Nothing And Not hfPropertyId.Value = String.Empty Then
                    Dim PropertyId As Integer = Integer.Parse(hfPropertyId.Value)

                    Dim PropertyFeatureId As Integer = CCFramework.ContentManagement.PropertyModule.PropertyFeatureController.Create(PropertyId, FeatureId)
                    LoadPropertyFeatures(PropertyId)
                End If
            Else
                lblFeatureMessage.Text = "There was an error trying to add this feature."
                lblFeatureMessage.Visible = True
            End If
        End Sub

        Public Sub dgPropertyFeatures_Remove(ByVal source As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
            Dim Id As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim SelectedPropertyFeature As PropertyFeature = CCFramework.ContentManagement.PropertyModule.PropertyFeatureController.GetElement(Id)

            CCFramework.ContentManagement.PropertyModule.PropertyFeatureController.Delete(Id)

            Dim PropertyId As Integer = Integer.Parse(hfPropertyId.Value)
            LoadPropertyFeatures(PropertyId)
        End Sub

        Private Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
            Dim CountryId As Integer = Integer.Parse(ddlCountry.SelectedValue)

            LoadProvinces(CountryId)
        End Sub

        Private Sub LoadCountries()
            ddlCountry.Items.Clear()
            ddlCountry.Items.Add(New ListItem("Please Select...", ""))
            ddlCountry.AppendDataBoundItems = True
            ddlCountry.DataSource = New CountryController().GetElements
            ddlCountry.DataBind()
        End Sub

        Private Sub LoadProvinces(ByVal CountryId As Integer)
            ddlProvince.Items.Clear()
            ddlProvince.Items.Add(New ListItem("Please Select...", ""))
            ddlProvince.AppendDataBoundItems = True
            ddlProvince.DataSource = New ProvinceController().GetCountryProvince(CountryId)
            ddlProvince.DataBind()
        End Sub

        Private Sub LoadPropertyImages(ByVal GalleryId As Integer)
            Dim PropertyImages As List(Of ImageGalleryItem) = CCFramework.ContentManagement.ImageGalleryItemController.GetGalleryItemsByGallery(GalleryId)

            If PropertyImages.Count > 0 Then
                dgPropertyImages.DataSource = PropertyImages
                dgPropertyImages.DataBind()
            Else
                dgPropertyImages.DataSource = Nothing
                dgPropertyImages.DataBind()
            End If
        End Sub

        Private Sub btnAddPropertyImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPropertyImage.Click
            Dim GalleryId As Integer = Integer.Parse(hfGalleryId.Value)

            Dim Title As String = txtImageTitle.Text
            Dim Description As String = txtImageDescription.Text
            Dim Order As Integer = Integer.Parse(txtImageOrder.Text)

            Dim ImageID As Integer
            If fuImage.HasFile Then
                ImageID = CCFramework.Core.ImageFunctions.UploadImage(fuImage)

                CCFramework.ContentManagement.ImageGalleryItemController.Create(Title, Description, GalleryId, ImageID, Order)

                lblImageMessage.Text = "Your property image has been saved!"
                lblImageMessage.Visible = True

                ClearPropertyImageForm()

                LoadPropertyImages(GalleryId)
            Else
                ImageID = 0

                lblImageMessage.Text = "You didn't select an image, no changes have been saved!"
                lblImageMessage.Visible = True
            End If
        End Sub

        Public Sub dgPropertyImages_Remove(ByVal source As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
            Dim Id As Integer = Integer.Parse(e.CommandArgument.ToString)

            If CCFramework.ContentManagement.ImageGalleryItemController.Delete(Id) Then
                lblImageMessage.Text = "The selected image was removed from this property!"
                lblImageMessage.Visible = True
            Else
                lblImageMessage.Text = "There was an error removing the selected image from this property!"
                lblImageMessage.Visible = True
            End If

            Dim GalleryId As Integer = Integer.Parse(hfGalleryId.Value)
            LoadPropertyImages(GalleryId)
        End Sub

        Private Sub ClearPropertyImageForm()
            txtImageTitle.Text = ""
            txtImageDescription.Text = ""
            txtImageOrder.Text = ""
        End Sub

        Private Sub lbShowList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbShowList.Click
            ClearControls()

            phPropertyDetails.Visible = False
            phPropertyList.Visible = True
        End Sub

        Private Sub PropertiesDataGridPageIndexChanged(ByVal Source As Object, ByVal E As DataGridPageChangedEventArgs) Handles dgProperties.PageIndexChanged
            dgProperties.CurrentPageIndex = E.NewPageIndex

            LoadProperties()
        End Sub

        Private Sub ImageGalleryDropDownSelectedIndexChanged(ByVal Sender As Object, ByVal E As EventArgs) Handles ImageGalleryDropDown.SelectedIndexChanged
            Dim PropertyId As Integer = Integer.Parse(hfPropertyId.Value)
            Dim ImageGalleryId As Integer = Integer.Parse(ImageGalleryDropDown.SelectedValue)

            CCFramework.ContentManagement.PropertyModule.PropertyController.ChangePropertyImageGallery(PropertyId, ImageGalleryId)

            LoadPropertyImages(ImageGalleryId)
        End Sub

        Protected Sub SetPropertyActive(ByVal Sender As Object, ByVal E As EventArgs)
            Dim CurrentCheckBox As CheckBox = CType(Sender, CheckBox)

            Dim PropertyId As Integer = Integer.Parse(CurrentCheckBox.Attributes("PropertyId"))
            Dim ActiveStatus As Boolean = CurrentCheckBox.Checked

            CCFramework.ContentManagement.PropertyModule.PropertyController.SetPropertyActive(PropertyId, ActiveStatus)
        End Sub

        Protected Sub SetPropertyVacant(ByVal Sender As Object, ByVal E As EventArgs)
            Dim CurrentCheckBox As CheckBox = CType(Sender, CheckBox)

            Dim PropertyId As Integer = Integer.Parse(CurrentCheckBox.Attributes("PropertyId"))
            Dim VacantStatus As Boolean = CurrentCheckBox.Checked

            CCFramework.ContentManagement.PropertyModule.PropertyController.SetPropertyVacant(PropertyId, VacantStatus)
        End Sub

    End Class
End Namespace