// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.BooleanType
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
  public sealed class BooleanType
  {
    /// <summary>Returns a <see langword="Boolean" /> value that corresponds to the specified string.</summary>
    /// <param name="Value">Required. String to convert to a <see langword="Boolean" /> value.</param>
    /// <returns>The <see langword="Boolean" /> value that corresponds to <paramref name="Value" />.</returns>
    public static bool FromString(string Value)
    {
      if (Value == null)
        Value = "";
      try
      {
        CultureInfo cultureInfo = Utils.GetCultureInfo();
        if (string.Compare(Value, bool.FalseString, true, cultureInfo) == 0)
          return false;
        if (string.Compare(Value, bool.TrueString, true, cultureInfo) == 0)
          return true;
        long i64Value = 0;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? (ulong) i64Value > 0UL : DoubleType.Parse(Value) != 0.0;
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Boolean"), (Exception) ex);
      }
    }

    /// <summary>Returns a <see langword="Boolean" /> value that corresponds to the specified object.</summary>
    /// <param name="Value">Required. Object to convert to a <see langword="Boolean" /> value.</param>
    /// <returns>The <see langword="Boolean" /> value that corresponds to <paramref name="Value" />.</returns>
    public static bool FromObject(object Value)
    {
      if (Value == null)
        return false;
      if (Value is IConvertible ValueInterface)
      {
        switch (ValueInterface.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? flag : ValueInterface.ToBoolean((IFormatProvider) null);
          case TypeCode.Byte:
            return Value is byte num ? num > (byte) 0 : ValueInterface.ToByte((IFormatProvider) null) > (byte) 0;
          case TypeCode.Int16:
            return Value is short num ? (uint) num > 0U : (uint) ValueInterface.ToInt16((IFormatProvider) null) > 0U;
          case TypeCode.Int32:
            return Value is int num ? (uint) num > 0U : (uint) ValueInterface.ToInt32((IFormatProvider) null) > 0U;
          case TypeCode.Int64:
            return Value is long num ? (ulong) num > 0UL : (ulong) ValueInterface.ToInt64((IFormatProvider) null) > 0UL;
          case TypeCode.Single:
            return Value is float num ? (double) num != 0.0 : (double) ValueInterface.ToSingle((IFormatProvider) null) != 0.0;
          case TypeCode.Double:
            return Value is double num ? num != 0.0 : ValueInterface.ToDouble((IFormatProvider) null) != 0.0;
          case TypeCode.Decimal:
            return BooleanType.DecimalToBoolean(ValueInterface);
          case TypeCode.String:
            return Value is string str ? BooleanType.FromString(str) : BooleanType.FromString(ValueInterface.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Boolean"));
    }

    private static bool DecimalToBoolean(IConvertible ValueInterface)
    {
      return Convert.ToBoolean(ValueInterface.ToDecimal((IFormatProvider) null));
    }
  }
}
