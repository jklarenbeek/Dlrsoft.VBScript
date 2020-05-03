// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.VisualBasic.ApplicationServices
{
  /// <summary>Provides data for the <see langword="My.Application.UnhandledException" /> event.</summary>
  [ComVisible(false)]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  public class UnhandledExceptionEventArgs : ThreadExceptionEventArgs
  {
    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs" /> class.</summary>
    /// <param name="exitApplication">A <see cref="T:System.Boolean" /> that indicates whether the application should exit upon exiting the exception handler.</param>
    /// <param name="exception">The <see cref="T:System.Exception" /> that occurred.</param>
    public UnhandledExceptionEventArgs(bool exitApplication, Exception exception)
      : base(exception)
    {
      this.ExitApplication = exitApplication;
    }

    /// <summary>Indicates whether the application should exit upon exiting the exception handler.</summary>
    /// <returns>A <see cref="T:System.Boolean" /> that indicates whether the application should exit upon exiting the exception handler.</returns>
    public bool ExitApplication { get; set; }
  }
}
