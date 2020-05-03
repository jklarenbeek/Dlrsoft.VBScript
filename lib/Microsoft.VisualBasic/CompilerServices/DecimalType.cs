// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.DecimalType
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>This class has been deprecated as of Visual Basic 2005.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class DecimalType
  {
    /// <summary>Returns a <see langword="Decimal" /> value that corresponds to the specified <see langword="Boolean" /> value.</summary>
    /// <param name="Value">Required. <see langword="Boolean" /> value to convert to a <see langword="Decimal" /> value.</param>
    /// <returns>The <see langword="Decimal" /> value that corresponds to <paramref name="Value" />.</returns>
    public static Decimal FromBoolean(bool Value)
    {
      return !Value ? new Decimal() : Decimal.MinusOne;
    }

    /// <summary>Returns a <see langword="Decimal" /> value that corresponds to the specified string.</summary>
    /// <param name="Value">Required. String to convert to a <see langword="Decimal" /> value.</param>
    /// <returns>The <see langword="Decimal" /> value that corresponds to <paramref name="Value" />.</returns>
    public static Decimal FromString(string Value)
    {
      return DecimalType.FromString(Value, (NumberFormatInfo) null);
    }

    /// <summary>Returns a <see langword="Decimal" /> value that corresponds to the specified string and number format information.</summary>
    /// <param name="Value">Required. String to convert to a <see langword="Decimal" /> value.</param>
    /// <param name="NumberFormat">A <see cref="T:System.Globalization.NumberFormatInfo" /> object that defines how numeric values are formatted and displayed, depending on the culture.</param>
    /// <returns>The <see langword="Decimal" /> value that corresponds to <paramref name="Value" />.</returns>
    public static Decimal FromString(string Value, NumberFormatInfo NumberFormat)
    {
      Decimal num;
      if (Value == null)
      {
        num = new Decimal();
      }
      else
      {
        try
        {
          long i64Value;
          num = !Utils.IsHexOrOctValue(Value, ref i64Value) ? DecimalType.Parse(Value, NumberFormat) : new Decimal(i64Value);
        }
        catch (OverflowException ex)
        {
          throw ExceptionUtils.VbMakeException(6);
        }
        catch (FormatException ex)
        {
          throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Decimal"));
        }
      }
      return num;
    }

    /// <summary>Returns a <see langword="Decimal" /> value that corresponds to the specified object.</summary>
    /// <param name="Value">Required. Object to convert to a <see langword="Decimal" /> value.</param>
    /// <returns>The <see langword="Decimal" /> value that corresponds to <paramref name="Value" />.</returns>
    public static Decimal FromObject(object Value)
    {
      return DecimalType.FromObject(Value, (NumberFormatInfo) null);
    }

    /// <summary>Returns a <see langword="Decimal" /> value that corresponds to the specified object and number format information.</summary>
    /// <param name="Value">Required. Object to convert to a <see langword="Decimal" /> value.</param>
    /// <param name="NumberFormat">A <see cref="T:System.Globalization.NumberFormatInfo" /> object that defines how numeric values are formatted and displayed, depending on the culture.</param>
    /// <returns>The <see langword="Decimal" /> value that corresponds to <paramref name="Value" />.</returns>
    public static Decimal FromObject(object Value, NumberFormatInfo NumberFormat)
    {
      Decimal num;
      if (Value == null)
      {
        num = new Decimal();
      }
      else
      {
        if (Value is IConvertible convertible)
        {
          switch (convertible.GetTypeCode())
          {
            case TypeCode.Boolean:
              num = DecimalType.FromBoolean(convertible.ToBoolean((IFormatProvider) null));
              goto label_14;
            case TypeCode.Byte:
              num = new Decimal((int) convertible.ToByte((IFormatProvider) null));
              goto label_14;
            case TypeCode.Int16:
              num = new Decimal((int) convertible.ToInt16((IFormatProvider) null));
              goto label_14;
            case TypeCode.Int32:
              num = new Decimal(convertible.ToInt32((IFormatProvider) null));
              goto label_14;
            case TypeCode.Int64:
              num = new Decimal(convertible.ToInt64((IFormatProvider) null));
              goto label_14;
            case TypeCode.Single:
              num = new Decimal(convertible.ToSingle((IFormatProvider) null));
              goto label_14;
            case TypeCode.Double:
              num = new Decimal(convertible.ToDouble((IFormatProvider) null));
              goto label_14;
            case TypeCode.Decimal:
              num = convertible.ToDecimal((IFormatProvider) null);
              goto label_14;
            case TypeCode.String:
              num = DecimalType.FromString(convertible.ToString((IFormatProvider) null), NumberFormat);
              goto label_14;
          }
        }
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Decimal"));
      }
label_14:
      return num;
    }

    /// <summary>Returns a <see langword="Decimal" /> value that corresponds to the specified string and number format information.</summary>
    /// <param name="Value">Required. String to convert to a <see langword="Decimal" /> value.</param>
    /// <param name="NumberFormat">A <see cref="T:System.Globalization.NumberFormatInfo" /> object that defines how numeric values are formatted and displayed, depending on the culture.</param>
    /// <returns>The <see langword="Decimal" /> value that corresponds to <paramref name="Value" />.</returns>
    public static Decimal Parse(string Value, NumberFormatInfo NumberFormat)
    {
      CultureInfo cultureInfo = Utils.GetCultureInfo();
      if (NumberFormat == null)
        NumberFormat = cultureInfo.NumberFormat;
      NumberFormatInfo normalizedNumberFormat = DecimalType.GetNormalizedNumberFormat(NumberFormat);
      Value = Utils.ToHalfwidthNumbers(Value, cultureInfo);
      try
      {
        return Decimal.Parse(Value, NumberStyles.Any, (IFormatProvider) normalizedNumberFormat);
      }
      catch (FormatException ex) when (NumberFormat != normalizedNumberFormat)
      {
        return Decimal.Parse(Value, NumberStyles.Any, (IFormatProvider) NumberFormat);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    internal static NumberFormatInfo GetNormalizedNumberFormat(
      NumberFormatInfo InNumberFormat)
    {
      NumberFormatInfo numberFormatInfo1 = InNumberFormat;
      NumberFormatInfo numberFormatInfo2;
      if (numberFormatInfo1.CurrencyDecimalSeparator != null && numberFormatInfo1.NumberDecimalSeparator != null && (numberFormatInfo1.CurrencyGroupSeparator != null && numberFormatInfo1.NumberGroupSeparator != null) && (numberFormatInfo1.CurrencyDecimalSeparator.Length == 1 && numberFormatInfo1.NumberDecimalSeparator.Length == 1 && (numberFormatInfo1.CurrencyGroupSeparator.Length == 1 && numberFormatInfo1.NumberGroupSeparator.Length == 1)) && ((int) numberFormatInfo1.CurrencyDecimalSeparator[0] == (int) numberFormatInfo1.NumberDecimalSeparator[0] && (int) numberFormatInfo1.CurrencyGroupSeparator[0] == (int) numberFormatInfo1.NumberGroupSeparator[0] && numberFormatInfo1.CurrencyDecimalDigits == numberFormatInfo1.NumberDecimalDigits))
      {
        numberFormatInfo2 = InNumberFormat;
      }
      else
      {
        NumberFormatInfo numberFormatInfo3 = InNumberFormat;
        if (numberFormatInfo3.CurrencyDecimalSeparator != null && numberFormatInfo3.NumberDecimalSeparator != null && (numberFormatInfo3.CurrencyDecimalSeparator.Length == numberFormatInfo3.NumberDecimalSeparator.Length && numberFormatInfo3.CurrencyGroupSeparator != null) && (numberFormatInfo3.NumberGroupSeparator != null && numberFormatInfo3.CurrencyGroupSeparator.Length == numberFormatInfo3.NumberGroupSeparator.Length))
        {
          int num1 = checked (numberFormatInfo3.CurrencyDecimalSeparator.Length - 1);
          int index1 = 0;
          while (index1 <= num1)
          {
            if ((int) numberFormatInfo3.CurrencyDecimalSeparator[index1] == (int) numberFormatInfo3.NumberDecimalSeparator[index1])
              checked { ++index1; }
            else
              goto label_13;
          }
          int num2 = checked (numberFormatInfo3.CurrencyGroupSeparator.Length - 1);
          int index2 = 0;
          while (index2 <= num2)
          {
            if ((int) numberFormatInfo3.CurrencyGroupSeparator[index2] == (int) numberFormatInfo3.NumberGroupSeparator[index2])
              checked { ++index2; }
            else
              goto label_13;
          }
          numberFormatInfo2 = InNumberFormat;
          goto label_14;
        }
label_13:
        NumberFormatInfo numberFormatInfo4 = (NumberFormatInfo) InNumberFormat.Clone();
        numberFormatInfo4.CurrencyDecimalSeparator = numberFormatInfo4.NumberDecimalSeparator;
        numberFormatInfo4.CurrencyGroupSeparator = numberFormatInfo4.NumberGroupSeparator;
        numberFormatInfo4.CurrencyDecimalDigits = numberFormatInfo4.NumberDecimalDigits;
        numberFormatInfo2 = numberFormatInfo4;
      }
label_14:
      return numberFormatInfo2;
    }
  }
}
