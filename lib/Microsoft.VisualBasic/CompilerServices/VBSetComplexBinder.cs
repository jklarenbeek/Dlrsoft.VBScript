// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.VBSetComplexBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class VBSetComplexBinder : SetMemberBinder
  {
    private static readonly int s_hash = typeof (VBSetComplexBinder).GetHashCode();
    private readonly bool _optimisticSet;
    private readonly bool _rValueBase;

    public VBSetComplexBinder(string memberName, bool optimisticSet, bool rValueBase)
      : base(memberName, true)
    {
      this._optimisticSet = optimisticSet;
      this._rValueBase = rValueBase;
    }

    public override DynamicMetaObject FallbackSetMember(
      DynamicMetaObject target,
      DynamicMetaObject value,
      DynamicMetaObject errorSuggestion)
    {
      DynamicMetaObject dynamicMetaObject;
      if (IDOUtils.NeedsDeferral(target, (DynamicMetaObject[]) null, value))
        dynamicMetaObject = this.Defer(target, value);
      else if (errorSuggestion != null && !NewLateBinding.CanBindSet(target.Value, this.Name, value.Value, this._optimisticSet, this._rValueBase))
      {
        dynamicMetaObject = errorSuggestion;
      }
      else
      {
        Expression expression = IDOUtils.ConvertToObject(value.Expression);
        Expression[] expressionArray = new Expression[1]
        {
          expression
        };
        dynamicMetaObject = new DynamicMetaObject((Expression) Expression.Block((Expression) Expression.Call(typeof (NewLateBinding).GetMethod("FallbackSetComplex"), target.Expression, (Expression) Expression.Constant((object) this.Name), (Expression) Expression.NewArrayInit(typeof (object), expressionArray), (Expression) Expression.Constant((object) this._optimisticSet), (Expression) Expression.Constant((object) this._rValueBase)), expression), IDOUtils.CreateRestrictions(target, (DynamicMetaObject[]) null, value));
      }
      return dynamicMetaObject;
    }

    public override bool Equals(object _other)
    {
      return _other is VBSetComplexBinder setComplexBinder && string.Equals(this.Name, setComplexBinder.Name) && this._optimisticSet == setComplexBinder._optimisticSet && this._rValueBase == setComplexBinder._rValueBase;
    }

    public override int GetHashCode()
    {
      return VBSetComplexBinder.s_hash ^ this.Name.GetHashCode() ^ this._optimisticSet.GetHashCode() ^ this._rValueBase.GetHashCode();
    }
  }
}
