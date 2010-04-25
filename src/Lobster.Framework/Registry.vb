'Lobster Framework 1.0 CTP
'Copyright(c) 2010, LobsterDb
'Licensed under the AGPL Version 3 license
'http://www.lobsterdb.com/license

Public Class Registry

    Public Function GetClassModelState() As JsonProxy
        Return New JsonProxy(My.Resources.MyResources.ClassModelClassModelState)
    End Function

    Public Sub InsertComposite(ByVal classId As String, ByVal key As String, _
                                    ByVal state As String)



    End Sub

End Class
