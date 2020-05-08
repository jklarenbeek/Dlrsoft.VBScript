using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualBasic
{
	public enum DateFormat
	{
		/// <summary>For real numbers, displays a date and time. If the number has no fractional part, displays only a date. If the number has no integer part, displays time only. Date and time display is determined by your computer's regional settings. This member is equivalent to the Visual Basic constant vbGeneralDate.</summary>
		GeneralDate,
		/// <summary>Displays a date using the long-date format specified in your computer's regional settings. This member is equivalent to the Visual Basic constant vbLongDate.</summary>
		LongDate,
		/// <summary>Displays a date using the short-date format specified in your computer's regional settings. This member is equivalent to the Visual Basic constant vbShortDate.</summary>
		ShortDate,
		/// <summary>Displays a time using the long-time format specified in your computer's regional settings. This member is equivalent to the Visual Basic constant vbLongTime.</summary>
		LongTime,
		/// <summary>Displays a time using the short-time format specified in your computer's regional settings. This member is equivalent to the Visual Basic constant vbShortTime.</summary>
		ShortTime
	}
}
