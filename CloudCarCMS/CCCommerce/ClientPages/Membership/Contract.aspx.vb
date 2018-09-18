Namespace CCCommerce.Membership
    Public Partial Class Contract
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If Not Request.QueryString("Product") Is Nothing Then
                    ProductID = CInt(Request.QueryString("Product"))
                Else
                    Response.Redirect("~/CCCommerce/Categories.aspx")
                End If
                If Not Request.QueryString("User") Is Nothing Then
                    UserID = CInt(Request.QueryString("User"))
                Else
                    Response.Redirect("~/CCCommerce/Categories.aspx")
                End If
            End If
        End Sub

        Private Sub btnSubmit_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnSubmit.Command

            If ckbAgree.Checked Then
                Response.Redirect("~/CCCommerce/ShoppingCart.aspx?Memberhip=yes&Product=" & ProductID & "&User=" & UserID & "&terms=yes")
            Else
                lblStatus.Text = "Please check that you agree to the terms of our membership agreement."
            End If

        End Sub

        Public Property UserID() As Integer
            Get
                If Session("UserID") Is Nothing Then
                    Session.Add("UserID", Nothing)
                End If

                Return CInt(Session("UserID"))
            End Get
            Set(ByVal value As Integer)
                If Not Session("UserID") Is Nothing Then
                    Session("UserID") = value
                Else
                    Session.Add("UserID", value)
                End If
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                If Session("ProductID") Is Nothing Then
                    Session.Add("ProductID", Nothing)
                End If

                Return CInt(Session("ProductID"))
            End Get
            Set(ByVal value As Integer)
                If Not Session("ProductID") Is Nothing Then
                    Session("ProductID") = value
                Else
                    Session.Add("ProductID", value)
                End If
            End Set
        End Property

    End Class
End NameSpace