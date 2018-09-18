Namespace CCControls.Validators

    Public Class RotatorOrderValidator
        Inherits BaseValidator

        Public Sub New()

        End Sub

        Protected Overrides Function ControlPropertiesValid() As Boolean
            Dim ctrlA As Control = TryCast(FindControl(ControlToValidate), TextBox)

            Return (Not ctrlA Is Nothing)
        End Function

        Protected Overrides Function EvaluateIsValid() As Boolean
            Dim Order As Integer = Integer.Parse(CType(Me.FindControl(Me.ControlToValidate), TextBox).Text)
            Dim PageID As Integer = Integer.Parse(CType(Me.FindControl(Me.PageControl), DropDownList).SelectedValue)

            Dim RotatorID As Integer

            If Not Integer.TryParse(CType(Me.FindControl(Me.RotatorIDControl), HiddenField).Value, RotatorID) Then
                RotatorID = 0
            End If

            'Me.ErrorMessage = Order.ToString & " | " & PageID.ToString & " | " & RotatorID.ToString

            Return CCFramework.ContentManagement.ImageRotatorController.GetPageOrderAvailable(PageID, Order, RotatorID)

        End Function

        Public Property PageControl() As String
            Get
                If Not ViewState("PageControl") Is Nothing Then
                    Return CStr(ViewState("PageControl"))
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                If Not ViewState("PageControl") Is Nothing Then
                    ViewState("PageControl") = value
                Else
                    ViewState.Add("PageControl", value)
                End If
            End Set
        End Property

        Public Property RotatorIDControl() As String
            Get
                If Not ViewState("RotatorIDControl") Is Nothing Then
                    Return CStr(ViewState("RotatorIDControl"))
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                If Not ViewState("RotatorIDControl") Is Nothing Then
                    ViewState("RotatorIDControl") = value
                Else
                    ViewState.Add("RotatorIDControl", value)
                End If
            End Set
        End Property

    End Class

End Namespace