Namespace CCFramework.Core

    Public Class Locator

        Public Shared Function CountriesList() As List(Of ListItem)
            Dim countryList As New List(Of ListItem)

            With countryList
                .Add(New ListItem("Canada", "Canada"))
                .Add(New ListItem("USA", "USA"))
                .Add(New ListItem("Austria", "Austria"))
                .Add(New ListItem("Belgium", "Belgium"))
                .Add(New ListItem("Croatia", "Croatia"))
                .Add(New ListItem("Cyprus", "Cyprus"))
                .Add(New ListItem("Czech Republic", "Czech Republic"))
                .Add(New ListItem("Denmark", "Denmark"))
                .Add(New ListItem("France", "France"))
                .Add(New ListItem("Germany", "Germany"))
                .Add(New ListItem("Greece", "Greece"))
                .Add(New ListItem("Hungary", "Hungary"))
                .Add(New ListItem("Iceland", "Iceland"))
                .Add(New ListItem("Ireland", "Ireland"))
                .Add(New ListItem("Italy", "Italy"))
                .Add(New ListItem("Lithuania", "Lithuania"))
                .Add(New ListItem("Malta", "Malta"))
                .Add(New ListItem("Netherlands", "Netherlands"))
                .Add(New ListItem("Northern Ireland", "Northern Ireland"))
                .Add(New ListItem("Norway", "Norway"))
                .Add(New ListItem("Poland", "Poland"))
                .Add(New ListItem("Portugal", "Portugal"))
                .Add(New ListItem("Romania", "Romania"))
                .Add(New ListItem("Spain", "Spain"))
                .Add(New ListItem("Sweden", "Sweden"))
                .Add(New ListItem("Switzerland", "Switzerland"))
                .Add(New ListItem("United Kingdom", "United Kingdom"))
                .Add(New ListItem("Barbados", "Barbados"))
                .Add(New ListItem("Bermuda", "Bermuda"))
                .Add(New ListItem("Cayman Islands", "Cayman Islands"))
                .Add(New ListItem("Curaco", "Curaco"))
                .Add(New ListItem("French Polynesia", "French Polynesia"))
                .Add(New ListItem("Guadeloupe", "Guadeloupe"))
                .Add(New ListItem("Kuwait", "Kuwait"))
                .Add(New ListItem("Mexico", "Mexico"))
                .Add(New ListItem("Morocco", "Morocco"))
                .Add(New ListItem("Panama", "Panama"))
                .Add(New ListItem("St Maarten", "St Maarten"))
                .Add(New ListItem("United Arab Emirates", "United Arab Emirates"))
            End With

            Return countryList
        End Function

    End Class

End Namespace