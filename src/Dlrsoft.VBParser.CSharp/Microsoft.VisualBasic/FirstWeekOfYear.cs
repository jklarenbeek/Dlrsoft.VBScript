using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualBasic
{
	/// <summary>Indicates the first week of the year to use when calling date-related functions.</summary>
	/// <filterpriority>1</filterpriority>
	public enum FirstWeekOfYear
	{
		/// <summary>The day of the week specified in your system settings as the first day of the week This member is equivalent to the Visual Basic constant vbUseSystem.</summary>
		System,
		/// <summary>The week in which January 1 occurs (default) This member is equivalent to the Visual Basic constant vbFirstJan1.</summary>
		Jan1,
		/// <summary>The first week that has at least four days in the new year This member is equivalent to the Visual Basic constant vbFirstFourDays.</summary>
		FirstFourDays,
		/// <summary>The first full week of the year This member is equivalent to the Visual Basic constant vbFirstFullWeek.</summary>
		FirstFullWeek
	}
}
