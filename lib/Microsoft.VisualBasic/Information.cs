// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.Information
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Reflection;
using System.Threading;

namespace Microsoft.VisualBasic
{
    /// <summary>The <see langword="Information" /> module contains the procedures used to return, test for, or verify information.</summary>
    [StandardModule]
    public sealed class Information
    {
        private static readonly int[] QBColorTable = new int[16]
        {
      0,
      8388608,
      32768,
      8421376,
      128,
      8388736,
      32896,
      12632256,
      8421504,
      16711680,
      65280,
      16776960,
      (int) byte.MaxValue,
      16711935,
      (int) ushort.MaxValue,
      16777215
        };

        /// <summary>Returns a <see langword="Boolean" /> value indicating whether a variable points to an array.</summary>
        /// <param name="VarName">Required. <see langword="Object" /> variable.</param>
        /// <returns>Returns a <see langword="Boolean" /> value indicating whether a variable points to an array.</returns>
        public static bool IsArray(object VarName)
        {
            return VarName != null && VarName is Array;
        }

        /// <summary>Returns a <see langword="Boolean" /> value indicating whether an expression represents a valid <see langword="Date" /> value.</summary>
        /// <param name="Expression">Required. <see langword="Object" /> expression.</param>
        /// <returns>Returns a <see langword="Boolean" /> value indicating whether an expression represents a valid <see langword="Date" /> value.</returns>
        public static bool IsDate(object Expression)
        {
            bool flag;
            switch (Expression)
            {
                case null:
                    flag = false;
                    break;
                case DateTime _:
                    flag = true;
                    break;
                case string str:
                    DateTime Result = DateTime.MinValue;
                    flag = Conversions.TryParseDate(str, ref Result);
                    break;
                default:
                    flag = false;
                    break;
            }
            return flag;
        }

        /// <summary>Returns a <see langword="Boolean" /> value indicating whether an expression evaluates to the <see cref="T:System.DBNull" /> class.</summary>
        /// <param name="Expression">Required. <see langword="Object" /> expression.</param>
        /// <returns>Returns a <see langword="Boolean" /> value indicating whether an expression evaluates to the <see cref="T:System.DBNull" /> class.</returns>
        public static bool IsDBNull(object Expression)
        {
            return Expression != null && Expression is DBNull;
        }

        /// <summary>Returns a <see langword="Boolean" /> value indicating whether an expression has no object assigned to it.</summary>
        /// <param name="Expression">Required. <see langword="Object" /> expression.</param>
        /// <returns>Returns a <see langword="Boolean" /> value indicating whether an expression has no object assigned to it.</returns>
        public static bool IsNothing(object Expression)
        {
            return Expression == null;
        }

        /// <summary>Returns a <see langword="Boolean" /> value indicating whether an expression is an exception type.</summary>
        /// <param name="Expression">Required. <see langword="Object" /> expression.</param>
        /// <returns>Returns a <see langword="Boolean" /> value indicating whether an expression is an exception type.</returns>
        public static bool IsError(object Expression)
        {
            return Expression != null && Expression is Exception;
        }

        /// <summary>Returns a <see langword="Boolean" /> value indicating whether an expression evaluates to a reference type.</summary>
        /// <param name="Expression">Required. <see langword="Object" /> expression.</param>
        /// <returns>Returns a <see langword="Boolean" /> value indicating whether an expression evaluates to a reference type.</returns>
        public static bool IsReference(object Expression)
        {
            return !(Expression is ValueType);
        }

        /// <summary>Returns the lowest available subscript for the indicated dimension of an array.</summary>
        /// <param name="Array">Required. Array of any data type. The array in which you want to find the lowest possible subscript of a dimension.</param>
        /// <param name="Rank">Optional. <see langword="Integer" />. The dimension for which the lowest possible subscript is to be returned. Use 1 for the first dimension, 2 for the second, and so on. If <paramref name="Rank" /> is omitted, 1 is assumed.</param>
        /// <returns>
        /// <see langword="Integer" />. The lowest value the subscript for the specified dimension can contain. <see langword="LBound" /> always returns 0 as long as <paramref name="Array" /> has been initialized, even if it has no elements, for example if it is a zero-length string. If <paramref name="Array" /> is <see langword="Nothing" />, <see langword="LBound" /> throws an <see cref="T:System.ArgumentNullException" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="Array" /> is <see langword="Nothing" />.</exception>
        /// <exception cref="T:System.RankException">
        /// <paramref name="Rank" /> less than 1, or <paramref name="Rank" /> is greater than the rank of <paramref name="Array" />.</exception>
        public static int LBound(Array Array, int Rank = 1)
        {
            if (Array == null)
                throw ExceptionUtils.VbMakeException((Exception)new ArgumentNullException(nameof(Array)), 9);
            if (Rank < 1 || Rank > Array.Rank)
                throw new RankException(SR.Format(SR.Argument_InvalidRank1, (object)nameof(Rank)));
            return Array.GetLowerBound(checked(Rank - 1));
        }

