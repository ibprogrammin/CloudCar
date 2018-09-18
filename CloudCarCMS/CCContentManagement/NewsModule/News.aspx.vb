Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.ContentManagement

Namespace CCContentManagement.NewsModule

    Partial Public Class News
        Inherits CloudCarContentPage

        Public Sub New()
            MyBase.New()
            Permalink = Settings.NewsPage
        End Sub

    End Class

End Namespace