﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.VBMath
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using Microsoft.VisualBasic.CompilerServices;
using System;

namespace Microsoft.VisualBasic
{
  /// <summary>The <see langword="VbMath" /> module contains procedures used to perform mathematical operations.</summary>
  [StandardModule]
  public sealed class VBMath
  {
    /// <summary>Returns a random number of type <see langword="Single" />.</summary>
    /// <returns>The next random number in the sequence.</returns>
    public static float Rnd()
    {
      return VBMath.Rnd(1f);
    }

    /// <summary>Returns a random number of type <see langword="Single" />.</summary>
    /// <param name="Number">Optional. A <see langword="Single" /> value or any valid <see langword="Single" /> expression.</param>
    /// <returns>If number is less than zero, Rnd generates the same number every time, using <paramref name="Number" /> as the seed. If number is greater than zero, Rnd generates the next random number in the sequence. If number is equal to zero, Rnd generates the most recently generated number. If number is not supplied, Rnd generates the next random number in the sequence.</returns>
    public static float Rnd(float Number)
    {
      ProjectData projectData = ProjectData.GetProjectData();
      int num1 = projectData.m_rndSeed;
      if ((double) Number != 0.0)
      {
        if ((double) Number < 0.0)
        {
          long num2 = (long) BitConverter.ToInt32(BitConverter.GetBytes(Number), 0) & (long) uint.MaxValue;
          num1 = checked ((int) (num2 + (num2 >> 24) & 16777215L));
        }
        num1 = checked ((int) ((long) num1 * 1140671485L + 12820163L & 16777215L));
      }
      projectData.m_rndSeed = num1;
      return (float) num1 / 1.677722E+07f;
    }

    /// <summary>Initializes the random-number generator.</summary>
    public static void Randomize()
    {
      ProjectData projectData = ProjectData.GetProjectData();
      float timer = VBMath.GetTimer();
      int rndSeed = projectData.m_rndSeed;
      int int32 = BitConverter.ToInt32(BitConverter.GetBytes(timer), 0);
      int num = (int32 & (int) ushort.MaxValue ^ int32 >> 16) << 8;
      projectData.m_rndSeed = rndSeed & -16776961 | num;
    }

    /// <summary>Initializes the random-number generator.</summary>
    /// <param name="Number">Optional. An <see langword="Object" /> or any valid numeric expression.</param>
    public static void Randomize(double Number)
    {
      ProjectData projectData = ProjectData.GetProjectData();
      int rndSeed = projectData.m_rndSeed;
      int num1 = !BitConverter.IsLittleEndian ? BitConverter.ToInt32(BitConverter.GetBytes(Number), 0) : BitConverter.ToInt32(BitConverter.GetBytes(Number), 4);
      int num2 = (num1 & (int) ushort.MaxValue ^ num1 >> 16) << 8;
      projectData.m_rndSeed = rndSeed & -16776961 | num2;
    }

    private static float GetTimer()
    {
      DateTime now = DateTime.Now;
      return (float) checked ((60 * now.Hour + now.Minute) * 60 + now.Second) + (float) now.Millisecond / 1000f;
    }
  }
}
