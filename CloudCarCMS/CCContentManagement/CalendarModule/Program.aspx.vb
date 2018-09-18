Imports CloudCar.CCFramework.Model

Namespace CCContentManagement.CalendarModule

    Partial Public Class Program
        Inherits RoutablePage

        Private Const HomeLink As String = "Home"
        Private Const ProgramsLink As String = "Programs"

        Private _Permalink As String
        Private _Program As CCFramework.Model.Program

        Public Property CurrentPermalink() As String
            Get
                Return _Permalink
            End Get
            Set(ByVal Value As String)
                _Permalink = Value
            End Set
        End Property

        Public Property CurrentProgram() As CCFramework.Model.Program
            Get
                Return _Program
            End Get
            Set(ByVal Value As CCFramework.Model.Program)
                _Program = Value
            End Set
        End Property

        'Make sure to recreate any custom controls on each request or they will be unable to post any data.
        Protected Overrides Sub OnInit(ByVal E As EventArgs)
            MyBase.OnInit(E)

            If _Permalink Is Nothing Then
                CurrentPermalink = (From v In RequestContext.RouteData.Values Where v.Key = "permalink" Select New With {.id = v.Value}).SingleOrDefault.id.ToString
                CurrentProgram = CCFramework.ContentManagement.CalendarModule.ProgramController.GetProgramFromLink(_Permalink)
            End If

            LoadProgram()
        End Sub

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

        End Sub

        Private Sub LoadProgram()
            ProgramHeading.Text = _Program.Name
            ProgramContent.Text = _Program.Content

            If Not _Program.IconImage = Nothing And Not _Program.IconImage = "0" Then
                ProgramIcon.AlternateText = _Program.Name
                ProgramIcon.ImageUrl = String.Format("/images/db/{0}/full/{1}", _Program.IconImage, CCFramework.Core.PictureController.GetPictureFilename(Integer.Parse(_Program.IconImage)))
                ProgramIcon.Visible = True
            End If

            ProgramKeywords.Attributes("content") = _Program.Keywords
            ProgramDescription.Attributes("content") = _Program.Description

            Dim ProgramContentPage As ContentPage = CCFramework.ContentManagement.ContentPageController.GetPageFromLink(CCFramework.Core.Settings.ProgramsPage)

            BreadCrumbContainer.Text = String.Format("<a href=""/"">{0}</a> &raquo; <a href=""/{1}.html"">{2}</a> &raquo; {3}", HomeLink, ProgramsLink, ProgramContentPage.breadcrumbTitle, _Program.PageTitle)

            Title = _Program.PageTitle & CCFramework.Core.Settings.SiteTitle

            If CCFramework.Core.Settings.EnableSSL And CCFramework.Core.Settings.FullSSL Then
                ProgramCanonical.Attributes("href") = String.Format("https://{0}/{1}/{2}.html", CCFramework.Core.Settings.HostName, ProgramsLink, _Program.Permalink)
            Else
                ProgramCanonical.Attributes("href") = String.Format("http://{0}/{1}/{2}.html", CCFramework.Core.Settings.HostName, ProgramsLink, _Program.Permalink)
            End If

        End Sub

    End Class
End NameSpace