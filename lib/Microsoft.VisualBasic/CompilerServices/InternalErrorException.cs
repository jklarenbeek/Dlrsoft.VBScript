// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.InternalErrorException
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>The exception thrown for internal Visual Basic compiler errors.</summary>
  internal sealed class InternalErrorException : Exception
  {
    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.VisualBasic.CompilerServices.InternalErrorException" /> class.</summary>
    public InternalErrorException()
      : base(Utils.GetResourceString(SR.InternalError_VisualBasicRuntime))
    {
    }
  }
}
