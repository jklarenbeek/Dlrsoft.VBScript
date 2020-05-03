// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.VBInvokeDefaultFallbackBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class VBInvokeDefaultFallbackBinder : GetIndexBinder
  {
    private static readonly int s_hash = typeof (VBInvokeDefaultFallbackBinder).GetHashCode();
    private readonly bool _reportErrors;

    public VBInvokeDefaultFallbackBinder(CallInfo callInfo, bool reportErrors)
      : base(callInfo)
    {
      this._reportErrors = reportErrors;
    }

    public override DynamicMetaObject FallbackGetIndex(
      DynamicMetaObject target,
      DynamicMetaObject[] packedArgs,
      DynamicMetaObject errorSuggestion)
    {
      DynamicMetaObject dynamicMetaObject;
      if (IDOUtils.NeedsDeferral(target, packedArgs, (DynamicMetaObject) null))
      {
        dynamicMetaObject = this.Defer(target, packedArgs);
      }
      else
      {
        Expression[] args = (Expression[]) null;
        string[] argNames = (string[]) null;
        object[] argValues = (object[]) null;
        IDOUtils.UnpackArguments(packedArgs, this.CallInfo, ref args, ref argNames, ref argValues);
        if (errorSuggestion != null && !NewLateBinding.CanBindInvokeDefault(target.Value, argValues, argNames, this._reportErrors))
        {
          dynamicMetaObject = errorSuggestion;
        }
        else
        {
          ParameterExpression parameterExpression = Expression.Variable(typeof (object), "result");
          ParameterExpression array = Expression.Variable(typeof (object[]), "array");
          Expression right = (Expression) Expression.Call(typeof (NewLateBinding).GetMethod("FallbackInvokeDefault2"), target.Expression, (Expression) Expression.Assign((Expression) array, (Expression) Expression.NewArrayInit(typeof (object), args)), (Expression) Expression.Constant((object) argNames, typeof (string[])), (Expression) Expression.Constant((object) this._reportErrors));
          dynamicMetaObject = new DynamicMetaObject((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[2]
          {
            parameterExpression,
            array
          }, (Expression) Expression.Assign((Expression) parameterExpression, right), IDOUtils.GetWriteBack(args, array), (Expression) parameterExpression), IDOUtils.CreateRestrictions(target, packedArgs, (DynamicMetaObject) null));
        }
      }
      return dynamicMetaObject;
    }

    public override bool Equals(object _other)
    {
      return _other is VBInvokeDefaultFallbackBinder defaultFallbackBinder && this.CallInfo.Equals((object) defaultFallbackBinder.CallInfo) && this._reportErrors == defaultFallbackBinder._reportErrors;
    }

    public override int GetHashCode()
    {
      return VBInvokeDefaultFallbackBinder.s_hash ^ this.CallInfo.GetHashCode() ^ this._reportErrors.GetHashCode();
    }
  }
}
