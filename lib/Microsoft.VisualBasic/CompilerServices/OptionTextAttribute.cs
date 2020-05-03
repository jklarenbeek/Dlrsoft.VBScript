// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.OptionTextAttribute
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>The Visual Basic compiler emits this helper class to indicate (for Visual Basic debugging) which comparison option, binary or text, is being used</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class OptionTextAttribute : Attribute
  {
  }
}
