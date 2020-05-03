// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.IRecordEnum
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal interface IRecordEnum
  {
    bool Callback(FieldInfo FieldInfo, ref object Value);
  }
}
