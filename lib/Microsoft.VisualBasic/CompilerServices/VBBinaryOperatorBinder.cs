// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.VBBinaryOperatorBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class VBBinaryOperatorBinder : BinaryOperationBinder
  {
    private static readonly int s_hash = typeof (VBBinaryOperatorBinder).GetHashCode();
    private readonly Symbols.UserDefinedOperator _Op;

    public VBBinaryOperatorBinder(Symbols.UserDefinedOperator op, ExpressionType linqOp)
      : base(linqOp)
    {
      this._Op = op;
    }

    public override DynamicMetaObject FallbackBinaryOperation(
      DynamicMetaObject target,
      DynamicMetaObject arg,
      DynamicMetaObject errorSuggestion)
    {
      DynamicMetaObject dynamicMetaObject;
      if (IDOUtils.NeedsDeferral(target, (DynamicMetaObject[]) null, arg))
      {
        dynamicMetaObject = this.Defer(target, arg);
      }
      else
      {
        if (errorSuggestion != null)
        {
          if ((object) Operators.GetCallableUserDefinedOperator(this._Op, target.Value, arg.Value) == null)
          {
            dynamicMetaObject = errorSuggestion;
            goto label_6;
          }
        }
        dynamicMetaObject = new DynamicMetaObject((Expression) Expression.Call(typeof (Operators).GetMethod("FallbackInvokeUserDefinedOperator"), (Expression) Expression.Constant((object) this._Op, typeof (object)), (Expression) Expression.NewArrayInit(typeof (object), IDOUtils.ConvertToObject(target.Expression), IDOUtils.ConvertToObject(arg.Expression))), IDOUtils.CreateRestrictions(target, (DynamicMetaObject[]) null, arg));
      }
label_6:
      return dynamicMetaObject;
    }

    public override bool Equals(object _other)
    {
      return _other is VBBinaryOperatorBinder binaryOperatorBinder && this._Op == binaryOperatorBinder._Op && this.Operation == binaryOperatorBinder.Operation;
    }

    public override int GetHashCode()
    {
      return VBBinaryOperatorBinder.s_hash ^ this._Op.GetHashCode() ^ this.Operation.GetHashCode();
    }
  }
}
