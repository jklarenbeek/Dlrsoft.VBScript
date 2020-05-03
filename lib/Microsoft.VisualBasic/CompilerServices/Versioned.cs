// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.Versioned
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>The <see cref="T:Microsoft.VisualBasic.CompilerServices.Versioned" /> module contains procedures used to interact with objects, applications, and systems.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class Versioned
  {
    /// <summary>Returns a Boolean value indicating whether an expression can be evaluated as a number.</summary>
    /// <param name="Expression">Required. <see langword="Object" /> expression.</param>
    /// <returns>Returns a Boolean value indicating whether an expression can be evaluated as a number.</returns>
    public static bool IsNumeric(object Expression)
    {
      bool flag;
      if (!(Expression is IConvertible convertible))
      {
        flag = false;
      }
      else
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.Boolean:
            flag = true;
            break;
          case TypeCode.Char:
          case TypeCode.String:
            string str = convertible.ToString((IFormatProvider) null);
            try
            {
              long i64Value = 0;
              if (Utils.IsHexOrOctValue(str, ref i64Value))
              {
                flag = true;
                break;
              }
            }
            catch (FormatException ex)
            {
              flag = false;
              break;
            }
            double Result = 0;
            flag = Conversions.TryParseDouble(str, ref Result);
            break;
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
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
      }
      return flag;
    }
  }
}
