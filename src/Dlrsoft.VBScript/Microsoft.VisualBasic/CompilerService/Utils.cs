using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerService
{
    public class Utils
    {
        internal static CultureInfo GetCultureInfo()
        {
            return Thread.CurrentThread.CurrentCulture;
        }
        internal static DateTimeFormatInfo GetDateTimeFormatInfo()
        {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat;
        }

        internal static bool IsHexOrOctValue(string Value, ref long i64Value)
        {
            int length = Value.Length;
            checked
            {
                int num = default(int);
                char c;
                while (true)
                {
                    if (num < length)
                    {
                        c = Value[num];
                        if (c == '&' && num + 2 < length)
                        {
                            break;
                        }
                        if (c != ' ' && c != '\u3000')
                        {
                            return false;
                        }
                        num++;
                        continue;
                    }
                    return false;
                }
                c = char.ToLower(Value[num + 1], CultureInfo.InvariantCulture);
                string value = ToHalfwidthNumbers(Value.Substring(num + 2), GetCultureInfo());
                switch (c)
                {
                    case 'h':
                        i64Value = Convert.ToInt64(value, 16);
                        break;
                    case 'o':
                        i64Value = Convert.ToInt64(value, 8);
                        break;
                    default:
                        throw new FormatException();
                }
                return true;
            }
        }
        internal static bool IsHexOrOctValue(string Value, ref ulong ui64Value)
        {
            int length = Value.Length;
            checked
            {
                int num = default(int);
                char c;
                while (true)
                {
                    if (num < length)
                    {
                        c = Value[num];
                        if (c == '&' && num + 2 < length)
                        {
                            break;
                        }
                        if (c != ' ' && c != '\u3000')
                        {
                            return false;
                        }
                        num++;
                        continue;
                    }
                    return false;
                }
                c = char.ToLower(Value[num + 1], CultureInfo.InvariantCulture);
                string value = ToHalfwidthNumbers(Value.Substring(num + 2), GetCultureInfo());
                switch (c)
                {
                    case 'h':
                        ui64Value = Convert.ToUInt64(value, 16);
                        break;
                    case 'o':
                        ui64Value = Convert.ToUInt64(value, 8);
                        break;
                    default:
                        throw new FormatException();
                }
                return true;
            }
        }


        /// <summary>Used by the Visual Basic compiler as a helper for Redim.</summary>
        /// <returns>The copied array.</returns>
        /// <param name="arySrc">The array to be copied.</param>
        /// <param name="aryDest">The destination array.</param>
        public static Array CopyArray(Array arySrc, Array aryDest)
        {
            if (arySrc == null)
            {
                return aryDest;
            }
            int length = arySrc.Length;
            if (length == 0)
            {
                return aryDest;
            }
            if (aryDest.Rank != arySrc.Rank)
            {
                throw new InvalidCastException(GetResourceString("Array_RankMismatch"));
            }
            checked
            {
                int num = aryDest.Rank - 2;
                for (int i = 0; i <= num; i++)
                {
                    if (aryDest.GetUpperBound(i) != arySrc.GetUpperBound(i))
                    {
                        throw new ArrayTypeMismatchException(GetResourceString("Array_TypeMismatch"));
                    }
                }
                if (length > aryDest.Length)
                {
                    length = aryDest.Length;
                }
                if (arySrc.Rank > 1)
                {
                    int rank = arySrc.Rank;
                    int length2 = arySrc.GetLength(rank - 1);
                    int length3 = aryDest.GetLength(rank - 1);
                    if (length3 == 0)
                    {
                        return aryDest;
                    }
                    int length4 = Math.Min(length2, length3);
                    int num2 = unchecked(arySrc.Length / length2) - 1;
                    for (int j = 0; j <= num2; j++)
                    {
                        Array.Copy(arySrc, j * length2, aryDest, j * length3, length4);
                    }
                }
                else
                {
                    Array.Copy(arySrc, aryDest, length);
                }
                return aryDest;
            }
        }

        private static Lazy<ResourceManager> m_VBAResourceManager = new Lazy<ResourceManager>(() => new ResourceManager("Microsoft.VisualBasic", Assembly.GetExecutingAssembly()), true);
        internal static ResourceManager VBAResourceManager => m_VBAResourceManager.Value;

        internal static string GetResourceString(vbErrors resourceId)
        {
            return GetResourceString("ID" + Conversions.ToString((int)resourceId));
        }
        internal static string GetResourceString(string resourceKey)
        {
            if (VBAResourceManager == null)
            {
                return "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";
            }
            string text;
            try
            {
                text = VBAResourceManager.GetString(resourceKey, GetCultureInfo());
                if (text == null)
                {
                    text = VBAResourceManager.GetString("ID95");
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
            catch (Exception)
            {
                text = "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";
            }
            return text;
        }
        internal static string GetResourceString(string resourceKey, params string[] args)
        {
            string text = null;
            string text2 = null;
            try
            {
                text = GetResourceString(resourceKey);
                text2 = string.Format(Thread.CurrentThread.CurrentCulture, text, args);
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
            }
            if (string.CompareOrdinal(text2, "") != 0)
            {
                return text2;
            }
            return text;
        }

        internal static string VBFriendlyName(object Obj)
        {
            if (Obj == null)
            {
                return "Nothing";
            }
            return VBFriendlyName(Obj.GetType());
        }
        internal static string VBFriendlyName(Type typ)
        {
            return VBFriendlyNameOfType(typ);
        }
        internal static string VBFriendlyNameOfType(Type typ, bool FullName = false)
        {
            string arraySuffixAndElementType = GetArraySuffixAndElementType(ref typ);
            string text;
            switch (typ.IsEnum ? TypeCode.Object : Type.GetTypeCode(typ))
            {
                case TypeCode.Boolean:
                    text = "Boolean";
                    break;
                case TypeCode.SByte:
                    text = "SByte";
                    break;
                case TypeCode.Byte:
                    text = "Byte";
                    break;
                case TypeCode.Int16:
                    text = "Short";
                    break;
                case TypeCode.UInt16:
                    text = "UShort";
                    break;
                case TypeCode.Int32:
                    text = "Integer";
                    break;
                case TypeCode.UInt32:
                    text = "UInteger";
                    break;
                case TypeCode.Int64:
                    text = "Long";
                    break;
                case TypeCode.UInt64:
                    text = "ULong";
                    break;
                case TypeCode.Decimal:
                    text = "Decimal";
                    break;
                case TypeCode.Single:
                    text = "Single";
                    break;
                case TypeCode.Double:
                    text = "Double";
                    break;
                case TypeCode.DateTime:
                    text = "Date";
                    break;
                case TypeCode.Char:
                    text = "Char";
                    break;
                case TypeCode.String:
                    text = "String";
                    break;
                case TypeCode.DBNull:
                    text = "DBNull";
                    break;
                default:
                    {
                        if (typ.IsGenericParameter)
                        {
                            text = typ.Name;
                            break;
                        }
                        string text2 = null;
                        string genericArgsSuffix = GetGenericArgsSuffix(typ);
                        string text3;
                        if (FullName)
                        {
                            if (typ.IsNested)
                            {
                                text2 = VBFriendlyNameOfType(typ.DeclaringType, FullName: true);
                                text3 = typ.Name;
                            }
                            else
                            {
                                text3 = typ.FullName;
                                if (text3 == null)
                                {
                                    text3 = typ.Name;
                                }
                            }
                        }
                        else
                        {
                            text3 = typ.Name;
                        }
                        if (genericArgsSuffix != null)
                        {
                            int num = text3.LastIndexOf('`');
                            if (num != -1)
                            {
                                text3 = text3.Substring(0, num);
                            }
                            text = text3 + genericArgsSuffix;
                        }
                        else
                        {
                            text = text3;
                        }
                        if (text2 != null)
                        {
                            text = text2 + "." + text;
                        }
                        break;
                    }
            }
            if (arraySuffixAndElementType != null)
            {
                text += arraySuffixAndElementType;
            }
            return text;
        }

        internal static string AdjustArraySuffix(string sRank)
        {
            string text = null;
            checked
            {
                for (int num = sRank.Length; num > 0; num--)
                {
                    char c = sRank[num - 1];
                    switch (c)
                    {
                        case ')':
                            text += "(";
                            break;
                        case '(':
                            text += ")";
                            break;
                        case ',':
                            text += Conversions.ToString(c);
                            break;
                        default:
                            text = Conversions.ToString(c) + text;
                            break;
                    }
                }
                return text;
            }
        }


        private static string GetArraySuffixAndElementType(ref Type typ)
        {
            if (!typ.IsArray)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                stringBuilder.Append("(");
                stringBuilder.Append(',', checked(typ.GetArrayRank() - 1));
                stringBuilder.Append(")");
                typ = typ.GetElementType();
            }
            while (typ.IsArray);
            return stringBuilder.ToString();
        }
        private static string GetGenericArgsSuffix(Type typ)
        {
            if (!typ.IsGenericType)
            {
                return null;
            }
            Type[] genericArguments = typ.GetGenericArguments();
            int num = genericArguments.Length;
            int num2 = num;
            checked
            {
                if (typ.IsNested && typ.DeclaringType.IsGenericType)
                {
                    num2 -= typ.DeclaringType.GetGenericArguments().Length;
                }
                if (num2 == 0)
                {
                    return null;
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("(Of ");
                int num3 = num - num2;
                int num4 = num - 1;
                for (int i = num3; i <= num4; i++)
                {
                    stringBuilder.Append(VBFriendlyNameOfType(genericArguments[i]));
                    if (i != num - 1)
                    {
                        stringBuilder.Append(',');
                    }
                }
                stringBuilder.Append(")");
                return stringBuilder.ToString();
            }
        }

        internal static string ToHalfwidthNumbers(string s, CultureInfo culture)
        {
            return s;

            //int num = culture.LCID & 0x3FF;
            //if (num != 4 && num != 17 && num != 18)
            //{
            //    return s;
            //}
            //return Strings.vbLCMapString(culture, 4194304, s);
        }

        internal static string OctFromLong(long Val)
        {
            string text = "";
            int num = Convert.ToInt32('0');
            checked
            {
                bool flag = default(bool);
                if (Val < 0)
                {
                    Val = long.MaxValue + Val + 1;
                    flag = true;
                }
                do
                {
                    int num2 = (int)unchecked(Val % 8);
                    Val >>= 3;
                    text += Conversions.ToString(Strings.ChrW(num2 + num));
                }
                while (Val > 0);
                text = Strings.StrReverse(text);
                if (flag)
                {
                    text = "1" + text;
                }
                return text;
            }
        }
        internal static string OctFromULong(ulong Val)
        {
            string text = "";
            int num = Convert.ToInt32('0');
            checked
            {
                do
                {
                    int num2 = (int)unchecked(Val % 8uL);
                    Val >>= 3;
                    text += Conversions.ToString(Strings.ChrW(num2 + num));
                }
                while (Val != 0L);
                return Strings.StrReverse(text);
            }
        }
        internal static string StdFormat(string s)
        {
            NumberFormatInfo numberFormat = Thread.CurrentThread.CurrentCulture.NumberFormat;
            int num = s.IndexOf(numberFormat.NumberDecimalSeparator);
            if (num == -1)
            {
                return s;
            }
            char c = default(char);
            char c2 = default(char);
            char c3 = default(char);
            try
            {
                c = s[0];
                c2 = s[1];
                c3 = s[2];
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
            }
            if (s[num] == '.')
            {
                if (c == '0' && c2 == '.')
                {
                    return s.Substring(1);
                }
                if ((c != '-' && c != '+' && c != ' ') || c2 != '0' || c3 != '.')
                {
                    return s;
                }
            }
            StringBuilder stringBuilder = new StringBuilder(s);
            stringBuilder[num] = '.';
            if (c == '0' && c2 == '.')
            {
                return stringBuilder.ToString(1, checked(stringBuilder.Length - 1));
            }
            if ((c == '-' || c == '+' || c == ' ') && c2 == '0' && c3 == '.')
            {
                stringBuilder.Remove(1, 1);
                return stringBuilder.ToString();
            }
            return stringBuilder.ToString();
        }

        internal static Encoding GetFileIOEncoding()
        {
            return Encoding.Default;
        }

        internal static int GetLocaleCodePage()
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage;
        }
    }
}
