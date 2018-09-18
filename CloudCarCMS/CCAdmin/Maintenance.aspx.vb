Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCAdmin

    Partial Public Class Maintenance
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadImageInformation()
                LoadAddressInformation()
                LoadShoppingCartInformation()
            End If
        End Sub

        Public Sub LoadImageInformation()
            TotalImagesLabel.Text = PictureController.GetPictureCount.ToString
            UsedImagesLabel.Text = PictureController.GetUsedPictures.Count.ToString
            UnusedImagesLabel.Text = PictureController.GetUnusedPictures.Count.ToString
            WaistedBytesLabel.Text = String.Format("{0} MB", (PictureController.GetWaistedImageSpace / 1000000).ToString)

            UnusedImagesRepeater.DataSource = PictureController.GetUnusedPictures.OrderBy(Function(p) p.PictureFileName)
            UnusedImagesRepeater.DataBind()
        End Sub

        Public Sub LoadAddressInformation()
            Dim CurrentAddresses As List(Of Address) = New AddressController().GetElements
            Dim CurrentUnusedAddresses As List(Of Address) = AddressController.GetUnusedAddresses

            TotalAddressesLabel.Text = CurrentAddresses.Count.ToString
            LinkedAddressesLabel.Text = (CurrentAddresses.Count - CurrentUnusedAddresses.Count).ToString
            UnusedAddressesLabel.Text = CurrentUnusedAddresses.Count.ToString

            UnusedAddressRepeater.DataSource = CurrentUnusedAddresses.OrderBy(Function(a) a.Address).ToList
            UnusedAddressRepeater.DataBind()
        End Sub

        Public Sub LoadShoppingCartInformation()
            TotalShoppingCartsLabel.Text = ShoppingCartController.GetUniqueShoppingCartCount.ToString
            RegisteredShoppingCartsLabel.Text = ShoppingCartController.GetUniqueRegisteredShoppingCartCount.ToString
            UnregisteredShoppingCartsLabel.Text = ShoppingCartController.GetUniqueUnregisteredShoppingCartCount.ToString
        End Sub

        Private Sub DeleteUnusedImagesButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles DeleteUnusedImagesButton.Click
            If PictureController.DeleteUnusedPictures() Then
                StatusMessageLabel.Text = String.Format("All unused images have been successfully removed from the database.")
                StatusMessageLabel.Visible = True

                LoadImageInformation()
            End If
        End Sub

        Public Sub DeleteSelectedImagesButtonClick(ByVal Sender As Object, ByVal Args As EventArgs)
            For Each CurrentItem As RepeaterItem In UnusedImagesRepeater.Items

                For Each CurrentControl As Control In CurrentItem.Controls
                    If CurrentControl.GetType.ToString = GetType(CheckBox).ToString Then
                        Dim CurrentCheckBox As CheckBox = CType(CurrentControl, CheckBox)

                        If CurrentCheckBox.Checked Then
                            Dim ImageId As Integer = Integer.Parse(CurrentCheckBox.Attributes("PictureID").ToString)

                            PictureController.DeletePicture(ImageId)
                        End If
                    End If
                Next
            Next

            StatusMessageLabel.Text = String.Format("The selected unused images have been successfully removed from the database.")
            StatusMessageLabel.Visible = True

            LoadImageInformation()
        End Sub

        Private Sub DeleteUnusedAddressesButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles DeleteUnusedAddressesButton.Click
            If AddressController.DeleteUnusedAddresses() > 0 Then
                StatusMessageLabel.Text = String.Format("All unused addresses have been successfully removed from the database.")
                StatusMessageLabel.Visible = True

                LoadAddressInformation()
            Else
                StatusMessageLabel.Text = String.Format("There are no addresses to be deleted.")
                StatusMessageLabel.Visible = True
            End If
        End Sub

        Public Sub DeleteSelectedAddressButtonClick(ByVal Sender As Object, ByVal Args As EventArgs)
            Dim CurrentAddressController As New AddressController()

            For Each CurrentItem As RepeaterItem In UnusedAddressRepeater.Items

                For Each CurrentControl As Control In CurrentItem.Controls
                    If CurrentControl.GetType.ToString = GetType(CheckBox).ToString Then
                        Dim CurrentCheckBox As CheckBox = CType(CurrentControl, CheckBox)

                        If CurrentCheckBox.Checked Then
                            Dim CurrentAddressId As Integer = Integer.Parse(CurrentCheckBox.Attributes("AddressID").ToString)

                            CurrentAddressController.Delete(CurrentAddressId)
                        End If
                    End If
                Next
            Next

            StatusMessageLabel.Text = String.Format("The selected unused addresses have been successfully removed from the database.")
            StatusMessageLabel.Visible = True

            LoadAddressInformation()
        End Sub

        Private Sub DeleteUnregisteredShoppingCartsButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles DeleteUnregisteredShoppingCartsButton.Click
            Dim DeleteShoppingCarts As Boolean = ShoppingCartController.DeleteUnregisteredShoppingCarts()

            If DeleteShoppingCarts Then
                StatusMessageLabel.Text = String.Format("All unregistered shopping carts have been successfully removed from the database.")
                StatusMessageLabel.Visible = True

                LoadShoppingCartInformation()
            End If
        End Sub

    End Class

End Namespace