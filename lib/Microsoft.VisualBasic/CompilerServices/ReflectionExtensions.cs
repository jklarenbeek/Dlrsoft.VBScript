// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.ReflectionExtensions
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
  [StandardModule]
  internal static class ReflectionExtensions
  {
    public static TypeCode GetTypeCode(this Type type)
    {
      return Type.GetTypeCode(type);
    }

    public static BindingFlags BindingFlagsInvokeMethod
    {
      get
      {
        return BindingFlags.InvokeMethod;
      }
    }

    public static BindingFlags BindingFlagsGetProperty
    {
      get
      {
        return BindingFlags.GetProperty;
      }
    }

    public static BindingFlags BindingFlagsSetProperty
    {
      get
      {
        return BindingFlags.SetProperty;
      }
    }

    public static BindingFlags BindingFlagsIgnoreReturn
    {
      get
      {
        return BindingFlags.IgnoreReturn;
      }
    }

    private static bool IsGenericallyEqual(this Type t1, Type t2)
    {
      return (object) t1 == null || (object) t2 == null ? (object) t1 == null && (object) t2 == null : t1.Equals(t2) || (t1.IsConstructedGenericType || t2.IsConstructedGenericType) && (t1.IsConstructedGenericType ? t1.GetGenericTypeDefinition() : t1).Equals(t2.IsConstructedGenericType ? t2.GetGenericTypeDefinition() : t2);
    }

    private static bool IsTypeParameterEquivalentToTypeInst(
      this Type typeParam,
      Type typeInst,
      MemberInfo member)
    {
      bool flag;
      if ((object) typeParam.DeclaringMethod != null)
      {
        if ((object) (member as MethodBase) == null)
        {
          flag = false;
        }
        else
        {
          MethodBase methodBase = (MethodBase) member;
          int parameterPosition = typeParam.GenericParameterPosition;
          Type[] typeArray = methodBase.IsGenericMethod ? methodBase.GetGenericArguments() : (Type[]) null;
          flag = typeArray != null && typeArray.Length > parameterPosition && typeArray[parameterPosition].Equals(typeInst);
        }
      }
      else
        flag = member.DeclaringType.GetGenericArguments()[typeParam.GenericParameterPosition].Equals(typeInst);
      return flag;
    }
  }
}
