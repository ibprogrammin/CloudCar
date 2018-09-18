Imports CloudCar.CCControls.ContentManagement.PageModule
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.ContentManagement
Imports CloudCar.CCControls.ContentManagement

Namespace CCContentManagement

    ''' <summary>
    ''' CloudCarContentPage is a framework class for building custom content pages.
    ''' The class must be inherited and you must implement all the controls found below.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class CloudCarContentPage
        Inherits RoutablePage

        Private ReadOnly _ErrorPageUrl As String = "/Home/Error.html"

        'Private ReadOnly _JQueryScriptLocation As String
        'Private ReadOnly _MainScriptLocation As String
        'Private ReadOnly _AdaptScriptLocation As String

        Protected WithEvents PageKeywordsMeta As HtmlGenericControl
        Protected WithEvents PageDescriptionMeta As HtmlGenericControl
        Protected WithEvents PageCanonicalMeta As HtmlGenericControl
        Protected WithEvents PageHeaderControl As HeaderControl
        Protected WithEvents PageRotatorControl As RotatorControl
        Protected WithEvents PageHeaderImageControl As HeaderImageControl
        Protected WithEvents PageContentControl As ContentControl
        Protected WithEvents PageChildMenuControl As ChildPageMenuControl
        Protected WithEvents PageBreadCrumbControl As BreadCrumbControl
        Protected WithEvents PageSecondaryContentControl As ContentControl
        Protected WithEvents PageScriptManagementControl As CloudCarScriptManagementControl
        Protected WithEvents PageScriptControl As ScriptControl
        Protected WithEvents PageCallToActionControl As CallToActionControl

        Public Sub New()
            '_JQueryScriptLocation = "http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"
            '_MainScriptLocation = String.Format("/CCTemplates/{0}/Scripts/main.js", Settings.Theme)
            '_AdaptScriptLocation = String.Format("/CCTemplates/{0}/Scripts/adapt.js", Settings.Theme)
        End Sub

        Private _PagePermalink As String
        Public Property Permalink() As String
            Get
                Return _PagePermalink
            End Get
            Set(ByVal Value As String)
                _PagePermalink = Value
            End Set
        End Property

        Protected Sub LoadContentPage(ByVal _Permalink As String, ByRef _PageHeaderControl As HeaderControl, ByRef _PageRotatorControl As RotatorControl, ByRef _PageBreadCrumbControl As BreadCrumbControl, ByRef _PageContentControl As ContentControl, ByRef _PageSecondaryContentControl As ContentControl, ByRef _PageHeaderImageControl As HeaderImageControl, ByVal ByRef_PageScriptManagementControl As CloudCarScriptManagementControl, ByRef _PageScriptControl As ScriptControl, ByRef _PageChildMenuControl As ChildPageMenuControl, ByRef _PageKeywordsMeta As HtmlGenericControl, ByRef _PageDescriptionMeta As HtmlGenericControl, ByRef _PageCanonicalMeta As HtmlGenericControl, _PageCallToActionControl As CallToActionControl)
            Dim CurrentPageId As Integer = ContentPageController.GetPageIdFromPermalink(_Permalink)

            If CurrentPageId = Nothing Then
                Response.Redirect(String.Format("{0}", _ErrorPageUrl))
            End If

            '_PageScriptManagementControl.AddScriptLocation(New ScriptLocation(_JQueryScriptLocation, 1))
            '_PageScriptManagementControl.AddScriptLocation(New ScriptLocation(_MainScriptLocation, 5))
            '_PageScriptManagementControl.AddScriptLocation(New ScriptLocation(_AdaptScriptLocation, 6))

            _PageHeaderControl.PageId = CurrentPageId
            _PageRotatorControl.PageId = CurrentPageId
            _PageBreadCrumbControl.PageId = CurrentPageId
            _PageContentControl.PageId = CurrentPageId
            _PageSecondaryContentControl.PageId = CurrentPageId
            _PageHeaderImageControl.PageId = CurrentPageId
            _PageScriptControl.PageId = CurrentPageId
            _PageChildMenuControl.PageId = CurrentPageId
            _PageCallToActionControl.PageId = CurrentPageId

            _PageKeywordsMeta.Attributes("content") = ContentPageController.GetPageKeywords(CurrentPageId)
            _PageDescriptionMeta.Attributes("content") = ContentPageController.GetPageDescription(CurrentPageId)

            If Settings.EnableSSL = True And Settings.FullSSL Then
                If Not _Permalink.ToLower = "home" Then
                    _PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/Home/{1}.html", Settings.HostName, _Permalink)
                Else
                    _PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/", Settings.HostName)
                End If
            Else
                If Not _Permalink.ToLower = "home" Then
                    _PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/Home/{1}.html", Settings.HostName, _Permalink)
                Else
                    _PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/", Settings.HostName)
                End If
            End If

            Title = String.Format("{0}{1}", ContentPageController.GetPageBrowserTitle(CurrentPageId), Settings.SiteTitle)
        End Sub

        Protected Overrides Sub OnInit(ByVal Args As EventArgs)
            LoadContentPage(_PagePermalink, PageHeaderControl, PageRotatorControl, PageBreadCrumbControl, PageContentControl, PageSecondaryContentControl, PageHeaderImageControl, PageScriptManagementControl, PageScriptControl, PageChildMenuControl, PageKeywordsMeta, PageDescriptionMeta, PageCanonicalMeta, PageCallToActionControl)
        End Sub

    End Class

End Namespace