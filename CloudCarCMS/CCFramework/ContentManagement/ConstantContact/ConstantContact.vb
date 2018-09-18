#Region "Imports"

Imports System.Data
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Text
Imports System.Web.UI

Imports ConstantContactBO
Imports ConstantContactUtility

#End Region

Namespace SMConstantContact

    ''' <summary>
    ''' This class contains API Key, Username and Password used to acces Constant resources.
    ''' Also, it contains definition for State/Province and Country.
    ''' It exposes methods to retrieve the user Contact List collection from the Constant 
    ''' server with or withoud the pre-filtering enabled
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ConstantContact

#Region "Authentication constants"
        ''' <summary>
        ''' API Application Key and is used to identify the application making an API request
        ''' </summary>
        Private Shared ReadOnly apiKey As String = ConfigurationManager.AppSettings("CCApiKey")
        ''' <summary>
        ''' Constant Contact Customer's user name
        ''' </summary>
        Private Shared ReadOnly username As String = ConfigurationManager.AppSettings("CCUsername")
        ''' <summary>
        ''' Constant Contact Customer's password
        ''' </summary>
        Private Shared ReadOnly password As String = ConfigurationManager.AppSettings("CCPassword")
#End Region

#Region "Constants"
        ''' <summary>
        ''' United States country code
        ''' </summary>
        Public Const UnitedStatesCountryCode As String = "us"
        ''' <summary>
        ''' Canada country code
        ''' </summary>
        Public Const CanadaCountryCode As String = "ca"
#End Region

#Region "Fields"
        Private Shared _authenticationData As AuthenticationData
#End Region

#Region "Properties"
        Public Shared ReadOnly Property AuthenticationData() As AuthenticationData
            Get
                Return _authenticationData
            End Get
        End Property
#End Region

#Region "Constructor"
        Shared Sub New()
            _authenticationData = New AuthenticationData(apiKey, username, password)
        End Sub
#End Region

#Region "Contact List"

        Public Shared Function GetUserContactListCollection(ByVal clientScript As ClientScriptManager) As List(Of ContactList)

            Dim list As List(Of ContactList) = Nothing
            Try

                Dim nextChunk As String = Nothing
                ' get first chunk of user Contact Lists from the server
                Dim currentContactLists As IList(Of ContactList) = Utility.GetUserContactListCollection(AuthenticationData, nextChunk)
                ' apply the prefilter to the list collection
                Dim prefilteredContactList As IEnumerable(Of ContactList) = PrefilterContactLists(currentContactLists)
                ' store the prefiltered collection
                list = New List(Of ContactList)(prefilteredContactList)

                While Not String.IsNullOrEmpty(nextChunk)
                    Dim currentChunk As String = nextChunk

                    currentContactLists.Clear()
                    ' get current chunk of user Contact Lists from the server
                    currentContactLists = Utility.GetUserContactListCollection(AuthenticationData, currentChunk, nextChunk)
                    ' apply the prefilter to the list collection
                    prefilteredContactList = PrefilterContactLists(currentContactLists)
                    ' store the prefiltered collection
                    list.AddRange(prefilteredContactList)
                End While

                ' sort contact list by Sort Order field
                'list.Sort(Utility.CompareContactListsBySortOrder)
            Catch ce As ConstantException
                Dim StringBuilder As StringBuilder = New StringBuilder()
                StringBuilder.Append("<script language='javascript'>")
                StringBuilder.AppendFormat("alert('{0}')", ce.Message)
                StringBuilder.Append("</script>")

                clientScript.RegisterStartupScript(GetType(Page), "AlertMessage", StringBuilder.ToString())
            End Try

            Return list
        End Function

#End Region

#Region "Prefiltering"

        Public Shared Function PrefilterContactLists(ByVal contactLists As IEnumerable(Of ContactList)) As IEnumerable(Of ContactList)
            Return contactLists
            '' define the collection of lists name do you want
            'string[] prefilterNames = new string[] { "List 1", "List 2", "List 3" };

            'foreach (ContactList contactList in contactLists)
            '{
            '    foreach (string contactListName in prefilterNames)
            '    {
            '        if (string.Equals(contactList.Name, contactListName, System.StringComparison.Ordinal))
            '        {
            '            yield return contactList;
            '        }
            '    }
            '}
        End Function

#End Region


