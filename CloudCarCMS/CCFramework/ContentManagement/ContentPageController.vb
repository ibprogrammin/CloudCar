Imports System.Data.Linq
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCFramework.ContentManagement

    Public Class ContentPageController

        Public Shared GetPageByIdFunc As Func(Of ContentDataContext, Integer, ContentPage) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, Id As Integer) (From p In db.ContentPages Where p.id = Id Select p).FirstOrDefault)

        Public Shared GetPageShowHeadingByIdFunc As Func(Of ContentDataContext, Integer, Boolean) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From p In CurrentDataContext.ContentPages _
                                       Where p.id = Id Select p.ShowHeading).FirstOrDefault)

        Public Shared GetPageTitleByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From p In CurrentDataContext.ContentPages _
                                       Where p.id = Id Select p.contentTitle).FirstOrDefault)

        Public Shared GetPageBrowserTitleByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) (From p In CurrentDataContext.ContentPages Where p.id = Id Select p.pageTitle).FirstOrDefault)

        Public Shared GetPageContentByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) (From p In CurrentDataContext.ContentPages Where p.id = Id Select p.pageContent).FirstOrDefault)

        Public Shared GetPageSecondaryContentByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) (From p In CurrentDataContext.ContentPages Where p.id = Id Select p.secondaryContent).FirstOrDefault)

        Public Shared GetPageCallToActionIdByIdFunc As Func(Of ContentDataContext, Integer, Integer?) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From p In CurrentDataContext.ContentPages _
                                       Where p.id = Id Select p.CallToActionId).FirstOrDefault)

        Public Shared GetPageHeaderImageIdByIdFunc As Func(Of ContentDataContext, Integer, Integer?) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) (From p In CurrentDataContext.ContentPages Where p.id = Id Select p.headerImageID).FirstOrDefault)

        Public Shared GetPagePermalinkByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) (From p In CurrentDataContext.ContentPages Where p.id = Id Select p.permalink).FirstOrDefault)

        Public Shared GetPageScriptByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) (From p In CurrentDataContext.ContentPages Where p.id = Id Select p.script).FirstOrDefault)

        Public Shared GetPageKeywordsByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) (From p In CurrentDataContext.ContentPages Where p.id = Id Select p.keywords).FirstOrDefault)

        Public Shared GetPageDescriptionByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) (From p In CurrentDataContext.ContentPages Where p.id = Id Select p.description).FirstOrDefault)

        Public Shared GetAllPagesFunc As Func(Of ContentDataContext, IQueryable(Of ContentPage)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From p In db.ContentPages Select p)

        Public Shared Function Create(ByVal ContentTitle As String, ByVal PageTitle As String, ByVal BreadCrumbTitle As String, ByVal PermaLink As String, ByVal Keywords As String, ByVal Description As String, ByVal Content As String, ByVal SecondaryContent As String, ByVal MenuID As Integer, ByVal MenuOrder As Integer, ByVal ParentPageID As Integer, ByVal Script As String, ByVal DisplayInSubMenu As Boolean, ShowHeading As Boolean, CallToActionId As Integer) As Integer
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentContentPage As New ContentPage

            CurrentContentPage.contentTitle = ContentTitle
            CurrentContentPage.pageTitle = PageTitle
            CurrentContentPage.breadcrumbTitle = BreadCrumbTitle
            CurrentContentPage.permalink = PermaLink
            CurrentContentPage.keywords = Keywords
            CurrentContentPage.description = Description
            CurrentContentPage.pageContent = Content
            CurrentContentPage.secondaryContent = SecondaryContent
            CurrentContentPage.parentPageID = ParentPageID
            CurrentContentPage.script = Script
            CurrentContentPage.displaysubmenu = DisplayInSubMenu
            CurrentContentPage.ShowHeading = ShowHeading
            CurrentContentPage.menuID = MenuID
            CurrentContentPage.menuOrder = MenuOrder
            CurrentContentPage.CallToActionId = CallToActionId

            CurrentDataContext.ContentPages.InsertOnSubmit(CurrentContentPage)
            CurrentDataContext.SubmitChanges()

            Dim NewPageId As Integer = CurrentContentPage.id

            CurrentDataContext.Dispose()

            Return NewPageId
        End Function

        Public Shared Sub Update(ByVal PageId As Integer, ByVal ContentTitle As String, ByVal PageTitle As String, ByVal BreadCrumbTitle As String, ByVal PermaLink As String, ByVal Keywords As String, ByVal Description As String, ByVal Content As String, ByVal SecondaryContent As String, ByVal MenuID As Integer, ByVal MenuOrder As Integer, ByVal ParentPageID As Integer, ByVal Script As String, ByVal DisplayInSubMenu As Boolean, ShowHeading As Boolean, CallToActionId As Integer)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentContentPage As ContentPage

            CurrentContentPage = GetPageByIdFunc(CurrentDataContext, PageId)

            If CurrentContentPage Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                CurrentContentPage.contentTitle = ContentTitle
                CurrentContentPage.pageTitle = PageTitle
                CurrentContentPage.ShowHeading = ShowHeading
                CurrentContentPage.permalink = PermaLink
                CurrentContentPage.breadcrumbTitle = BreadCrumbTitle
                CurrentContentPage.keywords = Keywords
                CurrentContentPage.description = Description
                CurrentContentPage.pageContent = Content
                CurrentContentPage.secondaryContent = SecondaryContent
                CurrentContentPage.parentPageID = ParentPageID
                CurrentContentPage.script = Script
                CurrentContentPage.displaysubmenu = DisplayInSubMenu
                CurrentContentPage.CallToActionId = CallToActionId

                CurrentContentPage.menuID = MenuID
                CurrentContentPage.menuOrder = MenuOrder

                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Function Delete(ByVal PageId As Integer) As Boolean
            Try
                Dim CurrentDataContext As New ContentDataContext

                Dim CurrentPage = GetPageByIdFunc(CurrentDataContext, PageId)

                CurrentDataContext.ContentPages.DeleteOnSubmit(CurrentPage)
                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Shared GetPageByTitleFunc As Func(Of ContentDataContext, String, ContentPage) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, title As String) (From p In db.ContentPages Where p.breadcrumbTitle Like title Select p).FirstOrDefault)

        Public Shared GetPageByPermalinkFunc As Func(Of ContentDataContext, String, ContentPage) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, permalink As String) (From p In db.ContentPages Where p.permalink Like permalink Select p).FirstOrDefault)

        Public Shared GetPageIdByPermalinkFunc As Func(Of ContentDataContext, String, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, Permalink As String) _
                                      (From p In db.ContentPages _
                                       Where p.permalink.ToLower Like Permalink _
                                       Select p.id).FirstOrDefault)

        Public Shared GetPermalinkExistsFunc As Func(Of ContentDataContext, String, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, permalink As String) (From p In db.ContentPages Where p.permalink Like permalink Select p).Count)

        Public Shared GetPermalinkExistsUpdateFunc As Func(Of ContentDataContext, String, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, permalink As String, pageId As Integer) (From p In db.ContentPages Where p.permalink Like permalink And Not p.id = pageId Select p).Count)

        Public Shared Function GetElement() As List(Of ContentPage)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentPages As List(Of ContentPage) = GetAllPagesFunc(CurrentDataContext).ToList

            If CurrentPages Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetElement = CurrentPages
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetElement(ByVal PageId As Integer) As ContentPage
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentPage As ContentPage

            CurrentPage = GetPageByIdFunc(CurrentDataContext, PageId)

            If CurrentPage Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetElement = CurrentPage
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetElement(ByVal Title As String) As ContentPage
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentPage As ContentPage

            CurrentPage = GetPageByTitleFunc(CurrentDataContext, Title)

            If CurrentPage Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetElement = CurrentPage
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetPageIdFromPermalink(ByVal Permalink As String) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentPageId As Integer = GetPageIdByPermalinkFunc(CurrentDataContext, Permalink.ToLower)

            If CurrentPageId = Nothing Then
                GetPageIdFromPermalink = 0
            Else
                GetPageIdFromPermalink = CurrentPageId
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetPageBrowserTitle(ByVal PageId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentTitle As String = GetPageBrowserTitleByIdFunc(CurrentDataContext, PageId)

            If CurrentTitle Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetPageBrowserTitle = CurrentTitle

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetPageContentTitle(ByVal PageId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentTitle As String = GetPageTitleByIdFunc(CurrentDataContext, PageId)

            If CurrentTitle Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetPageContentTitle = CurrentTitle

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetPageShowHeading(ByVal PageId As Integer) As Boolean
            Dim CurrentDataContext As New ContentDataContext
            Dim ShowHeading As Boolean = GetPageShowHeadingByIdFunc(CurrentDataContext, PageId)

            If ShowHeading Then
                CurrentDataContext.Dispose()

                Return True
            Else
                CurrentDataContext.Dispose()

                Return False
            End If
        End Function

        Public Shared Function GetPageCallToActioinId(ByVal PageId As Integer) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentCallToActionId As Integer? = GetPageCallToActionIdByIdFunc(CurrentDataContext, PageId)

            If Not CurrentCallToActionId.HasValue Then
                CurrentDataContext.Dispose()

                GetPageCallToActioinId = -1
            Else
                GetPageCallToActioinId = CInt(CurrentCallToActionId)

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetPageContent(ByVal PageId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentContent As String = GetPageContentByIdFunc(CurrentDataContext, PageId)

            If CurrentContent Is Nothing Then
                GetPageContent = ""
            Else
                GetPageContent = CurrentContent
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetPageSecondaryContent(ByVal PageId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentContent As String = GetPageSecondaryContentByIdFunc(CurrentDataContext, PageId)

            If CurrentContent Is Nothing Then
                GetPageSecondaryContent = ""
            Else
                GetPageSecondaryContent = CurrentContent
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetPageHeaderImageId(ByVal PageId As Integer) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentImageId As Integer? = GetPageHeaderImageIdByIdFunc(CurrentDataContext, PageId)

            If CurrentImageId.HasValue Then
                GetPageHeaderImageId = CurrentImageId.Value
            Else
                GetPageHeaderImageId = 0
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetPagePermalink(ByVal PageId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentPermalink As String = GetPagePermalinkByIdFunc(CurrentDataContext, PageId)

            If CurrentPermalink Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetPagePermalink = CurrentPermalink

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetPageScript(ByVal PageId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentScript As String = GetPageScriptByIdFunc(CurrentDataContext, PageId)

            If CurrentScript Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetPageScript = CurrentScript

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetPageKeywords(ByVal PageId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentKeywords As String = GetPageKeywordsByIdFunc(CurrentDataContext, PageId)

            If CurrentKeywords Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetPageKeywords = CurrentKeywords

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetPageDescription(ByVal PageId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentDescription As String = GetPageDescriptionByIdFunc(CurrentDataContext, PageId)

            If CurrentDescription Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetPageDescription = CurrentDescription

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetPageFromLink(ByVal Permalink As String) As ContentPage
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentContentPage As ContentPage

            CurrentContentPage = GetPageByPermalinkFunc(CurrentDataContext, Permalink)

            If CurrentContentPage Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetPageFromLink = CurrentContentPage

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function HasPermalink(ByVal Permalink As String) As Boolean
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentPageCount As Integer = GetPermalinkExistsFunc(CurrentDataContext, Permalink)

            If CurrentPageCount = 0 Then
                HasPermalink = False
            Else
                HasPermalink = True
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function HasPermalink(ByVal Permalink As String, ByVal PageId As Integer) As Boolean
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentPageCount As Integer = GetPermalinkExistsUpdateFunc(CurrentDataContext, Permalink, PageId)

            If CurrentPageCount = 0 Then
                HasPermalink = False
            Else
                HasPermalink = True
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetBreadCrumb(ByVal PageId As Integer, ByVal RootPage As String) As String
            Dim CurrentDataContext As New ContentDataContext

            Dim HasMoreNodes As Boolean = True
            Dim TopLevel As Boolean = True
            Dim CurrentOrder As Integer = 0
            Dim CurrentPageId As Integer = PageID

            Dim CurrentBreadCrumbs As New List(Of PageBreadCrumb)
            While HasMoreNodes = True
                'TODO fix the recursive error and make sure the user cant pick a page the is already linked in the bread crumb list.
                Dim CurrentBreadCrumbPage = GetPageByIdFunc(CurrentDataContext, CurrentPageId)

                CurrentBreadCrumbs.Add(New PageBreadCrumb(CurrentBreadCrumbPage.id, CurrentBreadCrumbPage.permalink, CurrentBreadCrumbPage.breadcrumbTitle, CurrentOrder, TopLevel))

                If Not CurrentBreadCrumbPage.parentPageID.HasValue OrElse CurrentBreadCrumbPage.parentPageID = 0 OrElse CurrentBreadCrumbPage.parentPageID = CurrentBreadCrumbPage.id Then
                    HasMoreNodes = False
                Else
                    If CurrentBreadCrumbPage.parentPageID.HasValue Then
                        CurrentPageId = CurrentBreadCrumbPage.parentPageID.Value
                    End If
                End If

                TopLevel = False
                CurrentOrder += 1
            End While

            Dim CurrentBreadCrumb As New StringBuilder

            CurrentBreadCrumb.AppendFormat("<a href='/{0}.html'>Home</a> {1} ", RootPage, Settings.BreadCrumbDelimiter)
            For Each CurrentItem As PageBreadCrumb In CurrentBreadCrumbs.OrderByDescending(Function(t) t.Order)
                If CurrentItem.IsTopLevel Then
                    CurrentBreadCrumb.Append(CurrentItem.Title)
                Else
                    CurrentBreadCrumb.AppendFormat("<a href='/{0}/{1}.html'>{2}</a> {3} ", RootPage, CurrentItem.Permalink, CurrentItem.Title, Settings.BreadCrumbDelimiter)
                End If
            Next

            GetBreadCrumb = CurrentBreadCrumb.ToString

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetPagesByParentPageIdFunc As Func(Of ContentDataContext, Integer, IQueryable(Of ContentPage)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) From p In CurrentDataContext.ContentPages Where p.parentPageID = Id Select p)

        Public Shared Function GetNestedPages(ByVal PageId As Integer) As List(Of ContentPage)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentContentPages As List(Of ContentPage) = GetPagesByParentPageIdFunc(CurrentDataContext, PageId).ToList

            If CurrentContentPages Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidContentPageException()
            Else
                GetNestedPages = CurrentContentPages

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared GetSubMenuLinksFunc As Func(Of ContentDataContext, IQueryable(Of SubMenuItem)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) _
                                      From c In db.ContentPages Where c.displaysubmenu = True Order By c.menuOrder _
                                      Select New SubMenuItem(c.permalink, c.breadcrumbTitle))

        Public Class InvalidContentPageException
            Inherits Exception

            Public Sub New()
                MyBase.New("The page you are looking for does not exist")
            End Sub

        End Class

        Public Class SubMenuItem
            Private pLink As String
            Private pTitle As String

            Public Sub New(ByVal link As String, ByVal title As String)
                pLink = link
                pTitle = title
            End Sub

            Public Property Link() As String
                Get
                    Return pLink
                End Get
                Set(ByVal value As String)
                    pLink = value
                End Set
            End Property

            Public Property Title() As String
                Get
                    Return pTitle
                End Get
                Set(ByVal value As String)
                    pTitle = value
                End Set
            End Property

        End Class

    End Class

    Public Class MenuController

        Public Shared Function CreateMenu(ByVal Title As String) As Integer
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentMenu As New Menu

            CurrentMenu.menu = Title

            CurrentDataContext.Menus.InsertOnSubmit(CurrentMenu)
            CurrentDataContext.SubmitChanges()

            CreateMenu = CurrentMenu.id

            CurrentDataContext.Dispose()
        End Function

        Public Shared Sub UpdateMenu(ByVal ID As Integer, ByVal Title As String)
            Dim CurrentDataContext As New ContentDataContext
            Dim menu As Menu

            menu = (From m In CurrentDataContext.Menus Where m.id = ID).SingleOrDefault

            If menu Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidMenuException()
            Else
                menu.menu = Title

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared GetMenusFunc As Func(Of ContentDataContext, IQueryable(Of Menu)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From m In CurrentDataContext.Menus Select m)

        Public Shared GetMenuByIdFunc As Func(Of ContentDataContext, Integer, Menu) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, MenuId As Integer) _
                                      (From m In CurrentDataContext.Menus _
                                       Where m.id = MenuId _
                                       Select m).FirstOrDefault)

        Public Shared Function GetMenu() As List(Of Menu)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentMenus As List(Of Menu) = GetMenusFunc(CurrentDataContext).ToList

            If CurrentMenus Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidMenuException()
            Else
                GetMenu = CurrentMenus

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetMenu(ByVal MenuId As Integer) As Menu
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentMenu As Menu = GetMenuByIdFunc(CurrentDataContext, MenuId)

            If CurrentMenu Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidMenuException()
            Else
                GetMenu = CurrentMenu

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetMenu(ByVal Title As String) As Menu
            Dim CurrentDataContext As New ContentDataContext
            Dim menu As Menu

            menu = (From m In CurrentDataContext.Menus Where m.menu Like Title).SingleOrDefault

            If menu Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidMenuException()
            Else
                GetMenu = menu

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Class InvalidMenuException
            Inherits Exception

            Public Sub New()
                MyBase.New("The menu you are looking for does not exist")
            End Sub

        End Class

    End Class

    Friend Class PageBreadCrumb
        Private _pageID As Integer
        Private _permalink As String
        Private _title As String
        Private _order As Integer
        Private _isTopLevel As Boolean

        Public Sub New(ByVal PageId As Integer, ByVal Permalink As String, ByVal Title As String, ByVal Order As Integer, ByVal IsTopLevel As Boolean)
            _pageID = PageId
            _permalink = Permalink
            _title = Title
            _order = Order
            _isTopLevel = IsTopLevel
        End Sub

        Public ReadOnly Property PageID() As Integer
            Get
                Return _pageID
            End Get
        End Property

        Public Property Permalink() As String
            Get
                Return _permalink
            End Get
            Set(ByVal value As String)
                _permalink = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Order() As Integer
            Get
                Return _order
            End Get
            Set(ByVal value As Integer)
                _order = value
            End Set
        End Property

        Public ReadOnly Property IsTopLevel() As Boolean
            Get
                Return _isTopLevel
            End Get
        End Property

    End Class

End Namespace