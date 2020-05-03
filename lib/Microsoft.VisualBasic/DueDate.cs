// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.DueDate
// Assembly: Microsoft.VisualBasic, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3959C06-CCCE-4BA9-A059-76BA0F7526A8
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\Microsoft.VisualBasic\8.0.0.0__b03f5f7f11d50a3a\Microsoft.VisualBasic.dll

namespace Microsoft.VisualBasic
{
    /// <summary>Indicates when payments are due when calling financial methods.</summary>
    public enum DueDate
    {
        /// <summary>Falls at the end of the date interval</summary>
        EndOfPeriod,
        /// <summary>Falls at the beginning of the date interval</summary>
        BegOfPeriod,
    }
}
