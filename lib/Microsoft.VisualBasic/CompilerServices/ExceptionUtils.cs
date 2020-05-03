// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.ExceptionUtils
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.IO;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>Provides exception handling support for the Visual Basic runtime. This class is not intended to be used from your code.</summary>
  internal sealed class ExceptionUtils
  {
    internal static Exception VbMakeIllegalForException()
    {
      return ExceptionUtils.VbMakeExceptionEx(92, Utils.GetResourceString(SR.ID92));
    }

    internal static Exception VbMakeObjNotSetException()
    {
      return ExceptionUtils.VbMakeExceptionEx(91, Utils.GetResourceString(SR.ID91));
    }

    internal static Exception VbMakeException(int hr)
    {
      string sMsg = hr <= 0 || hr > (int) ushort.MaxValue ? "" : Utils.GetResourceString((vbErrors) hr);
      return ExceptionUtils.VbMakeExceptionEx(hr, sMsg);
    }

    internal static Exception VbMakeException(Exception ex, int hr)
    {
      return ex;
    }

    private static Exception VbMakeExceptionEx(int number, string sMsg)
    {
      bool VBDefinedError;
      Exception exception = ExceptionUtils.BuildException(number, sMsg, ref VBDefinedError);
      int num = VBDefinedError ? 1 : 0;
      return exception;
    }

    internal static Exception BuildException(
      int Number,
      string Description,
      ref bool VBDefinedError)
    {
      VBDefinedError = true;
      Exception exception;
      switch (Number)
      {
        case -2147467261:
          exception = (Exception) new AccessViolationException();
          break;
        case 0:
          VBDefinedError = false;
          exception = new Exception(Description);
          break;
        case 3:
        case 20:
        case 94:
        case 100:
          exception = (Exception) new InvalidOperationException(Description);
          break;
        case 5:
        case 446:
        case 448:
        case 449:
          exception = (Exception) new ArgumentException(Description);
          break;
        case 6:
          exception = (Exception) new OverflowException(Description);
          break;
        case 7:
        case 14:
          exception = (Exception) new OutOfMemoryException(Description);
          break;
        case 9:
          exception = (Exception) new IndexOutOfRangeException(Description);
          break;
        case 11:
          exception = (Exception) new DivideByZeroException(Description);
          break;
        case 13:
          exception = (Exception) new InvalidCastException(Description);
          break;
        case 28:
          exception = (Exception) new StackOverflowException(Description);
          break;
        case 48:
          exception = (Exception) new TypeLoadException(Description);
          break;
        case 52:
        case 54:
        case 55:
        case 57:
        case 58:
        case 59:
        case 61:
        case 63:
        case 67:
        case 68:
        case 70:
        case 71:
        case 74:
        case 75:
          exception = (Exception) new IOException(Description);
          break;
        case 53:
          exception = (Exception) new FileNotFoundException(Description);
          break;
        case 62:
          exception = (Exception) new EndOfStreamException(Description);
          break;
        case 76:
        case 432:
          exception = (Exception) new FileNotFoundException(Description);
          break;
        case 91:
          exception = (Exception) new NullReferenceException(Description);
          break;
        case 422:
          exception = (Exception) new MissingFieldException(Description);
          break;
        case 429:
        case 462:
          exception = new Exception(Description);
          break;
        case 438:
          exception = (Exception) new MissingMemberException(Description);
          break;
        default:
          VBDefinedError = false;
          exception = new Exception(Description);
          break;
      }
      return exception;
    }
  }
}
