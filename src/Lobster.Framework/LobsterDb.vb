'Lobster Framework 1.0 CTP
'Copyright(c) 2010, LobsterDb
'Licensed under the AGPL Version 3 license
'http://www.lobsterdb.com/license

Imports Microsoft.SqlServer.Management.Smo

Public Class LobsterDb

    Public Shared Sub CreateDb(ByVal accountId As Integer)

        Dim server As New Server()

        Dim db As New Database(server, "LobsterDb_" & accountId)

        db.Create()

        BuildFrameworkSchema(New Session(accountId))

    End Sub

    Public Shared Sub BuildFrameworkSchema(ByVal session As Session)

        Dim classModel As ClassModelReader.ClassModel

        classModel = ClassModelReader.ClassModel.GetClassModel( _
            DomainModel.ClassModel.ClassModelId, session)
        Tables.Build(classModel, session)

        classModel = ClassModelReader.ClassModel.GetClassModel( _
            "c26f63b3-df37-49d6-a238-aa04009dabb1", session)
        Tables.Build(classModel, session)

    End Sub

    Public Shared Sub BuildRegistryTable(ByVal databaseName As String)

        Dim server As New Server()
        Dim db As Database = server.Databases(databaseName)

        Dim table As New Table(db, "Registry")

        'calling new Column() and then setting properties cause an exception

        Dim column As New Column(table, "RegistryItemId", Microsoft.SqlServer.Management.Smo.DataType.Int)
        column.Identity = True
        column.IdentitySeed = 1
        column.IdentityIncrement = 1
        table.Columns.Add(column)

        column = New Column(table, "CompositeModelId", Microsoft.SqlServer.Management.Smo.DataType.UniqueIdentifier)
        table.Columns.Add(column)

        column = New Column(table, "CompositeId", Microsoft.SqlServer.Management.Smo.DataType.UniqueIdentifier)
        table.Columns.Add(column)

        column = New Column(table, "State", Microsoft.SqlServer.Management.Smo.DataType.NVarCharMax)
        table.Columns.Add(column)

        table.Create()

    End Sub

    Public Shared Sub AddSystemObjectToRegistry(ByVal databaseName As String)

        Dim classModelClassModelId As String = "26761d27-392c-4466-ab5d-7849c7c91474"

        AddSystemObjectToRegistry(databaseName, classModelClassModelId, _
                                  classModelClassModelId, _
                                  My.Resources.MyResources.ClassModelClassModelState)

        Dim fieldModelClassModelId As String = "c26f63b3-df37-49d6-a238-aa04009dabb1"

        AddSystemObjectToRegistry(databaseName, classModelClassModelId, _
                                  fieldModelClassModelId, _
                                  My.Resources.MyResources.FieldModelClassModelState)

    End Sub

    Public Shared Sub AddSystemObjectToRegistry(ByVal databaseName As String, _
                                                 ByVal compositeModelId As String, _
                                                 ByVal compositeId As String, _
                                                 ByVal state As String)

        Dim connection As New LobsterConnection(databaseName)
        Dim command As New SqlCommand

        command.Connection = connection.Connection
        command.CommandText = "INSERT INTO Registry (CompositeModelId, CompositeId, " & _
            "State) VALUES ('" & compositeModelId & "', '" & compositeId & "', '" & _
            Replace(state, "'", "''") & "');"
        command.ExecuteNonQuery()

    End Sub

End Class
