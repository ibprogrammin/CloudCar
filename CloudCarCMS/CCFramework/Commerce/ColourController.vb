Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce

    Public Class ColourController
        Inherits DataControllerClass

        Public Shared GetColorByIdFunc As Func(Of CommerceDataContext, Integer, Color) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, Id As Integer) _
                                      (From c In CurrentDataContext.Colors Where c.ID = Id Select c).FirstOrDefault)

        Public Shared GetColorByNameFunc As Func(Of CommerceDataContext, String, Color) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, Name As String) _
                                      (From c In CurrentDataContext.Colors Where c.Name Like Name Select c).FirstOrDefault)

        Public Shared GetColorsFunc As Func(Of CommerceDataContext, IQueryable(Of Color)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                      From c In CurrentDataContext.Colors Select c)

        Public Shared GetOrderedColorsWithProductsFunc As Func(Of CommerceDataContext, IQueryable(Of Color)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                      From c In CurrentDataContext.Colors _
                                      Join pc In CurrentDataContext.ProductColors _
                                      On pc.ColorID Equals c.ID _
                                        Select c Distinct Order By c.RGBColourCode Descending)

        Public Overloads Function Create(ByVal Name As String, ByVal RgbColourCode As String) As Integer
            Dim Item As New Color
            Dim ItemId As Integer

            Item.Name = Name
            Item.RGBColourCode = RgbColourCode

            db.Colors.InsertOnSubmit(Item)
            db.SubmitChanges()

            ItemId = Item.ID

            Item = Nothing

            Return ItemId
        End Function

        Public Overloads Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim Item As Color = GetColorByIDFunc(db, Id)

                db.Colors.DeleteOnSubmit(Item)
                db.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal Id As Integer, ByVal Name As String, ByVal RgbColourCode As String) As Boolean
            Dim Item As Color

            Item = GetColorByIDFunc(db, Id)

            If item Is Nothing Then
                Throw New Exception("Color with ID: " & ID.ToString & " does not exist.")
            Else
                Item.Name = Name
                Item.RGBColourCode = RgbColourCode

                db.SubmitChanges()
            End If

            Item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal Id As Integer) As Color
            Dim Item As Color

            Item = GetColorByIDFunc(db, Id)

            If Item Is Nothing Then
                Throw New Exception("Color with ID: " & ID.ToString & " does not exist.")
            Else
                Return Item
            End If

            Item = Nothing
        End Function

        Public Overloads Function GetElement(ByVal Name As String) As Color
            Dim Item As Color

            Item = GetColorByNameFunc(db, Name)

            If Item Is Nothing Then
                Throw New Exception("Color with Name: " & Name & " does not exist.")
            Else
                Return Item
            End If
        End Function

        Public Overloads Function GetElements() As List(Of Color)
            Dim Items As List(Of Color) = GetColorsFunc(db).ToList

            If Items Is Nothing Then
                Throw New Exception("There are no Colors")
            Else
                Return Items

            End If
        End Function

        Public Shared Function GetColorsWithProducts() As List(Of Color)
            Dim CurrentDataContext As New CommerceDataContext

            Dim Items As List(Of Color) = GetOrderedColorsWithProductsFunc(CurrentDataContext).ToList

            If Items Is Nothing Then
                Throw New Exception("There are no Colors")
            Else
                GetColorsWithProducts = Items
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetColorNameByIdFunc As Func(Of CommerceDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ColorsId As Integer) _
                                      (From c In CurrentDataContext.Colors _
                                       Where c.ID = ColorsId _
                                       Select c.Name).FirstOrDefault)

        Public Shared Function GetBreadCrumb(ByVal ColorId As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentColor As String = GetColorNameByIdFunc(CurrentDataContext, ColorId)

            Dim BreadCrumbStringBuilder As New StringBuilder
            BreadCrumbStringBuilder.AppendFormat("<a href=""/"">Home</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("<a href=""/Shop/Index.html"">Shop</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("{0}", CurrentColor)

            GetBreadCrumb = BreadCrumbStringBuilder.ToString

            CurrentDataContext.Dispose()
        End Function

    End Class

End Namespace