<%@ LANGUAGE = VBScript %>
<h1>test nu</h1>
<%

k = Request.QueryString("key")

response.write "key : " & typename(k) & " = " & k & vbCrLf

Session(k) = "JOHAN"

response.write " => Session(key) : " & typename(Session("key")) & " = " & Session("key") & vbCrLf

%>