#Region "State/Province & Country definition"

        ''' <summary>
        ''' Gets the Country collection
        ''' </summary>
        ''' <returns>Array of strings containing Country names</returns>
        Public Shared Function GetCountryCollection() As DataTable

            Dim countryTable As DataTable = New DataTable()
            countryTable.Columns.Add("Name")
            countryTable.Columns.Add("Code")

            AddCountryToDataTable(countryTable, String.Empty, String.Empty)
            AddCountryToDataTable(countryTable, "United States", UnitedStatesCountryCode)
            AddCountryToDataTable(countryTable, "Canada", CanadaCountryCode)
            AddCountryToDataTable(countryTable, "Afghanistan", "af")
            AddCountryToDataTable(countryTable, "Aland Islands", "ax")
            AddCountryToDataTable(countryTable, "Albania", "al")
            AddCountryToDataTable(countryTable, "Algeria", "dz")
            AddCountryToDataTable(countryTable, "American Samoa", "as")
            AddCountryToDataTable(countryTable, "Andorra", "ad")
            AddCountryToDataTable(countryTable, "Angola", "ao")
            AddCountryToDataTable(countryTable, "Anguilla", "ai")
            AddCountryToDataTable(countryTable, "Antarctica", "aq")
            AddCountryToDataTable(countryTable, "Antigua and Barbuda", "ag")
            AddCountryToDataTable(countryTable, "Argentina", "ar")
            AddCountryToDataTable(countryTable, "Armenia", "am")
            AddCountryToDataTable(countryTable, "Aruba", "aw")
            AddCountryToDataTable(countryTable, "Australia", "au")
            AddCountryToDataTable(countryTable, "Austria", "at")
            AddCountryToDataTable(countryTable, "Azerbaijan", "az")
            AddCountryToDataTable(countryTable, "Bahamas", "bs")
            AddCountryToDataTable(countryTable, "Bahrain", "bh")
            AddCountryToDataTable(countryTable, "Bangladesh", "bd")
            AddCountryToDataTable(countryTable, "Barbados", "bb")
            AddCountryToDataTable(countryTable, "Belarus", "by")
            AddCountryToDataTable(countryTable, "Belgium", "be")
            AddCountryToDataTable(countryTable, "Belize", "bz")
            AddCountryToDataTable(countryTable, "Benin", "bj")
            AddCountryToDataTable(countryTable, "Bermuda", "bm")
            AddCountryToDataTable(countryTable, "Bhutan", "bt")
            AddCountryToDataTable(countryTable, "Bolivia", "bo")
            AddCountryToDataTable(countryTable, "Bosnia and Herzegovina", "ba")
            AddCountryToDataTable(countryTable, "Botswana", "bw")
            AddCountryToDataTable(countryTable, "Bouvet Island", "bv")
            AddCountryToDataTable(countryTable, "Brazil", "br")
            AddCountryToDataTable(countryTable, "British Indian Ocean Territory", "io")
            AddCountryToDataTable(countryTable, "Brunei Darussalam", "bn")
            AddCountryToDataTable(countryTable, "Bulgaria", "bg")
            AddCountryToDataTable(countryTable, "Burkina Faso", "bf")
            AddCountryToDataTable(countryTable, "Burundi", "bi")
            AddCountryToDataTable(countryTable, "Cambodia", "kh")
            AddCountryToDataTable(countryTable, "Cameroon", "cm")
            AddCountryToDataTable(countryTable, "Cape Verde", "cv")
            AddCountryToDataTable(countryTable, "Cayman Islands", "ky")
            AddCountryToDataTable(countryTable, "Central African Republic", "cf")
            AddCountryToDataTable(countryTable, "Chad", "td")
            AddCountryToDataTable(countryTable, "Chile", "cl")
            AddCountryToDataTable(countryTable, "China", "cn")
            AddCountryToDataTable(countryTable, "Christmas Island", "cx")
            AddCountryToDataTable(countryTable, "Cocos (Keeling) Islands", "cc")
            AddCountryToDataTable(countryTable, "Colombia", "co")
            AddCountryToDataTable(countryTable, "Comoros", "km")
            AddCountryToDataTable(countryTable, "Congo", "cg")
            AddCountryToDataTable(countryTable, "Congo, Democratic Republic of", "cd")
            AddCountryToDataTable(countryTable, "Cook Islands", "ck")
            AddCountryToDataTable(countryTable, "Costa Rica", "cr")
            AddCountryToDataTable(countryTable, "Cote D'Ivoire", "ci")
            AddCountryToDataTable(countryTable, "Croatia", "hr")
            AddCountryToDataTable(countryTable, "Cyprus", "cy")
            AddCountryToDataTable(countryTable, "Czech Republic", "cz")
            AddCountryToDataTable(countryTable, "Denmark", "dk")
            AddCountryToDataTable(countryTable, "Djibouti", "dj")
            AddCountryToDataTable(countryTable, "Dominica", "dm")
            AddCountryToDataTable(countryTable, "Dominican Republic", "do")
            AddCountryToDataTable(countryTable, "East Timor", "tl")    'remark:updated from server response
            AddCountryToDataTable(countryTable, "Ecuador", "ec")
            AddCountryToDataTable(countryTable, "Egypt", "eg")
            AddCountryToDataTable(countryTable, "El Salvador", "sv")
            AddCountryToDataTable(countryTable, "England", "U1") 'remark: updated from server response
            AddCountryToDataTable(countryTable, "Equatorial Guinea", "gq")
            AddCountryToDataTable(countryTable, "Eritrea", "er")
            AddCountryToDataTable(countryTable, "Estonia", "ee")
            AddCountryToDataTable(countryTable, "Ethiopia", "et")
            AddCountryToDataTable(countryTable, "Faroe Islands", "fo")
            AddCountryToDataTable(countryTable, "Faukland Islands", "fk")
            AddCountryToDataTable(countryTable, "Fiji", "fj")
            AddCountryToDataTable(countryTable, "Finland", "fi")
            AddCountryToDataTable(countryTable, "France", "fr")
            AddCountryToDataTable(countryTable, "French Guyana", "gf")
            AddCountryToDataTable(countryTable, "French Polynesia", "pf")
            AddCountryToDataTable(countryTable, "French Southern Territories", "tf")
            AddCountryToDataTable(countryTable, "Gabon", "ga")
            AddCountryToDataTable(countryTable, "Gambia", "gm")
            AddCountryToDataTable(countryTable, "Georgia", "ge")
            AddCountryToDataTable(countryTable, "Germany", "de")
            AddCountryToDataTable(countryTable, "Ghana", "gh")
            AddCountryToDataTable(countryTable, "Gibraltar", "gi")
            AddCountryToDataTable(countryTable, "Greece", "gr")
            AddCountryToDataTable(countryTable, "Greenland", "gl")
            AddCountryToDataTable(countryTable, "Grenada", "gd")
            AddCountryToDataTable(countryTable, "Guadeloupe", "gp")
            AddCountryToDataTable(countryTable, "Guam", "gu")
            AddCountryToDataTable(countryTable, "Guatemala", "gt")
            AddCountryToDataTable(countryTable, "Guernsey", "gg")
            AddCountryToDataTable(countryTable, "Guinea", "gn")
            AddCountryToDataTable(countryTable, "Guinea-Bissau", "gw")
            AddCountryToDataTable(countryTable, "Guyana", "gy")
            AddCountryToDataTable(countryTable, "Haiti", "ht")
            AddCountryToDataTable(countryTable, "Heard and McDonald Islands", "hm")
            AddCountryToDataTable(countryTable, "Honduras", "hn")
            AddCountryToDataTable(countryTable, "Hong Kong", "hk")
            AddCountryToDataTable(countryTable, "Hungary", "hu")
            AddCountryToDataTable(countryTable, "Iceland", "is")
            AddCountryToDataTable(countryTable, "India", "in")
            AddCountryToDataTable(countryTable, "Indonesia", "id")
            AddCountryToDataTable(countryTable, "Iraq", "iq")
            AddCountryToDataTable(countryTable, "Ireland", "ie")
            AddCountryToDataTable(countryTable, "Isle of Man", "im")
            AddCountryToDataTable(countryTable, "Israel", "il")
            AddCountryToDataTable(countryTable, "Italy", "it")
            AddCountryToDataTable(countryTable, "Jamaica", "jm")
            AddCountryToDataTable(countryTable, "Japan", "jp")
            AddCountryToDataTable(countryTable, "Jersey", "je")
            AddCountryToDataTable(countryTable, "Jordan", "jo")
            AddCountryToDataTable(countryTable, "Kazakhstan", "kz")
            AddCountryToDataTable(countryTable, "Kenya", "ke")
            AddCountryToDataTable(countryTable, "Kiribati", "ki")
            AddCountryToDataTable(countryTable, "Kuwait", "kw")
            AddCountryToDataTable(countryTable, "Kyrgyzstan", "kg")
            AddCountryToDataTable(countryTable, "Laos", "la")
            AddCountryToDataTable(countryTable, "Latvia", "lv")
            AddCountryToDataTable(countryTable, "Lebanon", "lb")
            AddCountryToDataTable(countryTable, "Lesotho", "ls")
            AddCountryToDataTable(countryTable, "Liberia", "lr")
            AddCountryToDataTable(countryTable, "Libya", "ly")
            AddCountryToDataTable(countryTable, "Liechtenstein", "li")
            AddCountryToDataTable(countryTable, "Lithuania", "lt")
            AddCountryToDataTable(countryTable, "Luxembourg", "lu")
            AddCountryToDataTable(countryTable, "Macao", "mo")
            AddCountryToDataTable(countryTable, "Macedonia", "mk")
            AddCountryToDataTable(countryTable, "Madagascar", "mg")
            AddCountryToDataTable(countryTable, "Malawi", "mw")
            AddCountryToDataTable(countryTable, "Malaysia", "my")
            AddCountryToDataTable(countryTable, "Maldives", "mv")
            AddCountryToDataTable(countryTable, "Mali", "ml")
            AddCountryToDataTable(countryTable, "Malta", "mt")
            AddCountryToDataTable(countryTable, "Marshall Islands", "mh")
            AddCountryToDataTable(countryTable, "Martinique", "mq")
            AddCountryToDataTable(countryTable, "Mauritania", "mr")
            AddCountryToDataTable(countryTable, "Mauritius", "mu")
            AddCountryToDataTable(countryTable, "Mayotte", "yt")
            AddCountryToDataTable(countryTable, "Mexico", "mx")
            AddCountryToDataTable(countryTable, "Micronesia", "fm")
            AddCountryToDataTable(countryTable, "Moldova", "md")
            AddCountryToDataTable(countryTable, "Monaco", "mc")
            AddCountryToDataTable(countryTable, "Mongolia", "mn")
            AddCountryToDataTable(countryTable, "Montenegro", "me")
            AddCountryToDataTable(countryTable, "Montserrat", "ms")
            AddCountryToDataTable(countryTable, "Morocco", "ma")
            AddCountryToDataTable(countryTable, "Mozambique", "mz")
            AddCountryToDataTable(countryTable, "Myanmar", "mm")
            AddCountryToDataTable(countryTable, "Namibia", "na")
            AddCountryToDataTable(countryTable, "Nauru", "nr")
            AddCountryToDataTable(countryTable, "Nepal", "np")
            AddCountryToDataTable(countryTable, "Netherlands", "nl")
            AddCountryToDataTable(countryTable, "Netherlands Antilles", "an")
            AddCountryToDataTable(countryTable, "Neutral Zone", "nt") 'remark:updated from server response
            AddCountryToDataTable(countryTable, "New Caledonia", "nc")
            AddCountryToDataTable(countryTable, "New Zealand", "nz")
            AddCountryToDataTable(countryTable, "Nicaragua", "ni")
            AddCountryToDataTable(countryTable, "Niger", "ne")
            AddCountryToDataTable(countryTable, "Nigeria", "ng")
            AddCountryToDataTable(countryTable, "Niue", "nu")
            AddCountryToDataTable(countryTable, "Norfolk Island", "nf")
            AddCountryToDataTable(countryTable, "Northern Ireland", "U4") 'remark:updated from server response
            AddCountryToDataTable(countryTable, "Northern Mariana Islands", "mp")
            AddCountryToDataTable(countryTable, "Norway", "no")
            AddCountryToDataTable(countryTable, "Oman", "om")
            AddCountryToDataTable(countryTable, "Pakistan", "pk")
            AddCountryToDataTable(countryTable, "Palau", "pw")
            AddCountryToDataTable(countryTable, "Palestinian Territory, Occupied", "ps")
            AddCountryToDataTable(countryTable, "Panama", "pa")
            AddCountryToDataTable(countryTable, "Papua New Guinea", "pg")
            AddCountryToDataTable(countryTable, "Paraguay", "py")
            AddCountryToDataTable(countryTable, "Peru", "pe")
            AddCountryToDataTable(countryTable, "Philippines", "ph")
            AddCountryToDataTable(countryTable, "Pitcairn", "pn")
            AddCountryToDataTable(countryTable, "Poland", "pl")
            AddCountryToDataTable(countryTable, "Portugal", "pt")
            AddCountryToDataTable(countryTable, "Puerto Rico", "pr")
            AddCountryToDataTable(countryTable, "Qatar", "qa")
            AddCountryToDataTable(countryTable, "Reunion", "re")
            AddCountryToDataTable(countryTable, "Romania", "ro")
            AddCountryToDataTable(countryTable, "Russian Federation", "ru")
            AddCountryToDataTable(countryTable, "Rwanda", "rw")
            AddCountryToDataTable(countryTable, "Saint Barthelemy", "bl")
            AddCountryToDataTable(countryTable, "Saint Helena", "sh")
            AddCountryToDataTable(countryTable, "Saint Kitts and Nevis", "kn")
            AddCountryToDataTable(countryTable, "Saint Lucia", "lc")
            AddCountryToDataTable(countryTable, "Saint Martin", "mf")
            AddCountryToDataTable(countryTable, "Saint Pierre and Miquelon", "pm")
            AddCountryToDataTable(countryTable, "Saint Vincent & the Grenadines", "vc")
            AddCountryToDataTable(countryTable, "Samoa", "ws")
            AddCountryToDataTable(countryTable, "San Marino", "sm")
            AddCountryToDataTable(countryTable, "Sao Tome and Principe", "st")
            AddCountryToDataTable(countryTable, "Saudi Arabia", "sa")
            AddCountryToDataTable(countryTable, "Scotland", "U3") 'remark:updated from server response
            AddCountryToDataTable(countryTable, "Senegal", "sn")
            AddCountryToDataTable(countryTable, "Serbia", "rs")
            AddCountryToDataTable(countryTable, "Seychelles", "sc")
            AddCountryToDataTable(countryTable, "Sierra Leone", "sl")
            AddCountryToDataTable(countryTable, "Singapore", "sg")
            AddCountryToDataTable(countryTable, "Slovakia", "sk")
            AddCountryToDataTable(countryTable, "Slovenia", "si")
            AddCountryToDataTable(countryTable, "Solomon Islands", "sb")
            AddCountryToDataTable(countryTable, "Somalia", "so")
            AddCountryToDataTable(countryTable, "South Africa", "za")
            AddCountryToDataTable(countryTable, "South Georgia & S. Sandwich Is.", "gs")
            AddCountryToDataTable(countryTable, "South Korea", "kr")
            AddCountryToDataTable(countryTable, "Spain", "es")
            AddCountryToDataTable(countryTable, "Sri Lanka", "lk")
            AddCountryToDataTable(countryTable, "Suriname", "sr")
            AddCountryToDataTable(countryTable, "Svalbard and Jan Mayen", "sj")
            AddCountryToDataTable(countryTable, "Swaziland", "sz")
            AddCountryToDataTable(countryTable, "Sweden", "se")
            AddCountryToDataTable(countryTable, "Switzerland", "ch")
            AddCountryToDataTable(countryTable, "Taiwan", "tw")
            AddCountryToDataTable(countryTable, "Tajikistan", "tj")
            AddCountryToDataTable(countryTable, "Tanzania", "tz")
            AddCountryToDataTable(countryTable, "Thailand", "th")
            AddCountryToDataTable(countryTable, "Togo", "tg")
            AddCountryToDataTable(countryTable, "Tokelau", "tk")
            AddCountryToDataTable(countryTable, "Tonga", "to")
            AddCountryToDataTable(countryTable, "Trinidad and Tobago", "tt")
            AddCountryToDataTable(countryTable, "Tunisia", "tn")
            AddCountryToDataTable(countryTable, "Turkey", "tr")
            AddCountryToDataTable(countryTable, "Turkmenistan", "tm")
            AddCountryToDataTable(countryTable, "Turks and Caicos Islands", "tc")
            AddCountryToDataTable(countryTable, "Tuvalu", "tv")
            AddCountryToDataTable(countryTable, "Uganda", "ug")
            AddCountryToDataTable(countryTable, "Ukraine", "ua")
            AddCountryToDataTable(countryTable, "United Arab Emirates", "ae")
            AddCountryToDataTable(countryTable, "United Kingdom", "gb")
            AddCountryToDataTable(countryTable, "United States Minor Outlying Is.", "um")
            AddCountryToDataTable(countryTable, "Uruguay", "uy")
            AddCountryToDataTable(countryTable, "Uzbekistan", "uz")
            AddCountryToDataTable(countryTable, "Vanuatu", "vu")
            AddCountryToDataTable(countryTable, "Vatican City State", "va")
            AddCountryToDataTable(countryTable, "Venezuela", "ve")
            AddCountryToDataTable(countryTable, "Viet Nam", "vn")
            AddCountryToDataTable(countryTable, "Virgin Islands, British", "vg")
            AddCountryToDataTable(countryTable, "Virgin Islands, U.S.", "vi")
            AddCountryToDataTable(countryTable, "Wales", "u2")
            AddCountryToDataTable(countryTable, "Wallis and Futuna", "wf")
            AddCountryToDataTable(countryTable, "Western Sahara", "eh")
            AddCountryToDataTable(countryTable, "Yemen", "ye")
            AddCountryToDataTable(countryTable, "Zambia", "zm")
            AddCountryToDataTable(countryTable, "Zimbabwe", "zw")

            Return countryTable
        End Function

        ''' <summary>
        ''' Gets the State/Province (US/Canada) collection
        ''' </summary>
        ''' <returns>Array of strings representing State/Provice (US/Canada) names</returns>
        Public Shared Function GetStateCollection() As DataTable
            Dim provinceTable As DataTable = New DataTable()
            provinceTable.Columns.Add("Name")
            provinceTable.Columns.Add("Code")
            provinceTable.Columns.Add("CountryCode")

            AddProvinceToDataTable(provinceTable, String.Empty, String.Empty, String.Empty)
            AddProvinceToDataTable(provinceTable, "Alabama", "AL", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Alaska", "AK", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Alberta", "AB", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "Arizona", "AZ", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Arkansas", "AR", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Armed Forces Americas", "AA", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Armed Forces Europe", "AE", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Armed Forces Pacific", "AP", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "British Columbia", "BC", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "California", "CA", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Colorado", "CO", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Connecticut", "CT", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Delaware", "DE", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "District of Columbia", "DC", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Florida", "FL", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Georgia", "GA", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Hawaii", "HI", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Idaho", "ID", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Illinois", "IL", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Indiana", "IN", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Iowa", "IA", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Kansas", "KS", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Kentucky", "KY", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Louisiana", "LA", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Maine", "ME", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Manitoba", "MB", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "Maryland", "MD", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Massachusetts", "MA", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Michigan", "MI", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Minnesota", "MN", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Mississippi", "MS", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Missouri", "MO", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Montana", "MT", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Nebraska", "NE", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Nevada", "NV", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "New Brunswick", "NB", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "New Hampshire", "NH", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "New Jersey", "NJ", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "New Mexico", "NM", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "New York", "NY", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Newfoundland and Labrador", "NL", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "North Carolina", "NC", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "North Dakota", "ND", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Northwest Territories", "NT", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "Nova Scotia", "NS", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "Nunavut", "NU", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "Ohio", "OH", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Oklahoma", "OK", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Ontario", "ON", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "Oregon", "OR", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Pennsylvania", "PA", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Prince Edward Island", "PE", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "Quebec", "QC", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "Rhode Island", "RI", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Saskatchewan", "SK", CanadaCountryCode)
            AddProvinceToDataTable(provinceTable, "South Carolina", "SC", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "South Dakota", "SD", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Tennessee", "TN", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Texas", "TX", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Utah", "UT", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Vermont", "VT", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Virginia", "VA", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Washington", "WA", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "West Virginia", "WV", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Wisconsin", "WI", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Wyoming", "WY", UnitedStatesCountryCode)
            AddProvinceToDataTable(provinceTable, "Yukon Territory", "YT", CanadaCountryCode)

            Return provinceTable
        End Function

        ''' <summary>
        ''' Creates a new row for the data table using specified name and code
        ''' </summary>
        ''' <param name="table"></param>
        ''' <param name="name"></param>
        ''' <param name="code"></param>
        Private Shared Sub AddCountryToDataTable(ByVal table As DataTable, ByVal name As String, ByVal code As String)
            Dim row As DataRow = table.NewRow()
            row("Name") = name
            row("Code") = code
            table.Rows.Add(row)
        End Sub

        ''' <summary>
        ''' Creates a new row for the data table using specified name and code and country code
        ''' </summary>
        ''' <param name="table"></param>
        ''' <param name="name"></param>
        ''' <param name="code"></param>
        ''' <param name="countryCode"></param>
        Private Shared Sub AddProvinceToDataTable(ByVal table As DataTable, ByVal name As String, ByVal code As String, ByVal countryCode As String)
            Dim row As DataRow = table.NewRow()
            row("Name") = name
            row("Code") = code
            row("CountryCode") = countryCode
            table.Rows.Add(row)
        End Sub

#End Region

    End Class

End Namespace