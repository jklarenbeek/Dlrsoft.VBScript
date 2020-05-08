using Microsoft.VisualBasic.CompilerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualBasic
{
	/// <summary>Indicates that a string should be treated as if it were fixed length.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class VBFixedStringAttribute : Attribute
	{
		private int m_Length;

		/// <summary>Gets the length of the string.</summary>
		/// <returns>Returns the length of the string.</returns>
		/// <filterpriority>1</filterpriority>
		public int Length => m_Length;

		/// <summary>Initializes the value of the SizeConst field.</summary>
		/// <param name="Length">The length of the fixed string.</param>
		public VBFixedStringAttribute(int Length)
		{
			if (Length < 1 || Length > 32767)
			{
				throw new ArgumentException(Utils.GetResourceString("Invalid_VBFixedString"));
			}
			m_Length = Length;
		}
	}

}
