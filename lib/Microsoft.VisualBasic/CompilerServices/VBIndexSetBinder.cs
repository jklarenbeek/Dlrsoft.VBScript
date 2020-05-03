// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.VBIndexSetBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class VBIndexSetBinder : SetIndexBinder
  {
    private static readonly int s_hash = typeof (VBIndexSetBinder).GetHashCode();

    public VBIndexSetBinder(CallInfo callInfo)
      : base(callInfo)
    {
    }

    public override DynamicMetaObject FallbackSetIndex(
      DynamicMetaObject target,
      DynamicMetaObject[] packedIndexes,
      DynamicMetaObject value,
      DynamicMetaObject errorSuggestion)
    {
      DynamicMetaObject dynamicMetaObject;
      if (IDOUtils.NeedsDeferral(target, packedIndexes, value))
      {
        Array.Resize<DynamicMetaObject>(ref packedIndexes, checked (packedIndexes.Length + 1));
        packedIndexes[checked (packedIndexes.Length - 1)] = value;
        dynamicMetaObject = this.Defer(target, packedIndexes);
      }
      else
      {
        string[] argNames = (string[]) null;
        Expression[] args = (Expression[]) null;
        object[] argValues = (object[]) null;
        IDOUtils.UnpackArguments(packedIndexes, this.CallInfo, ref args, ref argNames, ref argValues);
        object[] arguments = new object[checked (argValues.Length + 1)];
        argValues.CopyTo((Array) arguments, 0);
        arguments[argValues.Length] = value.Value;
        if (errorSuggestion != null && !NewLateBinding.CanIndexSetComplex(target.Value, arguments, argNames, false, false))
        {
          dynamicMetaObject = errorSuggestion;
        }
        else
        {
          Expression expression = IDOUtils.ConvertToObject(value.Expression);
          Expression[] expressionArray = new Expression[checked (args.Length + 1)];
          args.CopyTo((Array) expressionArray, 0);
          expressionArray[args.Length] = expression;
          dynamicMetaObject = new DynamicMetaObject((Expression) Expression.Block((Expression) Expression.Call(typeof (NewLateBinding).GetMethod("FallbackIndexSet"), target.Expression, (Expression) Expression.NewArrayInit(typeof (object), expressionArray), (Expression) Expression.Constant((object) argNames, typeof (string[]))), expression), IDOUtils.CreateRestrictions(target, packedIndexes, value));
        }
      }
      return dynamicMetaObject;
    }

    public override bool Equals(object _other)
    {
      return _other is VBIndexSetBinder vbIndexSetBinder && this.CallInfo.Equals((object) vbIndexSetBinder.CallInfo);
    }

    public override int GetHashCode()
    {
      return VBIndexSetBinder.s_hash ^ this.CallInfo.GetHashCode();
    }
  }
}
