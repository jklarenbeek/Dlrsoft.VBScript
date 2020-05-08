using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualBasic.CompilerService
{
	internal class Symbols
	{
		internal static TypeCode GetTypeCode(Type Type)
		{
			return Type.GetTypeCode(Type);
		}

		internal static bool IsEnum(Type Type)
		{
			return Type.IsEnum;
		}

		internal static bool IsNumericType(Type Type)
		{
			return IsNumericType(GetTypeCode(Type));
		}
		internal static bool IsNumericType(TypeCode TypeCode)
		{
			switch (TypeCode)
			{
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:
					return false;
			}
		}
	}
}
