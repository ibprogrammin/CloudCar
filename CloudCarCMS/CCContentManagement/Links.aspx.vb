Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCContentManagement

    Partial Public Class Links
        Inherits CloudCarContentPage

        Public Sub New()
            MyBase.New()
            Permalink = Settings.LinkPage
        End Sub

    End Class

End NameSpace