Imports System.Web.Routing
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce.ShoppingCart
Imports CloudCar.handlers.Routes

Public Class CloudCarGlobal
    Inherits HttpApplication

    Private ReadOnly _SslPages As String() = New String() {"/Store", "/Login.html", "/Register.html", "/Registration.html", "/Profile.html", "/Account.html", "/Change-Password.html", "/Reset-Password.html"} '"/Shop"
    Private ReadOnly _IgnoreExceptions As String() = New String() {"System.ArgumentException", "System.Web.HttpException"}

    Sub Application_Start(ByVal Sender As Object, ByVal Args As EventArgs)
        Application.Clear()
        Application.Add("userCount", 0)

        Settings.LoadSettings()

        ShoppingCartController.DeleteUnregisteredShoppingCarts()

        RegisterRoutes(RouteTable.Routes)
    End Sub

    Sub Session_Start(ByVal Sender As Object, ByVal Args As EventArgs)
        SessionId = Guid.NewGuid

        If Not RemoteAddressTable.Contains(GetUniqueBrowserSessionId()) Then
            RemoteAddressTable.Add(GetUniqueBrowserSessionId())
            Application("userCount") = CInt(Application("userCount")) + 1
        End If
    End Sub

    Sub Application_BeginRequest(ByVal Sender As Object, ByVal Args As EventArgs)
        Dim CurrentRequestPort As Integer = Request.Url.Port

        If Settings.EnableSSL Then
            If CurrentRequestPort = 80 Then
                If Settings.FullSSL Then
                    Response.Redirect(String.Format("https://{0}{1}{2}", Settings.SSLUrl, Request.FilePath, Request.Url.Query))
                Else
                    Dim CurrentFilePath As String = Request.FilePath.ToLower

                    For Each CurrentItem As String In _SslPages
                        If CurrentFilePath.StartsWith(CurrentItem.ToLower) Then
                            Response.Redirect(String.Format("https://{0}{1}{2}", Settings.SSLUrl, Request.FilePath, Request.Url.Query))
                        End If
                    Next
                End If
            End If
        End If
    End Sub

    Sub Application_AuthenticateRequest(ByVal Sender As Object, ByVal E As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal Sender As Object, ByVal E As EventArgs)
        'On Error Resume Next

        If Settings.TrackErrors Then
            Dim ErrorContainer As Exception = Server.GetLastError
            Dim ExceptionType As String = ErrorContainer.GetType.ToString
            Dim SaveException As Boolean = If((From ie As String In _IgnoreExceptions Where ie = ExceptionType Select ie).Count > 0, False, True)

            If SaveException Then
                Dim ErrorString As New StringBuilder

                ErrorString.AppendFormat("<b>Project:</b> {0}<br>", Settings.Project)
                ErrorString.AppendFormat("<b>Version:</b> {0}<br>", Settings.Version)

                If Not Membership.GetUser Is Nothing Then
                    ErrorString.AppendFormat("<b>Username:</b> {0}<br>", Membership.GetUser.UserName)
                End If

                If Request.QueryString.Count > 0 Then
                    ErrorString.Append("<br><b>Query Strings:</b> <br>")
                    For Each key In Request.QueryString.Keys
                        ErrorString.AppendFormat("{0}:{1}<br>", key, Request.Item(key.ToString))
                    Next
                End If

                If Not Request.UrlReferrer Is Nothing Then
                    ErrorString.AppendFormat("<b>Referrer:</b> {0}<br><br>", Request.UrlReferrer.ToString)
                End If

                'Server.GetLastError.InnerException.ToString.Replace(vbNewLine, "<br>")

                ErrorString.Append("<br><b>Error:</b><br>")
                ErrorString.Append(ErrorContainer.ToString.Replace(vbNewLine, "<br>"))

                SendEmails.Send(New Net.Mail.MailAddress(Settings.ErrorEmail), String.Format("{0} Site Error - {1}", Settings.CompanyName, ExceptionType), ErrorString.ToString)
                ErrorController.LogError(ErrorContainer, Context, Settings.Project)
            End If
        End If
    End Sub

    Sub Session_End(ByVal Sender As Object, ByVal E As EventArgs)
        'If Membership.GetUser Is Nothing Then
        '    If Not Session("SessionId") Is Nothing Then
        '        ShoppingCartController.ClearShoppingCartUserCheck(SessionId.ToString)
        '    End If
        'End If

        If Not Session Is Nothing Then
            Session.RemoveAll()
        End If

        If Not RemoteAddressTable.Contains(GetUniqueBrowserSessionId()) Then
            RemoteAddressTable.Remove(GetUniqueBrowserSessionId())
            Application("userCount") = CInt(Application("userCount")) - 1
        End If
    End Sub

    Sub Application_End(ByVal Sender As Object, ByVal E As EventArgs)
        ' Fires when the application ends
        Application.Remove("userCount")

        ShoppingCartController.DeleteUnregisteredShoppingCarts()
    End Sub

    Public Property RemoteAddressTable() As List(Of String)
        Get
            If Application("RemoteAddressTable") Is Nothing Then
                Application.Add("RemoteAddressTable", New List(Of String))

                Return CType(Application("RemoteAddressTable"), List(Of String))
            Else
                Return CType(Application("RemoteAddressTable"), List(Of String))
            End If
        End Get
        Set(Value As List(Of String))
            If Application("RemoteAddressTable") Is Nothing Then
                Application.Add("RemoteAddressTable", Value)
            Else
                Application("RemoteAddressTable") = Value
            End If
        End Set
    End Property

    Public Property SessionId() As Guid
        Get
            If Session("SessionId") Is Nothing Then
                Session.Add("SessionId", Guid.NewGuid)

                Return CType(Session("SessionId"), Guid)
            Else
                Return CType(Session("SessionId"), Guid)
            End If
        End Get
        Set(ByVal Value As Guid)
            If Session("SessionId") Is Nothing Then
                Session.Add("SessionId", Value)
            Else
                Session("SessionId") = Value
            End If
        End Set
    End Property

    Protected Function GetUserRemoteIpAddress() As String
        Dim CurrentIpAddress As String = String.Empty

        If Not HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR") Is Nothing Then
            CurrentIpAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString()
        ElseIf Not HttpContext.Current.Request.UserHostAddress.Length = 0 Then
            CurrentIpAddress = HttpContext.Current.Request.UserHostAddress
        End If

        Return CurrentIpAddress
    End Function

    Protected Function GetUserLocalIpAddress() As String
        Dim CurrentIpAddress As String = String.Empty

        If Not HttpContext.Current.Request.ServerVariables("LOCAL_ADDR") Is Nothing Then
            CurrentIpAddress = HttpContext.Current.Request.ServerVariables("LOCAL_ADDR").ToString()
        End If

        Return CurrentIpAddress
    End Function

    Protected Function GetUniqueBrowserSessionId() As String
        Dim UniqueBrowserSessionId As String = String.Empty

        UniqueBrowserSessionId &= GetUserRemoteIpAddress() & "-"
        UniqueBrowserSessionId &= HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToString & "-"

        If Not HttpContext.Current.Request.ServerVariables("HTTP_COOKIE") Is Nothing Then
            UniqueBrowserSessionId &= HttpContext.Current.Request.ServerVariables("HTTP_COOKIE").ToString
        End If

        Return UniqueBrowserSessionId
    End Function

    Private Sub RegisterRoutes(ByVal Routes As RouteCollection)
        ' Add an unnamed handler for simple requests
        Routes.Add(New Route("Home", New WebFormRouteHandler("~/Default.aspx")))
        Routes.Add("RouteHomePageA", New Route("Home", New WebFormRouteHandler("~/Default.aspx")))
        Routes.Add("RouteHomePageB", New Route("Home.html", New WebFormRouteHandler("~/Default.aspx")))
        Routes.Add("RouteHomePageC", New Route("Home/Index.html", New WebFormRouteHandler("~/Default.aspx")))

        Routes.Add("RouteLinks", New Route("Home/Links.html", New WebFormRouteHandler("~/CCContentManagement/Links.aspx")))
        Routes.Add("RouteCommunity", New Route("Home/community.html", New WebFormRouteHandler("~/CCContentManagement/Links.aspx")))
        Routes.Add("RouteMenu", New Route("Home/menu.html", New WebFormRouteHandler("~/CCContentManagement/ClientForms/Menu.aspx")))
        Routes.Add("RouteTestimonials", New Route(String.Format("Home/{0}.html", Settings.TestimonialPage), New WebFormRouteHandler("~/CCContentManagement/Testimonials.aspx")))

        Routes.Add("RouteVideos", New Route(String.Format("Home/{0}.html", Settings.VideosPage), New WebFormRouteHandler("~/CCContentManagement/VideosModule/Videos.aspx")))
        Routes.Add("RouteGallery", New Route("Home/Gallery.html", New WebFormRouteHandler("~/CCContentManagement/Gallery.aspx")))
        Routes.Add("RouteShopDisabled", New Route(String.Format("Home/{0}.html", Settings.StoreDisabledPage), New WebFormRouteHandler("~/CCContentManagement/ContentPage.aspx")))

        ' Content Page routing handler
        Routes.Add("RouteContentPage", New Route("Home/{permalink}.html", New WebFormRouteHandler("~/CCContentManagement/ContentPage.aspx")))

        Routes.Add("RouteEvents", New Route("Events/Index.html", New WebFormRouteHandler("~/CCContentManagement/EventModule/Events.aspx")))
        Routes.Add("RouteEventDetails", New Route("Events/{permalink}.html", New WebFormRouteHandler("~/CCContentManagement/EventModule/EventDetails.aspx")))

        Routes.Add("RouteNews", New Route("News/Index.html", New WebFormRouteHandler("~/CCContentManagement/NewsModule/News.aspx")))
        Routes.Add("RouteNewsDetails", New Route("News/{permalink}.html", New WebFormRouteHandler("~/CCContentManagement/NewsModule/NewsDetails.aspx")))

        Routes.Add("RouteForms", New Route("Forms/{permalink}.html", New WebFormRouteHandler("~/CCContentManagement/FormModule/FormDisplay.aspx")))

        ' Database Image Routing handler
        'TODO determine if this route is causing issues with static image loading taking a long time.
        Routes.Add("RouteDBImages", New Route("images/db/{id}/{size}/{filename}", New WebFormRouteHandler("~/handlers/Images/GetImage.aspx")))

        ' Blog Page routing handler
        Routes.Add("RouteBlog", New Route("Blog/Index.html", New WebFormRouteHandler("~/CCBlogging/BlogEntries.aspx")))
        Routes.Add("RouteBlogLatestEntry", New Route("Blog/LatestEntry.html", New RouteValueDictionary(New With {.latestEntry = True}), New WebFormRouteHandler("~/CCBlogging/Blog.aspx")))
        Routes.Add("RouteBlogEntry", New Route("Blog/{permalink}.html", New WebFormRouteHandler("~/CCBlogging/Blog.aspx")))

        ' Search Page routing handler
        Routes.Add("RouteSearch", New Route("Search/Index.html", New WebFormRouteHandler("~/Search.aspx")))
        Routes.Add("RouteSearchTerm", New Route("Search/Results.html", New WebFormRouteHandler("~/Search.aspx")))

        ' Account routing handlers 
        'TODO route all the account pages to an account directory to make security easier.
        Routes.Add("RouteLogin", New Route("Login.html", New WebFormRouteHandler("~/CCAuthentication/Login.aspx")))
        Routes.Add("RouteRegister", New Route("Register.html", New WebFormRouteHandler("~/CCAuthentication/Register.aspx")))
        Routes.Add("RouteRegistration", New Route("Registration.html", New WebFormRouteHandler("~/CCAuthentication/Register.aspx")))
        Routes.Add("RouteProfile", New Route("Profile.html", New WebFormRouteHandler("~/CCAuthentication/UserProfile.aspx")))
        Routes.Add("RouteAccount", New Route("Account.html", New WebFormRouteHandler("~/CCAuthentication/UserProfile.aspx")))
        Routes.Add("RouteChangePassword", New Route("Change-Password.html", New WebFormRouteHandler("~/CCAuthentication/ChangePassword.aspx")))
        Routes.Add("RouteResetPassword", New Route("Reset-Password.html", New WebFormRouteHandler("~/CCAuthentication/ResetPassword.aspx")))

        'Store Pages routing handlers
        Routes.Add("RouteShop", New Route("Shop/Index.html", New WebFormRouteHandler("~/CCCommerce/Categories.aspx")))
        Routes.Add("RouteTopSellers", New Route(String.Format("Shop/{0}.html", Settings.TopSellersPage), New WebFormRouteHandler("~/CCCommerce/TopSellers.aspx")))
        Routes.Add("RouteClearance", New Route(String.Format("Shop/{0}.html", Settings.ClearancePage), New WebFormRouteHandler("~/CCCommerce/Clearance.aspx")))
        Routes.Add("RouteShoppingCartA", New Route("Shop/ShoppingCart.html", New WebFormRouteHandler("~/CCCommerce/ShoppingCart.aspx")))
        Routes.Add("RouteShoppingCartB", New Route("ShoppingCart.html", New WebFormRouteHandler("~/CCCommerce/ShoppingCart.aspx")))
        Routes.Add("RouteCheckout", New Route("Shop/Checkout.html", New WebFormRouteHandler("~/CCCommerce/Checkout.aspx")))
        Routes.Add("RouteThankYouA", New Route("Shop/thank-you.html", New WebFormRouteHandler("~/CCCommerce/ThankYou.aspx")))
        Routes.Add("RouteThankYouB", New Route("thank-you.html", New WebFormRouteHandler("~/CCCommerce/ThankYou.aspx")))

        Routes.Add("RouteColors", New Route("Shop/Color/{permalink}.html", New WebFormRouteHandler("~/CCCommerce/Colours.aspx")))
        Routes.Add("RouteBrands", New Route("Shop/Brand/{permalink}.html", New WebFormRouteHandler("~/CCCommerce/Brands.aspx")))

        Routes.Add("RouteShopProduct", New Route("Shop/{category}/{product}.html", New WebFormRouteHandler("~/CCCommerce/Details.aspx")))
        Routes.Add("RouteShopCategory", New Route("Shop/{permalink}.html", New WebFormRouteHandler("~/CCCommerce/Products.aspx")))

        Routes.Add("RouteProduct", New Route("Product/{category}/{product}.html", New WebFormRouteHandler("~/CCCommerce/Details.aspx")))
        Routes.Add("RouteCategory", New Route("Product/{permalink}.html", New WebFormRouteHandler("~/CCCommerce/Products.aspx")))

        Routes.Add("RouteDownloads", New Route("Download/Product/{guid}/{filename}", New WebFormRouteHandler("~/handlers/Files/FileDownloadHandler.ashx")))
        Routes.Add("RouteReports", New Route("Report/{type}.csv", New WebFormRouteHandler("~/handlers/Reports/CSVReportHandler.ashx")))

        'Property Module
        Routes.Add("RoutePropertySearch", New Route("Property/Search.html", New WebFormRouteHandler("~/CCContentManagement/PropertyModule/PropertySearch.aspx")))
        Routes.Add("RouteProperty", New Route("Property/{permalink}.html", New WebFormRouteHandler("~/CCContentManagement/PropertyModule/Properties.aspx")))

        'Calendar Module
        Routes.Add("RouteCalendar", New Route("Calendar/Index.html", New WebFormRouteHandler("~/CCContentManagement/CalendarModule/Calendar.aspx")))
        Routes.Add("RoutePrograms", New Route("Programs.html", New WebFormRouteHandler("~/CCContentManagement/CalendarModule/ProgramList.aspx")))
        Routes.Add("RouteProgram", New Route("Programs/{permalink}.html", New WebFormRouteHandler("~/CCContentManagement/CalendarModule/Program.aspx")))
        Routes.Add("RouteInstructor", New Route("Instructor/{permalink}.html", New WebFormRouteHandler("~/CCContentManagement/CalendarModule/Instructor.aspx")))


        Routes.Add("RouteCustomClientForm1", New Route("Forms/Team-Fitness-Training-Quote.html", New WebFormRouteHandler("~/CCContentManagement/ClientForms/EstimateForm.aspx")))
        Routes.Add("RouteCustomClientForm2", New Route("Forms/Program-Sign-Up.html", New WebFormRouteHandler("~/CCContentManagement/ClientForms/SignUpForm.aspx")))
        Routes.Add("RouteCustomClientForm3", New Route("Forms/Grant-Applications.html", New WebFormRouteHandler("~/CCContentManagement/ClientForms/GrantsForm.aspx")))
        ''301 Redirects
        ''routes.Add("OldSite301", New Route("{*}", New LegacyRouteHandler("/Home/Index.html")))
        'Routes.Add("RedirectOldPortfolio", New Route("Portfolio.html", New LegacyRouteHandler("/Home/Portfolio.html", False)))
        'routes.Add("RedirectOldPDF", New Route("en/pdfs/{file}.pdf", New LegacyRouteHandler("~/Home.aspx")))
        'routes.Add("RedirectOldFile1A", New Route("en/{dir}/{dir2}/{dir3}/{dir4}/{file}.asp", New LegacyRouteHandler("~/Home.aspx")))
    End Sub

End Class