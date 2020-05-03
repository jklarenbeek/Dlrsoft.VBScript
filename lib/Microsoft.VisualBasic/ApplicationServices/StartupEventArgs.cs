// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.ApplicationServices.StartupEventArgs
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Microsoft.VisualBasic.ApplicationServices
{
  /// <summary>Provides data for the <see langword="My.Application.Startup" /> event.</summary>
  public class StartupEventArgs : CancelEventArgs
  {
    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.VisualBasic.ApplicationServices.StartupEventArgs" /> class.</summary>
    /// <param name="args">A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> object that contains the command-line arguments of the application.</param>
    public StartupEventArgs(ReadOnlyCollection<string> args)
    {
      if (args == null)
        args = new ReadOnlyCollection<string>((IList<string>) null);
      this.CommandLine = args;
    }

    /// <summary>Gets the command-line arguments of the application.</summary>
    /// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> object that contains the command-line arguments of the application.</returns>
    public ReadOnlyCollection<string> CommandLine { get; }
  }
}
