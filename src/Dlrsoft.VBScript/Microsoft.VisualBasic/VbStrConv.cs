using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualBasic
{
	/// <summary>Indicates which type of conversion to perform when calling the StrConv function.</summary>
	/// <filterpriority>1</filterpriority>
	[Flags]
	public enum VbStrConv
	{
		/// <summary>Performs no conversion.</summary>
		None = 0x0,
		/// <summary>Converts the string to uppercase characters. This member is equivalent to the Visual Basic constant vbUpperCase.</summary>
		Uppercase = 0x1,
		/// <summary>Converts the string to lowercase characters. This member is equivalent to the Visual Basic constant vbLowerCase.</summary>
		Lowercase = 0x2,
		/// <summary>Converts the first letter of every word in the string to uppercase. This member is equivalent to the Visual Basic constant vbProperCase.</summary>
		ProperCase = 0x3,
		/// <summary>Converts narrow (single-byte) characters in the string to wide (double-byte) characters. Applies to Asian locales. This member is equivalent to the Visual Basic constant vbWide.</summary>
		Wide = 0x4,
		/// <summary>Converts wide (double-byte) characters in the string to narrow (single-byte) characters. Applies to Asian locales. This member is equivalent to the Visual Basic constant vbNarrow.</summary>
		Narrow = 0x8,
		/// <summary>Converts Hiragana characters in the string to Katakana characters. Applies to Japanese locale only. This member is equivalent to the Visual Basic constant vbKatakana.</summary>
		Katakana = 0x10,
		/// <summary>Converts Katakana characters in the string to Hiragana characters. Applies to Japanese locale only. This member is equivalent to the Visual Basic constant vbHiragana.</summary>
		Hiragana = 0x20,
		/// <summary>Converts the string to Simplified Chinese characters. This member is equivalent to the Visual Basic constant vbSimplifiedChinese.</summary>
		SimplifiedChinese = 0x100,
		/// <summary>Converts the string to Traditional Chinese characters. This member is equivalent to the Visual Basic constant vbTraditionalChinese.</summary>
		TraditionalChinese = 0x200,
		/// <summary>Converts the string from file system rules for casing to linguistic rules. This member is equivalent to the Visual Basic constant vbLinguisticCasing.</summary>
		LinguisticCasing = 0x400
	}
}
