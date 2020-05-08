using Microsoft.VisualBasic.CompilerService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Microsoft.VisualBasic
{
	internal sealed class FormatInfoHolder : IFormatProvider
	{
		private NumberFormatInfo nfi;

		internal FormatInfoHolder(NumberFormatInfo nfi)
		{
			this.nfi = nfi;
		}

		private object GetFormat(Type service)
		{
			if ((object)service == typeof(NumberFormatInfo))
			{
				return nfi;
			}
			throw new ArgumentException(Utils.GetResourceString("InternalError"));
		}

		object IFormatProvider.GetFormat(Type service)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetFormat
			return this.GetFormat(service);
		}
	}
}
