Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core

    Public Class ApplicationFunctions

        Public Shared Function StripShortString(ByVal text As String, ByVal length As Integer) As String
            Dim newContent As String = RemoveHTML(text)

            If newContent.Length > length Then
                newContent = text.Substring(0, length) & "..."
            Else
                newContent = text
            End If

            Return newContent
        End Function

        Public Shared Function RemoveHTML(ByVal html As String) As String
            Dim parsedString As String = html

            parsedString = Regex.Replace(html, "<(.|\n)*?>", String.Empty)

            Return parsedString
        End Function

        Public Shared Function GetWebsitePaybackTotal() As PaybackTotal
            Return New PaybackTotal
        End Function

        Public Shared Function DisplaySizeColor(ByVal Color As String, ByVal Size As String) As String
            If Color = "None" AndAlso Not Size = "None" Then
                Return "" & Size & ""
            ElseIf Not Color = "None" AndAlso Size = "None" Then
                Return "" & Color & ""
            ElseIf Not Color = "None" AndAlso Not Size = "None" Then
                Return "" & Color & "/" & Size & "<br />"
            Else
                Return ""
            End If
        End Function

    End Class

    Public Class SalesFigure
        Private _id As Integer
        Private _name As String
        Private _quantitySold As Integer
        Private _cost As Decimal
        Private _retail As Decimal
        Private _profit As Decimal

        Public Shared GetSalesFiguresFunc As Func(Of CommerceDataContext, IQueryable(Of SalesFigure)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) _
                            From oi In db.OrderItems _
                            Join o In db.Orders On o.ID Equals oi.OrderID _
                            Join p In db.Products On p.ID Equals oi.ProductID _
                            Where o.ApprovalState = 1 And oi.Price > 0 _
                            Group By p.ID, p.Name Into QuantitySold = Sum(oi.Quantity), Cost = Sum(oi.Quantity * p.Cost), Retail = Sum(oi.Quantity * oi.Price) _
                            Select New SalesFigure With {.ID = ID, .Name = Name, .QuantitySold = QuantitySold, .Cost = Cost, .Retail = Retail, .Profit = Retail - Cost})

        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property QuantitySold() As Integer
            Get
                Return _quantitySold
            End Get
            Set(ByVal value As Integer)
                _quantitySold = value
            End Set
        End Property

        Public Property Cost() As Decimal
            Get
                Return _cost
            End Get
            Set(ByVal value As Decimal)
                _cost = value
            End Set
        End Property

        Public Property Retail() As Decimal
            Get
                Return _retail
            End Get
            Set(ByVal value As Decimal)
                _retail = value
            End Set
        End Property

        Public Property Profit() As Decimal
            Get
                Return _profit
            End Get
            Set(ByVal value As Decimal)
                _profit = value
            End Set
        End Property

    End Class

    Public Class PaybackTotal
        Private ReadOnly _SalesFigures As List(Of SalesFigure)

        Public Shared WebsiteCost As Decimal = Settings.WebsiteCost

        Public Sub New()
            Dim db As New CommerceDataContext

            _SalesFigures = Core.SalesFigure.GetSalesFiguresFunc(db).ToList
        End Sub

        Public Property SalesFigure(ByVal index As Integer) As SalesFigure
            Get
                Return _SalesFigures(index)
            End Get
            Set(ByVal value As SalesFigure)
                _SalesFigures(index) = value
            End Set
        End Property

        Public Function getCost() As Decimal
            Dim cost As Decimal = 0

            For Each item In _SalesFigures
                cost += item.Cost
            Next

            Return cost
        End Function

        Public Function getRetail() As Decimal
            Dim retail As Decimal = 0

            For Each item In _SalesFigures
                retail += item.Retail
            Next

            Return retail
        End Function

        Public Function getProfit() As Decimal
            Dim profit As Decimal = 0

            For Each item In _SalesFigures
                profit += item.Profit
            Next

            Return profit
        End Function

    End Class

End Namespace