Imports System.Runtime.CompilerServices

Namespace CCFramework.GAConnect

    Public Module GAExtensionMethods

        <Extension()> _
        Public Function ParseEnum(Of T)(Token As String) As T
            Return DirectCast([Enum].Parse(GetType(T), Token), T)
        End Function

    End Module

End Namespace