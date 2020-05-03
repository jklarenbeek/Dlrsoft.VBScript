// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>The Visual Basic compiler uses this class internally when initializing static local members; it is not meant to be called directly from your code.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  public sealed class StaticLocalInitFlag
  {
    /// <summary>Returns the state of the static local member's initialization flag (initialized or not).</summary>
    public short State;
  }
}
