<%
'--------------------------------------------------------------------
' Microsoft ADO
'
' Copyright (c) 1996-1998 Microsoft Corporation.
'
'
'
' ADO constants include file for VBScript
'
'--------------------------------------------------------------------

' ## ADODB.Command

' Enum CommandTypeEnum
Const adCmdUnspecified = -1
Const adCmdText = 1
Const adCmdTable = 2
Const adCmdStoredProc = 4
Const adCmdUnknown = 8
Const adCmdFile = 256
Const adCmdTableDirect = 512
' End Enum

' ## ADODB.Recordset

' Enum CursorLocationEnum
Const asUseNone = 1
Const adUseServer = 2
Const adUseClient = 3
' End Enum

' Enum CursorTypeEnum
Const adOpenUnspecified = -1
Const adOpenForwardOnly = 0
Const adOpenKeyset = 1
Const adOpenDynamic = 2
Const adOpenStatic = 3
' End Enum

' Enum CursorOptionEnum
Const adHoldRecords = &H00000100
Const adMovePrevious = &H00000200
Const adAddNew = &H01000400
Const adDelete = &H01000800
' End Enum

' Enum LockTypeEnum
Const adLockUnspecified = -1
Const adLockReadOnly = 1
Const adLockPessimistic = 2
Const adLockOptimistic = 3
Const adLockBatchOptimistic = 4
' End Enum

' Enum ExecuteOptionEnum
Const adOptionUnspecified = -1
Const adAsyncExecute = 16
Const adAsyncFetch = 32
Const adAsyncFetchNonBlocking = 64
Const adExecuteNoRecords = 128
Const adExecuteStream = 256
Const adExecuteRecord = 512
' End Enum

' Enum GetRowsOptionEnum
Const adGetRowsRest = -1
' End Enum

' Enum BookmarkEnum
Const adBookmarkCurrent = 0
Const adBookmarkFirst = 1
Const adBookmarkLast = 2
' End Enum

' Enum SearchDirectionEnum
Const adSearchBackward = -1
Const adSearchForward = 1
' End Enum

' ## ADODB.Parameter

' Enum DataTypeEnum
Const adEmpty = 0
Const adSmallInt = 2
Const adInteger = 3
Const adSingle = 4
Const adDouble = 5
Const adCurrency = 6
Const adDate = 7
Const adBSTR = 8
Const adIDispatch = 9
Const adError = 10
Const adBoolean = 11
Const adVariant = 12
Const adIUnknown = 13
Const adDecimal = 14
Const adTinyInt = 16
Const adUnsignedTinyInt = 17
Const adUnsignedSmallInt = 18
Const adUnsignedInt = 19
Const adBigInt = 20
Const adUnsignedBigInt = 21
Const adFileTime = 64
Const adGUID = 72
Const adBinary = 128
Const adChar = 129
Const adWChar = 130
Const adNumeric = 131
Const adUserDefined = 132
Const adDBDate = 133
Const adDBTime = 134
Const adDBTimeStamp = 135
Const adChapter = 136
Const adPropVariant = 138
Const adVarNumeric = 139
Const adVarChar = 200
Const adLongVarChar = 201
Const adVarWChar = 202
Const adLongVarWChar = 203
Const adVarBinary = 204
Const adLongVarBinary = 205
Const AdArray = 0x2000
' End Enum

' Enum RecordTypeEnum
Const adSimpleRecord = 0
Const adCollectionRecord = 1
Const adStructDoc = 2
' End Enum

'===
'=== Begin SQL extension functions ===
'===

Function SqlInt(expression, default) 
	Dim i
	If IsNull(expression) Or IsEmpty(expression) Then
		SqlInt = default
	ElseIf IsNumeric(expression) Then
		If Cstr(CInt(expression)) = expression Then
			SqlInt = Cstr(expression)
		Else
			SqlInt = default
		End If
	Else
		SqlInt = default	
	End If
End Function

Function SqlDbl(expression, default) 
	Dim i, locale, d, expr

	If IsNull(expression) Or IsEmpty(expression) Then
		SqlDbl = default
		Exit Function
	End If		
	
	locale = GetLocale()
	expr = Cstr(expression)
		
	If locale = 1043 Then
		expr = Replace(Cstr(expr), ".", ",")
	End If
	
	If IsNumeric(expr) Then
		d = CDbl(expr)
		If Cstr(d) = expr Then
			SqlDbl = Replace(Cstr(d), ",", ".")
		Else
			SqlDbl = default
		End If
	Else
		SqlDbl = default	
	End If
End Function

'=== End SQL extension functions ===

Function OpenDbConnection(dbServer, dbName, dbUser, dbPassword, timeOut)
 
  On Error Goto errorHandler
   
  Set cn = Server.CreateObject("ADODB.Connection")
  cn.Open ( _
    "Driver=SQL Server" & _
    ";Server="      & dbServer     & _
    ";Network="     & _
    ";Database="    & dbName       & _
    ";User ID="     & dbUser       & _
    ";Password="    & dbPassword   & _
    ";" _
  )
  
  Set OpenDbConnection = cn
  
  Exit Function
  
errorHandler:
  If Err.number = -2147467259 Then
    Response.Write("Error opening connection to database: " & Err.description)
  Else
    Response.Write(Err.number & " " & Err.description)
  End If

  Set OpenDbConnection = Nothing
  
End Function

Function CreateDbCmdText(sql)
  
  Dim cmd : Set cmd = Server.CreateObject("ADODB.command")
  cmd.CommandType = adCmdText
  cmd.CommandText = sql
  cmd.CommandTimeout = 15
 
  CreateDbCmdText = cmd

End Function

Function CreateDbCmdStoredProc(sql)
  
  Dim cmd : Set cmd = Server.CreateObject("ADODB.command")
  cmd.CommandType = adCmdStoredProc
  cmd.CommandText = sql
  cmd.CommandTimeout = 15
 
  CreateDbCmdStoredProc = cmd

End Function

Function OpenDbReadOnlyRecordSet(cmd, connection)

    Dim rds : Set rds = CreateObject("ADODB.RecordSet")
    rds.CursorLocation = adUseServer
    rds.Open cmd, connection, adOpenForwardOnly, adLockReadOnly

    OpenDbRecordSet = rds

End Function

%>
