Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class MenuItemController

        Public Shared GetMenuItemByIdFunc As Func(Of ContentDataContext, Integer, MenuItem) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, id As Integer) (From i In db.MenuItems Where i.ID = id Select i).FirstOrDefault)

        Public Shared GetAllMenuItemsFunc As Func(Of ContentDataContext, IQueryable(Of MenuItem)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From i In db.MenuItems Select i)

        Public Shared GetOrderedMenuItemsFunc As Func(Of ContentDataContext, IOrderedQueryable(Of MenuItem)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From i In db.MenuItems Select i Order By i.Order Ascending)

        Public Shared GetChildMenuItemCountFunc As Func(Of ContentDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, MenuItemId As Integer) (From i In db.MenuItems Where i.ParentID = MenuItemId Select i).Count)

        Public Shared GetChildMenuItemsFunc As Func(Of ContentDataContext, Integer, IOrderedQueryable(Of MenuItem)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, MenuItemId As Integer) From i In db.MenuItems Where i.ParentID = MenuItemId Select i Order By i.Order Ascending)

        Public Shared Function Create(ByVal Title As String, ByVal URL As String, ByVal Details As String, ByVal ParentID As Integer, ByVal [Order] As Integer, ByVal CssClass As String, ByVal Menu As String, ByVal IconImageURL As String) As Integer
            Dim db As New ContentDataContext
            Dim item As New MenuItem
            Dim itemId As Integer

            item.Title = Title
            item.URL = URL
            item.Details = Details
            item.ParentID = ParentID
            item.Order = [Order]
            item.CssClass = CssClass
            item.Menu = Menu
            item.IconImageUrl = IconImageURL

            db.MenuItems.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing
            db = Nothing

            Return itemId
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal Title As String, ByVal URL As String, ByVal Details As String, ByVal ParentID As Integer, ByVal [Order] As Integer, ByVal CssClass As String, ByVal Menu As String, ByVal IconImageURL As String)
            Dim db As New ContentDataContext

            Dim item As MenuItem = GetMenuItemByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Menu Item does not exist")
            Else
                item.Title = Title
                item.URL = URL
                item.Details = Details
                item.ParentID = ParentID
                item.Order = [Order]
                item.CssClass = CssClass
                item.Menu = Menu
                item.IconImageUrl = IconImageURL

                db.SubmitChanges()
            End If

            item = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim db As New ContentDataContext

                Dim item As MenuItem = GetMenuItemByIDFunc(db, ID)

                db.MenuItems.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElement(ByVal ID As Integer) As MenuItem
            Dim db As New ContentDataContext
            Dim item As MenuItem

            item = GetMenuItemByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Menu Item does not exist")
            Else
                Return item
            End If

            db = Nothing
        End Function

        Public Shared Function GetElements() As List(Of MenuItem)
            Dim db As New ContentDataContext

            Dim items As List(Of MenuItem) = GetAllMenuItemsFunc(db).ToList

            If items Is Nothing Then
                Throw New Exception("There are no Menu Items in the table.")
            Else
                Return items
            End If

            db = Nothing
        End Function

        Public Shared Function GetOrderedElements() As List(Of MenuItem)
            Dim db As New ContentDataContext

            Dim items As List(Of MenuItem) = GetOrderedMenuItemsFunc(db).ToList

            If items Is Nothing Then
                Throw New Exception("There are no Menu Items in the table.")
            Else
                Return items
            End If

            db = Nothing
        End Function

        Public Shared Function DoesMenuItemHaveChildren(ByVal MenuItemId As Integer) As Boolean
            Dim CurrentDataContext As New ContentDataContext

            If GetChildMenuItemCountFunc(CurrentDataContext, MenuItemId) > 0 Then
                DoesMenuItemHaveChildren = True
            Else
                DoesMenuItemHaveChildren = False
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetChildMenuItems(ByVal MenuItemId As Integer) As List(Of MenuItem)
            Dim CurrentDataContext As New ContentDataContext

            GetChildMenuItems = GetChildMenuItemsFunc(CurrentDataContext, MenuItemId).ToList

            CurrentDataContext.Dispose()
        End Function

    End Class

End Namespace