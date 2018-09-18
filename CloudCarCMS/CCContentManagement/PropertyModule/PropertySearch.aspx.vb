Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.ContentManagement.PropertyModule

Namespace CCContentManagement.PropertyModule

    Partial Public Class PropertySearch
        Inherits CloudCarContentPage

        'TODO Split web forms into two separate pages
        Public Sub New()
            MyBase.New()
            Permalink = Settings.PropertySearchPage
        End Sub

        Private Const _PriceStep As Integer = 50

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadSearchPage()

                LoadCities()
                LoadPrices()
            End If
        End Sub

        Private Sub LoadCities()
            Dim Cities As List(Of String) = PropertyController.GetPropertyCities()

            CityDropDown.Items.Add(New ListItem("Please select...", ""))
            CityDropDown.Items.Add(New ListItem("All", ""))

            For Each item In Cities
                CityDropDown.Items.Add(New ListItem(item, item))
            Next
        End Sub

        Private Sub LoadPrices()
            Dim LowestPrice As Decimal = PropertyController.GetLowestPropertyPrice()
            Dim HighestPrice As Decimal = PropertyController.GetHighestPropertyPrice()

            LowestPrice = LowestPrice - (LowestPrice Mod _PriceStep)
            HighestPrice = HighestPrice - (HighestPrice Mod _PriceStep) + _PriceStep

            For CurrentIndex As Integer = CInt(Math.Floor(LowestPrice)) To CInt(Math.Ceiling(HighestPrice)) Step _PriceStep
                PriceLowDropDown.Items.Add(New ListItem(CurrentIndex.ToString("C"), CurrentIndex.ToString))
                PriceHighDropDown.Items.Add(New ListItem(CurrentIndex.ToString("C"), CurrentIndex.ToString))
            Next CurrentIndex

            PriceLowDropDown.SelectedValue = PriceLowDropDown.Items(0).Value
            PriceHighDropDown.SelectedValue = PriceHighDropDown.Items(PriceHighDropDown.Items.Count - 1).Value
        End Sub

        Private Sub SearchButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles SearchButton.Click
            Dim City As String = String.Empty
            Dim PriceLow As Decimal = 0
            Dim PriceHigh As Decimal = 0
            Dim Bedrooms As Integer = 0
            Dim Bathrooms As Integer = 0

            If Not CityDropDown.SelectedValue Is Nothing Then
                City = CityDropDown.SelectedValue
            End If

            If Not PriceLowDropDown.SelectedValue Is Nothing Then
                PriceLow = Decimal.Parse(PriceLowDropDown.SelectedValue)
            End If

            If Not PriceHighDropDown.SelectedValue Is Nothing Then
                PriceHigh = Decimal.Parse(PriceHighDropDown.SelectedValue)
            End If

            If Not BedroomsTextBox.Text Is Nothing And Not BedroomsTextBox.Text = String.Empty Then
                Bedrooms = Integer.Parse(BedroomsTextBox.Text)
            End If

            If Not BathroomsTextBox.Text Is Nothing And Not BathroomsTextBox.Text = String.Empty Then
                Bathrooms = Integer.Parse(BathroomsTextBox.Text)
            End If

            LoadResultsPage()

            Dim PropertySearchResults As List(Of [Property]) = PropertyController.SearchProperties(City, PriceLow, PriceHigh, Bedrooms, Bathrooms)

            ResultsLiteral.Text = PropertySearchResults.Count.ToString

            PropertyResultsRepeater.DataSource = PropertySearchResults
            PropertyResultsRepeater.DataBind()
        End Sub

        Private Sub LoadSearchPage()
            Permalink = Settings.PropertySearchPage

            SearchPlaceHolder.Visible = True
            ResultsPlaceHolder.Visible = False
        End Sub

        Private Sub LoadResultsPage()
            Permalink = Settings.PropertyResultsPage

            SearchPlaceHolder.Visible = False
            ResultsPlaceHolder.Visible = True
        End Sub

    End Class

End NameSpace