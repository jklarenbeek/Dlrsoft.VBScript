// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.Symbols
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
    internal class Symbols
    {
        internal static readonly object[] NoArguments = new object[0];
        internal static readonly string[] NoArgumentNames = new string[0];
        internal static readonly Type[] NoTypeArguments = new Type[0];
        internal static readonly Type[] NoTypeParameters = new Type[0];
        internal static readonly string[] OperatorCLSNames = new string[28];
        internal static readonly string[] OperatorNames;

        static Symbols()
        {
            Symbols.OperatorCLSNames[1] = "op_Explicit";
            Symbols.OperatorCLSNames[2] = "op_Implicit";
            Symbols.OperatorCLSNames[3] = "op_True";
            Symbols.OperatorCLSNames[4] = "op_False";
            Symbols.OperatorCLSNames[5] = "op_UnaryNegation";
            Symbols.OperatorCLSNames[6] = "op_OnesComplement";
            Symbols.OperatorCLSNames[7] = "op_UnaryPlus";
            Symbols.OperatorCLSNames[8] = "op_Addition";
            Symbols.OperatorCLSNames[9] = "op_Subtraction";
            Symbols.OperatorCLSNames[10] = "op_Multiply";
            Symbols.OperatorCLSNames[11] = "op_Division";
            Symbols.OperatorCLSNames[12] = "op_Exponent";
            Symbols.OperatorCLSNames[13] = "op_IntegerDivision";
            Symbols.OperatorCLSNames[14] = "op_Concatenate";
            Symbols.OperatorCLSNames[15] = "op_LeftShift";
            Symbols.OperatorCLSNames[16] = "op_RightShift";
            Symbols.OperatorCLSNames[17] = "op_Modulus";
            Symbols.OperatorCLSNames[18] = "op_BitwiseOr";
            Symbols.OperatorCLSNames[19] = "op_ExclusiveOr";
            Symbols.OperatorCLSNames[20] = "op_BitwiseAnd";
            Symbols.OperatorCLSNames[21] = "op_Like";
            Symbols.OperatorCLSNames[22] = "op_Equality";
            Symbols.OperatorCLSNames[23] = "op_Inequality";
            Symbols.OperatorCLSNames[24] = "op_LessThan";
            Symbols.OperatorCLSNames[25] = "op_LessThanOrEqual";
            Symbols.OperatorCLSNames[26] = "op_GreaterThanOrEqual";
            Symbols.OperatorCLSNames[27] = "op_GreaterThan";
            Symbols.OperatorNames = new string[28];
            Symbols.OperatorNames[1] = "CType";
            Symbols.OperatorNames[2] = "CType";
            Symbols.OperatorNames[3] = "IsTrue";
            Symbols.OperatorNames[4] = "IsFalse";
            Symbols.OperatorNames[5] = "-";
            Symbols.OperatorNames[6] = "Not";
            Symbols.OperatorNames[7] = "+";
            Symbols.OperatorNames[8] = "+";
            Symbols.OperatorNames[9] = "-";
            Symbols.OperatorNames[10] = "*";
            Symbols.OperatorNames[11] = "/";
            Symbols.OperatorNames[12] = "^";
            Symbols.OperatorNames[13] = "\\";
            Symbols.OperatorNames[14] = "&";
            Symbols.OperatorNames[15] = "<<";
            Symbols.OperatorNames[16] = ">>";
            Symbols.OperatorNames[17] = "Mod";
            Symbols.OperatorNames[18] = "Or";
            Symbols.OperatorNames[19] = "Xor";
            Symbols.OperatorNames[20] = "And";
            Symbols.OperatorNames[21] = "Like";
            Symbols.OperatorNames[22] = "=";
            Symbols.OperatorNames[23] = "<>";
            Symbols.OperatorNames[24] = "<";
            Symbols.OperatorNames[25] = "<=";
            Symbols.OperatorNames[26] = ">=";
            Symbols.OperatorNames[27] = ">";
        }

        internal static bool IsUnaryOperator(Symbols.UserDefinedOperator op)
        {
            bool flag;
            switch (op)
            {
                case Symbols.UserDefinedOperator.Narrow:
                case Symbols.UserDefinedOperator.Widen:
                case Symbols.UserDefinedOperator.IsTrue:
                case Symbols.UserDefinedOperator.IsFalse:
                case Symbols.UserDefinedOperator.Negate:
                case Symbols.UserDefinedOperator.Not:
                case Symbols.UserDefinedOperator.UnaryPlus:
                    flag = true;
                    break;
                default:
                    flag = false;
                    break;
            }
            return flag;
        }

        internal static bool IsBinaryOperator(Symbols.UserDefinedOperator op)
        {
            return (uint)(op - (sbyte)8) <= 19U;
        }

        internal static bool IsUserDefinedOperator(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("op_", StringComparison.Ordinal);
        }

        internal static bool IsNarrowingConversionOperator(MethodBase method)
        {
            return method.IsSpecialName && method.Name.Equals(Symbols.OperatorCLSNames[1]);
        }

        internal static Symbols.UserDefinedOperator MapToUserDefinedOperator(MethodBase method)
        {
            int index = 1;
            Symbols.UserDefinedOperator userDefinedOperator;
            do
            {
                if (method.Name.Equals(Symbols.OperatorCLSNames[index]))
                {
                    int length = method.GetParameters().Length;
                    Symbols.UserDefinedOperator op = (Symbols.UserDefinedOperator)checked((sbyte)index);
                    if (length == 1 && Symbols.IsUnaryOperator(op) || length == 2 && Symbols.IsBinaryOperator(op))
                    {
                        userDefinedOperator = op;
                        goto label_6;
                    }
                }
                checked { ++index; }
            }
            while (index <= 27);
            userDefinedOperator = Symbols.UserDefinedOperator.UNDEF;
        label_6:
            return userDefinedOperator;
        }

        internal static TypeCode GetTypeCode(Type type)
        {
            return type.GetTypeCode();
        }

        internal static Type MapTypeCodeToType(TypeCode typeCode)
        {
            Type type;
            switch (typeCode)
            {
                case TypeCode.Object:
                    type = typeof(object);
                    break;
                case TypeCode.DBNull:
                    type = typeof(DBNull);
                    break;
                case TypeCode.Boolean:
                    type = typeof(bool);
                    break;
                case TypeCode.Char:
                    type = typeof(char);
                    break;
                case TypeCode.SByte:
                    type = typeof(sbyte);
                    break;
                case TypeCode.Byte:
                    type = typeof(byte);
                    break;
                case TypeCode.Int16:
                    type = typeof(short);
                    break;
                case TypeCode.UInt16:
                    type = typeof(ushort);
                    break;
                case TypeCode.Int32:
                    type = typeof(int);
                    break;
                case TypeCode.UInt32:
                    type = typeof(uint);
                    break;
                case TypeCode.Int64:
                    type = typeof(long);
                    break;
                case TypeCode.UInt64:
                    type = typeof(ulong);
                    break;
                case TypeCode.Single:
                    type = typeof(float);
                    break;
                case TypeCode.Double:
                    type = typeof(double);
                    break;
                case TypeCode.Decimal:
                    type = typeof(Decimal);
                    break;
                case TypeCode.DateTime:
                    type = typeof(DateTime);
                    break;
                case TypeCode.String:
                    type = typeof(string);
                    break;
                default:
                    type = (Type)null;
                    break;
            }
            return type;
        }

        internal static bool IsRootObjectType(Type type)
        {
            return (object)type == (object)typeof(object);
        }

        internal static bool IsRootEnumType(Type type)
        {
            return (object)type == (object)typeof(Enum);
        }

        internal static bool IsValueType(Type type)
        {
            return type.IsValueType;
        }

        internal static bool IsEnum(Type type)
        {
            return type.IsEnum;
        }

        internal static bool IsArrayType(Type type)
        {
            return type.IsArray;
        }

        internal static bool IsStringType(Type type)
        {
            return (object)type == (object)typeof(string);
        }

        internal static bool IsCharArrayRankOne(Type type)
        {
            return (object)type == (object)typeof(char[]);
        }

        internal static bool IsIntegralType(TypeCode typeCode)
        {
            bool flag;
            switch (typeCode)
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    flag = true;
                    break;
                default:
                    flag = false;
                    break;
            }
            return flag;
        }

        internal static bool IsNumericType(TypeCode typeCode)
        {
            bool flag;
            switch (typeCode)
            {
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
            return flag;
        }

        internal static bool IsNumericType(Type type)
        {
            return Symbols.IsNumericType(Symbols.GetTypeCode(type));
        }

        internal static bool IsIntrinsicType(TypeCode typeCode)
        {
            bool flag;
            switch (typeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
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
                case TypeCode.DateTime:
                case TypeCode.String:
                    flag = true;
                    break;
                default:
                    flag = false;
                    break;
            }
            return flag;
        }

        internal static bool IsIntrinsicType(Type type)
        {
            return Symbols.IsIntrinsicType(Symbols.GetTypeCode(type)) && !Symbols.IsEnum(type);
        }

        internal static bool IsClass(Type type)
        {
            return type.IsClass || Symbols.IsRootEnumType(type);
        }

        internal static bool IsClassOrValueType(Type type)
        {
            return Symbols.IsValueType(type) || Symbols.IsClass(type);
        }

        internal static bool IsInterface(Type type)
        {
            return type.IsInterface;
        }

        internal static bool IsClassOrInterface(Type type)
        {
            return Symbols.IsClass(type) || Symbols.IsInterface(type);
        }

        internal static bool IsReferenceType(Type type)
        {
            return Symbols.IsClass(type) || Symbols.IsInterface(type);
        }

        internal static bool IsGenericParameter(Type type)
        {
            return type.IsGenericParameter;
        }

        internal static bool IsCollectionInterface(Type type)
        {
            return type.IsInterface && (type.IsGenericType && ((object)type.GetGenericTypeDefinition() == (object)typeof(IList<>) || (object)type.GetGenericTypeDefinition() == (object)typeof(ICollection<>) || ((object)type.GetGenericTypeDefinition() == (object)typeof(IEnumerable<>) || (object)type.GetGenericTypeDefinition() == (object)typeof(IReadOnlyList<>)) || ((object)type.GetGenericTypeDefinition() == (object)typeof(IReadOnlyCollection<>) || (object)type.GetGenericTypeDefinition() == (object)typeof(IDictionary<,>) || (object)type.GetGenericTypeDefinition() == (object)typeof(IReadOnlyDictionary<,>))) || ((object)type == (object)typeof(IList) || (object)type == (object)typeof(ICollection) || ((object)type == (object)typeof(IEnumerable) || (object)type == (object)typeof(INotifyPropertyChanged)) || (object)type == (object)typeof(INotifyCollectionChanged)));
        }

        internal static bool Implements(Type implementor, Type @interface)
        {
            Type[] interfaces = implementor.GetInterfaces();
            int index = 0;
            bool flag;
            while (index < interfaces.Length)
            {
                if ((object)interfaces[index] == (object)@interface)
                {
                    flag = true;
                    goto label_6;
                }
                else
                    checked { ++index; }
            }
            flag = false;
        label_6:
            return flag;
        }

        internal static bool IsOrInheritsFrom(Type derived, Type @base)
        {
            bool flag;
            if ((object)derived == (object)@base)
            {
                flag = true;
            }
            else
            {
                if (derived.IsGenericParameter)
                {
                    if (Symbols.IsClass(@base) && (uint)(derived.GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) > 0U && Symbols.IsOrInheritsFrom(typeof(ValueType), @base))
                    {
                        flag = true;
                        goto label_20;
                    }
                    else
                    {
                        Type[] parameterConstraints = derived.GetGenericParameterConstraints();
                        int index = 0;
                        while (index < parameterConstraints.Length)
                        {
                            if (Symbols.IsOrInheritsFrom(parameterConstraints[index], @base))
                            {
                                flag = true;
                                goto label_20;
                            }
                            else
                                checked { ++index; }
                        }
                    }
                }
                else if (Symbols.IsInterface(derived))
                {
                    if (Symbols.IsInterface(@base))
                    {
                        Type[] interfaces = derived.GetInterfaces();
                        int index = 0;
                        while (index < interfaces.Length)
                        {
                            if ((object)interfaces[index] == (object)@base)
                            {
                                flag = true;
                                goto label_20;
                            }
                            else
                                checked { ++index; }
                        }
                    }
                }
                else if (Symbols.IsClass(@base) && Symbols.IsClassOrValueType(derived))
                {
                    flag = derived.IsSubclassOf(@base);
                    goto label_20;
                }
                flag = false;
            }
        label_20:
            return flag;
        }

        internal static bool IsGeneric(Type type)
        {
            return type.IsGenericType;
        }

        internal static bool IsInstantiatedGeneric(Type type)
        {
            return type.IsGenericType && !type.IsGenericTypeDefinition;
        }

        internal static bool IsGeneric(MethodBase method)
        {
            return method.IsGenericMethod;
        }

        internal static bool IsGeneric(MemberInfo member)
        {
            MethodBase method = member as MethodBase;
            return (object)method != null && Symbols.IsGeneric(method);
        }

        internal static bool IsRawGeneric(MethodBase method)
        {
            return method.IsGenericMethod && method.IsGenericMethodDefinition;
        }

        internal static Type[] GetTypeParameters(MemberInfo member)
        {
            MethodBase methodBase = member as MethodBase;
            return (object)methodBase != null ? methodBase.GetGenericArguments() : Symbols.NoTypeParameters;
        }

        internal static Type[] GetTypeArguments(Type type)
        {
            return type.GetGenericArguments();
        }

        internal static Type[] GetInterfaceConstraints(Type genericParameter)
        {
            return ((IEnumerable<Type>)genericParameter.GetInterfaces()).ToArray<Type>();
        }

        internal static Type GetClassConstraint(Type genericParameter)
        {
            Type baseType = genericParameter.BaseType;
            return !Symbols.IsRootObjectType(baseType) ? baseType : (Type)null;
        }

        internal static int IndexIn(Type possibleGenericParameter, MethodBase genericMethodDef)
        {
            return !Symbols.IsGenericParameter(possibleGenericParameter) || (object)possibleGenericParameter.DeclaringMethod == null || !Symbols.AreGenericMethodDefsEqual(possibleGenericParameter.DeclaringMethod, genericMethodDef) ? -1 : possibleGenericParameter.GenericParameterPosition;
        }

        internal static bool RefersToGenericParameter(Type referringType, MethodBase method)
        {
            bool flag;
            if (!Symbols.IsRawGeneric(method))
            {
                flag = false;
            }
            else
            {
                if (referringType.IsByRef)
                    referringType = Symbols.GetElementType(referringType);
                if (Symbols.IsGenericParameter(referringType))
                {
                    if (Symbols.AreGenericMethodDefsEqual(referringType.DeclaringMethod, method))
                    {
                        flag = true;
                        goto label_16;
                    }
                }
                else if (Symbols.IsGeneric(referringType))
                {
                    Type[] typeArguments = Symbols.GetTypeArguments(referringType);
                    int index = 0;
                    while (index < typeArguments.Length)
                    {
                        if (Symbols.RefersToGenericParameter(typeArguments[index], method))
                        {
                            flag = true;
                            goto label_16;
                        }
                        else
                            checked { ++index; }
                    }
                }
                else if (Symbols.IsArrayType(referringType))
                {
                    flag = Symbols.RefersToGenericParameter(referringType.GetElementType(), method);
                    goto label_16;
                }
                flag = false;
            }
        label_16:
            return flag;
        }

        internal static bool RefersToGenericParameterCLRSemantics(Type referringType, Type typ)
        {
            if (referringType.IsByRef)
                referringType = Symbols.GetElementType(referringType);
            bool flag;
            if (Symbols.IsGenericParameter(referringType))
            {
                if ((object)referringType.DeclaringType == (object)typ)
                {
                    flag = true;
                    goto label_14;
                }
            }
            else if (Symbols.IsGeneric(referringType))
            {
                Type[] typeArguments = Symbols.GetTypeArguments(referringType);
                int index = 0;
                while (index < typeArguments.Length)
                {
                    if (Symbols.RefersToGenericParameterCLRSemantics(typeArguments[index], typ))
                    {
                        flag = true;
                        goto label_14;
                    }
                    else
                        checked { ++index; }
                }
            }
            else if (Symbols.IsArrayType(referringType))
            {
                flag = Symbols.RefersToGenericParameterCLRSemantics(referringType.GetElementType(), typ);
                goto label_14;
            }
            flag = false;
        label_14:
            return flag;
        }

        internal static bool AreGenericMethodDefsEqual(MethodBase method1, MethodBase method2)
        {
            return method1 == method2 || method1.MetadataToken == method2.MetadataToken;
        }

        internal static bool IsShadows(MethodBase method)
        {
            return !method.IsHideBySig && (!method.IsVirtual || (method.Attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.PrivateScope || (((MethodInfo)method).GetRuntimeBaseDefinition().Attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.PrivateScope);
        }

        internal static bool IsShared(MemberInfo member)
        {
            bool flag;
            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    flag = ((MethodBase)member).IsStatic;
                    break;
                case MemberTypes.Field:
                    flag = ((FieldInfo)member).IsStatic;
                    break;
                case MemberTypes.Method:
                    flag = ((MethodBase)member).IsStatic;
                    break;
                case MemberTypes.Property:
                    flag = ((PropertyInfo)member).GetGetMethod().IsStatic;
                    break;
                default:
                    flag = false;
                    break;
            }
            return flag;
        }

        internal static bool IsParamArray(ParameterInfo parameter)
        {
            return Symbols.IsArrayType(parameter.ParameterType) && parameter.IsDefined(typeof(ParamArrayAttribute), false);
        }

        internal static Type GetElementType(Type type)
        {
            return type.GetElementType();
        }

        internal static bool AreParametersAndReturnTypesValid(
          ParameterInfo[] parameters,
          Type returnType)
        {
            bool flag;
            if ((object)returnType != null && (returnType.IsPointer || returnType.IsByRef))
            {
                flag = false;
            }
            else
            {
                if (parameters != null)
                {
                    ParameterInfo[] parameterInfoArray = parameters;
                    int index = 0;
                    while (index < parameterInfoArray.Length)
                    {
                        if (parameterInfoArray[index].ParameterType.IsPointer)
                        {
                            flag = false;
                            goto label_9;
                        }
                        else
                            checked { ++index; }
                    }
                }
                flag = true;
            }
        label_9:
            return flag;
        }

        internal static void GetAllParameterCounts(
          ParameterInfo[] parameters,
          ref int requiredParameterCount,
          ref int maximumParameterCount,
          ref int paramArrayIndex)
        {
            maximumParameterCount = parameters.Length;
            int index = checked(maximumParameterCount - 1);
            while (index >= 0)
            {
                if (!parameters[index].IsOptional)
                {
                    requiredParameterCount = checked(index + 1);
                    break;
                }
                checked { index += -1; }
            }
            if (maximumParameterCount == 0 || !Symbols.IsParamArray(parameters[checked(maximumParameterCount - 1)]))
                return;
            paramArrayIndex = checked(maximumParameterCount - 1);
            checked { --requiredParameterCount; }
        }

        internal static bool IsNonPublicRuntimeMember(MemberInfo member)
        {
            Type declaringType = member.DeclaringType;
            return !declaringType.IsPublic && (object)declaringType.Assembly == (object)Utils.VBRuntimeAssembly;
        }

        internal static bool HasFlag(BindingFlags flags, BindingFlags flagToTest)
        {
            return (uint)(flags & flagToTest) > 0U;
        }

        internal enum UserDefinedOperator : sbyte
        {
            UNDEF,
            Narrow,
            Widen,
            IsTrue,
            IsFalse,
            Negate,
            Not,
            UnaryPlus,
            Plus,
            Minus,
            Multiply,
            Divide,
            Power,
            IntegralDivide,
            Concatenate,
            ShiftLeft,
            ShiftRight,
            Modulus,
            Or,
            Xor,
            And,
            Like,
            Equal,
            NotEqual,
            Less,
            LessEqual,
            GreaterEqual,
            Greater,
            MAX,
        }

        internal sealed class Container
        {
            private static readonly MemberInfo[] s_noMembers = new MemberInfo[0];
            private readonly object _instance;
            private readonly Type _type;

            internal Container(object instance)
            {
                if (instance == null)
                    throw ExceptionUtils.VbMakeObjNotSetException();
                this._instance = instance;
                this._type = instance.GetType();
            }

            internal Container(Type type)
            {
                if ((object)type == null)
                    throw ExceptionUtils.VbMakeObjNotSetException();
                this._instance = (object)null;
                this._type = type;
            }

            internal bool IsWindowsRuntimeObject
            {
                get
                {
                    bool flag;
                    for (Type type = this._type; (object)type != null; type = type.BaseType)
                    {
                        if ((type.Attributes & TypeAttributes.WindowsRuntime) == TypeAttributes.WindowsRuntime)
                        {
                            flag = true;
                            goto label_8;
                        }
                        else if ((type.Attributes & TypeAttributes.Import) == TypeAttributes.Import)
                        {
                            flag = false;
                            goto label_8;
                        }
                    }
                    flag = false;
                label_8:
                    return flag;
                }
            }

            internal string VBFriendlyName
            {
                get
                {
                    return Utils.VBFriendlyName(this._type, this._instance);
                }
            }

            internal bool IsArray
            {
                get
                {
                    return Symbols.IsArrayType(this._type) && this._instance != null;
                }
            }

            internal bool IsValueType
            {
                get
                {
                    return Symbols.IsValueType(this._type) && this._instance != null;
                }
            }

            private static MemberInfo[] FilterInvalidMembers(MemberInfo[] members)
            {
                MemberInfo[] memberInfoArray1;
                if (members == null || members.Length == 0)
                {
                    memberInfoArray1 = (MemberInfo[])null;
                }
                else
                {
                    int num1 = 0;
                    int num2 = checked(members.Length - 1);
                    int index1 = 0;
                    while (index1 <= num2)
                    {
                        ParameterInfo[] parameters1 = (ParameterInfo[])null;
                        Type returnType = (Type)null;
                        switch (members[index1].MemberType)
                        {
                            case MemberTypes.Constructor:
                            case MemberTypes.Method:
                                MethodInfo member1 = (MethodInfo)members[index1];
                                parameters1 = member1.GetParameters();
                                returnType = member1.ReturnType;
                                break;
                            case MemberTypes.Field:
                                returnType = ((FieldInfo)members[index1]).FieldType;
                                break;
                            case MemberTypes.Property:
                                PropertyInfo member2 = (PropertyInfo)members[index1];
                                MethodInfo getMethod = member2.GetGetMethod();
                                if ((object)getMethod != null)
                                {
                                    parameters1 = getMethod.GetParameters();
                                }
                                else
                                {
                                    ParameterInfo[] parameters2 = member2.GetSetMethod().GetParameters();
                                    parameters1 = new ParameterInfo[checked(parameters2.Length - 2 + 1)];
                                    Array.Copy((Array)parameters2, (Array)parameters1, parameters1.Length);
                                }
                                returnType = member2.PropertyType;
                                break;
                        }
                        if (Symbols.AreParametersAndReturnTypesValid(parameters1, returnType))
                            checked { ++num1; }
                        else
                            members[index1] = (MemberInfo)null;
                        checked { ++index1; }
                    }
                    if (num1 == members.Length)
                        memberInfoArray1 = members;
                    else if (num1 > 0)
                    {
                        MemberInfo[] memberInfoArray2 = new MemberInfo[checked(num1 - 1 + 1)];
                        int index2 = 0;
                        int num3 = checked(members.Length - 1);
                        int index3 = 0;
                        while (index3 <= num3)
                        {
                            if ((object)members[index3] != null)
                            {
                                memberInfoArray2[index2] = members[index3];
                                checked { ++index2; }
                            }
                            checked { ++index3; }
                        }
                        memberInfoArray1 = memberInfoArray2;
                    }
                    else
                        memberInfoArray1 = (MemberInfo[])null;
                }
                return memberInfoArray1;
            }

            internal List<MemberInfo> LookupWinRTCollectionInterfaceMembers(
              string memberName)
            {
                List<MemberInfo> memberInfoList = new List<MemberInfo>();
                Type[] interfaces = this._type.GetInterfaces();
                int index = 0;
                while (index < interfaces.Length)
                {
                    Type type = interfaces[index];
                    if (Symbols.IsCollectionInterface(type))
                    {
                        MemberInfo[] member = type.GetMember(memberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                        if (member != null)
                            memberInfoList.AddRange((IEnumerable<MemberInfo>)member);
                    }
                    checked { ++index; }
                }
                return memberInfoList;
            }

            internal MemberInfo[] LookupNamedMembers(string memberName)
            {
                MemberInfo[] members;
                if (Symbols.IsGenericParameter(this._type))
                {
                    Type classConstraint = Symbols.GetClassConstraint(this._type);
                    members = (object)classConstraint == null ? (MemberInfo[])null : ((IEnumerable<MemberInfo>)classConstraint.GetMember(memberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)).ToArray<MemberInfo>();
                }
                else
                    members = ((IEnumerable<MemberInfo>)this._type.GetMember(memberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)).ToArray<MemberInfo>();
                if (this.IsWindowsRuntimeObject)
                {
                    List<MemberInfo> memberInfoList = this.LookupWinRTCollectionInterfaceMembers(memberName);
                    if (members != null)
                        memberInfoList.AddRange((IEnumerable<MemberInfo>)members);
                    members = memberInfoList.ToArray();
                }
                MemberInfo[] array = Symbols.Container.FilterInvalidMembers(members);
                if (array == null)
                    array = Symbols.Container.s_noMembers;
                else if (array.Length > 1)
                    Array.Sort<MemberInfo>(array, (IComparer<MemberInfo>)Symbols.Container.InheritanceSorter.Instance);
                return array;
            }

            private List<MemberInfo> LookupWinRTCollectionDefaultMembers(
              ref string defaultMemberName)
            {
                List<MemberInfo> memberInfoList = new List<MemberInfo>();
                Type[] interfaces = this._type.GetInterfaces();
                int index = 0;
                while (index < interfaces.Length)
                {
                    Type type = interfaces[index];
                    if (Symbols.IsCollectionInterface(type))
                    {
                        MemberInfo[] memberInfoArray = this.LookupDefaultMembers(ref defaultMemberName, type);
                        if (memberInfoArray != null)
                            memberInfoList.AddRange((IEnumerable<MemberInfo>)memberInfoArray);
                    }
                    checked { ++index; }
                }
                return memberInfoList;
            }

            private MemberInfo[] LookupDefaultMembers(
              ref string defaultMemberName,
              Type searchType)
            {
                string name = (string)null;
                Type type = searchType;
                do
                {
                    object[] array = ((IEnumerable<object>)type.GetCustomAttributes(typeof(DefaultMemberAttribute), false)).ToArray<object>();
                    if (array != null && array.Length > 0)
                    {
                        name = ((DefaultMemberAttribute)array[0]).MemberName;
                        break;
                    }
                    type = type.BaseType;
                }
                while ((object)type != null && !Symbols.IsRootObjectType(type));
                MemberInfo[] memberInfoArray;
                if (name != null)
                {
                    MemberInfo[] array = Symbols.Container.FilterInvalidMembers(((IEnumerable<MemberInfo>)type.GetMember(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)).ToArray<MemberInfo>());
                    if (array != null)
                    {
                        defaultMemberName = name;
                        if (array.Length > 1)
                            Array.Sort<MemberInfo>(array, (IComparer<MemberInfo>)Symbols.Container.InheritanceSorter.Instance);
                        memberInfoArray = array;
                        goto label_10;
                    }
                }
                memberInfoArray = Symbols.Container.s_noMembers;
            label_10:
                return memberInfoArray;
            }

            internal MemberInfo[] GetMembers(ref string memberName, bool reportErrors)
            {
                if (memberName == null)
                    memberName = "";
                MemberInfo[] memberInfoArray1;
                MemberInfo[] memberInfoArray2;
                if (Operators.CompareString(memberName, "", false) == 0)
                {
                    memberInfoArray1 = this.LookupDefaultMembers(ref memberName, this._type);
                    if (this.IsWindowsRuntimeObject)
                    {
                        List<MemberInfo> memberInfoList = this.LookupWinRTCollectionDefaultMembers(ref memberName);
                        if (memberInfoArray1 != null)
                            memberInfoList.AddRange((IEnumerable<MemberInfo>)memberInfoArray1);
                        memberInfoArray1 = memberInfoList.ToArray();
                    }
                    if (memberInfoArray1.Length == 0)
                    {
                        if (reportErrors)
                            throw new MissingMemberException(Utils.GetResourceString(SR.MissingMember_NoDefaultMemberFound1, this.VBFriendlyName));
                        memberInfoArray2 = memberInfoArray1;
                        goto label_16;
                    }
                }
                else
                {
                    memberInfoArray1 = this.LookupNamedMembers(memberName);
                    if (memberInfoArray1.Length == 0)
                    {
                        if (reportErrors)
                            throw new MissingMemberException(Utils.GetResourceString(SR.MissingMember_MemberNotFoundOnType2, memberName, this.VBFriendlyName));
                        memberInfoArray2 = memberInfoArray1;
                        goto label_16;
                    }
                }
                memberInfoArray2 = memberInfoArray1;
            label_16:
                return memberInfoArray2;
            }

            internal object GetFieldValue(FieldInfo field)
            {
                if (this._instance == null && !Symbols.IsShared((MemberInfo)field))
                    throw new NullReferenceException(Utils.GetResourceString(SR.NullReference_InstanceReqToAccessMember1, Utils.FieldToString(field)));
                if (Symbols.IsNonPublicRuntimeMember((MemberInfo)field))
                    throw new MissingMemberException();
                return field.GetValue(this._instance);
            }

            internal void SetFieldValue(FieldInfo field, object value)
            {
                if (field.IsInitOnly)
                    throw new MissingMemberException(Utils.GetResourceString(SR.MissingMember_ReadOnlyField2, field.Name, this.VBFriendlyName));
                if (this._instance == null && !Symbols.IsShared((MemberInfo)field))
                    throw new NullReferenceException(Utils.GetResourceString(SR.NullReference_InstanceReqToAccessMember1, Utils.FieldToString(field)));
                if (Symbols.IsNonPublicRuntimeMember((MemberInfo)field))
                    throw new MissingMemberException();
                field.SetValue(this._instance, Conversions.ChangeType(value, field.FieldType));
            }

            internal object GetArrayValue(object[] indices)
            {
                Array instance = (Array)this._instance;
                int rank = instance.Rank;
                if (indices.Length != rank)
                    throw new RankException();
                int num1 = (int)Conversions.ChangeType(indices[0], typeof(int));
                object obj;
                if (rank == 1)
                {
                    obj = instance.GetValue(num1);
                }
                else
                {
                    int index2 = (int)Conversions.ChangeType(indices[1], typeof(int));
                    if (rank == 2)
                    {
                        obj = instance.GetValue(num1, index2);
                    }
                    else
                    {
                        int index3 = (int)Conversions.ChangeType(indices[2], typeof(int));
                        if (rank == 3)
                        {
                            obj = instance.GetValue(num1, index2, index3);
                        }
                        else
                        {
                            int[] numArray = new int[checked(rank - 1 + 1)];
                            numArray[0] = num1;
                            numArray[1] = index2;
                            numArray[2] = index3;
                            int num2 = checked(rank - 1);
                            int index = 3;
                            while (index <= num2)
                            {
                                numArray[index] = (int)Conversions.ChangeType(indices[index], typeof(int));
                                checked { ++index; }
                            }
                            obj = instance.GetValue(numArray);
                        }
                    }
                }
                return obj;
            }

            internal void SetArrayValue(object[] arguments)
            {
                Array instance = (Array)this._instance;
                int rank = instance.Rank;
                if (checked(arguments.Length - 1) != rank)
                    throw new RankException();
                object Expression = arguments[checked(arguments.Length - 1)];
                Type elementType = this._type.GetElementType();
                int num1 = (int)Conversions.ChangeType(arguments[0], typeof(int));
                if (rank == 1)
                {
                    instance.SetValue(Conversions.ChangeType(Expression, elementType), num1);
                }
                else
                {
                    int index2 = (int)Conversions.ChangeType(arguments[1], typeof(int));
                    if (rank == 2)
                    {
                        instance.SetValue(Conversions.ChangeType(Expression, elementType), num1, index2);
                    }
                    else
                    {
                        int index3 = (int)Conversions.ChangeType(arguments[2], typeof(int));
                        if (rank == 3)
                        {
                            instance.SetValue(Conversions.ChangeType(Expression, elementType), num1, index2, index3);
                        }
                        else
                        {
                            int[] numArray = new int[checked(rank - 1 + 1)];
                            numArray[0] = num1;
                            numArray[1] = index2;
                            numArray[2] = index3;
                            int num2 = checked(rank - 1);
                            int index = 3;
                            while (index <= num2)
                            {
                                numArray[index] = (int)Conversions.ChangeType(arguments[index], typeof(int));
                                checked { ++index; }
                            }
                            instance.SetValue(Conversions.ChangeType(Expression, elementType), numArray);
                        }
                    }
                }
            }

            internal object InvokeMethod(
              Symbols.Method targetProcedure,
              object[] arguments,
              bool[] copyBack,
              BindingFlags flags)
            {
                MethodBase callTarget = NewLateBinding.GetCallTarget(targetProcedure, flags);
                object[] objArray = NewLateBinding.ConstructCallArguments(targetProcedure, arguments, flags);
                if (this._instance == null && !Symbols.IsShared((MemberInfo)callTarget))
                    throw new NullReferenceException(Utils.GetResourceString(SR.NullReference_InstanceReqToAccessMember1, targetProcedure.ToString()));
                if (Symbols.IsNonPublicRuntimeMember((MemberInfo)callTarget))
                    throw new MissingMemberException();
                object obj;
                try
                {
                    obj = callTarget.Invoke(this._instance, objArray);
                }
                catch (TargetInvocationException ex) when (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                OverloadResolution.ReorderArgumentArray(targetProcedure, objArray, arguments, copyBack, flags);
                return obj;
            }

            private class InheritanceSorter : IComparer<MemberInfo>
            {
                internal static readonly Symbols.Container.InheritanceSorter Instance = new Symbols.Container.InheritanceSorter();

                private InheritanceSorter()
                {
                }

                int IComparer<MemberInfo>.Compare(MemberInfo left, MemberInfo right)
                {
                    Type declaringType1 = left.DeclaringType;
                    Type declaringType2 = right.DeclaringType;
                    return (object)declaringType1 != (object)declaringType2 ? (!declaringType1.IsSubclassOf(declaringType2) ? 1 : -1) : 0;
                }
            }
        }

        internal sealed class Method
        {
            private MemberInfo _item;
            private MethodBase _rawItem;
            private ParameterInfo[] _parameters;
            private ParameterInfo[] _rawParameters;
            private ParameterInfo[] _rawParametersFromType;
            private Type _rawDeclaringType;
            internal readonly int ParamArrayIndex;
            internal readonly bool ParamArrayExpanded;
            internal bool NotCallable;
            internal bool RequiresNarrowingConversion;
            internal bool AllNarrowingIsFromObject;
            internal bool LessSpecific;
            internal bool ArgumentsValidated;
            internal int[] NamedArgumentMapping;
            internal Type[] TypeArguments;
            internal bool ArgumentMatchingDone;

            private Method(ParameterInfo[] parameters, int paramArrayIndex, bool paramArrayExpanded)
            {
                this._parameters = parameters;
                this._rawParameters = parameters;
                this.ParamArrayIndex = paramArrayIndex;
                this.ParamArrayExpanded = paramArrayExpanded;
                this.AllNarrowingIsFromObject = true;
            }

            internal Method(
              MethodBase method,
              ParameterInfo[] parameters,
              int paramArrayIndex,
              bool paramArrayExpanded)
              : this(parameters, paramArrayIndex, paramArrayExpanded)
            {
                this._item = (MemberInfo)method;
                this._rawItem = method;
            }

            internal Method(
              PropertyInfo property,
              ParameterInfo[] parameters,
              int paramArrayIndex,
              bool paramArrayExpanded)
              : this(parameters, paramArrayIndex, paramArrayExpanded)
            {
                this._item = (MemberInfo)property;
            }

            internal ParameterInfo[] Parameters
            {
                get
                {
                    return this._parameters;
                }
            }

            internal ParameterInfo[] RawParameters
            {
                get
                {
                    return this._rawParameters;
                }
            }

            internal ParameterInfo[] RawParametersFromType
            {
                get
                {
                    if (this._rawParametersFromType == null)
                    {
                        if (!this.IsProperty)
                        {
                            MethodInfo methodDefinition = (MethodInfo)this._item;
                            if (methodDefinition.IsGenericMethod)
                                methodDefinition = methodDefinition.GetGenericMethodDefinition();
                            Type type = methodDefinition.DeclaringType;
                            if (type.IsConstructedGenericType)
                                type = type.GetGenericTypeDefinition();
                            MethodBase methodBase = (MethodBase)null;
                            try
                            {
                                foreach (MethodInfo declaredMethod in type.GetTypeInfo().GetDeclaredMethods(methodDefinition.Name))
                                {
                                    if (declaredMethod.MetadataToken == methodDefinition.MetadataToken)
                                    {
                                        methodBase = (MethodBase)declaredMethod;
                                        break;
                                    }
                                }
                            }
                            finally
                            {
                                IEnumerator<MethodInfo> enumerator = null;
                                enumerator?.Dispose();
                            }
                            this._rawParametersFromType = methodBase.GetParameters();
                        }
                        else
                            this._rawParametersFromType = this._rawParameters;
                    }
                    return this._rawParametersFromType;
                }
            }

            internal Type DeclaringType
            {
                get
                {
                    return this._item.DeclaringType;
                }
            }

            internal Type RawDeclaringType
            {
                get
                {
                    if ((object)this._rawDeclaringType == null)
                    {
                        Type type = this._item.DeclaringType;
                        if (type.IsConstructedGenericType)
                            type = type.GetGenericTypeDefinition();
                        this._rawDeclaringType = type;
                    }
                    return this._rawDeclaringType;
                }
            }

            internal bool HasParamArray
            {
                get
                {
                    return this.ParamArrayIndex > -1;
                }
            }

            internal bool HasByRefParameter
            {
                get
                {
                    ParameterInfo[] parameters = this.Parameters;
                    int index = 0;
                    bool flag;
                    while (index < parameters.Length)
                    {
                        if (parameters[index].ParameterType.IsByRef)
                        {
                            flag = true;
                            goto label_6;
                        }
                        else
                            checked { ++index; }
                    }
                    flag = false;
                label_6:
                    return flag;
                }
            }

            internal bool IsProperty
            {
                get
                {
                    return this._item.MemberType == MemberTypes.Property;
                }
            }

            internal bool IsMethod
            {
                get
                {
                    return this._item.MemberType == MemberTypes.Method || this._item.MemberType == MemberTypes.Constructor;
                }
            }

            internal bool IsGeneric
            {
                get
                {
                    return Symbols.IsGeneric(this._item);
                }
            }

            internal Type[] TypeParameters
            {
                get
                {
                    return Symbols.GetTypeParameters(this._item);
                }
            }

            internal bool BindGenericArguments()
            {
                try
                {
                    this._item = (MemberInfo)((MethodInfo)this._rawItem).MakeGenericMethod(this.TypeArguments);
                    this._parameters = this.AsMethod().GetParameters();
                    return true;
                }
                catch (ArgumentException ex)
                {
                    return false;
                }
            }

            internal MethodBase AsMethod()
            {
                return this._item as MethodBase;
            }

            internal PropertyInfo AsProperty()
            {
                return this._item as PropertyInfo;
            }

            public static bool operator ==(Symbols.Method left, Symbols.Method right)
            {
                return (object)left._item == (object)right._item;
            }

            public static bool operator ==(MemberInfo left, Symbols.Method right)
            {
                return (object)left == (object)right._item;
            }

            public static bool operator !=(Symbols.Method left, Symbols.Method right)
            {
                return (object)left._item != (object)right._item;
            }

            public static bool operator !=(MemberInfo left, Symbols.Method right)
            {
                return (object)left != (object)right._item;
            }
            public override string ToString()
            {
                return Utils.MemberToString(this._item);
            }
        }

        internal sealed class TypedNothing
        {
            internal readonly Type Type;

            internal TypedNothing(Type type)
            {
                this.Type = type;
            }
        }
    }
}
