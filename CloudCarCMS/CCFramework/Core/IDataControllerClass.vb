Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core

    Public Interface IDataControllerClass
        Function Create() As Object
        Function Update(ByVal ID As Object) As Boolean
        Function Delete(ByVal ID As Object) As Boolean
        Function GetElement(ByVal ID As Object) As Object
        Function GetElements() As Object
    End Interface

    Public MustInherit Class DataControllerClass
        Implements IDataControllerClass
        Implements IDisposable

        Friend db As CommerceDataContext

        Public Sub New()
            db = New CommerceDataContext
        End Sub

        Public Overloads Function Create() As Object Implements IDataControllerClass.Create
            Return Nothing
        End Function

        Public Overridable Function Delete(ByVal ID As Object) As Boolean Implements IDataControllerClass.Delete
            Return False
        End Function

        Public Overridable Function Update(ByVal ID As Object) As Boolean Implements IDataControllerClass.Update
            Return False
        End Function

        Public Overridable Function GetElement(ByVal ID As Object) As Object Implements IDataControllerClass.GetElement
            Return Nothing
        End Function

        Public Overridable Function GetElements() As Object Implements IDataControllerClass.GetElements
            Return Nothing
        End Function

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                    db.Dispose()
                End If

                ' TODO: free your own state (unmanaged objects).
                ' TODO: set large fields to null.

                Me.disposedValue = True
            End If
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace