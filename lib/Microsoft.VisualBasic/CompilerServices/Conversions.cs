// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.Conversions
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>Provides methods that perform various type conversions.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class Conversions
  {
    private Conversions()
    {
    }

    /// <summary>Converts a string to a <see cref="T:System.Boolean" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>A <see langword="Boolean" /> value. Returns <see langword="False" /> if the string is null; otherwise, <see langword="True" />.</returns>
    public static bool ToBoolean(string Value)
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
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? (ulong) i64Value > 0UL : Conversions.ParseDouble(Value) != 0.0;
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Boolean"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to a <see cref="T:System.Boolean" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>A <see langword="Boolean" /> value. Returns <see langword="False" /> if the object is null; otherwise, <see langword="True" />.</returns>
    public static bool ToBoolean(object Value)
    {
      if (Value == null)
        return false;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? flag : convertible.ToBoolean((IFormatProvider) null);
          case TypeCode.SByte:
            return Value is sbyte num ? (uint) num > 0U : (uint) convertible.ToSByte((IFormatProvider) null) > 0U;
          case TypeCode.Byte:
            return Value is byte num ? num > (byte) 0 : convertible.ToByte((IFormatProvider) null) > (byte) 0;
          case TypeCode.Int16:
            return Value is short num ? (uint) num > 0U : (uint) convertible.ToInt16((IFormatProvider) null) > 0U;
          case TypeCode.UInt16:
            return Value is ushort num ? num > (ushort) 0 : convertible.ToUInt16((IFormatProvider) null) > (ushort) 0;
          case TypeCode.Int32:
            return Value is int num ? (uint) num > 0U : (uint) convertible.ToInt32((IFormatProvider) null) > 0U;
          case TypeCode.UInt32:
            return Value is uint num ? num > 0U : convertible.ToUInt32((IFormatProvider) null) > 0U;
          case TypeCode.Int64:
            return Value is long num ? (ulong) num > 0UL : (ulong) convertible.ToInt64((IFormatProvider) null) > 0UL;
          case TypeCode.UInt64:
            return Value is ulong num ? num > 0UL : convertible.ToUInt64((IFormatProvider) null) > 0UL;
          case TypeCode.Single:
            return Value is float num ? (double) num != 0.0 : (double) convertible.ToSingle((IFormatProvider) null) != 0.0;
          case TypeCode.Double:
            return Value is double num ? num != 0.0 : convertible.ToDouble((IFormatProvider) null) != 0.0;
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToBoolean((IFormatProvider) null) : Convert.ToBoolean(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Value is string str ? Conversions.ToBoolean(str) : Conversions.ToBoolean(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Boolean"));
    }

    /// <summary>Converts a string to a <see cref="T:System.Byte" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Byte" /> value of the string.</returns>
    public static byte ToByte(string Value)
    {
      if (Value == null)
        return 0;
      try
      {
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? checked ((byte) i64Value) : checked ((byte) Math.Round(Conversions.ParseDouble(Value)));
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Byte"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to a <see cref="T:System.Byte" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Byte" /> value of the object.</returns>
    public static byte ToByte(object Value)
    {
      if (Value == null)
        return 0;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? (byte) -(flag ? 1 : 0) : (byte) -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? checked ((byte) num) : checked ((byte) convertible.ToSByte((IFormatProvider) null));
          case TypeCode.Byte:
            return Value is byte num ? num : convertible.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? checked ((byte) num) : checked ((byte) convertible.ToInt16((IFormatProvider) null));
          case TypeCode.UInt16:
            return Value is ushort num ? checked ((byte) num) : checked ((byte) convertible.ToUInt16((IFormatProvider) null));
          case TypeCode.Int32:
            return Value is int num ? checked ((byte) num) : checked ((byte) convertible.ToInt32((IFormatProvider) null));
          case TypeCode.UInt32:
            return Value is uint num ? checked ((byte) num) : checked ((byte) convertible.ToUInt32((IFormatProvider) null));
          case TypeCode.Int64:
            return Value is long num ? checked ((byte) num) : checked ((byte) convertible.ToInt64((IFormatProvider) null));
          case TypeCode.UInt64:
            return Value is ulong num ? checked ((byte) num) : checked ((byte) convertible.ToUInt64((IFormatProvider) null));
          case TypeCode.Single:
            return Value is float num ? checked ((byte) Math.Round((double) num)) : checked ((byte) Math.Round((double) convertible.ToSingle((IFormatProvider) null)));
          case TypeCode.Double:
            return Value is double a ? checked ((byte) Math.Round(a)) : checked ((byte) Math.Round(convertible.ToDouble((IFormatProvider) null)));
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToByte((IFormatProvider) null) : Convert.ToByte(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Value is string str ? Conversions.ToByte(str) : Conversions.ToByte(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Byte"));
    }

    /// <summary>Converts a string to an <see cref="T:System.SByte" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="SByte" /> value of the string.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(string Value)
    {
      if (Value == null)
        return 0;
      try
      {
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? checked ((sbyte) i64Value) : checked ((sbyte) Math.Round(Conversions.ParseDouble(Value)));
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "SByte"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to an <see cref="T:System.SByte" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="SByte" /> value of the object.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(object Value)
    {
      if (Value == null)
        return 0;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? (sbyte) -(flag ? 1 : 0) : (sbyte) -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? num : convertible.ToSByte((IFormatProvider) null);
          case TypeCode.Byte:
            return Value is byte num ? checked ((sbyte) num) : checked ((sbyte) convertible.ToByte((IFormatProvider) null));
          case TypeCode.Int16:
            return Value is short num ? checked ((sbyte) num) : checked ((sbyte) convertible.ToInt16((IFormatProvider) null));
          case TypeCode.UInt16:
            return Value is ushort num ? checked ((sbyte) num) : checked ((sbyte) convertible.ToUInt16((IFormatProvider) null));
          case TypeCode.Int32:
            return Value is int num ? checked ((sbyte) num) : checked ((sbyte) convertible.ToInt32((IFormatProvider) null));
          case TypeCode.UInt32:
            return Value is uint num ? checked ((sbyte) num) : checked ((sbyte) convertible.ToUInt32((IFormatProvider) null));
          case TypeCode.Int64:
            return Value is long num ? checked ((sbyte) num) : checked ((sbyte) convertible.ToInt64((IFormatProvider) null));
          case TypeCode.UInt64:
            return Value is ulong num ? checked ((sbyte) num) : checked ((sbyte) convertible.ToUInt64((IFormatProvider) null));
          case TypeCode.Single:
            return Value is float num ? checked ((sbyte) Math.Round((double) num)) : checked ((sbyte) Math.Round((double) convertible.ToSingle((IFormatProvider) null)));
          case TypeCode.Double:
            return Value is double a ? checked ((sbyte) Math.Round(a)) : checked ((sbyte) Math.Round(convertible.ToDouble((IFormatProvider) null)));
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToSByte((IFormatProvider) null) : Convert.ToSByte(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Value is string str ? Conversions.ToSByte(str) : Conversions.ToSByte(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "SByte"));
    }

    /// <summary>Converts a string to a <see langword="Short" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Short" /> value of the string.</returns>
    public static short ToShort(string Value)
    {
      if (Value == null)
        return 0;
      try
      {
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? checked ((short) i64Value) : checked ((short) Math.Round(Conversions.ParseDouble(Value)));
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Short"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to a <see langword="Short" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Short" /> value of the object.</returns>
    public static short ToShort(object Value)
    {
      if (Value == null)
        return 0;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? (short) -(flag ? 1 : 0) : (short) -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? (short) num : (short) convertible.ToSByte((IFormatProvider) null);
          case TypeCode.Byte:
            return Value is byte num ? (short) num : (short) convertible.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? num : convertible.ToInt16((IFormatProvider) null);
          case TypeCode.UInt16:
            return Value is ushort num ? checked ((short) num) : checked ((short) convertible.ToUInt16((IFormatProvider) null));
          case TypeCode.Int32:
            return Value is int num ? checked ((short) num) : checked ((short) convertible.ToInt32((IFormatProvider) null));
          case TypeCode.UInt32:
            return Value is uint num ? checked ((short) num) : checked ((short) convertible.ToUInt32((IFormatProvider) null));
          case TypeCode.Int64:
            return Value is long num ? checked ((short) num) : checked ((short) convertible.ToInt64((IFormatProvider) null));
          case TypeCode.UInt64:
            return Value is ulong num ? checked ((short) num) : checked ((short) convertible.ToUInt64((IFormatProvider) null));
          case TypeCode.Single:
            return Value is float num ? checked ((short) Math.Round((double) num)) : checked ((short) Math.Round((double) convertible.ToSingle((IFormatProvider) null)));
          case TypeCode.Double:
            return Value is double a ? checked ((short) Math.Round(a)) : checked ((short) Math.Round(convertible.ToDouble((IFormatProvider) null)));
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToInt16((IFormatProvider) null) : Convert.ToInt16(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Value is string str ? Conversions.ToShort(str) : Conversions.ToShort(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Short"));
    }

    /// <summary>Converts a string to a <see langword="Ushort" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Ushort" /> value of the string.</returns>
    [CLSCompliant(false)]
    public static ushort ToUShort(string Value)
    {
      if (Value == null)
        return 0;
      try
      {
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? checked ((ushort) i64Value) : checked ((ushort) Math.Round(Conversions.ParseDouble(Value)));
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "UShort"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to a <see langword="Ushort" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Ushort" /> value of the object.</returns>
    [CLSCompliant(false)]
    public static ushort ToUShort(object Value)
    {
      if (Value == null)
        return 0;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? (ushort) -(flag ? 1 : 0) : (ushort) -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? checked ((ushort) num) : checked ((ushort) convertible.ToSByte((IFormatProvider) null));
          case TypeCode.Byte:
            return Value is byte num ? (ushort) num : (ushort) convertible.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? checked ((ushort) num) : checked ((ushort) convertible.ToInt16((IFormatProvider) null));
          case TypeCode.UInt16:
            return Value is ushort num ? num : convertible.ToUInt16((IFormatProvider) null);
          case TypeCode.Int32:
            return Value is int num ? checked ((ushort) num) : checked ((ushort) convertible.ToInt32((IFormatProvider) null));
          case TypeCode.UInt32:
            return Value is uint num ? checked ((ushort) num) : checked ((ushort) convertible.ToUInt32((IFormatProvider) null));
          case TypeCode.Int64:
            return Value is long num ? checked ((ushort) num) : checked ((ushort) convertible.ToInt64((IFormatProvider) null));
          case TypeCode.UInt64:
            return Value is ulong num ? checked ((ushort) num) : checked ((ushort) convertible.ToUInt64((IFormatProvider) null));
          case TypeCode.Single:
            return Value is float num ? checked ((ushort) Math.Round((double) num)) : checked ((ushort) Math.Round((double) convertible.ToSingle((IFormatProvider) null)));
          case TypeCode.Double:
            return Value is double a ? checked ((ushort) Math.Round(a)) : checked ((ushort) Math.Round(convertible.ToDouble((IFormatProvider) null)));
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToUInt16((IFormatProvider) null) : Convert.ToUInt16(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Value is string str ? Conversions.ToUShort(str) : Conversions.ToUShort(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "UShort"));
    }

    /// <summary>Converts a string to an integer value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="int" /> value of the string.</returns>
    public static int ToInteger(string Value)
    {
      if (Value == null)
        return 0;
      try
      {
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? checked ((int) i64Value) : checked ((int) Math.Round(Conversions.ParseDouble(Value)));
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Integer"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to an integer value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="int" /> value of the object.</returns>
    public static int ToInteger(object Value)
    {
      if (Value == null)
        return 0;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? -(flag ? 1 : 0) : -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? (int) num : (int) convertible.ToSByte((IFormatProvider) null);
          case TypeCode.Byte:
            return Value is byte num ? (int) num : (int) convertible.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? (int) num : (int) convertible.ToInt16((IFormatProvider) null);
          case TypeCode.UInt16:
            return Value is ushort num ? (int) num : (int) convertible.ToUInt16((IFormatProvider) null);
          case TypeCode.Int32:
            return Value is int num ? num : convertible.ToInt32((IFormatProvider) null);
          case TypeCode.UInt32:
            return Value is uint num ? checked ((int) num) : checked ((int) convertible.ToUInt32((IFormatProvider) null));
          case TypeCode.Int64:
            return Value is long num ? checked ((int) num) : checked ((int) convertible.ToInt64((IFormatProvider) null));
          case TypeCode.UInt64:
            return Value is ulong num ? checked ((int) num) : checked ((int) convertible.ToUInt64((IFormatProvider) null));
          case TypeCode.Single:
            return Value is float num ? checked ((int) Math.Round((double) num)) : checked ((int) Math.Round((double) convertible.ToSingle((IFormatProvider) null)));
          case TypeCode.Double:
            return Value is double a ? checked ((int) Math.Round(a)) : checked ((int) Math.Round(convertible.ToDouble((IFormatProvider) null)));
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToInt32((IFormatProvider) null) : Convert.ToInt32(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Value is string str ? Conversions.ToInteger(str) : Conversions.ToInteger(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Integer"));
    }

    /// <summary>Converts a string to a <see langword="Uint" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Uint" /> value of the string.</returns>
    [CLSCompliant(false)]
    public static uint ToUInteger(string Value)
    {
      if (Value == null)
        return 0;
      try
      {
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? checked ((uint) i64Value) : checked ((uint) Math.Round(Conversions.ParseDouble(Value)));
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "UInteger"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to a <see langword="Uint" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Uint" /> value of the object.</returns>
    [CLSCompliant(false)]
    public static uint ToUInteger(object Value)
    {
      if (Value == null)
        return 0;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? (uint) -(flag ? 1 : 0) : (uint) -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? checked ((uint) num) : checked ((uint) convertible.ToSByte((IFormatProvider) null));
          case TypeCode.Byte:
            return Value is byte num ? (uint) num : (uint) convertible.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? checked ((uint) num) : checked ((uint) convertible.ToInt16((IFormatProvider) null));
          case TypeCode.UInt16:
            return Value is ushort num ? (uint) num : (uint) convertible.ToUInt16((IFormatProvider) null);
          case TypeCode.Int32:
            return Value is int num ? checked ((uint) num) : checked ((uint) convertible.ToInt32((IFormatProvider) null));
          case TypeCode.UInt32:
            return Value is uint num ? num : convertible.ToUInt32((IFormatProvider) null);
          case TypeCode.Int64:
            return Value is long num ? checked ((uint) num) : checked ((uint) convertible.ToInt64((IFormatProvider) null));
          case TypeCode.UInt64:
            return Value is ulong num ? checked ((uint) num) : checked ((uint) convertible.ToUInt64((IFormatProvider) null));
          case TypeCode.Single:
            return Value is float num ? checked ((uint) Math.Round((double) num)) : checked ((uint) Math.Round((double) convertible.ToSingle((IFormatProvider) null)));
          case TypeCode.Double:
            return Value is double a ? checked ((uint) Math.Round(a)) : checked ((uint) Math.Round(convertible.ToDouble((IFormatProvider) null)));
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToUInt32((IFormatProvider) null) : Convert.ToUInt32(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Value is string str ? Conversions.ToUInteger(str) : Conversions.ToUInteger(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "UInteger"));
    }

    /// <summary>Converts a string to a <see langword="Long" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Long" /> value of the string.</returns>
    public static long ToLong(string Value)
    {
      if (Value == null)
        return 0;
      try
      {
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? i64Value : Convert.ToInt64(Conversions.ParseDecimal(Value, (NumberFormatInfo) null));
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Long"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to a <see langword="Long" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Long" /> value of the object.</returns>
    public static long ToLong(object Value)
    {
      if (Value == null)
        return 0;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? (long) -(flag ? 1 : 0) : (long) -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? (long) num : (long) convertible.ToSByte((IFormatProvider) null);
          case TypeCode.Byte:
            return Value is byte num ? (long) num : (long) convertible.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? (long) num : (long) convertible.ToInt16((IFormatProvider) null);
          case TypeCode.UInt16:
            return Value is ushort num ? (long) num : (long) convertible.ToUInt16((IFormatProvider) null);
          case TypeCode.Int32:
            return Value is int num ? (long) num : (long) convertible.ToInt32((IFormatProvider) null);
          case TypeCode.UInt32:
            return Value is uint num ? (long) num : (long) convertible.ToUInt32((IFormatProvider) null);
          case TypeCode.Int64:
            return Value is long num ? num : convertible.ToInt64((IFormatProvider) null);
          case TypeCode.UInt64:
            return Value is ulong num ? checked ((long) num) : checked ((long) convertible.ToUInt64((IFormatProvider) null));
          case TypeCode.Single:
            return Value is float num ? checked ((long) Math.Round((double) num)) : checked ((long) Math.Round((double) convertible.ToSingle((IFormatProvider) null)));
          case TypeCode.Double:
            return Value is double a ? checked ((long) Math.Round(a)) : checked ((long) Math.Round(convertible.ToDouble((IFormatProvider) null)));
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToInt64((IFormatProvider) null) : Convert.ToInt64(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Value is string str ? Conversions.ToLong(str) : Conversions.ToLong(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Long"));
    }

    /// <summary>Converts a string to a <see langword="Ulong" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Ulong" /> value of the string.</returns>
    [CLSCompliant(false)]
    public static ulong ToULong(string Value)
    {
      if (Value == null)
        return 0;
      try
      {
        ulong ui64Value;
        return Utils.IsHexOrOctValue(Value, ref ui64Value) ? ui64Value : Convert.ToUInt64(Conversions.ParseDecimal(Value, (NumberFormatInfo) null));
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "ULong"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to a <see langword="Ulong" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Ulong" /> value of the object.</returns>
    [CLSCompliant(false)]
    public static ulong ToULong(object Value)
    {
      if (Value == null)
        return 0;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? (ulong) -(flag ? 1 : 0) : (ulong) -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? checked ((ulong) num) : checked ((ulong) convertible.ToSByte((IFormatProvider) null));
          case TypeCode.Byte:
            return Value is byte num ? (ulong) num : (ulong) convertible.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? checked ((ulong) num) : checked ((ulong) convertible.ToInt16((IFormatProvider) null));
          case TypeCode.UInt16:
            return Value is ushort num ? (ulong) num : (ulong) convertible.ToUInt16((IFormatProvider) null);
          case TypeCode.Int32:
            return Value is int num ? checked ((ulong) num) : checked ((ulong) convertible.ToInt32((IFormatProvider) null));
          case TypeCode.UInt32:
            return Value is uint num ? (ulong) num : (ulong) convertible.ToUInt32((IFormatProvider) null);
          case TypeCode.Int64:
            return Value is long num ? checked ((ulong) num) : checked ((ulong) convertible.ToInt64((IFormatProvider) null));
          case TypeCode.UInt64:
            return Value is ulong num ? num : convertible.ToUInt64((IFormatProvider) null);
          case TypeCode.Single:
            return Value is float num ? checked ((ulong) Math.Round((double) num)) : checked ((ulong) Math.Round((double) convertible.ToSingle((IFormatProvider) null)));
          case TypeCode.Double:
            return Value is double a ? checked ((ulong) Math.Round(a)) : checked ((ulong) Math.Round(convertible.ToDouble((IFormatProvider) null)));
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToUInt64((IFormatProvider) null) : Convert.ToUInt64(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Value is string str ? Conversions.ToULong(str) : Conversions.ToULong(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "ULong"));
    }

    /// <summary>Converts a <see cref="T:System.Boolean" /> value to a <see cref="T:System.Decimal" /> value.</summary>
    /// <param name="Value">A Boolean value to convert.</param>
    /// <returns>The <see langword="Decimal" /> value of the Boolean value.</returns>
    public static Decimal ToDecimal(bool Value)
    {
      return !Value ? new Decimal() : Decimal.MinusOne;
    }

    /// <summary>Converts a string to a <see cref="T:System.Decimal" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Decimal" /> value of the string.</returns>
    public static Decimal ToDecimal(string Value)
    {
      return Conversions.ToDecimal(Value, (NumberFormatInfo) null);
    }

    internal static Decimal ToDecimal(string Value, NumberFormatInfo NumberFormat)
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
          num = !Utils.IsHexOrOctValue(Value, ref i64Value) ? Conversions.ParseDecimal(Value, NumberFormat) : new Decimal(i64Value);
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

    /// <summary>Converts an object to a <see cref="T:System.Decimal" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Decimal" /> value of the object.</returns>
    public static Decimal ToDecimal(object Value)
    {
      return Conversions.ToDecimal(Value, (NumberFormatInfo) null);
    }

    internal static Decimal ToDecimal(object Value, NumberFormatInfo NumberFormat)
    {
      Decimal num1;
      if (Value == null)
      {
        num1 = new Decimal();
      }
      else
      {
        if (Value is IConvertible convertible)
        {
          switch (convertible.GetTypeCode())
          {
            case TypeCode.Boolean:
              num1 = !(Value is bool flag) ? Conversions.ToDecimal(convertible.ToBoolean((IFormatProvider) null)) : Conversions.ToDecimal(flag);
              goto label_18;
            case TypeCode.SByte:
              num1 = !(Value is sbyte num2) ? new Decimal((int) convertible.ToSByte((IFormatProvider) null)) : new Decimal((int) num2);
              goto label_18;
            case TypeCode.Byte:
              num1 = !(Value is byte num2) ? new Decimal((int) convertible.ToByte((IFormatProvider) null)) : new Decimal((int) num2);
              goto label_18;
            case TypeCode.Int16:
              num1 = !(Value is short num2) ? new Decimal((int) convertible.ToInt16((IFormatProvider) null)) : new Decimal((int) num2);
              goto label_18;
            case TypeCode.UInt16:
              num1 = !(Value is ushort num2) ? new Decimal((int) convertible.ToUInt16((IFormatProvider) null)) : new Decimal((int) num2);
              goto label_18;
            case TypeCode.Int32:
              num1 = !(Value is int num2) ? new Decimal(convertible.ToInt32((IFormatProvider) null)) : new Decimal(num2);
              goto label_18;
            case TypeCode.UInt32:
              num1 = !(Value is uint num2) ? new Decimal(convertible.ToUInt32((IFormatProvider) null)) : new Decimal(num2);
              goto label_18;
            case TypeCode.Int64:
              num1 = !(Value is long num2) ? new Decimal(convertible.ToInt64((IFormatProvider) null)) : new Decimal(num2);
              goto label_18;
            case TypeCode.UInt64:
              num1 = !(Value is ulong num2) ? new Decimal(convertible.ToUInt64((IFormatProvider) null)) : new Decimal(num2);
              goto label_18;
            case TypeCode.Single:
              num1 = !(Value is float num2) ? new Decimal(convertible.ToSingle((IFormatProvider) null)) : new Decimal(num2);
              goto label_18;
            case TypeCode.Double:
              num1 = !(Value is double num2) ? new Decimal(convertible.ToDouble((IFormatProvider) null)) : new Decimal(num2);
              goto label_18;
            case TypeCode.Decimal:
              num1 = convertible.ToDecimal((IFormatProvider) null);
              goto label_18;
            case TypeCode.String:
              num1 = Conversions.ToDecimal(convertible.ToString((IFormatProvider) null), NumberFormat);
              goto label_18;
          }
        }
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Decimal"));
      }
label_18:
      return num1;
    }

    private static Decimal ParseDecimal(string Value, NumberFormatInfo NumberFormat)
    {
      CultureInfo cultureInfo = Utils.GetCultureInfo();
      if (NumberFormat == null)
        NumberFormat = cultureInfo.NumberFormat;
      NumberFormatInfo normalizedNumberFormat = Conversions.GetNormalizedNumberFormat(NumberFormat);
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

    private static NumberFormatInfo GetNormalizedNumberFormat(
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

    /// <summary>Converts a <see cref="T:System.String" /> to a <see cref="T:System.Single" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Single" /> value of the string.</returns>
    public static float ToSingle(string Value)
    {
      return Conversions.ToSingle(Value, (NumberFormatInfo) null);
    }

    internal static float ToSingle(string Value, NumberFormatInfo NumberFormat)
    {
      if (Value == null)
        return 0.0f;
      try
      {
        long i64Value;
        if (Utils.IsHexOrOctValue(Value, ref i64Value))
          return (float) i64Value;
        double d = Conversions.ParseDouble(Value, NumberFormat);
        if ((d < -3.40282346638529E+38 || d > 3.40282346638529E+38) && !double.IsInfinity(d))
          throw new OverflowException();
        return (float) d;
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Single"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to a <see cref="T:System.Single" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Single" /> value of the object.</returns>
    public static float ToSingle(object Value)
    {
      return Conversions.ToSingle(Value, (NumberFormatInfo) null);
    }

    internal static float ToSingle(object Value, NumberFormatInfo NumberFormat)
    {
      if (Value == null)
        return 0.0f;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? (float) -(flag ? 1 : 0) : (float) -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? (float) num : (float) convertible.ToSByte((IFormatProvider) null);
          case TypeCode.Byte:
            return Value is byte num ? (float) num : (float) convertible.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? (float) num : (float) convertible.ToInt16((IFormatProvider) null);
          case TypeCode.UInt16:
            return Value is ushort num ? (float) num : (float) convertible.ToUInt16((IFormatProvider) null);
          case TypeCode.Int32:
            return Value is int num ? (float) num : (float) convertible.ToInt32((IFormatProvider) null);
          case TypeCode.UInt32:
            return Value is uint num ? (float) num : (float) convertible.ToUInt32((IFormatProvider) null);
          case TypeCode.Int64:
            return Value is long num ? (float) num : (float) convertible.ToInt64((IFormatProvider) null);
          case TypeCode.UInt64:
            return Value is ulong num ? (float) num : (float) convertible.ToUInt64((IFormatProvider) null);
          case TypeCode.Single:
            return Value is float num ? num : convertible.ToSingle((IFormatProvider) null);
          case TypeCode.Double:
            return Value is double num ? (float) num : (float) convertible.ToDouble((IFormatProvider) null);
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToSingle((IFormatProvider) null) : Convert.ToSingle(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Conversions.ToSingle(convertible.ToString((IFormatProvider) null), NumberFormat);
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Single"));
    }

    /// <summary>Converts a string to a <see cref="T:System.Double" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Double" /> value of the string.</returns>
    public static double ToDouble(string Value)
    {
      return Conversions.ToDouble(Value, (NumberFormatInfo) null);
    }

    internal static double ToDouble(string Value, NumberFormatInfo NumberFormat)
    {
      if (Value == null)
        return 0.0;
      try
      {
        long i64Value;
        return Utils.IsHexOrOctValue(Value, ref i64Value) ? (double) i64Value : Conversions.ParseDouble(Value, NumberFormat);
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Double"), (Exception) ex);
      }
    }

    /// <summary>Converts an object to a <see cref="T:System.Double" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Double" /> value of the object.</returns>
    public static double ToDouble(object Value)
    {
      return Conversions.ToDouble(Value, (NumberFormatInfo) null);
    }

    internal static double ToDouble(object Value, NumberFormatInfo NumberFormat)
    {
      if (Value == null)
        return 0.0;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            return Value is bool flag ? (double) -(flag ? 1 : 0) : (double) -(convertible.ToBoolean((IFormatProvider) null) ? 1 : 0);
          case TypeCode.SByte:
            return Value is sbyte num ? (double) num : (double) convertible.ToSByte((IFormatProvider) null);
          case TypeCode.Byte:
            return Value is byte num ? (double) num : (double) convertible.ToByte((IFormatProvider) null);
          case TypeCode.Int16:
            return Value is short num ? (double) num : (double) convertible.ToInt16((IFormatProvider) null);
          case TypeCode.UInt16:
            return Value is ushort num ? (double) num : (double) convertible.ToUInt16((IFormatProvider) null);
          case TypeCode.Int32:
            return Value is int num ? (double) num : (double) convertible.ToInt32((IFormatProvider) null);
          case TypeCode.UInt32:
            return Value is uint num ? (double) num : (double) convertible.ToUInt32((IFormatProvider) null);
          case TypeCode.Int64:
            return Value is long num ? (double) num : (double) convertible.ToInt64((IFormatProvider) null);
          case TypeCode.UInt64:
            return Value is ulong num ? (double) num : (double) convertible.ToUInt64((IFormatProvider) null);
          case TypeCode.Single:
            return Value is float num ? (double) num : (double) convertible.ToSingle((IFormatProvider) null);
          case TypeCode.Double:
            return Value is double num ? num : convertible.ToDouble((IFormatProvider) null);
          case TypeCode.Decimal:
            return Value is Decimal ? convertible.ToDouble((IFormatProvider) null) : Convert.ToDouble(convertible.ToDecimal((IFormatProvider) null));
          case TypeCode.String:
            return Conversions.ToDouble(convertible.ToString((IFormatProvider) null), NumberFormat);
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Double"));
    }

    private static double ParseDouble(string Value)
    {
      return Conversions.ParseDouble(Value, (NumberFormatInfo) null);
    }

    internal static bool TryParseDouble(string Value, ref double Result)
    {
      CultureInfo cultureInfo = Utils.GetCultureInfo();
      NumberFormatInfo numberFormat = cultureInfo.NumberFormat;
      NumberFormatInfo normalizedNumberFormat = Conversions.GetNormalizedNumberFormat(numberFormat);
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

    private static double ParseDouble(string Value, NumberFormatInfo NumberFormat)
    {
      CultureInfo cultureInfo = Utils.GetCultureInfo();
      if (NumberFormat == null)
        NumberFormat = cultureInfo.NumberFormat;
      NumberFormatInfo normalizedNumberFormat = Conversions.GetNormalizedNumberFormat(NumberFormat);
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

    /// <summary>Converts a string to a <see cref="T:System.DateTime" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="DateTime" /> value of the string.</returns>
    public static DateTime ToDate(string Value)
    {
      DateTime Result;
      if (Conversions.TryParseDate(Value, ref Result))
        return Result;
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromStringTo, (object) Strings.Left(Value, 32), (object) "Date"));
    }

    /// <summary>Converts an object to a <see cref="T:System.DateTime" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="DateTime" /> value of the object.</returns>
    public static DateTime ToDate(object Value)
    {
      if (Value == null)
        return DateTime.MinValue;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.DateTime:
            return Value is DateTime dateTime ? dateTime : convertible.ToDateTime((IFormatProvider) null);
          case TypeCode.String:
            return Value is string str ? Conversions.ToDate(str) : Conversions.ToDate(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Date"));
    }

    internal static bool TryParseDate(string Value, ref DateTime Result)
    {
      CultureInfo cultureInfo = Utils.GetCultureInfo();
      return DateTime.TryParse(Utils.ToHalfwidthNumbers(Value, cultureInfo), (IFormatProvider) cultureInfo, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault, out Result);
    }

    /// <summary>Converts a string to a <see cref="T:System.Char" /> value.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>The <see langword="Char" /> value of the string.</returns>
    public static char ToChar(string Value)
    {
      char minValue;
      switch (Value)
      {
        case "":
        case null:
          minValue = char.MinValue;
          break;
        default:
          minValue = Value[0];
          break;
      }
      return minValue;
    }

    /// <summary>Converts an object to a <see cref="T:System.Char" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="Char" /> value of the object.</returns>
    public static char ToChar(object Value)
    {
      if (Value == null)
        return char.MinValue;
      if (Value is IConvertible convertible)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Char:
            return Value is char ch ? ch : convertible.ToChar((IFormatProvider) null);
          case TypeCode.String:
            return Value is string str ? Conversions.ToChar(str) : Conversions.ToChar(convertible.ToString((IFormatProvider) null));
        }
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Char"));
    }

    /// <summary>Converts a string to a one-dimensional <see cref="T:System.Char" /> array.</summary>
    /// <param name="Value">The string to convert.</param>
    /// <returns>A one-dimensional <see langword="Char" /> array.</returns>
    public static char[] ToCharArrayRankOne(string Value)
    {
      if (Value == null)
        Value = "";
      return Value.ToCharArray();
    }

    /// <summary>Converts an object to a one-dimensional <see cref="T:System.Char" /> array.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>A one-dimensional <see langword="Char" /> array.</returns>
    public static char[] ToCharArrayRankOne(object Value)
    {
      switch (Value)
      {
        case null:
          return "".ToCharArray();
        case char[] chArray when chArray.Rank == 1:
          return chArray;
        case IConvertible convertible when convertible.GetTypeCode() == TypeCode.String:
          return convertible.ToString((IFormatProvider) null).ToCharArray();
        default:
          throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "Char()"));
      }
    }

    /// <summary>Converts a <see cref="T:System.Boolean" /> value to a <see cref="T:System.String" />.</summary>
    /// <param name="Value">The <see langword="Boolean" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Boolean" /> value.</returns>
    public static string ToString(bool Value)
    {
      return !Value ? bool.FalseString : bool.TrueString;
    }

    /// <summary>Converts a <see cref="T:System.Byte" /> value to a <see cref="T:System.String" />.</summary>
    /// <param name="Value">The <see langword="Byte" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Byte" /> value.</returns>
    public static string ToString(byte Value)
    {
      return Value.ToString((string) null, (IFormatProvider) null);
    }

    /// <summary>Converts a <see cref="T:System.Char" /> value to a <see cref="T:System.String" />.</summary>
    /// <param name="Value">The <see langword="Char" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Char" /> value.</returns>
    public static string ToString(char Value)
    {
      return Value.ToString();
    }

    /// <summary>Converts a <see cref="T:System.Char" /> array to a string.</summary>
    /// <param name="Value">The <see langword="Char" /> array to convert.</param>
    /// <returns>The string representation of the specified array.</returns>
    public static string FromCharArray(char[] Value)
    {
      return new string(Value);
    }

    /// <summary>Converts a <see cref="T:System.Char" /> value to a string, given a byte count.</summary>
    /// <param name="Value">The <see langword="Char" /> value to convert.</param>
    /// <param name="Count">The byte count of the <see langword="Char" /> value.</param>
    /// <returns>The string representation of the specified value.</returns>
    public static string FromCharAndCount(char Value, int Count)
    {
      return new string(Value, Count);
    }

    /// <summary>Converts a subset of a <see cref="T:System.Char" /> array to a string.</summary>
    /// <param name="Value">The <see langword="Char" /> array to convert.</param>
    /// <param name="StartIndex">Zero-based index of the start position.</param>
    /// <param name="Length">Length of the subset in bytes.</param>
    /// <returns>The string representation of the specified array from the start position to the specified length.</returns>
    public static string FromCharArraySubset(char[] Value, int StartIndex, int Length)
    {
      return new string(Value, StartIndex, Length);
    }

    /// <summary>Converts a <see langword="Short" /> value to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The <see langword="Short" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Short" /> value.</returns>
    public static string ToString(short Value)
    {
      return Value.ToString((string) null, (IFormatProvider) null);
    }

    /// <summary>Converts an integer value to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The <see langword="int" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="int" /> value.</returns>
    public static string ToString(int Value)
    {
      return Value.ToString((string) null, (IFormatProvider) null);
    }

    /// <summary>Converts a <see langword="uint" /> value to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The <see langword="Uint" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Uint" /> value.</returns>
    [CLSCompliant(false)]
    public static string ToString(uint Value)
    {
      return Value.ToString((string) null, (IFormatProvider) null);
    }

    /// <summary>Converts a <see langword="Long" /> value to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The <see langword="Long" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Long" /> value.</returns>
    public static string ToString(long Value)
    {
      return Value.ToString((string) null, (IFormatProvider) null);
    }

    /// <summary>Converts a <see langword="Ulong" /> value to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The <see langword="Ulong" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Ulong" /> value.</returns>
    [CLSCompliant(false)]
    public static string ToString(ulong Value)
    {
      return Value.ToString((string) null, (IFormatProvider) null);
    }

    /// <summary>Converts a <see cref="T:System.Single" /> value (a single-precision floating point number) to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The <see langword="Single" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Single" /> value.</returns>
    public static string ToString(float Value)
    {
      return Conversions.ToString(Value, (NumberFormatInfo) null);
    }

    /// <summary>Converts a <see cref="T:System.Double" /> value to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The <see langword="Double" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Double" /> value.</returns>
    public static string ToString(double Value)
    {
      return Conversions.ToString(Value, (NumberFormatInfo) null);
    }

    /// <summary>Converts a <see cref="T:System.Single" /> value to a <see cref="T:System.String" /> value, using the specified number format.</summary>
    /// <param name="Value">The <see langword="Single" /> value to convert.</param>
    /// <param name="NumberFormat">The number format to use, according to <see cref="T:System.Globalization.NumberFormatInfo" />.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Single" /> value.</returns>
    public static string ToString(float Value, NumberFormatInfo NumberFormat)
    {
      return Value.ToString((string) null, (IFormatProvider) NumberFormat);
    }

    /// <summary>Converts a <see cref="T:System.Double" /> value to a <see cref="T:System.String" /> value, using the specified number format.</summary>
    /// <param name="Value">The <see langword="Double" /> value to convert.</param>
    /// <param name="NumberFormat">The number format to use, according to <see cref="T:System.Globalization.NumberFormatInfo" />.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Double" /> value.</returns>
    public static string ToString(double Value, NumberFormatInfo NumberFormat)
    {
      return Value.ToString("G", (IFormatProvider) NumberFormat);
    }

    /// <summary>Converts a <see cref="T:System.DateTime" /> value to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The <see langword="DateTime" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="DateTime" /> value.</returns>
    public static string ToString(DateTime Value)
    {
      long ticks = Value.TimeOfDay.Ticks;
      return ticks == Value.Ticks || Value.Year == 1899 && Value.Month == 12 && Value.Day == 30 ? Value.ToString("T", (IFormatProvider) null) : (ticks != 0L ? Value.ToString("G", (IFormatProvider) null) : Value.ToString("d", (IFormatProvider) null));
    }

    /// <summary>Converts a <see cref="T:System.Decimal" /> value to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The <see langword="Decimal" /> value to convert.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Decimal" /> value.</returns>
    public static string ToString(Decimal Value)
    {
      return Conversions.ToString(Value, (NumberFormatInfo) null);
    }

    /// <summary>Converts a <see cref="T:System.Decimal" /> value to a <see cref="T:System.String" /> value, using the specified number format.</summary>
    /// <param name="Value">The <see langword="decimal" /> value to convert.</param>
    /// <param name="NumberFormat">The number format to use, according to <see cref="T:System.Globalization.NumberFormatInfo" />.</param>
    /// <returns>The <see langword="String" /> representation of the <see langword="Decimal" /> value.</returns>
    public static string ToString(Decimal Value, NumberFormatInfo NumberFormat)
    {
      return Value.ToString("G", (IFormatProvider) NumberFormat);
    }

    /// <summary>Converts an object to a <see cref="T:System.String" /> value.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <returns>The <see langword="String" /> representation of the object.</returns>
    public static string ToString(object Value)
    {
      switch (Value)
      {
        case null:
          return (string) null;
        case string str:
          return str;
        case IConvertible convertible:
          switch (convertible.GetTypeCode())
          {
            case TypeCode.Boolean:
              return Conversions.ToString(convertible.ToBoolean((IFormatProvider) null));
            case TypeCode.Char:
              return Conversions.ToString(convertible.ToChar((IFormatProvider) null));
            case TypeCode.SByte:
              return Conversions.ToString((int) convertible.ToSByte((IFormatProvider) null));
            case TypeCode.Byte:
              return Conversions.ToString(convertible.ToByte((IFormatProvider) null));
            case TypeCode.Int16:
              return Conversions.ToString((int) convertible.ToInt16((IFormatProvider) null));
            case TypeCode.UInt16:
              return Conversions.ToString((uint) convertible.ToUInt16((IFormatProvider) null));
            case TypeCode.Int32:
              return Conversions.ToString(convertible.ToInt32((IFormatProvider) null));
            case TypeCode.UInt32:
              return Conversions.ToString(convertible.ToUInt32((IFormatProvider) null));
            case TypeCode.Int64:
              return Conversions.ToString(convertible.ToInt64((IFormatProvider) null));
            case TypeCode.UInt64:
              return Conversions.ToString(convertible.ToUInt64((IFormatProvider) null));
            case TypeCode.Single:
              return Conversions.ToString(convertible.ToSingle((IFormatProvider) null));
            case TypeCode.Double:
              return Conversions.ToString(convertible.ToDouble((IFormatProvider) null));
            case TypeCode.Decimal:
              return Conversions.ToString(convertible.ToDecimal((IFormatProvider) null));
            case TypeCode.DateTime:
              return Conversions.ToString(convertible.ToDateTime((IFormatProvider) null));
            case TypeCode.String:
              return convertible.ToString((IFormatProvider) null);
          }
          break;
        case char[] chArray:
          return new string(chArray);
      }
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) Utils.VBFriendlyName(Value), (object) "String"));
    }

    /// <summary>Converts an object to a generic type <paramref name="T" />.</summary>
    /// <param name="Value">The object to convert.</param>
    /// <typeparam name="T">The type to convert <paramref name="Value" /> to.</typeparam>
    /// <returns>A structure or object of generic type <paramref name="T" />.</returns>
    public static T ToGenericParameter<T>(object Value)
    {
      T obj;
      if (Value == null)
      {
        obj = default (T);
      }
      else
      {
        switch (typeof (T).GetTypeCode())
        {
          case TypeCode.Boolean:
            obj = (T) (ValueType) Conversions.ToBoolean(Value);
            break;
          case TypeCode.Char:
            obj = (T) (ValueType) Conversions.ToChar(Value);
            break;
          case TypeCode.SByte:
            obj = (T) (ValueType) Conversions.ToSByte(Value);
            break;
          case TypeCode.Byte:
            obj = (T) (ValueType) Conversions.ToByte(Value);
            break;
          case TypeCode.Int16:
            obj = (T) (ValueType) Conversions.ToShort(Value);
            break;
          case TypeCode.UInt16:
            obj = (T) (ValueType) Conversions.ToUShort(Value);
            break;
          case TypeCode.Int32:
            obj = (T) (ValueType) Conversions.ToInteger(Value);
            break;
          case TypeCode.UInt32:
            obj = (T) (ValueType) Conversions.ToUInteger(Value);
            break;
          case TypeCode.Int64:
            obj = (T) (ValueType) Conversions.ToLong(Value);
            break;
          case TypeCode.UInt64:
            obj = (T) (ValueType) Conversions.ToULong(Value);
            break;
          case TypeCode.Single:
            obj = (T) (ValueType) Conversions.ToSingle(Value);
            break;
          case TypeCode.Double:
            obj = (T) (ValueType) Conversions.ToDouble(Value);
            break;
          case TypeCode.Decimal:
            obj = (T) (ValueType) Conversions.ToDecimal(Value);
            break;
          case TypeCode.DateTime:
            obj = (T) (ValueType) Conversions.ToDate(Value);
            break;
          case TypeCode.String:
            obj = (T) Conversions.ToString(Value);
            break;
          default:
            obj = (T) Value;
            break;
        }
      }
      return obj;
    }

    private static object CastSByteEnum(sbyte Expression, Type TargetType)
    {
      return !Symbols.IsEnum(TargetType) ? (object) Expression : Enum.ToObject(TargetType, Expression);
    }

    private static object CastByteEnum(byte Expression, Type TargetType)
    {
      return !Symbols.IsEnum(TargetType) ? (object) Expression : Enum.ToObject(TargetType, Expression);
    }

    private static object CastInt16Enum(short Expression, Type TargetType)
    {
      return !Symbols.IsEnum(TargetType) ? (object) Expression : Enum.ToObject(TargetType, Expression);
    }

    private static object CastUInt16Enum(ushort Expression, Type TargetType)
    {
      return !Symbols.IsEnum(TargetType) ? (object) Expression : Enum.ToObject(TargetType, Expression);
    }

    private static object CastInt32Enum(int Expression, Type TargetType)
    {
      return !Symbols.IsEnum(TargetType) ? (object) Expression : Enum.ToObject(TargetType, Expression);
    }

    private static object CastUInt32Enum(uint Expression, Type TargetType)
    {
      return !Symbols.IsEnum(TargetType) ? (object) Expression : Enum.ToObject(TargetType, Expression);
    }

    private static object CastInt64Enum(long Expression, Type TargetType)
    {
      return !Symbols.IsEnum(TargetType) ? (object) Expression : Enum.ToObject(TargetType, Expression);
    }

    private static object CastUInt64Enum(ulong Expression, Type TargetType)
    {
      return !Symbols.IsEnum(TargetType) ? (object) Expression : Enum.ToObject(TargetType, Expression);
    }

    internal static object ForceValueCopy(object Expression, Type TargetType)
    {
      object obj;
      if (!(Expression is IConvertible convertible))
      {
        obj = Expression;
      }
      else
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            obj = (object) convertible.ToBoolean((IFormatProvider) null);
            break;
          case TypeCode.Char:
            obj = (object) convertible.ToChar((IFormatProvider) null);
            break;
          case TypeCode.SByte:
            obj = Conversions.CastSByteEnum(convertible.ToSByte((IFormatProvider) null), TargetType);
            break;
          case TypeCode.Byte:
            obj = Conversions.CastByteEnum(convertible.ToByte((IFormatProvider) null), TargetType);
            break;
          case TypeCode.Int16:
            obj = Conversions.CastInt16Enum(convertible.ToInt16((IFormatProvider) null), TargetType);
            break;
          case TypeCode.UInt16:
            obj = Conversions.CastUInt16Enum(convertible.ToUInt16((IFormatProvider) null), TargetType);
            break;
          case TypeCode.Int32:
            obj = Conversions.CastInt32Enum(convertible.ToInt32((IFormatProvider) null), TargetType);
            break;
          case TypeCode.UInt32:
            obj = Conversions.CastUInt32Enum(convertible.ToUInt32((IFormatProvider) null), TargetType);
            break;
          case TypeCode.Int64:
            obj = Conversions.CastInt64Enum(convertible.ToInt64((IFormatProvider) null), TargetType);
            break;
          case TypeCode.UInt64:
            obj = Conversions.CastUInt64Enum(convertible.ToUInt64((IFormatProvider) null), TargetType);
            break;
          case TypeCode.Single:
            obj = (object) convertible.ToSingle((IFormatProvider) null);
            break;
          case TypeCode.Double:
            obj = (object) convertible.ToDouble((IFormatProvider) null);
            break;
          case TypeCode.Decimal:
            obj = (object) convertible.ToDecimal((IFormatProvider) null);
            break;
          case TypeCode.DateTime:
            obj = (object) convertible.ToDateTime((IFormatProvider) null);
            break;
          default:
            obj = Expression;
            break;
        }
      }
      return obj;
    }

    private static object ChangeIntrinsicType(object Expression, Type TargetType)
    {
      switch (TargetType.GetTypeCode())
      {
        case TypeCode.Boolean:
          return (object) Conversions.ToBoolean(Expression);
        case TypeCode.Char:
          return (object) Conversions.ToChar(Expression);
        case TypeCode.SByte:
          return Conversions.CastSByteEnum(Conversions.ToSByte(Expression), TargetType);
        case TypeCode.Byte:
          return Conversions.CastByteEnum(Conversions.ToByte(Expression), TargetType);
        case TypeCode.Int16:
          return Conversions.CastInt16Enum(Conversions.ToShort(Expression), TargetType);
        case TypeCode.UInt16:
          return Conversions.CastUInt16Enum(Conversions.ToUShort(Expression), TargetType);
        case TypeCode.Int32:
          return Conversions.CastInt32Enum(Conversions.ToInteger(Expression), TargetType);
        case TypeCode.UInt32:
          return Conversions.CastUInt32Enum(Conversions.ToUInteger(Expression), TargetType);
        case TypeCode.Int64:
          return Conversions.CastInt64Enum(Conversions.ToLong(Expression), TargetType);
        case TypeCode.UInt64:
          return Conversions.CastUInt64Enum(Conversions.ToULong(Expression), TargetType);
        case TypeCode.Single:
          return (object) Conversions.ToSingle(Expression);
        case TypeCode.Double:
          return (object) Conversions.ToDouble(Expression);
        case TypeCode.Decimal:
          return (object) Conversions.ToDecimal(Expression);
        case TypeCode.DateTime:
          return (object) Conversions.ToDate(Expression);
        case TypeCode.String:
          return (object) Conversions.ToString(Expression);
        default:
          throw new Exception();
      }
    }

    /// <summary>Converts an object to the specified type.</summary>
    /// <param name="Expression">The object to convert.</param>
    /// <param name="TargetType">The type to which to convert the object.</param>
    /// <returns>An object of the specified target type.</returns>
    public static object ChangeType(object Expression, Type TargetType)
    {
      return Conversions.ChangeType(Expression, TargetType, false);
    }

    [SecuritySafeCritical]
    internal static object ChangeType(object Expression, Type TargetType, bool Dynamic)
    {
      if ((object) TargetType == null)
        throw new ArgumentException(SR.Format(SR.Argument_InvalidNullValue1, (object) nameof (TargetType)));
      object obj;
      if (Expression == null)
      {
        obj = !Symbols.IsValueType(TargetType) ? (object) null : Activator.CreateInstance(TargetType);
      }
      else
      {
        Type type = Expression.GetType();
        if (TargetType.IsByRef)
          TargetType = TargetType.GetElementType();
        if ((object) TargetType == (object) type || Symbols.IsRootObjectType(TargetType))
          obj = Expression;
        else if (Symbols.IsIntrinsicType(TargetType.GetTypeCode()) && Symbols.IsIntrinsicType(type.GetTypeCode()))
          obj = Conversions.ChangeIntrinsicType(Expression, TargetType);
        else if (TargetType.IsInstanceOfType(Expression))
          obj = Expression;
        else if (Symbols.IsCharArrayRankOne(TargetType) && Symbols.IsStringType(type))
          obj = (object) Conversions.ToCharArrayRankOne((string) Expression);
        else if (Symbols.IsStringType(TargetType) && Symbols.IsCharArrayRankOne(type))
        {
          obj = (object) new string((char[]) Expression);
        }
        else
        {
          if (Dynamic)
          {
            IDynamicMetaObjectProvider idmop = IDOUtils.TryCastToIDMOP(Expression);
            if (idmop != null)
            {
              obj = IDOBinder.UserDefinedConversion(idmop, TargetType);
              goto label_20;
            }
          }
          obj = Conversions.ObjectUserDefinedConversion(Expression, TargetType);
        }
      }
label_20:
      return obj;
    }

    /// <summary>Converts an object to the specified type.</summary>
    /// <param name="Expression">The object to convert.</param>
    /// <param name="TargetType">The type to which to convert the object.</param>
    /// <returns>An object of the specified target type.</returns>
    [Obsolete("do not use this method", true)]
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object FallbackUserDefinedConversion(object Expression, Type TargetType)
    {
      return Conversions.ObjectUserDefinedConversion(Expression, TargetType);
    }

    [DebuggerHidden]
    [DebuggerStepThrough]
    private static object ObjectUserDefinedConversion(object Expression, Type TargetType)
    {
      Type type = Expression.GetType();
      if (ConversionResolution.ClassifyPredefinedConversion(TargetType, type) == ConversionResolution.ConversionClass.None && (Symbols.IsClassOrValueType(type) || Symbols.IsClassOrValueType(TargetType)) && (!Symbols.IsIntrinsicType(type) || !Symbols.IsIntrinsicType(TargetType)))
      {
        Symbols.Method operatorMethod = (Symbols.Method) null;
        ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyUserDefinedConversion(TargetType, type, ref operatorMethod);
        if ((object) operatorMethod != null)
          return Conversions.ChangeType(new Symbols.Container(operatorMethod.DeclaringType).InvokeMethod(operatorMethod, new object[1]
          {
            Expression
          }, (bool[]) null, BindingFlags.InvokeMethod), TargetType);
        if (conversionClass == ConversionResolution.ConversionClass.Ambiguous)
          throw new InvalidCastException(Utils.GetResourceString(SR.AmbiguousCast2, Utils.VBFriendlyName(type), Utils.VBFriendlyName(TargetType)));
      }
      throw new InvalidCastException(Utils.GetResourceString(SR.InvalidCast_FromTo, Utils.VBFriendlyName(type), Utils.VBFriendlyName(TargetType)));
    }

    internal static bool CanUserDefinedConvert(object Expression, Type TargetType)
    {
      Type type = Expression.GetType();
      bool flag;
      if (ConversionResolution.ClassifyPredefinedConversion(TargetType, type) == ConversionResolution.ConversionClass.None && (Symbols.IsClassOrValueType(type) || Symbols.IsClassOrValueType(TargetType)) && (!Symbols.IsIntrinsicType(type) || !Symbols.IsIntrinsicType(TargetType)))
      {
        Symbols.Method operatorMethod = (Symbols.Method) null;
        int num = (int) ConversionResolution.ClassifyUserDefinedConversion(TargetType, type, ref operatorMethod);
        flag = operatorMethod != null;
      }
      else
        flag = false;
      return flag;
    }
  }
}
