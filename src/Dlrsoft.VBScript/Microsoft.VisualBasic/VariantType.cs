using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualBasic
{
	/// <summary>Indicates the type of a variant object, returned by the VarType function.</summary>
	/// <filterpriority>1</filterpriority>
	public enum VariantType
	{
		/// <summary>Null reference. This member is equivalent to the Visual Basic constant vbEmpty.</summary>
		Empty = 0,
		/// <summary>Null object. This member is equivalent to the Visual Basic constant vbNull.</summary>
		Null = 1,
		/// <summary>Short. (-32,768 through 32,767.)</summary>
		Short = 2,
		/// <summary>Integer. (-2,147,483,648 through 2,147,483,647.) This member is equivalent to the Visual Basic constant vbInteger.</summary>
		Integer = 3,
		/// <summary>Single. (-3.402823E+38 through -1.401298E-45 for negative values; 1.401298E-45 through 3.402823E+38 for positive values.) This member is equivalent to the Visual Basic constant vbSingle.</summary>
		Single = 4,
		/// <summary>Double. (-1.79769313486231E+308 through -4.94065645841247E-324 for negative values; 4.94065645841247E-324 through 1.79769313486231E+308 for positive values.) This member is equivalent to the Visual Basic constant vbDouble.</summary>
		Double = 5,
		/// <summary>Currency. This member is equivalent to the Visual Basic constant vbCurrency.</summary>
		Currency = 6,
		/// <summary>Date. (0:00:00 on January 1, 0001 through 11:59:59 PM on December 31, 9999.) This member is equivalent to the Visual Basic constant vbDate.</summary>
		Date = 7,
		/// <summary>String. (0 to approximately 2 billion Unicode characters.) This member is equivalent to the Visual Basic constant vbString.</summary>
		String = 8,
		/// <summary>Any type can be stored in a variable of type Object. This member is equivalent to the Visual Basic constant vbObject.</summary>
		Object = 9,
		/// <summary>
		///   <see cref="T:System.Exception" />
		/// </summary>
		Error = 10,
		/// <summary>Boolean. (True or False.) This member is equivalent to the Visual Basic constant vbBoolean.</summary>
		Boolean = 11,
		/// <summary>Variant. This member is equivalent to the Visual Basic constant vbVariant.</summary>
		Variant = 12,
		/// <summary>DataObject.</summary>
		DataObject = 13,
		/// <summary>Decimal. (0 through +/-79,228,162,514,264,337,593,543,950,335 with no decimal point; 0 through +/-7.9228162514264337593543950335 with 28 places to the right of the decimal; smallest non-zero number is +/-0.0000000000000000000000000001.) This member is equivalent to the Visual Basic constant vbDecimal.</summary>
		Decimal = 14,
		/// <summary>Byte. (0 through 255.) This member is equivalent to the Visual Basic constant vbByte.</summary>
		Byte = 17,
		/// <summary>Char. (0 through 65535.) This member is equivalent to the Visual Basic constant vbChar.</summary>
		Char = 18,
		/// <summary>Long. (-9,223,372,036,854,775,808 through 9,223,372,036,854,775,807.) This member is equivalent to the Visual Basic constant vbLong.</summary>
		Long = 20,
		/// <summary>User-defined type. Each member of the structure has a range determined by its data type and independent of the ranges of the other members. This member is equivalent to the Visual Basic constant vbUserDefinedType.</summary>
		UserDefinedType = 36,
		/// <summary>Array. This member is equivalent to the Visual Basic constant vbArray.</summary>
		Array = 0x2000
	}
}
