Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core.ClientControlls.BoxCalculatorController

    Public Class Room

        Public Shared GetRoomByIDFunc As Func(Of CommerceDataContext, Integer, BCRoom) = _
                CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) (From r In db.BCRooms Where r.ID = id Select r).FirstOrDefault)

        Public Shared Function Create(ByVal Name As String) As Integer
            Dim db As New CommerceDataContext

            Dim room As New BCRoom

            room.RoomName = Name

            db.BCRooms.InsertOnSubmit(room)
            db.SubmitChanges()

            Create = room.ID

            db.Dispose()
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal Name As String)
            Dim db As New CommerceDataContext
            Dim room As BCRoom

            room = GetRoomByIDFunc(db, ID)

            If room Is Nothing Then
                Throw New InvalidRoomException()
            Else
                room.RoomName = Name

                db.SubmitChanges()
            End If

            room = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal RoomID As Integer) As Boolean
            Try
                Dim db As New CommerceDataContext

                Dim page = GetRoomByIDFunc(db, RoomID)

                db.BCRooms.DeleteOnSubmit(page)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared GetAllRoomsFunc As Func(Of CommerceDataContext, IQueryable(Of BCRoom)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From r In db.BCRooms Select r)

        Public Shared Function GetRooms() As System.Linq.IQueryable(Of BCRoom)
            Dim db As New CommerceDataContext

            Dim rooms = GetAllRoomsFunc(db)

            If rooms Is Nothing Then
                Throw New InvalidRoomException()
            Else
                GetRooms = rooms
            End If

            db = Nothing
        End Function

        Public Shared Function GetRoom(ByVal ID As Integer) As BCRoom
            Dim db As New CommerceDataContext
            Dim room As BCRoom

            room = GetRoomByIDFunc(db, ID)

            If room Is Nothing Then
                Throw New InvalidRoomException()
            Else
                GetRoom = room
            End If

            db = Nothing
        End Function

        Public Class InvalidRoomException
            Inherits Exception

            Public Sub New()
                MyBase.New("The room you are looking for does not exist")
            End Sub

        End Class

    End Class


    Public Class RoomProduct

        Public Shared GetRoomProductByIDFunc As Func(Of CommerceDataContext, Integer, BCRoomProduct) = _
                CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) (From r In db.BCRoomProducts Where r.ID = id Select r).FirstOrDefault)

        Public Shared Function Create(ByVal RoomID As Integer, ByVal ProductID As Integer, ByVal Reoccurs As Boolean, ByVal Quantity As Integer) As Integer
            Dim db As New CommerceDataContext

            Dim product As New BCRoomProduct

            product.RoomID = RoomID
            product.ProductID = ProductID
            product.Reoccurs = Reoccurs
            product.Quantity = Quantity

            db.BCRoomProducts.InsertOnSubmit(product)
            db.SubmitChanges()

            Create = product.ID

            db.Dispose()
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal RoomID As Integer, ByVal ProductID As Integer, ByVal Reoccurs As Boolean, ByVal Quantity As Integer)
            Dim db As New CommerceDataContext
            Dim product As BCRoomProduct

            product = GetRoomProductByIDFunc(db, ID)

            If product Is Nothing Then
                Throw New InvalidRoomException()
            Else
                product.RoomID = RoomID
                product.ProductID = ProductID
                product.Reoccurs = Reoccurs
                product.Quantity = Quantity

                db.SubmitChanges()
            End If

            product = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal RoomProductID As Integer) As Boolean
            Try
                Dim db As New CommerceDataContext

                Dim product = GetRoomProductByIDFunc(db, RoomProductID)

                db.BCRoomProducts.DeleteOnSubmit(product)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared GetAllRoomProductsFunc As Func(Of CommerceDataContext, IQueryable(Of BCRoomProduct)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From r In db.BCRoomProducts Select r)

        Public Shared GetRoomProductsByRoomIDFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of BCRoomProduct)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, RoomID As Integer) From rp In db.BCRoomProducts Where rp.RoomID = RoomID Select rp)

        Public Shared Function GetRoomProducts() As System.Linq.IQueryable(Of BCRoomProduct)
            Dim db As New CommerceDataContext

            Dim products = GetAllRoomProductsFunc(db)

            If products Is Nothing Then
                Throw New InvalidRoomException()
            Else
                GetRoomProducts = products
            End If

            db = Nothing
        End Function

        Public Shared Function GetRoomProduct(ByVal RoomProductID As Integer) As BCRoomProduct
            Dim db As New CommerceDataContext
            Dim product As BCRoomProduct

            product = GetRoomProductByIDFunc(db, RoomProductID)

            If product Is Nothing Then
                Throw New InvalidRoomException()
            Else
                GetRoomProduct = product
            End If

            db = Nothing
        End Function

        Public Shared Function GetRoomProductsByRoom(ByVal RoomID As Integer) As System.Linq.IQueryable(Of BCRoomProduct)
            Dim db As New CommerceDataContext

            Dim products = GetRoomProductsByRoomIDFunc(db, RoomID)

            If products Is Nothing Then
                Throw New InvalidRoomException()
            Else
                GetRoomProductsByRoom = products
            End If

            db = Nothing
        End Function

        Public Class InvalidRoomException
            Inherits Exception

            Public Sub New()
                MyBase.New("The room product you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace