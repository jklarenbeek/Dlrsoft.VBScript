using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualBasic.CompilerService
{
	/// <summary>Specifies that the current Option Compare setting should be passed as the default value for an argument. </summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
	public sealed class OptionCompareAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute" /> class.</summary>
		public OptionCompareAttribute()
		{
		}
	}
}
