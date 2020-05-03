using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualBasic
{
	public sealed class VBMath
	{
		static Random rnd = new Random();

		/// <summary>Returns a random number of type Single.</summary>
		/// <returns>The next random number in the sequence.</returns>
		/// <filterpriority>1</filterpriority>
		public static float Rnd()
		{
			return (float)rnd.NextDouble();
		}

		/// <summary>Returns a random number of type Single.</summary>
		/// <returns>If number is less than zero, Rnd generates the same number every time, using <paramref name="Number" /> as the seed. If number is greater than zero, Rnd generates the next random number in the sequence. If number is equal to zero, Rnd generates the most recently generated number. If number is not supplied, Rnd generates the next random number in the sequence.</returns>
		/// <param name="Number">Optional. A Single value or any valid Single expression.</param>
		/// <filterpriority>1</filterpriority>
		public static float Rnd(float Number)
		{
			return (float)(rnd.NextDouble() * Number);
		}

		/// <summary>Initializes the random-number generator.</summary>
		/// <filterpriority>1</filterpriority>
		public static void Randomize()
		{
			rnd = new Random();
		}

		/// <summary>Initializes the random-number generator.</summary>
		/// <param name="Number">Optional. An Object or any valid numeric expression.</param>
		/// <filterpriority>1</filterpriority>
		public static void Randomize(double Number)
		{
			rnd = new Random((int)(Number % int.MaxValue));
		}
	}

}
