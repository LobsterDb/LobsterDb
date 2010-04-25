Module test
    Public Sub testme()
        Stop
        'Dim str As String = "{'ClassName':'ClassModelReader','MethodName':'GetState','Arguments':{'ClassModelId':'26761d27-392c-4466-ab5d-7849c7c91474','ClassModelSource':'Framework'}}"
        Dim str As String = "{'ClassName':'ClassModelReader','MethodName':'GetListOfChildClassModels','Arguments':{'ClassModelId':'d4fdaff6-357e-4c58-bdc0-be758ec7a625','ClassModelSource':'Application'}}"
        Dim reader As New ClassModelReader(New JsonProxy(Str))
        Stop
        Debug.Print(reader.RouteCall().GetJson())
    End Sub
End Module