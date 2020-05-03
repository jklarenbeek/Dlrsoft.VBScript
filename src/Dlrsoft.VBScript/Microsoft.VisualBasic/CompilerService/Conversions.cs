using Microsoft.VisualBasic.CompilerService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	internal class Conversions
	{
		/// <summary>Converts a string to a <see cref="T:System.Boolean" /> value.</summary>
		/// <returns>A Boolean value. Returns False if the string is null; otherwise, True.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static bool ToBoolean(string Value)
		{
			if (Value == null)
			{
				Value = "";
			}
			try
			{
				CultureInfo cultureInfo = Utils.GetCultureInfo();
				if (string.Compare(Value, bool.FalseString, ignoreCase: true, cultureInfo) == 0)
				{
					return false;
				}
				if (string.Compare(Value, bool.TrueString, ignoreCase: true, cultureInfo) == 0)
				{
					return true;
				}
				long i64Value = default(long);
				if (Utils.IsHexOrOctValue(Value, ref i64Value))
				{
					return i64Value != 0;
				}
				return ParseDouble(Value) != 0.0;
			}
			catch (FormatException innerException)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "Boolean"), innerException);
			}
		}

		/// <summary>Converts an object to a <see cref="T:System.Boolean" /> value.</summary>
		/// <returns>A Boolean value. Returns False if the object is null; otherwise, True.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static bool ToBoolean(object Value)
		{
			if (Value == null)
			{
				return false;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
					case TypeCode.Boolean:
						if (Value is bool)
						{
							return (bool)Value;
						}
						return convertible.ToBoolean(null);
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (sbyte)Value != 0;
						}
						return convertible.ToSByte(null) != 0;
					case TypeCode.Byte:
						if (Value is byte)
						{
							return (byte)Value != 0;
						}
						return convertible.ToByte(null) != 0;
					case TypeCode.Int16:
						if (Value is short)
						{
							return (short)Value != 0;
						}
						return convertible.ToInt16(null) != 0;
					case TypeCode.UInt16:
						if (Value is ushort)
						{
							return (ushort)Value != 0;
						}
						return convertible.ToUInt16(null) != 0;
					case TypeCode.Int32:
						if (Value is int)
						{
							return (int)Value != 0;
						}
						return convertible.ToInt32(null) != 0;
					case TypeCode.UInt32:
						if (Value is uint)
						{
							return (uint)Value != 0;
						}
						return convertible.ToUInt32(null) != 0;
					case TypeCode.Int64:
						if (Value is long)
						{
							return (long)Value != 0;
						}
						return convertible.ToInt64(null) != 0;
					case TypeCode.UInt64:
						if (Value is ulong)
						{
							return (ulong)Value != 0;
						}
						return convertible.ToUInt64(null) != 0;
					case TypeCode.Decimal:
						if (Value is decimal)
						{
							return convertible.ToBoolean(null);
						}
						return Convert.ToBoolean(convertible.ToDecimal(null));
					case TypeCode.Single:
						if (Value is float)
						{
							return (float)Value != 0f;
						}
						return convertible.ToSingle(null) != 0f;
					case TypeCode.Double:
						if (Value is double)
						{
							return (double)Value != 0.0;
						}
						return convertible.ToDouble(null) != 0.0;
					case TypeCode.String:
						{
							string text = Value as string;
							if (text != null)
							{
								return ToBoolean(text);
							}
							return ToBoolean(convertible.ToString(null));
						}
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Boolean"));
		}

		/// <summary>Converts a string to a <see cref="T:System.Byte" /> value.</summary>
		/// <returns>The Byte value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static byte ToByte(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				try
				{
					long i64Value = default(long);
					if (Utils.IsHexOrOctValue(Value, ref i64Value))
					{
						return (byte)i64Value;
					}
					return (byte)Math.Round(ParseDouble(Value));
				}
				catch (FormatException innerException)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "Byte"), innerException);
				}
			}
		}

		/// <summary>Converts an object to a <see cref="T:System.Byte" /> value.</summary>
		/// <returns>The Byte value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static byte ToByte(object Value)
		{
			if (Value == null)
			{
				return 0;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
						case TypeCode.Boolean:
							unchecked
							{
								if (Value is bool)
								{
									return (byte)(0 - (((bool)Value) ? 1 : 0));
								}
								return (byte)(0 - (convertible.ToBoolean(null) ? 1 : 0));
							}
						case TypeCode.SByte:
							if (Value is sbyte)
							{
								return (byte)(sbyte)Value;
							}
							return (byte)convertible.ToSByte(null);
						case TypeCode.Byte:
							if (Value is byte)
							{
								return (byte)Value;
							}
							return convertible.ToByte(null);
						case TypeCode.Int16:
							if (Value is short)
							{
								return (byte)(short)Value;
							}
							return (byte)convertible.ToInt16(null);
						case TypeCode.UInt16:
							if (Value is ushort)
							{
								return (byte)(ushort)Value;
							}
							return (byte)convertible.ToUInt16(null);
						case TypeCode.Int32:
							if (Value is int)
							{
								return (byte)(int)Value;
							}
							return (byte)convertible.ToInt32(null);
						case TypeCode.UInt32:
							if (Value is uint)
							{
								return (byte)(uint)Value;
							}
							return (byte)convertible.ToUInt32(null);
						case TypeCode.Int64:
							if (Value is long)
							{
								return (byte)(long)Value;
							}
							return (byte)convertible.ToInt64(null);
						case TypeCode.UInt64:
							if (Value is ulong)
							{
								return (byte)(ulong)Value;
							}
							return (byte)convertible.ToUInt64(null);
						case TypeCode.Decimal:
							if (Value is decimal)
							{
								return convertible.ToByte(null);
							}
							return Convert.ToByte(convertible.ToDecimal(null));
						case TypeCode.Single:
							if (Value is float)
							{
								return (byte)Math.Round((float)Value);
							}
							return (byte)Math.Round(convertible.ToSingle(null));
						case TypeCode.Double:
							if (Value is double)
							{
								return (byte)Math.Round((double)Value);
							}
							return (byte)Math.Round(convertible.ToDouble(null));
						case TypeCode.String:
							{
								string text = Value as string;
								if (text != null)
								{
									return ToByte(text);
								}
								return ToByte(convertible.ToString(null));
							}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Byte"));
			}
		}

		/// <summary>Converts a string to an <see cref="T:System.SByte" /> value.</summary>
		/// <returns>The SByte value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static sbyte ToSByte(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				try
				{
					long i64Value = default(long);
					if (Utils.IsHexOrOctValue(Value, ref i64Value))
					{
						return (sbyte)i64Value;
					}
					return (sbyte)Math.Round(ParseDouble(Value));
				}
				catch (FormatException innerException)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "SByte"), innerException);
				}
			}
		}

		/// <summary>Converts an object to an <see cref="T:System.SByte" /> value.</summary>
		/// <returns>The SByte value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static sbyte ToSByte(object Value)
		{
			if (Value == null)
			{
				return 0;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
						case TypeCode.Boolean:
							unchecked
							{
								if (Value is bool)
								{
									return (sbyte)(0 - (((bool)Value) ? 1 : 0));
								}
								return (sbyte)(0 - (convertible.ToBoolean(null) ? 1 : 0));
							}
						case TypeCode.SByte:
							if (Value is sbyte)
							{
								return (sbyte)Value;
							}
							return convertible.ToSByte(null);
						case TypeCode.Byte:
							if (Value is byte)
							{
								return (sbyte)(byte)Value;
							}
							return (sbyte)convertible.ToByte(null);
						case TypeCode.Int16:
							if (Value is short)
							{
								return (sbyte)(short)Value;
							}
							return (sbyte)convertible.ToInt16(null);
						case TypeCode.UInt16:
							if (Value is ushort)
							{
								return (sbyte)(ushort)Value;
							}
							return (sbyte)convertible.ToUInt16(null);
						case TypeCode.Int32:
							if (Value is int)
							{
								return (sbyte)(int)Value;
							}
							return (sbyte)convertible.ToInt32(null);
						case TypeCode.UInt32:
							if (Value is uint)
							{
								return (sbyte)(uint)Value;
							}
							return (sbyte)convertible.ToUInt32(null);
						case TypeCode.Int64:
							if (Value is long)
							{
								return (sbyte)(long)Value;
							}
							return (sbyte)convertible.ToInt64(null);
						case TypeCode.UInt64:
							if (Value is ulong)
							{
								return (sbyte)(ulong)Value;
							}
							return (sbyte)convertible.ToUInt64(null);
						case TypeCode.Decimal:
							if (Value is decimal)
							{
								return convertible.ToSByte(null);
							}
							return Convert.ToSByte(convertible.ToDecimal(null));
						case TypeCode.Single:
							if (Value is float)
							{
								return (sbyte)Math.Round((float)Value);
							}
							return (sbyte)Math.Round(convertible.ToSingle(null));
						case TypeCode.Double:
							if (Value is double)
							{
								return (sbyte)Math.Round((double)Value);
							}
							return (sbyte)Math.Round(convertible.ToDouble(null));
						case TypeCode.String:
							{
								string text = Value as string;
								if (text != null)
								{
									return ToSByte(text);
								}
								return ToSByte(convertible.ToString(null));
							}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "SByte"));
			}
		}

		/// <summary>Converts a string to a Short value.</summary>
		/// <returns>The Short value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static short ToShort(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				try
				{
					long i64Value = default(long);
					if (Utils.IsHexOrOctValue(Value, ref i64Value))
					{
						return (short)i64Value;
					}
					return (short)Math.Round(ParseDouble(Value));
				}
				catch (FormatException innerException)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "Short"), innerException);
				}
			}
		}

		/// <summary>Converts an object to a Short value.</summary>
		/// <returns>The Short value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static short ToShort(object Value)
		{
			if (Value == null)
			{
				return 0;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
						case TypeCode.Boolean:
							unchecked
							{
								if (Value is bool)
								{
									return (short)(0 - (((bool)Value) ? 1 : 0));
								}
								return (short)(0 - (convertible.ToBoolean(null) ? 1 : 0));
							}
						case TypeCode.SByte:
							if (Value is sbyte)
							{
								return (sbyte)Value;
							}
							return convertible.ToSByte(null);
						case TypeCode.Byte:
							if (Value is byte)
							{
								return (byte)Value;
							}
							return convertible.ToByte(null);
						case TypeCode.Int16:
							if (Value is short)
							{
								return (short)Value;
							}
							return convertible.ToInt16(null);
						case TypeCode.UInt16:
							if (Value is ushort)
							{
								return (short)(ushort)Value;
							}
							return (short)convertible.ToUInt16(null);
						case TypeCode.Int32:
							if (Value is int)
							{
								return (short)(int)Value;
							}
							return (short)convertible.ToInt32(null);
						case TypeCode.UInt32:
							if (Value is uint)
							{
								return (short)(uint)Value;
							}
							return (short)convertible.ToUInt32(null);
						case TypeCode.Int64:
							if (Value is long)
							{
								return (short)(long)Value;
							}
							return (short)convertible.ToInt64(null);
						case TypeCode.UInt64:
							if (Value is ulong)
							{
								return (short)(ulong)Value;
							}
							return (short)convertible.ToUInt64(null);
						case TypeCode.Decimal:
							if (Value is decimal)
							{
								return convertible.ToInt16(null);
							}
							return Convert.ToInt16(convertible.ToDecimal(null));
						case TypeCode.Single:
							if (Value is float)
							{
								return (short)Math.Round((float)Value);
							}
							return (short)Math.Round(convertible.ToSingle(null));
						case TypeCode.Double:
							if (Value is double)
							{
								return (short)Math.Round((double)Value);
							}
							return (short)Math.Round(convertible.ToDouble(null));
						case TypeCode.String:
							{
								string text = Value as string;
								if (text != null)
								{
									return ToShort(text);
								}
								return ToShort(convertible.ToString(null));
							}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Short"));
			}
		}

		/// <summary>Converts a string to a Ushort value.</summary>
		/// <returns>The Ushort value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static ushort ToUShort(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				try
				{
					long i64Value = default(long);
					if (Utils.IsHexOrOctValue(Value, ref i64Value))
					{
						return (ushort)i64Value;
					}
					return (ushort)Math.Round(ParseDouble(Value));
				}
				catch (FormatException innerException)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "UShort"), innerException);
				}
			}
		}

		/// <summary>Converts an object to a Ushort value.</summary>
		/// <returns>The Ushort value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static ushort ToUShort(object Value)
		{
			if (Value == null)
			{
				return 0;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
						case TypeCode.Boolean:
							unchecked
							{
								if (Value is bool)
								{
									return (ushort)(0 - (((bool)Value) ? 1 : 0));
								}
								return (ushort)(0 - (convertible.ToBoolean(null) ? 1 : 0));
							}
						case TypeCode.SByte:
							if (Value is sbyte)
							{
								return (ushort)(sbyte)Value;
							}
							return (ushort)convertible.ToSByte(null);
						case TypeCode.Byte:
							if (Value is byte)
							{
								return (byte)Value;
							}
							return convertible.ToByte(null);
						case TypeCode.Int16:
							if (Value is short)
							{
								return (ushort)(short)Value;
							}
							return (ushort)convertible.ToInt16(null);
						case TypeCode.UInt16:
							if (Value is ushort)
							{
								return (ushort)Value;
							}
							return convertible.ToUInt16(null);
						case TypeCode.Int32:
							if (Value is int)
							{
								return (ushort)(int)Value;
							}
							return (ushort)convertible.ToInt32(null);
						case TypeCode.UInt32:
							if (Value is uint)
							{
								return (ushort)(uint)Value;
							}
							return (ushort)convertible.ToUInt32(null);
						case TypeCode.Int64:
							if (Value is long)
							{
								return (ushort)(long)Value;
							}
							return (ushort)convertible.ToInt64(null);
						case TypeCode.UInt64:
							if (Value is ulong)
							{
								return (ushort)(ulong)Value;
							}
							return (ushort)convertible.ToUInt64(null);
						case TypeCode.Decimal:
							if (Value is decimal)
							{
								return convertible.ToUInt16(null);
							}
							return Convert.ToUInt16(convertible.ToDecimal(null));
						case TypeCode.Single:
							if (Value is float)
							{
								return (ushort)Math.Round((float)Value);
							}
							return (ushort)Math.Round(convertible.ToSingle(null));
						case TypeCode.Double:
							if (Value is double)
							{
								return (ushort)Math.Round((double)Value);
							}
							return (ushort)Math.Round(convertible.ToDouble(null));
						case TypeCode.String:
							{
								string text = Value as string;
								if (text != null)
								{
									return ToUShort(text);
								}
								return ToUShort(convertible.ToString(null));
							}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "UShort"));
			}
		}

		/// <summary>Converts a string to an integer value.</summary>
		/// <returns>The int value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static int ToInteger(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				try
				{
					long i64Value = default(long);
					if (Utils.IsHexOrOctValue(Value, ref i64Value))
					{
						return (int)i64Value;
					}
					return (int)Math.Round(ParseDouble(Value));
				}
				catch (FormatException innerException)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "Integer"), innerException);
				}
			}
		}

		/// <summary>Converts an object to an integer value.</summary>
		/// <returns>The int value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static int ToInteger(object Value)
		{
			if (Value == null)
			{
				return 0;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
						case TypeCode.Boolean:
							unchecked
							{
								if (Value is bool)
								{
									return 0 - (((bool)Value) ? 1 : 0);
								}
								return 0 - (convertible.ToBoolean(null) ? 1 : 0);
							}
						case TypeCode.SByte:
							if (Value is sbyte)
							{
								return (sbyte)Value;
							}
							return convertible.ToSByte(null);
						case TypeCode.Byte:
							if (Value is byte)
							{
								return (byte)Value;
							}
							return convertible.ToByte(null);
						case TypeCode.Int16:
							if (Value is short)
							{
								return (short)Value;
							}
							return convertible.ToInt16(null);
						case TypeCode.UInt16:
							if (Value is ushort)
							{
								return (ushort)Value;
							}
							return convertible.ToUInt16(null);
						case TypeCode.Int32:
							if (Value is int)
							{
								return (int)Value;
							}
							return convertible.ToInt32(null);
						case TypeCode.UInt32:
							if (Value is uint)
							{
								return (int)(uint)Value;
							}
							return (int)convertible.ToUInt32(null);
						case TypeCode.Int64:
							if (Value is long)
							{
								return (int)(long)Value;
							}
							return (int)convertible.ToInt64(null);
						case TypeCode.UInt64:
							if (Value is ulong)
							{
								return (int)(ulong)Value;
							}
							return (int)convertible.ToUInt64(null);
						case TypeCode.Decimal:
							if (Value is decimal)
							{
								return convertible.ToInt32(null);
							}
							return Convert.ToInt32(convertible.ToDecimal(null));
						case TypeCode.Single:
							if (Value is float)
							{
								return (int)Math.Round((float)Value);
							}
							return (int)Math.Round(convertible.ToSingle(null));
						case TypeCode.Double:
							if (Value is double)
							{
								return (int)Math.Round((double)Value);
							}
							return (int)Math.Round(convertible.ToDouble(null));
						case TypeCode.String:
							{
								string text = Value as string;
								if (text != null)
								{
									return ToInteger(text);
								}
								return ToInteger(convertible.ToString(null));
							}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Integer"));
			}
		}

		/// <summary>Converts a string to a Uint value.</summary>
		/// <returns>The Uint value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static uint ToUInteger(string Value)
		{
			if (Value == null)
			{
				return 0u;
			}
			checked
			{
				try
				{
					long i64Value = default(long);
					if (Utils.IsHexOrOctValue(Value, ref i64Value))
					{
						return (uint)i64Value;
					}
					return (uint)Math.Round(ParseDouble(Value));
				}
				catch (FormatException innerException)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "UInteger"), innerException);
				}
			}
		}

		/// <summary>Converts an object to a Uint value.</summary>
		/// <returns>The Uint value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static uint ToUInteger(object Value)
		{
			if (Value == null)
			{
				return 0u;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
						case TypeCode.Boolean:
							unchecked
							{
								if (Value is bool)
								{
									return (uint)(0 - (((bool)Value) ? 1 : 0));
								}
								return (uint)(0 - (convertible.ToBoolean(null) ? 1 : 0));
							}
						case TypeCode.SByte:
							if (Value is sbyte)
							{
								return (uint)(sbyte)Value;
							}
							return (uint)convertible.ToSByte(null);
						case TypeCode.Byte:
							if (Value is byte)
							{
								return (byte)Value;
							}
							return convertible.ToByte(null);
						case TypeCode.Int16:
							if (Value is short)
							{
								return (uint)(short)Value;
							}
							return (uint)convertible.ToInt16(null);
						case TypeCode.UInt16:
							if (Value is ushort)
							{
								return (ushort)Value;
							}
							return convertible.ToUInt16(null);
						case TypeCode.Int32:
							if (Value is int)
							{
								return (uint)(int)Value;
							}
							return (uint)convertible.ToInt32(null);
						case TypeCode.UInt32:
							if (Value is uint)
							{
								return (uint)Value;
							}
							return convertible.ToUInt32(null);
						case TypeCode.Int64:
							if (Value is long)
							{
								return (uint)(long)Value;
							}
							return (uint)convertible.ToInt64(null);
						case TypeCode.UInt64:
							if (Value is ulong)
							{
								return (uint)(ulong)Value;
							}
							return (uint)convertible.ToUInt64(null);
						case TypeCode.Decimal:
							if (Value is decimal)
							{
								return convertible.ToUInt32(null);
							}
							return Convert.ToUInt32(convertible.ToDecimal(null));
						case TypeCode.Single:
							if (Value is float)
							{
								return (uint)Math.Round((float)Value);
							}
							return (uint)Math.Round(convertible.ToSingle(null));
						case TypeCode.Double:
							if (Value is double)
							{
								return (uint)Math.Round((double)Value);
							}
							return (uint)Math.Round(convertible.ToDouble(null));
						case TypeCode.String:
							{
								string text = Value as string;
								if (text != null)
								{
									return ToUInteger(text);
								}
								return ToUInteger(convertible.ToString(null));
							}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "UInteger"));
			}
		}

		/// <summary>Converts a string to a Long value.</summary>
		/// <returns>The Long value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static long ToLong(string Value)
		{
			if (Value == null)
			{
				return 0L;
			}
			try
			{
				long i64Value = default(long);
				if (Utils.IsHexOrOctValue(Value, ref i64Value))
				{
					return i64Value;
				}
				return Convert.ToInt64(ParseDecimal(Value, null));
			}
			catch (FormatException innerException)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "Long"), innerException);
			}
		}

		/// <summary>Converts an object to a Long value.</summary>
		/// <returns>The Long value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static long ToLong(object Value)
		{
			if (Value == null)
			{
				return 0L;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
						case TypeCode.Boolean:
							unchecked
							{
								if (Value is bool)
								{
									return 0 - (((bool)Value) ? 1 : 0);
								}
								return 0 - (convertible.ToBoolean(null) ? 1 : 0);
							}
						case TypeCode.SByte:
							if (Value is sbyte)
							{
								return (sbyte)Value;
							}
							return convertible.ToSByte(null);
						case TypeCode.Byte:
							if (Value is byte)
							{
								return (byte)Value;
							}
							return convertible.ToByte(null);
						case TypeCode.Int16:
							if (Value is short)
							{
								return (short)Value;
							}
							return convertible.ToInt16(null);
						case TypeCode.UInt16:
							if (Value is ushort)
							{
								return (ushort)Value;
							}
							return convertible.ToUInt16(null);
						case TypeCode.Int32:
							if (Value is int)
							{
								return (int)Value;
							}
							return convertible.ToInt32(null);
						case TypeCode.UInt32:
							if (Value is uint)
							{
								return (uint)Value;
							}
							return convertible.ToUInt32(null);
						case TypeCode.Int64:
							if (Value is long)
							{
								return (long)Value;
							}
							return convertible.ToInt64(null);
						case TypeCode.UInt64:
							if (Value is ulong)
							{
								return (long)(ulong)Value;
							}
							return (long)convertible.ToUInt64(null);
						case TypeCode.Decimal:
							if (Value is decimal)
							{
								return convertible.ToInt64(null);
							}
							return Convert.ToInt64(convertible.ToDecimal(null));
						case TypeCode.Single:
							if (Value is float)
							{
								return (long)Math.Round((float)Value);
							}
							return (long)Math.Round(convertible.ToSingle(null));
						case TypeCode.Double:
							if (Value is double)
							{
								return (long)Math.Round((double)Value);
							}
							return (long)Math.Round(convertible.ToDouble(null));
						case TypeCode.String:
							{
								string text = Value as string;
								if (text != null)
								{
									return ToLong(text);
								}
								return ToLong(convertible.ToString(null));
							}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Long"));
			}
		}

		/// <summary>Converts a string to a Ulong value.</summary>
		/// <returns>The Ulong value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static ulong ToULong(string Value)
		{
			if (Value == null)
			{
				return 0uL;
			}
			try
			{
				ulong ui64Value = default(ulong);
				if (Utils.IsHexOrOctValue(Value, ref ui64Value))
				{
					return ui64Value;
				}
				return Convert.ToUInt64(ParseDecimal(Value, null));
			}
			catch (FormatException innerException)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "ULong"), innerException);
			}
		}

		/// <summary>Converts an object to a Ulong value.</summary>
		/// <returns>The Ulong value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static ulong ToULong(object Value)
		{
			if (Value == null)
			{
				return 0uL;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
						case TypeCode.Boolean:
							unchecked
							{
								if (Value is bool)
								{
									return (ulong)(0 - (((bool)Value) ? 1 : 0));
								}
								return (ulong)(0 - (convertible.ToBoolean(null) ? 1 : 0));
							}
						case TypeCode.SByte:
							if (Value is sbyte)
							{
								return (ulong)(sbyte)Value;
							}
							return (ulong)convertible.ToSByte(null);
						case TypeCode.Byte:
							if (Value is byte)
							{
								return (byte)Value;
							}
							return convertible.ToByte(null);
						case TypeCode.Int16:
							if (Value is short)
							{
								return (ulong)(short)Value;
							}
							return (ulong)convertible.ToInt16(null);
						case TypeCode.UInt16:
							if (Value is ushort)
							{
								return (ushort)Value;
							}
							return convertible.ToUInt16(null);
						case TypeCode.Int32:
							if (Value is int)
							{
								return (ulong)(int)Value;
							}
							return (ulong)convertible.ToInt32(null);
						case TypeCode.UInt32:
							if (Value is uint)
							{
								return (uint)Value;
							}
							return convertible.ToUInt32(null);
						case TypeCode.Int64:
							if (Value is long)
							{
								return (ulong)(long)Value;
							}
							return (ulong)convertible.ToInt64(null);
						case TypeCode.UInt64:
							if (Value is ulong)
							{
								return (ulong)Value;
							}
							return convertible.ToUInt64(null);
						case TypeCode.Decimal:
							if (Value is decimal)
							{
								return convertible.ToUInt64(null);
							}
							return Convert.ToUInt64(convertible.ToDecimal(null));
						case TypeCode.Single:
							if (Value is float)
							{
								return (ulong)Math.Round((float)Value);
							}
							return (ulong)Math.Round(convertible.ToSingle(null));
						case TypeCode.Double:
							if (Value is double)
							{
								return (ulong)Math.Round((double)Value);
							}
							return (ulong)Math.Round(convertible.ToDouble(null));
						case TypeCode.String:
							{
								string text = Value as string;
								if (text != null)
								{
									return ToULong(text);
								}
								return ToULong(convertible.ToString(null));
							}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "ULong"));
			}
		}

		/// <summary>Converts a <see cref="T:System.Boolean" /> value to a <see cref="T:System.Decimal" /> value.</summary>
		/// <returns>The Decimal value of the Boolean value.</returns>
		/// <param name="Value">A Boolean value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static decimal ToDecimal(bool Value)
		{
			if (Value)
			{
				return -1m;
			}
			return default(decimal);
		}

		/// <summary>Converts a string to a <see cref="T:System.Decimal" /> value.</summary>
		/// <returns>The Decimal value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static decimal ToDecimal(string Value)
		{
			return ToDecimal(Value, null);
		}

		internal static decimal ToDecimal(string Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return default(decimal);
			}
			try
			{
				long i64Value = default(long);
				if (Utils.IsHexOrOctValue(Value, ref i64Value))
				{
					return new decimal(i64Value);
				}
				return ParseDecimal(Value, NumberFormat);
			}
			catch (OverflowException)
			{
				throw ExceptionUtils.VbMakeException(6);
			}
			catch (FormatException)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "Decimal"));
			}
		}

		/// <summary>Converts an object to a <see cref="T:System.Decimal" /> value.</summary>
		/// <returns>The Decimal value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static decimal ToDecimal(object Value)
		{
			return ToDecimal(Value, null);
		}

		internal static decimal ToDecimal(object Value, NumberFormatInfo NumberFormat)
		{
			decimal result;
			if (Value == null)
			{
				result = default(decimal);
				goto IL_0296;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
					case TypeCode.Boolean:
						break;
					case TypeCode.SByte:
						goto IL_009c;
					case TypeCode.Byte:
						goto IL_00c9;
					case TypeCode.Int16:
						goto IL_00f6;
					case TypeCode.UInt16:
						goto IL_0123;
					case TypeCode.Int32:
						goto IL_0150;
					case TypeCode.UInt32:
						goto IL_017d;
					case TypeCode.Int64:
						goto IL_01aa;
					case TypeCode.UInt64:
						goto IL_01d7;
					case TypeCode.Decimal:
						return convertible.ToDecimal(null);
					case TypeCode.Single:
						goto IL_0211;
					case TypeCode.Double:
						goto IL_0238;
					case TypeCode.String:
						return ToDecimal(convertible.ToString(null), NumberFormat);
					default:
						goto IL_026f;
				}
				if (Value is bool)
				{
					return ToDecimal((bool)Value);
				}
				return ToDecimal(convertible.ToBoolean(null));
			}
			goto IL_026f;
		IL_00c9:
			result = ((Value is byte) ? new decimal((byte)Value) : new decimal(convertible.ToByte(null)));
			goto IL_0296;
		IL_0123:
			result = ((Value is ushort) ? new decimal((ushort)Value) : new decimal(convertible.ToUInt16(null)));
			goto IL_0296;
		IL_01aa:
			result = ((Value is long) ? new decimal((long)Value) : new decimal(convertible.ToInt64(null)));
			goto IL_0296;
		IL_026f:
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Decimal"));
		IL_0296:
			return result;
		IL_0238:
			result = ((Value is double) ? new decimal((double)Value) : new decimal(convertible.ToDouble(null)));
			goto IL_0296;
		IL_00f6:
			result = ((Value is short) ? new decimal((short)Value) : new decimal(convertible.ToInt16(null)));
			goto IL_0296;
		IL_009c:
			result = ((Value is sbyte) ? new decimal((sbyte)Value) : new decimal(convertible.ToSByte(null)));
			goto IL_0296;
		IL_0211:
			result = ((Value is float) ? new decimal((float)Value) : new decimal(convertible.ToSingle(null)));
			goto IL_0296;
		IL_0150:
			result = ((Value is int) ? new decimal((int)Value) : new decimal(convertible.ToInt32(null)));
			goto IL_0296;
		IL_017d:
			result = ((Value is uint) ? new decimal((uint)Value) : new decimal(convertible.ToUInt32(null)));
			goto IL_0296;
		IL_01d7:
			result = ((Value is ulong) ? new decimal((ulong)Value) : new decimal(convertible.ToUInt64(null)));
			goto IL_0296;
		}

		private static decimal ParseDecimal(string Value, NumberFormatInfo NumberFormat)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			if (NumberFormat == null)
			{
				NumberFormat = cultureInfo.NumberFormat;
			}
			NumberFormatInfo normalizedNumberFormat = GetNormalizedNumberFormat(NumberFormat);
			Value = Utils.ToHalfwidthNumbers(Value, cultureInfo);
			try
			{
				return decimal.Parse(Value, NumberStyles.Any, normalizedNumberFormat);
			}
			catch (FormatException) when (NumberFormat != normalizedNumberFormat)
			{
				return decimal.Parse(Value, NumberStyles.Any, NumberFormat);
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
		}

		private static NumberFormatInfo GetNormalizedNumberFormat(NumberFormatInfo InNumberFormat)
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

		/// <summary>Converts a <see cref="T:System.String" /> to a <see cref="T:System.Single" /> value.</summary>
		/// <returns>The Single value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static float ToSingle(string Value)
		{
			return ToSingle(Value, null);
		}

		internal static float ToSingle(string Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0f;
			}
			try
			{
				long i64Value = default(long);
				if (Utils.IsHexOrOctValue(Value, ref i64Value))
				{
					return i64Value;
				}
				double num = ParseDouble(Value, NumberFormat);
				if ((num < -3.4028234663852886E+38 || num > 3.4028234663852886E+38) && !double.IsInfinity(num))
				{
					throw new OverflowException();
				}
				return (float)num;
			}
			catch (FormatException innerException)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "Single"), innerException);
			}
		}

		/// <summary>Converts an object to a <see cref="T:System.Single" /> value.</summary>
		/// <returns>The Single value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static float ToSingle(object Value)
		{
			return ToSingle(Value, null);
		}

		internal static float ToSingle(object Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0f;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
					case TypeCode.Boolean:
						if (Value is bool)
						{
							return 0 - (((bool)Value) ? 1 : 0);
						}
						return 0 - (convertible.ToBoolean(null) ? 1 : 0);
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (sbyte)Value;
						}
						return convertible.ToSByte(null);
					case TypeCode.Byte:
						if (Value is byte)
						{
							return (int)(byte)Value;
						}
						return (int)convertible.ToByte(null);
					case TypeCode.Int16:
						if (Value is short)
						{
							return (short)Value;
						}
						return convertible.ToInt16(null);
					case TypeCode.UInt16:
						if (Value is ushort)
						{
							return (int)(ushort)Value;
						}
						return (int)convertible.ToUInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (int)Value;
						}
						return convertible.ToInt32(null);
					case TypeCode.UInt32:
						if (!(Value is uint))
						{
							return convertible.ToUInt32(null);
						}
						return (uint)Value;
					case TypeCode.Int64:
						if (Value is long)
						{
							return (long)Value;
						}
						return convertible.ToInt64(null);
					case TypeCode.UInt64:
						if (!(Value is ulong))
						{
							return convertible.ToUInt64(null);
						}
						return (ulong)Value;
					case TypeCode.Decimal:
						if (Value is decimal)
						{
							return convertible.ToSingle(null);
						}
						return Convert.ToSingle(convertible.ToDecimal(null));
					case TypeCode.Single:
						if (Value is float)
						{
							return (float)Value;
						}
						return convertible.ToSingle(null);
					case TypeCode.Double:
						if (Value is double)
						{
							return (float)(double)Value;
						}
						return (float)convertible.ToDouble(null);
					case TypeCode.String:
						return ToSingle(convertible.ToString(null), NumberFormat);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Single"));
		}

		/// <summary>Converts a string to a <see cref="T:System.Double" /> value.</summary>
		/// <returns>The Double value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static double ToDouble(string Value)
		{
			return ToDouble(Value, null);
		}

		internal static double ToDouble(string Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0.0;
			}
			try
			{
				long i64Value = default(long);
				if (Utils.IsHexOrOctValue(Value, ref i64Value))
				{
					return i64Value;
				}
				return ParseDouble(Value, NumberFormat);
			}
			catch (FormatException innerException)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "Double"), innerException);
			}
		}

		/// <summary>Converts an object to a <see cref="T:System.Double" /> value.</summary>
		/// <returns>The Double value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static double ToDouble(object Value)
		{
			return ToDouble(Value, null);
		}

		internal static double ToDouble(object Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0.0;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
					case TypeCode.Boolean:
						if (Value is bool)
						{
							return 0 - (((bool)Value) ? 1 : 0);
						}
						return 0 - (convertible.ToBoolean(null) ? 1 : 0);
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (sbyte)Value;
						}
						return convertible.ToSByte(null);
					case TypeCode.Byte:
						if (Value is byte)
						{
							return (int)(byte)Value;
						}
						return (int)convertible.ToByte(null);
					case TypeCode.Int16:
						if (Value is short)
						{
							return (short)Value;
						}
						return convertible.ToInt16(null);
					case TypeCode.UInt16:
						if (Value is ushort)
						{
							return (int)(ushort)Value;
						}
						return (int)convertible.ToUInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (int)Value;
						}
						return convertible.ToInt32(null);
					case TypeCode.UInt32:
						if (!(Value is uint))
						{
							return convertible.ToUInt32(null);
						}
						return (uint)Value;
					case TypeCode.Int64:
						if (Value is long)
						{
							return (long)Value;
						}
						return convertible.ToInt64(null);
					case TypeCode.UInt64:
						if (!(Value is ulong))
						{
							return convertible.ToUInt64(null);
						}
						return (ulong)Value;
					case TypeCode.Decimal:
						if (Value is decimal)
						{
							return convertible.ToDouble(null);
						}
						return Convert.ToDouble(convertible.ToDecimal(null));
					case TypeCode.Single:
						if (Value is float)
						{
							return (float)Value;
						}
						return convertible.ToSingle(null);
					case TypeCode.Double:
						if (Value is double)
						{
							return (double)Value;
						}
						return convertible.ToDouble(null);
					case TypeCode.String:
						return ToDouble(convertible.ToString(null), NumberFormat);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Double"));
		}

		private static double ParseDouble(string Value)
		{
			return ParseDouble(Value, null);
		}

		internal static bool TryParseDouble(string Value, ref double Result)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			NumberFormatInfo numberFormat = cultureInfo.NumberFormat;
			NumberFormatInfo normalizedNumberFormat = GetNormalizedNumberFormat(numberFormat);
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

		private static double ParseDouble(string Value, NumberFormatInfo NumberFormat)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			if (NumberFormat == null)
			{
				NumberFormat = cultureInfo.NumberFormat;
			}
			NumberFormatInfo normalizedNumberFormat = GetNormalizedNumberFormat(NumberFormat);
			Value = Utils.ToHalfwidthNumbers(Value, cultureInfo);
			try
			{
				return double.Parse(Value, NumberStyles.Any, normalizedNumberFormat);
			}
			catch (FormatException) when (NumberFormat != normalizedNumberFormat)
			{
				return double.Parse(Value, NumberStyles.Any, NumberFormat);
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
		}

		/// <summary>Converts a string to a <see cref="T:System.DateTime" /> value.</summary>
		/// <returns>The DateTime value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static DateTime ToDate(string Value)
		{
			DateTime Result = default(DateTime);
			if (TryParseDate(Value, ref Result))
			{
				return Result;
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", Strings.Left(Value, 32), "Date"));
		}

		/// <summary>Converts an object to a <see cref="T:System.DateTime" /> value.</summary>
		/// <returns>The DateTime value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static DateTime ToDate(object Value)
		{
			if (Value != null)
			{
				IConvertible convertible = Value as IConvertible;
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
						case TypeCode.DateTime:
							if (Value is DateTime)
							{
								return (DateTime)Value;
							}
							return convertible.ToDateTime(null);
						case TypeCode.String:
							{
								string text = Value as string;
								if (text != null)
								{
									return ToDate(text);
								}
								return ToDate(convertible.ToString(null));
							}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Date"));
			}
			DateTime result = default(DateTime);
			return result;
		}

		internal static bool TryParseDate(string Value, ref DateTime Result)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			return DateTime.TryParse(Utils.ToHalfwidthNumbers(Value, cultureInfo), cultureInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault, out Result);
		}

		/// <summary>Converts a string to a <see cref="T:System.Char" /> value.</summary>
		/// <returns>The Char value of the string.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static char ToChar(string Value)
		{
			if (Value == null || Value.Length == 0)
			{
				return '\0';
			}
			return Value[0];
		}

		/// <summary>Converts an object to a <see cref="T:System.Char" /> value.</summary>
		/// <returns>The Char value of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static char ToChar(object Value)
		{
			if (Value == null)
			{
				return '\0';
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
					case TypeCode.Char:
						if (Value is char)
						{
							return (char)Value;
						}
						return convertible.ToChar(null);
					case TypeCode.String:
						{
							string text = Value as string;
							if (text != null)
							{
								return ToChar(text);
							}
							return ToChar(convertible.ToString(null));
						}
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Char"));
		}

		/// <summary>Converts a string to a one-dimensional <see cref="T:System.Char" /> array.</summary>
		/// <returns>A one-dimensional Char array.</returns>
		/// <param name="Value">The string to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static char[] ToCharArrayRankOne(string Value)
		{
			if (Value == null)
			{
				Value = "";
			}
			return Value.ToCharArray();
		}

		/// <summary>Converts an object to a one-dimensional <see cref="T:System.Char" /> array.</summary>
		/// <returns>A one-dimensional Char array.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static char[] ToCharArrayRankOne(object Value)
		{
			if (Value == null)
			{
				return "".ToCharArray();
			}
			char[] array = Value as char[];
			if (array != null && array.Rank == 1)
			{
				return array;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null && convertible.GetTypeCode() == TypeCode.String)
			{
				return convertible.ToString(null).ToCharArray();
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "Char()"));
		}

		/// <summary>Converts a <see cref="T:System.Boolean" /> value to a <see cref="T:System.String" />.</summary>
		/// <returns>The String representation of the Boolean value.</returns>
		/// <param name="Value">The Boolean value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(bool Value)
		{
			if (Value)
			{
				return bool.TrueString;
			}
			return bool.FalseString;
		}

		/// <summary>Converts a <see cref="T:System.Byte" /> value to a <see cref="T:System.String" />.</summary>
		/// <returns>The String representation of the Byte value.</returns>
		/// <param name="Value">The Byte value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(byte Value)
		{
			return Value.ToString(null, null);
		}

		/// <summary>Converts a <see cref="T:System.Char" /> value to a <see cref="T:System.String" />.</summary>
		/// <returns>The String representation of the Char value.</returns>
		/// <param name="Value">The Char value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(char Value)
		{
			return Value.ToString();
		}

		/// <summary>Converts a <see cref="T:System.Char" /> array to a string.</summary>
		/// <returns>The string representation of the specified array.</returns>
		/// <param name="Value">The Char array to convert.</param>
		public static string FromCharArray(char[] Value)
		{
			return new string(Value);
		}

		/// <summary>Converts a <see cref="T:System.Char" /> value to a string, given a byte count.</summary>
		/// <returns>The string representation of the specified value.</returns>
		/// <param name="Value">The Char value to convert.</param>
		/// <param name="Count">The byte count of the Char value.</param>
		public static string FromCharAndCount(char Value, int Count)
		{
			return new string(Value, Count);
		}

		/// <summary>Converts a subset of a <see cref="T:System.Char" /> array to a string.</summary>
		/// <returns>The string representation of the specified array from the start position to the specified length.</returns>
		/// <param name="Value">The Char array to convert.</param>
		/// <param name="StartIndex">Zero-based index of the start position.</param>
		/// <param name="Length">Length of the subset in bytes.</param>
		public static string FromCharArraySubset(char[] Value, int StartIndex, int Length)
		{
			return new string(Value, StartIndex, Length);
		}

		/// <summary>Converts a Short value to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the Short value.</returns>
		/// <param name="Value">The Short value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(short Value)
		{
			return Value.ToString(null, null);
		}

		/// <summary>Converts an integer value to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the int value.</returns>
		/// <param name="Value">The int value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(int Value)
		{
			return Value.ToString(null, null);
		}

		/// <summary>Converts a uint value to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the Uint value.</returns>
		/// <param name="Value">The Uint value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string ToString(uint Value)
		{
			return Value.ToString(null, null);
		}

		/// <summary>Converts a Long value to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the Long value.</returns>
		/// <param name="Value">The Long value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(long Value)
		{
			return Value.ToString(null, null);
		}

		/// <summary>Converts a Ulong value to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the Ulong value.</returns>
		/// <param name="Value">The Ulong value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public static string ToString(ulong Value)
		{
			return Value.ToString(null, null);
		}

		/// <summary>Converts a <see cref="T:System.Single" /> value (a single-precision floating point number) to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the Single value.</returns>
		/// <param name="Value">The Single value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(float Value)
		{
			return ToString(Value, null);
		}

		/// <summary>Converts a <see cref="T:System.Double" /> value to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the Double value.</returns>
		/// <param name="Value">The Double value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(double Value)
		{
			return ToString(Value, null);
		}

		/// <summary>Converts a <see cref="T:System.Single" /> value to a <see cref="T:System.String" /> value, using the specified number format.</summary>
		/// <returns>The String representation of the Single value.</returns>
		/// <param name="Value">The Single value to convert.</param>
		/// <param name="NumberFormat">The number format to use, according to <see cref="T:System.Globalization.NumberFormatInfo" />.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(float Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString(null, NumberFormat);
		}

		/// <summary>Converts a <see cref="T:System.Double" /> value to a <see cref="T:System.String" /> value, using the specified number format.</summary>
		/// <returns>The String representation of the Double value.</returns>
		/// <param name="Value">The Double value to convert.</param>
		/// <param name="NumberFormat">The number format to use, according to <see cref="T:System.Globalization.NumberFormatInfo" />.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(double Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString("G", NumberFormat);
		}

		/// <summary>Converts a <see cref="T:System.DateTime" /> value to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the DateTime value.</returns>
		/// <param name="Value">The DateTime value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(DateTime Value)
		{
			long ticks = Value.TimeOfDay.Ticks;
			if (ticks == Value.Ticks || (Value.Year == 1899 && Value.Month == 12 && Value.Day == 30))
			{
				return Value.ToString("T", null);
			}
			if (ticks == 0L)
			{
				return Value.ToString("d", null);
			}
			return Value.ToString("G", null);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> value to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the Decimal value.</returns>
		/// <param name="Value">The Decimal value to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(decimal Value)
		{
			return ToString(Value, null);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> value to a <see cref="T:System.String" /> value, using the specified number format.</summary>
		/// <returns>The String representation of the Decimal value.</returns>
		/// <param name="Value">The decimal value to convert.</param>
		/// <param name="NumberFormat">The number format to use, according to <see cref="T:System.Globalization.NumberFormatInfo" />.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(decimal Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString("G", NumberFormat);
		}

		/// <summary>Converts an object to a <see cref="T:System.String" /> value.</summary>
		/// <returns>The String representation of the object.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string ToString(object Value)
		{
			if (Value == null)
			{
				return null;
			}
			string text = Value as string;
			if (text != null)
			{
				return text;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
					case TypeCode.Boolean:
						return ToString(convertible.ToBoolean(null));
					case TypeCode.SByte:
						return ToString((int)convertible.ToSByte(null));
					case TypeCode.Byte:
						return ToString(convertible.ToByte(null));
					case TypeCode.Int16:
						return ToString((int)convertible.ToInt16(null));
					case TypeCode.UInt16:
						return ToString((uint)convertible.ToUInt16(null));
					case TypeCode.Int32:
						return ToString(convertible.ToInt32(null));
					case TypeCode.UInt32:
						return ToString(convertible.ToUInt32(null));
					case TypeCode.Int64:
						return ToString(convertible.ToInt64(null));
					case TypeCode.UInt64:
						return ToString(convertible.ToUInt64(null));
					case TypeCode.Decimal:
						return ToString(convertible.ToDecimal(null));
					case TypeCode.Single:
						return ToString(convertible.ToSingle(null));
					case TypeCode.Double:
						return ToString(convertible.ToDouble(null));
					case TypeCode.Char:
						return ToString(convertible.ToChar(null));
					case TypeCode.DateTime:
						return ToString(convertible.ToDateTime(null));
					case TypeCode.String:
						return convertible.ToString(null);
				}
			}
			else
			{
				char[] array = Value as char[];
				if (array != null)
				{
					return new string(array);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(Value), "String"));
		}

		/// <summary>Converts an object to a generic type <paramref name="T" />.</summary>
		/// <returns>A structure or object of generic type <paramref name="T" />.</returns>
		/// <param name="Value">The object to convert.</param>
		/// <typeparam name="T">The type to convert <paramref name="Value" /> to.</typeparam>
		public static T ToGenericParameter<T>(object Value)
		{
			if (Value == null)
			{
				return default(T);
			}
			switch (Symbols.GetTypeCode(typeof(T)))
			{
				case TypeCode.Boolean:
					return (T)(object)ToBoolean(Value);
				case TypeCode.SByte:
					return (T)(object)ToSByte(Value);
				case TypeCode.Byte:
					return (T)(object)ToByte(Value);
				case TypeCode.Int16:
					return (T)(object)ToShort(Value);
				case TypeCode.UInt16:
					return (T)(object)ToUShort(Value);
				case TypeCode.Int32:
					return (T)(object)ToInteger(Value);
				case TypeCode.UInt32:
					return (T)(object)ToUInteger(Value);
				case TypeCode.Int64:
					return (T)(object)ToLong(Value);
				case TypeCode.UInt64:
					return (T)(object)ToULong(Value);
				case TypeCode.Decimal:
					return (T)(object)ToDecimal(Value);
				case TypeCode.Single:
					return (T)(object)ToSingle(Value);
				case TypeCode.Double:
					return (T)(object)ToDouble(Value);
				case TypeCode.DateTime:
					return (T)(object)ToDate(Value);
				case TypeCode.Char:
					return (T)(object)ToChar(Value);
				case TypeCode.String:
					return (T)(object)ToString(Value);
				default:
					return (T)Value;
			}
		}

		private static object CastSByteEnum(sbyte Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastByteEnum(byte Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastInt16Enum(short Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastUInt16Enum(ushort Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastInt32Enum(int Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastUInt32Enum(uint Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastInt64Enum(long Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastUInt64Enum(ulong Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		internal static object ForceValueCopy(object Expression, Type TargetType)
		{
			IConvertible convertible = Expression as IConvertible;
			if (convertible == null)
			{
				return Expression;
			}
			switch (convertible.GetTypeCode())
			{
				case TypeCode.Boolean:
					return convertible.ToBoolean(null);
				case TypeCode.SByte:
					return CastSByteEnum(convertible.ToSByte(null), TargetType);
				case TypeCode.Byte:
					return CastByteEnum(convertible.ToByte(null), TargetType);
				case TypeCode.Int16:
					return CastInt16Enum(convertible.ToInt16(null), TargetType);
				case TypeCode.UInt16:
					return CastUInt16Enum(convertible.ToUInt16(null), TargetType);
				case TypeCode.Int32:
					return CastInt32Enum(convertible.ToInt32(null), TargetType);
				case TypeCode.UInt32:
					return CastUInt32Enum(convertible.ToUInt32(null), TargetType);
				case TypeCode.Int64:
					return CastInt64Enum(convertible.ToInt64(null), TargetType);
				case TypeCode.UInt64:
					return CastUInt64Enum(convertible.ToUInt64(null), TargetType);
				case TypeCode.Decimal:
					return convertible.ToDecimal(null);
				case TypeCode.Single:
					return convertible.ToSingle(null);
				case TypeCode.Double:
					return convertible.ToDouble(null);
				case TypeCode.DateTime:
					return convertible.ToDateTime(null);
				case TypeCode.Char:
					return convertible.ToChar(null);
				default:
					return Expression;
			}
		}

		private static object ChangeIntrinsicType(object Expression, Type TargetType)
		{
			switch (Symbols.GetTypeCode(TargetType))
			{
				case TypeCode.Boolean:
					return ToBoolean(Expression);
				case TypeCode.SByte:
					return CastSByteEnum(ToSByte(Expression), TargetType);
				case TypeCode.Byte:
					return CastByteEnum(ToByte(Expression), TargetType);
				case TypeCode.Int16:
					return CastInt16Enum(ToShort(Expression), TargetType);
				case TypeCode.UInt16:
					return CastUInt16Enum(ToUShort(Expression), TargetType);
				case TypeCode.Int32:
					return CastInt32Enum(ToInteger(Expression), TargetType);
				case TypeCode.UInt32:
					return CastUInt32Enum(ToUInteger(Expression), TargetType);
				case TypeCode.Int64:
					return CastInt64Enum(ToLong(Expression), TargetType);
				case TypeCode.UInt64:
					return CastUInt64Enum(ToULong(Expression), TargetType);
				case TypeCode.Decimal:
					return ToDecimal(Expression);
				case TypeCode.Single:
					return ToSingle(Expression);
				case TypeCode.Double:
					return ToDouble(Expression);
				case TypeCode.DateTime:
					return ToDate(Expression);
				case TypeCode.Char:
					return ToChar(Expression);
				case TypeCode.String:
					return ToString(Expression);
				default:
					throw new Exception();
			}
		}
	}
}
