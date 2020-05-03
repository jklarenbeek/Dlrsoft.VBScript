// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.VBIndexSetComplexBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class VBIndexSetComplexBinder : SetIndexBinder
  {
    private static readonly int s_hash = typeof (VBIndexSetComplexBinder).GetHashCode();
    private readonly bool _optimisticSet;
    private readonly bool _rValueBase;

    public VBIndexSetComplexBinder(CallInfo callInfo, bool optimisticSet, bool rValueBase)
      : base(callInfo)
    {
      this._optimisticSet = optimisticSet;
      this._rValueBase = rValueBase;
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
        if (errorSuggestion != null && !NewLateBinding.CanIndexSetComplex(target.Value, arguments, argNames, this._optimisticSet, this._rValueBase))
        {
          dynamicMetaObject = errorSuggestion;
        }
        else
        {
          Expression expression = IDOUtils.ConvertToObject(value.Expression);
          Expression[] expressionArray = new Expression[checked (args.Length + 1)];
          args.CopyTo((Array) expressionArray, 0);
          expressionArray[args.Length] = expression;
          dynamicMetaObject = new DynamicMetaObject((Expression) Expression.Block((Expression) Expression.Call(typeof (NewLateBinding).GetMethod("FallbackIndexSetComplex"), target.Expression, (Expression) Expression.NewArrayInit(typeof (object), expressionArray), (Expression) Expression.Constant((object) argNames, typeof (string[])), (Expression) Expression.Constant((object) this._optimisticSet), (Expression) Expression.Constant((object) this._rValueBase)), expression), IDOUtils.CreateRestrictions(target, packedIndexes, value));
        }
      }
      return dynamicMetaObject;
    }

    public override bool Equals(object _other)
    {
      return _other is VBIndexSetComplexBinder setComplexBinder && this.CallInfo.Equals((object) setComplexBinder.CallInfo) && this._optimisticSet == setComplexBinder._optimisticSet && this._rValueBase == setComplexBinder._rValueBase;
    }

    public override int GetHashCode()
    {
      return VBIndexSetComplexBinder.s_hash ^ this.CallInfo.GetHashCode() ^ this._optimisticSet.GetHashCode() ^ this._rValueBase.GetHashCode();
    }
  }
}
