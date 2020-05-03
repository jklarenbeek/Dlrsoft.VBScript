// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.VBFixedStringAttribute
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;

namespace Microsoft.VisualBasic
{
  /// <summary>Indicates that a string should be treated as if it were fixed length.</summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public sealed class VBFixedStringAttribute : Attribute
  {
    /// <summary>Gets the length of the string.</summary>
    /// <returns>The length of the string.</returns>
    public int Length { get; }

    /// <summary>Initializes the value of the <see langword="SizeConst" /> field.</summary>
    /// <param name="Length">The length of the fixed string.</param>
    public VBFixedStringAttribute(int Length)
    {
      if (Length < 1 || Length > (int) short.MaxValue)
        throw new ArgumentException(SR.Invalid_VBFixedString);
      this.Length = Length;
    }
  }
}
