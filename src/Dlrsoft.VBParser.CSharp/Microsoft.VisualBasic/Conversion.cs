using Microsoft.VisualBasic.CompilerService;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic
{
    public class Conversion
    {

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static short Fix(short Number)
		{
			return Number;
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static int Fix(int Number)
		{
			return Number;
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static long Fix(long Number)
		{
			return Number;
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static double Fix(double Number)
		{
			if (Number >= 0.0)
			{
				return Math.Floor(Number);
			}
			return 0.0 - Math.Floor(0.0 - Number);
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static float Fix(float Number)
		{
			if (Number >= 0f)
			{
				return (float)Math.Floor(Number);
			}
			return (float)(0.0 - Math.Floor(0f - Number));
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Fix(decimal Number)
		{
			if (Number < 0m)
			{
				return decimal.Negate(decimal.Floor(decimal.Negate(Number)));
			}
			return decimal.Floor(Number);
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static object Fix(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", "Number"));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
						return Number;
					case TypeCode.Single:
						return Fix(convertible.ToSingle(null));
					case TypeCode.Double:
						return Fix(convertible.ToDouble(null));
					case TypeCode.Decimal:
						return Fix(convertible.ToDecimal(null));
					case TypeCode.Boolean:
						return convertible.ToInt32(null);
					case TypeCode.String:
						return Fix(Conversions.ToDouble(convertible.ToString(null)));
				}
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_NotNumericType2", "Number", Number.GetType().FullName));
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static short Int(short Number)
		{
			return Number;
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static int Int(int Number)
		{
			return Number;
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static long Int(long Number)
		{
			return Number;
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static double Int(double Number)
		{
			return Math.Floor(Number);
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static float Int(float Number)
		{
			return (float)Math.Floor(Number);
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Int(decimal Number)
		{
			return decimal.Floor(Number);
		}

		/// <summary>Return the integer portion of a number.</summary>
		/// <returns>Return the integer portion of a number.</returns>
		/// <param name="Number">Required. A number of type Double or any valid numeric expression. If <paramref name="Number" /> contains Nothing, Nothing is returned.</param>
		/// <exception cref="T:System.ArgumentNullException">Number is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">Number is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		public static object Int(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", "Number"));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
						return Number;
					case TypeCode.Single:
						return Int(convertible.ToSingle(null));
					case TypeCode.Double:
						return Int(convertible.ToDouble(null));
					case TypeCode.Decimal:
						return Int(convertible.ToDecimal(null));
					case TypeCode.Boolean:
						return convertible.ToInt32(null);
					case TypeCode.String:
						return Int(Conversions.ToDouble(convertible.ToString(null)));
				}
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_NotNumericType2", "Number", Number.GetType().FullName));
		}

		/// <summary>Returns a string representing the hexadecimal value of a number.</summary>
		/// <returns>Returns a string representing the hexadecimal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string Hex(sbyte Number)
		{
			return Number.ToString("X");
		}

		/// <summary>Returns a string representing the hexadecimal value of a number.</summary>
		/// <returns>Returns a string representing the hexadecimal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Hex(byte Number)
		{
			return Number.ToString("X");
		}

		/// <summary>Returns a string representing the hexadecimal value of a number.</summary>
		/// <returns>Returns a string representing the hexadecimal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Hex(short Number)
		{
			return Number.ToString("X");
		}

		/// <summary>Returns a string representing the hexadecimal value of a number.</summary>
		/// <returns>Returns a string representing the hexadecimal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string Hex(ushort Number)
		{
			return Number.ToString("X");
		}

		/// <summary>Returns a string representing the hexadecimal value of a number.</summary>
		/// <returns>Returns a string representing the hexadecimal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Hex(int Number)
		{
			return Number.ToString("X");
		}

		/// <summary>Returns a string representing the hexadecimal value of a number.</summary>
		/// <returns>Returns a string representing the hexadecimal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string Hex(uint Number)
		{
			return Number.ToString("X");
		}

		/// <summary>Returns a string representing the hexadecimal value of a number.</summary>
		/// <returns>Returns a string representing the hexadecimal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Hex(long Number)
		{
			return Number.ToString("X");
		}

		/// <summary>Returns a string representing the hexadecimal value of a number.</summary>
		/// <returns>Returns a string representing the hexadecimal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string Hex(ulong Number)
		{
			return Number.ToString("X");
		}

		/// <summary>Returns a string representing the hexadecimal value of a number.</summary>
		/// <returns>Returns a string representing the hexadecimal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Hex(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", "Number"));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible != null)
			{
				long num;
				switch (convertible.GetTypeCode())
				{
					case TypeCode.SByte:
						return Hex(convertible.ToSByte(null));
					case TypeCode.Byte:
						return Hex(convertible.ToByte(null));
					case TypeCode.Int16:
						return Hex(convertible.ToInt16(null));
					case TypeCode.UInt16:
						return Hex(convertible.ToUInt16(null));
					case TypeCode.Int32:
						return Hex(convertible.ToInt32(null));
					case TypeCode.UInt32:
						return Hex(convertible.ToUInt32(null));
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						num = convertible.ToInt64(null);
						goto IL_0130;
					case TypeCode.UInt64:
						return Hex(convertible.ToUInt64(null));
					case TypeCode.String:
						{
							try
							{
								num = Conversions.ToLong(convertible.ToString(null));
							}
							catch (OverflowException)
							{
								return Hex(Conversions.ToULong(convertible.ToString(null)));
							}
							goto IL_0130;
						}
					IL_0130:
						if (num == 0L)
						{
							return "0";
						}
						if (num > 0)
						{
							return Hex(num);
						}
						if (num >= int.MinValue)
						{
							return Hex(checked((int)num));
						}
						return Hex(num);
				}
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", "Number", Utils.VBFriendlyName(Number)));
		}

		/// <summary>Returns a string representing the octal value of a number.</summary>
		/// <returns>Returns a string representing the octal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string Oct(sbyte Number)
		{
			return Utils.OctFromLong((long)Number & 0xFFL);
		}

		/// <summary>Returns a string representing the octal value of a number.</summary>
		/// <returns>Returns a string representing the octal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Oct(byte Number)
		{
			return Utils.OctFromULong(Number);
		}

		/// <summary>Returns a string representing the octal value of a number.</summary>
		/// <returns>Returns a string representing the octal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Oct(short Number)
		{
			return Utils.OctFromLong((long)Number & 0xFFFFL);
		}

		/// <summary>Returns a string representing the octal value of a number.</summary>
		/// <returns>Returns a string representing the octal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string Oct(ushort Number)
		{
			return Utils.OctFromULong(Number);
		}

		/// <summary>Returns a string representing the octal value of a number.</summary>
		/// <returns>Returns a string representing the octal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Oct(int Number)
		{
			return Utils.OctFromLong(Number & uint.MaxValue);
		}

		/// <summary>Returns a string representing the octal value of a number.</summary>
		/// <returns>Returns a string representing the octal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string Oct(uint Number)
		{
			return Utils.OctFromULong(Number);
		}

		/// <summary>Returns a string representing the octal value of a number.</summary>
		/// <returns>Returns a string representing the octal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Oct(long Number)
		{
			return Utils.OctFromLong(Number);
		}

		/// <summary>Returns a string representing the octal value of a number.</summary>
		/// <returns>Returns a string representing the octal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string Oct(ulong Number)
		{
			return Utils.OctFromULong(Number);
		}

		/// <summary>Returns a string representing the octal value of a number.</summary>
		/// <returns>Returns a string representing the octal value of a number.</returns>
		/// <param name="Number">Required. Any valid numeric expression or String expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Oct(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", "Number"));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible != null)
			{
				long num;
				switch (convertible.GetTypeCode())
				{
					case TypeCode.SByte:
						return Oct(convertible.ToSByte(null));
					case TypeCode.Byte:
						return Oct(convertible.ToByte(null));
					case TypeCode.Int16:
						return Oct(convertible.ToInt16(null));
					case TypeCode.UInt16:
						return Oct(convertible.ToUInt16(null));
					case TypeCode.Int32:
						return Oct(convertible.ToInt32(null));
					case TypeCode.UInt32:
						return Oct(convertible.ToUInt32(null));
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						num = convertible.ToInt64(null);
						goto IL_0130;
					case TypeCode.UInt64:
						return Oct(convertible.ToUInt64(null));
					case TypeCode.String:
						{
							try
							{
								num = Conversions.ToLong(convertible.ToString(null));
							}
							catch (OverflowException)
							{
								return Oct(Conversions.ToULong(convertible.ToString(null)));
							}
							goto IL_0130;
						}
					IL_0130:
						if (num == 0L)
						{
							return "0";
						}
						if (num > 0)
						{
							return Oct(num);
						}
						if (num >= int.MinValue)
						{
							return Oct(checked((int)num));
						}
						return Oct(num);
				}
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", "Number", Utils.VBFriendlyName(Number)));
		}

		/// <summary>Returns a String representation of a number.</summary>
		/// <returns>Returns a String representation of a number.</returns>
		/// <param name="Number">Required. An Object containing any valid numeric expression.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Number" /> is not specified.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="Number" /> is not a numeric type.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Str(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", "Number"));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible == null)
			{
				throw new InvalidCastException(Utils.GetResourceString("ArgumentNotNumeric1", "Number"));
			}
			string text;
			switch (convertible.GetTypeCode())
			{
				case TypeCode.DBNull:
					return "Null";
				case TypeCode.Boolean:
					if (convertible.ToBoolean(null))
					{
						return "True";
					}
					return "False";
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
					text = Conversions.ToString(Number);
					break;
				case TypeCode.String:
					try
					{
						text = Conversions.ToString(Conversions.ToDouble(convertible.ToString(null)));
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
						goto default;
					}
					break;
				default:
					throw new InvalidCastException(Utils.GetResourceString("ArgumentNotNumeric1", "Number"));
			}
			if (text.Length > 0 && text[0] != '-')
			{
				return " " + Utils.StdFormat(text);
			}
			return Utils.StdFormat(text);
		}
	}
}
