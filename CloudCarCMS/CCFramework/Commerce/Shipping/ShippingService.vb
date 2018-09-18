Namespace CCFramework.Commerce.Shipping

    Public Class ShippingAuthentication
        Private _UserName As String
        Private _PassWord As String
        Private _AuthenticationKey As String
        Private _CompanyName As String
        Private _AttentionName As String
        Private _PhoneNumber As String
        Private _FaxNumber As String

        Public Sub New(ByVal UserName As String, ByVal PassWord As String, ByVal AuthenticationKey As String, ByVal CompanyName As String, ByVal AttentionName As String, ByVal PhoneNumber As String, ByVal FaxNumber As String)
            _UserName = UserName
            _PassWord = PassWord
            _AuthenticationKey = AuthenticationKey
            _CompanyName = CompanyName
            _AttentionName = AttentionName
            _PhoneNumber = PhoneNumber
            _FaxNumber = FaxNumber
        End Sub

        Public Property UserName As String
            Get
                Return _UserName
            End Get
            Set(ByVal Value As String)
                _UserName = Value
            End Set
        End Property

        Public Property PassWord As String
            Get
                Return _PassWord
            End Get
            Set(ByVal Value As String)
                _PassWord = Value
            End Set
        End Property

        Public Property AuthenticationKey As String
            Get
                Return _AuthenticationKey
            End Get
            Set(ByVal Value As String)
                _AuthenticationKey = Value
            End Set
        End Property

        Public Property CompanyName() As String
            Get
                Return _CompanyName
            End Get
            Set(ByVal Value As String)
                _CompanyName = Value
            End Set
        End Property

        Public Property AttentionName() As String
            Get
                Return _AttentionName
            End Get
            Set(ByVal Value As String)
                _AttentionName = Value
            End Set
        End Property

        Public Property PhoneNumber() As String
            Get
                Return _PhoneNumber
            End Get
            Set(ByVal Value As String)
                _PhoneNumber = Value
            End Set
        End Property

        Public Property FaxNumber() As String
            Get
                Return _FaxNumber
            End Get
            Set(ByVal Value As String)
                _FaxNumber = Value
            End Set
        End Property

    End Class

    Public Class ShippingItem
        Private _Quantity As Integer
        Private _Weight As Double
        Private _Length As Double
        Private _Width As Double
        Private _Height As Double
        Private _Description As String
        Private _Price As Double

        Public Sub New(ByVal Quantity As Integer, ByVal Weight As Double, ByVal Length As Double, ByVal Width As Double, ByVal Height As Double, ByVal Description As String, Optional ByVal Price As Double = 0)
            _Quantity = quantity
            _Weight = weight
            _Length = length
            _Width = width
            _Height = height
            _Description = description
            _Price = price
        End Sub

        Property Quantity() As Integer
            Get
                Return _Quantity
            End Get
            Set(ByVal value As Integer)
                _Quantity = value
            End Set
        End Property

        Property Weight() As Double
            Get
                Return _Weight
            End Get
            Set(ByVal value As Double)
                _Weight = value
            End Set
        End Property

        Property Length() As Double
            Get
                Return _Length
            End Get
            Set(ByVal value As Double)
                _Length = value
            End Set
        End Property

        Property Width() As Double
            Get
                Return _Width
            End Get
            Set(ByVal value As Double)
                _Width = value
            End Set
        End Property

        Property Height() As Double
            Get
                Return _Height
            End Get
            Set(ByVal value As Double)
                _Height = value
            End Set
        End Property

        Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Property Price() As Double
            Get
                Return _Price
            End Get
            Set(ByVal value As Double)
                _Price = value
            End Set
        End Property

    End Class

    Public Class ShippingRate
        Private _Id As Integer
        Private _Name As String
        Private _Company As String
        Private _Description As String
        Private _Rate As Double
        Private _ShippingDate As String
        Private _DeliveryDate As String
        Private ReadOnly _GUID As Guid

        Public Sub New(ByVal Id As Integer, ByVal Name As String, ByVal Company As String, ByVal Description As String, ByVal Rate As Double, ByVal ShippingDate As String, ByVal DeliveryDate As String)
            _Id = Id
            _Name = Name
            _Company = Company
            _Description = Description
            _Rate = Rate
            _ShippingDate = ShippingDate
            _DeliveryDate = DeliveryDate
            _GUID = Guid.NewGuid()
        End Sub

        Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal Value As Integer)
                _Id = Value
            End Set
        End Property

        Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
            End Set
        End Property

        Property Company() As String
            Get
                Return _Company
            End Get
            Set(ByVal Value As String)
                _Company = Value
            End Set
        End Property

        Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

        Property Rate() As Double
            Get
                Return _Rate
            End Get
            Set(ByVal Value As Double)
                _Rate = Value
            End Set
        End Property

        Property ShippingDate() As String
            Get
                Return _ShippingDate
            End Get
            Set(ByVal Value As String)
                _ShippingDate = Value
            End Set
        End Property

        Property DeliveryDate() As String
            Get
                Return _DeliveryDate
            End Get
            Set(ByVal Value As String)
                _DeliveryDate = Value
            End Set
        End Property

        Public ReadOnly Property UniqueId() As Guid
            Get
                Return _GUID
            End Get
        End Property

    End Class

    Public Class ShippingAddress
        Private _FirstAddressLine As String
        Private _SecondAddressLine As String
        Private _City As String
        Private _ProvinceOrState As String
        Private _ProvinceOrStateCode As String
        Private _PostalCode As String
        Private _Country As String
        Private _CountryCode As String

        Public Sub New(ByVal FirstAddressLine As String, ByVal SecondAddressLine As String, ByVal City As String, ByVal ProvinceOrState As String, ByVal ProvinceOrStateCode As String, ByVal PostalCode As String, ByVal Country As String, ByVal CountryCode As String)
            _FirstAddressLine = FirstAddressLine
            _SecondAddressLine = SecondAddressLine
            _City = City
            _ProvinceOrState = ProvinceOrState
            _ProvinceOrStateCode = ProvinceOrStateCode
            _PostalCode = PostalCode
            _Country = Country
            _CountryCode = CountryCode
        End Sub

        Public Property FirstAddressLine() As String
            Get
                Return _FirstAddressLine
            End Get
            Set(ByVal Value As String)
                _FirstAddressLine = Value
            End Set
        End Property

        Public Property SecondAddressLine() As String
            Get
                Return _SecondAddressLine
            End Get
            Set(ByVal Value As String)
                _SecondAddressLine = Value
            End Set
        End Property

        Public Property City() As String
            Get
                Return _City
            End Get
            Set(ByVal Value As String)
                _City = Value
            End Set
        End Property

        Public Property ProvinceOrState() As String
            Get
                Return _ProvinceOrState
            End Get
            Set(ByVal Value As String)
                _ProvinceOrState = Value
            End Set
        End Property

        Public Property ProvinceOrStateCode() As String
            Get
                Return _ProvinceOrStateCode
            End Get
            Set(ByVal Value As String)
                _ProvinceOrStateCode = Value
            End Set
        End Property

        Public Property PostalCode() As String
            Get
                Return _PostalCode
            End Get
            Set(ByVal Value As String)
                _PostalCode = Value
            End Set
        End Property

        Public Property Country() As String
            Get
                Return _Country
            End Get
            Set(ByVal Value As String)
                _Country = Value
            End Set
        End Property

        Public Property CountryCode() As String
            Get
                Return _CountryCode
            End Get
            Set(ByVal Value As String)
                _CountryCode = Value
            End Set
        End Property

    End Class

    Public Enum Language
        English = 1
        French = 2
    End Enum

    Public Enum WeightUnit
        Pounds = 0
        Kilograms = 1
    End Enum

    Public Enum DimensionUnit
        Inches = 0
        Centimeters = 1
    End Enum

End Namespace