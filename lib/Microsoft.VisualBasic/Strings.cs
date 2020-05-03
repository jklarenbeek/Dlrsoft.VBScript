// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.Strings
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic
{
    /// <summary>The <see langword="Strings" /> module contains procedures used to perform string operations.</summary>
    [StandardModule]
    public sealed class Strings
    {
        internal static readonly CompareInfo m_InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;

        /// <summary>Returns an integer value representing the character code corresponding to a character.</summary>
        /// <param name="String">Required. Any valid <see langword="Char" /> or <see langword="String" /> expression. If <paramref name="String" /> is a <see langword="String" /> expression, only the first character of the string is used for input. If <paramref name="String" /> is <see langword="Nothing" /> or contains no characters, an <see cref="T:System.ArgumentException" /> error occurs.</param>
        /// <returns>The character code corresponding to a character.</returns>
        public static int Asc(char String)
        {
            int int32 = Convert.ToInt32(String);
            if (int32 < 128)
                return int32;
            try
            {
                Encoding fileIoEncoding = Utils.GetFileIOEncoding();
                char[] chars = new char[1] { String };
                if (fileIoEncoding.IsSingleByte)
                {
                    byte[] bytes = new byte[1];
                    fileIoEncoding.GetBytes(chars, 0, 1, bytes, 0);
                    return (int)bytes[0];
                }
                byte[] bytes1 = new byte[2];
                if (fileIoEncoding.GetBytes(chars, 0, 1, bytes1, 0) == 1)
                    return (int)bytes1[0];
                if (BitConverter.IsLittleEndian)
                {
                    byte num = bytes1[0];
                    bytes1[0] = bytes1[1];
                    bytes1[1] = num;
                }
                return (int)BitConverter.ToInt16(bytes1, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns an integer value representing the character code corresponding to a character.</summary>
        /// <param name="String">Required. Any valid <see langword="Char" /> or <see langword="String" /> expression. If <paramref name="String" /> is a <see langword="String" /> expression, only the first character of the string is used for input. If <paramref name="String" /> is <see langword="Nothing" /> or contains no characters, an <see cref="T:System.ArgumentException" /> error occurs.</param>
        /// <returns>The character code corresponding to a character.</returns>
        public static int Asc(string String)
        {
            switch (String)
            {
                case "":
                case null:
                    throw new ArgumentException(SR.Format(SR.Argument_LengthGTZero1, (object)nameof(String)), nameof(String));
                default:
                    return Strings.Asc(String[0]);
            }
        }

        /// <summary>Returns an integer value representing the character code corresponding to a character.</summary>
        /// <param name="String">Required. Any valid <see langword="Char" /> or <see langword="String" /> expression. If <paramref name="String" /> is a <see langword="String" /> expression, only the first character of the string is used for input. If <paramref name="String" /> is <see langword="Nothing" /> or contains no characters, an <see cref="T:System.ArgumentException" /> error occurs.</param>
        /// <returns>The character code corresponding to a character.</returns>
        public static int AscW(string String)
        {
            switch (String)
            {
                case "":
                case null:
                    throw new ArgumentException(SR.Format(SR.Argument_LengthGTZero1, (object)nameof(String)), nameof(String));
                default:
                    return (int)String[0];
            }
        }

        /// <summary>Returns an integer value representing the character code corresponding to a character.</summary>
        /// <param name="String">Required. Any valid <see langword="Char" /> or <see langword="String" /> expression. If <paramref name="String" /> is a <see langword="String" /> expression, only the first character of the string is used for input. If <paramref name="String" /> is <see langword="Nothing" /> or contains no characters, an <see cref="T:System.ArgumentException" /> error occurs.</param>
        /// <returns>The character code corresponding to a character.</returns>
        public static int AscW(char String)
        {
            return (int)String;
        }

        /// <summary>Returns the character associated with the specified character code.</summary>
        /// <param name="CharCode">Required. An <see langword="Integer" /> expression representing the code point, or character code, for the character.</param>
        /// <returns>The character associated with the specified character code.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="CharCode" /> &lt; 0 or &gt; 255 for <see langword="Chr" />.</exception>
        public static char Chr(int CharCode)
        {
            if (CharCode < (int)short.MinValue || CharCode > (int)ushort.MaxValue)
                throw new ArgumentException(SR.Format(SR.Argument_RangeTwoBytes1, (object)nameof(CharCode)), nameof(CharCode));
            if (CharCode >= 0 && CharCode <= (int)sbyte.MaxValue)
                return Convert.ToChar(CharCode);
            try
            {
                Encoding encoding = Encoding.GetEncoding(Utils.GetLocaleCodePage());
                if (encoding.IsSingleByte && (CharCode < 0 || CharCode > (int)byte.MaxValue))
                    throw ExceptionUtils.VbMakeException(5);
                char[] chars = new char[2];
                byte[] bytes = new byte[2];
                Decoder decoder = encoding.GetDecoder();
                if (CharCode >= 0 && CharCode <= (int)byte.MaxValue)
                {
                    bytes[0] = checked((byte)(CharCode & (int)byte.MaxValue));
                    decoder.GetChars(bytes, 0, 1, chars, 0);
                }
                else
                {
                    bytes[0] = checked((byte)((CharCode & 65280) >> 8));
                    bytes[1] = checked((byte)(CharCode & (int)byte.MaxValue));
                    decoder.GetChars(bytes, 0, 2, chars, 0);
                }
                return chars[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns the character associated with the specified character code.</summary>
        /// <param name="CharCode">Required. An <see langword="Integer" /> expression representing the code point, or character code, for the character.</param>
        /// <returns>The character associated with the specified character code.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="CharCode" /> &lt; -32768 or &gt; 65535 for <see langword="ChrW" />.</exception>
        public static char ChrW(int CharCode)
        {
            if (CharCode < (int)short.MinValue || CharCode > (int)ushort.MaxValue)
                throw new ArgumentException(SR.Format(SR.Argument_RangeTwoBytes1, (object)nameof(CharCode)), nameof(CharCode));
            return Convert.ToChar(CharCode & (int)ushort.MaxValue);
        }

        /// <summary>Returns a zero-based array containing a subset of a <see langword="String" /> array based on specified filter criteria.</summary>
        /// <param name="Source">Required. One-dimensional array of strings to be searched.</param>
        /// <param name="Match">Required. String to search for.</param>
        /// <param name="Include">Optional. <see langword="Boolean" /> value indicating whether to return substrings that include or exclude <paramref name="Match" />. If <paramref name="Include" /> is <see langword="True" />, the <see langword="Filter" /> function returns the subset of the array that contains <paramref name="Match" /> as a substring. If <paramref name="Include" /> is <see langword="False" />, the <see langword="Filter" /> function returns the subset of the array that does not contain <paramref name="Match" /> as a substring.</param>
        /// <param name="Compare">Optional. Numeric value indicating the kind of string comparison to use. See "Settings" for values.</param>
        /// <returns>A zero-based array containing a subset of a <see langword="String" /> array based on specified filter criteria.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Source" /> is <see langword="Nothing" /> or is not a one-dimensional array.</exception>
        public static string[] Filter(
          object[] Source,
          string Match,
          bool Include = true,
          [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
        {
            int num1 = Information.UBound((Array)Source, 1);
            string[] Source1 = new string[checked(num1 + 1)];
            try
            {
                int num2 = num1;
                int index = 0;
                while (index <= num2)
                {
                    Source1[index] = Conversions.ToString(Source[index]);
                    checked { ++index; }
                }
            }
            catch (StackOverflowException ex)
            {
                throw ex;
            }
            catch (OutOfMemoryException ex)
            {
                throw ex;
            }
            catch (ThreadAbortException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(SR.Format(SR.Argument_InvalidValueType2, (object)nameof(Source), (object)"String"), nameof(Source));
            }
            return Strings.Filter(Source1, Match, Include, Compare);
        }

        /// <summary>Returns a zero-based array containing a subset of a <see langword="String" /> array based on specified filter criteria.</summary>
        /// <param name="Source">Required. One-dimensional array of strings to be searched.</param>
        /// <param name="Match">Required. String to search for.</param>
        /// <param name="Include">Optional. <see langword="Boolean" /> value indicating whether to return substrings that include or exclude <paramref name="Match" />. If <paramref name="Include" /> is <see langword="True" />, the <see langword="Filter" /> function returns the subset of the array that contains <paramref name="Match" /> as a substring. If <paramref name="Include" /> is <see langword="False" />, the <see langword="Filter" /> function returns the subset of the array that does not contain <paramref name="Match" /> as a substring.</param>
        /// <param name="Compare">Optional. Numeric value indicating the kind of string comparison to use. See "Settings" for values.</param>
        /// <returns>A zero-based array containing a subset of a <see langword="String" /> array based on specified filter criteria.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Source" /> is <see langword="Nothing" /> or is not a one-dimensional array.</exception>
        public static string[] Filter(
          string[] Source,
          string Match,
          bool Include = true,
          [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
        {
            try
            {
                if (Source.Rank != 1)
                    throw new ArgumentException(SR.Argument_RankEQOne1, nameof(Source));
                switch (Match)
                {
                    case "":
                    case null:
                        return (string[])null;
                    default:
                        int length = Source.Length;
                        CompareInfo compareInfo = Utils.GetCultureInfo().CompareInfo;
                        CompareOptions options = CompareOptions.None;
                        if (Compare == CompareMethod.Text)
                            options = CompareOptions.IgnoreCase;
                        string[] strArray = new string[checked(length - 1 + 1)];
                        int num = checked(length - 1);
                        int index1 = 0;
                        int index2 = 0;
                        while (index1 <= num)
                        {
                            string source = Source[index1];
                            if (source != null && compareInfo.IndexOf(source, Match, options) >= 0 == Include)
                            {
                                strArray[index2] = source;
                                checked { ++index2; }
                            }
                            checked { ++index1; }
                        }
                        if (index2 == 0)
                            return new string[0];
                        return index2 == strArray.Length ? strArray : (string[])Utils.CopyArray((Array)strArray, (Array)new string[checked(index2 - 1 + 1)]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns an integer specifying the start position of the first occurrence of one string within another.</summary>
        /// <param name="String1">Required. <see langword="String" /> expression being searched.</param>
        /// <param name="String2">Required. <see langword="String" /> expression sought.</param>
        /// <param name="Compare">Optional. Specifies the type of string comparison. If <paramref name="Compare" /> is omitted, the <see langword="Option Compare" /> setting determines the type of comparison.</param>
        /// <returns>If
        /// 
        /// InStr returns
        /// 
        /// 
        ///               <paramref name="String1" /> is zero length or <see langword="Nothing" /> 0
        /// 
        /// 
        ///               <paramref name="String2" /> is zero length or <see langword="Nothing" /> The starting position for the search, which defaults to the first character position.
        /// 
        /// 
        ///               <paramref name="String2" /> is not found
        /// 
        /// 0
        /// 
        /// 
        ///               <paramref name="String2" /> is found within <paramref name="String1" /> Position where match begins</returns>
        public static int InStr(string String1, string String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
        {
            return Compare != CompareMethod.Binary ? checked(Strings.InternalInStrText(0, String1, String2) + 1) : checked(Strings.InternalInStrBinary(0, String1, String2) + 1);
        }

        /// <summary>Returns an integer specifying the start position of the first occurrence of one string within another.</summary>
        /// <param name="Start">Optional. Numeric expression that sets the starting position for each search. If omitted, search begins at the first character position. The start index is 1-based.</param>
        /// <param name="String1">Required. <see langword="String" /> expression being searched.</param>
        /// <param name="String2">Required. <see langword="String" /> expression sought.</param>
        /// <param name="Compare">Optional. Specifies the type of string comparison. If <paramref name="Compare" /> is omitted, the <see langword="Option Compare" /> setting determines the type of comparison.</param>
        /// <returns>If
        /// 
        /// InStr returns
        /// 
        /// 
        ///               <paramref name="String1" /> is zero length or <see langword="Nothing" /> 0
        /// 
        /// 
        ///               <paramref name="String2" /> is zero length or <see langword="Nothing" /><paramref name="start" /><paramref name="String2" /> is not found
        /// 
        /// 0
        /// 
        /// 
        ///               <paramref name="String2" /> is found within <paramref name="String1" /> Position where match begins
        /// 
        /// 
        ///               <paramref name="Start" /> &gt; length of <paramref name="String1" /> 0</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Start" /> &lt; 1.</exception>
        public static int InStr(int Start, string String1, string String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
        {
            if (Start < 1)
                throw new ArgumentException(SR.Format(SR.Argument_GTZero1, (object)nameof(Start)), nameof(Start));
            return Compare != CompareMethod.Binary ? checked(Strings.InternalInStrText(Start - 1, String1, String2) + 1) : checked(Strings.InternalInStrBinary(Start - 1, String1, String2) + 1);
        }

        private static int InternalInStrBinary(int StartPos, string sSrc, string sFind)
        {
            int num1 = sSrc == null ? 0 : sSrc.Length;
            int num2;
            if (StartPos > num1 || num1 == 0)
            {
                num2 = -1;
            }
            else
            {
                switch (sFind)
                {
                    case "":
                    case null:
                        num2 = StartPos;
                        break;
                    default:
                        num2 = Strings.m_InvariantCompareInfo.IndexOf(sSrc, sFind, StartPos, CompareOptions.Ordinal);
                        break;
                }
            }
            return num2;
        }

        private static int InternalInStrText(int lStartPos, string sSrc, string sFind)
        {
            int num1 = sSrc == null ? 0 : sSrc.Length;
            int num2;
            if (lStartPos > num1 || num1 == 0)
            {
                num2 = -1;
            }
            else
            {
                switch (sFind)
                {
                    case "":
                    case null:
                        num2 = lStartPos;
                        break;
                    default:
                        num2 = Utils.GetCultureInfo().CompareInfo.IndexOf(sSrc, sFind, lStartPos, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
                        break;
                }
            }
            return num2;
        }

        /// <summary>Returns the position of the first occurrence of one string within another, starting from the right side of the string.</summary>
        /// <param name="StringCheck">Required. String expression being searched.</param>
        /// <param name="StringMatch">Required. String expression being searched for.</param>
        /// <param name="Start">Optional. Numeric expression setting the one-based starting position for each search, starting from the left side of the string. If <paramref name="Start" /> is omitted then -1 is used, meaning the search begins at the last character position. Search then proceeds from right to left.</param>
        /// <param name="Compare">Optional. Numeric value indicating the kind of comparison to use when evaluating substrings. If omitted, a binary comparison is performed. See Settings for values.</param>
        /// <returns>If
        /// 
        /// InStrRev returns
        /// 
        /// 
        ///               <paramref name="StringCheck" /> is zero-length
        /// 
        /// 0
        /// 
        /// 
        ///               <paramref name="StringMatch" /> is zero-length
        /// 
        /// 
        ///               <paramref name="Start" /><paramref name="StringMatch" /> is not found
        /// 
        /// 0
        /// 
        /// 
        ///               <paramref name="StringMatch" /> is found within <paramref name="StringCheck" /> Position at which the first match is found, starting with the right side of the string.
        /// 
        /// 
        ///               <paramref name="Start" /> is greater than length of <paramref name="StringMatch" /> 0</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Start" /> = 0 or <paramref name="Start" /> &lt; -1.</exception>
        public static int InStrRev(
          string StringCheck,
          string StringMatch,
          int Start = -1,
          [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
        {
            try
            {
                if (Start == 0 || Start < -1)
                    throw new ArgumentException(SR.Format(SR.Argument_MinusOneOrGTZero1, (object)nameof(Start)), nameof(Start));
                int num = StringCheck != null ? StringCheck.Length : 0;
                if (Start == -1)
                    Start = num;
                if (Start > num || num == 0)
                    return 0;
                switch (StringMatch)
                {
                    case "":
                    case null:
                        return Start;
                    default:
                        return Compare == CompareMethod.Binary ? checked(Strings.m_InvariantCompareInfo.LastIndexOf(StringCheck, StringMatch, Start - 1, Start, CompareOptions.Ordinal) + 1) : checked(Utils.GetCultureInfo().CompareInfo.LastIndexOf(StringCheck, StringMatch, Start - 1, Start, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) + 1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(bool Expression)
        {
            return 2;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        [CLSCompliant(false)]
        public static int Len(sbyte Expression)
        {
            return 1;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(byte Expression)
        {
            return 1;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(short Expression)
        {
            return 2;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        [CLSCompliant(false)]
        public static int Len(ushort Expression)
        {
            return 2;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(int Expression)
        {
            return 4;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        [CLSCompliant(false)]
        public static int Len(uint Expression)
        {
            return 4;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(long Expression)
        {
            return 8;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        [CLSCompliant(false)]
        public static int Len(ulong Expression)
        {
            return 8;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(Decimal Expression)
        {
            return 8;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(float Expression)
        {
            return 4;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(double Expression)
        {
            return 8;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(DateTime Expression)
        {
            return 8;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(char Expression)
        {
            return 2;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(string Expression)
        {
            return Expression != null ? Expression.Length : 0;
        }

        /// <summary>Returns an integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</summary>
        /// <param name="Expression">Any valid <see langword="String" /> expression or variable name. If <paramref name="Expression" /> is of type <see langword="Object" />, the <see langword="Len" /> function returns the size as it will be written to the file by the <see langword="FilePut" /> function.</param>
        /// <returns>An integer containing either the number of characters in a string or the nominal number of bytes required to store a variable.</returns>
        public static int Len(object Expression)
        {
            if (Expression == null)
                return 0;
            if (Expression is IConvertible convertible)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return 2;
                    case TypeCode.Char:
                        return 2;
                    case TypeCode.SByte:
                        return 1;
                    case TypeCode.Byte:
                        return 1;
                    case TypeCode.Int16:
                        return 2;
                    case TypeCode.UInt16:
                        return 2;
                    case TypeCode.Int32:
                        return 4;
                    case TypeCode.UInt32:
                        return 4;
                    case TypeCode.Int64:
                        return 8;
                    case TypeCode.UInt64:
                        return 8;
                    case TypeCode.Single:
                        return 4;
                    case TypeCode.Double:
                        return 8;
                    case TypeCode.Decimal:
                        return 16;
                    case TypeCode.DateTime:
                        return 8;
                    case TypeCode.String:
                        return Expression.ToString().Length;
                }
            }
            else if (Expression is char[] chArray)
                return chArray.Length;
            if (Expression is ValueType)
                throw new NotSupportedException("You cannot use Len to get the length of a struct.");
            throw ExceptionUtils.VbMakeException(13);
        }

        /// <summary>Returns a string containing a specified number of characters from the left side of a string.</summary>
        /// <param name="str">Required. <see langword="String" /> expression from which the leftmost characters are returned.</param>
        /// <param name="Length">Required. <see langword="Integer" /> expression. Numeric expression indicating how many characters to return. If 0, a zero-length string ("") is returned. If greater than or equal to the number of characters in <paramref name="str" />, the entire string is returned.</param>
        /// <returns>A string containing a specified number of characters from the left side of a string.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Length" /> &lt; 0.</exception>
        public static string Left(string str, int Length)
        {
            if (Length < 0)
                throw new ArgumentException(SR.Format(SR.Argument_GEZero1, (object)nameof(Length)), nameof(Length));
            return Length == 0 || str == null ? "" : (Length < str.Length ? str.Substring(0, Length) : str);
        }

        /// <summary>Returns a string containing a copy of a specified string with no leading spaces (<see langword="LTrim" />), no trailing spaces (<see langword="RTrim" />), or no leading or trailing spaces (<see langword="Trim" />).</summary>
        /// <param name="str">Required. Any valid <see langword="String" /> expression.</param>
        /// <returns>A string containing a copy of a specified string with no leading spaces (<see langword="LTrim" />), no trailing spaces (<see langword="RTrim" />), or no leading or trailing spaces (<see langword="Trim" />).</returns>
        public static string LTrim(string str)
        {
            string str1;
            switch (str)
            {
                case "":
                case null:
                    str1 = "";
                    break;
                default:
                    switch (str[0])
                    {
                        case ' ':
                        case '　':
                            str1 = str.TrimStart(Utils.m_achIntlSpace);
                            break;
                        default:
                            str1 = str;
                            break;
                    }
                    break;
            }
            return str1;
        }

        /// <summary>Returns a string that contains all the characters starting from a specified position in a string.</summary>
        /// <param name="str">Required. <see langword="String" /> expression from which characters are returned.</param>
        /// <param name="Start">Required. <see langword="Integer" /> expression. Starting position of the characters to return. If <paramref name="Start" /> is greater than the number of characters in <paramref name="str" />, the <see langword="Mid" /> function returns a zero-length string (""). <paramref name="Start" /> is one-based.</param>
        /// <returns>A string that consists of all the characters starting from the specified position in the string.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Start" /> &lt;= 0 or <paramref name="Length" /> &lt; 0.</exception>
        public static string Mid(string str, int Start)
        {
            try
            {
                return str == null ? (string)null : Strings.Mid(str, Start, str.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns a string that contains a specified number of characters starting from a specified position in a string.</summary>
        /// <param name="str">Required. <see langword="String" /> expression from which characters are returned.</param>
        /// <param name="Start">Required. <see langword="Integer" /> expression. Starting position of the characters to return. If <paramref name="Start" /> is greater than the number of characters in <paramref name="str" />, the <see langword="Mid" /> function returns a zero-length string (""). <paramref name="Start" /> is one based.</param>
        /// <param name="Length">Optional. <see langword="Integer" /> expression. Number of characters to return. If omitted or if there are fewer than <paramref name="Length" /> characters in the text (including the character at position <paramref name="Start" />), all characters from the start position to the end of the string are returned.</param>
        /// <returns>A string that consists of the specified number of characters starting from the specified position in the string.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Start" /> &lt;= 0 or <paramref name="Length" /> &lt; 0.</exception>
        public static string Mid(string str, int Start, int Length)
        {
            if (Start <= 0)
                throw new ArgumentException(SR.Format(SR.Argument_GTZero1, (object)nameof(Start)), nameof(Start));
            if (Length < 0)
                throw new ArgumentException(SR.Format(SR.Argument_GEZero1, (object)nameof(Length)), nameof(Length));
            string str1;
            if (Length == 0 || str == null)
            {
                str1 = "";
            }
            else
            {
                int length = str.Length;
                str1 = Start <= length ? (checked(Start + Length) <= length ? str.Substring(checked(Start - 1), Length) : str.Substring(checked(Start - 1))) : "";
            }
            return str1;
        }

        /// <summary>Returns a string containing a specified number of characters from the right side of a string.</summary>
        /// <param name="str">Required. <see langword="String" /> expression from which the rightmost characters are returned.</param>
        /// <param name="Length">Required. <see langword="Integer" />. Numeric expression indicating how many characters to return. If 0, a zero-length string ("") is returned. If greater than or equal to the number of characters in <paramref name="str" />, the entire string is returned.</param>
        /// <returns>A string containing a specified number of characters from the right side of a string.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Length" /> &lt; 0.</exception>
        public static string Right(string str, int Length)
        {
            if (Length < 0)
                throw new ArgumentException(SR.Format(SR.Argument_GEZero1, (object)nameof(Length)), nameof(Length));
            string str1;
            if (Length == 0 || str == null)
            {
                str1 = "";
            }
            else
            {
                int length = str.Length;
                str1 = Length < length ? str.Substring(checked(length - Length), Length) : str;
            }
            return str1;
        }

        /// <summary>Returns a string containing a copy of a specified string with no leading spaces (<see langword="LTrim" />), no trailing spaces (<see langword="RTrim" />), or no leading or trailing spaces (<see langword="Trim" />).</summary>
        /// <param name="str">Required. Any valid <see langword="String" /> expression.</param>
        /// <returns>A string containing a copy of a specified string with no leading spaces (<see langword="LTrim" />), no trailing spaces (<see langword="RTrim" />), or no leading or trailing spaces (<see langword="Trim" />).</returns>
        public static string RTrim(string str)
        {
            try
            {
                switch (str)
                {
                    case "":
                    case null:
                        return "";
                    default:
                        switch (str[checked(str.Length - 1)])
                        {
                            case ' ':
                            case '　':
                                return str.TrimEnd(Utils.m_achIntlSpace);
                            default:
                                return str;
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns a string containing a copy of a specified string with no leading spaces (<see langword="LTrim" />), no trailing spaces (<see langword="RTrim" />), or no leading or trailing spaces (<see langword="Trim" />).</summary>
        /// <param name="str">Required. Any valid <see langword="String" /> expression.</param>
        /// <returns>A string containing a copy of a specified string with no leading spaces (<see langword="LTrim" />), no trailing spaces (<see langword="RTrim" />), or no leading or trailing spaces (<see langword="Trim" />).</returns>
        public static string Trim(string str)
        {
            try
            {
                switch (str)
                {
                    case "":
                    case null:
                        return "";
                    default:
                        switch (str[0])
                        {
                            case ' ':
                            case '　':
                                return str.Trim(Utils.m_achIntlSpace);
                            default:
                                switch (str[checked(str.Length - 1)])
                                {
                                    case ' ':
                                    case '　':
                                        return str.Trim(Utils.m_achIntlSpace);
                                    default:
                                        return str;
                                }
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ////////////////////////////////////////////////////////
        //                                                    //
        //     Microsoft Dropped The Ball, we picked it up    //
        //                                                    //
        ////////////////////////////////////////////////////////

        /// <summary>Returns a string in which a specified substring has been replaced with another substring a specified number of times.</summary>
        /// <param name="Expression">Required. String expression containing substring to replace.</param>
        /// <param name="Find">Required. Substring being searched for.</param>
        /// <param name="Replacement">Required. Replacement substring.</param>
        /// <param name="Start">Optional. Position within <paramref name="Expression" /> that starts a substring used for replacement. The return value of <see langword="Replace" /> is a string that begins at <paramref name="Start" />, with appropriate substitutions. If omitted, 1 is assumed.</param>
        /// <param name="Count">Optional. Number of substring substitutions to perform. If omitted, the default value is -1, which means "make all possible substitutions."</param>
        /// <param name="Compare">Optional. Numeric value indicating the kind of comparison to use when evaluating substrings. See Settings for values.</param>
        /// <returns>
        ///         <see langword="Replace" /> returns the following values.
        ///  If
        /// 
        ///  Replace returns
        /// 
        /// <paramref name="Find" /> is zero-length or <see langword="Nothing" /> Copy of <paramref name="Expression" /><paramref name="Replace" /> is zero-length
        /// 
        ///  Copy of <paramref name="Expression" /> with no occurrences of <paramref name="Find" /><paramref name="Expression" /> is zero-length or <see langword="Nothing" />, or <paramref name="Start" /> is greater than length of <paramref name="Expression" /><see langword="Nothing" /><paramref name="Count" /> is 0
        /// 
        ///  Copy of <paramref name="Expression" /></returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Count" /> &lt; -1 or <paramref name="Start" /> &lt;= 0.</exception>
        public static string Replace(string Expression, string Find, string Replacement, int Start = 1, int Count = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
        {
            try
            {
                if (Count < -1)
                    throw new ArgumentException(Utils.GetResourceString("Argument_GEMinusOne1", nameof(Count)));
                if (Start <= 0)
                    throw new ArgumentException(Utils.GetResourceString("Argument_GTZero1", nameof(Start)));
                if (Expression == null || Start > Expression.Length)
                    return (string)null;
                if (Start != 1)
                    Expression = Expression.Substring(checked(Start - 1));
                switch (Find)
                {
                    case "":
                    case null:
                    label_10:
                        return Expression;
                    default:
                        switch (Count)
                        {
                            case -1:
                                Count = Expression.Length;
                                break;
                            case 0:
                                goto label_10;
                        }
                        return Strings.ReplaceInternal(Expression, Find, Replacement, Count, Compare);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string ReplaceInternal(string Expression, string Find, string Replacement, int Count, CompareMethod Compare)
        {
            int length1 = Expression.Length;
            int length2 = Find.Length;
            StringBuilder stringBuilder = new StringBuilder(length1);
            CompareInfo compareInfo;
            CompareOptions options;
            if (Compare == CompareMethod.Text)
            {
                compareInfo = Utils.GetCultureInfo().CompareInfo;
                options = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
            }
            else
            {
                compareInfo = Strings.m_InvariantCompareInfo;
                options = CompareOptions.Ordinal;
            }
            int startIndex = 0;
            int num1;
            int num2 = 0;
            for (; startIndex < length1; startIndex = checked(num1 + length2))
            {
                if (num2 == Count)
                {
                    stringBuilder.Append(Expression.Substring(startIndex));
                    break;
                }
                num1 = compareInfo.IndexOf(Expression, Find, startIndex, options);
                if (num1 < 0)
                {
                    stringBuilder.Append(Expression.Substring(startIndex));
                    break;
                }
                stringBuilder.Append(Expression.Substring(startIndex, checked(num1 - startIndex)));
                stringBuilder.Append(Replacement);
                checked { ++num2; }
            }
            return stringBuilder.ToString();
        }

        /// <summary>Returns a string created by joining a number of substrings contained in an array.</summary>
        /// <param name="SourceArray">Required. One-dimensional array containing substrings to be joined.</param>
        /// <param name="Delimiter">Optional. Any string, used to separate the substrings in the returned string. If omitted, the space character (" ") is used. If <paramref name="Delimiter" /> is a zero-length string ("") or <see langword="Nothing" />, all items in the list are concatenated with no delimiters.</param>
        /// <returns>A string created by joining a number of substrings contained in an array.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="SourceArray" /> is not one dimensional.</exception>
        public static string Join(object[] SourceArray, string Delimiter = " ")
        {
            int num1 = Information.UBound((Array)SourceArray, 1);
            string[] SourceArray1 = new string[checked(num1 + 1)];
            try
            {
                int num2 = num1;
                int index = 0;
                while (index <= num2)
                {
                    SourceArray1[index] = Conversions.ToString(SourceArray[index]);
                    checked { ++index; }
                }
            }
            catch (StackOverflowException ex)
            {
                throw ex;
            }
            catch (OutOfMemoryException ex)
            {
                throw ex;
            }
            catch (ThreadAbortException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", nameof(SourceArray), "String"));
            }
            return Strings.Join(SourceArray1, Delimiter);
        }

        /// <summary>Returns a string created by joining a number of substrings contained in an array.</summary>
        /// <param name="SourceArray">Required. One-dimensional array containing substrings to be joined.</param>
        /// <param name="Delimiter">Optional. Any string, used to separate the substrings in the returned string. If omitted, the space character (" ") is used. If <paramref name="Delimiter" /> is a zero-length string ("") or <see langword="Nothing" />, all items in the list are concatenated with no delimiters.</param>
        /// <returns>A string created by joining a number of substrings contained in an array.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="SourceArray" /> is not one dimensional.</exception>
        public static string Join(string[] SourceArray, string Delimiter = " ")
        {
            try
            {
                if (Strings.IsArrayEmpty((Array)SourceArray))
                    return (string)null;
                if (SourceArray.Rank != 1)
                    throw new ArgumentException(Utils.GetResourceString("Argument_RankEQOne1"));
                return string.Join(Delimiter, SourceArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool IsArrayEmpty(Array array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>Returns -1, 0, or 1, based on the result of a string comparison.</summary>
        /// <param name="String1">Required. Any valid <see langword="String" /> expression.</param>
        /// <param name="String2">Required. Any valid <see langword="String" /> expression.</param>
        /// <param name="Compare">Optional. Specifies the type of string comparison. If <paramref name="Compare" /> is omitted, the <see langword="Option Compare" /> setting determines the type of comparison.</param>
        /// <returns>The <see langword="StrComp" /> function has the following return values.
        ///  If
        /// 
        ///  StrComp returns
        /// 
        /// <paramref name="String1" /> sorts ahead of <paramref name="String2" /> -1
        /// 
        /// <paramref name="String1" /> is equal to <paramref name="String2" /> 0
        /// 
        /// <paramref name="String1" /> sorts after <paramref name="String2" /> 1</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Compare" /> value is not valid.</exception>
        public static int StrComp(string String1, string String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
        {
            try
            {
                if (Compare == CompareMethod.Binary)
                    return Operators.CompareString(String1, String2, false);
                if (Compare == CompareMethod.Text)
                    return Operators.CompareString(String1, String2, true);
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", nameof(Compare)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns a string or object consisting of the specified character repeated the specified number of times.</summary>
        /// <param name="Number">Required. <see langword="Integer" /> expression. The length to the string to be returned.</param>
        /// <param name="Character">Required. Any valid <see langword="Char" />, <see langword="String" />, or <see langword="Object" /> expression. Only the first character of the expression will be used. If Character is of type <see langword="Object" />, it must contain either a <see langword="Char" /> or a <see langword="String" /> value.</param>
        /// <returns>A string or object consisting of the specified character repeated the specified number of times.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Number" /> is less than 0 or <paramref name="Character" /> type is not valid.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="Character" /> is <see langword="Nothing" />.</exception>
        public static object StrDup(int Number, object Character)
        {
            if (Number < 0)
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", nameof(Number)));
            if (Character == null)
                throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", nameof(Character)));
            char c;
            if (Character is string str)
            {
                if (str.Length == 0)
                    throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", nameof(Character)));
                c = str[0];
            }
            else
            {
                try
                {
                    c = Conversions.ToChar(Character);
                }
                catch (StackOverflowException ex)
                {
                    throw ex;
                }
                catch (OutOfMemoryException ex)
                {
                    throw ex;
                }
                catch (ThreadAbortException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", nameof(Character)));
                }
            }
            return (object)new string(c, Number);
        }

        /// <summary>Returns a string or object consisting of the specified character repeated the specified number of times.</summary>
        /// <param name="Number">Required. <see langword="Integer" /> expression. The length to the string to be returned.</param>
        /// <param name="Character">Required. Any valid <see langword="Char" />, <see langword="String" />, or <see langword="Object" /> expression. Only the first character of the expression will be used. If Character is of type <see langword="Object" />, it must contain either a <see langword="Char" /> or a <see langword="String" /> value.</param>
        /// <returns>A string or object consisting of the specified character repeated the specified number of times.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Number" /> is less than 0 or <paramref name="Character" /> type is not valid.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="Character" /> is <see langword="Nothing" />.</exception>
        public static string StrDup(int Number, char Character)
        {
            if (Number < 0)
                throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", nameof(Number)));
            return new string(Character, Number);
        }

        /// <summary>Returns a string or object consisting of the specified character repeated the specified number of times.</summary>
        /// <param name="Number">Required. <see langword="Integer" /> expression. The length to the string to be returned.</param>
        /// <param name="Character">Required. Any valid <see langword="Char" />, <see langword="String" />, or <see langword="Object" /> expression. Only the first character of the expression will be used. If Character is of type <see langword="Object" />, it must contain either a <see langword="Char" /> or a <see langword="String" /> value.</param>
        /// <returns>A string or object consisting of the specified character repeated the specified number of times.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Number" /> is less than 0 or <paramref name="Character" /> type is not valid.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="Character" /> is <see langword="Nothing" />.</exception>
        public static string StrDup(int Number, string Character)
        {
            if (Number < 0)
                throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", nameof(Number)));
            switch (Character)
            {
                case "":
                case null:
                    throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", nameof(Character)));
                default:
                    return new string(Character[0], Number);
            }
        }

        /// <summary>Returns a string in which the character order of a specified string is reversed.</summary>
        /// <param name="Expression">Required. String expression whose characters are to be reversed. If <paramref name="Expression" /> is a zero-length string (""), a zero-length string is returned.</param>
        /// <returns>A string in which the character order of a specified string is reversed.</returns>
        public static string StrReverse(string Expression)
        {
            if (Expression == null)
                return "";
            int length = Expression.Length;
            if (length == 0)
                return "";
            int num = checked(length - 1);
            int SrcIndex = 0;
            while (SrcIndex <= num)
            {
                switch (char.GetUnicodeCategory(Expression[SrcIndex]))
                {
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.EnclosingMark:
                    case UnicodeCategory.Surrogate:
                        return Strings.InternalStrReverse(Expression, SrcIndex, length);
                    default:
                        checked { ++SrcIndex; }
                        continue;
                }
            }
            char[] charArray = Expression.ToCharArray();
            Array.Reverse((Array)charArray);
            return new string(charArray);
        }

        private static string InternalStrReverse(string Expression, int SrcIndex, int Length)
        {
            StringBuilder stringBuilder = new StringBuilder(Length);
            stringBuilder.Length = Length;
            TextElementEnumerator elementEnumerator = StringInfo.GetTextElementEnumerator(Expression, SrcIndex);
            if (!elementEnumerator.MoveNext())
                return "";
            int index1 = 0;
            int index2 = checked(Length - 1);
            while (index1 < SrcIndex)
            {
                stringBuilder[index2] = Expression[index1];
                checked { --index2; }
                checked { ++index1; }
            }
            int num = elementEnumerator.ElementIndex;
            while (index2 >= 0)
            {
                SrcIndex = num;
                num = !elementEnumerator.MoveNext() ? Length : elementEnumerator.ElementIndex;
                int index3 = checked(num - 1);
                while (index3 >= SrcIndex)
                {
                    stringBuilder[index2] = Expression[index3];
                    checked { --index2; }
                    checked { --index3; }
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>Returns a string consisting of the specified number of spaces.</summary>
        /// <param name="Number">Required. <see langword="Integer" /> expression. The number of spaces you want in the string.</param>
        /// <returns>A string consisting of the specified number of spaces.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="Number" /> &lt; 0.</exception>
        public static string Space(int Number)
        {
            if (Number >= 0)
                return new string(' ', Number);
            throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", nameof(Number)));
        }

        /// <summary>Returns a zero-based, one-dimensional array containing a specified number of substrings.</summary>
        /// <param name="Expression">Required. <see langword="String" /> expression containing substrings and delimiters.</param>
        /// <param name="Delimiter">Optional. Any single character used to identify substring limits. If <paramref name="Delimiter" /> is omitted, the space character (" ") is assumed to be the delimiter.</param>
        /// <param name="Limit">Optional. Maximum number of substrings into which the input string should be split. The default, -1, indicates that the input string should be split at every occurrence of the <paramref name="Delimiter" /> string.</param>
        /// <param name="Compare">Optional. Numeric value indicating the comparison to use when evaluating substrings. See "Settings" for values.</param>
        /// <returns>
        /// <see langword="String" /> array. If <paramref name="Expression" /> is a zero-length string (""), <see langword="Split" /> returns a single-element array containing a zero-length string. If <paramref name="Delimiter" /> is a zero-length string, or if it does not appear anywhere in <paramref name="Expression" />, <see langword="Split" /> returns a single-element array containing the entire <paramref name="Expression" /> string.</returns>
        public static string[] Split(string Expression, string Delimiter = " ", int Limit = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
        {
            try
            {
                switch (Expression)
                {
                    case "":
                    case null:
                        return new string[1] { "" };
                    default:
                        if (Limit == -1)
                            Limit = checked(Expression.Length + 1);
                        if ((Delimiter != null ? Delimiter.Length : 0) != 0)
                            return Strings.SplitHelper(Expression, Delimiter, Limit, (int)Compare);
                        return new string[1] { Expression };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string[] SplitHelper(string sSrc, string sFind, int cMaxSubStrings, int Compare)
        {
            int num1 = sFind != null ? sFind.Length : 0;
            int num2 = sSrc != null ? sSrc.Length : 0;
            if (num1 == 0)
                return new string[1] { sSrc };
            if (num2 == 0)
                return new string[1] { sSrc };
            int num3 = 20;
            if (num3 > cMaxSubStrings)
                num3 = cMaxSubStrings;
            string[] strArray = new string[checked(num3 + 1)];
            CompareOptions options;
            CompareInfo compareInfo;
            if (Compare == 0)
            {
                options = CompareOptions.Ordinal;
                compareInfo = Strings.m_InvariantCompareInfo;
            }
            else
            {
                compareInfo = Utils.GetCultureInfo().CompareInfo;
                options = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
            }
            int startIndex = 0;
            int index = 0;
            while (startIndex < num2)
            {
                int num4 = compareInfo.IndexOf(sSrc, sFind, startIndex, checked(num2 - startIndex), options);
                if (num4 == -1 || checked(index + 1) == cMaxSubStrings)
                {
                    string str = sSrc.Substring(startIndex) ?? "";
                    strArray[index] = str;
                    break;
                }
                string str1 = sSrc.Substring(startIndex, checked(num4 - startIndex)) ?? "";
                strArray[index] = str1;
                startIndex = checked(num4 + num1);
                checked { ++index; }
                if (index > num3)
                {
                    checked { num3 += 20; }
                    if (num3 > cMaxSubStrings)
                        num3 = checked(cMaxSubStrings + 1);
                    strArray = (string[])Utils.CopyArray((Array)strArray, (Array)new string[checked(num3 + 1)]);
                }
                strArray[index] = "";
                if (index == cMaxSubStrings)
                {
                    string str2 = sSrc.Substring(startIndex) ?? "";
                    strArray[index] = str2;
                    break;
                }
            }
            return checked(index + 1) == strArray.Length ? strArray : (string[])Utils.CopyArray((Array)strArray, (Array)new string[checked(index + 1)]);
        }

        /// <summary>Returns a string or character converted to lowercase.</summary>
        /// <param name="Value">Required. Any valid <see langword="String" /> or <see langword="Char" /> expression.</param>
        /// <returns>A string or character converted to lowercase.</returns>
        public static string LCase(string Value)
        {
            try
            {
                return Value == null ? (string)null : Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns a string or character converted to lowercase.</summary>
        /// <param name="Value">Required. Any valid <see langword="String" /> or <see langword="Char" /> expression.</param>
        /// <returns>A string or character converted to lowercase.</returns>
        public static char LCase(char Value)
        {
            try
            {
                return Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns a string or character containing the specified string converted to uppercase.</summary>
        /// <param name="Value">Required. Any valid <see langword="String" /> or <see langword="Char" /> expression.</param>
        /// <returns>A string or character containing the specified string converted to uppercase.</returns>
        public static string UCase(string Value)
        {
            try
            {
                return Value == null ? "" : Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns a string or character containing the specified string converted to uppercase.</summary>
        /// <param name="Value">Required. Any valid <see langword="String" /> or <see langword="Char" /> expression.</param>
        /// <returns>A string or character containing the specified string converted to uppercase.</returns>
        public static char UCase(char Value)
        {
            try
            {
                return Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns an expression formatted as a currency value using the currency symbol defined in the system control panel.</summary>
        /// <param name="Expression">Required. Expression to be formatted.</param>
        /// <param name="NumDigitsAfterDecimal">Optional. Numeric value indicating how many places are displayed to the right of the decimal. Default value is -1, which indicates that the computer's regional settings are used.</param>
        /// <param name="IncludeLeadingDigit">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> enumeration that indicates whether or not a leading zero is displayed for fractional values. See "Remarks" for values.</param>
        /// <param name="UseParensForNegativeNumbers">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> enumeration that indicates whether or not to place negative values within parentheses. See "Remarks" for values.</param>
        /// <param name="GroupDigits">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> enumeration that indicates whether or not numbers are grouped using the group delimiter specified in the computer's regional settings. See "Remarks" for values.</param>
        /// <returns>An expression formatted as a currency value using the currency symbol defined in the system control panel.</returns>
        /// <exception cref="T:System.ArgumentException">Number of digits after decimal point is greater than 99.</exception>
        /// <exception cref="T:System.InvalidCastException">Type is not numeric.</exception>
        public static string FormatCurrency(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
        {
            IFormatProvider formatProvider = (IFormatProvider)null;
            try
            {
                Strings.ValidateTriState(IncludeLeadingDigit);
                Strings.ValidateTriState(UseParensForNegativeNumbers);
                Strings.ValidateTriState(GroupDigits);
                if (NumDigitsAfterDecimal > 99)
                    throw new ArgumentException(Utils.GetResourceString("Argument_Range0to99_1", nameof(NumDigitsAfterDecimal)));
                if (Expression == null)
                    return "";
                Type type = Expression.GetType();
                if (type == typeof(string))
                    Expression = (object)Conversions.ToDouble(Expression);
                else if (!Symbols.IsNumericType(type))
                    throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(type), "Currency"));
                IFormattable formattable = (IFormattable)Expression;
                if (IncludeLeadingDigit == TriState.False)
                {
                    double num = Conversions.ToDouble(Expression);
                    if (num >= 1.0 || num <= -1.0)
                        IncludeLeadingDigit = TriState.True;
                }
                string currencyFormatString = Strings.GetCurrencyFormatString(IncludeLeadingDigit, NumDigitsAfterDecimal, UseParensForNegativeNumbers, GroupDigits, ref formatProvider);
                return formattable.ToString(currencyFormatString, formatProvider);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns a string expression representing a date/time value.</summary>
        /// <param name="Expression">Required. <see langword="Date" /> expression to be formatted.</param>
        /// <param name="NamedFormat">Optional. Numeric value that indicates the date/time format used. If omitted, <see langword="DateFormat.GeneralDate" /> is used.</param>
        /// <returns>A string expression representing a date/time value.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="NamedFormat" /> setting is not valid.</exception>
        public static string FormatDateTime(DateTime Expression, DateFormat NamedFormat = DateFormat.GeneralDate)
        {
            try
            {
                string format;
                switch (NamedFormat)
                {
                    case DateFormat.GeneralDate:
                        format = Expression.TimeOfDay.Ticks != Expression.Ticks ? (Expression.TimeOfDay.Ticks != 0L ? "G" : "d") : "T";
                        break;
                    case DateFormat.LongDate:
                        format = "D";
                        break;
                    case DateFormat.ShortDate:
                        format = "d";
                        break;
                    case DateFormat.LongTime:
                        format = "T";
                        break;
                    case DateFormat.ShortTime:
                        format = "HH:mm";
                        break;
                    default:
                        throw ExceptionUtils.VbMakeException(5);
                }
                return Expression.ToString(format, (IFormatProvider)null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns an expression formatted as a number.</summary>
        /// <param name="Expression">Required. Expression to be formatted.</param>
        /// <param name="NumDigitsAfterDecimal">Optional. Numeric value indicating how many places are displayed to the right of the decimal. The default value is -1, which indicates that the computer's regional settings are used.</param>
        /// <param name="IncludeLeadingDigit">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether a leading 0 is displayed for fractional values. See "Settings" for values.</param>
        /// <param name="UseParensForNegativeNumbers">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether to place negative values within parentheses. See "Settings" for values.</param>
        /// <param name="GroupDigits">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether or not numbers are grouped using the group delimiter specified in the locale settings. See "Settings" for values.</param>
        /// <returns>An expression formatted as a number.</returns>
        /// <exception cref="T:System.InvalidCastException">Type is not numeric.</exception>
        public static string FormatNumber(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
        {
            try
            {
                Strings.ValidateTriState(IncludeLeadingDigit);
                Strings.ValidateTriState(UseParensForNegativeNumbers);
                Strings.ValidateTriState(GroupDigits);
                if (Expression == null)
                    return "";
                Type type = Expression.GetType();
                if (type == typeof(string))
                    Expression = (object)Conversions.ToDouble(Expression);
                else if (type == typeof(bool))
                    Expression = !Conversions.ToBoolean(Expression) ? (object)0.0 : (object)-1.0;
                else if (!Symbols.IsNumericType(type))
                    throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(type), "Currency"));
                return ((IFormattable)Expression).ToString(Strings.GetNumberFormatString(NumDigitsAfterDecimal, IncludeLeadingDigit, UseParensForNegativeNumbers, GroupDigits), (IFormatProvider)null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Returns an expression formatted as a percentage (that is, multiplied by 100) with a trailing % character.</summary>
        /// <param name="Expression">Required. Expression to be formatted.</param>
        /// <param name="NumDigitsAfterDecimal">Optional. Numeric value indicating how many places to the right of the decimal are displayed. Default value is -1, which indicates that the locale settings are used.</param>
        /// <param name="IncludeLeadingDigit">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether or not a leading zero displays for fractional values. See "Settings" for values.</param>
        /// <param name="UseParensForNegativeNumbers">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether or not to place negative values within parentheses. See "Settings" for values.</param>
        /// <param name="GroupDigits">Optional. <see cref="T:Microsoft.VisualBasic.TriState" /> constant that indicates whether or not numbers are grouped using the group delimiter specified in the locale settings. See "Settings" for values.</param>
        /// <returns>An expression formatted as a percentage (that is, multiplied by 100) with a trailing % character.</returns>
        /// <exception cref="T:System.InvalidCastException">Type is not numeric.</exception>
        public static string FormatPercent(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
        {
            Strings.ValidateTriState(IncludeLeadingDigit);
            Strings.ValidateTriState(UseParensForNegativeNumbers);
            Strings.ValidateTriState(GroupDigits);
            if (Expression == null)
                return "";
            Type type = Expression.GetType();
            if (type == typeof(string))
                Expression = (object)Conversions.ToDouble(Expression);
            else if (!Symbols.IsNumericType(type))
                throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", Utils.VBFriendlyName(type), "numeric"));
            return ((IFormattable)Expression).ToString(Strings.GetFormatString(NumDigitsAfterDecimal, IncludeLeadingDigit, UseParensForNegativeNumbers, GroupDigits, Strings.FormatType.Percent), (IFormatProvider)null);
        }

        internal static string GetFormatString(int NumDigitsAfterDecimal, TriState IncludeLeadingDigit, TriState UseParensForNegativeNumbers, TriState GroupDigits, Strings.FormatType FormatTypeValue)
        {
            StringBuilder stringBuilder = new StringBuilder(30);
            NumberFormatInfo format = (NumberFormatInfo)Utils.GetCultureInfo().GetFormat(typeof(NumberFormatInfo));
            if (NumDigitsAfterDecimal < -1)
                throw ExceptionUtils.VbMakeException(5);
            if (NumDigitsAfterDecimal == -1)
            {
                switch (FormatTypeValue)
                {
                    case Strings.FormatType.Number:
                        NumDigitsAfterDecimal = format.NumberDecimalDigits;
                        break;
                    case Strings.FormatType.Percent:
                        NumDigitsAfterDecimal = format.NumberDecimalDigits;
                        break;
                    case Strings.FormatType.Currency:
                        NumDigitsAfterDecimal = format.CurrencyDecimalDigits;
                        break;
                }
            }
            if (GroupDigits == TriState.UseDefault)
            {
                GroupDigits = TriState.True;
                switch (FormatTypeValue)
                {
                    case Strings.FormatType.Number:
                        if (Strings.IsArrayEmpty((Array)format.NumberGroupSizes))
                        {
                            GroupDigits = TriState.False;
                            break;
                        }
                        break;
                    case Strings.FormatType.Percent:
                        if (Strings.IsArrayEmpty((Array)format.PercentGroupSizes))
                        {
                            GroupDigits = TriState.False;
                            break;
                        }
                        break;
                    case Strings.FormatType.Currency:
                        if (Strings.IsArrayEmpty((Array)format.CurrencyGroupSizes))
                        {
                            GroupDigits = TriState.False;
                            break;
                        }
                        break;
                }
            }
            if (UseParensForNegativeNumbers == TriState.UseDefault)
            {
                UseParensForNegativeNumbers = TriState.False;
                switch (FormatTypeValue)
                {
                    case Strings.FormatType.Number:
                        if (format.NumberNegativePattern == 0)
                        {
                            UseParensForNegativeNumbers = TriState.True;
                            break;
                        }
                        break;
                    case Strings.FormatType.Currency:
                        if (format.CurrencyNegativePattern == 0)
                        {
                            UseParensForNegativeNumbers = TriState.True;
                            break;
                        }
                        break;
                }
            }
            string str1 = GroupDigits != TriState.True ? "" : "#,##";
            string str2 = IncludeLeadingDigit == TriState.False ? "#" : "0";
            string str3 = NumDigitsAfterDecimal <= 0 ? "" : "." + new string('0', NumDigitsAfterDecimal);
            if (FormatTypeValue == Strings.FormatType.Currency)
                stringBuilder.Append(format.CurrencySymbol);
            stringBuilder.Append(str1);
            stringBuilder.Append(str2);
            stringBuilder.Append(str3);
            if (FormatTypeValue == Strings.FormatType.Percent)
                stringBuilder.Append(format.PercentSymbol);
            if (UseParensForNegativeNumbers == TriState.True)
            {
                string str4 = stringBuilder.ToString();
                stringBuilder.Append(";(");
                stringBuilder.Append(str4);
                stringBuilder.Append(")");
            }
            return stringBuilder.ToString();
        }

        internal static string GetNumberFormatString(int NumDigitsAfterDecimal, TriState IncludeLeadingDigit, TriState UseParensForNegativeNumbers, TriState GroupDigits)
        {
            NumberFormatInfo format = (NumberFormatInfo)Utils.GetCultureInfo().GetFormat(typeof(NumberFormatInfo));
            if (NumDigitsAfterDecimal == -1)
                NumDigitsAfterDecimal = format.NumberDecimalDigits;
            else if (NumDigitsAfterDecimal > 99 || NumDigitsAfterDecimal < -1)
                throw new ArgumentException(Utils.GetResourceString("Argument_Range0to99_1", nameof(NumDigitsAfterDecimal)));
            if (GroupDigits == TriState.UseDefault)
                GroupDigits = format.NumberGroupSizes == null || format.NumberGroupSizes.Length == 0 ? TriState.False : TriState.True;
            int index1 = format.NumberNegativePattern;
            switch (UseParensForNegativeNumbers)
            {
                case TriState.UseDefault:
                    UseParensForNegativeNumbers = index1 != 0 ? TriState.False : TriState.True;
                    break;
                case TriState.False:
                    if (index1 == 0)
                    {
                        index1 = 1;
                        break;
                    }
                    break;
                default:
                    UseParensForNegativeNumbers = TriState.True;
                    switch (index1)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            index1 = 0;
                            break;
                    }
                    break;
            }
            if (UseParensForNegativeNumbers == TriState.UseDefault)
                UseParensForNegativeNumbers = TriState.True;
            string Expression = "n;" + Strings.NumberNegativeFormatStrings[index1];
            if (string.CompareOrdinal("-", format.NegativeSign) != 0)
                Expression = Expression.Replace("-", "\"" + format.NegativeSign + "\"");
            string Replacement = IncludeLeadingDigit == TriState.False ? "#" : "0";
            if (GroupDigits != TriState.False && format.NumberGroupSizes.Length != 0)
            {
                if (format.NumberGroupSizes.Length == 1)
                {
                    Replacement = "#," + new string('#', format.NumberGroupSizes[0]) + Replacement;
                }
                else
                {
                    Replacement = new string('#', checked(format.NumberGroupSizes[0] - 1)) + Replacement;
                    int upperBound = format.NumberGroupSizes.GetUpperBound(0);
                    int index2 = 1;
                    while (index2 <= upperBound)
                    {
                        Replacement = "," + new string('#', format.NumberGroupSizes[index2]) + "," + Replacement;
                        checked { ++index2; }
                    }
                }
            }
            if (NumDigitsAfterDecimal > 0)
                Replacement = Replacement + "." + new string('0', NumDigitsAfterDecimal);
            return Strings.Replace(Expression, "n", Replacement, 1, -1, CompareMethod.Binary);
        }

        private static void ValidateTriState(TriState Param)
        {
            if (Param != TriState.True && Param != TriState.False && Param != TriState.UseDefault)
                throw ExceptionUtils.VbMakeException(5);
        }

        internal static string GetCurrencyFormatString(TriState IncludeLeadingDigit, int NumDigitsAfterDecimal, TriState UseParensForNegativeNumbers, TriState GroupDigits, ref IFormatProvider formatProvider)
        {
            string str1 = "C";
            NumberFormatInfo nfi = (NumberFormatInfo)((NumberFormatInfo)Utils.GetCultureInfo().GetFormat(typeof(NumberFormatInfo))).Clone();
            if (GroupDigits == TriState.False)
                nfi.CurrencyGroupSizes = new int[1] { 0 };
            int currencyPositivePattern = nfi.CurrencyPositivePattern;
            int index = nfi.CurrencyNegativePattern;
            switch (UseParensForNegativeNumbers)
            {
                case TriState.UseDefault:
                    switch (index)
                    {
                        case 0:
                        case 4:
                        case 14:
                        case 15:
                            UseParensForNegativeNumbers = TriState.True;
                            break;
                        default:
                            UseParensForNegativeNumbers = TriState.False;
                            break;
                    }
                    break;
                case TriState.False:
                    switch (index)
                    {
                        case 0:
                            index = 1;
                            break;
                        case 4:
                            index = 5;
                            break;
                        case 14:
                            index = 9;
                            break;
                        case 15:
                            index = 10;
                            break;
                    }
                    break;
                default:
                    UseParensForNegativeNumbers = TriState.True;
                    switch (index)
                    {
                        case 1:
                        case 2:
                        case 3:
                            index = 0;
                            break;
                        case 5:
                        case 6:
                        case 7:
                            index = 4;
                            break;
                        case 8:
                        case 10:
                        case 13:
                            index = 15;
                            break;
                        case 9:
                        case 11:
                        case 12:
                            index = 14;
                            break;
                    }
                    break;
            }
            nfi.CurrencyNegativePattern = index;
            if (NumDigitsAfterDecimal == -1)
                NumDigitsAfterDecimal = nfi.CurrencyDecimalDigits;
            nfi.CurrencyDecimalDigits = NumDigitsAfterDecimal;
            formatProvider = (IFormatProvider)new FormatInfoHolder(nfi);
            if (IncludeLeadingDigit != TriState.False)
                return str1;
            nfi.NumberGroupSizes = nfi.CurrencyGroupSizes;
            string str2 = Strings.CurrencyPositiveFormatStrings[currencyPositivePattern] + ";" + Strings.CurrencyNegativeFormatStrings[index];
            string newValue = GroupDigits != TriState.False ? (IncludeLeadingDigit != TriState.False ? "#,##0" : "#,###") : (IncludeLeadingDigit != TriState.False ? "0" : "#");
            if (NumDigitsAfterDecimal > 0)
                newValue = newValue + "." + new string('0', NumDigitsAfterDecimal);
            if (string.CompareOrdinal("$", nfi.CurrencySymbol) != 0)
                str2 = str2.Replace("$", nfi.CurrencySymbol.Replace("'", "''"));
            return str2.Replace("n", newValue);
        }


        private static readonly string[] CurrencyPositiveFormatStrings = new string[4]
        {
            "'$'n",
            "n'$'",
            "'$' n",
            "n '$'"
        };
        private static readonly string[] CurrencyNegativeFormatStrings = new string[16]
        {
            "('$'n)",
            "-'$'n",
            "'$'-n",
            "'$'n-",
            "(n'$')",
            "-n'$'",
            "n-'$'",
            "n'$'-",
            "-n '$'",
            "-'$' n",
            "n '$'-",
            "'$' n-",
            "'$'- n",
            "n- '$'",
            "('$' n)",
            "(n '$')"
        };
        private static readonly string[] NumberNegativeFormatStrings = new string[5]
        {
            "(n)",
            "-n",
            "- n",
            "n-",
            "n -"
        };

        internal enum FormatType
        {
            Number,
            Percent,
            Currency,
        }
    }
}
