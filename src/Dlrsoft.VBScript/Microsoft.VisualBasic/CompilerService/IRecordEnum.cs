using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.VisualBasic.CompilerService
{
	internal interface IRecordEnum
	{
		bool Callback(FieldInfo FieldInfo, ref object Value);
	}
}
