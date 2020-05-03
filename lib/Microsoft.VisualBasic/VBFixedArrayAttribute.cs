// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.VBFixedArrayAttribute
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;

namespace Microsoft.VisualBasic
{
  /// <summary>Indicates that an array in a structure or non-local variable should be treated as a fixed-length array.</summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public sealed class VBFixedArrayAttribute : Attribute
  {
    internal int FirstBound;
    internal int SecondBound;

    /// <summary>Returns the bounds of the array.</summary>
    /// <returns>Contains an integer array that represents the bounds of the array.</returns>
    public int[] Bounds
    {
      get
      {
        int[] numArray;
        if (this.SecondBound == -1)
          numArray = new int[1]{ this.FirstBound };
        else
          numArray = new int[2]
          {
            this.FirstBound,
            this.SecondBound
          };
        return numArray;
      }
    }

    /// <summary>Returns the size of the array.</summary>
    /// <returns>Contains an integer that represents the number of elements in the array.</returns>
    public int Length
    {
      get
      {
        return this.SecondBound != -1 ? checked (this.FirstBound + 1 * this.SecondBound + 1) : checked (this.FirstBound + 1);
      }
    }

    /// <summary>Initializes the value of the <see langword="Bounds" /> property.</summary>
    /// <param name="UpperBound1">Initializes the value of upper field, which represents the size of the first dimension of the array.</param>
    public VBFixedArrayAttribute(int UpperBound1)
    {
      if (UpperBound1 < 0)
        throw new ArgumentException(SR.Invalid_VBFixedArray);
      this.FirstBound = UpperBound1;
      this.SecondBound = -1;
    }

    /// <summary>Initializes the value of the <see langword="Bounds" /> property.</summary>
    /// <param name="UpperBound1">Initializes the value of upper field, which represents the size of the first dimension of the array.</param>
    /// <param name="UpperBound2">Initializes the value of upper field, which represents the size of the second dimension of the array.</param>
    public VBFixedArrayAttribute(int UpperBound1, int UpperBound2)
    {
      if (UpperBound1 < 0 || UpperBound2 < 0)
        throw new ArgumentException(SR.Invalid_VBFixedArray);
      this.FirstBound = UpperBound1;
      this.SecondBound = UpperBound2;
    }
  }
}