        /// <summary>Returns the highest available subscript for the indicated dimension of an array.</summary>
        /// <param name="Array">Required. Array of any data type. The array in which you want to find the highest possible subscript of a dimension.</param>
        /// <param name="Rank">Optional. <see langword="Integer" />. The dimension for which the highest possible subscript is to be returned. Use 1 for the first dimension, 2 for the second, and so on. If <paramref name="Rank" /> is omitted, 1 is assumed.</param>
        /// <returns>
        /// <see langword="Integer" />. The highest value the subscript for the specified dimension can contain. If <paramref name="Array" /> has only one element, <see langword="UBound" /> returns 0. If <paramref name="Array" /> has no elements, for example if it is a zero-length string, <see langword="UBound" /> returns -1.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="Array" /> is <see langword="Nothing" />.</exception>
        /// <exception cref="T:System.RankException">
        /// <paramref name="Rank" /> is less than 1, or <paramref name="Rank" /> is greater than the rank of <paramref name="Array" />.</exception>
        public static int UBound(Array Array, int Rank = 1)
        {
            if (Array == null)
                throw ExceptionUtils.VbMakeException((Exception)new ArgumentNullException(nameof(Array)), 9);
            if (Rank < 1 || Rank > Array.Rank)
                throw new RankException(SR.Format(SR.Argument_InvalidRank1, (object)nameof(Rank)));
            return Array.GetUpperBound(checked(Rank - 1));
        }

        /// <summary>Returns an <see langword="Integer" /> value representing the RGB color code corresponding to the specified color number.</summary>
        /// <param name="Color">Required. A whole number in the range 0-15.</param>
        /// <returns>Returns an <see langword="Integer" /> value representing the RGB color code corresponding to the specified color number.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Color" /> is outside of range 0 to 15, inclusive.</exception>
        public static int QBColor(int Color)
        {
            if ((Color & 65520) != 0)
                throw new ArgumentException(SR.Format(SR.Argument_InvalidValue1, (object)nameof(Color)), nameof(Color));
            return Information.QBColorTable[Color];
        }

        /// <summary>Returns an <see langword="Integer" /> value representing an RGB color value from a set of red, green and blue color components.</summary>
        /// <param name="Red">Required. <see langword="Integer" /> in the range 0-255, inclusive, that represents the intensity of the red component of the color.</param>
        /// <param name="Green">Required. <see langword="Integer" /> in the range 0-255, inclusive, that represents the intensity of the green component of the color.</param>
        /// <param name="Blue">Required. <see langword="Integer" /> in the range 0-255, inclusive, that represents the intensity of the blue component of the color.</param>
        /// <returns>Returns an <see langword="Integer" /> value representing an RGB color value from a set of red, green and blue color components.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Green" />, <paramref name="Blue" />, or <paramref name="Red" /> is outside of range 0 to 255, inclusive.</exception>
        public static int RGB(int Red, int Green, int Blue)
        {
            if ((Red & int.MinValue) != 0)
                throw new ArgumentException(SR.Format(SR.Argument_InvalidValue1, (object)nameof(Red)), nameof(Red));
            if ((Green & int.MinValue) != 0)
                throw new ArgumentException(SR.Format(SR.Argument_InvalidValue1, (object)nameof(Green)), nameof(Green));
            if ((Blue & int.MinValue) != 0)
                throw new ArgumentException(SR.Format(SR.Argument_InvalidValue1, (object)nameof(Blue)), nameof(Blue));
            if (Red > (int)byte.MaxValue)
                Red = (int)byte.MaxValue;
            if (Green > (int)byte.MaxValue)
                Green = (int)byte.MaxValue;
            if (Blue > (int)byte.MaxValue)
                Blue = (int)byte.MaxValue;
            return checked(Blue * 65536 + Green * 256 + Red);
        }

