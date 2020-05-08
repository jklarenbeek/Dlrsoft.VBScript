using Microsoft.VisualBasic.CompilerService;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic
{
	public class Strings
	{
		internal enum FormatType
		{
			Number,
			Percent,
			Currency
		}

		private static readonly string[] CurrencyPositiveFormatStrings = new string[4]
		{
			"'$'n",
			"n'$'",
			"'$' n",
			"n '$'"
		};

		private static readonly string[] CurrencyNegativeFormatStrings = new string[16]
		{
			"('$'n)",
			"-'$'n",
			"'$'-n",
			"'$'n-",
			"(n'$')",
			"-n'$'",
			"n-'$'",
			"n'$'-",
			"-n '$'",
			"-'$' n",
			"n '$'-",
			"'$' n-",
			"'$'- n",
			"n- '$'",
			"('$' n)",
			"(n '$')"
		};

		private static readonly string[] NumberNegativeFormatStrings = new string[5]
		{
			"(n)",
			"-n",
			"- n",
			"n-",
			"n -"
		};

		internal static char[] m_achIntlSpace = new char[2]
		{
			' ',
			'\u3000'
		};


		internal static readonly CompareInfo m_InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;

		private static object m_SyncObject = new object();

		private static CultureInfo m_LastUsedYesNoCulture;

		private static string m_CachedYesNoFormatStyle;

		private static CultureInfo m_LastUsedOnOffCulture;

		private static string m_CachedOnOffFormatStyle;

		private static CultureInfo m_LastUsedTrueFalseCulture;

		private static string m_CachedTrueFalseFormatStyle;

		private static string CachedYesNoFormatStyle
		{
			get
			{
				CultureInfo cultureInfo = Utils.GetCultureInfo();
				object syncObject = m_SyncObject;
				CheckForSyncLockOnValueType(syncObject);
				bool lockTaken = false;
				try
				{
					Monitor.Enter(syncObject, ref lockTaken);
					if (m_LastUsedYesNoCulture != cultureInfo)
					{
						m_LastUsedYesNoCulture = cultureInfo;
						m_CachedYesNoFormatStyle = Utils.GetResourceString("YesNoFormatStyle");
					}
					return m_CachedYesNoFormatStyle;
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(syncObject);
					}
				}
			}
		}
		private static string CachedOnOffFormatStyle
		{
			get
			{
				CultureInfo cultureInfo = Utils.GetCultureInfo();
				object syncObject = m_SyncObject;
				CheckForSyncLockOnValueType(syncObject);
				bool lockTaken = false;
				try
				{
					Monitor.Enter(syncObject, ref lockTaken);
					if (m_LastUsedOnOffCulture != cultureInfo)
					{
						m_LastUsedOnOffCulture = cultureInfo;
						m_CachedOnOffFormatStyle = Utils.GetResourceString("OnOffFormatStyle");
					}
					return m_CachedOnOffFormatStyle;
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(syncObject);
					}
				}
			}
		}
		private static string CachedTrueFalseFormatStyle
		{
			get
			{
				CultureInfo cultureInfo = Utils.GetCultureInfo();
				object syncObject = m_SyncObject;
				CheckForSyncLockOnValueType(syncObject);
				bool lockTaken = false;
				try
				{
					Monitor.Enter(syncObject, ref lockTaken);
					if (m_LastUsedTrueFalseCulture != cultureInfo)
					{
						m_LastUsedTrueFalseCulture = cultureInfo;
						m_CachedTrueFalseFormatStyle = Utils.GetResourceString("TrueFalseFormatStyle");
					}
					return m_CachedTrueFalseFormatStyle;
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(syncObject);
					}
				}
			}
		}

		private static void CheckForSyncLockOnValueType(object Expression)
		{
			if (Expression != null && Expression.GetType().IsValueType)
				throw new ArgumentException(Utils.GetResourceString("SyncLockRequiresReferenceType1", Utils.VBFriendlyName(Expression.GetType())));
		}

		private static bool FormatNamed(object Expression, string Style, ref string ReturnValue)
		{
			int length = Style.Length;
			ReturnValue = null;
			switch (length)
			{
				case 5:
					{
						char c4 = Style[0];
						if ((c4 == 'F' || c4 == 'f') && string.Compare(Style, "fixed", StringComparison.OrdinalIgnoreCase) == 0)
						{
							ReturnValue = Conversions.ToDouble(Expression).ToString("0.00", null);
							return true;
						}
						break;
					}
				case 6:
					switch (Style[0])
					{
						case 'Y':
						case 'y':
							if (string.Compare(Style, "yes/no", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = (0 - (Conversions.ToBoolean(Expression) ? 1 : 0)).ToString(CachedYesNoFormatStyle, null);
								return true;
							}
							break;
						case 'O':
						case 'o':
							if (string.Compare(Style, "on/off", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = (0 - (Conversions.ToBoolean(Expression) ? 1 : 0)).ToString(CachedOnOffFormatStyle, null);
								return true;
							}
							break;
					}
					break;
				case 7:
					{
						char c3 = Style[0];
						if ((c3 == 'P' || c3 == 'p') && string.Compare(Style, "percent", StringComparison.OrdinalIgnoreCase) == 0)
						{
							ReturnValue = Conversions.ToDouble(Expression).ToString("0.00%", null);
							return true;
						}
						break;
					}
				case 8:
					switch (Style[0])
					{
						case 'S':
						case 's':
							if (string.Compare(Style, "standard", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = Conversions.ToDouble(Expression).ToString("N2", null);
								return true;
							}
							break;
						case 'C':
						case 'c':
							if (string.Compare(Style, "currency", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = Conversions.ToDouble(Expression).ToString("C", null);
								return true;
							}
							break;
					}
					break;
				case 9:
					switch (Style[5])
					{
						case 'T':
						case 't':
							if (string.Compare(Style, "long time", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = Conversions.ToDate(Expression).ToString("T", null);
								return true;
							}
							break;
						case 'D':
						case 'd':
							if (string.Compare(Style, "long date", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = Conversions.ToDate(Expression).ToString("D", null);
								return true;
							}
							break;
					}
					break;
				case 10:
					switch (Style[6])
					{
						case 'A':
						case 'a':
							if (string.Compare(Style, "true/false", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = (0 - (Conversions.ToBoolean(Expression) ? 1 : 0)).ToString(CachedTrueFalseFormatStyle, null);
								return true;
							}
							break;
						case 'T':
						case 't':
							if (string.Compare(Style, "short time", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = Conversions.ToDate(Expression).ToString("t", null);
								return true;
							}
							break;
						case 'D':
						case 'd':
							if (string.Compare(Style, "short date", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = Conversions.ToDate(Expression).ToString("d", null);
								return true;
							}
							break;
						case 'I':
						case 'i':
							if (string.Compare(Style, "scientific", StringComparison.OrdinalIgnoreCase) == 0)
							{
								double d = Conversions.ToDouble(Expression);
								if (double.IsNaN(d) || double.IsInfinity(d))
								{
									ReturnValue = d.ToString("G", null);
								}
								else
								{
									ReturnValue = d.ToString("0.00E+00", null);
								}
								return true;
							}
							break;
					}
					break;
				case 11:
					switch (Style[7])
					{
						case 'T':
						case 't':
							if (string.Compare(Style, "medium time", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = Conversions.ToDate(Expression).ToString("T", null);
								return true;
							}
							break;
						case 'D':
						case 'd':
							if (string.Compare(Style, "medium date", StringComparison.OrdinalIgnoreCase) == 0)
							{
								ReturnValue = Conversions.ToDate(Expression).ToString("D", null);
								return true;
							}
							break;
					}
					break;
				case 12:
					{
						char c2 = Style[0];
						if ((c2 == 'G' || c2 == 'g') && string.Compare(Style, "general date", StringComparison.OrdinalIgnoreCase) == 0)
						{
							ReturnValue = Conversions.ToDate(Expression).ToString("G", null);
							return true;
						}
						break;
					}
				case 14:
					{
						char c = Style[0];
						if ((c == 'G' || c == 'g') && string.Compare(Style, "general number", StringComparison.OrdinalIgnoreCase) == 0)
						{
							ReturnValue = Conversions.ToDouble(Expression).ToString("G", null);
							return true;
						}
						break;
					}
			}
			return false;
		}

		/// <summary>Returns a string formatted according to instructions contained in a format String expression.</summary>
		/// <returns>Returns a string formatted according to instructions contained in a format String expression.</returns>
		/// <param name="Expression">Required. Any valid expression.</param>
		/// <param name="Style">Optional. A valid named or user-defined format String expression.</param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Format(object Expression, string Style = "")
		{
			try
			{
				IFormatProvider formatProvider = null;
				IFormattable formattable = null;
				if (Expression == null || (object)Expression.GetType() == null)
				{
					return "";
				}
				if (Style != null && Style.Length != 0)
				{
					IConvertible convertible = (IConvertible)Expression;
					TypeCode typeCode = convertible.GetTypeCode();
					if (Style.Length > 0)
					{
						try
						{
							string ReturnValue = null;
							if (FormatNamed(Expression, Style, ref ReturnValue))
							{
								return ReturnValue;
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
							return Conversions.ToString(Expression);
						}
					}
					formattable = (Expression as IFormattable);
					if (formattable == null)
					{
						typeCode = Convert.GetTypeCode(Expression);
						if (typeCode != TypeCode.String && typeCode != TypeCode.Boolean)
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Expression"));
						}
					}
					switch (typeCode)
					{
						case TypeCode.Boolean:
							return string.Format(formatProvider, Style, new object[1]
							{
						Conversions.ToString(convertible.ToBoolean(null))
							});
						case TypeCode.Object:
						case TypeCode.Char:
						case TypeCode.SByte:
						case TypeCode.Byte:
						case TypeCode.Int16:
						case TypeCode.UInt16:
						case TypeCode.Int32:
						case TypeCode.UInt32:
						case TypeCode.Int64:
						case TypeCode.UInt64:
						case TypeCode.Decimal:
						case TypeCode.DateTime:
							return formattable.ToString(Style, formatProvider);
						case TypeCode.DBNull:
							return "";
						case TypeCode.Double:
							{
								double num2 = convertible.ToDouble(null);
								if (Style == null || Style.Length == 0)
								{
									return Conversions.ToString(num2);
								}
								if (num2 == 0.0)
								{
									num2 = 0.0;
								}
								return num2.ToString(Style, formatProvider);
							}
						case TypeCode.Empty:
							return "";
						case TypeCode.Single:
							{
								float num = convertible.ToSingle(null);
								if (Style == null || Style.Length == 0)
								{
									return Conversions.ToString(num);
								}
								if (num == 0f)
								{
									num = 0f;
								}
								return num.ToString(Style, formatProvider);
							}
						case TypeCode.String:
							return string.Format(formatProvider, Style, new object[1]
							{
						Expression
							});
						default:
							return formattable.ToString(Style, formatProvider);
					}
				}
				return Conversions.ToString(Expression);
			}
			catch (Exception ex5)
			{
				throw ex5;
			}
		}

		/// <summary>Returns an expression formatted as a currency value using the currency symbol defined in the system control panel.</summary>
		/// <returns>Returns an expression formatted as a currency value using the currency symbol defined in the system control panel.</returns>
		/// <param name="Expression">Required. Expression to be formatted.</param>
		/// <param name="NumDigitsAfterDecimal">Optional. Numeric value indicating how many places are displayed to the right of the decimal. Default value is –1, which indicates that the computer's regional settings are used.</param>
		/// <param name="IncludeLeadingDigit">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> enumeration that indicates whether or not a leading zero is displayed for fractional values. See "Remarks" for values.</param>
		/// <param name="UseParensForNegativeNumbers">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> enumeration that indicates whether or not to place negative values within parentheses. See "Remarks" for values.</param>
		/// <param name="GroupDigits">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> enumeration that indicates whether or not numbers are grouped using the group delimiter specified in the computer's regional settings. See "Remarks" for values.</param>
		/// <exception cref="T:System.ArgumentException">Number of digits after decimal point is greater than 99.</exception>
		/// <exception cref="T:System.InvalidCastException">Type is not numeric.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string FormatCurrency(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
		{
			IFormatProvider formatProvider = null;
			try
			{
				ValidateTriState(IncludeLeadingDigit);
				ValidateTriState(UseParensForNegativeNumbers);
				ValidateTriState(GroupDigits);
				if (NumDigitsAfterDecimal > 99)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_Range0to99_1", "NumDigitsAfterDecimal"));
				}
				if (Expression == null)
				{
					return "";
				}
				Type type = Expression.GetType();
				if ((object)type == typeof(string))
				{
					Expression = Conversions.ToDouble(Expression);
				}
				else if (!Symbols.IsNumericType(type))
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(type), "Currency"));
				}
				IFormattable obj = (IFormattable)Expression;
				if (IncludeLeadingDigit == TriState.False)
				{
					double num = Conversions.ToDouble(Expression);
					if (num >= 1.0 || num <= -1.0)
					{
						IncludeLeadingDigit = TriState.True;
					}
				}
				string currencyFormatString = GetCurrencyFormatString(IncludeLeadingDigit, NumDigitsAfterDecimal, UseParensForNegativeNumbers, GroupDigits, ref formatProvider);
				return obj.ToString(currencyFormatString, formatProvider);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns a string expression representing a date/time value.</summary>
		/// <returns>Returns a string expression representing a date/time value.</returns>
		/// <param name="Expression">Required. Date expression to be formatted.</param>
		/// <param name="NamedFormat">Optional. Numeric value that indicates the date/time format used. If omitted, DateFormat.GeneralDate is used.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="NamedFormat" /> setting is not valid.</exception>
		/// <filterpriority>1</filterpriority>
		public static string FormatDateTime(DateTime Expression, DateFormat NamedFormat = DateFormat.GeneralDate)
		{
			try
			{
				string format;
				switch (NamedFormat)
				{
					case DateFormat.LongDate:
						format = "D";
						break;
					case DateFormat.ShortDate:
						format = "d";
						break;
					case DateFormat.LongTime:
						format = "T";
						break;
					case DateFormat.ShortTime:
						format = "HH:mm";
						break;
					case DateFormat.GeneralDate:
						format = ((Expression.TimeOfDay.Ticks != Expression.Ticks) ? ((Expression.TimeOfDay.Ticks != 0L) ? "G" : "d") : "T");
						break;
					default:
						throw ExceptionUtils.VbMakeException(5);
				}
				return Expression.ToString(format, null);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns an expression formatted as a number.</summary>
		/// <returns>Returns an expression formatted as a number.</returns>
		/// <param name="Expression">Required. Expression to be formatted.</param>
		/// <param name="NumDigitsAfterDecimal">Optional. Numeric value indicating how many places are displayed to the right of the decimal. The default value is –1, which indicates that the computer's regional settings are used.</param>
		/// <param name="IncludeLeadingDigit">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether a leading 0 is displayed for fractional values. See "Settings" for values.</param>
		/// <param name="UseParensForNegativeNumbers">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether to place negative values within parentheses. See "Settings" for values.</param>
		/// <param name="GroupDigits">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether or not numbers are grouped using the group delimiter specified in the locale settings. See "Settings" for values.</param>
		/// <exception cref="T:System.InvalidCastException">Type is not numeric.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string FormatNumber(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
		{
			try
			{
				ValidateTriState(IncludeLeadingDigit);
				ValidateTriState(UseParensForNegativeNumbers);
				ValidateTriState(GroupDigits);
				if (Expression == null)
				{
					return "";
				}
				Type type = Expression.GetType();
				if ((object)type == typeof(string))
				{
					Expression = Conversions.ToDouble(Expression);
				}
				else if ((object)type == typeof(bool))
				{
					Expression = ((!Conversions.ToBoolean(Expression)) ? ((object)0.0) : ((object)(-1.0)));
				}
				else if (!Symbols.IsNumericType(type))
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(type), "Currency"));
				}
				return ((IFormattable)Expression).ToString(GetNumberFormatString(NumDigitsAfterDecimal, IncludeLeadingDigit, UseParensForNegativeNumbers, GroupDigits), null);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static string GetFormatString(int NumDigitsAfterDecimal, TriState IncludeLeadingDigit, TriState UseParensForNegativeNumbers, TriState GroupDigits, FormatType FormatTypeValue)
		{
			StringBuilder stringBuilder = new StringBuilder(30);
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)Utils.GetCultureInfo().GetFormat(typeof(NumberFormatInfo));
			if (NumDigitsAfterDecimal < -1)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
			if (NumDigitsAfterDecimal == -1)
			{
				switch (FormatTypeValue)
				{
					case FormatType.Percent:
						NumDigitsAfterDecimal = numberFormatInfo.NumberDecimalDigits;
						break;
					case FormatType.Number:
						NumDigitsAfterDecimal = numberFormatInfo.NumberDecimalDigits;
						break;
					case FormatType.Currency:
						NumDigitsAfterDecimal = numberFormatInfo.CurrencyDecimalDigits;
						break;
				}
			}
			if (GroupDigits == TriState.UseDefault)
			{
				GroupDigits = TriState.True;
				switch (FormatTypeValue)
				{
					case FormatType.Percent:
						if (IsArrayEmpty(numberFormatInfo.PercentGroupSizes))
						{
							GroupDigits = TriState.False;
						}
						break;
					case FormatType.Number:
						if (IsArrayEmpty(numberFormatInfo.NumberGroupSizes))
						{
							GroupDigits = TriState.False;
						}
						break;
					case FormatType.Currency:
						if (IsArrayEmpty(numberFormatInfo.CurrencyGroupSizes))
						{
							GroupDigits = TriState.False;
						}
						break;
				}
			}
			if (UseParensForNegativeNumbers == TriState.UseDefault)
			{
				UseParensForNegativeNumbers = TriState.False;
				switch (FormatTypeValue)
				{
					case FormatType.Number:
						if (numberFormatInfo.NumberNegativePattern == 0)
						{
							UseParensForNegativeNumbers = TriState.True;
						}
						break;
					case FormatType.Currency:
						if (numberFormatInfo.CurrencyNegativePattern == 0)
						{
							UseParensForNegativeNumbers = TriState.True;
						}
						break;
				}
			}
			string value = (GroupDigits != TriState.True) ? "" : "#,##";
			string value2 = (IncludeLeadingDigit == TriState.False) ? "#" : "0";
			string value3 = (NumDigitsAfterDecimal <= 0) ? "" : ("." + new string('0', NumDigitsAfterDecimal));
			if (FormatTypeValue == FormatType.Currency)
			{
				stringBuilder.Append(numberFormatInfo.CurrencySymbol);
			}
			stringBuilder.Append(value);
			stringBuilder.Append(value2);
			stringBuilder.Append(value3);
			if (FormatTypeValue == FormatType.Percent)
			{
				stringBuilder.Append(numberFormatInfo.PercentSymbol);
			}
			if (UseParensForNegativeNumbers == TriState.True)
			{
				string value4 = stringBuilder.ToString();
				stringBuilder.Append(";(");
				stringBuilder.Append(value4);
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}

		internal static string GetCurrencyFormatString(TriState IncludeLeadingDigit, int NumDigitsAfterDecimal, TriState UseParensForNegativeNumbers, TriState GroupDigits, ref IFormatProvider formatProvider)
		{
			string result = "C";
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)Utils.GetCultureInfo().GetFormat(typeof(NumberFormatInfo));
			numberFormatInfo = (NumberFormatInfo)numberFormatInfo.Clone();
			if (GroupDigits == TriState.False)
			{
				numberFormatInfo.CurrencyGroupSizes = new int[1];
			}
			int currencyPositivePattern = numberFormatInfo.CurrencyPositivePattern;
			int num = numberFormatInfo.CurrencyNegativePattern;
			switch (UseParensForNegativeNumbers)
			{
				case TriState.UseDefault:
					UseParensForNegativeNumbers = ((num == 0 || num == 4 || (uint)(num - 14) <= 1u) ? TriState.True : TriState.False);
					break;
				case TriState.False:
					switch (num)
					{
						case 0:
							num = 1;
							break;
						case 4:
							num = 5;
							break;
						case 14:
							num = 9;
							break;
						case 15:
							num = 10;
							break;
					}
					break;
				default:
					UseParensForNegativeNumbers = TriState.True;
					switch (num)
					{
						case 1:
						case 2:
						case 3:
							num = 0;
							break;
						case 5:
						case 6:
						case 7:
							num = 4;
							break;
						case 8:
						case 10:
						case 13:
							num = 15;
							break;
						case 9:
						case 11:
						case 12:
							num = 14;
							break;
					}
					break;
			}
			numberFormatInfo.CurrencyNegativePattern = num;
			if (NumDigitsAfterDecimal == -1)
			{
				NumDigitsAfterDecimal = numberFormatInfo.CurrencyDecimalDigits;
			}
			numberFormatInfo.CurrencyDecimalDigits = NumDigitsAfterDecimal;
			formatProvider = new FormatInfoHolder(numberFormatInfo);
			if (IncludeLeadingDigit == TriState.False)
			{
				numberFormatInfo.NumberGroupSizes = numberFormatInfo.CurrencyGroupSizes;
				string text = CurrencyPositiveFormatStrings[currencyPositivePattern] + ";" + CurrencyNegativeFormatStrings[num];
				string text2 = (GroupDigits == TriState.False) ? ((IncludeLeadingDigit != 0) ? "0" : "#") : ((IncludeLeadingDigit != 0) ? "#,##0" : "#,###");
				if (NumDigitsAfterDecimal > 0)
				{
					text2 = text2 + "." + new string('0', NumDigitsAfterDecimal);
				}
				if (string.CompareOrdinal("$", numberFormatInfo.CurrencySymbol) != 0)
				{
					text = text.Replace("$", numberFormatInfo.CurrencySymbol.Replace("'", "''"));
				}
				result = text.Replace("n", text2);
			}
			return result;
		}

		internal static string GetNumberFormatString(int NumDigitsAfterDecimal, TriState IncludeLeadingDigit, TriState UseParensForNegativeNumbers, TriState GroupDigits)
		{
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)Utils.GetCultureInfo().GetFormat(typeof(NumberFormatInfo));
			switch (NumDigitsAfterDecimal)
			{
				case -1:
					NumDigitsAfterDecimal = numberFormatInfo.NumberDecimalDigits;
					break;
				default:
					throw new ArgumentException(Utils.GetResourceString("Argument_Range0to99_1", "NumDigitsAfterDecimal"));
			}
			if (GroupDigits == TriState.UseDefault)
			{
				GroupDigits = ((numberFormatInfo.NumberGroupSizes != null && numberFormatInfo.NumberGroupSizes.Length != 0) ? TriState.True : TriState.False);
			}
			int num = numberFormatInfo.NumberNegativePattern;
			switch (UseParensForNegativeNumbers)
			{
				case TriState.UseDefault:
					UseParensForNegativeNumbers = ((num == 0) ? TriState.True : TriState.False);
					break;
				case TriState.False:
					if (num == 0)
					{
						num = 1;
					}
					break;
				default:
					UseParensForNegativeNumbers = TriState.True;
					if ((uint)(num - 1) <= 3u)
					{
						num = 0;
					}
					break;
			}
			if (UseParensForNegativeNumbers == TriState.UseDefault)
			{
				UseParensForNegativeNumbers = TriState.True;
			}
			string text = "n;" + NumberNegativeFormatStrings[num];
			if (string.CompareOrdinal("-", numberFormatInfo.NegativeSign) != 0)
			{
				text = text.Replace("-", "\"" + numberFormatInfo.NegativeSign + "\"");
			}
			string text2 = (IncludeLeadingDigit == TriState.False) ? "#" : "0";
			checked
			{
				if (GroupDigits != 0 && numberFormatInfo.NumberGroupSizes.Length != 0)
				{
					if (numberFormatInfo.NumberGroupSizes.Length == 1)
					{
						text2 = "#," + new string('#', numberFormatInfo.NumberGroupSizes[0]) + text2;
					}
					else
					{
						text2 = new string('#', numberFormatInfo.NumberGroupSizes[0] - 1) + text2;
						int upperBound = numberFormatInfo.NumberGroupSizes.GetUpperBound(0);
						for (int i = 1; i <= upperBound; i++)
						{
							text2 = "," + new string('#', numberFormatInfo.NumberGroupSizes[i]) + "," + text2;
						}
					}
				}
				if (NumDigitsAfterDecimal > 0)
				{
					text2 = text2 + "." + new string('0', NumDigitsAfterDecimal);
				}
				return Replace(text, "n", text2);
			}
		}

		/// <summary>Returns an expression formatted as a percentage (that is, multiplied by 100) with a trailing % character.</summary>
		/// <returns>Returns an expression formatted as a percentage (that is, multiplied by 100) with a trailing % character.</returns>
		/// <param name="Expression">Required. Expression to be formatted.</param>
		/// <param name="NumDigitsAfterDecimal">Optional. Numeric value indicating how many places to the right of the decimal are displayed. Default value is –1, which indicates that the locale settings are used.</param>
		/// <param name="IncludeLeadingDigit">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether or not a leading zero displays for fractional values. See "Settings" for values.</param>
		/// <param name="UseParensForNegativeNumbers">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether or not to place negative values within parentheses. See "Settings" for values.</param>
		/// <param name="GroupDigits">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether or not numbers are grouped using the group delimiter specified in the locale settings. See "Settings" for values.</param>
		/// <exception cref="T:System.InvalidCastException">Type is not numeric.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string FormatPercent(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
		{
			ValidateTriState(IncludeLeadingDigit);
			ValidateTriState(UseParensForNegativeNumbers);
			ValidateTriState(GroupDigits);
			if (Expression == null)
			{
				return "";
			}
			Type type = Expression.GetType();
			if ((object)type == typeof(string))
			{
				Expression = Conversions.ToDouble(Expression);
			}
			else if (!Symbols.IsNumericType(type))
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(type), "numeric"));
			}
			IFormattable obj = (IFormattable)Expression;
			string formatString = GetFormatString(NumDigitsAfterDecimal, IncludeLeadingDigit, UseParensForNegativeNumbers, GroupDigits, FormatType.Percent);
			return obj.ToString(formatString, null);
		}

		/// <summary>Returns a Char value representing the character from the specified index in the supplied string.</summary>
		/// <returns>Char value representing the character from the specified index in the supplied string.</returns>
		/// <param name="str">Required. Any valid String expression.</param>
		/// <param name="Index">Required. Integer expression. The (1-based) index of the character in <paramref name="str" /> to be returned.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="str" /> is Nothing, <paramref name="Index" /> &lt; 1, or <paramref name="Index" /> is greater than index of last character of <paramref name="str" />.</exception>
		/// <filterpriority>1</filterpriority>
		private static void ValidateTriState(TriState Param)
		{
			if (Param != TriState.True && Param != 0 && Param != TriState.UseDefault)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
		}
		private static bool IsArrayEmpty(Array array)
		{
			if (array == null)
				return true;

			return array.Length == 0;
		}

		public static int Asc(char String)
		{
			int num = Convert.ToInt32(String);
			if (num < 128)
			{
				return num;
			}
			try
			{
				Encoding fileIOEncoding = Utils.GetFileIOEncoding();
				char[] chars = new char[1]
				{
			String
				};
				byte[] array;
				if (fileIOEncoding.IsSingleByte)
				{
					array = new byte[1];
					fileIOEncoding.GetBytes(chars, 0, 1, array, 0);
					return array[0];
				}
				array = new byte[2];
				if (fileIOEncoding.GetBytes(chars, 0, 1, array, 0) == 1)
				{
					return array[0];
				}
				if (BitConverter.IsLittleEndian)
				{
					byte b = array[0];
					array[0] = array[1];
					array[1] = b;
				}
				return BitConverter.ToInt16(array, 0);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns an Integer value representing the character code corresponding to a character.</summary>
		/// <returns>Returns an Integer value representing the character code corresponding to a character.</returns>
		/// <param name="String">Required. Any valid Char or String expression. If <paramref name="String" /> is a String expression, only the first character of the string is used for input. If <paramref name="String" /> is Nothing or contains no characters, an <see cref="T:System.ArgumentException" /> error occurs.</param>
		/// <filterpriority>1</filterpriority>
		public static int Asc(string String)
		{
			if (String == null || String.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", "String"));
			}
			return Asc(String[0]);
		}

		/// <summary>Returns an Integer value representing the character code corresponding to a character.</summary>
		/// <returns>Returns an Integer value representing the character code corresponding to a character.</returns>
		/// <param name="String">Required. Any valid Char or String expression. If <paramref name="String" /> is a String expression, only the first character of the string is used for input. If <paramref name="String" /> is Nothing or contains no characters, an <see cref="T:System.ArgumentException" /> error occurs.</param>
		/// <filterpriority>1</filterpriority>
		public static int AscW(string String)
		{
			if (String == null || String.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", "String"));
			}
			return String[0];
		}
		
		/// <summary>Returns an Integer value representing the character code corresponding to a character.</summary>
		/// <returns>Returns an Integer value representing the character code corresponding to a character.</returns>
		/// <param name="String">Required. Any valid Char or String expression. If <paramref name="String" /> is a String expression, only the first character of the string is used for input. If <paramref name="String" /> is Nothing or contains no characters, an <see cref="T:System.ArgumentException" /> error occurs.</param>
		/// <filterpriority>1</filterpriority>
		public static int AscW(char String)
		{
			return String;
		}

		/// <summary>Returns the character associated with the specified character code.</summary>
		/// <returns>Returns the character associated with the specified character code.</returns>
		/// <param name="CharCode">Required. An Integer expression representing the <paramref name="code point" />, or character code, for the character.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="CharCode" /> &lt; 0 or &gt; 255 for Chr.</exception>
		/// <filterpriority>1</filterpriority>
		public static char Chr(int CharCode)
		{
			if (CharCode < -32768 || CharCode > 65535)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_RangeTwoBytes1", "CharCode"));
			}
			if (CharCode >= 0 && CharCode <= 127)
			{
				return Convert.ToChar(CharCode);
			}
			checked
			{
				try
				{
					Encoding encoding = Encoding.GetEncoding(Utils.GetLocaleCodePage());
					if (encoding.IsSingleByte && (CharCode < 0 || CharCode > 255))
					{
						throw ExceptionUtils.VbMakeException(5);
					}
					char[] array = new char[2];
					byte[] array2 = new byte[2];
					Decoder decoder = encoding.GetDecoder();
					if (CharCode >= 0 && CharCode <= 255)
					{
						array2[0] = (byte)(CharCode & 0xFF);
						decoder.GetChars(array2, 0, 1, array, 0);
					}
					else
					{
						array2[0] = (byte)((CharCode & 0xFF00) >> 8);
						array2[1] = (byte)(CharCode & 0xFF);
						decoder.GetChars(array2, 0, 2, array, 0);
					}
					return array[0];
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		/// <summary>Returns the character associated with the specified character code.</summary>
		/// <returns>Returns the character associated with the specified character code.</returns>
		/// <param name="CharCode">Required. An Integer expression representing the <paramref name="code point" />, or character code, for the character. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="CharCode" /> &lt; -32768 or &gt; 65535 for ChrW.</exception>
		/// <filterpriority>1</filterpriority>
		public static char ChrW(int CharCode)
		{
			if (CharCode < -32768 || CharCode > 65535)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_RangeTwoBytes1", "CharCode"));
			}
			return Convert.ToChar(CharCode & 0xFFFF);
		}

		/// <summary>Returns a string containing a specified number of characters from the left side of a string.</summary>
		/// <returns>Returns a string containing a specified number of characters from the left side of a string.</returns>
		/// <param name="str">Required. String expression from which the leftmost characters are returned.</param>
		/// <param name="Length">Required. Integer expression. Numeric expression indicating how many characters to return. If 0, a zero-length string ("") is returned. If greater than or equal to the number of characters in <paramref name="str" />, the entire string is returned.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Length" /> &lt; 0.</exception>
		/// <filterpriority>1</filterpriority>
		public static string Left(string str, int Length)
		{
			if (Length < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", "Length"));
			}
			if (Length == 0 || str == null)
			{
				return "";
			}
			if (Length >= str.Length)
			{
				return str;
			}
			return str.Substring(0, Length);
		}

		/// <summary>Returns a string containing a specified number of characters from a string.</summary>
		/// <returns>Returns a string containing a specified number of characters from a string.</returns>
		/// <param name="str">Required. String expression from which characters are returned.</param>
		/// <param name="Start">Required. Integer expression. Starting position of the characters to return. If <paramref name="Start" /> is greater than the number of characters in <paramref name="str" />, the Mid function returns a zero-length string (""). <paramref name="Start" /> is one based.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Start" /> &lt;= 0 or <paramref name="Length" /> &lt; 0.</exception>
		/// <filterpriority>1</filterpriority>
		public static string Mid(string str, int Start)
		{
			try
			{
				if (str == null)
				{
					return null;
				}
				return Mid(str, Start, str.Length);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns a string containing a specified number of characters from a string.</summary>
		/// <returns>Returns a string containing a specified number of characters from a string.</returns>
		/// <param name="str">Required. String expression from which characters are returned.</param>
		/// <param name="Start">Required. Integer expression. Starting position of the characters to return. If <paramref name="Start" /> is greater than the number of characters in <paramref name="str" />, the Mid function returns a zero-length string (""). <paramref name="Start" /> is one based.</param>
		/// <param name="Length">Optional. Integer expression. Number of characters to return. If omitted or if there are fewer than <paramref name="Length" /> characters in the text (including the character at position <paramref name="Start" />), all characters from the start position to the end of the string are returned.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Start" /> &lt;= 0 or <paramref name="Length" /> &lt; 0.</exception>
		/// <filterpriority>1</filterpriority>
		public static string Mid(string str, int Start, int Length)
		{
			if (Start <= 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GTZero1", "Start"));
			}
			if (Length < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", "Length"));
			}
			if (Length == 0 || str == null)
			{
				return "";
			}
			int length = str.Length;
			if (Start > length)
			{
				return "";
			}
			checked
			{
				if (Start + Length > length)
				{
					return str.Substring(Start - 1);
				}
				return str.Substring(Start - 1, Length);
			}
		}

		/// <summary>Returns a string containing a specified number of characters from the right side of a string.</summary>
		/// <returns>Returns a string containing a specified number of characters from the right side of a string.</returns>
		/// <param name="str">Required. String expression from which the rightmost characters are returned.</param>
		/// <param name="Length">Required. Integer. Numeric expression indicating how many characters to return. If 0, a zero-length string ("") is returned. If greater than or equal to the number of characters in <paramref name="str" />, the entire string is returned.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Length" /> &lt; 0.</exception>
		/// <filterpriority>1</filterpriority>
		public static string Right(string str, int Length)
		{
			if (Length < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", "Length"));
			}
			if (Length == 0 || str == null)
			{
				return "";
			}
			int length = str.Length;
			if (Length >= length)
			{
				return str;
			}
			return str.Substring(checked(length - Length), Length);
		}

		/// <summary>Returns a string containing a copy of a specified string with no leading spaces (LTrim), no trailing spaces (RTrim), or no leading or trailing spaces (Trim).</summary>
		/// <returns>Returns a string containing a copy of a specified string with no leading spaces (LTrim), no trailing spaces (RTrim), or no leading or trailing spaces (Trim).</returns>
		/// <param name="str">Required. Any valid String expression.</param>
		/// <filterpriority>1</filterpriority>
		public static string LTrim(string str)
		{
			if (str == null || str.Length == 0)
			{
				return "";
			}
			char c = str[0];
			if (c == ' ' || c == '\u3000')
			{
				return str.TrimStart(m_achIntlSpace);
			}
			return str;
		}

		/// <summary>Returns a string containing a copy of a specified string with no leading spaces (LTrim), no trailing spaces (RTrim), or no leading or trailing spaces (Trim).</summary>
		/// <returns>Returns a string containing a copy of a specified string with no leading spaces (LTrim), no trailing spaces (RTrim), or no leading or trailing spaces (Trim).</returns>
		/// <param name="str">Required. Any valid String expression.</param>
		/// <filterpriority>1</filterpriority>
		public static string Trim(string str)
		{
			try
			{
				if (str == null || str.Length == 0)
				{
					return "";
				}
				char c = str[0];
				if (c == ' ' || c == '\u3000')
				{
					return str.Trim(m_achIntlSpace);
				}
				c = str[checked(str.Length - 1)];
				if (c == ' ' || c == '\u3000')
				{
					return str.Trim(m_achIntlSpace);
				}
				return str;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns a string containing a copy of a specified string with no leading spaces (LTrim), no trailing spaces (RTrim), or no leading or trailing spaces (Trim).</summary>
		/// <returns>Returns a string containing a copy of a specified string with no leading spaces (LTrim), no trailing spaces (RTrim), or no leading or trailing spaces (Trim).</returns>
		/// <param name="str">Required. Any valid String expression.</param>
		/// <filterpriority>1</filterpriority>
		public static string RTrim(string str)
		{
			try
			{
				if (str == null || str.Length == 0)
				{
					return "";
				}
				char c = str[checked(str.Length - 1)];
				if (c == ' ' || c == '\u3000')
				{
					return str.TrimEnd(m_achIntlSpace);
				}
				return str;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static string LCase(string Value)
		{
			try
			{
				if (Value == null)
				{
					return null;
				}
				return Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns a string or character converted to lowercase.</summary>
		/// <returns>Returns a string or character converted to lowercase.</returns>
		/// <param name="Value">Required. Any valid String or Char expression.</param>
		/// <filterpriority>1</filterpriority>
		public static char LCase(char Value)
		{
			try
			{
				return Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns a string or character containing the specified string converted to uppercase.</summary>
		/// <returns>Returns a string or character containing the specified string converted to uppercase.</returns>
		/// <param name="Value">Required. Any valid String or Char expression.</param>
		/// <filterpriority>1</filterpriority>
		public static string UCase(string Value)
		{
			try
			{
				if (Value == null)
				{
					return "";
				}
				return Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns a string or character containing the specified string converted to uppercase.</summary>
		/// <returns>Returns a string or character containing the specified string converted to uppercase.</returns>
		/// <param name="Value">Required. Any valid String or Char expression.</param>
		/// <filterpriority>1</filterpriority>
		public static char UCase(char Value)
		{
			try
			{
				return Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(bool Expression)
		{
			return 2;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static int Len(sbyte Expression)
		{
			return 1;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(byte Expression)
		{
			return 1;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(short Expression)
		{
			return 2;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static int Len(ushort Expression)
		{
			return 2;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(int Expression)
		{
			return 4;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static int Len(uint Expression)
		{
			return 4;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(long Expression)
		{
			return 8;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static int Len(ulong Expression)
		{
			return 8;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(decimal Expression)
		{
			return 8;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(float Expression)
		{
			return 4;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(double Expression)
		{
			return 8;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(DateTime Expression)
		{
			return 8;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(char Expression)
		{
			return 2;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		public static int Len(string Expression)
		{
			return Expression?.Length ?? 0;
		}

		/// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
		/// <returns>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
		/// <param name="Expression">Any valid String expression or variable name. If <paramref name="Expression" /> is of type Object, the Len function returns the size as it will be written to the file by the FilePut function.</param>
		/// <filterpriority>1</filterpriority>
		[SecuritySafeCritical]
		public static int Len(object Expression)
		{
			if (Expression == null)
			{
				return 0;
			}
			IConvertible convertible = Expression as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
					case TypeCode.Boolean:
						return 2;
					case TypeCode.SByte:
						return 1;
					case TypeCode.Byte:
						return 1;
					case TypeCode.Int16:
						return 2;
					case TypeCode.UInt16:
						return 2;
					case TypeCode.Int32:
						return 4;
					case TypeCode.UInt32:
						return 4;
					case TypeCode.Int64:
						return 8;
					case TypeCode.UInt64:
						return 8;
					case TypeCode.Decimal:
						return 16;
					case TypeCode.Single:
						return 4;
					case TypeCode.Double:
						return 8;
					case TypeCode.DateTime:
						return 8;
					case TypeCode.Char:
						return 2;
					case TypeCode.String:
						return Expression.ToString().Length;
				}
			}
			else
			{
				char[] array = Expression as char[];
				if (array != null)
				{
					return array.Length;
				}
			}
			if (Expression is ValueType)
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
				int recordLength = StructUtils.GetRecordLength(Expression, 1);
				PermissionSet.RevertAssert();
				return recordLength;
			}
			throw ExceptionUtils.VbMakeException(13);
		}

		/// <summary>Returns a string consisting of the specified number of spaces.</summary>
		/// <returns>Returns a string consisting of the specified number of spaces.</returns>
		/// <param name="Number">Required. Integer expression. The number of spaces you want in the string.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> &lt; 0.</exception>
		/// <filterpriority>1</filterpriority>
		public static string Space(int Number)
		{
			if (Number >= 0)
			{
				return new string(' ', Number);
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", "Number"));
		}

		/// <summary>Returns a string or object consisting of the specified character repeated the specified number of times.</summary>
		/// <returns>Returns a string or object consisting of the specified character repeated the specified number of times.</returns>
		/// <param name="Number">Required. Integer expression. The length to the string to be returned.</param>
		/// <param name="Character">Required. Any valid Char, String, or Object expression. Only the first character of the expression will be used. If Character is of type Object, it must contain either a Char or a String value. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is less than 0 or <paramref name="Character" /> type is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Character" /> is Nothing.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static object StrDup(int Number, object Character)
		{
			if (Number < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Number"));
			}
			if (Character == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", "Character"));
			}
			string text = Character as string;
			char c;
			if (text != null)
			{
				if (text.Length == 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", "Character"));
				}
				c = text[0];
			}
			else
			{
				try
				{
					c = Conversions.ToChar(Character);
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
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Character"));
				}
			}
			return new string(c, Number);
		}

		/// <summary>Returns a string or object consisting of the specified character repeated the specified number of times.</summary>
		/// <returns>Returns a string or object consisting of the specified character repeated the specified number of times.</returns>
		/// <param name="Number">Required. Integer expression. The length to the string to be returned.</param>
		/// <param name="Character">Required. Any valid Char, String, or Object expression. Only the first character of the expression will be used. If Character is of type Object, it must contain either a Char or a String value. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is less than 0 or <paramref name="Character" /> type is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Character" /> is Nothing.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string StrDup(int Number, char Character)
		{
			if (Number < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", "Number"));
			}
			return new string(Character, Number);
		}

		/// <summary>Returns a string or object consisting of the specified character repeated the specified number of times.</summary>
		/// <returns>Returns a string or object consisting of the specified character repeated the specified number of times.</returns>
		/// <param name="Number">Required. Integer expression. The length to the string to be returned.</param>
		/// <param name="Character">Required. Any valid Char, String, or Object expression. Only the first character of the expression will be used. If Character is of type Object, it must contain either a Char or a String value. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Number" /> is less than 0 or <paramref name="Character" /> type is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="Character" /> is Nothing.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string StrDup(int Number, string Character)
		{
			if (Number < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", "Number"));
			}
			if (Character == null || Character.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", "Character"));
			}
			return new string(Character[0], Number);
		}

		/// <summary>Returns -1, 0, or 1, based on the result of a string comparison. </summary>
		/// <returns>The StrComp function has the following return values.IfStrComp returns<paramref name="String1" /> sorts ahead of <paramref name="String2" />-1<paramref name="String1" /> is equal to <paramref name="String2" /> 0<paramref name="String1" /> sorts after <paramref name="String2" /> 1</returns>
		/// <param name="String1">Required. Any valid String expression.</param>
		/// <param name="String2">Required. Any valid String expression.</param>
		/// <param name="Compare">Optional. Specifies the type of string comparison. If <paramref name="Compare" /> is omitted, the Option Compare setting determines the type of comparison.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Compare" /> value is not valid.</exception>
		/// <filterpriority>1</filterpriority>
		public static int StrComp(string String1, string String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			try
			{
				switch (Compare)
				{
					case CompareMethod.Binary:
						return CompareString(String1, String2, TextCompare: false);
					case CompareMethod.Text:
						return CompareString(String1, String2, TextCompare: true);
					default:
						throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Compare"));
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static int CompareString(string Left, string Right, bool TextCompare)
		{
			if ((object)Left == Right)
			{
				return 0;
			}
			if (Left == null)
			{
				if (Right.Length == 0)
				{
					return 0;
				}
				return -1;
			}
			if (Right == null)
			{
				if (Left.Length == 0)
				{
					return 0;
				}
				return 1;
			}
			int num = (!TextCompare) ? string.CompareOrdinal(Left, Right) : Utils.GetCultureInfo().CompareInfo.Compare(Left, Right, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
			if (num == 0)
			{
				return 0;
			}
			if (num > 0)
			{
				return 1;
			}
			return -1;
		}

		/// <summary>Returns a string converted as specified.</summary>
		/// <returns>Returns a string converted as specified.</returns>
		/// <param name="str">Required. String expression to be converted.</param>
		/// <param name="Conversion">Required. <see cref="T:Microsoft.VisualBasic.VbStrConv" /> member. The enumeration value specifying the type of conversion to perform.</param>
		/// <param name="LocaleID">Optional. The LocaleID value, if different from the system LocaleID value. (The system LocaleID value is the default.)</param>
		/// <exception cref="T:System.ArgumentException">Unsupported <paramref name="LocaleID" />, <paramref name="Conversion" /> &lt; 0 or &gt; 2048, or unsupported conversion for specified locale.</exception>
		/// <filterpriority>1</filterpriority>
		public static string StrConv(string str, VbStrConv Conversion, int LocaleID = 0)
		{
			try
			{
				CultureInfo cultureInfo;
				if (LocaleID == 0 || LocaleID == 1)
				{
					cultureInfo = Utils.GetCultureInfo();
					LocaleID = cultureInfo.LCID;
				}
				else
				{
					try
					{
						cultureInfo = new CultureInfo(LocaleID & 0xFFFF);
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
						throw new ArgumentException(Utils.GetResourceString("Argument_LCIDNotSupported1", Conversions.ToString(LocaleID)));
					}
				}
				int num = PRIMARYLANGID(LocaleID);
				if ((Conversion & ~(VbStrConv.Uppercase | VbStrConv.Lowercase | VbStrConv.Wide | VbStrConv.Narrow | VbStrConv.Katakana | VbStrConv.Hiragana | VbStrConv.SimplifiedChinese | VbStrConv.TraditionalChinese | VbStrConv.LinguisticCasing)) != 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidVbStrConv"));
				}
				int num2 = default(int);
				switch (Conversion & (VbStrConv.SimplifiedChinese | VbStrConv.TraditionalChinese))
				{
					case VbStrConv.SimplifiedChinese | VbStrConv.TraditionalChinese:
						throw new ArgumentException(Utils.GetResourceString("Argument_StrConvSCandTC"));
					case VbStrConv.SimplifiedChinese:
						if (!IsValidCodePage(936) || !IsValidCodePage(950))
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_SCNotSupported"));
						}
						num2 |= 0x2000000;
						break;
					case VbStrConv.TraditionalChinese:
						if (!IsValidCodePage(936) || !IsValidCodePage(950))
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_TCNotSupported"));
						}
						num2 |= 0x4000000;
						break;
				}
				switch (Conversion & VbStrConv.ProperCase)
				{
					case VbStrConv.None:
						if ((Conversion & VbStrConv.LinguisticCasing) != 0)
						{
							throw new ArgumentException(Utils.GetResourceString("LinguisticRequirements"));
						}
						break;
					case VbStrConv.ProperCase:
						num2 = 0;
						break;
					case VbStrConv.Uppercase:
						if (Conversion == VbStrConv.Uppercase)
						{
							return cultureInfo.TextInfo.ToUpper(str);
						}
						num2 |= 0x200;
						break;
					case VbStrConv.Lowercase:
						if (Conversion == VbStrConv.Lowercase)
						{
							return cultureInfo.TextInfo.ToLower(str);
						}
						num2 |= 0x100;
						break;
				}
				if ((Conversion & (VbStrConv.Katakana | VbStrConv.Hiragana)) != 0 && (num != 17 || !ValidLCID(LocaleID)))
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_JPNNotSupported"));
				}
				if ((Conversion & (VbStrConv.Wide | VbStrConv.Narrow)) != 0)
				{
					if (num != 17 && num != 18 && num != 4)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_WideNarrowNotApplicable"));
					}
					if (!ValidLCID(LocaleID))
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_LocalNotSupported"));
					}
				}
				switch (Conversion & (VbStrConv.Wide | VbStrConv.Narrow))
				{
					case VbStrConv.Wide | VbStrConv.Narrow:
						throw new ArgumentException(Utils.GetResourceString("Argument_IllegalWideNarrow"));
					case VbStrConv.Wide:
						num2 |= 0x800000;
						break;
					case VbStrConv.Narrow:
						num2 |= 0x400000;
						break;
				}
				switch (Conversion & (VbStrConv.Katakana | VbStrConv.Hiragana))
				{
					case VbStrConv.Katakana | VbStrConv.Hiragana:
						throw new ArgumentException(Utils.GetResourceString("Argument_IllegalKataHira"));
					case VbStrConv.Katakana:
						num2 |= 0x200000;
						break;
					case VbStrConv.Hiragana:
						num2 |= 0x100000;
						break;
				}
				if ((Conversion & VbStrConv.ProperCase) == VbStrConv.ProperCase)
				{
					return ProperCaseString(cultureInfo, num2, str);
				}
				//if (num2 != 0)
				//{
				//	return vbLCMapString(cultureInfo, num2, str);
				//}
				return str;
			}
			catch (Exception ex5)
			{
				throw ex5;
			}
		}

		private static int PRIMARYLANGID(int lcid)
		{
			return lcid & 0x3FF;
		}
		internal static bool IsValidCodePage(int codepage)
		{
			bool result = false;
			try
			{
				if (Encoding.GetEncoding(codepage) == null)
				{
					return result;
				}
				result = true;
				return result;
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
				return result;
			}
		}
		internal static bool ValidLCID(int LocaleID)
		{
			try
			{
				new CultureInfo(LocaleID);
				return true;
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
		}
		private static string ProperCaseString(CultureInfo loc, int dwMapFlags, string sSrc)
		{
			if ((sSrc?.Length ?? 0) == 0)
			{
				return "";
			}
			return loc.TextInfo.ToTitleCase(sSrc);

			//StringBuilder stringBuilder = new StringBuilder(vbLCMapString(loc, dwMapFlags | 0x100, sSrc));
			//return loc.TextInfo.ToTitleCase(stringBuilder.ToString());
		}

		/// <summary>Returns a string created by joining a number of substrings contained in an array.</summary>
		/// <returns>Returns a string created by joining a number of substrings contained in an array.</returns>
		/// <param name="SourceArray">Required. One-dimensional array containing substrings to be joined.</param>
		/// <param name="Delimiter">Optional. Any string, used to separate the substrings in the returned string. If omitted, the space character (" ") is used. If <paramref name="Delimiter" /> is a zero-length string ("") or Nothing, all items in the list are concatenated with no delimiters.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="SourceArray" /> is not one dimensional.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Join(object[] SourceArray, string Delimiter = " ")
		{
			int num = Information.UBound(SourceArray);
			checked
			{
				string[] array = new string[num + 1];
				try
				{
					int num2 = num;
					for (int i = 0; i <= num2; i++)
					{
						array[i] = Conversions.ToString(SourceArray[i]);
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
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", "SourceArray", "String"));
				}
				return Join(array, Delimiter);
			}
		}

		/// <summary>Returns a string created by joining a number of substrings contained in an array.</summary>
		/// <returns>Returns a string created by joining a number of substrings contained in an array.</returns>
		/// <param name="SourceArray">Required. One-dimensional array containing substrings to be joined.</param>
		/// <param name="Delimiter">Optional. Any string, used to separate the substrings in the returned string. If omitted, the space character (" ") is used. If <paramref name="Delimiter" /> is a zero-length string ("") or Nothing, all items in the list are concatenated with no delimiters.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="SourceArray" /> is not one dimensional.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string Join(string[] SourceArray, string Delimiter = " ")
		{
			try
			{
				if (IsArrayEmpty(SourceArray))
				{
					return null;
				}
				if (SourceArray.Rank != 1)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_RankEQOne1"));
				}
				return string.Join(Delimiter, SourceArray);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Returns a zero-based, one-dimensional array containing a specified number of substrings.</summary>
		/// <returns>String array. If <paramref name="Expression" /> is a zero-length string (""), Split returns a single-element array containing a zero-length string. If <paramref name="Delimiter" /> is a zero-length string, or if it does not appear anywhere in <paramref name="Expression" />, Split returns a single-element array containing the entire <paramref name="Expression" /> string.</returns>
		/// <param name="Expression">Required. String expression containing substrings and delimiters.</param>
		/// <param name="Delimiter">Optional. Any single character used to identify substring limits. If <paramref name="Delimiter" /> is omitted, the space character (" ") is assumed to be the delimiter.</param>
		/// <param name="Limit">Optional. Maximum number of substrings into which the input string should be split. The default, –1, indicates that the input string should be split at every occurrence of the <paramref name="Delimiter" /> string.</param>
		/// <param name="Compare">Optional. Numeric value indicating the comparison to use when evaluating substrings. See "Settings" for values.</param>
		/// <filterpriority>1</filterpriority>
		public static string[] Split(string Expression, string Delimiter = " ", int Limit = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			try
			{
				if (Expression == null || Expression.Length == 0)
				{
					return new string[1]
					{
				""
					};
				}
				if (Limit == -1)
				{
					Limit = checked(Expression.Length + 1);
				}
				if ((Delimiter?.Length ?? 0) == 0)
				{
					return new string[1]
					{
				Expression
					};
				}
				return SplitHelper(Expression, Delimiter, Limit, (int)Compare);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static string[] SplitHelper(string sSrc, string sFind, int cMaxSubStrings, int Compare)
		{
			int num = sFind?.Length ?? 0;
			int num2 = sSrc?.Length ?? 0;
			if (num == 0)
			{
				return new string[1]
				{
			sSrc
				};
			}
			if (num2 == 0)
			{
				return new string[1]
				{
			sSrc
				};
			}
			int num3 = 20;
			if (num3 > cMaxSubStrings)
			{
				num3 = cMaxSubStrings;
			}
			checked
			{
				string[] array = new string[num3 + 1];
				CompareOptions options;
				CompareInfo compareInfo;
				if (Compare == 0)
				{
					options = CompareOptions.Ordinal;
					compareInfo = m_InvariantCompareInfo;
				}
				else
				{
					compareInfo = Utils.GetCultureInfo().CompareInfo;
					options = (CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
				}
				int num4 = default(int);
				int num6 = default(int);
				while (num4 < num2)
				{
					int num5 = compareInfo.IndexOf(sSrc, sFind, num4, num2 - num4, options);
					string text;
					if (num5 == -1 || num6 + 1 == cMaxSubStrings)
					{
						text = sSrc.Substring(num4);
						if (text == null)
						{
							text = "";
						}
						array[num6] = text;
						break;
					}
					text = sSrc.Substring(num4, num5 - num4);
					if (text == null)
					{
						text = "";
					}
					array[num6] = text;
					num4 = num5 + num;
					num6++;
					if (num6 > num3)
					{
						num3 += 20;
						if (num3 > cMaxSubStrings)
						{
							num3 = cMaxSubStrings + 1;
						}
						array = (string[])Utils.CopyArray(array, new string[num3 + 1]);
					}
					array[num6] = "";
					if (num6 == cMaxSubStrings)
					{
						text = sSrc.Substring(num4);
						if (text == null)
						{
							text = "";
						}
						array[num6] = text;
						break;
					}
				}
				if (num6 + 1 == array.Length)
				{
					return array;
				}
				return (string[])Utils.CopyArray(array, new string[num6 + 1]);
			}
		}

		/// <summary>Returns a string in which a specified substring has been replaced with another substring a specified number of times.</summary>
		/// <returns>Replace returns the following values.IfReplace returns<paramref name="Find" /> is zero-length or NothingCopy of <paramref name="Expression" /><paramref name="Replace" /> is zero-lengthCopy of <paramref name="Expression" /> with no occurrences of <paramref name="Find" /><paramref name="Expression" /> is zero-length or Nothing, or <paramref name="Start" /> is greater than length of <paramref name="Expression" />Nothing<paramref name="Count" /> is 0Copy of <paramref name="Expression" /></returns>
		/// <param name="Expression">Required. String expression containing substring to replace.</param>
		/// <param name="Find">Required. Substring being searched for.</param>
		/// <param name="Replacement">Required. Replacement substring.</param>
		/// <param name="Start">Optional. Position within <paramref name="Expression" /> that starts a substring used for replacement. The return value of Replace is a string that begins at <paramref name="Start" />, with appropriate substitutions. If omitted, 1 is assumed.</param>
		/// <param name="Count">Optional. Number of substring substitutions to perform. If omitted, the default value is –1, which means "make all possible substitutions."</param>
		/// <param name="Compare">Optional. Numeric value indicating the kind of comparison to use when evaluating substrings. See Settings for values.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Count" /> &lt; -1 or <paramref name="Start" /> &lt;= 0.</exception>
		/// <filterpriority>1</filterpriority>
		public static string Replace(string Expression, string Find, string Replacement, int Start = 1, int Count = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			try
			{
				if (Count < -1)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_GEMinusOne1", "Count"));
				}
				if (Start <= 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_GTZero1", "Start"));
				}
				if (Expression == null || Start > Expression.Length)
				{
					return null;
				}
				if (Start != 1)
				{
					Expression = Expression.Substring(checked(Start - 1));
				}
				if (Find == null || Find.Length == 0 || Count == 0)
				{
					return Expression;
				}
				if (Count == -1)
				{
					Count = Expression.Length;
				}
				return ReplaceInternal(Expression, Find, Replacement, Count, Compare);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		private static string ReplaceInternal(string Expression, string Find, string Replacement, int Count, CompareMethod Compare)
		{
			int length = Expression.Length;
			int length2 = Find.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			CompareInfo compareInfo;
			CompareOptions options;
			if (Compare == CompareMethod.Text)
			{
				compareInfo = Utils.GetCultureInfo().CompareInfo;
				options = (CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
			}
			else
			{
				compareInfo = m_InvariantCompareInfo;
				options = CompareOptions.Ordinal;
			}
			checked
			{
				int num = default(int);
				int num2 = default(int);
				while (num < length)
				{
					if (num2 == Count)
					{
						stringBuilder.Append(Expression.Substring(num));
						break;
					}
					int num3 = compareInfo.IndexOf(Expression, Find, num, options);
					if (num3 < 0)
					{
						stringBuilder.Append(Expression.Substring(num));
						break;
					}
					stringBuilder.Append(Expression.Substring(num, num3 - num));
					stringBuilder.Append(Replacement);
					num2++;
					num = num3 + length2;
				}
				return stringBuilder.ToString();
			}
		}

		/// <summary>Returns a string in which the character order of a specified string is reversed.</summary>
		/// <returns>Returns a string in which the character order of a specified string is reversed.</returns>
		/// <param name="Expression">Required. String expression whose characters are to be reversed. If <paramref name="Expression" /> is a zero-length string (""), a zero-length string is returned.</param>
		/// <filterpriority>1</filterpriority>
		public static string StrReverse(string Expression)
		{
			if (Expression == null)
			{
				return "";
			}
			int length = Expression.Length;
			if (length == 0)
			{
				return "";
			}
			checked
			{
				int num = length - 1;
				for (int i = 0; i <= num; i++)
				{
					UnicodeCategory unicodeCategory = char.GetUnicodeCategory(Expression[i]);
					if (unicodeCategory == UnicodeCategory.Surrogate || unicodeCategory == UnicodeCategory.NonSpacingMark || unicodeCategory == UnicodeCategory.SpacingCombiningMark || unicodeCategory == UnicodeCategory.EnclosingMark)
					{
						return InternalStrReverse(Expression, i, length);
					}
				}
				char[] array = Expression.ToCharArray();
				Array.Reverse(array);
				return new string(array);
			}
		}

		/// <summary>Returns an integer specifying the start position of the first occurrence of one string within another.</summary>
		/// <returns>IfInStr returns<paramref name="String1" /> is zero length or Nothing0<paramref name="String2" /> is zero length or Nothing<paramref name="start" /><paramref name="String2" /> is not found0<paramref name="String2" /> is found within <paramref name="String1" />Position where match begins</returns>
		/// <param name="String1">Required. String expression being searched.</param>
		/// <param name="String2">Required. String expression sought.</param>
		/// <param name="Compare">Optional. Specifies the type of string comparison. If <paramref name="Compare" /> is omitted, the Option Compare setting determines the type of comparison. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Start" /> &lt; 1.</exception>
		/// <filterpriority>1</filterpriority>
		public static int InStr(string String1, string String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			checked
			{
				if (Compare == CompareMethod.Binary)
				{
					return InternalInStrBinary(0, String1, String2) + 1;
				}
				return InternalInStrText(0, String1, String2) + 1;
			}
		}

		/// <summary>Returns an integer specifying the start position of the first occurrence of one string within another.</summary>
		/// <returns>IfInStr returns<paramref name="String1" /> is zero length or Nothing0<paramref name="String2" /> is zero length or Nothing<paramref name="start" /><paramref name="String2" /> is not found0<paramref name="String2" /> is found within <paramref name="String1" />Position where match begins<paramref name="Start" /> &gt; length of <paramref name="String1" />0</returns>
		/// <param name="Start">Optional. Numeric expression that sets the starting position for each search. If omitted, search begins at the first character position. The start index is 1-based.</param>
		/// <param name="String1">Required. String expression being searched.</param>
		/// <param name="String2">Required. String expression sought.</param>
		/// <param name="Compare">Optional. Specifies the type of string comparison. If <paramref name="Compare" /> is omitted, the Option Compare setting determines the type of comparison. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Start" /> &lt; 1.</exception>
		/// <filterpriority>1</filterpriority>
		public static int InStr(int Start, string String1, string String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			if (Start < 1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GTZero1", "Start"));
			}
			checked
			{
				if (Compare == CompareMethod.Binary)
				{
					return InternalInStrBinary(Start - 1, String1, String2) + 1;
				}
				return InternalInStrText(Start - 1, String1, String2) + 1;
			}
		}

		/// <summary>Returns the position of the first occurrence of one string within another, starting from the right side of the string.</summary>
		/// <returns>IfInStrRev returns<paramref name="StringCheck" /> is zero-length0<paramref name="StringMatch" /> is zero-length<paramref name="Start" /><paramref name="StringMatch" /> is not found0<paramref name="StringMatch" /> is found within <paramref name="StringCheck" />Position at which the first match is found, starting with the right side of the string.<paramref name="Start" /> is greater than length of <paramref name="StringMatch" />0</returns>
		/// <param name="StringCheck">Required. String expression being searched.</param>
		/// <param name="StringMatch">Required. String expression being searched for.</param>
		/// <param name="Start">Optional. Numeric expression setting the one-based starting position for each search, starting from the left side of the string. If <paramref name="Start" /> is omitted then –1 is used, meaning the search begins at the last character position. Search then proceeds from right to left.</param>
		/// <param name="Compare">Optional. Numeric value indicating the kind of comparison to use when evaluating substrings. If omitted, a binary comparison is performed. See Settings for values.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Start" /> = 0 or <paramref name="Start" /> &lt; -1.</exception>
		/// <filterpriority>1</filterpriority>
		public static int InStrRev(string StringCheck, string StringMatch, int Start = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			checked
			{
				try
				{
					if (Start == 0 || Start < -1)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_MinusOneOrGTZero1", "Start"));
					}
					int num = StringCheck?.Length ?? 0;
					if (Start == -1)
					{
						Start = num;
					}
					if (Start > num || num == 0)
					{
						return 0;
					}
					if (StringMatch == null || StringMatch.Length == 0)
					{
						return Start;
					}
					if (Compare == CompareMethod.Binary)
					{
						return m_InvariantCompareInfo.LastIndexOf(StringCheck, StringMatch, Start - 1, Start, CompareOptions.Ordinal) + 1;
					}
					return Utils.GetCultureInfo().CompareInfo.LastIndexOf(StringCheck, StringMatch, Start - 1, Start, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) + 1;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		private static int InternalInStrBinary(int StartPos, string sSrc, string sFind)
		{
			int num = sSrc?.Length ?? 0;
			if (StartPos > num || num == 0)
			{
				return -1;
			}
			if (sFind == null || sFind.Length == 0)
			{
				return StartPos;
			}
			return m_InvariantCompareInfo.IndexOf(sSrc, sFind, StartPos, CompareOptions.Ordinal);
		}

		private static int InternalInStrText(int lStartPos, string sSrc, string sFind)
		{
			int num = sSrc?.Length ?? 0;
			if (lStartPos > num || num == 0)
			{
				return -1;
			}
			if (sFind == null || sFind.Length == 0)
			{
				return lStartPos;
			}
			return Utils.GetCultureInfo().CompareInfo.IndexOf(sSrc, sFind, lStartPos, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
		}

		private static string InternalStrReverse(string Expression, int SrcIndex, int Length)
		{
			StringBuilder stringBuilder = new StringBuilder(Length);
			stringBuilder.Length = Length;
			TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(Expression, SrcIndex);
			if (!textElementEnumerator.MoveNext())
			{
				return "";
			}
			int i = 0;
			checked
			{
				int num = Length - 1;
				for (; i < SrcIndex; i++)
				{
					stringBuilder[num] = Expression[i];
					num--;
				}
				int num2 = textElementEnumerator.ElementIndex;
				while (num >= 0)
				{
					SrcIndex = num2;
					num2 = ((!textElementEnumerator.MoveNext()) ? Length : textElementEnumerator.ElementIndex);
					for (i = num2 - 1; i >= SrcIndex; i--)
					{
						stringBuilder[num] = Expression[i];
						num--;
					}
				}
				return stringBuilder.ToString();
			}
		}
	}
}
