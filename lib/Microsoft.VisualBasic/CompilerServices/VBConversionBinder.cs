// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.VBConversionBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class VBConversionBinder : ConvertBinder
  {
    private static readonly int s_hash = typeof (VBConversionBinder).GetHashCode();

    public VBConversionBinder(Type t)
      : base(t, true)
    {
    }

    public override DynamicMetaObject FallbackConvert(
      DynamicMetaObject target,
      DynamicMetaObject errorSuggestion)
    {
      return !IDOUtils.NeedsDeferral(target, (DynamicMetaObject[]) null, (DynamicMetaObject) null) ? (errorSuggestion == null || Conversions.CanUserDefinedConvert(target.Value, this.Type) ? new DynamicMetaObject((Expression) Expression.Convert((Expression) Expression.Call(typeof (Conversions).GetMethod("FallbackUserDefinedConversion"), target.Expression, (Expression) Expression.Constant((object) this.Type, typeof (Type))), this.ReturnType), IDOUtils.CreateRestrictions(target, (DynamicMetaObject[]) null, (DynamicMetaObject) null)) : errorSuggestion) : this.Defer(target);
    }

    public override bool Equals(object _other)
    {
      return _other is VBConversionBinder conversionBinder && this.Type.Equals(conversionBinder.Type);
    }

    public override int GetHashCode()
    {
      return VBConversionBinder.s_hash ^ this.Type.GetHashCode();
    }
  }
}
