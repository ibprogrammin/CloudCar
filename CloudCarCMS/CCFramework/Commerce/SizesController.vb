Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce

    Public Class SizeController
        Inherits DataControllerClass

        Public Shared GetSizeByIDFunc As Func(Of CommerceDataContext, Integer, Size) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ID As Integer) _
                                      (From s In CurrentDataContext.Sizes Where s.ID = ID Select s).FirstOrDefault)

        Public Shared GetSizesFunc As Func(Of CommerceDataContext, IQueryable(Of Size)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                      From s In CurrentDataContext.Sizes Select s)

        Public Overloads Function Create(ByVal Name As String, ByVal Abreviation As String) As Integer
            Dim Item As New Size
            Dim ItemId As Integer

            Item.Name = Name
            Item.Abreviation = Abreviation

            db.Sizes.InsertOnSubmit(Item)
            db.SubmitChanges()

            ItemId = Item.ID

            Item = Nothing

            Return ItemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetSizeByIDFunc(db, ID)

                db.Sizes.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal Name As String, ByVal Abreviation As String) As Boolean
            Dim Item As Size

            Item = GetSizeByIDFunc(db, ID)

            If Item Is Nothing Then
                Throw New Exception("Size with ID: " & ID.ToString & " does not exist.")
            Else
                Item.Name = Name
                Item.Abreviation = Abreviation

                db.SubmitChanges()
            End If

            Item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As Size
            Dim Item As Size

            Item = GetSizeByIDFunc(db, ID)

            If Item Is Nothing Then
                Throw New Exception("Size with ID: " & ID.ToString & " does not exist.")
            Else
                Return Item
            End If

            Item = Nothing
        End Function

        Public Overloads Function GetElements() As Collections.Generic.List(Of Size)
            Dim Items As List(Of Size) = GetSizesFunc(db).ToList

            If Items Is Nothing Then
                Throw New Exception("There are no Sizes")
            Else
                Return Items
            End If
        End Function

    End Class

End Namespace