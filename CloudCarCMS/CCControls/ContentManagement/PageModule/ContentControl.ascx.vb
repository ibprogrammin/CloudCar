Imports CloudCar.CCFramework.ContentManagement

Namespace CCControls.ContentManagement.PageModule

    Public Class ContentControl
        Inherits UserControl

        Public Property PageId() As Integer
            Get
                Return CInt(ViewState("PageId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageId") = Value

                If Not PageContentType = Nothing Then
                    LoadPageContent()
                End If
            End Set
        End Property

        Public Property PageContentType() As ContentType
            Get
                Return CType(ViewState("PageContentType"), ContentType)
            End Get
            Set(ByVal Value As ContentType)
                ViewState("PageContentType") = Value

                If Not PageId = Nothing Then
                    LoadPageContent()
                End If
            End Set
        End Property

        Private Sub LoadPageContent()
            Dim CurrentContent As String

            Select Case PageContentType
                Case ContentType.Primary
                    CurrentContent = ContentPageController.GetPageContent(PageId)
                Case ContentType.Secondary
                    CurrentContent = ContentPageController.GetPageSecondaryContent(PageId)
                Case Else
                    CurrentContent = ContentPageController.GetPageContent(PageId)
            End Select

            'TODO Change to -#!COMMENTBOX!#- to avoid the issue with Html Decoding
            If CurrentContent.Contains("[COMMENTBOX]") _
                    OrElse CurrentContent.Contains("[NEWSBOX]") _
                    OrElse CurrentContent.Contains("[TESTIMONIALBOX]") _
                    OrElse CurrentContent.Contains("[SIGNUPBOX]") Then

                Dim CurrentSeperatorStrings As String() = {"[", "]"}
                Dim CurrentPageContent As String() = CurrentContent.Split(CurrentSeperatorStrings, StringSplitOptions.RemoveEmptyEntries)

                For Each Item As String In CurrentPageContent
                    Select Case Item.Trim
                        Case "COMMENTBOX"
                            Dim CurrentCommentBox As ContactControl = CType(LoadControl("~/CCControls/ContentManagement/ContactControl.ascx"), ContactControl)
                            CurrentCommentBox.ID = "cbComments"
                            ContentPlaceHolder.Controls.Add(CurrentCommentBox)
                        Case "NEWSBOX"
                            Dim CurrentNewsBox As RecentEventAndNewsControl = CType(LoadControl("~/CCControls/ContentManagement/RecentEventAndNewsControl.ascx"), RecentEventAndNewsControl)
                            CurrentNewsBox.ID = "cbNews"
                            ContentPlaceHolder.Controls.Add(CurrentNewsBox)
                        Case "SIGNUPBOX"
                            Dim CurrentSignUpBox As SubscribeControl = CType(LoadControl("~/CCControls/SubscribeControl.ascx"), SubscribeControl)
                            CurrentSignUpBox.ID = "cbSignUp"
                            ContentPlaceHolder.Controls.Add(CurrentSignUpBox)
                        Case "TESTIMONIALBOX"
                            Dim CurrentTestimonialBox As TestimonialControl = CType(LoadControl("~/CCControls/ContentManagement/TestimonialControl.ascx"), TestimonialControl)
                            CurrentTestimonialBox.ID = "cbTestimonial"
                            CurrentTestimonialBox.Count = 1
                            ContentPlaceHolder.Controls.Add(CurrentTestimonialBox)
                        Case Else
                            ContentPlaceHolder.Controls.Add(New Literal() With {.Text = Item})
                    End Select
                Next
            Else
                ContentPlaceHolder.Controls.Add(New Literal() With {.Text = CurrentContent})
            End If
        End Sub

    End Class

    Public Enum ContentType
        None = 0
        Primary = 1
        Secondary = 2
    End Enum

End Namespace