Namespace CCControls

    Partial Public Class HelpButtonControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

        End Sub

        Public Property ImageUrl() As String
            Get
                Return hbcImgIcon.ImageUrl
            End Get
            Set(ByVal value As String)
                hbcImgIcon.ImageUrl = value
            End Set
        End Property

        Public Property Height() As Unit
            Get
                Return hbcImgIcon.Height
            End Get
            Set(ByVal value As Unit)
                hbcImgIcon.Height = value
            End Set
        End Property

        Public Property Width() As Unit
            Get
                Return hbcImgIcon.Width
            End Get
            Set(ByVal value As Unit)
                hbcImgIcon.Width = value
                hbcPopupControlExtender.OffsetY = CInt(value.Value)
            End Set
        End Property

        'Public Property HelpTextId() As Int32
        '    Get
        '        Return ViewState("HelpTextId")
        '    End Get
        '    Set(ByVal value As Int32)
        '        ViewState("HelpTextId") = value
        '        Dim db As New CommerceDataContext
        '        'Dim helpText As String = "Some help text" '= From ht In db.HelpText Where ht.helptextID = value Select ht.Text
        '        'Dim helpText As String = From ht In db.HelpText Where ht.helptextID = value Select ht.Text
        '        Dim helpText As String = (From ht In db.Brands Where ht.ID = value Select ht.Description).SingleOrDefault

        '        hbcLiHelpText.Text = helpText
        '    End Set
        'End Property

        Public Property HelpText() As String
            Get
                Return CStr(ViewState("HelpText"))
            End Get
            Set(ByVal value As String)

                ViewState("HelpText") = value

                hbcLiHelpText.Text = value

            End Set
        End Property

    End Class

End Namespace