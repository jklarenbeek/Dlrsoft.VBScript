using Microsoft.VisualBasic.CompilerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualBasic
{
	/// <summary>Indicates that an array in a structure or non-local variable should be treated as a fixed-length array.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class VBFixedArrayAttribute : Attribute
	{
		internal int FirstBound;

		internal int SecondBound;

		/// <summary>Returns the bounds of the array.</summary>
		/// <returns>Contains an integer array that represents the bounds of the array.</returns>
		/// <filterpriority>1</filterpriority>
		public int[] Bounds
		{
			get
			{
				if (SecondBound == -1)
				{
					return new int[1]
					{
					FirstBound
					};
				}
				return new int[2]
				{
				FirstBound,
				SecondBound
				};
			}
		}

		/// <summary>Returns the size of the array.</summary>
		/// <returns>Contains an integer that represents the number of elements in the array.</returns>
		/// <filterpriority>1</filterpriority>
		public int Length
		{
			get
			{
				checked
				{
					if (SecondBound == -1)
					{
						return FirstBound + 1;
					}
					return (FirstBound + 1) * (SecondBound + 1);
				}
			}
		}

		/// <summary>Initializes the value of the Bounds property.</summary>
		/// <param name="UpperBound1">Initializes the value of upper field, which represents the size of the first dimension of the array.</param>
		public VBFixedArrayAttribute(int UpperBound1)
		{
			if (UpperBound1 < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Invalid_VBFixedArray"));
			}
			FirstBound = UpperBound1;
			SecondBound = -1;
		}

		/// <summary>Initializes the value of the Bounds property.</summary>
		/// <param name="UpperBound1">Initializes the value of upper field, which represents the size of the first dimension of the array.</param>
		/// <param name="UpperBound2">Initializes the value of upper field, which represents the size of the second dimension of the array.</param>
		public VBFixedArrayAttribute(int UpperBound1, int UpperBound2)
		{
			if (UpperBound1 < 0 || UpperBound2 < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Invalid_VBFixedArray"));
			}
			FirstBound = UpperBound1;
			SecondBound = UpperBound2;
		}
	}
}
