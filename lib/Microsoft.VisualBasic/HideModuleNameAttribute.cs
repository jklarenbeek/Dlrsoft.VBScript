// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.HideModuleNameAttribute
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic
{
  /// <summary>The <see langword="HideModuleNameAttribute" /> attribute, when applied to a module, allows the module members to be accessed using only the qualification needed for the module.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class HideModuleNameAttribute : Attribute
  {
  }
}
