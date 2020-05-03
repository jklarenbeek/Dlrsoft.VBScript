// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.TriState
// Assembly: Microsoft.VisualBasic, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3959C06-CCCE-4BA9-A059-76BA0F7526A8
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\Microsoft.VisualBasic\8.0.0.0__b03f5f7f11d50a3a\Microsoft.VisualBasic.dll

namespace Microsoft.VisualBasic
{
    /// <summary>Indicates a Boolean value or whether the default should be used when calling number-formatting functions.</summary>
    public enum TriState
    {
        /// <summary>Default setting. The numeric value of this member is -2.</summary>
        UseDefault = -2, // 0xFFFFFFFE
        /// <summary>True. The numeric value of this member is -1.</summary>
        True = -1, // 0xFFFFFFFF
        /// <summary>False. The numeric value of this member is 0.</summary>
        False = 0,
    }
}
