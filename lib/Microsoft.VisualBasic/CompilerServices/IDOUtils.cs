// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.IDOUtils
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class IDOUtils
  {
    private static CacheSet<CallSiteBinder> s_binderCache = new CacheSet<CallSiteBinder>(64);

    private static CallSiteBinder GetCachedBinder(CallSiteBinder action)
    {
      return IDOUtils.s_binderCache.GetExistingOrAdd(action);
    }

    internal static IDynamicMetaObjectProvider TryCastToIDMOP(object o)
    {
      return !(o is IDynamicMetaObjectProvider metaObjectProvider) ? (IDynamicMetaObjectProvider) null : metaObjectProvider;
    }

    internal static ExpressionType? LinqOperator(
      Symbols.UserDefinedOperator vbOperator)
    {
      ExpressionType? nullable;
      switch (vbOperator)
      {
        case Symbols.UserDefinedOperator.Negate:
          nullable = new ExpressionType?(ExpressionType.Negate);
          break;
        case Symbols.UserDefinedOperator.Not:
          nullable = new ExpressionType?(ExpressionType.Not);
          break;
        case Symbols.UserDefinedOperator.UnaryPlus:
          nullable = new ExpressionType?(ExpressionType.UnaryPlus);
          break;
        case Symbols.UserDefinedOperator.Plus:
          nullable = new ExpressionType?(ExpressionType.Add);
          break;
        case Symbols.UserDefinedOperator.Minus:
          nullable = new ExpressionType?(ExpressionType.Subtract);
          break;
        case Symbols.UserDefinedOperator.Multiply:
          nullable = new ExpressionType?(ExpressionType.Multiply);
          break;
        case Symbols.UserDefinedOperator.Divide:
          nullable = new ExpressionType?(ExpressionType.Divide);
          break;
        case Symbols.UserDefinedOperator.Power:
          nullable = new ExpressionType?(ExpressionType.Power);
          break;
        case Symbols.UserDefinedOperator.ShiftLeft:
          nullable = new ExpressionType?(ExpressionType.LeftShift);
          break;
        case Symbols.UserDefinedOperator.ShiftRight:
          nullable = new ExpressionType?(ExpressionType.RightShift);
          break;
        case Symbols.UserDefinedOperator.Modulus:
          nullable = new ExpressionType?(ExpressionType.Modulo);
          break;
        case Symbols.UserDefinedOperator.Or:
          nullable = new ExpressionType?(ExpressionType.Or);
          break;
        case Symbols.UserDefinedOperator.Xor:
          nullable = new ExpressionType?(ExpressionType.ExclusiveOr);
          break;
        case Symbols.UserDefinedOperator.And:
          nullable = new ExpressionType?(ExpressionType.And);
          break;
        case Symbols.UserDefinedOperator.Equal:
          nullable = new ExpressionType?(ExpressionType.Equal);
          break;
        case Symbols.UserDefinedOperator.NotEqual:
          nullable = new ExpressionType?(ExpressionType.NotEqual);
          break;
        case Symbols.UserDefinedOperator.Less:
          nullable = new ExpressionType?(ExpressionType.LessThan);
          break;
        case Symbols.UserDefinedOperator.LessEqual:
          nullable = new ExpressionType?(ExpressionType.LessThanOrEqual);
          break;
        case Symbols.UserDefinedOperator.GreaterEqual:
          nullable = new ExpressionType?(ExpressionType.GreaterThanOrEqual);
          break;
        case Symbols.UserDefinedOperator.Greater:
          nullable = new ExpressionType?(ExpressionType.GreaterThan);
          break;
        default:
          nullable = new ExpressionType?();
          break;
      }
      return nullable;
    }

    public static void CopyBackArguments(CallInfo callInfo, object[] packedArgs, object[] args)
    {
      if (packedArgs == args)
        return;
      int length = packedArgs.Length;
      int argumentCount = callInfo.ArgumentCount;
      int num1 = checked (length - callInfo.ArgumentNames.Count);
      int num2 = checked (length - 1);
      int index = 0;
      while (index <= num2)
      {
        args[index] = packedArgs[index < argumentCount ? checked (index + num1) % argumentCount : index];
        checked { ++index; }
      }
    }

    public static void PackArguments(
      int valueArgs,
      string[] argNames,
      object[] args,
      ref object[] packedArgs,
      ref CallInfo callInfo)
    {
      if (argNames == null)
        argNames = new string[0];
      callInfo = new CallInfo(checked (args.Length - valueArgs), argNames);
      if (argNames.Length > 0)
      {
        packedArgs = new object[checked (args.Length - 1 + 1)];
        int num1 = checked (args.Length - valueArgs);
        int num2 = checked (num1 - 1);
        int index1 = 0;
        while (index1 <= num2)
        {
          packedArgs[index1] = args[checked (index1 + argNames.Length) % num1];
          checked { ++index1; }
        }
        int num3 = num1;
        int num4 = checked (args.Length - 1);
        int index2 = num3;
        while (index2 <= num4)
        {
          packedArgs[index2] = args[index2];
          checked { ++index2; }
        }
      }
      else
        packedArgs = args;
    }

    public static void UnpackArguments(
      DynamicMetaObject[] packedArgs,
      CallInfo callInfo,
      ref Expression[] args,
      ref string[] argNames,
      ref object[] argValues)
    {
      int length = packedArgs.Length;
      int argumentCount = callInfo.ArgumentCount;
      args = new Expression[checked (length - 1 + 1)];
      argValues = new object[checked (length - 1 + 1)];
      int count = callInfo.ArgumentNames.Count;
      int num1 = checked (length - count);
      int num2 = checked (argumentCount - 1);
      int index1 = 0;
      while (index1 <= num2)
      {
        DynamicMetaObject packedArg = packedArgs[checked (index1 + num1) % argumentCount];
        args[index1] = packedArg.Expression;
        argValues[index1] = packedArg.Value;
        checked { ++index1; }
      }
      int num3 = argumentCount;
      int num4 = checked (length - 1);
      int index2 = num3;
      while (index2 <= num4)
      {
        DynamicMetaObject packedArg = packedArgs[index2];
        args[index2] = packedArg.Expression;
        argValues[index2] = packedArg.Value;
        checked { ++index2; }
      }
      argNames = new string[checked (count - 1 + 1)];
      callInfo.ArgumentNames.CopyTo(argNames, 0);
    }

    public static Expression GetWriteBack(
      Expression[] arguments,
      ParameterExpression array)
    {
      List<Expression> expressionList = new List<Expression>();
      int num = checked (arguments.Length - 1);
      int index = 0;
      while (index <= num)
      {
        if (arguments[index] is ParameterExpression parameterExpression && parameterExpression.IsByRef)
          expressionList.Add((Expression) Expression.Assign((Expression) parameterExpression, (Expression) Expression.ArrayIndex((Expression) array, (Expression) Expression.Constant((object) index))));
        checked { ++index; }
      }
      Expression expression;
      switch (expressionList.Count)
      {
        case 0:
          expression = (Expression) Expression.Empty();
          break;
        case 1:
          expression = expressionList[0];
          break;
        default:
          expression = (Expression) Expression.Block((IEnumerable<Expression>) expressionList);
          break;
      }
      return expression;
    }

    public static Expression ConvertToObject(Expression valueExpression)
    {
      return !valueExpression.Type.Equals(typeof (object)) ? (Expression) Expression.Convert(valueExpression, typeof (object)) : valueExpression;
    }

    public static object CreateRefCallSiteAndInvoke(
      CallSiteBinder action,
      object instance,
      object[] arguments)
    {
      action = IDOUtils.GetCachedBinder(action);
      switch (arguments.Length)
      {
        case 0:
          CallSite<SiteDelegate0> callSite1 = CallSite<SiteDelegate0>.Create(action);
          return callSite1.Target((CallSite) callSite1, instance);
        case 1:
          CallSite<SiteDelegate1> callSite2 = CallSite<SiteDelegate1>.Create(action);
          return callSite2.Target((CallSite) callSite2, instance, ref arguments[0]);
        case 2:
          CallSite<SiteDelegate2> callSite3 = CallSite<SiteDelegate2>.Create(action);
          return callSite3.Target((CallSite) callSite3, instance, ref arguments[0], ref arguments[1]);
        case 3:
          CallSite<SiteDelegate3> callSite4 = CallSite<SiteDelegate3>.Create(action);
          return callSite4.Target((CallSite) callSite4, instance, ref arguments[0], ref arguments[1], ref arguments[2]);
        case 4:
          CallSite<SiteDelegate4> callSite5 = CallSite<SiteDelegate4>.Create(action);
          return callSite5.Target((CallSite) callSite5, instance, ref arguments[0], ref arguments[1], ref arguments[2], ref arguments[3]);
        case 5:
          CallSite<SiteDelegate5> callSite6 = CallSite<SiteDelegate5>.Create(action);
          return callSite6.Target((CallSite) callSite6, instance, ref arguments[0], ref arguments[1], ref arguments[2], ref arguments[3], ref arguments[4]);
        case 6:
          CallSite<SiteDelegate6> callSite7 = CallSite<SiteDelegate6>.Create(action);
          return callSite7.Target((CallSite) callSite7, instance, ref arguments[0], ref arguments[1], ref arguments[2], ref arguments[3], ref arguments[4], ref arguments[5]);
        case 7:
          CallSite<SiteDelegate7> callSite8 = CallSite<SiteDelegate7>.Create(action);
          return callSite8.Target((CallSite) callSite8, instance, ref arguments[0], ref arguments[1], ref arguments[2], ref arguments[3], ref arguments[4], ref arguments[5], ref arguments[6]);
        default:
          Type[] typeArray = new Type[checked (arguments.Length + 2 + 1)];
          Type type = typeof (object).MakeByRefType();
          typeArray[0] = typeof (CallSite);
          typeArray[1] = typeof (object);
          typeArray[checked (typeArray.Length - 1)] = typeof (object);
          int num = checked (typeArray.Length - 2);
          int index = 2;
          while (index <= num)
          {
            typeArray[index] = type;
            checked { ++index; }
          }
          CallSite callSite9 = CallSite.Create(Expression.GetDelegateType(typeArray), action);
          object[] objArray = new object[checked (arguments.Length + 1 + 1)];
          objArray[0] = (object) callSite9;
          objArray[1] = instance;
          arguments.CopyTo((Array) objArray, 2);
          Delegate @delegate = (Delegate) callSite9.GetType().GetField("Target").GetValue((object) callSite9);
          try
          {
            object obj = @delegate.DynamicInvoke(objArray);
            Array.Copy((Array) objArray, 2, (Array) arguments, 0, arguments.Length);
            return obj;
          }
          catch (TargetInvocationException ex)
          {
            throw ex.InnerException;
          }
      }
    }

    public static object CreateFuncCallSiteAndInvoke(
      CallSiteBinder action,
      object instance,
      object[] arguments)
    {
      action = IDOUtils.GetCachedBinder(action);
      switch (arguments.Length)
      {
        case 0:
          CallSite<Func<CallSite, object, object>> callSite1 = CallSite<Func<CallSite, object, object>>.Create(action);
          return callSite1.Target((CallSite) callSite1, instance);
        case 1:
          CallSite<Func<CallSite, object, object, object>> callSite2 = CallSite<Func<CallSite, object, object, object>>.Create(action);
          return callSite2.Target((CallSite) callSite2, instance, arguments[0]);
        case 2:
          CallSite<Func<CallSite, object, object, object, object>> callSite3 = CallSite<Func<CallSite, object, object, object, object>>.Create(action);
          return callSite3.Target((CallSite) callSite3, instance, arguments[0], arguments[1]);
        case 3:
          CallSite<Func<CallSite, object, object, object, object, object>> callSite4 = CallSite<Func<CallSite, object, object, object, object, object>>.Create(action);
          return callSite4.Target((CallSite) callSite4, instance, arguments[0], arguments[1], arguments[2]);
        case 4:
          CallSite<Func<CallSite, object, object, object, object, object, object>> callSite5 = CallSite<Func<CallSite, object, object, object, object, object, object>>.Create(action);
          return callSite5.Target((CallSite) callSite5, instance, arguments[0], arguments[1], arguments[2], arguments[3]);
        case 5:
          CallSite<Func<CallSite, object, object, object, object, object, object, object>> callSite6 = CallSite<Func<CallSite, object, object, object, object, object, object, object>>.Create(action);
          return callSite6.Target((CallSite) callSite6, instance, arguments[0], arguments[1], arguments[2], arguments[3], arguments[4]);
        case 6:
          CallSite<Func<CallSite, object, object, object, object, object, object, object, object>> callSite7 = CallSite<Func<CallSite, object, object, object, object, object, object, object, object>>.Create(action);
          return callSite7.Target((CallSite) callSite7, instance, arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
        case 7:
          CallSite<Func<CallSite, object, object, object, object, object, object, object, object, object>> callSite8 = CallSite<Func<CallSite, object, object, object, object, object, object, object, object, object>>.Create(action);
          return callSite8.Target((CallSite) callSite8, instance, arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5], arguments[6]);
        default:
          Type[] typeArray = new Type[checked (arguments.Length + 2 + 1)];
          typeArray[0] = typeof (CallSite);
          int num = checked (typeArray.Length - 1);
          int index = 1;
          while (index <= num)
          {
            typeArray[index] = typeof (object);
            checked { ++index; }
          }
          CallSite callSite9 = CallSite.Create(Expression.GetDelegateType(typeArray), action);
          object[] objArray = new object[checked (arguments.Length + 1 + 1)];
          objArray[0] = (object) callSite9;
          objArray[1] = instance;
          arguments.CopyTo((Array) objArray, 2);
          Delegate @delegate = (Delegate) callSite9.GetType().GetField("Target").GetValue((object) callSite9);
          try
          {
            return @delegate.DynamicInvoke(objArray);
          }
          catch (TargetInvocationException ex)
          {
            throw ex.InnerException;
          }
      }
    }

    public static object CreateConvertCallSiteAndInvoke(ConvertBinder action, object instance)
    {
      CallSite callSite = CallSite.Create(Expression.GetFuncType(typeof (CallSite), typeof (object), action.Type), IDOUtils.GetCachedBinder((CallSiteBinder) action));
      object[] objArray = new object[2]
      {
        (object) callSite,
        instance
      };
      Delegate @delegate = (Delegate) callSite.GetType().GetField("Target").GetValue((object) callSite);
      try
      {
        return @delegate.DynamicInvoke(objArray);
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    internal static BindingRestrictions CreateRestrictions(
      DynamicMetaObject target,
      DynamicMetaObject[] args = null,
      DynamicMetaObject value = null)
    {
      BindingRestrictions bindingRestrictions = IDOUtils.CreateRestriction(target);
      if (args != null)
      {
        DynamicMetaObject[] dynamicMetaObjectArray = args;
        int index = 0;
        while (index < dynamicMetaObjectArray.Length)
        {
          DynamicMetaObject metaObject = dynamicMetaObjectArray[index];
          bindingRestrictions = bindingRestrictions.Merge(IDOUtils.CreateRestriction(metaObject));
          checked { ++index; }
        }
      }
      if (value != null)
        bindingRestrictions = bindingRestrictions.Merge(IDOUtils.CreateRestriction(value));
      return bindingRestrictions;
    }

    private static BindingRestrictions CreateRestriction(
      DynamicMetaObject metaObject)
    {
      return metaObject.Value != null ? metaObject.Restrictions.Merge(BindingRestrictions.GetTypeRestriction(metaObject.Expression, metaObject.LimitType)) : metaObject.Restrictions.Merge(BindingRestrictions.GetInstanceRestriction(metaObject.Expression, (object) null));
    }

    internal static bool NeedsDeferral(
      DynamicMetaObject target,
      DynamicMetaObject[] args = null,
      DynamicMetaObject value = null)
    {
      bool flag;
      if (!target.HasValue)
        flag = true;
      else if (value != null && !value.HasValue)
      {
        flag = true;
      }
      else
      {
        if (args != null)
        {
          DynamicMetaObject[] dynamicMetaObjectArray = args;
          int index = 0;
          while (index < dynamicMetaObjectArray.Length)
          {
            if (!dynamicMetaObjectArray[index].HasValue)
            {
              flag = true;
              goto label_11;
            }
            else
              checked { ++index; }
          }
        }
        flag = false;
      }
label_11:
      return flag;
    }
  }
}
