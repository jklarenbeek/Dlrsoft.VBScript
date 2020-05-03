// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.IDOBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class IDOBinder
  {
    internal static readonly object missingMemberSentinel = new object();

    internal static bool[] GetCopyBack()
    {
      return IDOBinder.SaveCopyBack.GetCopyBack();
    }

    internal static object IDOCall(
      IDynamicMetaObjectProvider instance,
      string memberName,
      object[] arguments,
      string[] argumentNames,
      bool[] copyBack,
      bool ignoreReturn)
    {
      using (new IDOBinder.SaveCopyBack(copyBack))
      {
        CallInfo callInfo = (CallInfo) null;
        object[] packedArgs = (object[]) null;
        IDOUtils.PackArguments(0, argumentNames, arguments, ref packedArgs, ref callInfo);
        try
        {
          return IDOUtils.CreateRefCallSiteAndInvoke((CallSiteBinder) new VBCallBinder(memberName, callInfo, ignoreReturn), (object) instance, packedArgs);
        }
        finally
        {
          IDOUtils.CopyBackArguments(callInfo, packedArgs, arguments);
        }
      }
    }

    internal static object IDOGet(
      IDynamicMetaObjectProvider instance,
      string memberName,
      object[] arguments,
      string[] argumentNames,
      bool[] copyBack)
    {
      using (new IDOBinder.SaveCopyBack(copyBack))
      {
        object[] packedArgs = (object[]) null;
        CallInfo callInfo = (CallInfo) null;
        IDOUtils.PackArguments(0, argumentNames, arguments, ref packedArgs, ref callInfo);
        try
        {
          return IDOUtils.CreateRefCallSiteAndInvoke((CallSiteBinder) new VBGetBinder(memberName, callInfo), (object) instance, packedArgs);
        }
        finally
        {
          IDOUtils.CopyBackArguments(callInfo, packedArgs, arguments);
        }
      }
    }

    internal static object IDOInvokeDefault(
      IDynamicMetaObjectProvider instance,
      object[] arguments,
      string[] argumentNames,
      bool reportErrors,
      bool[] copyBack)
    {
      using (new IDOBinder.SaveCopyBack(copyBack))
      {
        object[] packedArgs = (object[]) null;
        CallInfo callInfo = (CallInfo) null;
        IDOUtils.PackArguments(0, argumentNames, arguments, ref packedArgs, ref callInfo);
        try
        {
          return IDOUtils.CreateRefCallSiteAndInvoke((CallSiteBinder) new VBInvokeDefaultBinder(callInfo, reportErrors), (object) instance, packedArgs);
        }
        finally
        {
          IDOUtils.CopyBackArguments(callInfo, packedArgs, arguments);
        }
      }
    }

    internal static object IDOFallbackInvokeDefault(
      IDynamicMetaObjectProvider instance,
      object[] arguments,
      string[] argumentNames,
      bool reportErrors,
      bool[] copyBack)
    {
      using (new IDOBinder.SaveCopyBack(copyBack))
      {
        object[] packedArgs = (object[]) null;
        CallInfo callInfo = (CallInfo) null;
        IDOUtils.PackArguments(0, argumentNames, arguments, ref packedArgs, ref callInfo);
        try
        {
          return IDOUtils.CreateRefCallSiteAndInvoke((CallSiteBinder) new VBInvokeDefaultFallbackBinder(callInfo, reportErrors), (object) instance, packedArgs);
        }
        finally
        {
          IDOUtils.CopyBackArguments(callInfo, packedArgs, arguments);
        }
      }
    }

    internal static void IDOSet(
      IDynamicMetaObjectProvider instance,
      string memberName,
      string[] argumentNames,
      object[] arguments)
    {
      using (new IDOBinder.SaveCopyBack((bool[]) null))
      {
        if (arguments.Length == 1)
        {
          IDOUtils.CreateFuncCallSiteAndInvoke((CallSiteBinder) new VBSetBinder(memberName), (object) instance, arguments);
        }
        else
        {
          object callSiteAndInvoke = IDOUtils.CreateFuncCallSiteAndInvoke((CallSiteBinder) new VBGetMemberBinder(memberName), (object) instance, Symbols.NoArguments);
          if (callSiteAndInvoke == IDOBinder.missingMemberSentinel)
            NewLateBinding.ObjectLateSet((object) instance, (Type) null, memberName, arguments, argumentNames, Symbols.NoTypeArguments);
          else
            NewLateBinding.LateIndexSet(callSiteAndInvoke, arguments, argumentNames);
        }
      }
    }

    internal static void IDOSetComplex(
      IDynamicMetaObjectProvider instance,
      string memberName,
      object[] arguments,
      string[] argumentNames,
      bool optimisticSet,
      bool rValueBase)
    {
      using (new IDOBinder.SaveCopyBack((bool[]) null))
      {
        if (arguments.Length == 1)
        {
          IDOUtils.CreateFuncCallSiteAndInvoke((CallSiteBinder) new VBSetComplexBinder(memberName, optimisticSet, rValueBase), (object) instance, arguments);
        }
        else
        {
          object callSiteAndInvoke = IDOUtils.CreateFuncCallSiteAndInvoke((CallSiteBinder) new VBGetMemberBinder(memberName), (object) instance, Symbols.NoArguments);
          if (callSiteAndInvoke == IDOBinder.missingMemberSentinel)
            NewLateBinding.ObjectLateSetComplex((object) instance, (Type) null, memberName, arguments, argumentNames, Symbols.NoTypeArguments, optimisticSet, rValueBase);
          else
            NewLateBinding.LateIndexSetComplex(callSiteAndInvoke, arguments, argumentNames, optimisticSet, rValueBase);
        }
      }
    }

    internal static void IDOIndexSet(
      IDynamicMetaObjectProvider instance,
      object[] arguments,
      string[] argumentNames)
    {
      using (new IDOBinder.SaveCopyBack((bool[]) null))
      {
        object[] packedArgs = (object[]) null;
        CallInfo callInfo = (CallInfo) null;
        IDOUtils.PackArguments(1, argumentNames, arguments, ref packedArgs, ref callInfo);
        IDOUtils.CreateFuncCallSiteAndInvoke((CallSiteBinder) new VBIndexSetBinder(callInfo), (object) instance, packedArgs);
      }
    }

    internal static void IDOIndexSetComplex(
      IDynamicMetaObjectProvider instance,
      object[] arguments,
      string[] argumentNames,
      bool optimisticSet,
      bool rValueBase)
    {
      using (new IDOBinder.SaveCopyBack((bool[]) null))
      {
        object[] packedArgs = (object[]) null;
        CallInfo callInfo = (CallInfo) null;
        IDOUtils.PackArguments(1, argumentNames, arguments, ref packedArgs, ref callInfo);
        IDOUtils.CreateFuncCallSiteAndInvoke((CallSiteBinder) new VBIndexSetComplexBinder(callInfo, optimisticSet, rValueBase), (object) instance, packedArgs);
      }
    }

    internal static object UserDefinedConversion(
      IDynamicMetaObjectProvider expression,
      Type targetType)
    {
      return IDOUtils.CreateConvertCallSiteAndInvoke((ConvertBinder) new VBConversionBinder(targetType), (object) expression);
    }

    internal static object InvokeUserDefinedOperator(
      Symbols.UserDefinedOperator op,
      object[] arguments)
    {
      ExpressionType? nullable = IDOUtils.LinqOperator(op);
      object obj;
      if (!nullable.HasValue)
      {
        obj = Operators.InvokeObjectUserDefinedOperator(op, arguments);
      }
      else
      {
        ExpressionType linqOp = nullable.Value;
        CallSiteBinder action = arguments.Length != 1 ? (CallSiteBinder) new VBBinaryOperatorBinder(op, linqOp) : (CallSiteBinder) new VBUnaryOperatorBinder(op, linqOp);
        object instance = arguments[0];
        object[] objArray;
        if (arguments.Length != 1)
          objArray = new object[1]{ arguments[1] };
        else
          objArray = Symbols.NoArguments;
        object[] arguments1 = objArray;
        obj = IDOUtils.CreateFuncCallSiteAndInvoke(action, instance, arguments1);
      }
      return obj;
    }

    private struct SaveCopyBack : IDisposable
    {
      [ThreadStatic]
      private static bool[] s_savedCopyBack;
      private bool[] _oldCopyBack;

      public SaveCopyBack(bool[] copyBack)
        : this()
      {
        this._oldCopyBack = IDOBinder.SaveCopyBack.s_savedCopyBack;
        IDOBinder.SaveCopyBack.s_savedCopyBack = copyBack;
      }

      public void Dispose()
      {
        IDOBinder.SaveCopyBack.s_savedCopyBack = this._oldCopyBack;
      }

      internal static bool[] GetCopyBack()
      {
        return IDOBinder.SaveCopyBack.s_savedCopyBack;
      }
    }
  }
}
