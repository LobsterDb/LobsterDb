'Lobster Framework 1.0 CTP
'Copyright(c) 2010, LobsterDb
'Licensed under the AGPL Version 3 license
'http://www.lobsterdb.com/license

Namespace ClassModelReader

    Public Class ClassModel

        Private _classModelId As String
        Private _objectSource As ObjectSource
        Private _databaseConnection As LobsterConnection
        Private _stateGraph As JsonProxy
        Private _childClassModels As Collection

        Public Shared Function GetClassModel(ByVal classModelId As String, _
                                             ByVal session As Session) As ClassModel
            Dim stateGraph As String
            Dim childClassModels As New Collection()
            Select Case classModelId
                Case "26761d27-392c-4466-ab5d-7849c7c91474"
                    stateGraph = My.Resources.MyResources.ClassModelClassModelState
                    Dim fieldModelClassModel As ClassModel = _
                        GetClassModel("c26f63b3-df37-49d6-a238-aa04009dabb1", session)
                    childClassModels.Add(fieldModelClassModel)
                    Return New ClassModel(stateGraph, childClassModels, session)
                Case "c26f63b3-df37-49d6-a238-aa04009dabb1"
                    stateGraph = My.Resources.MyResources.FieldModelClassModelState
                    Return New ClassModel(stateGraph, childClassModels, session)
                Case Else
                    Return GetApplicationClassModel(classModelId, session)
            End Select

        End Function

        Public Shared Function GetStateGraph(ByVal classModelId As String, _
                                             ByVal session As Session) As String
            Dim childClassModels As New Collection()
            Select Case classModelId
                Case "26761d27-392c-4466-ab5d-7849c7c91474"
                    Return My.Resources.MyResources.ClassModelClassModelState
                Case "c26f63b3-df37-49d6-a238-aa04009dabb1"
                    Return My.Resources.MyResources.FieldModelClassModelState
                Case Else
                    Dim classModelClassModel As ClassModel = GetClassModel( _
                        "26761d27-392c-4466-ab5d-7849c7c91474", session)
                    Return New StateGraph().GetState(classModelClassModel, _
                                                           classModelId, _
                                                           session)
            End Select

        End Function

        Public Shared Function GetApplicationClassModel(ByVal classModelId As String, _
                                                        ByVal session As Session) _
                                                        As ClassModel

            Dim classModelClassModel As ClassModel = GetClassModel( _
                "26761d27-392c-4466-ab5d-7849c7c91474", session)

            Dim stateGraph As String = New StateGraph().GetState(classModelClassModel, _
                                                   classModelId, _
                                                   session)

            Dim reader As LobsterReader = session.DatabaseConnection.GetReader( _
                "SELECT * FROM ClassModels where ParentClassModelId = '" & _
                classModelId & "'")

            Dim childClassModels As Collection = New Collection

            Do While reader.Read()
                childClassModels.Add(ClassModel.GetClassModel( _
                                  reader.GetGuid("ClassModelId").ToString, _
                                  New Session(1)))
            Loop

            reader.Close()

            Return New ClassModel(stateGraph, childClassModels, session)

        End Function

        Sub New(ByVal stateString As String, ByVal childClassModels As Collection, _
                ByVal session As Session)

            _stateGraph = New JsonProxy(stateString)
            _childClassModels = childClassModels

        End Sub

        Public Function GetChildClassModels() As Collection
            Return _childClassModels
        End Function

        Public Function GetListOfChildClassModels() As String
            Dim json As String = "["
            Dim needsComma As Boolean
            For Each componentModel As ClassModelReader.ClassModel _
                In _childClassModels
                If needsComma Then
                    json &= ","
                Else
                    needsComma = True
                End If
                json &= "'" & componentModel.ClassModelId & "'"
            Next
            Return json & "]"
        End Function

        Public Function GetFieldModels() As FieldModels

            Dim fieldModels As New FieldModels(_stateGraph)
            GetFieldModels = fieldModels

        End Function

        ReadOnly Property ClassModelId() As String
            Get
                ClassModelId = _stateGraph.GetString("ClassModelId")
            End Get
        End Property

        ReadOnly Property ObjectSource() As ObjectSource
            Get
                Return _objectSource
            End Get
        End Property

        ReadOnly Property KeyName() As String
            Get
                KeyName = Name & "Id"
            End Get
        End Property

        ReadOnly Property KeyType() As lafDataType
            Get
                If Me.ClassModelId = DomainModel.ClassModel.ClassModelId Or Me.ClassModelId = "c26f63b3-df37-49d6-a238-aa04009dabb1" Then
                    Return lafDataType.lafGuid
                Else
                    Return lafDataType.lafInteger
                End If
            End Get
        End Property

        Property Name() As String
            Get
                Name = _stateGraph.GetString("Name")
            End Get
            Set(ByVal value As String)
                _stateGraph.SetString("Name", value)
            End Set
        End Property

        Property SetNameOverload() As String
            Get
                SetNameOverload = _stateGraph.GetString("SetNameOverload")
            End Get
            Set(ByVal value As String)
                _stateGraph.SetString("SetNameOverload", value)
            End Set
        End Property

        ReadOnly Property SetName() As String
            Get
                If SetNameOverload = "" Then
                    SetName = Name & "s"
                Else
                    SetName = SetNameOverload
                End If
            End Get
        End Property

        ReadOnly Property TableName() As String
            Get
                If SetName = "FieldModels" Then
                    TableName = "ClassModels_FieldModels"
                Else
                    TableName = SetName
                End If
            End Get
        End Property

    End Class

End Namespace
