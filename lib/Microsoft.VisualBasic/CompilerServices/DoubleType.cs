// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.DoubleType
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>This class has been deprecated as of Visual Basic 2005.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class DoubleType
  {
    /// <summary>Returns a <see langword="Double" /> value that corresponds to the specified string.</summary>
    /// <param name="Value">Required. String to convert to a <see langword="Double" /> value.</param>
    /// <returns>The <see langword="Double" /> value corresponding to <paramref name="Value" />.</returns>
    public static double FromString(string Value)
    {
      return DoubleType.FromString(Value, (NumberFormatInfo) null);
    }

    /// <summary>Returns a <see langword="Double" /> value that corresponds to the specified string and number format information.</summary>
    /// <param name="Value">Required. String to convert to a <see langword="Double" /> value.</param>
    /// <param name="NumberFormat">A <see cref="T:System.Globalization.NumberFormatInfo" /> object that defines how numeric values are formatted and displayed, depending on the culture.</param>
    /// <returns>The <see langword="Double" /> value corresponding to <paramref name="Value" />.</returns>
    public static double FromString(string Value, NumberFormatInfo NumberFormat)
    {
      if (Value == null)
        return 0.0;
      try
      {
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? (double) i64Value : DoubleType.Parse(Value, NumberFormat);
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Double"), (Exception) ex);
      }
    }

    /// <summary>Returns a <see langword="Double" /> value that corresponds to the specified object.</summary>
    /// <param name="Value">Required. Object to convert to a <see langword="Double" /> value.</param>
    /// <returns>The <see langword="Double" /> value corresponding to <paramref name="Value" />.</returns>
    public static double FromObject(object Value)
    {
      return DoubleType.FromObject(Value, (NumberFormatInfo) null);
    }

    /// <summary>Returns a <see langword="Double" /> value that corresponds to the specified object.</summary>
    /// <param name="Value">Required. Object to convert to a <see langword="Double" /> value.</param>
    /// <param name="NumberFormat">A <see cref="T:System.Globalization.NumberFormatInfo" /> object that defines how numeric values are formatted and displayed, depending on the culture.</param>
    /// <returns>The <see langword="Double" /> value corresponding to <paramref name="Value" />.</returns>
    public static double FromObject(object Value, NumberFormatInfo NumberFormat)
    {
      if (Value == null)
        return 0.0;
      if (Value is IConvertible ValueInterface)
      {
        switch (ValueInterface.GetTypeCode())
        {
          case TypeCode.Boolean:
            return (double) -(ValueInterface.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.Byte:
            return Value is byte num ? (double) num : (double) ValueInterface.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? (double) num : (double) ValueInterface.ToInt16((IFormatProvider) null);
          case TypeCode.Int32:
            return Value is int num ? (double) num : (double) ValueInterface.ToInt32((IFormatProvider) null);
          case TypeCode.Int64:
            return Value is long num ? (double) num : (double) ValueInterface.ToInt64((IFormatProvider) null);
          case TypeCode.Single:
            return Value is float num ? (double) num : (double) ValueInterface.ToSingle((IFormatProvider) null);
          case TypeCode.Double:
            return Value is double num ? num : ValueInterface.ToDouble((IFormatProvider) null);
          case TypeCode.Decimal:
            return DoubleType.DecimalToDouble(ValueInterface);
          case TypeCode.String:
            return DoubleType.FromString(ValueInterface.ToString((IFormatProvider) null), NumberFormat);
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Double"));
    }

    private static double DecimalToDouble(IConvertible ValueInterface)
    {
      return Convert.ToDouble(ValueInterface.ToDecimal((IFormatProvider) null));
    }

    /// <summary>Returns a <see langword="Double" /> value that corresponds to the specified string.</summary>
    /// <param name="Value">Required. String to convert to a <see langword="Double" /> value.</param>
    /// <returns>The <see langword="Double" /> value corresponding to <paramref name="Value" />.</returns>
    public static double Parse(string Value)
    {
      return DoubleType.Parse(Value, (NumberFormatInfo) null);
    }

    internal static bool TryParse(string Value, ref double Result)
    {
      CultureInfo cultureInfo = Utils.GetCultureInfo();
      NumberFormatInfo numberFormat = cultureInfo.NumberFormat;
      NumberFormatInfo normalizedNumberFormat = DecimalType.GetNormalizedNumberFormat(numberFormat);
      Value = Utils.ToHalfwidthNumbers(Value, cultureInfo);
      if (numberFormat == normalizedNumberFormat)
        return double.TryParse(Value, NumberStyles.Any, (IFormatProvider) normalizedNumberFormat, out Result);
      try
      {
        Result = double.Parse(Value, NumberStyles.Any, (IFormatProvider) normalizedNumberFormat);
        return true;
      }
      catch (FormatException ex1)
      {
        try
        {
          return double.TryParse(Value, NumberStyles.Any, (IFormatProvider) numberFormat, out Result);
        }
        catch (ArgumentException ex2)
        {
          return false;
        }
      }
      catch (StackOverflowException ex)
      {
        throw ex;
      }
      catch (OutOfMemoryException ex)
      {
        throw ex;
      }
      catch (ThreadAbortException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    /// <summary>Returns a <see langword="Double" /> value that corresponds to the specified string and number format information.</summary>
    /// <param name="Value">Required. String to convert to a <see langword="Double" /> value.</param>
    /// <param name="NumberFormat">A <see cref="T:System.Globalization.NumberFormatInfo" /> object that defines how numeric values are formatted and displayed, depending on the culture.</param>
    /// <returns>The <see langword="Double" /> value corresponding to <paramref name="Value" />.</returns>
    public static double Parse(string Value, NumberFormatInfo NumberFormat)
    {
      CultureInfo cultureInfo = Utils.GetCultureInfo();
      if (NumberFormat == null)
        NumberFormat = cultureInfo.NumberFormat;
      NumberFormatInfo normalizedNumberFormat = DecimalType.GetNormalizedNumberFormat(NumberFormat);
      Value = Utils.ToHalfwidthNumbers(Value, cultureInfo);
      try
      {
        return double.Parse(Value, NumberStyles.Any, (IFormatProvider) normalizedNumberFormat);
      }
      catch (FormatException ex) when (NumberFormat != normalizedNumberFormat)
      {
        return double.Parse(Value, NumberStyles.Any, (IFormatProvider) NumberFormat);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
