using Microsoft.VisualBasic.CompilerService;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic
{
	public class DateAndTime
	{
		public static DateTime Today => DateTime.Today;
		public static DateTime Now => DateTime.Now;

		public static DateTime TimeOfDay
		{
			get
			{
				long ticks = DateTime.Now.TimeOfDay.Ticks;
				checked
				{
					return new DateTime(ticks - unchecked(ticks % 10000000));
				}
			}
		}
		public static double Timer => (double)(DateTime.Now.Ticks % 864000000000L) / 10000000.0;

		private static Calendar CurrentCalendar => Thread.CurrentThread.CurrentCulture.Calendar;

		/// <summary>Returns an Integer value from 1 through 9999 representing the year.</summary>
		/// <returns>Returns an Integer value from 1 through 9999 representing the year.</returns>
		/// <param name="DateValue">Required. Date value from which you want to extract the year.</param>
		/// <filterpriority>1</filterpriority>
		public static int Year(DateTime DateValue)
		{
			return CurrentCalendar.GetYear(DateValue);
		}

		/// <summary>Returns an Integer value from 1 through 12 representing the month of the year.</summary>
		/// <returns>Returns an Integer value from 1 through 12 representing the month of the year.</returns>
		/// <param name="DateValue">Required. Date value from which you want to extract the month.</param>
		/// <filterpriority>1</filterpriority>
		public static int Month(DateTime DateValue)
		{
			return CurrentCalendar.GetMonth(DateValue);
		}

		/// <summary>Returns an Integer value from 1 through 31 representing the day of the month.</summary>
		/// <returns>Returns an Integer value from 1 through 31 representing the day of the month.</returns>
		/// <param name="DateValue">Required. Date value from which you want to extract the day.</param>
		/// <filterpriority>1</filterpriority>
		public static int Day(DateTime DateValue)
		{
			return CurrentCalendar.GetDayOfMonth(DateValue);
		}

		/// <summary>Returns an Integer value from 0 through 23 representing the hour of the day.</summary>
		/// <returns>Returns an Integer value from 0 through 23 representing the hour of the day.</returns>
		/// <param name="TimeValue">Required. Date value from which you want to extract the hour.</param>
		/// <filterpriority>1</filterpriority>
		public static int Hour(DateTime TimeValue)
		{
			return CurrentCalendar.GetHour(TimeValue);
		}

		/// <summary>Returns an Integer value from 0 through 59 representing the minute of the hour.</summary>
		/// <returns>Returns an Integer value from 0 through 59 representing the minute of the hour.</returns>
		/// <param name="TimeValue">Required. Date value from which you want to extract the minute.</param>
		/// <filterpriority>1</filterpriority>
		public static int Minute(DateTime TimeValue)
		{
			return CurrentCalendar.GetMinute(TimeValue);
		}

		/// <summary>Returns an Integer value from 0 through 59 representing the second of the minute.</summary>
		/// <returns>Returns an Integer value from 0 through 59 representing the second of the minute.</returns>
		/// <param name="TimeValue">Required. Date value from which you want to extract the second.</param>
		/// <filterpriority>1</filterpriority>
		public static int Second(DateTime TimeValue)
		{
			return CurrentCalendar.GetSecond(TimeValue);
		}

		/// <summary>Returns a Date value representing a specified year, month, and day, with the time information set to midnight (00:00:00).</summary>
		/// <returns>Returns a Date value representing a specified year, month, and day, with the time information set to midnight (00:00:00).</returns>
		/// <param name="Year">Required. Integer expression from 1 through 9999. However, values below this range are also accepted. If <paramref name="Year" /> is 0 through 99, it is interpreted as being between 1930 and 2029, as explained in the "Remarks" section below. If <paramref name="Year" /> is less than 1, it is subtracted from the current year.</param>
		/// <param name="Month">Required. Integer expression from 1 through 12. However, values outside this range are also accepted. The value of <paramref name="Month" /> is offset by 1 and applied to January of the calculated year. In other words, (<paramref name="Month" /> - 1) is added to January. The year is recalculated if necessary. The following results illustrate this effect:If <paramref name="Month" /> is 1, the result is January of the calculated year.If <paramref name="Month" /> is 0, the result is December of the previous year.If <paramref name="Month" /> is -1, the result is November of the previous year.If <paramref name="Month" /> is 13, the result is January of the following year.</param>
		/// <param name="Day">Required. Integer expression from 1 through 31. However, values outside this range are also accepted. The value of <paramref name="Day" /> is offset by 1 and applied to the first day of the calculated month. In other words, (<paramref name="Day" /> - 1) is added to the first of the month. The month and year are recalculated if necessary. The following results illustrate this effect:If <paramref name="Day" /> is 1, the result is the first day of the calculated month.If <paramref name="Day" /> is 0, the result is the last day of the previous month.If <paramref name="Day" /> is -1, the result is the penultimate day of the previous month.If <paramref name="Day" /> is past the end of the current month, the result is the appropriate day of the following month. For example, if <paramref name="Month" /> is 4 and <paramref name="Day" /> is 31, the result is May 1.</param>
		/// <filterpriority>1</filterpriority>
		public static DateTime DateSerial(int Year, int Month, int Day)
		{
			Calendar currentCalendar = CurrentCalendar;
			checked
			{
				if (Year < 0)
				{
					Year = currentCalendar.GetYear(DateTime.Today) + Year;
				}
				else if (Year < 100)
				{
					Year = currentCalendar.ToFourDigitYear(Year);
				}
				if (currentCalendar is GregorianCalendar && Month >= 1 && Month <= 12 && Day >= 1 && Day <= 28)
				{
					return new DateTime(Year, Month, Day);
				}
				DateTime time;
				try
				{
					time = currentCalendar.ToDateTime(Year, 1, 1, 0, 0, 0, 0);
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Year"));
				}
				try
				{
					time = currentCalendar.AddMonths(time, Month - 1);
				}
				catch (StackOverflowException ex5)
				{
					throw ex5;
				}
				catch (OutOfMemoryException ex6)
				{
					throw ex6;
				}
				catch (ThreadAbortException ex7)
				{
					throw ex7;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Month"));
				}
				try
				{
					time = currentCalendar.AddDays(time, Day - 1);
				}
				catch (StackOverflowException ex9)
				{
					throw ex9;
				}
				catch (OutOfMemoryException ex10)
				{
					throw ex10;
				}
				catch (ThreadAbortException ex11)
				{
					throw ex11;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Day"));
				}
				return time;
			}
		}

		/// <summary>Returns a Date value representing a specified hour, minute, and second, with the date information set relative to January 1 of the year 1.</summary>
		/// <returns>Returns a Date value representing a specified hour, minute, and second, with the date information set relative to January 1 of the year 1.</returns>
		/// <param name="Hour">Required. Integer expression from 0 through 23. However, values outside this range are also accepted.</param>
		/// <param name="Minute">Required. Integer expression from 0 through 59. However, values outside this range are also accepted. The value of <paramref name="Minute" /> is added to the calculated hour, so a negative value specifies minutes before that hour.</param>
		/// <param name="Second">Required. Integer expression from 0 through 59. However, values outside this range are also accepted. The value of <paramref name="Second" /> is added to the calculated minute, so a negative value specifies seconds before that minute.</param>
		/// <exception cref="T:System.ArgumentException">An argument is outside the range -2,147,483,648 through 2,147,483,647</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Calculated time is less than negative 24 hours.</exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime TimeSerial(int Hour, int Minute, int Second)
		{
			checked
			{
				int num = Hour * 60 * 60 + Minute * 60 + Second;
				if (num < 0)
				{
					num += 86400;
				}
				return new DateTime(unchecked((long)num) * 10000000L);
			}
		}

		/// <summary>Returns a Date value containing the date information represented by a string, with the time information set to midnight (00:00:00).</summary>
		/// <returns>Date value containing the date information represented by a string, with the time information set to midnight (00:00:00).</returns>
		/// <param name="StringDate">Required. String expression representing a date/time value from 00:00:00 on January 1 of the year 1 through 23:59:59 on December 31, 9999.</param>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="StringDate" /> includes invalid time information.</exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime DateValue(string StringDate)
		{
			return Conversions.ToDate(StringDate).Date;
		}

		/// <summary>Returns a Date value containing the time information represented by a string, with the date information set to January 1 of the year 1.</summary>
		/// <returns>Returns a Date value containing the time information represented by a string, with the date information set to January 1 of the year 1.</returns>
		/// <param name="StringTime">Required. String expression representing a date/time value from 00:00:00 on January 1 of the year 1 through 23:59:59 on December 31, 9999.</param>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="StringTime" /> includes invalid date information.</exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime TimeValue(string StringTime)
		{
			return new DateTime(Conversions.ToDate(StringTime).Ticks % 864000000000L);
		}

		/// <summary>Returns an Integer value containing a number representing the day of the week.</summary>
		/// <returns>Returns an Integer value containing a number representing the day of the week.</returns>
		/// <param name="DateValue">Required. Date value for which you want to determine the day of the week.</param>
		/// <param name="DayOfWeek">Optional. A value chosen from the FirstDayOfWeek enumeration that specifies the first day of the week. If not specified, FirstDayOfWeek.Sunday is used.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="DayOfWeek" /> is less than 0 or more than 7.</exception>
		/// <filterpriority>1</filterpriority>
		public static int Weekday(DateTime DateValue, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday)
		{
			switch (DayOfWeek)
			{
				case FirstDayOfWeek.System:
					DayOfWeek = (FirstDayOfWeek)checked(DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek + 1);
					break;
				default:
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "DayOfWeek"));
				case FirstDayOfWeek.Sunday:
				case FirstDayOfWeek.Monday:
				case FirstDayOfWeek.Tuesday:
				case FirstDayOfWeek.Wednesday:
				case FirstDayOfWeek.Thursday:
				case FirstDayOfWeek.Friday:
				case FirstDayOfWeek.Saturday:
					break;
			}
			checked
			{
				return unchecked(checked(unchecked((int)checked(CurrentCalendar.GetDayOfWeek(DateValue) + 1)) - unchecked((int)DayOfWeek) + 7) % 7) + 1;
			}
		}


		/// <summary>Returns a String value containing the name of the specified month.</summary>
		/// <returns>Returns a String value containing the name of the specified month.</returns>
		/// <param name="Month">Required. Integer. The numeric designation of the month, from 1 through 13; 1 indicates January and 12 indicates December. You can use the value 13 with a 13-month calendar. If your system is using a 12-month calendar and <paramref name="Month" /> is 13, MonthName returns an empty string.</param>
		/// <param name="Abbreviate">Optional. Boolean value that indicates if the month name is to be abbreviated. If omitted, the default is False, which means the month name is not abbreviated.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Month" /> is less than 1 or greater than 13.</exception>
		/// <filterpriority>1</filterpriority>
		public static string MonthName(int Month, bool Abbreviate = false)
		{
			if (Month < 1 || Month > 13)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Month"));
			}
			string text = (!Abbreviate) ? Utils.GetDateTimeFormatInfo().GetMonthName(Month) : Utils.GetDateTimeFormatInfo().GetAbbreviatedMonthName(Month);
			if (text.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Month"));
			}
			return text;
		}

		/// <summary>Returns a String value containing the name of the specified weekday.</summary>
		/// <returns>Returns a String value containing the name of the specified weekday.</returns>
		/// <param name="Weekday">Required. Integer. The numeric designation for the weekday, from 1 through 7; 1 indicates the first day of the week and 7 indicates the last day of the week. The identities of the first and last days depend on the setting of <paramref name="FirstDayOfWeekValue" />.</param>
		/// <param name="Abbreviate">Optional. Boolean value that indicates if the weekday name is to be abbreviated. If omitted, the default is False, which means the weekday name is not abbreviated.</param>
		/// <param name="FirstDayOfWeekValue">Optional. A value chosen from the FirstDayOfWeek enumeration that specifies the first day of the week. If not specified, FirstDayOfWeek.System is used.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="Weekday" /> is less than 1 or greater than 7, or <paramref name="FirstDayOfWeekValue" /> is less than 0 or greater than 7.</exception>
		/// <filterpriority>1</filterpriority>
		public static string WeekdayName(int Weekday, bool Abbreviate = false, FirstDayOfWeek FirstDayOfWeekValue = FirstDayOfWeek.System)
		{
			if (Weekday < 1 || Weekday > 7)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Weekday"));
			}
			if (FirstDayOfWeekValue < FirstDayOfWeek.System || FirstDayOfWeekValue > FirstDayOfWeek.Saturday)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "FirstDayOfWeekValue"));
			}
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)Utils.GetCultureInfo().GetFormat(typeof(DateTimeFormatInfo));
			if (FirstDayOfWeekValue == FirstDayOfWeek.System)
			{
				FirstDayOfWeekValue = (FirstDayOfWeek)checked(dateTimeFormatInfo.FirstDayOfWeek + 1);
			}
			string text;
			try
			{
				text = ((!Abbreviate) ? dateTimeFormatInfo.GetDayName((DayOfWeek)((int)checked(Weekday + FirstDayOfWeekValue - 2) % 7)) : dateTimeFormatInfo.GetAbbreviatedDayName((DayOfWeek)((int)checked(Weekday + FirstDayOfWeekValue - 2) % 7)));
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Weekday"));
			}
			if (text.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Weekday"));
			}
			return text;
		}

        public static int DatePart(DateInterval Interval, DateTime DateValue, FirstDayOfWeek FirstDayOfWeekValue = FirstDayOfWeek.Sunday, FirstWeekOfYear FirstWeekOfYearValue = FirstWeekOfYear.Jan1)
        {
            switch (Interval)
            {
                case DateInterval.Year:
                    return CurrentCalendar.GetYear(DateValue);
                case DateInterval.Month:
                    return CurrentCalendar.GetMonth(DateValue);
                case DateInterval.Day:
                    return CurrentCalendar.GetDayOfMonth(DateValue);
                case DateInterval.Hour:
                    return CurrentCalendar.GetHour(DateValue);
                case DateInterval.Minute:
                    return CurrentCalendar.GetMinute(DateValue);
                case DateInterval.Second:
                    return CurrentCalendar.GetSecond(DateValue);
                case DateInterval.Weekday:
                    return Weekday(DateValue, FirstDayOfWeekValue);
                case DateInterval.WeekOfYear:
                    {
                        DayOfWeek firstDayOfWeek = (DayOfWeek)((FirstDayOfWeekValue != 0) ? ((int)checked(FirstDayOfWeekValue - 1)) : ((int)Utils.GetCultureInfo().DateTimeFormat.FirstDayOfWeek));
                        CalendarWeekRule rule = default(CalendarWeekRule);
                        switch (FirstWeekOfYearValue)
                        {
                            case FirstWeekOfYear.System:
                                rule = Utils.GetCultureInfo().DateTimeFormat.CalendarWeekRule;
                                break;
                            case FirstWeekOfYear.Jan1:
                                rule = CalendarWeekRule.FirstDay;
                                break;
                            case FirstWeekOfYear.FirstFourDays:
                                rule = CalendarWeekRule.FirstFourDayWeek;
                                break;
                            case FirstWeekOfYear.FirstFullWeek:
                                rule = CalendarWeekRule.FirstFullWeek;
                                break;
                        }
                        return CurrentCalendar.GetWeekOfYear(DateValue, rule, firstDayOfWeek);
                    }
                case DateInterval.Quarter:
                    checked
                    {
                        return unchecked(checked(DateValue.Month - 1) / 3) + 1;
                    }
                case DateInterval.DayOfYear:
                    return CurrentCalendar.GetDayOfYear(DateValue);
                default:
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Interval"));
            }
        }
        /// <summary>Returns an Integer value containing the specified component of a given Date value.</summary>
        /// <returns>Returns an Integer value containing the specified component of a given Date value.</returns>
        /// <param name="Interval">Required. DateInterval enumeration value or String expression representing the part of the date/time value you want to return.</param>
        /// <param name="DateValue">Required. Date value that you want to evaluate.</param>
        /// <param name="DayOfWeek">Optional. A value chosen from the FirstDayOfWeek enumeration that specifies the first day of the week. If not specified, FirstDayOfWeek.Sunday is used.</param>
        /// <param name="WeekOfYear">Optional. A value chosen from the FirstWeekOfYear enumeration that specifies the first week of the year. If not specified, FirstWeekOfYear.Jan1 is used.</param>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public static int DatePart(string Interval, object DateValue, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
        {
            DateTime dateValue;
            try
            {
                dateValue = Conversions.ToDate(DateValue);
            }
            catch (StackOverflowException ex)
            {
                throw ex;
            }
            catch (OutOfMemoryException ex2)
            {
                throw ex2;
            }
            catch (ThreadAbortException ex3)
            {
                throw ex3;
            }
            catch (Exception)
            {
                throw new InvalidCastException(Utils.GetResourceString("Argument_InvalidDateValue1", "DateValue"));
            }
            return DatePart(DateIntervalFromString(Interval), dateValue, DayOfWeek, WeekOfYear);
        }
        private static DateInterval DateIntervalFromString(string Interval)
        {
            if (Interval != null)
            {
                Interval = Interval.ToUpperInvariant();
            }
            switch (Interval)
            {
                case "YYYY":
                    return DateInterval.Year;
                case "Y":
                    return DateInterval.DayOfYear;
                case "M":
                    return DateInterval.Month;
                case "D":
                    return DateInterval.Day;
                case "H":
                    return DateInterval.Hour;
                case "N":
                    return DateInterval.Minute;
                case "S":
                    return DateInterval.Second;
                case "WW":
                    return DateInterval.WeekOfYear;
                case "W":
                    return DateInterval.Weekday;
                case "Q":
                    return DateInterval.Quarter;
                default:
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", "Interval"));
            }
        }
    }
}
