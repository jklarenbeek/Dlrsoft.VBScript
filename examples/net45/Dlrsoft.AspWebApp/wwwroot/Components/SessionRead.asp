<%@ LANGUAGE = VBScript %>
<% Option Explicit %>

<!*************************
This sample is provided for educational purposes only. It is not intended to be 
used in a production environment, has not been tested in a production environment, 
and Microsoft will not provide technical support for it. 
*************************>

<HTML>
    <HEAD>
        <TITLE>Session Read Only Object</TITLE>
    </HEAD>

    <BODY BGCOLOR="White" TOPMARGIN="10" LEFTMARGIN="10">
        <h1>Read Session Object</h1>
        <pre><%
            Response.Write "Session.SessionID: " & Session.SessionID & vbCrLf
            Response.Write "Session.Contents.Count: " & Session.Contents.Count & vbCrLf
            Response.Write "Session.CodePage: " & Session.CodePage & vbCrLf
            Response.Write "Session.LCID: " & Session.LCID & vbCrLf
            Response.Write "Session.Timeout: " & Session.Timeout & vbCrLf
            Response.Write vbCrLf

            Dim name, idx
            'Use a For Each ... Next to loop through the entire collection
            For Each name in Session.Contents
              'Is this session variable an array?
              If IsArray(Session(name)) then
                'If it is an array, loop through each element one at a time
                For idx = LBound(Session(name)) to UBound(Session(name))
                  Response.Write "Session(" & name & ")(" & idx & ") = " & Session(name)(idx)
                  Response.Write vbCrLf
                Next
              Else
                'We aren't dealing with an array, so just display the variable
                Response.Write "Session(" & name & ") = " & Session.Contents(name)
                Response.Write vbCrLf
              End If
            Next
        %></pre>
        <p><a href="SessionWrite.asp">go to sessionwrite.asp</a></p>
    </BODY>
</HTML>
