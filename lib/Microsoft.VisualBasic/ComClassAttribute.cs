// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.ComClassAttribute
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;

namespace Microsoft.VisualBasic
{
  /// <summary>The <see langword="ComClassAttribute" /> attribute instructs the compiler to add metadata that allows a class to be exposed as a COM object.</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class ComClassAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see langword="ComClassAttribute" /> class.</summary>
    public ComClassAttribute()
    {
    }

    /// <summary>Initializes a new instance of the <see langword="ComClassAttribute" /> class.</summary>
    /// <param name="_ClassID">Initializes the value of the <see langword="ClassID" /> property that is used to uniquely identify a class.</param>
    public ComClassAttribute(string _ClassID)
    {
      this.ClassID = _ClassID;
    }

    /// <summary>Initializes a new instance of the <see langword="ComClassAttribute" /> class.</summary>
    /// <param name="_ClassID">Initializes the value of the <see langword="ClassID" /> property that is used to uniquely identify a class.</param>
    /// <param name="_InterfaceID">Initializes the value of the <see langword="InterfaceID" /> property that is used to uniquely identify an interface.</param>
    public ComClassAttribute(string _ClassID, string _InterfaceID)
    {
      this.ClassID = _ClassID;
      this.InterfaceID = _InterfaceID;
    }

    /// <summary>Initializes a new instance of the <see langword="ComClassAttribute" /> class.</summary>
    /// <param name="_ClassID">Initializes the value of the <see langword="ClassID" /> property that is used to uniquely identify a class.</param>
    /// <param name="_InterfaceID">Initializes the value of the <see langword="InterfaceID" /> property that is used to uniquely identify an interface.</param>
    /// <param name="_EventId">Initializes the value of the <see langword="EventID" /> property that is used to uniquely identify an event.</param>
    public ComClassAttribute(string _ClassID, string _InterfaceID, string _EventId)
    {
      this.ClassID = _ClassID;
      this.InterfaceID = _InterfaceID;
      this.EventID = _EventId;
    }

    /// <summary>Gets a class ID used to uniquely identify a class.</summary>
    /// <returns>Read-only. A string that can be used by the compiler to uniquely identify the class when a COM object is created.</returns>
    public string ClassID { get; }

    /// <summary>Gets an interface ID used to uniquely identify an interface.</summary>
    /// <returns>Read-only. A string that can be used by the compiler to uniquely identify an interface for the class when a COM object is created.</returns>
    public string InterfaceID { get; }

    /// <summary>Gets an event ID used to uniquely identify an event.</summary>
    /// <returns>Read only. A string that can be used by the compiler to uniquely identify an event for the class when a COM object is created.</returns>
    public string EventID { get; }

    /// <summary>Indicates that the COM interface name shadows another member of the class or base class.</summary>
    /// <returns>A <see langword="Boolean" /> value that indicates that the COM interface name shadows another member of the class or base class.</returns>
    public bool InterfaceShadows { get; set; }
  }
}
