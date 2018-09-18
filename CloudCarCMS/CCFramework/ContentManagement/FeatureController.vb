Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class FeatureController

        Public Shared GetFeatureByIdFunc As Func(Of CommerceDataContext, Integer, Feature) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Id As Integer) (From f In db.Features Where f.Id = Id Select f).FirstOrDefault)

        Public Shared GetAllFeaturesFunc As Func(Of CommerceDataContext, IQueryable(Of Feature)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From f In db.Features Select f)

        Public Shared GetFeaturesByPropertyIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Feature)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, PropertyId As Integer) From p In db.PropertyFeatures Join f In db.Features On f.Id Equals p.PropertyId Where p.PropertyId = PropertyId Select f)

        Public Shared Function Create(ByVal Name As String, ByVal Details As String) As Integer
            Dim db As New CommerceDataContext
            Dim item As New Feature
            Dim itemId As Integer

            item.Name = Name
            item.Details = Details

            db.Features.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.Id

            item = Nothing
            db = Nothing

            Return itemId
        End Function

        Public Shared Sub Update(ByVal Id As Integer, ByVal Name As String, ByVal Details As String)
            Dim db As New CommerceDataContext

            Dim item As Feature = GetFeatureByIDFunc(db, Id)

            If item Is Nothing Then
                Throw New InvalidFeatureException("Feature does not exist")
            Else
                item.Name = Name
                item.Details = Details

                db.SubmitChanges()
            End If

            item = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim db As New CommerceDataContext

                Dim item As Feature = GetFeatureByIDFunc(db, Id)

                db.Features.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElement(ByVal Id As Integer) As Feature
            Dim db As New CommerceDataContext

            Dim item As Feature = GetFeatureByIDFunc(db, Id)

            If item Is Nothing Then
                Throw New InvalidFeatureException("Feature does not exist")
            Else
                Return item
            End If

            db = Nothing
        End Function

        Public Shared Function GetElements() As List(Of Feature)
            Dim db As New CommerceDataContext

            Dim items As List(Of Feature) = GetAllFeaturesFunc(db).ToList

            If items Is Nothing Then
                Throw New InvalidFeatureException("There are no features stored in the data table.")
            Else
                Return items
            End If

            db = Nothing
        End Function

        Public Shared Function GetFeaturesByProperty(ByVal PropertyID As Integer) As List(Of Feature)
            Dim db As New CommerceDataContext

            Dim items As List(Of Feature) = GetFeaturesByPropertyIdFunc(db, PropertyID).ToList

            If items Is Nothing Then
                Throw New InvalidFeatureException("There are no features associated with this property.")
            Else
                Return items
            End If

            db = Nothing
        End Function

    End Class

    Public Class InvalidFeatureException
        Inherits Exception

        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub

    End Class

End Namespace