        /// <summary>Returns an <see langword="Integer" /> value containing the data type classification of a variable.</summary>
        /// <param name="VarName">Required. <see langword="Object" /> variable. If <see langword="Option Strict" /> is <see langword="Off" />, you can pass a variable of any data type except a structure.</param>
        /// <returns>Returns an <see langword="Integer" /> value containing the data type classification of a variable.</returns>
        public static VariantType VarType(object VarName)
        {
            return VarName != null ? Information.VarTypeFromComType(VarName.GetType()) : VariantType.Object;
        }

        internal static VariantType VarTypeFromComType(Type typ)
        {
            VariantType variantType1;
            if ((object)typ == null)
                variantType1 = VariantType.Object;
            else if (typ.IsArray)
            {
                typ = typ.GetElementType();
                if (typ.IsArray)
                {
                    variantType1 = VariantType.Object | VariantType.Array;
                }
                else
                {
                    VariantType variantType2 = Information.VarTypeFromComType(typ);
                    variantType1 = (variantType2 & VariantType.Array) == VariantType.Empty ? variantType2 | VariantType.Array : VariantType.Object | VariantType.Array;
                }
            }
            else
            {
                if (typ.IsEnum)
                    typ = Enum.GetUnderlyingType(typ);
                if ((object)typ == null)
                {
                    variantType1 = VariantType.Empty;
                }
                else
                {
                    switch (Type.GetTypeCode(typ))
                    {
                        case TypeCode.DBNull:
                            variantType1 = VariantType.Null;
                            break;
                        case TypeCode.Boolean:
                            variantType1 = VariantType.Boolean;
                            break;
                        case TypeCode.Char:
                            variantType1 = VariantType.Char;
                            break;
                        case TypeCode.Byte:
                            variantType1 = VariantType.Byte;
                            break;
                        case TypeCode.Int16:
                            variantType1 = VariantType.Short;
                            break;
                        case TypeCode.Int32:
                            variantType1 = VariantType.Integer;
                            break;
                        case TypeCode.Int64:
                            variantType1 = VariantType.Long;
                            break;
                        case TypeCode.Single:
                            variantType1 = VariantType.Single;
                            break;
                        case TypeCode.Double:
                            variantType1 = VariantType.Double;
                            break;
                        case TypeCode.Decimal:
                            variantType1 = VariantType.Decimal;
                            break;
                        case TypeCode.DateTime:
                            variantType1 = VariantType.Date;
                            break;
                        case TypeCode.String:
                            variantType1 = VariantType.String;
                            break;
                        default:
                            variantType1 = (object)typ == (object)typeof(Missing) || (object)typ == (object)typeof(Exception) || typ.IsSubclassOf(typeof(Exception)) ? VariantType.Error : (!typ.IsValueType ? VariantType.Object : VariantType.UserDefinedType);
                            break;
                    }
                }
            }
            return variantType1;
        }

        internal static bool IsOldNumericTypeCode(TypeCode TypCode)
        {
            bool flag;
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
                    flag = true;
                    break;
                default:
                    flag = false;
                    break;
            }
            return flag;
        }

