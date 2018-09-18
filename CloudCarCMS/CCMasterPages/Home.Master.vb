Namespace CCMasterPages

    Partial Public Class HomeMaster
        Inherits MasterPage

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            Response.AppendHeader("Refresh", String.Format("{0};URL={1}", Convert.ToString((Session.Timeout * 60)), Request.FilePath))
            'Response.AppendHeader("Expires", String.Format("Tue, 30 Apr 2013 20:00:00 GMT"))
            'Response.AppendHeader("ETag", String.Format("10c24bc-4ab-457e1c1f"))

            Page.Header.DataBind()
        End Sub

    End Class

End NameSpace