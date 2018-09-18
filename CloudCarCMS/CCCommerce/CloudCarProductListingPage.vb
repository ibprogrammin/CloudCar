Imports CloudCar.CCFramework.Core
Imports CloudCar.CCControls.Commerce
Imports CloudCar.CCControls.Commerce.ProductControls
Imports CloudCar.CCControls.ContentManagement
Imports CloudCar.CCFramework.Model

Namespace CCCommerce

    Public MustInherit Class CloudCarProductListingPage
        Inherits RoutablePage

        Protected WithEvents StatusMessageLabel As Label
        Protected WithEvents ModalPopupConfirm As Global.AjaxControlToolkit.ModalPopupExtender
        Protected WithEvents SelectedProductsDisplay As Label
        Protected WithEvents PageScriptManagementControl As CloudCarScriptManagementControl

        Protected MustOverride Sub RefreshDataSources()
        Protected MustOverride Sub LoadDetails(Permalink As String)
        Protected MustOverride Function FilterProducts(Products As List(Of Product)) As List(Of Product)

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Settings.StoreEnabled Then
                Response.Redirect(Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                'TODO Move this script to shared
                PageScriptManagementControl = New CloudCarScriptManagementControl
                Dim MultiZoomScript As New ScriptLocation(String.Format("/CCTemplates/{0}/Scripts/jLensView/jlensview.js", Settings.Theme), 2)
                PageScriptManagementControl.AddScriptLocation(MultiZoomScript)

                Dim CurrentPermalink As String = (From v In RequestContext.RouteData.Values Where v.Key = "permalink" Select New With {.id = v.Value}).SingleOrDefault.id.ToString

                RefreshDataSources()

                If Not CurrentPermalink Is Nothing Then
                    LoadDetails(CurrentPermalink)
                End If
            End If

            StatusMessageLabel.Visible = False
        End Sub

        Protected Sub ProductControlCartItemAdded(ByVal Sender As Object, ByVal Args As EventArgs)
            StatusMessageLabel.Text = String.Empty

            If Settings.CartPrompt Then
                Dim CurrentShoppingDetailsControl As ShoppingDetailsControl = CType(Me.Master.FindControl("MasterShoppingDetailsControl"), ShoppingDetailsControl)

                CurrentShoppingDetailsControl.LoadDetails()

                SelectedProductsDisplay.Visible = True
                ModalPopupConfirm.Show()
            Else
                Response.RedirectToRoute("RouteShoppingCartA")
            End If
        End Sub

        Protected Sub ProductControlOutOfStock(ByVal Sender As Object, ByVal Args As ProductControlEventArgs)

            If Args.Inventory = 0 Then
                StatusMessageLabel.Text = "<p style=""color: red;"">Sorry! We currently have none of those items in stock.</p>"
            Else
                StatusMessageLabel.Text = "<p style=""color: red;"">Sorry! We currently only have ( " & Args.Inventory & " ) items in stock.</p>"
            End If

            StatusMessageLabel.Visible = True

        End Sub

        Protected Sub ProductControlAddMembership(ByVal Sender As Object, ByVal Args As ProductControlEventArgs)
            'Dim sdc As ShoppingDetailsControl = CType(Me.Master.FindControl("MasterShoppingDetailsControl"), ShoppingDetailsControl)

            'sdc.LoadDetails()

            StatusMessageLabel.Text = ""

            If Args.IsRegisteredUser Then
                If Not Args.HasMembership Then
                    Response.Redirect("~/CCCommerce/Membership/MemberApp.aspx?ProductID=" & Args.ProductID)
                Else
                    StatusMessageLabel.Text = "You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account."
                End If
            Else
                If Not Args.HasMembership Then
                    Response.Redirect("~/CCCommerce/Membership/MemberApp.aspx?ProductID=" & Args.ProductID)
                Else
                    StatusMessageLabel.Text = "You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account."
                End If

            End If

        End Sub

        Protected Sub FilterDropDownSelectedIndexChanged(Sender As Object, E As EventArgs)
            Dim CurrentPermalink As String = (From v In RequestContext.RouteData.Values Where v.Key = "permalink" Select New With {.id = v.Value}).SingleOrDefault.id.ToString

            If Not CurrentPermalink Is Nothing Then
                LoadDetails(CurrentPermalink)
            End If
        End Sub

        Protected Sub FilterByPrice(ByRef Products As List(Of Product), Value As String)
            If Not Value = "0" Then
                Dim LowPrice As Decimal
                Dim HighPrice As Decimal

                If Value.Contains("-") Then
                    Dim SeparatorIndex As Integer = Value.IndexOf("-", StringComparison.Ordinal)

                    LowPrice = CDec(Value.Substring(0, SeparatorIndex))
                    HighPrice = CDec(Value.Substring(SeparatorIndex + 1, Value.Length - (SeparatorIndex + 1)))

                    Products = Products.Where(Function(f) f.Price >= LowPrice AndAlso f.Price <= HighPrice).ToList
                Else
                    LowPrice = CDec(Value)

                    Products = Products.Where(Function(f) f.Price >= LowPrice).ToList()
                End If
            End If
        End Sub

        Protected Sub FilterByColor(ByRef Products As List(Of Product), Value As String)
            Dim CurrentColorId As Integer
            If Not Value = String.Empty Then
                CurrentColorId = Integer.Parse(Value)
            Else
                CurrentColorId = 0
            End If

            If Not CurrentColorId = 0 Then
                Products = Products.Where(Function(f) (f.ProductColors.Where(Function(p) p.ColorID = CurrentColorId)).Count > 0).ToList
            End If
        End Sub

        Protected Sub FilterByBrand(ByRef Products As List(Of Product), Value As String)
            Dim CurrentBrandId As Integer
            If Not Value = String.Empty Then
                CurrentBrandId = Integer.Parse(Value)
            Else
                CurrentBrandId = 0
            End If

            If Not CurrentBrandId = 0 Then
                Products = Products.Where(Function(f) f.BrandID = CurrentBrandId).ToList
            End If
        End Sub

    End Class

End Namespace