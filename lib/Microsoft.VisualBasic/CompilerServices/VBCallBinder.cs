// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.VBCallBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class VBCallBinder : InvokeMemberBinder
  {
    private static readonly int s_hash = typeof (VBCallBinder).GetHashCode();
    private readonly bool _ignoreReturn;

    public VBCallBinder(string memberName, CallInfo callInfo, bool ignoreReturn)
      : base(memberName, true, callInfo)
    {
      this._ignoreReturn = ignoreReturn;
    }

    public override DynamicMetaObject FallbackInvokeMember(
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
        if (errorSuggestion != null && !NewLateBinding.CanBindCall(target.Value, this.Name, argValues, argNames, this._ignoreReturn))
        {
          dynamicMetaObject = errorSuggestion;
        }
        else
        {
          ParameterExpression parameterExpression = Expression.Variable(typeof (object), "result");
          ParameterExpression array = Expression.Variable(typeof (object[]), "array");
          Expression right = (Expression) Expression.Call(typeof (NewLateBinding).GetMethod("FallbackCall"), target.Expression, (Expression) Expression.Constant((object) this.Name, typeof (string)), (Expression) Expression.Assign((Expression) array, (Expression) Expression.NewArrayInit(typeof (object), args)), (Expression) Expression.Constant((object) argNames, typeof (string[])), (Expression) Expression.Constant((object) this._ignoreReturn, typeof (bool)));
          dynamicMetaObject = new DynamicMetaObject((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[2]
          {
            parameterExpression,
            array
          }, (Expression) Expression.Assign((Expression) parameterExpression, right), IDOUtils.GetWriteBack(args, array), (Expression) parameterExpression), IDOUtils.CreateRestrictions(target, packedArgs, (DynamicMetaObject) null));
        }
      }
      return dynamicMetaObject;
    }

    public override DynamicMetaObject FallbackInvoke(
      DynamicMetaObject target,
      DynamicMetaObject[] packedArgs,
      DynamicMetaObject errorSuggestion)
    {
      return new VBInvokeBinder(this.CallInfo, true).FallbackInvoke(target, packedArgs, errorSuggestion);
    }

    public override bool Equals(object _other)
    {
      return _other is VBCallBinder vbCallBinder && string.Equals(this.Name, vbCallBinder.Name) && this.CallInfo.Equals((object) vbCallBinder.CallInfo) && this._ignoreReturn == vbCallBinder._ignoreReturn;
    }

    public override int GetHashCode()
    {
      return VBCallBinder.s_hash ^ this.Name.GetHashCode() ^ this.CallInfo.GetHashCode() ^ this._ignoreReturn.GetHashCode();
    }
  }
}
