Namespace CCControls.Validators

    Public Class PermalinkValidator
        Inherits BaseValidator

        Private _permalinkType As PermalinkType
        Private _itemID As Integer

        Public Sub New()

        End Sub

        Protected Overrides Function ControlPropertiesValid() As Boolean
            Dim ctrl As Control = TryCast(FindControl(ControlToValidate), TextBox)
            Return (Not ctrl Is Nothing)
        End Function

        Protected Overrides Function EvaluateIsValid() As Boolean
            If Not ItemID = Nothing Then
                Return CheckPermalinkValid(CType(Me.FindControl(Me.ControlToValidate), TextBox).Text, _permalinkType, ItemID)
            Else
                Return CheckPermalinkValid(CType(Me.FindControl(Me.ControlToValidate), TextBox).Text, _permalinkType)
            End If
        End Function

        Public Shared Function CheckPermalinkValid(ByVal ValidatePermalink As String, ByVal CurrentPermalinkType As PermalinkType) As Boolean
            Dim isValid As Boolean = False

            Select Case currentPermalinkType
                Case PermalinkType.ContentPage
                    If Not CCFramework.ContentManagement.ContentPageController.HasPermalink(validatePermalink) Then
                        isValid = True
                    End If
                Case PermalinkType.Blog
                    If Not CCFramework.Blogging.BlogController.HasPermalink(validatePermalink) Then
                        isValid = True
                    End If
                Case PermalinkType.Author
                    If Not CCFramework.Blogging.AuthorController.HasPermalink(validatePermalink) Then
                        isValid = True
                    End If
                Case PermalinkType.Properties
                    If Not CCFramework.ContentManagement.PropertyModule.PropertyController.HasPermalink(ValidatePermalink) Then
                        isValid = True
                    End If
            End Select

            Return isValid
        End Function

        Public Shared Function CheckPermalinkValid(ByVal ValidatePermalink As String, ByVal CurrentPermalinkType As PermalinkType, ByVal CurrentItemId As Integer) As Boolean
            Dim isValid As Boolean = False

            Select Case currentPermalinkType
                Case PermalinkType.ContentPage
                    If Not CCFramework.ContentManagement.ContentPageController.HasPermalink(validatePermalink, currentItemId) Then
                        isValid = True
                    End If
                Case PermalinkType.Blog
                    If Not CCFramework.Blogging.BlogController.HasPermalink(validatePermalink, currentItemId) Then
                        isValid = True
                    End If
                Case PermalinkType.Author
                    If Not CCFramework.Blogging.AuthorController.HasPermalink(validatePermalink, currentItemId) Then
                        isValid = True
                    End If
                Case PermalinkType.Properties
                    If Not CCFramework.ContentManagement.PropertyModule.PropertyController.HasPermalink(ValidatePermalink, CurrentItemId) Then
                        isValid = True
                    End If
            End Select

            Return isValid
        End Function

        Public Property PermalinkType() As PermalinkType
            Get
                Return _permalinkType
            End Get
            Set(ByVal value As PermalinkType)
                _permalinkType = value
            End Set
        End Property

        Public Property ItemID() As Integer
            Get
                'Return _itemID
                If Not ViewState("ItemID") Is Nothing Then
                    Return CInt(ViewState("ItemID"))
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Integer)
                '_itemID = value
                If Not ViewState("ItemID") Is Nothing Then
                    ViewState("ItemID") = value
                Else
                    ViewState.Add("ItemID", value)
                End If
            End Set
        End Property

    End Class

    Public Enum PermalinkType
        Undefined = 0
        ContentPage = 1
        Blog = 2
        Author = 3
        Properties = 4
    End Enum

End Namespace