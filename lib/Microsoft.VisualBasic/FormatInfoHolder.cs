// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.FormatInfoHolder
// Assembly: Microsoft.VisualBasic, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3959C06-CCCE-4BA9-A059-76BA0F7526A8
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\Microsoft.VisualBasic\8.0.0.0__b03f5f7f11d50a3a\Microsoft.VisualBasic.dll

using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Globalization;

namespace Microsoft.VisualBasic
{
    internal sealed class FormatInfoHolder : IFormatProvider
    {
        private NumberFormatInfo nfi;

        internal FormatInfoHolder(NumberFormatInfo nfi)
        {
            this.nfi = nfi;
        }

        object IFormatProvider.GetFormat(Type service)
        {
            if (service == typeof(NumberFormatInfo))
                return (object)this.nfi;
            throw new ArgumentException(Utils.GetResourceString("InternalError"));
        }
    }
}
