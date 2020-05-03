// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;

namespace Microsoft.VisualBasic.Devices
{
  /// <summary>Provides data for the <see langword="My.Application.NetworkAvailabilityChanged" /> and <see langword="My.Computer.Network.NetworkAvailabilityChanged" /> events.</summary>
  public class NetworkAvailableEventArgs : EventArgs
  {
    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs" /> class.</summary>
    /// <param name="networkAvailable">A <see cref="T:System.Boolean" /> that indicates whether a network is available to the application.</param>
    public NetworkAvailableEventArgs(bool networkAvailable)
    {
      this.IsNetworkAvailable = networkAvailable;
    }

    /// <summary>Gets a value indicating whether a network is available to the application.</summary>
    /// <returns>A <see cref="T:System.Boolean" /> that indicates whether a network is available to the application.</returns>
    public bool IsNetworkAvailable { get; }
  }
}
