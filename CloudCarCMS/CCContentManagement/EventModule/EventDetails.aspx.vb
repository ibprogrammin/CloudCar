Imports CloudCar.CCFramework.ContentManagement.EventsModule
Imports CloudCar.CCFramework
Imports CloudCar.CCFramework.Core

Namespace CCContentManagement.EventModule

    Public Class EventDetails
        Inherits RoutablePage

        Public ReadOnly Property Permalink() As String
            Get
                Return (From CurrentValues In RequestContext.RouteData.Values Where CurrentValues.Key = "permalink" Select New With {.id = CurrentValues.Value}).SingleOrDefault.id.ToString
            End Get
        End Property

        Private Sub LoadEvent()
            Dim CurrentEvent As Model.Event = EventsController.GetItem(Permalink)

            If Not CurrentEvent Is Nothing Then

                Me.Title = String.Format("{0}{1}", CurrentEvent.Title, Settings.SiteTitle)

                Dim CurrentDescription As String = TextFunctions.RemoveHTML(CurrentEvent.Details)
                If CurrentDescription.Length > 160 Then
                    PageDescriptionMeta.Attributes("content") = CurrentDescription.Substring(0, 160)
                Else
                    PageDescriptionMeta.Attributes("content") = CurrentDescription
                End If

                If Settings.EnableSSL = True And Settings.FullSSL Then
                    PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/News/{1}", Settings.HostName, CurrentEvent.Permalink)
                Else
                    PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/News/{1}.html", Settings.HostName, CurrentEvent.Permalink)
                End If

                TitleLiteral.Text = CurrentEvent.Title
                ContentLiteral.Text = CurrentEvent.Details
                StartDateLiteral.Text = CType(CurrentEvent.EventDate, DateTime).ToLongDateString
                TimeLiteral.Text = String.Format(" - {0}", CurrentEvent.Time)
                LocationLiteral.Text = CurrentEvent.Location

                If CurrentEvent.ImageId.HasValue Then
                    ThumbnailImage.ImageUrl = String.Format("/images/db/{0}/full/{1}.jpg", CurrentEvent.ImageId, CurrentEvent.Title.Replace(" ", "").Replace("'", ""))
                    ThumbnailImage.Visible = True
                End If

                EventBreadCrumbControl.PageId = CurrentEvent.Id
            Else
                Throw New Exception("The Event Id has not been set")
            End If
        End Sub

        Protected Overrides Sub OnInit(ByVal Args As EventArgs)
            LoadEvent()

            MyBase.OnInit(Args)
        End Sub

    End Class

End Namespace