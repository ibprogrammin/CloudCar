Namespace CCContentManagement

    Partial Public Class CmsPage
        Inherits CloudCarContentPage

        Protected Overrides Sub OnInit(ByVal Args As EventArgs)
            If Permalink Is Nothing Then
                Permalink = (From CurrentValues In RequestContext.RouteData.Values Where CurrentValues.Key = "permalink" Select New With {.id = CurrentValues.Value}).FirstOrDefault.id.ToString
            End If

            MyBase.OnInit(Args)
        End Sub

    End Class

End Namespace