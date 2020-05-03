// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.VBGetMemberBinder
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Dynamic;
using System.Linq.Expressions;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class VBGetMemberBinder : GetMemberBinder, IInvokeOnGetBinder
  {
    private static readonly int s_hash = typeof (VBGetMemberBinder).GetHashCode();

    public VBGetMemberBinder(string name)
      : base(name, true)
    {
    }

    public override DynamicMetaObject FallbackGetMember(
      DynamicMetaObject target,
      DynamicMetaObject errorSuggestion)
    {
      return errorSuggestion == null ? new DynamicMetaObject((Expression) Expression.Constant(IDOBinder.missingMemberSentinel), IDOUtils.CreateRestrictions(target, (DynamicMetaObject[]) null, (DynamicMetaObject) null)) : errorSuggestion;
    }

    public override bool Equals(object _other)
    {
      return _other is VBGetMemberBinder vbGetMemberBinder && string.Equals(this.Name, vbGetMemberBinder.Name);
    }

    public override int GetHashCode()
    {
      return VBGetMemberBinder.s_hash ^ this.Name.GetHashCode();
    }

    bool IInvokeOnGetBinder.InvokeOnGet
    {
      get
      {
        return false;
      }
    }
  }
}
