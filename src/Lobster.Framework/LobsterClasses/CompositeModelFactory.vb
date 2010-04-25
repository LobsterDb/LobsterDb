Public Class CompositeModelFactory

    Private Shared _collection As Collection

    Public Shared Function GetClassModelReader(ByVal key As String, _
                                               ByVal session As Session) _
                                               As ClassModelReader.ClassModel

        If _collection Is Nothing Then
            _collection = New Collection
            'Dim compositeMetaModel As CompositeModel = CompositeMetaModelBootstrapper.
            '_collection.Add(GetMetaModel, "26761d27-392c-4466-ab5d-7849c7c91474")
        End If

        If _collection.Contains(key) = False Then
            Dim classModel As ClassModelReader.ClassModel = _
                ClassModelReader.ClassModel.GetClassModel(key, session)
            _collection.Add(classModel, key)
            Return classModel
        Else
            Return CType(_collection.Item(key), ClassModelReader.ClassModel)
        End If

    End Function

End Class
