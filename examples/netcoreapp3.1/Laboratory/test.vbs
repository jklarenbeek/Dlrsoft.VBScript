dim a, b, c
'a = -2147483648  ' compile overflow error...
b = -1-2147483647 ' b = -2147483648
c = -2-2147483647 ' c = 2147483647   NO OVERFLOW ERROR...!!!?!???
Response.WriteLine(CStr(b))
Response.WriteLine(CStr(c))
