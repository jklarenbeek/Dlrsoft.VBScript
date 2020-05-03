// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.Constants
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Diagnostics;

namespace Microsoft.VisualBasic
{
  /// <summary>The <see langword="Constants" /> module contains miscellaneous constants. These constants can be used anywhere in your code.</summary>
  [DebuggerNonUserCode]
  [StandardModule]
  public sealed class Constants
  {
    /// <summary>Represents a carriage-return character combined with a linefeed character for print and display functions.</summary>
    public const string vbCrLf = "\r\n";
    /// <summary>Represents a newline character for print and display functions.</summary>
    [Obsolete("For a carriage return and line feed, use vbCrLf.  For the current platform's newline, use System.Environment.NewLine.")]
    public const string vbNewLine = "\r\n";
    /// <summary>Represents a carriage-return character for print and display functions.</summary>
    public const string vbCr = "\r";
    /// <summary>Represents a linefeed character for print and display functions.</summary>
    public const string vbLf = "\n";
    /// <summary>Represents a backspace character for print and display functions.</summary>
    public const string vbBack = "\b";
    /// <summary>Represents a form-feed character for print functions.</summary>
    public const string vbFormFeed = "\f";
    /// <summary>Represents a tab character for print and display functions.</summary>
    public const string vbTab = "\t";
    /// <summary>Represents a carriage-return character for print functions.</summary>
    public const string vbVerticalTab = "\v";
    /// <summary>Represents a null character for print and display functions.</summary>
    public const string vbNullChar = "\0";
    /// <summary>Represents a zero-length string for print and display functions, and for calling external procedures.</summary>
    public const string vbNullString = null;
    /// <summary>Specifies that a binary comparison should be performed when comparison functions are called.</summary>
    public const CompareMethod vbBinaryCompare = CompareMethod.Binary;
    /// <summary>Indicates that a text comparison should be performed when comparison functions are called.</summary>
    public const CompareMethod vbTextCompare = CompareMethod.Text;
  }
}
