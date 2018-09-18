Namespace CCControls.ContentManagement

    Partial Public Class TestimonialControl
        Inherits UserControl

        Protected Overrides Sub Onload(ByVal Args As EventArgs)
            If Not Page.IsPostBack Then
                Dim CurrentTestimonialController As New CCFramework.ContentManagement.TestimonialController

                If Count = 0 Then
                    TestimonialRepeater.DataSource = CurrentTestimonialController.GetElements.OrderByDescending(Function(t) t.ApprovedOn)
                    TestimonialRepeater.DataBind()
                Else
                    Dim SelectRandomRecord As New Random()
                    Dim RandomRecordIndex As Integer

                    RandomRecordIndex = SelectRandomRecord.Next(0, CurrentTestimonialController.GetElements().Count())

                    TestimonialRepeater.DataSource = CurrentTestimonialController.GetElements.OrderByDescending(Function(t) t.ApprovedOn).Skip(RandomRecordIndex).Take(Count)
                    TestimonialRepeater.DataBind()
                End If

            End If
        End Sub

        Public Property Count() As Integer
            Get
                If Not ViewState("Count") Is Nothing Then
                    Return CType(ViewState("Count"), Integer)
                Else
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                If Not ViewState("Count") Is Nothing Then
                    ViewState("Count") = Value
                Else
                    ViewState.Add("Count", Value)
                End If
            End Set
        End Property

    End Class

End Namespace