Namespace CCControls

    Partial Public Class SearchControl
        Inherits UserControl

        Private Sub SearchButtonServerClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles SearchButton.Click

            'If Session("SearchQuery") Is Nothing Then
            'Session.Add("SearchQuery", SearchTextBox.Text)
            'Else
            'Session("SearchQuery") = SearchTextBox.Text
            'End If

            'Response.RedirectToRoute("RouteSearch")
            Response.Redirect(String.Format("/Search/Results.html?q={0}", Server.HtmlEncode(SearchTextBox.Text)))
        End Sub

    End Class

End Namespace