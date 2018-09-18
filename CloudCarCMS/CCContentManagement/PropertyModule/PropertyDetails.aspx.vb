Imports CloudCar.CCFramework.Model

Namespace CCContentManagement.PropertyModule

    Partial Public Class PropertyDetails1
        Inherits RoutablePage

        Private mPermalink As String
        Private mProperty As [Property]

        Public Property Permalink() As String
            Get
                Return mPermalink
            End Get
            Set(ByVal value As String)
                mPermalink = value
            End Set
        End Property

        Public Property CurrentProperty() As [Property]
            Get
                If Not ViewState("Property") Is Nothing Then
                    Return CType(ViewState("Property"), [Property])
                Else
                    Return Nothing
                End If
                'Return mProperty
            End Get
            Set(ByVal value As [Property])
                If Not ViewState("Property") Is Nothing Then
                    ViewState("Property") = value
                Else
                    ViewState.Add("Property", value)
                End If
                'mProperty = value
            End Set
        End Property

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                EmailListingLabel.Visible = False
                BookViewingLabel.Visible = False
            End If
        End Sub

        Protected Overrides Sub OnInit(ByVal E As EventArgs)
            MyBase.OnInit(E)

            If mPermalink Is Nothing Then
                Permalink = (From v In RequestContext.RouteData.Values Where v.Key = "permalink" Select New With {.id = v.Value}).SingleOrDefault.id.ToString
                CurrentProperty = CCFramework.ContentManagement.PropertyModule.PropertyController.GetPropertyFromLink(mPermalink)
            End If

            LoadProperty()
        End Sub

        Private _GoogleMapAddress As String
        Public Property GoogleMapAddress() As String
            Get
                Return _GoogleMapAddress
            End Get
            Set(ByVal value As String)
                _GoogleMapAddress = value
            End Set
        End Property

        Private Sub LoadProperty()
            If Not CurrentProperty Is Nothing Then
                Dim PropertyId As Integer = CurrentProperty.Id

                Dim Title As String = CurrentProperty.Title
                Dim Details As String = CurrentProperty.Details
                Dim Testiomonial As String = CurrentProperty.Testimonial
                'Dim Address As String = CurrentProperty.Address.Address & ", " & CurrentProperty.Address.City & ", " & CurrentProperty.Address.Province.Name & ", " & CurrentProperty.Address.Province.Country.Name & ", " & CurrentProperty.Address.PCZIP
                Dim Address As String = CurrentProperty.Address.City & ", " & CurrentProperty.Address.Province.Name

                _GoogleMapAddress = CurrentProperty.Address.Address & ", " & CurrentProperty.Address.City & ", " & CurrentProperty.Address.Province.Code & ", " & CurrentProperty.Address.PCZIP & ", " & CurrentProperty.Address.Province.Country.Name

                Dim Keywords As String = CurrentProperty.Keywords
                Dim Description As String = CurrentProperty.Description
                Dim PageTitle As String = CurrentProperty.PageTitle

                Dim Price As Decimal = CurrentProperty.Price
                Dim Bedrooms As Integer = CurrentProperty.Bedrooms
                Dim Bathrooms As Integer = CurrentProperty.Bathrooms

                Dim ListingId As String = CurrentProperty.ListingId

                Dim Features As List(Of Feature) = CCFramework.ContentManagement.FeatureController.GetFeaturesByProperty(PropertyId)

                If Features.Count > 0 Then
                    FeaturesPanel.Visible = True

                    rptFeatures.DataSource = Features
                    rptFeatures.DataBind()
                End If

                igcGallery.GalleryId = CurrentProperty.ImageGalleryId.Value
                igcGallery.Category = CurrentProperty.Title

                PageKeywordsMeta.Attributes("content") = Keywords
                PageDescriptionMeta.Attributes("content") = Description

                MyBase.Title = PageTitle '& SMCore.Settings.SiteTitle

                litTitle.Text = Title
                litDetails.Text = Details
                litTestimonial.Text = Testiomonial
                litAddress.Text = Address

                litPrice.Text = "$" & Price.ToString("0,0")
                litBedrooms.Text = Bedrooms.ToString
                litBathrooms.Text = Bathrooms.ToString

                litListingId.Text = ListingId

                WalkscoreLink.Text = "View Walkscore for " & CurrentProperty.Title
                WalkscoreLink.NavigateUrl = String.Format("http://www.walkscore.com/score/{0}-{1}-{2}-{3}", CurrentProperty.Address.Address, CurrentProperty.Address.City, CurrentProperty.Address.Province.Code, CurrentProperty.Address.Province.Country.Name).Replace(" ", "-").Replace(",", "").Replace("&", "")
            Else
                litTitle.Text = "Invalid Property!"
                litDetails.Text = "The property you are viewing is invalid please try again or try another search."
            End If

        End Sub

        Private Sub EmailListingButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles EmailListingButton.Click
            BookViewingLabel.Visible = False
            EmailListingLabel.Visible = True

            PropertyPopUp.PopupControlID = "EmailListingLabel"
            PropertyPopUp.CancelControlID = "CancelListingButton"
            PropertyPopUp.Show()
        End Sub

        Private Sub SubmitListingButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SubmitListingButton.Click
            'If Not EmailAddressTextBox.Text Is Nothing Then
            'If Not EmailAddressTextBox.Text = String.Empty Then
            Dim Subject As String

            Dim Message As New StringBuilder

            If Not CurrentProperty Is Nothing Then
                Subject = String.Format("Check out this property: {0}", CurrentProperty.Title)

                Dim PropertyPictureId As Integer = CCFramework.ContentManagement.ImageGalleryController.GetFirstGalleryImage(CurrentProperty.ImageGalleryId.Value)

                Message.Append(String.Format("<img src=""http://{0}/images/db/{1}/140/{2}"" /><br>" & vbNewLine, CCFramework.Core.Settings.HostName, PropertyPictureId, CCFramework.Core.PictureController.GetPicture(PropertyPictureId).PictureFileName))
                Message.Append(String.Format("<b>{0}</b><br>" & vbNewLine, CurrentProperty.Title))
                Message.Append(String.Format("{0}, {1}, {2}<br>" & vbNewLine, CurrentProperty.Address.Address, CurrentProperty.Address.City, CurrentProperty.Address.Province.Name))
                Message.Append(String.Format("{0} - Bedrooms<br>" & vbNewLine, CurrentProperty.Bedrooms))
                Message.Append(String.Format("{0} - Bathrooms<br><br>" & vbNewLine & vbNewLine, CurrentProperty.Bathrooms))
                Message.Append(String.Format("{0} <br><br>" & vbNewLine & vbNewLine, CurrentProperty.Description))
                Message.Append(String.Format("<b>Features:</b><br><ul> " & vbNewLine))

                For Each FeatureItem As Feature In CCFramework.ContentManagement.FeatureController.GetFeaturesByProperty(CurrentProperty.Id)
                    Message.Append(String.Format("<li>{0}</li>" & vbNewLine, FeatureItem.Name))
                Next

                Message.Append(String.Format("</ul><br><br>" & vbNewLine))
                Message.Append(String.Format("<a href=""http://{0}/Property/{1}.html"">Click to go to website</a><br><br>" & vbNewLine, CCFramework.Core.Settings.HostName, CurrentProperty.Permalink))

                Dim EmailAddress As New Net.Mail.MailAddress(EmailAddressTextBox.Text)

                CCFramework.Core.SendEmails.Send(EmailAddress, Subject, Message.ToString)

                'StatusMessage.Text = "The listing has been sent!"
                'StatusMessage.Visible = True

                EmailListingLabel.Visible = False
                PropertyPopUp.Hide()

                Dim FadeAwayScript As New StringBuilder

                FadeAwayScript.Append("<script type=""text/javascript"">")
                FadeAwayScript.Append("PromptHide();")
                FadeAwayScript.Append("function PromptHide() {")
                FadeAwayScript.Append("$(""#PromptStatusMessage"").fadeIn('slow').delay(2000).fadeOut('slow');")
                FadeAwayScript.Append("}")
                FadeAwayScript.Append("</script>")

                ScriptManager.RegisterStartupScript(InnerUpdatePanel, InnerUpdatePanel.GetType(), "CallFadeAwayScript1", FadeAwayScript.ToString, False)
                'Page.ClientScript.RegisterStartupScript(MyBase.GetType(), "CallFadeAwayScript", FadeAwayScript.ToString)
            End If

            'End If
            'End If
        End Sub

        Private Sub BookViewingButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles BookViewingButton.Click
            BookViewingLabel.Visible = True
            EmailListingLabel.Visible = False

            PropertyPopUp.PopupControlID = "BookViewingLabel"
            PropertyPopUp.CancelControlID = "CancelViewingButton"
            PropertyPopUp.Show()
        End Sub

        Private Const VerificationAnswer As Integer = 7

        Private Sub SubmitViewingButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SubmitViewingButton.Click
            If Integer.Parse(VerificationTextBox.Text) = VerificationAnswer Then

                Dim Subject As String

                Dim Message As New StringBuilder

                If Not CurrentProperty Is Nothing Then
                    Subject = String.Format("There has been a viewing request for: {0}", CurrentProperty.Title)

                    Message.Append(String.Format("<b>Property:</b> {0} ({1}) <br>" & vbNewLine & vbNewLine, CurrentProperty.Title, CurrentProperty.ListingId))
                    Message.Append(String.Format("<b>Name:</b> {0} <br>" & vbNewLine, NameTextBox.Text))
                    Message.Append(String.Format("<b>Phone Number:</b> {0} <br>" & vbNewLine, PhoneTextBox.Text))
                    Message.Append(String.Format("<b>Email:</b> {0} <br>" & vbNewLine, EmailTextBox.Text))
                    Message.Append(String.Format("<b>Time:</b> {0} <br>" & vbNewLine & vbNewLine, TimeTextBox.Text))
                    Message.Append(String.Format("<b>Move-in-Date:</b> {0} <br>" & vbNewLine & vbNewLine, MoveInTextBox.Text))

                    Dim EmailAddress As New Net.Mail.MailAddress(CCFramework.Core.Settings.AdminEmail)

                    CCFramework.Core.SendEmails.Send(EmailAddress, Subject, Message.ToString)

                    'StatusMessage.Text = "We have received your request!"
                    'StatusMessage.Visible = True

                    BookViewingLabel.Visible = False
                    PropertyPopUp.Hide()

                    Dim FadeAwayScript As New StringBuilder

                    FadeAwayScript.Append("<script type=""text/javascript"">")
                    FadeAwayScript.Append("PromptHide();")
                    FadeAwayScript.Append("function PromptHide() {")
                    FadeAwayScript.Append("$(""#PromptStatusMessage"").fadeIn('slow').delay(2000).fadeOut('slow');")
                    FadeAwayScript.Append("}")
                    FadeAwayScript.Append("</script>")

                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CallFadeAwayScript2", FadeAwayScript.ToString, False)
                    'ClientScript.RegisterStartupScript( MyBase.GetType(), "CallFadeAwayScript", FadeAwayScript.ToString)
                End If
            Else
                StatusMessage.Text = "You answered the verification question incorrectly."
                StatusMessage.Visible = True

                BookViewingLabel.Visible = False
                PropertyPopUp.Hide()
            End If
        End Sub

    End Class
End NameSpace