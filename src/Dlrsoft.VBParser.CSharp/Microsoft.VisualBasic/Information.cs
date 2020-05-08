using Microsoft.VisualBasic.CompilerService;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic
{
	public class Information
	{
		public static bool IsArray(object VarName)
		{
			if (VarName == null)
			{
				return false;
			}
			return VarName is Array;
		}

		public static bool IsDate(object Expression)
		{
			if (Expression == null)
			{
				return false;
			}
			if (Expression is DateTime)
			{
				return true;
			}
			string text = Expression as string;
			DateTime Result = default(DateTime);
			if (text != null)
			{
				return Conversions.TryParseDate(text, ref Result);
			}
			return false;
		}

		public static bool IsNumeric(object Expression)
		{
			IConvertible convertible = Expression as IConvertible;
			if (convertible == null)
			{
				char[] array = Expression as char[];
				if (array == null)
				{
					return false;
				}
				Expression = new string(array);
			}
			TypeCode typeCode = convertible.GetTypeCode();
			if (typeCode == TypeCode.String || typeCode == TypeCode.Char)
			{
				string value = convertible.ToString(null);
				try
				{
					long i64Value = default(long);
					if (Utils.IsHexOrOctValue(value, ref i64Value))
					{
						return true;
					}
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					return false;
				}
				double Result = default(double);
				return DoubleTypeTryParse(value, ref Result);
			}
			return IsOldNumericTypeCode(typeCode);
		}
		private static bool DoubleTypeTryParse(string Value, ref double Result)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			NumberFormatInfo numberFormat = cultureInfo.NumberFormat;
			NumberFormatInfo normalizedNumberFormat = DecimalTypeGetNormalizedNumberFormat(numberFormat);
			Value = Utils.ToHalfwidthNumbers(Value, cultureInfo);
			if (numberFormat == normalizedNumberFormat)
			{
				return double.TryParse(Value, NumberStyles.Any, normalizedNumberFormat, out Result);
			}
			try
			{
				Result = double.Parse(Value, NumberStyles.Any, normalizedNumberFormat);
				return true;
			}
			catch (FormatException)
			{
				try
				{
					return double.TryParse(Value, NumberStyles.Any, numberFormat, out Result);
				}
				catch (ArgumentException)
				{
					return false;
				}
			}
			catch (StackOverflowException ex3)
			{
				throw ex3;
			}
			catch (OutOfMemoryException ex4)
			{
				throw ex4;
			}
			catch (ThreadAbortException ex5)
			{
				throw ex5;
			}
			catch (Exception)
			{
				return false;
			}
		}
		private static NumberFormatInfo DecimalTypeGetNormalizedNumberFormat(NumberFormatInfo InNumberFormat)
		{
			NumberFormatInfo numberFormatInfo = InNumberFormat;
			if (numberFormatInfo.CurrencyDecimalSeparator != null && numberFormatInfo.NumberDecimalSeparator != null && numberFormatInfo.CurrencyGroupSeparator != null && numberFormatInfo.NumberGroupSeparator != null && numberFormatInfo.CurrencyDecimalSeparator.Length == 1 && numberFormatInfo.NumberDecimalSeparator.Length == 1 && numberFormatInfo.CurrencyGroupSeparator.Length == 1 && numberFormatInfo.NumberGroupSeparator.Length == 1 && numberFormatInfo.CurrencyDecimalSeparator[0] == numberFormatInfo.NumberDecimalSeparator[0] && numberFormatInfo.CurrencyGroupSeparator[0] == numberFormatInfo.NumberGroupSeparator[0] && numberFormatInfo.CurrencyDecimalDigits == numberFormatInfo.NumberDecimalDigits)
			{
				return InNumberFormat;
			}
			numberFormatInfo = null;
			NumberFormatInfo numberFormatInfo2 = InNumberFormat;
			checked
			{
				if (numberFormatInfo2.CurrencyDecimalSeparator != null && numberFormatInfo2.NumberDecimalSeparator != null && numberFormatInfo2.CurrencyDecimalSeparator.Length == numberFormatInfo2.NumberDecimalSeparator.Length && numberFormatInfo2.CurrencyGroupSeparator != null && numberFormatInfo2.NumberGroupSeparator != null && numberFormatInfo2.CurrencyGroupSeparator.Length == numberFormatInfo2.NumberGroupSeparator.Length)
				{
					int num = numberFormatInfo2.CurrencyDecimalSeparator.Length - 1;
					int num2 = 0;
					while (true)
					{
						if (num2 <= num)
						{
							if (numberFormatInfo2.CurrencyDecimalSeparator[num2] != numberFormatInfo2.NumberDecimalSeparator[num2])
							{
								break;
							}
							num2++;
							continue;
						}
						int num3 = numberFormatInfo2.CurrencyGroupSeparator.Length - 1;
						num2 = 0;
						while (true)
						{
							if (num2 <= num3)
							{
								if (numberFormatInfo2.CurrencyGroupSeparator[num2] != numberFormatInfo2.NumberGroupSeparator[num2])
								{
									break;
								}
								num2++;
								continue;
							}
							return InNumberFormat;
						}
						break;
					}
				}
				else
				{
					numberFormatInfo2 = null;
				}
				NumberFormatInfo obj = (NumberFormatInfo)InNumberFormat.Clone();
				obj.CurrencyDecimalSeparator = obj.NumberDecimalSeparator;
				obj.CurrencyGroupSeparator = obj.NumberGroupSeparator;
				obj.CurrencyDecimalDigits = obj.NumberDecimalDigits;
				return obj;
			}
		}
		internal static bool IsOldNumericTypeCode(TypeCode TypCode)
		{
			switch (TypCode)
			{
				case TypeCode.Boolean:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:
					return false;
			}
		}

		/// <summary>Returns the lowest available subscript for the indicated dimension of an array.</summary>
		/// <returns>Integer. The lowest value the subscript for the specified dimension can contain. LBound always returns 0 as long as <paramref name="Array" /> has been initialized, even if it has no elements, for example if it is a zero-length string. If <paramref name="Array" /> is Nothing, LBound throws an <see cref="T:System.ArgumentNullException" />.</returns>
		/// <param name="Array">Required. Array of any data type. The array in which you want to find the lowest possible subscript of a dimension.</param>
		/// <param name="Rank">Optional. Integer. The dimension for which the lowest possible subscript is to be returned. Use 1 for the first dimension, 2 for the second, and so on. If <paramref name="Rank" /> is omitted, 1 is assumed.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Array" /> is Nothing.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="Rank" /> less than 1, or <paramref name="Rank" /> is greater than the rank of <paramref name="Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static int LBound(Array Array, int Rank = 1)
		{
			if (Array == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", "Array"));
			}
			if (Rank < 1 || Rank > Array.Rank)
			{
				throw new RankException(Utils.GetResourceString("Argument_InvalidRank1", "Rank"));
			}
			return Array.GetLowerBound(checked(Rank - 1));
		}

		/// <summary>Returns the highest available subscript for the indicated dimension of an array.</summary>
		/// <returns>Integer. The highest value the subscript for the specified dimension can contain. If <paramref name="Array" /> has only one element, UBound returns 0. If <paramref name="Array" /> has no elements, for example if it is a zero-length string, UBound returns -1. </returns>
		/// <param name="Array">Required. Array of any data type. The array in which you want to find the highest possible subscript of a dimension.</param>
		/// <param name="Rank">Optional. Integer. The dimension for which the highest possible subscript is to be returned. Use 1 for the first dimension, 2 for the second, and so on. If <paramref name="Rank" /> is omitted, 1 is assumed.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Array" /> is Nothing.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="Rank" /> is less than 1, or <paramref name="Rank" /> is greater than the rank of <paramref name="Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static int UBound(Array Array, int Rank = 1)
		{
			if (Array == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", "Array"));
			}
			if (Rank < 1 || Rank > Array.Rank)
			{
				throw new RankException(Utils.GetResourceString("Argument_InvalidRank1", "Rank"));
			}
			return Array.GetUpperBound(checked(Rank - 1));
		}

		public static int RGB(int Red, int Green, int Blue)
		{
			if ((Red & int.MinValue) != 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Red"));
			}
			if ((Green & int.MinValue) != 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Green"));
			}
			if ((Blue & int.MinValue) != 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Blue"));
			}
			if (Red > 255)
			{
				Red = 255;
			}
			if (Green > 255)
			{
				Green = 255;
			}
			if (Blue > 255)
			{
				Blue = 255;
			}
			return checked(Blue * 65536 + Green * 256 + Red);
		}

		public static string TypeName(object VarName)
		{
			if (VarName == null)
			{
				return "Nothing";
			}
			Type type = VarName.GetType();
			bool flag = default(bool);
			if (type.IsArray)
			{
				flag = true;
				type = type.GetElementType();
			}
			string text;
			if (type.IsEnum)
			{
				text = type.Name;
				goto IL_011e;
			}
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.DBNull:
					break;
				case TypeCode.Int16:
					goto IL_009c;
				case TypeCode.Int32:
					goto IL_00a7;
				case TypeCode.Single:
					goto IL_00b2;
				case TypeCode.Double:
					goto IL_00ba;
				case TypeCode.DateTime:
					goto IL_00c2;
				case TypeCode.String:
					goto IL_00ca;
				case TypeCode.Boolean:
					goto IL_00d2;
				case TypeCode.Decimal:
					goto IL_00da;
				case TypeCode.Byte:
					goto IL_00e2;
				case TypeCode.Char:
					goto IL_00ea;
				case TypeCode.Int64:
					goto IL_00f2;
				default:
					goto IL_00fa;
			}
			text = "DBNull";
			goto IL_0138;
		IL_00e2:
			text = "Byte";
			goto IL_0138;
		IL_00fa:
			text = type.Name;
			if (type.IsCOMObject && string.CompareOrdinal(text, "__ComObject") == 0)
			{
				throw new NotSupportedException("COM objects are not supported.");
			}
			goto IL_011e;
		IL_00ea:
			text = "Char";
			goto IL_0138;
		IL_00d2:
			text = "Boolean";
			goto IL_0138;
		IL_011e:
			int num = text.IndexOf('+');
			if (num >= 0)
			{
				text = text.Substring(checked(num + 1));
			}
			goto IL_0138;
		IL_0138:
			if (flag)
			{
				Array array = (Array)VarName;
				text = ((array.Rank != 1) ? (text + "[" + new string(',', checked(array.Rank - 1)) + "]") : (text + "[]"));
				text = OldVBFriendlyNameOfTypeName(text);
			}
			return text;
		IL_00ca:
			text = "String";
			goto IL_0138;
		IL_00c2:
			text = "Date";
			goto IL_0138;
		IL_00da:
			text = "Decimal";
			goto IL_0138;
		IL_00ba:
			text = "Double";
			goto IL_0138;
		IL_00b2:
			text = "Single";
			goto IL_0138;
		IL_00f2:
			text = "Long";
			goto IL_0138;
		IL_00a7:
			text = "Integer";
			goto IL_0138;
		IL_009c:
			text = "Short";
			goto IL_0138;
		}

		internal static string OldVBFriendlyNameOfTypeName(string typename)
		{
			string text = null;
			checked
			{
				int num = typename.Length - 1;
				if (typename[num] == ']')
				{
					int num2 = typename.IndexOf('[');
					text = ((num2 + 1 != num) ? typename.Substring(num2, num - num2 + 1).Replace('[', '(').Replace(']', ')') : "()");
					typename = typename.Substring(0, num2);
				}
				string text2 = OldVbTypeName(typename);
				if (text2 == null)
				{
					text2 = typename;
				}
				if (text == null)
				{
					return text2;
				}
				return text2 + Utils.AdjustArraySuffix(text);
			}
		}
		internal static string OldVbTypeName(string UrtName)
		{
			UrtName = Strings.Trim(UrtName).ToUpperInvariant();
			if (string.CompareOrdinal(Strings.Left(UrtName, 7), "SYSTEM.") == 0)
			{
				UrtName = Strings.Mid(UrtName, 8);
			}
			switch (UrtName)
			{
				case "OBJECT":
					return "Object";
				case "INT16":
					return "Short";
				case "INT32":
					return "Integer";
				case "SINGLE":
					return "Single";
				case "DOUBLE":
					return "Double";
				case "DATETIME":
					return "Date";
				case "STRING":
					return "String";
				case "BOOLEAN":
					return "Boolean";
				case "DECIMAL":
					return "Decimal";
				case "BYTE":
					return "Byte";
				case "CHAR":
					return "Char";
				case "INT64":
					return "Long";
				default:
					return null;
			}
		}

		internal static VariantType VarTypeFromComType(Type typ)
		{
			if ((object)typ == null)
			{
				return VariantType.Object;
			}
			if (typ.IsArray)
			{
				typ = typ.GetElementType();
				if (typ.IsArray)
				{
					return (VariantType)8201;
				}
				VariantType variantType = VarTypeFromComType(typ);
				if ((variantType & VariantType.Array) != 0)
				{
					return (VariantType)8201;
				}
				return variantType | VariantType.Array;
			}
			if (typ.IsEnum)
			{
				typ = Enum.GetUnderlyingType(typ);
			}
			if ((object)typ == null)
			{
				return VariantType.Empty;
			}
			switch (Type.GetTypeCode(typ))
			{
				case TypeCode.String:
					return VariantType.String;
				case TypeCode.Int32:
					return VariantType.Integer;
				case TypeCode.Int16:
					return VariantType.Short;
				case TypeCode.Int64:
					return VariantType.Long;
				case TypeCode.Single:
					return VariantType.Single;
				case TypeCode.Double:
					return VariantType.Double;
				case TypeCode.DateTime:
					return VariantType.Date;
				case TypeCode.Boolean:
					return VariantType.Boolean;
				case TypeCode.Decimal:
					return VariantType.Decimal;
				case TypeCode.Byte:
					return VariantType.Byte;
				case TypeCode.Char:
					return VariantType.Char;
				case TypeCode.DBNull:
					return VariantType.Null;
				default:
					if ((object)typ == typeof(Missing) || (object)typ == typeof(Exception) || typ.IsSubclassOf(typeof(Exception)))
					{
						return VariantType.Error;
					}
					if (typ.IsValueType)
					{
						return VariantType.UserDefinedType;
					}
					return VariantType.Object;
			}
		}
	}
}
