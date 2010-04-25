﻿'LobsterDb
'Copyright(c) 2010, LobsterDb
'Licensed under the AGPL Version 3 license
'http://www.lobsterdb.com/license

Public Class CompositeSetBroker
    Implements IServicesProxy

    Private _request As JsonProxy

    Sub New(ByVal request As JsonProxy)
        _request = request
    End Sub

    Public Function RouteCall() As JsonString Implements IServicesProxy.RouteCall

        Dim methodName As String = _request.GetString("MethodName")
        Dim arguments As JsonProxy = _request.GetDataObject("Arguments")

        Dim compositeModelId As String = arguments.GetString("CompositeModelId")

        'Dim classType As ObjectSource = CType(arguments.GetString("ObjectSource"),  _
        'ObjectSource)

        Dim session As New Session(1)

        Dim compositeModel As Lobster.Framework.ClassModelReader.ClassModel = _
            Lobster.Framework.ClassModelReader.ClassModel.GetClassModel( _
                compositeModelId, session)

        Dim returnValue As String
        Dim compositeSet As New CompositeSet()

        Select Case methodName
            Case Is = "GetObjectPickerData"
                returnValue = compositeSet.GetObjectPickerData(compositeModel, _
                                                               session)
            Case Else
                Throw New Exception("CompositeSetBroker.RouteCall")
        End Select

        Dim returnObject As New JsonString()

        returnObject.AddJson("ReturnValue", returnValue)

        Return returnObject

    End Function

End Class
