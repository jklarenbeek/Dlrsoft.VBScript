<%@ LANGUAGE = VBScript %>
<% Option Explicit %>

<!-- *************************
This sample is provided for educational purposes only. It is not intended to be 
used in a production environment, has not been tested in a production environment, 
and Microsoft will not provide technical support for it. 
************************* -->

<% 
  
Session("MyHitTime") = Now
  
Dim cnt
cnt = Session("MyHitCount")
if cnt = Nothing then
  cnt = 1
else
  cnt = cnt + 1
end if

Session("MyHitCount") = cnt 

%>

<HTML>
    <HEAD>
        <TITLE>Session Object</TITLE>
    </HEAD>

    <BODY BGCOLOR="White" TOPMARGIN="10" LEFTMARGIN="10">
        <h1>SessionWrite</h1>
        <pre><%

            Response.Write "Session.SessionID: " & Session.SessionID & vbCrLf
            Response.Write "Session.Contents.Count: " & Session.Contents.Count & vbCrLf
            Response.Write "Session.CodePage: " & Session.CodePage & vbCrLf
            Response.Write "Session.LCID: " & Session.LCID & vbCrLf
            Response.Write "Session.Timeout: " & Session.Timeout & vbCrLf
            Response.Write vbCrLf

            Dim key, val
            if Request.QueryString("key").Count > 0 then
               key = Request.QueryString("key")
               val = Request.QueryString("value")
               Response.Write "Adding: "
               Response.Write key & ":" & TypeName(key)
               Response.Write " = "
               Response.Write val & ":" & TypeName(val)
               Response.Write vbCrLf
               Response.Write vbCrLf
               Session(key) = val
            end if

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

        <hr />
        <form method="get">
        <div>
            <span>Set</span>
            <input name="key" type="text" placeholder="key">
            <input name="value" type="text" placeholder="value">
            <input type="submit" value="Submit">
        </div>
        </form>

        <p><a href="SessionRead.asp">go to sessionread.asp</a></p>

    </BODY>
</HTML>
