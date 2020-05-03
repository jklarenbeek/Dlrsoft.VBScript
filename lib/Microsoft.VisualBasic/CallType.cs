// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CallType
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

namespace Microsoft.VisualBasic
{
  /// <summary>Indicates the type of procedure being invoked when calling the <see langword="CallByName" /> function.</summary>
  public enum CallType
  {
    /// <summary>A method is being invoked.  This member is equivalent to the Visual Basic constant <see langword="vbMethod" />.</summary>
    Method = 1,
    /// <summary>A property value is being retrieved.  This member is equivalent to the Visual Basic constant <see langword="vbGet" />.</summary>
    Get = 2,
    /// <summary>An Object property value is being determined. This member is equivalent to the Visual Basic constant <see langword="vbLet" />.</summary>
    Let = 4,
    /// <summary>A property value is being determined.  This member is equivalent to the Visual Basic constant <see langword="vbSet" />.</summary>
    Set = 8,
  }
}
