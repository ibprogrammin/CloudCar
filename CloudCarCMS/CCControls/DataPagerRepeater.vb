Namespace CCControls

    <ToolboxData("<{0}:DataPagerRepeater runat=""server"" PersistantDataSource=""True""></{0}:DataPagerRepeater>")> _
    Public Class DataPagerRepeater
        Inherits Repeater
        Implements IPageableItemContainer, INamingContainer

        Public ReadOnly Property MaximumRows() As Integer Implements IPageableItemContainer.MaximumRows
            Get
                If Not ViewState("_maximumRows") Is Nothing Then
                    Return CInt(ViewState("_maximumRows"))
                Else
                    Return -1
                End If
            End Get
        End Property

        Public ReadOnly Property StartRowIndex() As Integer Implements IPageableItemContainer.StartRowIndex
            Get
                If Not ViewState("_startRowIndex") Is Nothing Then
                    Return CInt(ViewState("_startRowIndex"))
                Else
                    Return -1
                End If
            End Get
        End Property

        Public Property TotalRows() As Integer
            Get
                If Not ViewState("_totalRows") Is Nothing Then
                    Return CInt(ViewState("_totalRows"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("_totalRows") = value
            End Set
        End Property

        Public Property PagingInDataSource() As Boolean
            Get
                If Not ViewState("PageingInDataSource") Is Nothing Then
                    Return CBool(ViewState("PageingInDataSource"))
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState("PageingInDataSource") = value
            End Set
        End Property

        Public Property PersistentDataSource() As Boolean
            Get
                If Not ViewState("PersistentDataSource") Is Nothing Then
                    Return CBool(ViewState("PersistentDataSource"))
                Else
                    Return True
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState("PersistentDataSource") = value
            End Set
        End Property

        Public ReadOnly Property NeedsDataSource() As Boolean
            Get
                If PagingInDataSource Then
                    Return True
                ElseIf IsBoundUsingDataSourceID = False And Not Page.IsPostBack Then
                    Return True
                ElseIf IsBoundUsingDataSourceID = False And PersistentDataSource = False And Page.IsPostBack Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Protected Overrides Sub LoadViewState(ByVal SavedState As Object)
            MyBase.LoadViewState(SavedState)

            'If Page.IsPostBack Then
            '    If PersistentDataSource And Not ViewState("DataSource") Is Nothing Then
            '        Me.DataSource = ViewState("DataSource")
            '        Me.DataBind()
            '    End If
            'End If
        End Sub

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Page.IsPostBack Then
                If NeedsDataSource Then ' And Not FetchingData Then
                    If PagingInDataSource Then
                        SetPageProperties(StartRowIndex, MaximumRows, True)
                    End If

                    RaiseEvent FetchingData(Me, Nothing)
                End If

                If Not IsBoundUsingDataSourceID And PersistentDataSource And Not ViewState("DataSource") Is Nothing Then
                    Me.DataSource = ViewState("DataSource")
                    Me.DataBind()
                End If

                If IsBoundUsingDataSourceID Then
                    Me.DataBind()
                End If
            End If

            MyBase.OnLoad(e)
        End Sub

        Public Sub SetPageProperties(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal databind As Boolean) Implements System.Web.UI.WebControls.IPageableItemContainer.SetPageProperties
            ViewState("_startRowIndex") = startRowIndex
            ViewState("_maximumRows") = maximumRows

            If TotalRows > -1 Then
                'If Not TotalRowCountAvailable Is Nothing Then
                RaiseEvent TotalRowCountAvailable(Me, New PageEventArgs(CInt(ViewState("_startRowIndex")), CInt(ViewState("_maximumRows")), TotalRows))
                'End If
            End If
        End Sub

        Protected Overrides Sub OnDataPropertyChanged()
            If Not MaximumRows = -1 Or IsBoundUsingDataSourceID Then
                Me.RequiresDataBinding = True
            Else
                MyBase.OnDataPropertyChanged()
            End If
        End Sub

        Protected Overrides Sub RenderChildren(ByVal writer As System.Web.UI.HtmlTextWriter)
            If Not MaximumRows = -1 And Not PagingInDataSource Then
                For Each item As RepeaterItem In Me.Items
                    If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then
                        item.Visible = False

                        If item.ItemIndex >= CInt(ViewState("_startRowIndex")) And item.ItemIndex < (CInt(ViewState("_startRowIndex")) + CInt(ViewState("_maximumRows"))) Then
                            item.Visible = True
                        End If
                    Else
                        item.Visible = True
                    End If
                Next
            End If

            MyBase.RenderChildren(writer)
        End Sub

        Protected Overrides Function GetData() As IEnumerable
            Dim dataObjects As IEnumerable = MyBase.GetData

            If dataObjects Is Nothing AndAlso Not DataSource Is Nothing Then
                If TypeOf DataSource Is IEnumerable Then
                    dataObjects = CType(DataSource, IEnumerable)
                Else
                    dataObjects = CType(DataSource, ComponentModel.IListSource).GetList
                End If
            End If

            If Not PagingInDataSource AndAlso Not MaximumRows = -1 And Not dataObjects Is Nothing Then
                Dim i As Integer = -1

                If Not dataObjects Is Nothing Then
                    i = 0
                    For Each o As Object In dataObjects
                        i += 1
                    Next
                End If

                ViewState("_totalRows") = i

                If Not IsBoundUsingDataSourceID And PersistentDataSource Then
                    ViewState("DataSource") = DataSource
                End If

                SetPageProperties(StartRowIndex, MaximumRows, True)
            End If

            If PagingInDataSource And Not Page.IsPostBack Then
                SetPageProperties(StartRowIndex, MaximumRows, True)
            End If

            Return dataObjects
        End Function


        'Public Overrides Sub DataBind()
        '    MyBase.DataBind()

        '    If Not MaximumRows = -1 Then
        '        Dim i As Integer = 0
        '        For Each o As Object In GetData()
        '            i += 1
        '        Next

        '        ViewState("_totalRows") = i

        '        If PersistentDataSource Then
        '            ViewState("DataSource") = Me.DataSource
        '        End If

        '        SetPageProperties(StartRowIndex, MaximumRows, True)
        '    End If
        'End Sub

        'Protected Overrides Function GetData() As System.Collections.IEnumerable
        '    Return MyBase.GetData()
        'End Function

        Public Event TotalRowCountAvailable(ByVal sender As Object, ByVal e As PageEventArgs) Implements System.Web.UI.WebControls.IPageableItemContainer.TotalRowCountAvailable

        Public Event FetchingData As EventHandler(Of PageEventArgs) '(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PageEventArgs)

    End Class

End Namespace


'public void SetPageProperties(int startRowIndex, int maximumRows, bool databind)
'{
'    ViewState["_startRowIndex"] = startRowIndex;
'    ViewState["_maximumRows"] = maximumRows;

'    if (TotalRows > -1)
'    {
'        if (TotalRowCountAvailable != null)
'        {
'            TotalRowCountAvailable(this, 
'               new PageEventArgs((int)ViewState["_startRowIndex"], 
'               (int)ViewState["_maximumRows"], TotalRows));
'        }
'    }
'}
