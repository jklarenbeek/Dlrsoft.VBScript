<html>
	<head>
	</head>
	<body>
	<h1>ServerVariables</h1>
	<hr />
	<%
	url = Request.ServerVariables("HTTP_URL")
x = instr(1, url, "url=", 1)
if x > 0 then
	url = Mid(url, x + 4)
end if
	%>
		<pre>Request.ServerVariables(&quot;HTTP_URL&quot;)=<%=url%></pre>
		<hr />
		<ul>
		<%
		for each x in Request.ServerVariables
		  y = Request.ServerVariables(x)
		  response.write("<li><b>" & x & "</b>=<span>" & y & "</span></li>")
		next
		%>
		</ul>
	</body>
</html>