        /// <summary>Returns a <see langword="Boolean" /> value indicating whether an expression can be evaluated as a number.</summary>
        /// <param name="Expression">Required. <see langword="Object" /> expression.</param>
        /// <returns>Returns a <see langword="Boolean" /> value indicating whether an expression can be evaluated as a number.</returns>
        public static bool IsNumeric(object Expression)
        {
            bool flag;

            if (Expression is IConvertible convertible)
            {

            }
            else if (Expression is char[] chArray)
            {

            }
            else
            {

            }


            switch (Expression)
            {
                case IConvertible:
                label_3:
                    IConvertible convertible = (IConvertible)Expression;
                    TypeCode typeCode = convertible.GetTypeCode();
                    switch (typeCode)
                    {
                        case TypeCode.Char:
                        case TypeCode.String:
                            string str = convertible.ToString((IFormatProvider)null);
                            try
                            {
                                long i64Value = 0;
                                if (Utils.IsHexOrOctValue(str, ref i64Value))
                                {
                                    flag = true;
                                    break;
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
                                flag = false;
                                break;
                            }
                            double Result = 0;
                            flag = DoubleType.TryParse(str, ref Result);
                            break;
                        default:
                            flag = Information.IsOldNumericTypeCode(typeCode);
                            break;
                    }
                    break;
                case char[] chArray:
                    Expression = (object)new string(chArray);
                    goto label_3;
                default:
                    flag = false;
                    break;
            }
            return flag;
        }

        /// <summary>Returns a <see langword="String" /> value containing data-type information about a variable.</summary>
        /// <param name="VarName">Required. <see langword="Object" /> variable. If <see langword="Option Strict" /> is <see langword="Off" />, you can pass a variable of any data type except a structure.</param>
        /// <returns>Returns a <see langword="String" /> value containing data-type information about a variable.</returns>
        public static string TypeName(object VarName)
        {
            if (VarName == null)
                return "Nothing";
            Type type = VarName.GetType();
            bool flag = false;
            if (type.IsArray)
            {
                flag = true;
                type = type.GetElementType();
            }
            string strA;
            if (type.IsEnum)
            {
                strA = type.Name;
            }
            else
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.DBNull:
                        strA = "DBNull";
                        goto label_23;
                    case TypeCode.Boolean:
                        strA = "Boolean";
                        goto label_23;
                    case TypeCode.Char:
                        strA = "Char";
                        goto label_23;
                    case TypeCode.Byte:
                        strA = "Byte";
                        goto label_23;
                    case TypeCode.Int16:
                        strA = "Short";
                        goto label_23;
                    case TypeCode.Int32:
                        strA = "Integer";
                        goto label_23;
                    case TypeCode.Int64:
                        strA = "Long";
                        goto label_23;
                    case TypeCode.Single:
                        strA = "Single";
                        goto label_23;
                    case TypeCode.Double:
                        strA = "Double";
                        goto label_23;
                    case TypeCode.Decimal:
                        strA = "Decimal";
                        goto label_23;
                    case TypeCode.DateTime:
                        strA = "Date";
                        goto label_23;
                    case TypeCode.String:
                        strA = "String";
                        goto label_23;
                    default:
                        strA = type.Name;
                        break;
                }
            }
            int num = strA.IndexOf('+');
            if (num >= 0)
                strA = strA.Substring(checked(num + 1));
            label_23:
            if (flag)
            {
                Array array = (Array)VarName;
                strA = Information.OldVBFriendlyNameOfTypeName(array.Rank != 1 ? strA + "[" + new string(',', checked(array.Rank - 1)) + "]" : strA + "[]");
            }
            return strA;
        }

        /// <summary>Returns a <see langword="String" /> value containing the Visual Basic data type name of a variable.</summary>
        /// <param name="UrtName">Required. <see langword="String" /> variable containing a type name used by the common language runtime.</param>
        /// <returns>Returns a <see langword="String" /> value containing the Visual Basic data type name of a variable.</returns>
        public static string VbTypeName(string UrtName)
        {
            return Information.OldVbTypeName(UrtName);
        }

        internal static string OldVbTypeName(string UrtName)
        {
            UrtName = Strings.Trim(UrtName).ToUpperInvariant();
            if (Operators.CompareString(Strings.Left(UrtName, 7), "SYSTEM.", false) == 0)
                UrtName = Strings.Mid(UrtName, 8);
            string Left = UrtName;
            if (Operators.CompareString(Left, "OBJECT", false) == 0)
                return "Object";
            if (Operators.CompareString(Left, "INT16", false) == 0)
                return "Short";
            if (Operators.CompareString(Left, "INT32", false) == 0)
                return "Integer";
            if (Operators.CompareString(Left, "SINGLE", false) == 0)
                return "Single";
            if (Operators.CompareString(Left, "DOUBLE", false) == 0)
                return "Double";
            if (Operators.CompareString(Left, "DATETIME", false) == 0)
                return "Date";
            if (Operators.CompareString(Left, "STRING", false) == 0)
                return "String";
            if (Operators.CompareString(Left, "BOOLEAN", false) == 0)
                return "Boolean";
            if (Operators.CompareString(Left, "DECIMAL", false) == 0)
                return "Decimal";
            if (Operators.CompareString(Left, "BYTE", false) == 0)
                return "Byte";
            if (Operators.CompareString(Left, "CHAR", false) == 0)
                return "Char";
            return Operators.CompareString(Left, "INT64", false) == 0 ? "Long" : (string)null;
        }

        internal static string OldVBFriendlyNameOfTypeName(string typename)
        {
            string sRank = (string)null;
            int index = checked(typename.Length - 1);
            if (typename[index] == ']')
            {
                int num = typename.IndexOf('[');
                sRank = checked(num + 1) != index ? typename.Substring(num, checked(index - num + 1)).Replace('[', '(').Replace(']', ')') : "()";
                typename = typename.Substring(0, num);
            }
            string str = Information.OldVbTypeName(typename) ?? typename;
            return sRank == null ? str : str + Utils.AdjustArraySuffix(sRank);
        }
    }
}
