// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.VBSetBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class VBSetBinder : SetMemberBinder
  {
    private static readonly int s_hash = typeof (VBSetBinder).GetHashCode();

    public VBSetBinder(string memberName)
      : base(memberName, true)
    {
    }

    public override DynamicMetaObject FallbackSetMember(
      DynamicMetaObject target,
      DynamicMetaObject value,
      DynamicMetaObject errorSuggestion)
    {
      DynamicMetaObject dynamicMetaObject;
      if (IDOUtils.NeedsDeferral(target, (DynamicMetaObject[]) null, value))
        dynamicMetaObject = this.Defer(target, value);
      else if (errorSuggestion != null && !NewLateBinding.CanBindSet(target.Value, this.Name, value.Value, false, false))
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
        dynamicMetaObject = new DynamicMetaObject((Expression) Expression.Block((Expression) Expression.Call(typeof (NewLateBinding).GetMethod("FallbackSet"), target.Expression, (Expression) Expression.Constant((object) this.Name), (Expression) Expression.NewArrayInit(typeof (object), expressionArray)), expression), IDOUtils.CreateRestrictions(target, (DynamicMetaObject[]) null, value));
      }
      return dynamicMetaObject;
    }

    public override bool Equals(object _other)
    {
      return _other is VBSetBinder vbSetBinder && string.Equals(this.Name, vbSetBinder.Name);
    }

    public override int GetHashCode()
    {
      return VBSetBinder.s_hash ^ this.Name.GetHashCode();
    }
  }
}
