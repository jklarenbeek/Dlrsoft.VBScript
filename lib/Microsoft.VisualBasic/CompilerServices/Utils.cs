// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.Utils
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
    /// <summary>Contains utilities that the Visual Basic compiler uses.</summary>
    [DebuggerNonUserCode]
    public sealed class Utils
    {
        private static readonly object s_resourceManagerSyncObj = new object();
        internal static char[] m_achIntlSpace = new char[2]
        {
      ' ',
      '　'
        };
        private static readonly Type s_voidType = Type.GetType("System.Void");
        private static Assembly s_VBRuntimeAssembly;

        private static char IntToHex(int n)
        {
            return n > 9 ? Strings.ChrW(checked(n - 10 + 97)) : Strings.ChrW(checked(n + 48));
        }

        internal static string GetResourceString(vbErrors ResourceId)
        {
            string str = "ID" + Conversions.ToString((int)ResourceId);
            return SR.GetResourceString(str, str);
        }

        /// <summary>Retrieves and formats a localized resource string or error message.</summary>
        /// <param name="ResourceKey">The identifier of the string or error message to retrieve.</param>
        /// <param name="Args">An array of parameters to replace placeholders in the string or error message.</param>
        /// <returns>A formatted resource string or error message.</returns>
        internal static string GetResourceString(string resourceKey, params string[] args)
        {
            return SR.Format(resourceKey, (object[])args);
        }

        internal static CultureInfo GetCultureInfo()
        {
            return CultureInfo.CurrentCulture;
        }

        internal static CultureInfo GetInvariantCultureInfo()
        {
            return CultureInfo.InvariantCulture;
        }

        internal static Assembly VBRuntimeAssembly
        {
            get
            {
                Assembly vbRuntimeAssembly;
                if ((object)Utils.s_VBRuntimeAssembly != null)
                {
                    vbRuntimeAssembly = Utils.s_VBRuntimeAssembly;
                }
                else
                {
                    Utils.s_VBRuntimeAssembly = typeof(Utils).Assembly;
                    vbRuntimeAssembly = Utils.s_VBRuntimeAssembly;
                }
                return vbRuntimeAssembly;
            }
        }

        internal static string ToHalfwidthNumbers(string s, CultureInfo culture)
        {
            return s;
        }

        internal static bool IsHexOrOctValue(string value, ref long i64Value)
        {
            int length = value.Length;
            int index = 0;
            bool flag;
            while (index < length)
            {
                char ch = value[index];
                if (ch != '&' || checked(index + 2) >= length)
                {
                    if (ch != ' ' && ch != '　')
                    {
                        flag = false;
                        goto label_13;
                    }
                    else
                        checked { ++index; }
                }
                else
                {
                    char lowerInvariant = char.ToLowerInvariant(value[checked(index + 1)]);
                    string halfwidthNumbers = Utils.ToHalfwidthNumbers(value.Substring(checked(index + 2)), Utils.GetCultureInfo());
                    if (lowerInvariant == 'h')
                    {
                        i64Value = Convert.ToInt64(halfwidthNumbers, 16);
                    }
                    else
                    {
                        if (lowerInvariant != 'o')
                            throw new FormatException();
                        i64Value = Convert.ToInt64(halfwidthNumbers, 8);
                    }
                    flag = true;
                    goto label_13;
                }
            }
            flag = false;
        label_13:
            return flag;
        }

        internal static bool IsHexOrOctValue(string value, ref ulong ui64Value)
        {
            int length = value.Length;
            int index = 0;
            bool flag;
            while (index < length)
            {
                char ch = value[index];
                if (ch != '&' || checked(index + 2) >= length)
                {
                    if (ch != ' ' && ch != '　')
                    {
                        flag = false;
                        goto label_13;
                    }
                    else
                        checked { ++index; }
                }
                else
                {
                    char lowerInvariant = char.ToLowerInvariant(value[checked(index + 1)]);
                    string halfwidthNumbers = Utils.ToHalfwidthNumbers(value.Substring(checked(index + 2)), Utils.GetCultureInfo());
                    if (lowerInvariant == 'h')
                    {
                        ui64Value = Convert.ToUInt64(halfwidthNumbers, 16);
                    }
                    else
                    {
                        if (lowerInvariant != 'o')
                            throw new FormatException();
                        ui64Value = Convert.ToUInt64(halfwidthNumbers, 8);
                    }
                    flag = true;
                    goto label_13;
                }
            }
            flag = false;
        label_13:
            return flag;
        }

        internal static string VBFriendlyName(object obj)
        {
            return obj != null ? Utils.VBFriendlyName(obj.GetType(), obj) : "Nothing";
        }

        internal static string VBFriendlyName(Type typ)
        {
            return Utils.VBFriendlyNameOfType(typ, false);
        }

        internal static string VBFriendlyName(Type typ, object o)
        {
            return Utils.VBFriendlyNameOfType(typ, false);
        }

        internal static string VBFriendlyNameOfType(Type typ, bool fullName = false)
        {
            string suffixAndElementType = Utils.GetArraySuffixAndElementType(ref typ);
            string str1;
            switch (!typ.IsEnum ? Type.GetTypeCode(typ) : TypeCode.Object)
            {
                case TypeCode.Boolean:
                    str1 = "Boolean";
                    break;
                case TypeCode.Char:
                    str1 = "Char";
                    break;
                case TypeCode.SByte:
                    str1 = "SByte";
                    break;
                case TypeCode.Byte:
                    str1 = "Byte";
                    break;
                case TypeCode.Int16:
                    str1 = "Short";
                    break;
                case TypeCode.UInt16:
                    str1 = "UShort";
                    break;
                case TypeCode.Int32:
                    str1 = "Integer";
                    break;
                case TypeCode.UInt32:
                    str1 = "UInteger";
                    break;
                case TypeCode.Int64:
                    str1 = "Long";
                    break;
                case TypeCode.UInt64:
                    str1 = "ULong";
                    break;
                case TypeCode.Single:
                    str1 = "Single";
                    break;
                case TypeCode.Double:
                    str1 = "Double";
                    break;
                case TypeCode.Decimal:
                    str1 = "Decimal";
                    break;
                case TypeCode.DateTime:
                    str1 = "Date";
                    break;
                case TypeCode.String:
                    str1 = "String";
                    break;
                default:
                    if (Symbols.IsGenericParameter(typ))
                    {
                        str1 = typ.Name;
                        break;
                    }
                    string str2 = (string)null;
                    string genericArgsSuffix = Utils.GetGenericArgsSuffix(typ);
                    string str3;
                    if (fullName)
                    {
                        if ((object)typ.DeclaringType != null)
                        {
                            str2 = Utils.VBFriendlyNameOfType(typ.DeclaringType, true);
                            str3 = typ.Name;
                        }
                        else
                            str3 = typ.FullName ?? typ.Name;
                    }
                    else
                        str3 = typ.Name;
                    if (genericArgsSuffix != null)
                    {
                        int length = str3.LastIndexOf('`');
                        if (length != -1)
                            str3 = str3.Substring(0, length);
                        str1 = str3 + genericArgsSuffix;
                    }
                    else
                        str1 = str3;
                    if (str2 != null)
                    {
                        str1 = str2 + "." + str1;
                        break;
                    }
                    break;
            }
            if (suffixAndElementType != null)
                str1 += suffixAndElementType;
            return str1;
        }

        private static string GetArraySuffixAndElementType(ref Type typ)
        {
            string str;
            if (!typ.IsArray)
            {
                str = (string)null;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                do
                {
                    stringBuilder.Append("(");
                    stringBuilder.Append(',', checked(typ.GetArrayRank() - 1));
                    stringBuilder.Append(")");
                    typ = typ.GetElementType();
                }
                while (typ.IsArray);
                str = stringBuilder.ToString();
            }
            return str;
        }

        private static string GetGenericArgsSuffix(Type typ)
        {
            string str;
            if (!typ.IsGenericType)
            {
                str = (string)null;
            }
            else
            {
                Type[] genericArguments = typ.GetGenericArguments();
                int length = genericArguments.Length;
                int num1 = length;
                if ((object)typ.DeclaringType != null && typ.DeclaringType.IsGenericType)
                    checked { num1 -= typ.DeclaringType.GetGenericArguments().Length; }
                if (num1 == 0)
                {
                    str = (string)null;
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("(Of ");
                    int num2 = checked(length - num1);
                    int num3 = checked(length - 1);
                    int index = num2;
                    while (index <= num3)
                    {
                        stringBuilder.Append(Utils.VBFriendlyNameOfType(genericArguments[index], false));
                        if (index != checked(length - 1))
                            stringBuilder.Append(',');
                        checked { ++index; }
                    }
                    stringBuilder.Append(")");
                    str = stringBuilder.ToString();
                }
            }
            return str;
        }

        internal static string ParameterToString(ParameterInfo parameter)
        {
            string str1 = "";
            Type typ = parameter.ParameterType;
            if (parameter.IsOptional)
                str1 += "[";
            if (typ.IsByRef)
            {
                str1 += "ByRef ";
                typ = typ.GetElementType();
            }
            else if (Symbols.IsParamArray(parameter))
                str1 += "ParamArray ";
            string str2 = str1 + parameter.Name + " As " + Utils.VBFriendlyNameOfType(typ, true);
            if (parameter.IsOptional)
            {
                object defaultValue = parameter.DefaultValue;
                if (defaultValue == null)
                {
                    str2 += " = Nothing";
                }
                else
                {
                    Type type = defaultValue.GetType();
                    if ((object)type != (object)Utils.s_voidType)
                    {
                        if (Symbols.IsEnum(type))
                            throw new InvalidOperationException();
                        str2 = str2 + " = " + Conversions.ToString(defaultValue);
                    }
                }
                str2 += "]";
            }
            return str2;
        }

        /// <summary>Returns a Visual Basic method signature.</summary>
        /// <param name="Method">A <see cref="T:System.Reflection.MethodBase" /> object to return a Visual Basic method signature for.</param>
        /// <returns>The Visual Basic method signature for the supplied <see cref="T:System.Reflection.MethodBase" /> object.</returns>
        internal static string MethodToString(MethodBase method)
        {
            Type typ1 = (Type)null;
            string str1 = "";
            if (method.MemberType == MemberTypes.Method)
                typ1 = ((MethodInfo)method).ReturnType;
            if (method.IsPublic)
                str1 += "Public ";
            else if (method.IsPrivate)
                str1 += "Private ";
            else if (method.IsAssembly)
                str1 += "Friend ";
            if ((method.Attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope)
            {
                if (!method.DeclaringType.IsInterface)
                    str1 += "Overrides ";
            }
            else if (Symbols.IsShared((MemberInfo)method))
                str1 += "Shared ";
            Symbols.UserDefinedOperator userDefinedOperator = Symbols.UserDefinedOperator.UNDEF;
            if (Symbols.IsUserDefinedOperator(method))
                userDefinedOperator = Symbols.MapToUserDefinedOperator(method);
            string str2;
            switch (userDefinedOperator)
            {
                case Symbols.UserDefinedOperator.UNDEF:
                    str2 = (object)typ1 == null || (object)typ1 == (object)Utils.s_voidType ? str1 + "Sub " : str1 + "Function ";
                    break;
                case Symbols.UserDefinedOperator.Narrow:
                    str1 += "Narrowing ";
                    goto default;
                case Symbols.UserDefinedOperator.Widen:
                    str1 += "Widening ";
                    goto default;
                default:
                    str2 = str1 + "Operator ";
                    break;
            }
            string str3 = userDefinedOperator == Symbols.UserDefinedOperator.UNDEF ? (method.MemberType != MemberTypes.Constructor ? str2 + method.Name : str2 + "New") : str2 + Symbols.OperatorNames[(int)userDefinedOperator];
            if (Symbols.IsGeneric(method))
            {
                string str4 = str3 + "(Of ";
                bool flag = true;
                Type[] typeParameters = Symbols.GetTypeParameters((MemberInfo)method);
                int index = 0;
                while (index < typeParameters.Length)
                {
                    Type typ2 = typeParameters[index];
                    if (!flag)
                        str4 += ", ";
                    else
                        flag = false;
                    str4 += Utils.VBFriendlyNameOfType(typ2, false);
                    checked { ++index; }
                }
                str3 = str4 + ")";
            }
            string str5 = str3 + "(";
            bool flag1 = true;
            ParameterInfo[] parameters = method.GetParameters();
            int index1 = 0;
            while (index1 < parameters.Length)
            {
                ParameterInfo parameter = parameters[index1];
                if (!flag1)
                    str5 += ", ";
                else
                    flag1 = false;
                str5 += Utils.ParameterToString(parameter);
                checked { ++index1; }
            }
            string str6 = str5 + ")";
            if ((object)typ1 != null && (object)typ1 != (object)Utils.s_voidType)
                str6 = str6 + " As " + Utils.VBFriendlyNameOfType(typ1, true);
            return str6;
        }

        internal static string PropertyToString(PropertyInfo prop)
        {
            string str1 = "";
            MethodInfo methodInfo = prop.GetGetMethod();
            Utils.PropertyKind propertyKind;
            ParameterInfo[] parameterInfoArray1;
            Type typ;
            if ((object)methodInfo != null)
            {
                propertyKind = (object)prop.GetSetMethod() == null ? Utils.PropertyKind.ReadOnly : Utils.PropertyKind.ReadWrite;
                parameterInfoArray1 = methodInfo.GetParameters();
                typ = methodInfo.ReturnType;
            }
            else
            {
                propertyKind = Utils.PropertyKind.WriteOnly;
                methodInfo = prop.GetSetMethod();
                ParameterInfo[] parameters = methodInfo.GetParameters();
                parameterInfoArray1 = new ParameterInfo[checked(parameters.Length - 2 + 1)];
                Array.Copy((Array)parameters, (Array)parameterInfoArray1, parameterInfoArray1.Length);
                typ = parameters[checked(parameters.Length - 1)].ParameterType;
            }
            string str2 = str1 + "Public ";
            if ((methodInfo.Attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope)
            {
                if (!prop.DeclaringType.IsInterface)
                    str2 += "Overrides ";
            }
            else if (Symbols.IsShared((MemberInfo)methodInfo))
                str2 += "Shared ";
            if (propertyKind == Utils.PropertyKind.ReadOnly)
                str2 += "ReadOnly ";
            if (propertyKind == Utils.PropertyKind.WriteOnly)
                str2 += "WriteOnly ";
            string str3 = str2 + "Property " + prop.Name + "(";
            bool flag = true;
            ParameterInfo[] parameterInfoArray2 = parameterInfoArray1;
            int index = 0;
            while (index < parameterInfoArray2.Length)
            {
                ParameterInfo parameter = parameterInfoArray2[index];
                if (!flag)
                    str3 += ", ";
                else
                    flag = false;
                str3 += Utils.ParameterToString(parameter);
                checked { ++index; }
            }
            return str3 + ") As " + Utils.VBFriendlyNameOfType(typ, true);
        }

        internal static string MemberToString(MemberInfo member)
        {
            string name;
            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                case MemberTypes.Method:
                    name = Utils.MethodToString((MethodBase)member);
                    break;
                case MemberTypes.Field:
                    name = Utils.FieldToString((FieldInfo)member);
                    break;
                case MemberTypes.Property:
                    name = Utils.PropertyToString((PropertyInfo)member);
                    break;
                default:
                    name = member.Name;
                    break;
            }
            return name;
        }

        internal static string FieldToString(FieldInfo field)
        {
            string str = "";
            Type fieldType = field.FieldType;
            if (field.IsPublic)
                str += "Public ";
            else if (field.IsPrivate)
                str += "Private ";
            else if (field.IsAssembly)
                str += "Friend ";
            else if (field.IsFamily)
                str += "Protected ";
            else if (field.IsFamilyOrAssembly)
                str += "Protected Friend ";
            return str + field.Name + " As " + Utils.VBFriendlyNameOfType(fieldType, true);
        }

        private Utils()
        {
        }

        /// <summary>Used by the Visual Basic compiler as a helper for <see langword="Redim" />.</summary>
        /// <param name="arySrc">The array to be copied.</param>
        /// <param name="aryDest">The destination array.</param>
        /// <returns>The copied array.</returns>
        public static Array CopyArray(Array arySrc, Array aryDest)
        {
            Array array;
            if (arySrc == null)
            {
                array = aryDest;
            }
            else
            {
                int length1 = arySrc.Length;
                if (length1 == 0)
                {
                    array = aryDest;
                }
                else
                {
                    if (aryDest.Rank != arySrc.Rank)
                        throw new InvalidCastException();
                    int num1 = checked(aryDest.Rank - 2);
                    int dimension = 0;
                    while (dimension <= num1)
                    {
                        if (aryDest.GetUpperBound(dimension) != arySrc.GetUpperBound(dimension))
                            throw new ArrayTypeMismatchException();
                        checked { ++dimension; }
                    }
                    if (length1 > aryDest.Length)
                        length1 = aryDest.Length;
                    if (arySrc.Rank > 1)
                    {
                        int rank = arySrc.Rank;
                        int length2 = arySrc.GetLength(checked(rank - 1));
                        int length3 = aryDest.GetLength(checked(rank - 1));
                        if (length3 == 0)
                        {
                            array = aryDest;
                            goto label_21;
                        }
                        else
                        {
                            int length4 = length2 > length3 ? length3 : length2;
                            int num2 = checked(unchecked(arySrc.Length / length2) - 1);
                            int num3 = 0;
                            while (num3 <= num2)
                            {
                                Array.Copy(arySrc, checked(num3 * length2), aryDest, checked(num3 * length3), length4);
                                checked { ++num3; }
                            }
                        }
                    }
                    else
                        Array.Copy(arySrc, aryDest, length1);
                    array = aryDest;
                }
            }
        label_21:
            return array;
        }

        internal static Encoding GetFileIOEncoding()
        {
            return Encoding.Default;
        }

        internal static int GetLocaleCodePage()
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage;
        }

        private enum PropertyKind
        {
            ReadWrite,
            ReadOnly,
            WriteOnly,
        }

        internal static DateTimeFormatInfo GetDateTimeFormatInfo()
        {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat;
        }

    }
}
