// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.Operators
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>Provides late-bound math operators, such as <see cref="M:Microsoft.VisualBasic.CompilerServices.Operators.AddObject(System.Object,System.Object)" /> and <see cref="M:Microsoft.VisualBasic.CompilerServices.Operators.CompareObject(System.Object,System.Object,System.Boolean)" />, which the Visual Basic compiler uses internally.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class Operators
  {
    internal static readonly object Boxed_ZeroDouble = (object) 0.0;
    internal static readonly object Boxed_ZeroSinge = (object) 0.0f;
    internal static readonly object Boxed_ZeroDecimal = (object) Decimal.Zero;
    internal static readonly object Boxed_ZeroLong = (object) 0L;
    internal static readonly object Boxed_ZeroInteger = (object) 0;
    internal static readonly object Boxed_ZeroShort = (object) (short) 0;
    internal static readonly object Boxed_ZeroULong = (object) 0UL;
    internal static readonly object Boxed_ZeroUInteger = (object) 0U;
    internal static readonly object Boxed_ZeroUShort = (object) (ushort) 0;
    internal static readonly object Boxed_ZeroSByte = (object) (sbyte) 0;
    internal static readonly object Boxed_ZeroByte = (object) (byte) 0;

    internal static List<Symbols.Method> CollectOperators(
      Symbols.UserDefinedOperator op,
      Type type1,
      Type type2,
      ref bool foundType1Operators,
      ref bool foundType2Operators)
    {
      int num1 = type2 != null ? 1 : 0;
      int num2;
      List<Symbols.Method> methodList1;
      if (!Symbols.IsRootObjectType(type1) && Symbols.IsClassOrValueType(type1))
      {
        MemberInfo[] members = new Symbols.Container(type1).LookupNamedMembers(Symbols.OperatorCLSNames[(int) op]);
        int argumentCount = Interaction.IIf<int>(Symbols.IsUnaryOperator(op), 1, 2);
        int num3 = 0;
        ref int local1 = ref num3;
        num2 = 0;
        ref int local2 = ref num2;
        methodList1 = OverloadResolution.CollectOverloadCandidates(members, (object[]) null, argumentCount, (string[]) null, (Type[]) null, true, (Type) null, ref local1, ref local2, (Symbols.Container) null);
        if (methodList1.Count > 0)
          foundType1Operators = true;
      }
      else
        methodList1 = new List<Symbols.Method>();
      if (num1 != 0 && !Symbols.IsRootObjectType(type2) && Symbols.IsClassOrValueType(type2))
      {
        Type @base = type1;
        while ((object) @base != null && !Symbols.IsOrInheritsFrom(type2, @base))
          @base = @base.BaseType;
        MemberInfo[] members = new Symbols.Container(type2).LookupNamedMembers(Symbols.OperatorCLSNames[(int) op]);
        int argumentCount = Interaction.IIf<int>(Symbols.IsUnaryOperator(op), 1, 2);
        Type terminatingScope = @base;
        num2 = 0;
        ref int local1 = ref num2;
        int num3 = 0;
        ref int local2 = ref num3;
        List<Symbols.Method> methodList2 = OverloadResolution.CollectOverloadCandidates(members, (object[]) null, argumentCount, (string[]) null, (Type[]) null, true, terminatingScope, ref local1, ref local2, (Symbols.Container) null);
        if (methodList2.Count > 0)
          foundType2Operators = true;
        methodList1.AddRange((IEnumerable<Symbols.Method>) methodList2);
      }
      return methodList1;
    }

    internal static Symbols.Method ResolveUserDefinedOperator(
      Symbols.UserDefinedOperator op,
      object[] arguments,
      bool reportErrors)
    {
      arguments = (object[]) arguments.Clone();
      Type type1 = (Type) null;
      Type type2;
      if (arguments[0] == null)
      {
        type1 = arguments[1].GetType();
        type2 = type1;
        arguments[0] = (object) new Symbols.TypedNothing(type2);
      }
      else
      {
        type2 = arguments[0].GetType();
        if (arguments.Length > 1)
        {
          if (arguments[1] != null)
          {
            type1 = arguments[1].GetType();
          }
          else
          {
            type1 = type2;
            arguments[1] = (object) new Symbols.TypedNothing(type1);
          }
        }
      }
      bool foundType1Operators;
      bool foundType2Operators;
      List<Symbols.Method> candidates = Operators.CollectOperators(op, type2, type1, ref foundType1Operators, ref foundType2Operators);
      OverloadResolution.ResolutionFailure failure;
      return candidates.Count <= 0 ? (Symbols.Method) null : OverloadResolution.ResolveOverloadedCall(Symbols.OperatorNames[(int) op], candidates, arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, ReflectionExtensions.BindingFlagsInvokeMethod, reportErrors, ref failure);
    }

    internal static object InvokeUserDefinedOperator(
      Symbols.Method operatorMethod,
      bool forceArgumentValidation,
      params object[] arguments)
    {
      if ((!operatorMethod.ArgumentsValidated || forceArgumentValidation) && !OverloadResolution.CanMatchArguments(operatorMethod, arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, false, (List<string>) null))
      {
        string str1 = "";
        List<string> errors = new List<string>();
        OverloadResolution.CanMatchArguments(operatorMethod, arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, false, errors);
        try
        {
          foreach (string str2 in errors)
            str1 = str1 + "\r\n    " + str2;
        }
        finally
        {
          List<string>.Enumerator enumerator;
          enumerator.Dispose();
        }
        throw new InvalidCastException(Utils.GetResourceString(SR.MatchArgumentFailure2, operatorMethod.ToString(), str1));
      }
      return new Symbols.Container(operatorMethod.DeclaringType).InvokeMethod(operatorMethod, arguments, (bool[]) null, ReflectionExtensions.BindingFlagsInvokeMethod);
    }

    internal static object InvokeUserDefinedOperator(
      Symbols.UserDefinedOperator op,
      params object[] arguments)
    {
      return IDOUtils.TryCastToIDMOP(arguments[0]) == null ? Operators.InvokeObjectUserDefinedOperator(op, arguments) : IDOBinder.InvokeUserDefinedOperator(op, arguments);
    }

    /// <summary>Executes a late-bound evaluation of a user-defined operator. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="vbOp">The user-defined operator.</param>
    /// <param name="Arguments">Argument values to pass to the user-defined operator.</param>
    /// <returns>The result returned from the user-defined operator.</returns>
    [DebuggerStepThrough]
    [Obsolete("do not use this method", true)]
    [DebuggerHidden]
    public static object FallbackInvokeUserDefinedOperator(object vbOp, object[] arguments)
    {
      return Operators.InvokeObjectUserDefinedOperator((Symbols.UserDefinedOperator) Conversions.ToSByte(vbOp), arguments);
    }

    internal static object InvokeObjectUserDefinedOperator(
      Symbols.UserDefinedOperator op,
      object[] arguments)
    {
      Symbols.Method operatorMethod = Operators.ResolveUserDefinedOperator(op, arguments, true);
      if ((object) operatorMethod != null)
        return Operators.InvokeUserDefinedOperator(operatorMethod, false, arguments);
      if (arguments.Length > 1)
        throw Operators.GetNoValidOperatorException(op, arguments[0], arguments[1]);
      throw Operators.GetNoValidOperatorException(op, arguments[0]);
    }

    internal static Symbols.Method GetCallableUserDefinedOperator(
      Symbols.UserDefinedOperator op,
      params object[] arguments)
    {
      Symbols.Method targetProcedure = Operators.ResolveUserDefinedOperator(op, arguments, false);
      return (object) targetProcedure == null || targetProcedure.ArgumentsValidated || OverloadResolution.CanMatchArguments(targetProcedure, arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, false, (List<string>) null) ? targetProcedure : (Symbols.Method) null;
    }

    private Operators()
    {
    }

    private static sbyte ToVBBool(object conv)
    {
      return (sbyte) -(Convert.ToBoolean(conv) ? 1 : 0);
    }

    private static TypeCode GetTypeCode(object o)
    {
      return o != null ? o.GetType().GetTypeCode() : TypeCode.Empty;
    }

    private static Type GetEnumResult(object left, object right)
    {
      Type type1;
      if (left != null)
      {
        if (left is Enum)
        {
          if (right == null)
          {
            type1 = left.GetType();
            goto label_10;
          }
          else if (right is Enum)
          {
            Type type2 = left.GetType();
            if ((object) type2 == (object) right.GetType())
            {
              type1 = type2;
              goto label_10;
            }
          }
        }
      }
      else if (right is Enum)
      {
        type1 = right.GetType();
        goto label_10;
      }
      type1 = (Type) null;
label_10:
      return type1;
    }

    private static Exception GetNoValidOperatorException(
      Symbols.UserDefinedOperator op,
      object operand)
    {
      return (Exception) new InvalidCastException(Utils.GetResourceString(SR.UnaryOperand2, Symbols.OperatorNames[(int) op], Utils.VBFriendlyName(operand)));
    }

    private static Exception GetNoValidOperatorException(
      Symbols.UserDefinedOperator op,
      object left,
      object right)
    {
      string str1;
      if (left == null)
        str1 = "'Nothing'";
      else if (left is string str2)
        str1 = Utils.GetResourceString(SR.NoValidOperator_StringType1, Strings.Left(str2, 32));
      else
        str1 = Utils.GetResourceString(SR.NoValidOperator_NonStringType1, Utils.VBFriendlyName(left));
      string str3;
      if (right == null)
        str3 = "'Nothing'";
      else if (right is string str2)
        str3 = Utils.GetResourceString(SR.NoValidOperator_StringType1, Strings.Left(str2, 32));
      else
        str3 = Utils.GetResourceString(SR.NoValidOperator_NonStringType1, Utils.VBFriendlyName(right));
      return (Exception) new InvalidCastException(Utils.GetResourceString(SR.BinaryOperands3, Symbols.OperatorNames[(int) op], str1, str3));
    }

    /// <summary>Represents the Visual Basic equal (=) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>
    /// <see langword="True" /> if <paramref name="Left" /> and <paramref name="Right" /> are equal; otherwise, <see langword="False" />.</returns>
    public static object CompareObjectEqual(object Left, object Right, bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      object obj;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          obj = (object) false;
          break;
        case Operators.CompareClass.UserDefined:
          obj = Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Equal, Left, Right);
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Equal, Left, Right);
        default:
          obj = (object) (compareClass == Operators.CompareClass.Equal);
          break;
      }
      return obj;
    }

    /// <summary>Represents the overloaded Visual Basic equals (=) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>The result of the overloaded equals operator. <see langword="False" /> if operator overloading is not supported.</returns>
    public static bool ConditionalCompareObjectEqual(object Left, object Right, bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      bool flag;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          flag = false;
          break;
        case Operators.CompareClass.UserDefined:
          flag = Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Equal, Left, Right));
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Equal, Left, Right);
        default:
          flag = compareClass == Operators.CompareClass.Equal;
          break;
      }
      return flag;
    }

    /// <summary>Represents the Visual Basic not-equal (&lt;&gt;) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>
    /// <see langword="True" /> if <paramref name="Left" /> is not equal to <paramref name="Right" />; otherwise, <see langword="False" />.</returns>
    public static object CompareObjectNotEqual(object Left, object Right, bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      object obj;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          obj = (object) true;
          break;
        case Operators.CompareClass.UserDefined:
          obj = Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.NotEqual, Left, Right);
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.NotEqual, Left, Right);
        default:
          obj = (object) ((uint) compareClass > 0U);
          break;
      }
      return obj;
    }

    /// <summary>Represents the overloaded Visual Basic not-equal (&lt;&gt;) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>The result of the overloaded not-equal operator. <see langword="False" /> if operator overloading is not supported.</returns>
    public static bool ConditionalCompareObjectNotEqual(
      object Left,
      object Right,
      bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      bool flag;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          flag = true;
          break;
        case Operators.CompareClass.UserDefined:
          flag = Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.NotEqual, Left, Right));
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.NotEqual, Left, Right);
        default:
          flag = (uint) compareClass > 0U;
          break;
      }
      return flag;
    }

    /// <summary>Represents the Visual Basic less-than (&lt;) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>
    /// <see langword="True" /> if <paramref name="Left" /> is less than <paramref name="Right" />; otherwise, <see langword="False" />.</returns>
    public static object CompareObjectLess(object Left, object Right, bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      object obj;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          obj = (object) false;
          break;
        case Operators.CompareClass.UserDefined:
          obj = Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Less, Left, Right);
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Less, Left, Right);
        default:
          obj = (object) (compareClass < Operators.CompareClass.Equal);
          break;
      }
      return obj;
    }

    /// <summary>Represents the overloaded Visual Basic less-than (&lt;) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>The result of the overloaded less-than operator. <see langword="False" /> if operator overloading is not supported.</returns>
    public static bool ConditionalCompareObjectLess(object Left, object Right, bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      bool flag;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          flag = false;
          break;
        case Operators.CompareClass.UserDefined:
          flag = Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Less, Left, Right));
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Less, Left, Right);
        default:
          flag = compareClass < Operators.CompareClass.Equal;
          break;
      }
      return flag;
    }

    /// <summary>Represents the Visual Basic less-than or equal-to (&lt;=) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>
    /// <see langword="True" /> if <paramref name="Left" /> is less than or equal to <paramref name="Right" />; otherwise, <see langword="False" />.</returns>
    public static object CompareObjectLessEqual(object Left, object Right, bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      object obj;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          obj = (object) false;
          break;
        case Operators.CompareClass.UserDefined:
          obj = Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.LessEqual, Left, Right);
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.LessEqual, Left, Right);
        default:
          obj = (object) (compareClass <= Operators.CompareClass.Equal);
          break;
      }
      return obj;
    }

    /// <summary>Represents the overloaded Visual Basic less-than or equal-to (&lt;=) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>The result of the overloaded less-than or equal-to operator. <see langword="False" /> if operator overloading is not supported.</returns>
    public static bool ConditionalCompareObjectLessEqual(
      object Left,
      object Right,
      bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      bool flag;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          flag = false;
          break;
        case Operators.CompareClass.UserDefined:
          flag = Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.LessEqual, Left, Right));
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.LessEqual, Left, Right);
        default:
          flag = compareClass <= Operators.CompareClass.Equal;
          break;
      }
      return flag;
    }

    /// <summary>Represents the Visual Basic greater-than or equal-to (&gt;=) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>
    /// <see langword="True" /> if <paramref name="Left" /> is greater than or equal to <paramref name="Right" />; otherwise, <see langword="False" />.</returns>
    public static object CompareObjectGreaterEqual(object Left, object Right, bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      object obj;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          obj = (object) false;
          break;
        case Operators.CompareClass.UserDefined:
          obj = Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.GreaterEqual, Left, Right);
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.GreaterEqual, Left, Right);
        default:
          obj = (object) (compareClass >= Operators.CompareClass.Equal);
          break;
      }
      return obj;
    }

    /// <summary>Represents the overloaded Visual Basic greater-than or equal-to (&gt;=) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>The result of the overloaded greater-than or equal-to operator. <see langword="False" /> if operator overloading is not supported.</returns>
    public static bool ConditionalCompareObjectGreaterEqual(
      object Left,
      object Right,
      bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      bool flag;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          flag = false;
          break;
        case Operators.CompareClass.UserDefined:
          flag = Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.GreaterEqual, Left, Right));
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.GreaterEqual, Left, Right);
        default:
          flag = compareClass >= Operators.CompareClass.Equal;
          break;
      }
      return flag;
    }

    /// <summary>Represents the Visual Basic greater-than (&gt;) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>
    /// <see langword="True" /> if <paramref name="Left" /> is greater than <paramref name="Right" />; otherwise, <see langword="False" />.</returns>
    public static object CompareObjectGreater(object Left, object Right, bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      object obj;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          obj = (object) false;
          break;
        case Operators.CompareClass.UserDefined:
          obj = Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Greater, Left, Right);
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Greater, Left, Right);
        default:
          obj = (object) (compareClass > Operators.CompareClass.Equal);
          break;
      }
      return obj;
    }

    /// <summary>Represents the overloaded Visual Basic greater-than (&gt;) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>The result of the overloaded greater-than operator. <see langword="False" /> if operator overloading is not supported.</returns>
    public static bool ConditionalCompareObjectGreater(object Left, object Right, bool TextCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
      bool flag;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          flag = false;
          break;
        case Operators.CompareClass.UserDefined:
          flag = Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Greater, Left, Right));
          break;
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Greater, Left, Right);
        default:
          flag = compareClass > Operators.CompareClass.Equal;
          break;
      }
      return flag;
    }

    /// <summary>Represents Visual Basic comparison operators.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>Value
    /// 
    /// Condition
    /// 
    /// -1
    /// 
    /// 
    ///               <paramref name="Left" /> is less than <paramref name="Right" />.
    /// 
    /// 0
    /// 
    /// 
    ///               <paramref name="Left" /> and <paramref name="Right" /> are equal.
    /// 
    /// 1
    /// 
    /// 
    ///               <paramref name="Left" /> is greater than <paramref name="Right" />.</returns>
    internal static int CompareObject(object left, object right, bool textCompare)
    {
      Operators.CompareClass compareClass = Operators.CompareObject2(left, right, textCompare);
      int num;
      switch (compareClass)
      {
        case Operators.CompareClass.Unordered:
          num = 0;
          break;
        case Operators.CompareClass.UserDefined:
        case Operators.CompareClass.Undefined:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.IsTrue, left, right);
        default:
          num = (int) compareClass;
          break;
      }
      return num;
    }

    private static Operators.CompareClass CompareObject2(
      object left,
      object right,
      bool textCompare)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(left);
      TypeCode typeCode2 = Operators.GetTypeCode(right);
      if (typeCode1 == TypeCode.Object && left is char[] chArray && (typeCode2 == TypeCode.String || typeCode2 == TypeCode.Empty || typeCode2 == TypeCode.Object && right is char[]))
      {
        left = (object) new string(chArray);
        typeCode1 = TypeCode.String;
      }
      if (typeCode2 == TypeCode.Object && right is char[] chArray && (typeCode1 == TypeCode.String || typeCode1 == TypeCode.Empty))
      {
        right = (object) new string(chArray);
        typeCode2 = TypeCode.String;
      }
      Operators.CompareClass compareClass;
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
          compareClass = Operators.CompareClass.Equal;
          break;
        case TypeCode.Boolean:
          compareClass = Operators.CompareBoolean(false, Convert.ToBoolean(right));
          break;
        case TypeCode.Char:
          compareClass = Operators.CompareChar(char.MinValue, Convert.ToChar(right));
          break;
        case TypeCode.SByte:
          compareClass = Operators.CompareInt32(0, (int) Convert.ToSByte(right));
          break;
        case TypeCode.Byte:
          compareClass = Operators.CompareInt32(0, (int) Convert.ToByte(right));
          break;
        case TypeCode.Int16:
          compareClass = Operators.CompareInt32(0, (int) Convert.ToInt16(right));
          break;
        case TypeCode.UInt16:
          compareClass = Operators.CompareInt32(0, (int) Convert.ToUInt16(right));
          break;
        case TypeCode.Int32:
          compareClass = Operators.CompareInt32(0, Convert.ToInt32(right));
          break;
        case TypeCode.UInt32:
          compareClass = Operators.CompareUInt32(0U, Convert.ToUInt32(right));
          break;
        case TypeCode.Int64:
          compareClass = Operators.CompareInt64(0L, Convert.ToInt64(right));
          break;
        case TypeCode.UInt64:
          compareClass = Operators.CompareUInt64(0UL, Convert.ToUInt64(right));
          break;
        case TypeCode.Single:
          compareClass = Operators.CompareSingle(0.0f, Convert.ToSingle(right));
          break;
        case TypeCode.Double:
          compareClass = Operators.CompareDouble(0.0, Convert.ToDouble(right));
          break;
        case TypeCode.Decimal:
          compareClass = Operators.CompareDecimal((object) Decimal.Zero, right);
          break;
        case TypeCode.DateTime:
          compareClass = Operators.CompareDate(DateTime.MinValue, Convert.ToDateTime(right));
          break;
        case TypeCode.String:
          compareClass = (Operators.CompareClass) Operators.CompareString((string) null, Convert.ToString(right), textCompare);
          break;
        case (TypeCode) 57:
          compareClass = Operators.CompareBoolean(Convert.ToBoolean(left), false);
          break;
        case (TypeCode) 60:
          compareClass = Operators.CompareBoolean(Convert.ToBoolean(left), Convert.ToBoolean(right));
          break;
        case (TypeCode) 62:
          compareClass = Operators.CompareInt32((int) Operators.ToVBBool(left), (int) Convert.ToSByte(right));
          break;
        case (TypeCode) 63:
        case (TypeCode) 64:
          compareClass = Operators.CompareInt32((int) Operators.ToVBBool(left), (int) Convert.ToInt16(right));
          break;
        case (TypeCode) 65:
        case (TypeCode) 66:
          compareClass = Operators.CompareInt32((int) Operators.ToVBBool(left), Convert.ToInt32(right));
          break;
        case (TypeCode) 67:
        case (TypeCode) 68:
          compareClass = Operators.CompareInt64((long) Operators.ToVBBool(left), Convert.ToInt64(right));
          break;
        case (TypeCode) 69:
        case (TypeCode) 72:
          compareClass = Operators.CompareDecimal((object) Operators.ToVBBool(left), right);
          break;
        case (TypeCode) 70:
          compareClass = Operators.CompareSingle((float) Operators.ToVBBool(left), Convert.ToSingle(right));
          break;
        case (TypeCode) 71:
          compareClass = Operators.CompareDouble((double) Operators.ToVBBool(left), Convert.ToDouble(right));
          break;
        case (TypeCode) 75:
          compareClass = Operators.CompareBoolean(Convert.ToBoolean(left), Conversions.ToBoolean(Convert.ToString(right)));
          break;
        case (TypeCode) 76:
          compareClass = Operators.CompareChar(Convert.ToChar(left), char.MinValue);
          break;
        case (TypeCode) 80:
          compareClass = Operators.CompareChar(Convert.ToChar(left), Convert.ToChar(right));
          break;
        case (TypeCode) 94:
        case (TypeCode) 346:
        case (TypeCode) 360:
          compareClass = (Operators.CompareClass) Operators.CompareString(Convert.ToString(left), Convert.ToString(right), textCompare);
          break;
        case (TypeCode) 95:
          compareClass = Operators.CompareInt32((int) Convert.ToSByte(left), 0);
          break;
        case (TypeCode) 98:
          compareClass = Operators.CompareInt32((int) Convert.ToSByte(left), (int) Operators.ToVBBool(right));
          break;
        case (TypeCode) 100:
          compareClass = Operators.CompareInt32((int) Convert.ToSByte(left), (int) Convert.ToSByte(right));
          break;
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 119:
        case (TypeCode) 121:
        case (TypeCode) 138:
        case (TypeCode) 139:
        case (TypeCode) 140:
          compareClass = Operators.CompareInt32((int) Convert.ToInt16(left), (int) Convert.ToInt16(right));
          break;
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 123:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 157:
        case (TypeCode) 159:
        case (TypeCode) 161:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
        case (TypeCode) 180:
          compareClass = Operators.CompareInt32(Convert.ToInt32(left), Convert.ToInt32(right));
          break;
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 125:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 163:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 195:
        case (TypeCode) 197:
        case (TypeCode) 199:
        case (TypeCode) 201:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 220:
          compareClass = Operators.CompareInt64(Convert.ToInt64(left), Convert.ToInt64(right));
          break;
        case (TypeCode) 107:
        case (TypeCode) 110:
        case (TypeCode) 129:
        case (TypeCode) 145:
        case (TypeCode) 148:
        case (TypeCode) 167:
        case (TypeCode) 183:
        case (TypeCode) 186:
        case (TypeCode) 205:
        case (TypeCode) 221:
        case (TypeCode) 224:
        case (TypeCode) 233:
        case (TypeCode) 235:
        case (TypeCode) 237:
        case (TypeCode) 239:
        case (TypeCode) 243:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 300:
          compareClass = Operators.CompareDecimal(left, right);
          break;
        case (TypeCode) 108:
        case (TypeCode) 127:
        case (TypeCode) 146:
        case (TypeCode) 165:
        case (TypeCode) 184:
        case (TypeCode) 203:
        case (TypeCode) 222:
        case (TypeCode) 241:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 262:
        case (TypeCode) 298:
          compareClass = Operators.CompareSingle(Convert.ToSingle(left), Convert.ToSingle(right));
          break;
        case (TypeCode) 109:
        case (TypeCode) 128:
        case (TypeCode) 147:
        case (TypeCode) 166:
        case (TypeCode) 185:
        case (TypeCode) 204:
        case (TypeCode) 223:
        case (TypeCode) 242:
        case (TypeCode) 261:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 299:
          compareClass = Operators.CompareDouble(Convert.ToDouble(left), Convert.ToDouble(right));
          break;
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          compareClass = Operators.CompareDouble(Convert.ToDouble(left), Conversions.ToDouble(Convert.ToString(right)));
          break;
        case (TypeCode) 114:
          compareClass = Operators.CompareInt32((int) Convert.ToByte(left), 0);
          break;
        case (TypeCode) 117:
          compareClass = Operators.CompareInt32((int) Convert.ToInt16(left), (int) Operators.ToVBBool(right));
          break;
        case (TypeCode) 120:
          compareClass = Operators.CompareInt32((int) Convert.ToByte(left), (int) Convert.ToByte(right));
          break;
        case (TypeCode) 122:
        case (TypeCode) 158:
        case (TypeCode) 160:
          compareClass = Operators.CompareInt32((int) Convert.ToUInt16(left), (int) Convert.ToUInt16(right));
          break;
        case (TypeCode) 124:
        case (TypeCode) 162:
        case (TypeCode) 196:
        case (TypeCode) 198:
        case (TypeCode) 200:
          compareClass = Operators.CompareUInt32(Convert.ToUInt32(left), Convert.ToUInt32(right));
          break;
        case (TypeCode) 126:
        case (TypeCode) 164:
        case (TypeCode) 202:
        case (TypeCode) 234:
        case (TypeCode) 236:
        case (TypeCode) 238:
        case (TypeCode) 240:
          compareClass = Operators.CompareUInt64(Convert.ToUInt64(left), Convert.ToUInt64(right));
          break;
        case (TypeCode) 133:
          compareClass = Operators.CompareInt32((int) Convert.ToInt16(left), 0);
          break;
        case (TypeCode) 136:
          compareClass = Operators.CompareInt32((int) Convert.ToInt16(left), (int) Operators.ToVBBool(right));
          break;
        case (TypeCode) 152:
          compareClass = Operators.CompareInt32((int) Convert.ToUInt16(left), 0);
          break;
        case (TypeCode) 155:
          compareClass = Operators.CompareInt32(Convert.ToInt32(left), (int) Operators.ToVBBool(right));
          break;
        case (TypeCode) 171:
          compareClass = Operators.CompareInt32(Convert.ToInt32(left), 0);
          break;
        case (TypeCode) 174:
          compareClass = Operators.CompareInt32(Convert.ToInt32(left), (int) Operators.ToVBBool(right));
          break;
        case (TypeCode) 190:
          compareClass = Operators.CompareUInt32(Convert.ToUInt32(left), 0U);
          break;
        case (TypeCode) 193:
          compareClass = Operators.CompareInt64(Convert.ToInt64(left), (long) Operators.ToVBBool(right));
          break;
        case (TypeCode) 209:
          compareClass = Operators.CompareInt64(Convert.ToInt64(left), 0L);
          break;
        case (TypeCode) 212:
          compareClass = Operators.CompareInt64(Convert.ToInt64(left), (long) Operators.ToVBBool(right));
          break;
        case (TypeCode) 228:
          compareClass = Operators.CompareUInt64(Convert.ToUInt64(left), 0UL);
          break;
        case (TypeCode) 231:
          compareClass = Operators.CompareDecimal(left, (object) Operators.ToVBBool(right));
          break;
        case (TypeCode) 247:
          compareClass = Operators.CompareSingle(Convert.ToSingle(left), 0.0f);
          break;
        case (TypeCode) 250:
          compareClass = Operators.CompareSingle(Convert.ToSingle(left), (float) Operators.ToVBBool(right));
          break;
        case (TypeCode) 266:
          compareClass = Operators.CompareDouble(Convert.ToDouble(left), 0.0);
          break;
        case (TypeCode) 269:
          compareClass = Operators.CompareDouble(Convert.ToDouble(left), (double) Operators.ToVBBool(right));
          break;
        case (TypeCode) 285:
          compareClass = Operators.CompareDecimal(left, (object) Decimal.Zero);
          break;
        case (TypeCode) 288:
          compareClass = Operators.CompareDecimal(left, (object) Operators.ToVBBool(right));
          break;
        case (TypeCode) 304:
          compareClass = Operators.CompareDate(Convert.ToDateTime(left), DateTime.MinValue);
          break;
        case (TypeCode) 320:
          compareClass = Operators.CompareDate(Convert.ToDateTime(left), Convert.ToDateTime(right));
          break;
        case (TypeCode) 322:
          compareClass = Operators.CompareDate(Convert.ToDateTime(left), Conversions.ToDate(Convert.ToString(right)));
          break;
        case (TypeCode) 342:
          compareClass = (Operators.CompareClass) Operators.CompareString(Convert.ToString(left), (string) null, textCompare);
          break;
        case (TypeCode) 345:
          compareClass = Operators.CompareBoolean(Conversions.ToBoolean(Convert.ToString(left)), Convert.ToBoolean(right));
          break;
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          compareClass = Operators.CompareDouble(Conversions.ToDouble(Convert.ToString(left)), Convert.ToDouble(right));
          break;
        case (TypeCode) 358:
          compareClass = Operators.CompareDate(Conversions.ToDate(Convert.ToString(left)), Convert.ToDateTime(right));
          break;
        default:
          compareClass = typeCode1 == TypeCode.Object || typeCode2 == TypeCode.Object ? Operators.CompareClass.UserDefined : Operators.CompareClass.Undefined;
          break;
      }
      return compareClass;
    }

    private static Operators.CompareClass CompareBoolean(bool left, bool right)
    {
      return left != right ? ((left ? 1 : 0) >= (right ? 1 : 0) ? Operators.CompareClass.Less : Operators.CompareClass.Greater) : Operators.CompareClass.Equal;
    }

    private static Operators.CompareClass CompareInt32(int left, int right)
    {
      return left != right ? (left <= right ? Operators.CompareClass.Less : Operators.CompareClass.Greater) : Operators.CompareClass.Equal;
    }

    private static Operators.CompareClass CompareUInt32(uint left, uint right)
    {
      return (int) left != (int) right ? (left <= right ? Operators.CompareClass.Less : Operators.CompareClass.Greater) : Operators.CompareClass.Equal;
    }

    private static Operators.CompareClass CompareInt64(long left, long right)
    {
      return left != right ? (left <= right ? Operators.CompareClass.Less : Operators.CompareClass.Greater) : Operators.CompareClass.Equal;
    }

    private static Operators.CompareClass CompareUInt64(ulong left, ulong right)
    {
      return (long) left != (long) right ? (left <= right ? Operators.CompareClass.Less : Operators.CompareClass.Greater) : Operators.CompareClass.Equal;
    }

    private static Operators.CompareClass CompareDecimal(object left, object right)
    {
      int num = Decimal.Compare(Convert.ToDecimal(left), Convert.ToDecimal(right));
      return num != 0 ? (num <= 0 ? Operators.CompareClass.Less : Operators.CompareClass.Greater) : Operators.CompareClass.Equal;
    }

    private static Operators.CompareClass CompareSingle(float left, float right)
    {
      return (double) left != (double) right ? ((double) left >= (double) right ? ((double) left <= (double) right ? Operators.CompareClass.Unordered : Operators.CompareClass.Greater) : Operators.CompareClass.Less) : Operators.CompareClass.Equal;
    }

    private static Operators.CompareClass CompareDouble(double left, double right)
    {
      return left != right ? (left >= right ? (left <= right ? Operators.CompareClass.Unordered : Operators.CompareClass.Greater) : Operators.CompareClass.Less) : Operators.CompareClass.Equal;
    }

    private static Operators.CompareClass CompareDate(DateTime left, DateTime right)
    {
      int num = DateTime.Compare(left, right);
      return num != 0 ? (num <= 0 ? Operators.CompareClass.Less : Operators.CompareClass.Greater) : Operators.CompareClass.Equal;
    }

    private static Operators.CompareClass CompareChar(char left, char right)
    {
      return (int) left != (int) right ? ((int) left <= (int) right ? Operators.CompareClass.Less : Operators.CompareClass.Greater) : Operators.CompareClass.Equal;
    }

    /// <summary>Performs binary or text string comparison when given two strings.</summary>
    /// <param name="Left">Required. Any <see langword="String" /> expression.</param>
    /// <param name="Right">Required. Any <see langword="String" /> expression.</param>
    /// <param name="TextCompare">Required. <see langword="True" /> to perform a case-insensitive string comparison; otherwise, <see langword="False" />.</param>
    /// <returns>Value
    /// 
    /// Condition
    /// 
    /// -1
    /// 
    /// 
    ///               <paramref name="Left" /> is less than <paramref name="Right" />.
    /// 
    /// 0
    /// 
    /// 
    ///               <paramref name="Left" /> is equal to <paramref name="Right" />.
    /// 
    /// 1
    /// 
    /// 
    ///               <paramref name="Left" /> is greater than <paramref name="Right" />.</returns>
    public static int CompareString(string Left, string Right, bool TextCompare)
    {
      int num1;
      if ((object) Left == (object) Right)
        num1 = 0;
      else if (Left == null)
        num1 = Right.Length != 0 ? -1 : 0;
      else if (Right == null)
      {
        num1 = Left.Length != 0 ? 1 : 0;
      }
      else
      {
        int num2 = !TextCompare ? string.CompareOrdinal(Left, Right) : Utils.GetCultureInfo().CompareInfo.Compare(Left, Right, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
        num1 = num2 != 0 ? (num2 <= 0 ? -1 : 1) : 0;
      }
      return num1;
    }

    /// <summary>Represents the Visual Basic unary plus (+) operator.</summary>
    /// <param name="Operand">Required. Any numeric expression.</param>
    /// <returns>The value of <paramref name="Operand" />. (The sign of the <paramref name="Operand" /> is unchanged.)</returns>
    public static object PlusObject(object Operand)
    {
      if (Operand == null)
        return Operators.Boxed_ZeroInteger;
      switch (Operators.GetTypeCode(Operand))
      {
        case TypeCode.Empty:
          return Operators.Boxed_ZeroInteger;
        case TypeCode.Object:
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.UnaryPlus, Operand);
        case TypeCode.Boolean:
          return (object) (short) -(Convert.ToBoolean(Operand) ? 1 : 0);
        case TypeCode.SByte:
          return (object) Convert.ToSByte(Operand);
        case TypeCode.Byte:
          return (object) Convert.ToByte(Operand);
        case TypeCode.Int16:
          return (object) Convert.ToInt16(Operand);
        case TypeCode.UInt16:
          return (object) Convert.ToUInt16(Operand);
        case TypeCode.Int32:
          return (object) Convert.ToInt32(Operand);
        case TypeCode.UInt32:
          return (object) Convert.ToUInt32(Operand);
        case TypeCode.Int64:
          return (object) Convert.ToInt64(Operand);
        case TypeCode.UInt64:
          return (object) Convert.ToUInt64(Operand);
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return Operand;
        case TypeCode.String:
          return (object) Conversions.ToDouble(Convert.ToString(Operand));
        default:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.UnaryPlus, Operand);
      }
    }

    /// <summary>Represents the Visual Basic unary minus (-) operator.</summary>
    /// <param name="Operand">Required. Any numeric expression.</param>
    /// <returns>The negative value of <paramref name="Operand" />.</returns>
    public static object NegateObject(object Operand)
    {
      Operators.GetTypeCode(Operand);
      switch (Operand != null ? (int) Operand.GetType().GetTypeCode() : 0)
      {
        case 0:
          return Operators.Boxed_ZeroInteger;
        case 1:
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Negate, Operand);
        case 3:
          return Operand is bool operand ? Operators.NegateBoolean(operand) : Operators.NegateBoolean(Convert.ToBoolean(Operand));
        case 5:
          return Operand is sbyte operand ? Operators.NegateSByte(operand) : Operators.NegateSByte(Convert.ToSByte(Operand));
        case 6:
          return Operand is byte operand ? Operators.NegateByte(operand) : Operators.NegateByte(Convert.ToByte(Operand));
        case 7:
          return Operand is short operand ? Operators.NegateInt16(operand) : Operators.NegateInt16(Convert.ToInt16(Operand));
        case 8:
          return Operand is ushort operand ? Operators.NegateUInt16(operand) : Operators.NegateUInt16(Convert.ToUInt16(Operand));
        case 9:
          return Operand is int operand ? Operators.NegateInt32(operand) : Operators.NegateInt32(Convert.ToInt32(Operand));
        case 10:
          return Operand is uint operand ? Operators.NegateUInt32(operand) : Operators.NegateUInt32(Convert.ToUInt32(Operand));
        case 11:
          return Operand is long operand ? Operators.NegateInt64(operand) : Operators.NegateInt64(Convert.ToInt64(Operand));
        case 12:
          return Operand is ulong operand ? Operators.NegateUInt64(operand) : Operators.NegateUInt64(Convert.ToUInt64(Operand));
        case 13:
          return Operand is float operand ? Operators.NegateSingle(operand) : Operators.NegateSingle(Convert.ToSingle(Operand));
        case 14:
          return Operand is double operand ? Operators.NegateDouble(operand) : Operators.NegateDouble(Convert.ToDouble(Operand));
        case 15:
          return Operand is Decimal operand ? Operators.NegateDecimal(operand) : Operators.NegateDecimal(Convert.ToDecimal(Operand));
        case 18:
          return Operand is string operand ? Operators.NegateString(operand) : Operators.NegateString(Convert.ToString(Operand));
        default:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Negate, Operand);
      }
    }

    private static object NegateBoolean(bool operand)
    {
      return (object) -(short) -(operand ? 1 : 0);
    }

    private static object NegateSByte(sbyte operand)
    {
      return operand != sbyte.MinValue ? (object) -operand : (object) (short) 128;
    }

    private static object NegateByte(byte operand)
    {
      return (object) (short) -operand;
    }

    private static object NegateInt16(short operand)
    {
      return operand != short.MinValue ? (object) -operand : (object) 32768;
    }

    private static object NegateUInt16(ushort operand)
    {
      return (object) (int) checked (-operand);
    }

    private static object NegateInt32(int operand)
    {
      return operand != int.MinValue ? (object) checked (-operand) : (object) 2147483648L;
    }

    private static object NegateUInt32(uint operand)
    {
      return (object) (long) checked (-operand);
    }

    private static object NegateInt64(long operand)
    {
      return operand != long.MinValue ? (object) checked (-operand) : (object) new Decimal(0, int.MinValue, 0, false, (byte) 0);
    }

    private static object NegateUInt64(ulong operand)
    {
      return (object) Decimal.Negate(new Decimal(operand));
    }

    private static object NegateDecimal(Decimal operand)
    {
      try
      {
        return (object) Decimal.Negate(operand);
      }
      catch (OverflowException ex)
      {
        return (object) -Convert.ToDouble(operand);
      }
    }

    private static object NegateSingle(float operand)
    {
      return (object) (float) -(double) operand;
    }

    private static object NegateDouble(double operand)
    {
      return (object) -operand;
    }

    private static object NegateString(string operand)
    {
      return (object) -Conversions.ToDouble(operand);
    }

    /// <summary>Represents the Visual Basic <see langword="Not" /> operator.</summary>
    /// <param name="Operand">Required. Any <see langword="Boolean" /> or numeric expression.</param>
    /// <returns>For <see langword="Boolean" /> operations, <see langword="False" /> if <paramref name="Operand" /> is <see langword="True" />; otherwise, <see langword="True" />. For bitwise operations, 1 if <paramref name="Operand" /> is 0; otherwise, 0.</returns>
    public static object NotObject(object Operand)
    {
      switch (Operators.GetTypeCode(Operand))
      {
        case TypeCode.Empty:
          return (object) -1;
        case TypeCode.Object:
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Not, Operand);
        case TypeCode.Boolean:
          return Operators.NotBoolean(Convert.ToBoolean(Operand));
        case TypeCode.SByte:
          return Operators.NotSByte(Convert.ToSByte(Operand), Operand.GetType());
        case TypeCode.Byte:
          return Operators.NotByte(Convert.ToByte(Operand), Operand.GetType());
        case TypeCode.Int16:
          return Operators.NotInt16(Convert.ToInt16(Operand), Operand.GetType());
        case TypeCode.UInt16:
          return Operators.NotUInt16(Convert.ToUInt16(Operand), Operand.GetType());
        case TypeCode.Int32:
          return Operators.NotInt32(Convert.ToInt32(Operand), Operand.GetType());
        case TypeCode.UInt32:
          return Operators.NotUInt32(Convert.ToUInt32(Operand), Operand.GetType());
        case TypeCode.Int64:
          return Operators.NotInt64(Convert.ToInt64(Operand), Operand.GetType());
        case TypeCode.UInt64:
          return Operators.NotUInt64(Convert.ToUInt64(Operand), Operand.GetType());
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return Operators.NotInt64(Convert.ToInt64(Operand));
        case TypeCode.String:
          return Operators.NotInt64(Conversions.ToLong(Convert.ToString(Operand)));
        default:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Not, Operand);
      }
    }

    private static object NotBoolean(bool operand)
    {
      return (object) !operand;
    }

    private static object NotSByte(sbyte operand, Type operandType)
    {
      sbyte num = ~operand;
      return !operandType.IsEnum ? (object) num : Enum.ToObject(operandType, num);
    }

    private static object NotByte(byte operand, Type operandType)
    {
      byte num = ~operand;
      return !operandType.IsEnum ? (object) num : Enum.ToObject(operandType, num);
    }

    private static object NotInt16(short operand, Type operandType)
    {
      short num = ~operand;
      return !operandType.IsEnum ? (object) num : Enum.ToObject(operandType, num);
    }

    private static object NotUInt16(ushort operand, Type operandType)
    {
      ushort num = ~operand;
      return !operandType.IsEnum ? (object) num : Enum.ToObject(operandType, num);
    }

    private static object NotInt32(int operand, Type operandType)
    {
      int num = ~operand;
      return !operandType.IsEnum ? (object) num : Enum.ToObject(operandType, num);
    }

    private static object NotUInt32(uint operand, Type operandType)
    {
      uint num = ~operand;
      return !operandType.IsEnum ? (object) num : Enum.ToObject(operandType, num);
    }

    private static object NotInt64(long operand)
    {
      return (object) ~operand;
    }

    private static object NotInt64(long operand, Type operandType)
    {
      long num = ~operand;
      return !operandType.IsEnum ? (object) num : Enum.ToObject(operandType, num);
    }

    private static object NotUInt64(ulong operand, Type operandType)
    {
      ulong num = ~operand;
      return !operandType.IsEnum ? (object) num : Enum.ToObject(operandType, num);
    }

    /// <summary>Represents the Visual Basic <see langword="And" /> operator.</summary>
    /// <param name="Left">Required. Any <see langword="Boolean" /> or numeric expression.</param>
    /// <param name="Right">Required. Any <see langword="Boolean" /> or numeric expression.</param>
    /// <returns>For <see langword="Boolean" /> operations, <see langword="True" /> if both <paramref name="Left" /> and <paramref name="Right" /> evaluate to <see langword="True" />; otherwise, <see langword="False" />. For bitwise operations, 1 if both <paramref name="Left" /> and <paramref name="Right" /> evaluate to 1; otherwise, 0.</returns>
    public static object AndObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
          return Operators.Boxed_ZeroInteger;
        case TypeCode.Boolean:
        case (TypeCode) 57:
          return (object) false;
        case TypeCode.SByte:
        case (TypeCode) 95:
          return Operators.AndSByte((sbyte) 0, (sbyte) 0, Operators.GetEnumResult(Left, Right));
        case TypeCode.Byte:
        case (TypeCode) 114:
          return Operators.AndByte((byte) 0, (byte) 0, Operators.GetEnumResult(Left, Right));
        case TypeCode.Int16:
        case (TypeCode) 133:
          return Operators.AndInt16((short) 0, (short) 0, Operators.GetEnumResult(Left, Right));
        case TypeCode.UInt16:
        case (TypeCode) 152:
          return Operators.AndUInt16((ushort) 0, (ushort) 0, Operators.GetEnumResult(Left, Right));
        case TypeCode.Int32:
        case (TypeCode) 171:
          return Operators.AndInt32(0, 0, Operators.GetEnumResult(Left, Right));
        case TypeCode.UInt32:
        case (TypeCode) 190:
          return Operators.AndUInt32(0U, 0U, Operators.GetEnumResult(Left, Right));
        case TypeCode.Int64:
        case (TypeCode) 209:
          return Operators.AndInt64(0L, 0L, Operators.GetEnumResult(Left, Right));
        case TypeCode.UInt64:
        case (TypeCode) 228:
          return Operators.AndUInt64(0UL, 0UL, Operators.GetEnumResult(Left, Right));
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return Operators.AndInt64(0L, Convert.ToInt64(Right), (Type) null);
        case TypeCode.String:
          return Operators.AndInt64(0L, Conversions.ToLong(Convert.ToString(Right)), (Type) null);
        case (TypeCode) 60:
          return Operators.AndBoolean(Convert.ToBoolean(Left), Convert.ToBoolean(Right));
        case (TypeCode) 62:
          return Operators.AndSByte(Operators.ToVBBool(Left), Convert.ToSByte(Right), (Type) null);
        case (TypeCode) 63:
        case (TypeCode) 64:
          return Operators.AndInt16((short) Operators.ToVBBool(Left), Convert.ToInt16(Right), (Type) null);
        case (TypeCode) 65:
        case (TypeCode) 66:
          return Operators.AndInt32((int) Operators.ToVBBool(Left), Convert.ToInt32(Right), (Type) null);
        case (TypeCode) 67:
        case (TypeCode) 68:
        case (TypeCode) 69:
        case (TypeCode) 70:
        case (TypeCode) 71:
        case (TypeCode) 72:
          return Operators.AndInt64((long) Operators.ToVBBool(Left), Convert.ToInt64(Right), (Type) null);
        case (TypeCode) 75:
          return Operators.AndBoolean(Convert.ToBoolean(Left), Conversions.ToBoolean(Convert.ToString(Right)));
        case (TypeCode) 98:
          return Operators.AndSByte(Convert.ToSByte(Left), Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 100:
          return Operators.AndSByte(Convert.ToSByte(Left), Convert.ToSByte(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 119:
        case (TypeCode) 121:
        case (TypeCode) 138:
        case (TypeCode) 139:
          return Operators.AndInt16(Convert.ToInt16(Left), Convert.ToInt16(Right), (Type) null);
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 123:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 157:
        case (TypeCode) 159:
        case (TypeCode) 161:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
          return Operators.AndInt32(Convert.ToInt32(Left), Convert.ToInt32(Right), (Type) null);
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 107:
        case (TypeCode) 108:
        case (TypeCode) 109:
        case (TypeCode) 110:
        case (TypeCode) 125:
        case (TypeCode) 127:
        case (TypeCode) 128:
        case (TypeCode) 129:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 145:
        case (TypeCode) 146:
        case (TypeCode) 147:
        case (TypeCode) 148:
        case (TypeCode) 163:
        case (TypeCode) 165:
        case (TypeCode) 166:
        case (TypeCode) 167:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 183:
        case (TypeCode) 184:
        case (TypeCode) 185:
        case (TypeCode) 186:
        case (TypeCode) 195:
        case (TypeCode) 197:
        case (TypeCode) 199:
        case (TypeCode) 201:
        case (TypeCode) 203:
        case (TypeCode) 204:
        case (TypeCode) 205:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 221:
        case (TypeCode) 222:
        case (TypeCode) 223:
        case (TypeCode) 224:
        case (TypeCode) 233:
        case (TypeCode) 235:
        case (TypeCode) 237:
        case (TypeCode) 239:
        case (TypeCode) 241:
        case (TypeCode) 242:
        case (TypeCode) 243:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 261:
        case (TypeCode) 262:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 298:
        case (TypeCode) 299:
        case (TypeCode) 300:
          return Operators.AndInt64(Convert.ToInt64(Left), Convert.ToInt64(Right), (Type) null);
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          return Operators.AndInt64(Convert.ToInt64(Left), Conversions.ToLong(Convert.ToString(Right)), (Type) null);
        case (TypeCode) 117:
        case (TypeCode) 136:
          return Operators.AndInt16(Convert.ToInt16(Left), (short) Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 120:
          return Operators.AndByte(Convert.ToByte(Left), Convert.ToByte(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 122:
        case (TypeCode) 158:
          return Operators.AndUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right), (Type) null);
        case (TypeCode) 124:
        case (TypeCode) 162:
        case (TypeCode) 196:
        case (TypeCode) 198:
          return Operators.AndUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right), (Type) null);
        case (TypeCode) 126:
        case (TypeCode) 164:
        case (TypeCode) 202:
        case (TypeCode) 234:
        case (TypeCode) 236:
        case (TypeCode) 238:
          return Operators.AndUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right), (Type) null);
        case (TypeCode) 140:
          return Operators.AndInt16(Convert.ToInt16(Left), Convert.ToInt16(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 155:
        case (TypeCode) 174:
          return Operators.AndInt32(Convert.ToInt32(Left), (int) Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 160:
          return Operators.AndUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 180:
          return Operators.AndInt32(Convert.ToInt32(Left), Convert.ToInt32(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 193:
        case (TypeCode) 212:
        case (TypeCode) 231:
        case (TypeCode) 250:
        case (TypeCode) 269:
        case (TypeCode) 288:
          return Operators.AndInt64(Convert.ToInt64(Left), (long) Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 200:
          return Operators.AndUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 220:
          return Operators.AndInt64(Convert.ToInt64(Left), Convert.ToInt64(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 240:
          return Operators.AndUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 247:
        case (TypeCode) 266:
        case (TypeCode) 285:
          return Operators.AndInt64(Convert.ToInt64(Left), 0L, (Type) null);
        case (TypeCode) 342:
          return Operators.AndInt64(Conversions.ToLong(Convert.ToString(Left)), 0L, (Type) null);
        case (TypeCode) 345:
          return Operators.AndBoolean(Conversions.ToBoolean(Convert.ToString(Left)), Convert.ToBoolean(Right));
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          return Operators.AndInt64(Conversions.ToLong(Convert.ToString(Left)), Convert.ToInt64(Right), (Type) null);
        case (TypeCode) 360:
          return Operators.AndInt64(Conversions.ToLong(Convert.ToString(Left)), Conversions.ToLong(Convert.ToString(Right)), (Type) null);
        default:
          if (typeCode1 != TypeCode.Object && typeCode2 != TypeCode.Object)
            throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.And, Left, Right);
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.And, Left, Right);
      }
    }

    private static object AndBoolean(bool left, bool right)
    {
      return (object) (left & right);
    }

    private static object AndSByte(sbyte left, sbyte right, Type enumType = null)
    {
      sbyte num = (sbyte) ((int) left & (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object AndByte(byte left, byte right, Type enumType = null)
    {
      byte num = (byte) ((int) left & (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object AndInt16(short left, short right, Type enumType = null)
    {
      short num = (short) ((int) left & (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object AndUInt16(ushort left, ushort right, Type enumType = null)
    {
      ushort num = (ushort) ((int) left & (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object AndInt32(int left, int right, Type enumType = null)
    {
      int num = left & right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object AndUInt32(uint left, uint right, Type enumType = null)
    {
      uint num = left & right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object AndInt64(long left, long right, Type enumType = null)
    {
      long num = left & right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object AndUInt64(ulong left, ulong right, Type enumType = null)
    {
      ulong num = left & right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    /// <summary>Represents the Visual Basic <see langword="Or" /> operator.</summary>
    /// <param name="Left">Required. Any <see langword="Boolean" /> or numeric expression.</param>
    /// <param name="Right">Required. Any <see langword="Boolean" /> or numeric expression.</param>
    /// <returns>For <see langword="Boolean" /> operations, <see langword="False" /> if both <paramref name="Left" /> and <paramref name="Right" /> evaluate to <see langword="False" />; otherwise, <see langword="True" />. For bitwise operations, 0 if both <paramref name="Left" /> and <paramref name="Right" /> evaluate to 0; otherwise, 1.</returns>
    public static object OrObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
          return Operators.Boxed_ZeroInteger;
        case TypeCode.Boolean:
          return Operators.OrBoolean(false, Convert.ToBoolean(Right));
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return Right;
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return Operators.OrInt64(0L, Convert.ToInt64(Right), (Type) null);
        case TypeCode.String:
          return Operators.OrInt64(0L, Conversions.ToLong(Convert.ToString(Right)), (Type) null);
        case (TypeCode) 57:
          return Operators.OrBoolean(Convert.ToBoolean(Left), false);
        case (TypeCode) 60:
          return Operators.OrBoolean(Convert.ToBoolean(Left), Convert.ToBoolean(Right));
        case (TypeCode) 62:
          return Operators.OrSByte(Operators.ToVBBool(Left), Convert.ToSByte(Right), (Type) null);
        case (TypeCode) 63:
        case (TypeCode) 64:
          return Operators.OrInt16((short) Operators.ToVBBool(Left), Convert.ToInt16(Right), (Type) null);
        case (TypeCode) 65:
        case (TypeCode) 66:
          return Operators.OrInt32((int) Operators.ToVBBool(Left), Convert.ToInt32(Right), (Type) null);
        case (TypeCode) 67:
        case (TypeCode) 68:
        case (TypeCode) 69:
        case (TypeCode) 70:
        case (TypeCode) 71:
        case (TypeCode) 72:
          return Operators.OrInt64((long) Operators.ToVBBool(Left), Convert.ToInt64(Right), (Type) null);
        case (TypeCode) 75:
          return Operators.OrBoolean(Convert.ToBoolean(Left), Conversions.ToBoolean(Convert.ToString(Right)));
        case (TypeCode) 95:
        case (TypeCode) 114:
        case (TypeCode) 133:
        case (TypeCode) 152:
        case (TypeCode) 171:
        case (TypeCode) 190:
        case (TypeCode) 209:
        case (TypeCode) 228:
          return Left;
        case (TypeCode) 98:
          return Operators.OrSByte(Convert.ToSByte(Left), Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 100:
          return Operators.OrSByte(Convert.ToSByte(Left), Convert.ToSByte(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 119:
        case (TypeCode) 121:
        case (TypeCode) 138:
        case (TypeCode) 139:
          return Operators.OrInt16(Convert.ToInt16(Left), Convert.ToInt16(Right), (Type) null);
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 123:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 157:
        case (TypeCode) 159:
        case (TypeCode) 161:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
          return Operators.OrInt32(Convert.ToInt32(Left), Convert.ToInt32(Right), (Type) null);
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 107:
        case (TypeCode) 108:
        case (TypeCode) 109:
        case (TypeCode) 110:
        case (TypeCode) 125:
        case (TypeCode) 127:
        case (TypeCode) 128:
        case (TypeCode) 129:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 145:
        case (TypeCode) 146:
        case (TypeCode) 147:
        case (TypeCode) 148:
        case (TypeCode) 163:
        case (TypeCode) 165:
        case (TypeCode) 166:
        case (TypeCode) 167:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 183:
        case (TypeCode) 184:
        case (TypeCode) 185:
        case (TypeCode) 186:
        case (TypeCode) 195:
        case (TypeCode) 197:
        case (TypeCode) 199:
        case (TypeCode) 201:
        case (TypeCode) 203:
        case (TypeCode) 204:
        case (TypeCode) 205:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 221:
        case (TypeCode) 222:
        case (TypeCode) 223:
        case (TypeCode) 224:
        case (TypeCode) 233:
        case (TypeCode) 235:
        case (TypeCode) 237:
        case (TypeCode) 239:
        case (TypeCode) 241:
        case (TypeCode) 242:
        case (TypeCode) 243:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 261:
        case (TypeCode) 262:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 298:
        case (TypeCode) 299:
        case (TypeCode) 300:
          return Operators.OrInt64(Convert.ToInt64(Left), Convert.ToInt64(Right), (Type) null);
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          return Operators.OrInt64(Convert.ToInt64(Left), Conversions.ToLong(Convert.ToString(Right)), (Type) null);
        case (TypeCode) 117:
        case (TypeCode) 136:
          return Operators.OrInt16(Convert.ToInt16(Left), (short) Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 120:
          return Operators.OrByte(Convert.ToByte(Left), Convert.ToByte(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 122:
        case (TypeCode) 158:
          return Operators.OrUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right), (Type) null);
        case (TypeCode) 124:
        case (TypeCode) 162:
        case (TypeCode) 196:
        case (TypeCode) 198:
          return Operators.OrUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right), (Type) null);
        case (TypeCode) 126:
        case (TypeCode) 164:
        case (TypeCode) 202:
        case (TypeCode) 234:
        case (TypeCode) 236:
        case (TypeCode) 238:
          return Operators.OrUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right), (Type) null);
        case (TypeCode) 140:
          return Operators.OrInt16(Convert.ToInt16(Left), Convert.ToInt16(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 155:
        case (TypeCode) 174:
          return Operators.OrInt32(Convert.ToInt32(Left), (int) Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 160:
          return Operators.OrUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 180:
          return Operators.OrInt32(Convert.ToInt32(Left), Convert.ToInt32(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 193:
        case (TypeCode) 212:
        case (TypeCode) 231:
        case (TypeCode) 250:
        case (TypeCode) 269:
        case (TypeCode) 288:
          return Operators.OrInt64(Convert.ToInt64(Left), (long) Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 200:
          return Operators.OrUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 220:
          return Operators.OrInt64(Convert.ToInt64(Left), Convert.ToInt64(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 240:
          return Operators.OrUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 247:
        case (TypeCode) 266:
        case (TypeCode) 285:
          return Operators.OrInt64(Convert.ToInt64(Left), 0L, (Type) null);
        case (TypeCode) 342:
          return Operators.OrInt64(Conversions.ToLong(Convert.ToString(Left)), 0L, (Type) null);
        case (TypeCode) 345:
          return Operators.OrBoolean(Conversions.ToBoolean(Convert.ToString(Left)), Convert.ToBoolean(Right));
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          return Operators.OrInt64(Conversions.ToLong(Convert.ToString(Left)), Convert.ToInt64(Right), (Type) null);
        case (TypeCode) 360:
          return Operators.OrInt64(Conversions.ToLong(Convert.ToString(Left)), Conversions.ToLong(Convert.ToString(Right)), (Type) null);
        default:
          if (typeCode1 != TypeCode.Object && typeCode2 != TypeCode.Object)
            throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Or, Left, Right);
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Or, Left, Right);
      }
    }

    private static object OrBoolean(bool left, bool right)
    {
      return (object) (left | right);
    }

    private static object OrSByte(sbyte left, sbyte right, Type enumType = null)
    {
      sbyte num = (sbyte) ((int) left | (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object OrByte(byte left, byte right, Type enumType = null)
    {
      byte num = (byte) ((int) left | (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object OrInt16(short left, short right, Type enumType = null)
    {
      short num = (short) ((int) left | (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object OrUInt16(ushort left, ushort right, Type enumType = null)
    {
      ushort num = (ushort) ((int) left | (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object OrInt32(int left, int right, Type enumType = null)
    {
      int num = left | right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object OrUInt32(uint left, uint right, Type enumType = null)
    {
      uint num = left | right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object OrInt64(long left, long right, Type enumType = null)
    {
      long num = left | right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object OrUInt64(ulong left, ulong right, Type enumType = null)
    {
      ulong num = left | right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    /// <summary>Represents the Visual Basic <see langword="Xor" /> operator.</summary>
    /// <param name="Left">Required. Any <see langword="Boolean" /> or numeric expression.</param>
    /// <param name="Right">Required. Any <see langword="Boolean" /> or numeric expression.</param>
    /// <returns>A <see langword="Boolean" /> or numeric value. For a <see langword="Boolean" /> comparison, the return value is the logical exclusion (exclusive logical disjunction) of two <see langword="Boolean" /> values. For bitwise (numeric) operations, the return value is a numeric value that represents the bitwise exclusion (exclusive bitwise disjunction) of two numeric bit patterns. For more information, see Xor Operator.</returns>
    public static object XorObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
          return Operators.Boxed_ZeroInteger;
        case TypeCode.Boolean:
          return Operators.XorBoolean(false, Convert.ToBoolean(Right));
        case TypeCode.SByte:
          return Operators.XorSByte((sbyte) 0, Convert.ToSByte(Right), Operators.GetEnumResult(Left, Right));
        case TypeCode.Byte:
          return Operators.XorByte((byte) 0, Convert.ToByte(Right), Operators.GetEnumResult(Left, Right));
        case TypeCode.Int16:
          return Operators.XorInt16((short) 0, Convert.ToInt16(Right), Operators.GetEnumResult(Left, Right));
        case TypeCode.UInt16:
          return Operators.XorUInt16((ushort) 0, Convert.ToUInt16(Right), Operators.GetEnumResult(Left, Right));
        case TypeCode.Int32:
          return Operators.XorInt32(0, Convert.ToInt32(Right), Operators.GetEnumResult(Left, Right));
        case TypeCode.UInt32:
          return Operators.XorUInt32(0U, Convert.ToUInt32(Right), Operators.GetEnumResult(Left, Right));
        case TypeCode.Int64:
          return Operators.XorInt64(0L, Convert.ToInt64(Right), Operators.GetEnumResult(Left, Right));
        case TypeCode.UInt64:
          return Operators.XorUInt64(0UL, Convert.ToUInt64(Right), Operators.GetEnumResult(Left, Right));
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return Operators.XorInt64(0L, Convert.ToInt64(Right), (Type) null);
        case TypeCode.String:
          return Operators.XorInt64(0L, Conversions.ToLong(Convert.ToString(Right)), (Type) null);
        case (TypeCode) 57:
          return Operators.XorBoolean(Convert.ToBoolean(Left), false);
        case (TypeCode) 60:
          return Operators.XorBoolean(Convert.ToBoolean(Left), Convert.ToBoolean(Right));
        case (TypeCode) 62:
          return Operators.XorSByte(Operators.ToVBBool(Left), Convert.ToSByte(Right), (Type) null);
        case (TypeCode) 63:
        case (TypeCode) 64:
          return Operators.XorInt16((short) Operators.ToVBBool(Left), Convert.ToInt16(Right), (Type) null);
        case (TypeCode) 65:
        case (TypeCode) 66:
          return Operators.XorInt32((int) Operators.ToVBBool(Left), Convert.ToInt32(Right), (Type) null);
        case (TypeCode) 67:
        case (TypeCode) 68:
        case (TypeCode) 69:
        case (TypeCode) 70:
        case (TypeCode) 71:
        case (TypeCode) 72:
          return Operators.XorInt64((long) Operators.ToVBBool(Left), Convert.ToInt64(Right), (Type) null);
        case (TypeCode) 75:
          return Operators.XorBoolean(Convert.ToBoolean(Left), Conversions.ToBoolean(Convert.ToString(Right)));
        case (TypeCode) 95:
          return Operators.XorSByte(Convert.ToSByte(Left), (sbyte) 0, Operators.GetEnumResult(Left, Right));
        case (TypeCode) 98:
          return Operators.XorSByte(Convert.ToSByte(Left), Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 100:
          return Operators.XorSByte(Convert.ToSByte(Left), Convert.ToSByte(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 119:
        case (TypeCode) 121:
        case (TypeCode) 138:
        case (TypeCode) 139:
          return Operators.XorInt16(Convert.ToInt16(Left), Convert.ToInt16(Right), (Type) null);
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 123:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 157:
        case (TypeCode) 159:
        case (TypeCode) 161:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
          return Operators.XorInt32(Convert.ToInt32(Left), Convert.ToInt32(Right), (Type) null);
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 107:
        case (TypeCode) 108:
        case (TypeCode) 109:
        case (TypeCode) 110:
        case (TypeCode) 125:
        case (TypeCode) 127:
        case (TypeCode) 128:
        case (TypeCode) 129:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 145:
        case (TypeCode) 146:
        case (TypeCode) 147:
        case (TypeCode) 148:
        case (TypeCode) 163:
        case (TypeCode) 165:
        case (TypeCode) 166:
        case (TypeCode) 167:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 183:
        case (TypeCode) 184:
        case (TypeCode) 185:
        case (TypeCode) 186:
        case (TypeCode) 195:
        case (TypeCode) 197:
        case (TypeCode) 199:
        case (TypeCode) 201:
        case (TypeCode) 203:
        case (TypeCode) 204:
        case (TypeCode) 205:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 221:
        case (TypeCode) 222:
        case (TypeCode) 223:
        case (TypeCode) 224:
        case (TypeCode) 233:
        case (TypeCode) 235:
        case (TypeCode) 237:
        case (TypeCode) 239:
        case (TypeCode) 241:
        case (TypeCode) 242:
        case (TypeCode) 243:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 261:
        case (TypeCode) 262:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 298:
        case (TypeCode) 299:
        case (TypeCode) 300:
          return Operators.XorInt64(Convert.ToInt64(Left), Convert.ToInt64(Right), (Type) null);
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          return Operators.XorInt64(Convert.ToInt64(Left), Conversions.ToLong(Convert.ToString(Right)), (Type) null);
        case (TypeCode) 114:
          return Operators.XorByte(Convert.ToByte(Left), (byte) 0, Operators.GetEnumResult(Left, Right));
        case (TypeCode) 117:
        case (TypeCode) 136:
          return Operators.XorInt16(Convert.ToInt16(Left), (short) Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 120:
          return Operators.XorByte(Convert.ToByte(Left), Convert.ToByte(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 122:
        case (TypeCode) 158:
          return Operators.XorUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right), (Type) null);
        case (TypeCode) 124:
        case (TypeCode) 162:
        case (TypeCode) 196:
        case (TypeCode) 198:
          return Operators.XorUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right), (Type) null);
        case (TypeCode) 126:
        case (TypeCode) 164:
        case (TypeCode) 202:
        case (TypeCode) 234:
        case (TypeCode) 236:
        case (TypeCode) 238:
          return Operators.XorUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right), (Type) null);
        case (TypeCode) 133:
          return Operators.XorInt16(Convert.ToInt16(Left), (short) 0, Operators.GetEnumResult(Left, Right));
        case (TypeCode) 140:
          return Operators.XorInt16(Convert.ToInt16(Left), Convert.ToInt16(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 152:
          return Operators.XorUInt16(Convert.ToUInt16(Left), (ushort) 0, Operators.GetEnumResult(Left, Right));
        case (TypeCode) 155:
        case (TypeCode) 174:
          return Operators.XorInt32(Convert.ToInt32(Left), (int) Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 160:
          return Operators.XorUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 171:
          return Operators.XorInt32(Convert.ToInt32(Left), 0, Operators.GetEnumResult(Left, Right));
        case (TypeCode) 180:
          return Operators.XorInt32(Convert.ToInt32(Left), Convert.ToInt32(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 190:
          return Operators.XorUInt32(Convert.ToUInt32(Left), 0U, Operators.GetEnumResult(Left, Right));
        case (TypeCode) 193:
        case (TypeCode) 212:
        case (TypeCode) 231:
        case (TypeCode) 250:
        case (TypeCode) 269:
        case (TypeCode) 288:
          return Operators.XorInt64(Convert.ToInt64(Left), (long) Operators.ToVBBool(Right), (Type) null);
        case (TypeCode) 200:
          return Operators.XorUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 209:
          return Operators.XorInt64(Convert.ToInt64(Left), 0L, Operators.GetEnumResult(Left, Right));
        case (TypeCode) 220:
          return Operators.XorInt64(Convert.ToInt64(Left), Convert.ToInt64(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 228:
          return Operators.XorUInt64(Convert.ToUInt64(Left), 0UL, Operators.GetEnumResult(Left, Right));
        case (TypeCode) 240:
          return Operators.XorUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right), Operators.GetEnumResult(Left, Right));
        case (TypeCode) 247:
        case (TypeCode) 266:
        case (TypeCode) 285:
          return Operators.XorInt64(Convert.ToInt64(Left), 0L, (Type) null);
        case (TypeCode) 342:
          return Operators.XorInt64(Conversions.ToLong(Convert.ToString(Left)), 0L, (Type) null);
        case (TypeCode) 345:
          return Operators.XorBoolean(Conversions.ToBoolean(Convert.ToString(Left)), Convert.ToBoolean(Right));
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          return Operators.XorInt64(Conversions.ToLong(Convert.ToString(Left)), Convert.ToInt64(Right), (Type) null);
        case (TypeCode) 360:
          return Operators.XorInt64(Conversions.ToLong(Convert.ToString(Left)), Conversions.ToLong(Convert.ToString(Right)), (Type) null);
        default:
          if (typeCode1 != TypeCode.Object && typeCode2 != TypeCode.Object)
            throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Xor, Left, Right);
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Xor, Left, Right);
      }
    }

    private static object XorBoolean(bool left, bool right)
    {
      return (object) (left ^ right);
    }

    private static object XorSByte(sbyte left, sbyte right, Type enumType = null)
    {
      sbyte num = (sbyte) ((int) left ^ (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object XorByte(byte left, byte right, Type enumType = null)
    {
      byte num = (byte) ((int) left ^ (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object XorInt16(short left, short right, Type enumType = null)
    {
      short num = (short) ((int) left ^ (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object XorUInt16(ushort left, ushort right, Type enumType = null)
    {
      ushort num = (ushort) ((int) left ^ (int) right);
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object XorInt32(int left, int right, Type enumType = null)
    {
      int num = left ^ right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object XorUInt32(uint left, uint right, Type enumType = null)
    {
      uint num = left ^ right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object XorInt64(long left, long right, Type enumType = null)
    {
      long num = left ^ right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    private static object XorUInt64(ulong left, ulong right, Type enumType = null)
    {
      ulong num = left ^ right;
      return (object) enumType == null ? (object) num : Enum.ToObject(enumType, num);
    }

    /// <summary>Represents the Visual Basic addition (+) operator.</summary>
    /// <param name="Left">Required. Any numeric expression.</param>
    /// <param name="Right">Required. Any numeric expression.</param>
    /// <returns>The sum of <paramref name="Left" /> and <paramref name="Right" />.</returns>
    public static object AddObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      if (typeCode1 == TypeCode.Object && Left is char[] chArray && (typeCode2 == TypeCode.String || typeCode2 == TypeCode.Empty || typeCode2 == TypeCode.Object && Right is char[]))
      {
        Left = (object) new string(chArray);
        typeCode1 = TypeCode.String;
      }
      if (typeCode2 == TypeCode.Object && Right is char[] chArray && (typeCode1 == TypeCode.String || typeCode1 == TypeCode.Empty))
      {
        Right = (object) new string(chArray);
        typeCode2 = TypeCode.String;
      }
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
          return Operators.Boxed_ZeroInteger;
        case TypeCode.Boolean:
          return Operators.AddInt16((short) 0, (short) Operators.ToVBBool(Right));
        case TypeCode.Char:
          return Operators.AddString("\0", Convert.ToString(Right));
        case TypeCode.SByte:
          return (object) Convert.ToSByte(Right);
        case TypeCode.Byte:
          return (object) Convert.ToByte(Right);
        case TypeCode.Int16:
          return (object) Convert.ToInt16(Right);
        case TypeCode.UInt16:
          return (object) Convert.ToUInt16(Right);
        case TypeCode.Int32:
          return (object) Convert.ToInt32(Right);
        case TypeCode.UInt32:
          return (object) Convert.ToUInt32(Right);
        case TypeCode.Int64:
          return (object) Convert.ToInt64(Right);
        case TypeCode.UInt64:
          return (object) Convert.ToUInt64(Right);
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
        case TypeCode.String:
          return Right;
        case TypeCode.DateTime:
          return Operators.AddString(Conversions.ToString(DateTime.MinValue), Conversions.ToString(Convert.ToDateTime(Right)));
        case (TypeCode) 57:
          return Operators.AddInt16((short) Operators.ToVBBool(Left), (short) 0);
        case (TypeCode) 60:
          return Operators.AddInt16((short) Operators.ToVBBool(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 62:
          return Operators.AddSByte(Operators.ToVBBool(Left), Convert.ToSByte(Right));
        case (TypeCode) 63:
        case (TypeCode) 64:
          return Operators.AddInt16((short) Operators.ToVBBool(Left), Convert.ToInt16(Right));
        case (TypeCode) 65:
        case (TypeCode) 66:
          return Operators.AddInt32((int) Operators.ToVBBool(Left), Convert.ToInt32(Right));
        case (TypeCode) 67:
        case (TypeCode) 68:
          return Operators.AddInt64((long) Operators.ToVBBool(Left), Convert.ToInt64(Right));
        case (TypeCode) 69:
        case (TypeCode) 72:
          return Operators.AddDecimal((object) Operators.ToVBBool(Left), (object) Convert.ToDecimal(Right));
        case (TypeCode) 70:
          return Operators.AddSingle((float) Operators.ToVBBool(Left), Convert.ToSingle(Right));
        case (TypeCode) 71:
          return Operators.AddDouble((double) Operators.ToVBBool(Left), Convert.ToDouble(Right));
        case (TypeCode) 75:
          return Operators.AddDouble((double) Operators.ToVBBool(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 76:
          return Operators.AddString(Convert.ToString(Left), "\0");
        case (TypeCode) 80:
        case (TypeCode) 94:
        case (TypeCode) 346:
          return Operators.AddString(Convert.ToString(Left), Convert.ToString(Right));
        case (TypeCode) 95:
          return (object) Convert.ToSByte(Left);
        case (TypeCode) 98:
          return Operators.AddSByte(Convert.ToSByte(Left), Operators.ToVBBool(Right));
        case (TypeCode) 100:
          return Operators.AddSByte(Convert.ToSByte(Left), Convert.ToSByte(Right));
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 119:
        case (TypeCode) 121:
        case (TypeCode) 138:
        case (TypeCode) 139:
        case (TypeCode) 140:
          return Operators.AddInt16(Convert.ToInt16(Left), Convert.ToInt16(Right));
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 123:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 157:
        case (TypeCode) 159:
        case (TypeCode) 161:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
        case (TypeCode) 180:
          return Operators.AddInt32(Convert.ToInt32(Left), Convert.ToInt32(Right));
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 125:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 163:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 195:
        case (TypeCode) 197:
        case (TypeCode) 199:
        case (TypeCode) 201:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 220:
          return Operators.AddInt64(Convert.ToInt64(Left), Convert.ToInt64(Right));
        case (TypeCode) 107:
        case (TypeCode) 110:
        case (TypeCode) 129:
        case (TypeCode) 145:
        case (TypeCode) 148:
        case (TypeCode) 167:
        case (TypeCode) 183:
        case (TypeCode) 186:
        case (TypeCode) 205:
        case (TypeCode) 221:
        case (TypeCode) 224:
        case (TypeCode) 233:
        case (TypeCode) 235:
        case (TypeCode) 237:
        case (TypeCode) 239:
        case (TypeCode) 243:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 300:
          return Operators.AddDecimal(Left, Right);
        case (TypeCode) 108:
        case (TypeCode) 127:
        case (TypeCode) 146:
        case (TypeCode) 165:
        case (TypeCode) 184:
        case (TypeCode) 203:
        case (TypeCode) 222:
        case (TypeCode) 241:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 262:
        case (TypeCode) 298:
          return Operators.AddSingle(Convert.ToSingle(Left), Convert.ToSingle(Right));
        case (TypeCode) 109:
        case (TypeCode) 128:
        case (TypeCode) 147:
        case (TypeCode) 166:
        case (TypeCode) 185:
        case (TypeCode) 204:
        case (TypeCode) 223:
        case (TypeCode) 242:
        case (TypeCode) 261:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 299:
          return Operators.AddDouble(Convert.ToDouble(Left), Convert.ToDouble(Right));
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          return Operators.AddDouble(Convert.ToDouble(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 114:
          return (object) Convert.ToByte(Left);
        case (TypeCode) 117:
        case (TypeCode) 136:
          return Operators.AddInt16(Convert.ToInt16(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 120:
          return Operators.AddByte(Convert.ToByte(Left), Convert.ToByte(Right));
        case (TypeCode) 122:
        case (TypeCode) 158:
        case (TypeCode) 160:
          return Operators.AddUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right));
        case (TypeCode) 124:
        case (TypeCode) 162:
        case (TypeCode) 196:
        case (TypeCode) 198:
        case (TypeCode) 200:
          return Operators.AddUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right));
        case (TypeCode) 126:
        case (TypeCode) 164:
        case (TypeCode) 202:
        case (TypeCode) 234:
        case (TypeCode) 236:
        case (TypeCode) 238:
        case (TypeCode) 240:
          return Operators.AddUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right));
        case (TypeCode) 133:
          return (object) Convert.ToInt16(Left);
        case (TypeCode) 152:
          return (object) Convert.ToUInt16(Left);
        case (TypeCode) 155:
        case (TypeCode) 174:
          return Operators.AddInt32(Convert.ToInt32(Left), (int) Operators.ToVBBool(Right));
        case (TypeCode) 171:
          return (object) Convert.ToInt32(Left);
        case (TypeCode) 190:
          return (object) Convert.ToUInt32(Left);
        case (TypeCode) 193:
        case (TypeCode) 212:
          return Operators.AddInt64(Convert.ToInt64(Left), (long) Operators.ToVBBool(Right));
        case (TypeCode) 209:
          return (object) Convert.ToInt64(Left);
        case (TypeCode) 228:
          return (object) Convert.ToUInt64(Left);
        case (TypeCode) 231:
        case (TypeCode) 288:
          return Operators.AddDecimal(Left, (object) Operators.ToVBBool(Right));
        case (TypeCode) 247:
        case (TypeCode) 266:
        case (TypeCode) 285:
        case (TypeCode) 342:
          return Left;
        case (TypeCode) 250:
          return Operators.AddSingle(Convert.ToSingle(Left), (float) Operators.ToVBBool(Right));
        case (TypeCode) 269:
          return Operators.AddDouble(Convert.ToDouble(Left), (double) Operators.ToVBBool(Right));
        case (TypeCode) 304:
          return Operators.AddString(Conversions.ToString(DateTime.MinValue), Conversions.ToString(Conversions.ToDate(Left)));
        case (TypeCode) 320:
          return Operators.AddString(Conversions.ToString(Convert.ToDateTime(Left)), Conversions.ToString(Convert.ToDateTime(Right)));
        case (TypeCode) 322:
          return Operators.AddString(Conversions.ToString(Convert.ToDateTime(Left)), Convert.ToString(Right));
        case (TypeCode) 345:
          return Operators.AddDouble(Conversions.ToDouble(Convert.ToString(Left)), (double) Operators.ToVBBool(Right));
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          return Operators.AddDouble(Conversions.ToDouble(Convert.ToString(Left)), Convert.ToDouble(Right));
        case (TypeCode) 358:
          return Operators.AddString(Convert.ToString(Left), Conversions.ToString(Convert.ToDateTime(Right)));
        case (TypeCode) 360:
          return Operators.AddString(Convert.ToString(Left), Convert.ToString(Right));
        default:
          if (typeCode1 != TypeCode.Object && typeCode2 != TypeCode.Object)
            throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Plus, Left, Right);
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Plus, Left, Right);
      }
    }

    private static object AddByte(byte left, byte right)
    {
      short num = checked ((short) unchecked ((int) left + (int) right));
      return num <= (short) byte.MaxValue ? (object) checked ((byte) num) : (object) num;
    }

    private static object AddSByte(sbyte left, sbyte right)
    {
      short num = checked ((short) unchecked ((int) left + (int) right));
      return num > (short) sbyte.MaxValue || num < (short) sbyte.MinValue ? (object) num : (object) checked ((sbyte) num);
    }

    private static object AddInt16(short left, short right)
    {
      int num = checked ((int) left + (int) right);
      return num > (int) short.MaxValue || num < (int) short.MinValue ? (object) num : (object) checked ((short) num);
    }

    private static object AddUInt16(ushort left, ushort right)
    {
      int num = checked ((int) left + (int) right);
      return num <= (int) ushort.MaxValue ? (object) checked ((ushort) num) : (object) num;
    }

    private static object AddInt32(int left, int right)
    {
      long num = checked ((long) left + (long) right);
      return num > (long) int.MaxValue || num < (long) int.MinValue ? (object) num : (object) checked ((int) num);
    }

    private static object AddUInt32(uint left, uint right)
    {
      long num = checked ((long) left + (long) right);
      return num <= (long) uint.MaxValue ? (object) checked ((uint) num) : (object) num;
    }

    private static object AddInt64(long left, long right)
    {
      try
      {
        return (object) checked (left + right);
      }
      catch (OverflowException ex)
      {
        return (object) Decimal.Add(new Decimal(left), new Decimal(right));
      }
    }

    private static object AddUInt64(ulong left, ulong right)
    {
      try
      {
        return (object) checked (left + right);
      }
      catch (OverflowException ex)
      {
        return (object) Decimal.Add(new Decimal(left), new Decimal(right));
      }
    }

    private static object AddDecimal(object left, object right)
    {
      Decimal d1 = Convert.ToDecimal(left);
      Decimal d2 = Convert.ToDecimal(right);
      try
      {
        return (object) Decimal.Add(d1, d2);
      }
      catch (OverflowException ex)
      {
        return (object) (Convert.ToDouble(d1) + Convert.ToDouble(d2));
      }
    }

    private static object AddSingle(float left, float right)
    {
      double d = (double) left + (double) right;
      return d > 3.40282346638529E+38 || d < -3.40282346638529E+38 ? (!double.IsInfinity(d) || !float.IsInfinity(left) && !float.IsInfinity(right) ? (object) d : (object) (float) d) : (object) (float) d;
    }

    private static object AddDouble(double left, double right)
    {
      return (object) (left + right);
    }

    private static object AddString(string left, string right)
    {
      return (object) (left + right);
    }

    /// <summary>Represents the Visual Basic subtraction (-) operator.</summary>
    /// <param name="Left">Required. Any numeric expression.</param>
    /// <param name="Right">Required. Any numeric expression.</param>
    /// <returns>The difference between <paramref name="Left" /> and <paramref name="Right" />.</returns>
    public static object SubtractObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
          return Operators.Boxed_ZeroInteger;
        case TypeCode.Boolean:
          return Operators.SubtractInt16((short) 0, (short) Operators.ToVBBool(Right));
        case TypeCode.SByte:
          return Operators.SubtractSByte((sbyte) 0, Convert.ToSByte(Right));
        case TypeCode.Byte:
          return Operators.SubtractByte((byte) 0, Convert.ToByte(Right));
        case TypeCode.Int16:
          return Operators.SubtractInt16((short) 0, Convert.ToInt16(Right));
        case TypeCode.UInt16:
          return Operators.SubtractUInt16((ushort) 0, Convert.ToUInt16(Right));
        case TypeCode.Int32:
          return Operators.SubtractInt32(0, Convert.ToInt32(Right));
        case TypeCode.UInt32:
          return Operators.SubtractUInt32(0U, Convert.ToUInt32(Right));
        case TypeCode.Int64:
          return Operators.SubtractInt64(0L, Convert.ToInt64(Right));
        case TypeCode.UInt64:
          return Operators.SubtractUInt64(0UL, Convert.ToUInt64(Right));
        case TypeCode.Single:
          return Operators.SubtractSingle(0.0f, Convert.ToSingle(Right));
        case TypeCode.Double:
          return Operators.SubtractDouble(0.0, Convert.ToDouble(Right));
        case TypeCode.Decimal:
          return Operators.SubtractDecimal((object) Decimal.Zero, Right);
        case TypeCode.String:
          return Operators.SubtractDouble(0.0, Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 57:
          return Operators.SubtractInt16((short) Operators.ToVBBool(Left), (short) 0);
        case (TypeCode) 60:
          return Operators.SubtractInt16((short) Operators.ToVBBool(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 62:
          return Operators.SubtractSByte(Operators.ToVBBool(Left), Convert.ToSByte(Right));
        case (TypeCode) 63:
        case (TypeCode) 64:
          return Operators.SubtractInt16((short) Operators.ToVBBool(Left), Convert.ToInt16(Right));
        case (TypeCode) 65:
        case (TypeCode) 66:
          return Operators.SubtractInt32((int) Operators.ToVBBool(Left), Convert.ToInt32(Right));
        case (TypeCode) 67:
        case (TypeCode) 68:
          return Operators.SubtractInt64((long) Operators.ToVBBool(Left), Convert.ToInt64(Right));
        case (TypeCode) 69:
        case (TypeCode) 72:
          return Operators.SubtractDecimal((object) Operators.ToVBBool(Left), (object) Convert.ToDecimal(Right));
        case (TypeCode) 70:
          return Operators.SubtractSingle((float) Operators.ToVBBool(Left), Convert.ToSingle(Right));
        case (TypeCode) 71:
          return Operators.SubtractDouble((double) Operators.ToVBBool(Left), Convert.ToDouble(Right));
        case (TypeCode) 75:
          return Operators.SubtractDouble((double) Operators.ToVBBool(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 95:
          return (object) Convert.ToSByte(Left);
        case (TypeCode) 98:
          return Operators.SubtractSByte(Convert.ToSByte(Left), Operators.ToVBBool(Right));
        case (TypeCode) 100:
          return Operators.SubtractSByte(Convert.ToSByte(Left), Convert.ToSByte(Right));
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 119:
        case (TypeCode) 121:
        case (TypeCode) 138:
        case (TypeCode) 139:
        case (TypeCode) 140:
          return Operators.SubtractInt16(Convert.ToInt16(Left), Convert.ToInt16(Right));
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 123:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 157:
        case (TypeCode) 159:
        case (TypeCode) 161:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
        case (TypeCode) 180:
          return Operators.SubtractInt32(Convert.ToInt32(Left), Convert.ToInt32(Right));
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 125:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 163:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 195:
        case (TypeCode) 197:
        case (TypeCode) 199:
        case (TypeCode) 201:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 220:
          return Operators.SubtractInt64(Convert.ToInt64(Left), Convert.ToInt64(Right));
        case (TypeCode) 107:
        case (TypeCode) 110:
        case (TypeCode) 129:
        case (TypeCode) 145:
        case (TypeCode) 148:
        case (TypeCode) 167:
        case (TypeCode) 183:
        case (TypeCode) 186:
        case (TypeCode) 205:
        case (TypeCode) 221:
        case (TypeCode) 224:
        case (TypeCode) 233:
        case (TypeCode) 235:
        case (TypeCode) 237:
        case (TypeCode) 239:
        case (TypeCode) 243:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 300:
          return Operators.SubtractDecimal(Left, Right);
        case (TypeCode) 108:
        case (TypeCode) 127:
        case (TypeCode) 146:
        case (TypeCode) 165:
        case (TypeCode) 184:
        case (TypeCode) 203:
        case (TypeCode) 222:
        case (TypeCode) 241:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 262:
        case (TypeCode) 298:
          return Operators.SubtractSingle(Convert.ToSingle(Left), Convert.ToSingle(Right));
        case (TypeCode) 109:
        case (TypeCode) 128:
        case (TypeCode) 147:
        case (TypeCode) 166:
        case (TypeCode) 185:
        case (TypeCode) 204:
        case (TypeCode) 223:
        case (TypeCode) 242:
        case (TypeCode) 261:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 299:
          return Operators.SubtractDouble(Convert.ToDouble(Left), Convert.ToDouble(Right));
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          return Operators.SubtractDouble(Convert.ToDouble(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 114:
          return (object) Convert.ToByte(Left);
        case (TypeCode) 117:
        case (TypeCode) 136:
          return Operators.SubtractInt16(Convert.ToInt16(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 120:
          return Operators.SubtractByte(Convert.ToByte(Left), Convert.ToByte(Right));
        case (TypeCode) 122:
        case (TypeCode) 158:
        case (TypeCode) 160:
          return Operators.SubtractUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right));
        case (TypeCode) 124:
        case (TypeCode) 162:
        case (TypeCode) 196:
        case (TypeCode) 198:
        case (TypeCode) 200:
          return Operators.SubtractUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right));
        case (TypeCode) 126:
        case (TypeCode) 164:
        case (TypeCode) 202:
        case (TypeCode) 234:
        case (TypeCode) 236:
        case (TypeCode) 238:
        case (TypeCode) 240:
          return Operators.SubtractUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right));
        case (TypeCode) 133:
          return (object) Convert.ToInt16(Left);
        case (TypeCode) 152:
          return (object) Convert.ToUInt16(Left);
        case (TypeCode) 155:
        case (TypeCode) 174:
          return Operators.SubtractInt32(Convert.ToInt32(Left), (int) Operators.ToVBBool(Right));
        case (TypeCode) 171:
          return (object) Convert.ToInt32(Left);
        case (TypeCode) 190:
          return (object) Convert.ToUInt32(Left);
        case (TypeCode) 193:
        case (TypeCode) 212:
          return Operators.SubtractInt64(Convert.ToInt64(Left), (long) Operators.ToVBBool(Right));
        case (TypeCode) 209:
          return (object) Convert.ToInt64(Left);
        case (TypeCode) 228:
          return (object) Convert.ToUInt64(Left);
        case (TypeCode) 231:
        case (TypeCode) 288:
          return Operators.SubtractDecimal(Left, (object) Operators.ToVBBool(Right));
        case (TypeCode) 247:
        case (TypeCode) 266:
        case (TypeCode) 285:
          return Left;
        case (TypeCode) 250:
          return Operators.SubtractSingle(Convert.ToSingle(Left), (float) Operators.ToVBBool(Right));
        case (TypeCode) 269:
          return Operators.SubtractDouble(Convert.ToDouble(Left), (double) Operators.ToVBBool(Right));
        case (TypeCode) 342:
          return (object) Conversions.ToDouble(Convert.ToString(Left));
        case (TypeCode) 345:
          return Operators.SubtractDouble(Conversions.ToDouble(Convert.ToString(Left)), (double) Operators.ToVBBool(Right));
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          return Operators.SubtractDouble(Conversions.ToDouble(Convert.ToString(Left)), Convert.ToDouble(Right));
        case (TypeCode) 360:
          return Operators.SubtractDouble(Conversions.ToDouble(Convert.ToString(Left)), Conversions.ToDouble(Convert.ToString(Right)));
        default:
          if (typeCode1 != TypeCode.Object && typeCode2 != TypeCode.Object && (typeCode1 != TypeCode.DateTime || typeCode2 != TypeCode.DateTime) && ((typeCode1 != TypeCode.DateTime || typeCode2 != TypeCode.Empty) && (typeCode1 != TypeCode.Empty || typeCode2 != TypeCode.DateTime)))
            throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Minus, Left, Right);
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Minus, Left, Right);
      }
    }

    private static object SubtractByte(byte left, byte right)
    {
      short num = checked ((short) unchecked ((int) left - (int) right));
      return num >= (short) 0 ? (object) checked ((byte) num) : (object) num;
    }

    private static object SubtractSByte(sbyte left, sbyte right)
    {
      short num = checked ((short) unchecked ((int) left - (int) right));
      return num < (short) sbyte.MinValue || num > (short) sbyte.MaxValue ? (object) num : (object) checked ((sbyte) num);
    }

    private static object SubtractInt16(short left, short right)
    {
      int num = checked ((int) left - (int) right);
      return num < (int) short.MinValue || num > (int) short.MaxValue ? (object) num : (object) checked ((short) num);
    }

    private static object SubtractUInt16(ushort left, ushort right)
    {
      int num = checked ((int) left - (int) right);
      return num >= 0 ? (object) checked ((ushort) num) : (object) num;
    }

    private static object SubtractInt32(int left, int right)
    {
      long num = checked ((long) left - (long) right);
      return num < (long) int.MinValue || num > (long) int.MaxValue ? (object) num : (object) checked ((int) num);
    }

    private static object SubtractUInt32(uint left, uint right)
    {
      long num = checked ((long) left - (long) right);
      return num >= 0L ? (object) checked ((uint) num) : (object) num;
    }

    private static object SubtractInt64(long left, long right)
    {
      try
      {
        return (object) checked (left - right);
      }
      catch (OverflowException ex)
      {
        return (object) Decimal.Subtract(new Decimal(left), new Decimal(right));
      }
    }

    private static object SubtractUInt64(ulong left, ulong right)
    {
      try
      {
        return (object) checked (left - right);
      }
      catch (OverflowException ex)
      {
        return (object) Decimal.Subtract(new Decimal(left), new Decimal(right));
      }
    }

    private static object SubtractDecimal(object left, object right)
    {
      Decimal d1 = Convert.ToDecimal(left);
      Decimal d2 = Convert.ToDecimal(right);
      try
      {
        return (object) Decimal.Subtract(d1, d2);
      }
      catch (OverflowException ex)
      {
        return (object) (Convert.ToDouble(d1) - Convert.ToDouble(d2));
      }
    }

    private static object SubtractSingle(float left, float right)
    {
      double d = (double) left - (double) right;
      return d > 3.40282346638529E+38 || d < -3.40282346638529E+38 ? (!double.IsInfinity(d) || !float.IsInfinity(left) && !float.IsInfinity(right) ? (object) d : (object) (float) d) : (object) (float) d;
    }

    private static object SubtractDouble(double left, double right)
    {
      return (object) (left - right);
    }

    /// <summary>Represents the Visual Basic multiply (*) operator.</summary>
    /// <param name="Left">Required. Any numeric expression.</param>
    /// <param name="Right">Required. Any numeric expression.</param>
    /// <returns>The product of <paramref name="Left" /> and <paramref name="Right" />.</returns>
    public static object MultiplyObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
        case TypeCode.Int32:
        case (TypeCode) 171:
          return Operators.Boxed_ZeroInteger;
        case TypeCode.Boolean:
        case TypeCode.Int16:
        case (TypeCode) 57:
        case (TypeCode) 133:
          return Operators.Boxed_ZeroShort;
        case TypeCode.SByte:
        case (TypeCode) 95:
          return Operators.Boxed_ZeroSByte;
        case TypeCode.Byte:
        case (TypeCode) 114:
          return Operators.Boxed_ZeroByte;
        case TypeCode.UInt16:
        case (TypeCode) 152:
          return Operators.Boxed_ZeroUShort;
        case TypeCode.UInt32:
        case (TypeCode) 190:
          return Operators.Boxed_ZeroUInteger;
        case TypeCode.Int64:
        case (TypeCode) 209:
          return Operators.Boxed_ZeroLong;
        case TypeCode.UInt64:
        case (TypeCode) 228:
          return Operators.Boxed_ZeroULong;
        case TypeCode.Single:
        case (TypeCode) 247:
          return Operators.Boxed_ZeroSinge;
        case TypeCode.Double:
        case (TypeCode) 266:
          return Operators.Boxed_ZeroDouble;
        case TypeCode.Decimal:
        case (TypeCode) 285:
          return Operators.Boxed_ZeroDecimal;
        case TypeCode.String:
          return Operators.MultiplyDouble(0.0, Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 60:
          return Operators.MultiplyInt16((short) Operators.ToVBBool(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 62:
          return Operators.MultiplySByte(Operators.ToVBBool(Left), Convert.ToSByte(Right));
        case (TypeCode) 63:
        case (TypeCode) 64:
          return Operators.MultiplyInt16((short) Operators.ToVBBool(Left), Convert.ToInt16(Right));
        case (TypeCode) 65:
        case (TypeCode) 66:
          return Operators.MultiplyInt32((int) Operators.ToVBBool(Left), Convert.ToInt32(Right));
        case (TypeCode) 67:
        case (TypeCode) 68:
          return Operators.MultiplyInt64((long) Operators.ToVBBool(Left), Convert.ToInt64(Right));
        case (TypeCode) 69:
        case (TypeCode) 72:
          return Operators.MultiplyDecimal((object) Operators.ToVBBool(Left), (object) Convert.ToDecimal(Right));
        case (TypeCode) 70:
          return Operators.MultiplySingle((float) Operators.ToVBBool(Left), Convert.ToSingle(Right));
        case (TypeCode) 71:
          return Operators.MultiplyDouble((double) Operators.ToVBBool(Left), Convert.ToDouble(Right));
        case (TypeCode) 75:
          return Operators.MultiplyDouble((double) Operators.ToVBBool(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 98:
          return Operators.MultiplySByte(Convert.ToSByte(Left), Operators.ToVBBool(Right));
        case (TypeCode) 100:
          return Operators.MultiplySByte(Convert.ToSByte(Left), Convert.ToSByte(Right));
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 119:
        case (TypeCode) 121:
        case (TypeCode) 138:
        case (TypeCode) 139:
        case (TypeCode) 140:
          return Operators.MultiplyInt16(Convert.ToInt16(Left), Convert.ToInt16(Right));
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 123:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 157:
        case (TypeCode) 159:
        case (TypeCode) 161:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
        case (TypeCode) 180:
          return Operators.MultiplyInt32(Convert.ToInt32(Left), Convert.ToInt32(Right));
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 125:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 163:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 195:
        case (TypeCode) 197:
        case (TypeCode) 199:
        case (TypeCode) 201:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 220:
          return Operators.MultiplyInt64(Convert.ToInt64(Left), Convert.ToInt64(Right));
        case (TypeCode) 107:
        case (TypeCode) 110:
        case (TypeCode) 129:
        case (TypeCode) 145:
        case (TypeCode) 148:
        case (TypeCode) 167:
        case (TypeCode) 183:
        case (TypeCode) 186:
        case (TypeCode) 205:
        case (TypeCode) 221:
        case (TypeCode) 224:
        case (TypeCode) 233:
        case (TypeCode) 235:
        case (TypeCode) 237:
        case (TypeCode) 239:
        case (TypeCode) 243:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 300:
          return Operators.MultiplyDecimal(Left, Right);
        case (TypeCode) 108:
        case (TypeCode) 127:
        case (TypeCode) 146:
        case (TypeCode) 165:
        case (TypeCode) 184:
        case (TypeCode) 203:
        case (TypeCode) 222:
        case (TypeCode) 241:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 262:
        case (TypeCode) 298:
          return Operators.MultiplySingle(Convert.ToSingle(Left), Convert.ToSingle(Right));
        case (TypeCode) 109:
        case (TypeCode) 128:
        case (TypeCode) 147:
        case (TypeCode) 166:
        case (TypeCode) 185:
        case (TypeCode) 204:
        case (TypeCode) 223:
        case (TypeCode) 242:
        case (TypeCode) 261:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 299:
          return Operators.MultiplyDouble(Convert.ToDouble(Left), Convert.ToDouble(Right));
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          return Operators.MultiplyDouble(Convert.ToDouble(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 117:
        case (TypeCode) 136:
          return Operators.MultiplyInt16(Convert.ToInt16(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 120:
          return Operators.MultiplyByte(Convert.ToByte(Left), Convert.ToByte(Right));
        case (TypeCode) 122:
        case (TypeCode) 158:
        case (TypeCode) 160:
          return Operators.MultiplyUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right));
        case (TypeCode) 124:
        case (TypeCode) 162:
        case (TypeCode) 196:
        case (TypeCode) 198:
        case (TypeCode) 200:
          return Operators.MultiplyUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right));
        case (TypeCode) 126:
        case (TypeCode) 164:
        case (TypeCode) 202:
        case (TypeCode) 234:
        case (TypeCode) 236:
        case (TypeCode) 238:
        case (TypeCode) 240:
          return Operators.MultiplyUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right));
        case (TypeCode) 155:
        case (TypeCode) 174:
          return Operators.MultiplyInt32(Convert.ToInt32(Left), (int) Operators.ToVBBool(Right));
        case (TypeCode) 193:
        case (TypeCode) 212:
          return Operators.MultiplyInt64(Convert.ToInt64(Left), (long) Operators.ToVBBool(Right));
        case (TypeCode) 231:
        case (TypeCode) 288:
          return Operators.MultiplyDecimal(Left, (object) Operators.ToVBBool(Right));
        case (TypeCode) 250:
          return Operators.MultiplySingle(Convert.ToSingle(Left), (float) Operators.ToVBBool(Right));
        case (TypeCode) 269:
          return Operators.MultiplyDouble(Convert.ToDouble(Left), (double) Operators.ToVBBool(Right));
        case (TypeCode) 342:
          return Operators.MultiplyDouble(Conversions.ToDouble(Convert.ToString(Left)), 0.0);
        case (TypeCode) 345:
          return Operators.MultiplyDouble(Conversions.ToDouble(Convert.ToString(Left)), (double) Operators.ToVBBool(Right));
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          return Operators.MultiplyDouble(Conversions.ToDouble(Convert.ToString(Left)), Convert.ToDouble(Right));
        case (TypeCode) 360:
          return Operators.MultiplyDouble(Conversions.ToDouble(Convert.ToString(Left)), Conversions.ToDouble(Convert.ToString(Right)));
        default:
          if (typeCode1 != TypeCode.Object && typeCode2 != TypeCode.Object)
            throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Multiply, Left, Right);
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Multiply, Left, Right);
      }
    }

    private static object MultiplyByte(byte left, byte right)
    {
      int num = checked ((int) left * (int) right);
      return num <= (int) byte.MaxValue ? (object) checked ((byte) num) : (num <= (int) short.MaxValue ? (object) checked ((short) num) : (object) num);
    }

    private static object MultiplySByte(sbyte left, sbyte right)
    {
      short num = checked ((short) unchecked ((int) left * (int) right));
      return num > (short) sbyte.MaxValue || num < (short) sbyte.MinValue ? (object) num : (object) checked ((sbyte) num);
    }

    private static object MultiplyInt16(short left, short right)
    {
      int num = checked ((int) left * (int) right);
      return num > (int) short.MaxValue || num < (int) short.MinValue ? (object) num : (object) checked ((short) num);
    }

    private static object MultiplyUInt16(ushort left, ushort right)
    {
      long num = checked ((long) left * (long) right);
      return num <= (long) ushort.MaxValue ? (object) checked ((ushort) num) : (num <= (long) int.MaxValue ? (object) checked ((int) num) : (object) num);
    }

    private static object MultiplyInt32(int left, int right)
    {
      long num = checked ((long) left * (long) right);
      return num > (long) int.MaxValue || num < (long) int.MinValue ? (object) num : (object) checked ((int) num);
    }

    private static object MultiplyUInt32(uint left, uint right)
    {
      ulong num = checked ((ulong) left * (ulong) right);
      return num <= (ulong) uint.MaxValue ? (object) checked ((uint) num) : (Decimal.Compare(new Decimal(num), new Decimal(-1, int.MaxValue, 0, false, (byte) 0)) <= 0 ? (object) checked ((long) num) : (object) new Decimal(num));
    }

    private static object MultiplyInt64(long left, long right)
    {
      try
      {
        return (object) checked (left * right);
      }
      catch (OverflowException ex)
      {
      }
      try
      {
        return (object) Decimal.Multiply(new Decimal(left), new Decimal(right));
      }
      catch (OverflowException ex)
      {
        return (object) ((double) left * (double) right);
      }
    }

    private static object MultiplyUInt64(ulong left, ulong right)
    {
      try
      {
        return (object) checked (left * right);
      }
      catch (OverflowException ex)
      {
      }
      try
      {
        return (object) Decimal.Multiply(new Decimal(left), new Decimal(right));
      }
      catch (OverflowException ex)
      {
        return (object) ((double) left * (double) right);
      }
    }

    private static object MultiplyDecimal(object left, object right)
    {
      Decimal d1 = Convert.ToDecimal(left);
      Decimal d2 = Convert.ToDecimal(right);
      try
      {
        return (object) Decimal.Multiply(d1, d2);
      }
      catch (OverflowException ex)
      {
        return (object) (Convert.ToDouble(d1) * Convert.ToDouble(d2));
      }
    }

    private static object MultiplySingle(float left, float right)
    {
      double d = (double) left * (double) right;
      return d > 3.40282346638529E+38 || d < -3.40282346638529E+38 ? (!double.IsInfinity(d) || !float.IsInfinity(left) && !float.IsInfinity(right) ? (object) d : (object) (float) d) : (object) (float) d;
    }

    private static object MultiplyDouble(double left, double right)
    {
      return (object) (left * right);
    }

    /// <summary>Represents the Visual Basic division (/) operator.</summary>
    /// <param name="Left">Required. Any numeric expression.</param>
    /// <param name="Right">Required. Any numeric expression.</param>
    /// <returns>The full quotient of <paramref name="Left" /> divided by <paramref name="Right" />, including any remainder.</returns>
    public static object DivideObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
          return Operators.DivideDouble(0.0, 0.0);
        case TypeCode.Boolean:
          return Operators.DivideDouble(0.0, (double) Operators.ToVBBool(Right));
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Double:
          return Operators.DivideDouble(0.0, Convert.ToDouble(Right));
        case TypeCode.Single:
          return Operators.DivideSingle(0.0f, Convert.ToSingle(Right));
        case TypeCode.Decimal:
          return Operators.DivideDecimal((object) Decimal.Zero, Right);
        case TypeCode.String:
          return Operators.DivideDouble(0.0, Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 57:
          return Operators.DivideDouble((double) Operators.ToVBBool(Left), 0.0);
        case (TypeCode) 60:
          return Operators.DivideDouble((double) Operators.ToVBBool(Left), (double) Operators.ToVBBool(Right));
        case (TypeCode) 62:
        case (TypeCode) 63:
        case (TypeCode) 64:
        case (TypeCode) 65:
        case (TypeCode) 66:
        case (TypeCode) 67:
        case (TypeCode) 68:
        case (TypeCode) 69:
        case (TypeCode) 71:
          return Operators.DivideDouble((double) Operators.ToVBBool(Left), Convert.ToDouble(Right));
        case (TypeCode) 70:
          return Operators.DivideSingle((float) Operators.ToVBBool(Left), Convert.ToSingle(Right));
        case (TypeCode) 72:
          return Operators.DivideDecimal((object) Operators.ToVBBool(Left), Right);
        case (TypeCode) 75:
          return Operators.DivideDouble((double) Operators.ToVBBool(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 95:
        case (TypeCode) 114:
        case (TypeCode) 133:
        case (TypeCode) 152:
        case (TypeCode) 171:
        case (TypeCode) 190:
        case (TypeCode) 209:
        case (TypeCode) 228:
        case (TypeCode) 266:
          return Operators.DivideDouble(Convert.ToDouble(Left), 0.0);
        case (TypeCode) 98:
        case (TypeCode) 117:
        case (TypeCode) 136:
        case (TypeCode) 155:
        case (TypeCode) 174:
        case (TypeCode) 193:
        case (TypeCode) 212:
        case (TypeCode) 231:
        case (TypeCode) 269:
          return Operators.DivideDouble(Convert.ToDouble(Left), (double) Operators.ToVBBool(Right));
        case (TypeCode) 100:
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 107:
        case (TypeCode) 109:
        case (TypeCode) 119:
        case (TypeCode) 120:
        case (TypeCode) 121:
        case (TypeCode) 122:
        case (TypeCode) 123:
        case (TypeCode) 124:
        case (TypeCode) 125:
        case (TypeCode) 126:
        case (TypeCode) 128:
        case (TypeCode) 138:
        case (TypeCode) 139:
        case (TypeCode) 140:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 145:
        case (TypeCode) 147:
        case (TypeCode) 157:
        case (TypeCode) 158:
        case (TypeCode) 159:
        case (TypeCode) 160:
        case (TypeCode) 161:
        case (TypeCode) 162:
        case (TypeCode) 163:
        case (TypeCode) 164:
        case (TypeCode) 166:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
        case (TypeCode) 180:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 183:
        case (TypeCode) 185:
        case (TypeCode) 195:
        case (TypeCode) 196:
        case (TypeCode) 197:
        case (TypeCode) 198:
        case (TypeCode) 199:
        case (TypeCode) 200:
        case (TypeCode) 201:
        case (TypeCode) 202:
        case (TypeCode) 204:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 220:
        case (TypeCode) 221:
        case (TypeCode) 223:
        case (TypeCode) 233:
        case (TypeCode) 234:
        case (TypeCode) 235:
        case (TypeCode) 236:
        case (TypeCode) 237:
        case (TypeCode) 238:
        case (TypeCode) 239:
        case (TypeCode) 240:
        case (TypeCode) 242:
        case (TypeCode) 261:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 299:
          return Operators.DivideDouble(Convert.ToDouble(Left), Convert.ToDouble(Right));
        case (TypeCode) 108:
        case (TypeCode) 127:
        case (TypeCode) 146:
        case (TypeCode) 165:
        case (TypeCode) 184:
        case (TypeCode) 203:
        case (TypeCode) 222:
        case (TypeCode) 241:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 262:
        case (TypeCode) 298:
          return Operators.DivideSingle(Convert.ToSingle(Left), Convert.ToSingle(Right));
        case (TypeCode) 110:
        case (TypeCode) 129:
        case (TypeCode) 148:
        case (TypeCode) 167:
        case (TypeCode) 186:
        case (TypeCode) 205:
        case (TypeCode) 224:
        case (TypeCode) 243:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 300:
          return Operators.DivideDecimal(Left, Right);
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          return Operators.DivideDouble(Convert.ToDouble(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 247:
          return Operators.DivideSingle(Convert.ToSingle(Left), 0.0f);
        case (TypeCode) 250:
          return Operators.DivideSingle(Convert.ToSingle(Left), (float) Operators.ToVBBool(Right));
        case (TypeCode) 285:
          return Operators.DivideDecimal(Left, (object) Decimal.Zero);
        case (TypeCode) 288:
          return Operators.DivideDecimal(Left, (object) Operators.ToVBBool(Right));
        case (TypeCode) 342:
          return Operators.DivideDouble(Conversions.ToDouble(Convert.ToString(Left)), 0.0);
        case (TypeCode) 345:
          return Operators.DivideDouble(Conversions.ToDouble(Convert.ToString(Left)), (double) Operators.ToVBBool(Right));
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          return Operators.DivideDouble(Conversions.ToDouble(Convert.ToString(Left)), Convert.ToDouble(Right));
        case (TypeCode) 360:
          return Operators.DivideDouble(Conversions.ToDouble(Convert.ToString(Left)), Conversions.ToDouble(Convert.ToString(Right)));
        default:
          if (typeCode1 != TypeCode.Object && typeCode2 != TypeCode.Object)
            throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Divide, Left, Right);
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Divide, Left, Right);
      }
    }

    private static object DivideDecimal(object left, object right)
    {
      Decimal d1 = Convert.ToDecimal(left);
      Decimal d2 = Convert.ToDecimal(right);
      try
      {
        return (object) Decimal.Divide(d1, d2);
      }
      catch (OverflowException ex)
      {
        return (object) (float) ((double) Convert.ToSingle(d1) / (double) Convert.ToSingle(d2));
      }
    }

    private static object DivideSingle(float left, float right)
    {
      float f = left / right;
      return !float.IsInfinity(f) ? (object) f : (float.IsInfinity(left) || float.IsInfinity(right) ? (object) f : (object) ((double) left / (double) right));
    }

    private static object DivideDouble(double left, double right)
    {
      return (object) (left / right);
    }

    /// <summary>Represents the Visual Basic exponent (^) operator.</summary>
    /// <param name="Left">Required. Any numeric expression.</param>
    /// <param name="Right">Required. Any numeric expression.</param>
    /// <returns>The result of <paramref name="Left" /> raised to the power of <paramref name="Right" />.</returns>
    public static object ExponentObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      double x;
      object obj;
      switch (typeCode1)
      {
        case TypeCode.Empty:
          x = 0.0;
          break;
        case TypeCode.Object:
          obj = Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Power, Left, Right);
          goto label_15;
        case TypeCode.Boolean:
          x = (double) Operators.ToVBBool(Left);
          break;
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          x = Convert.ToDouble(Left);
          break;
        case TypeCode.String:
          x = Conversions.ToDouble(Convert.ToString(Left));
          break;
        default:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Power, Left, Right);
      }
      double y;
      switch (typeCode2)
      {
        case TypeCode.Empty:
          y = 0.0;
          break;
        case TypeCode.Object:
          obj = Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Power, Left, Right);
          goto label_15;
        case TypeCode.Boolean:
          y = (double) Operators.ToVBBool(Right);
          break;
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          y = Convert.ToDouble(Right);
          break;
        case TypeCode.String:
          y = Conversions.ToDouble(Convert.ToString(Right));
          break;
        default:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Power, Left, Right);
      }
      obj = (object) Math.Pow(x, y);
label_15:
      return obj;
    }

    /// <summary>Represents the Visual Basic <see langword="Mod" /> operator.</summary>
    /// <param name="Left">Required. Any numeric expression.</param>
    /// <param name="Right">Required. Any numeric expression.</param>
    /// <returns>The remainder after <paramref name="Left" /> is divided by <paramref name="Right" />.</returns>
    public static object ModObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
          return Operators.ModInt32(0, 0);
        case TypeCode.Boolean:
          return Operators.ModInt16((short) 0, (short) Operators.ToVBBool(Right));
        case TypeCode.SByte:
          return Operators.ModSByte((sbyte) 0, Convert.ToSByte(Right));
        case TypeCode.Byte:
          return Operators.ModByte((byte) 0, Convert.ToByte(Right));
        case TypeCode.Int16:
          return Operators.ModInt16((short) 0, Convert.ToInt16(Right));
        case TypeCode.UInt16:
          return Operators.ModUInt16((ushort) 0, Convert.ToUInt16(Right));
        case TypeCode.Int32:
          return Operators.ModInt32(0, Convert.ToInt32(Right));
        case TypeCode.UInt32:
          return Operators.ModUInt32(0U, Convert.ToUInt32(Right));
        case TypeCode.Int64:
          return Operators.ModInt64(0L, Convert.ToInt64(Right));
        case TypeCode.UInt64:
          return Operators.ModUInt64(0UL, Convert.ToUInt64(Right));
        case TypeCode.Single:
          return Operators.ModSingle(0.0f, Convert.ToSingle(Right));
        case TypeCode.Double:
          return Operators.ModDouble(0.0, Convert.ToDouble(Right));
        case TypeCode.Decimal:
          return Operators.ModDecimal((object) Decimal.Zero, (object) Convert.ToDecimal(Right));
        case TypeCode.String:
          return Operators.ModDouble(0.0, Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 57:
          return Operators.ModInt16((short) Operators.ToVBBool(Left), (short) 0);
        case (TypeCode) 60:
          return Operators.ModInt16((short) Operators.ToVBBool(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 62:
          return Operators.ModSByte(Operators.ToVBBool(Left), Convert.ToSByte(Right));
        case (TypeCode) 63:
        case (TypeCode) 64:
          return Operators.ModInt16((short) Operators.ToVBBool(Left), Convert.ToInt16(Right));
        case (TypeCode) 65:
        case (TypeCode) 66:
          return Operators.ModInt32((int) Operators.ToVBBool(Left), Convert.ToInt32(Right));
        case (TypeCode) 67:
        case (TypeCode) 68:
          return Operators.ModInt64((long) Operators.ToVBBool(Left), Convert.ToInt64(Right));
        case (TypeCode) 69:
        case (TypeCode) 72:
          return Operators.ModDecimal((object) Operators.ToVBBool(Left), (object) Convert.ToDecimal(Right));
        case (TypeCode) 70:
          return Operators.ModSingle((float) Operators.ToVBBool(Left), Convert.ToSingle(Right));
        case (TypeCode) 71:
          return Operators.ModDouble((double) Operators.ToVBBool(Left), Convert.ToDouble(Right));
        case (TypeCode) 75:
          return Operators.ModDouble((double) Operators.ToVBBool(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 95:
          return Operators.ModSByte(Convert.ToSByte(Left), (sbyte) 0);
        case (TypeCode) 98:
          return Operators.ModSByte(Convert.ToSByte(Left), Operators.ToVBBool(Right));
        case (TypeCode) 100:
          return Operators.ModSByte(Convert.ToSByte(Left), Convert.ToSByte(Right));
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 119:
        case (TypeCode) 121:
        case (TypeCode) 138:
        case (TypeCode) 139:
        case (TypeCode) 140:
          return Operators.ModInt16(Convert.ToInt16(Left), Convert.ToInt16(Right));
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 123:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 157:
        case (TypeCode) 159:
        case (TypeCode) 161:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
        case (TypeCode) 180:
          return Operators.ModInt32(Convert.ToInt32(Left), Convert.ToInt32(Right));
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 125:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 163:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 195:
        case (TypeCode) 197:
        case (TypeCode) 199:
        case (TypeCode) 201:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 220:
          return Operators.ModInt64(Convert.ToInt64(Left), Convert.ToInt64(Right));
        case (TypeCode) 107:
        case (TypeCode) 110:
        case (TypeCode) 129:
        case (TypeCode) 145:
        case (TypeCode) 148:
        case (TypeCode) 167:
        case (TypeCode) 183:
        case (TypeCode) 186:
        case (TypeCode) 205:
        case (TypeCode) 221:
        case (TypeCode) 224:
        case (TypeCode) 233:
        case (TypeCode) 235:
        case (TypeCode) 237:
        case (TypeCode) 239:
        case (TypeCode) 243:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 300:
          return Operators.ModDecimal(Left, Right);
        case (TypeCode) 108:
        case (TypeCode) 127:
        case (TypeCode) 146:
        case (TypeCode) 165:
        case (TypeCode) 184:
        case (TypeCode) 203:
        case (TypeCode) 222:
        case (TypeCode) 241:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 262:
        case (TypeCode) 298:
          return Operators.ModSingle(Convert.ToSingle(Left), Convert.ToSingle(Right));
        case (TypeCode) 109:
        case (TypeCode) 128:
        case (TypeCode) 147:
        case (TypeCode) 166:
        case (TypeCode) 185:
        case (TypeCode) 204:
        case (TypeCode) 223:
        case (TypeCode) 242:
        case (TypeCode) 261:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 299:
          return Operators.ModDouble(Convert.ToDouble(Left), Convert.ToDouble(Right));
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          return Operators.ModDouble(Convert.ToDouble(Left), Conversions.ToDouble(Convert.ToString(Right)));
        case (TypeCode) 114:
          return Operators.ModByte(Convert.ToByte(Left), (byte) 0);
        case (TypeCode) 117:
        case (TypeCode) 136:
          return Operators.ModInt16(Convert.ToInt16(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 120:
          return Operators.ModByte(Convert.ToByte(Left), Convert.ToByte(Right));
        case (TypeCode) 122:
        case (TypeCode) 158:
        case (TypeCode) 160:
          return Operators.ModUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right));
        case (TypeCode) 124:
        case (TypeCode) 162:
        case (TypeCode) 196:
        case (TypeCode) 198:
        case (TypeCode) 200:
          return Operators.ModUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right));
        case (TypeCode) 126:
        case (TypeCode) 164:
        case (TypeCode) 202:
        case (TypeCode) 234:
        case (TypeCode) 236:
        case (TypeCode) 238:
        case (TypeCode) 240:
          return Operators.ModUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right));
        case (TypeCode) 133:
          return Operators.ModInt16(Convert.ToInt16(Left), (short) 0);
        case (TypeCode) 152:
          return Operators.ModUInt16(Convert.ToUInt16(Left), (ushort) 0);
        case (TypeCode) 155:
        case (TypeCode) 174:
          return Operators.ModInt32(Convert.ToInt32(Left), (int) Operators.ToVBBool(Right));
        case (TypeCode) 171:
          return Operators.ModInt32(Convert.ToInt32(Left), 0);
        case (TypeCode) 190:
          return Operators.ModUInt32(Convert.ToUInt32(Left), 0U);
        case (TypeCode) 193:
        case (TypeCode) 212:
          return Operators.ModInt64(Convert.ToInt64(Left), (long) Operators.ToVBBool(Right));
        case (TypeCode) 209:
          return Operators.ModInt64(Convert.ToInt64(Left), 0L);
        case (TypeCode) 228:
          return Operators.ModUInt64(Convert.ToUInt64(Left), 0UL);
        case (TypeCode) 231:
        case (TypeCode) 288:
          return Operators.ModDecimal(Left, (object) Operators.ToVBBool(Right));
        case (TypeCode) 247:
          return Operators.ModSingle(Convert.ToSingle(Left), 0.0f);
        case (TypeCode) 250:
          return Operators.ModSingle(Convert.ToSingle(Left), (float) Operators.ToVBBool(Right));
        case (TypeCode) 266:
          return Operators.ModDouble(Convert.ToDouble(Left), 0.0);
        case (TypeCode) 269:
          return Operators.ModDouble(Convert.ToDouble(Left), (double) Operators.ToVBBool(Right));
        case (TypeCode) 285:
          return Operators.ModDecimal(Left, (object) Decimal.Zero);
        case (TypeCode) 342:
          return Operators.ModDouble(Conversions.ToDouble(Convert.ToString(Left)), 0.0);
        case (TypeCode) 345:
          return Operators.ModDouble(Conversions.ToDouble(Convert.ToString(Left)), (double) Operators.ToVBBool(Right));
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          return Operators.ModDouble(Conversions.ToDouble(Convert.ToString(Left)), Convert.ToDouble(Right));
        case (TypeCode) 360:
          return Operators.ModDouble(Conversions.ToDouble(Convert.ToString(Left)), Conversions.ToDouble(Convert.ToString(Right)));
        default:
          if (typeCode1 != TypeCode.Object && typeCode2 != TypeCode.Object)
            throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Modulus, Left, Right);
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Modulus, Left, Right);
      }
    }

    private static object ModSByte(sbyte left, sbyte right)
    {
      return (object) checked ((sbyte) unchecked ((int) left % (int) right));
    }

    private static object ModByte(byte left, byte right)
    {
      return (object) checked ((byte) unchecked ((uint) left % (uint) right));
    }

    private static object ModInt16(short left, short right)
    {
      int num = (int) left % (int) right;
      return num < (int) short.MinValue || num > (int) short.MaxValue ? (object) num : (object) checked ((short) num);
    }

    private static object ModUInt16(ushort left, ushort right)
    {
      return (object) checked ((ushort) unchecked ((uint) left % (uint) right));
    }

    private static object ModInt32(int left, int right)
    {
      long num = (long) left % (long) right;
      return num < (long) int.MinValue || num > (long) int.MaxValue ? (object) num : (object) checked ((int) num);
    }

    private static object ModUInt32(uint left, uint right)
    {
      return (object) (left % right);
    }

    private static object ModInt64(long left, long right)
    {
      return left != long.MinValue || right != -1L ? (object) (left % right) : (object) 0L;
    }

    private static object ModUInt64(ulong left, ulong right)
    {
      return (object) (left % right);
    }

    private static object ModDecimal(object left, object right)
    {
      return (object) Decimal.Remainder(Convert.ToDecimal(left), Convert.ToDecimal(right));
    }

    private static object ModSingle(float left, float right)
    {
      return (object) (float) ((double) left % (double) right);
    }

    private static object ModDouble(double left, double right)
    {
      return (object) (left % right);
    }

    /// <summary>Represents the Visual Basic integer division (\) operator.</summary>
    /// <param name="Left">Required. Any numeric expression.</param>
    /// <param name="Right">Required. Any numeric expression.</param>
    /// <returns>The integer quotient of <paramref name="Left" /> divided by <paramref name="Right" />, which discards any remainder and retains only the integer portion.</returns>
    public static object IntDivideObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      switch (checked (unchecked ((int) typeCode1) * 19) + typeCode2)
      {
        case TypeCode.Empty:
          return Operators.IntDivideInt32(0, 0);
        case TypeCode.Boolean:
          return Operators.IntDivideInt16((short) 0, (short) Operators.ToVBBool(Right));
        case TypeCode.SByte:
          return Operators.IntDivideSByte((sbyte) 0, Convert.ToSByte(Right));
        case TypeCode.Byte:
          return Operators.IntDivideByte((byte) 0, Convert.ToByte(Right));
        case TypeCode.Int16:
          return Operators.IntDivideInt16((short) 0, Convert.ToInt16(Right));
        case TypeCode.UInt16:
          return Operators.IntDivideUInt16((ushort) 0, Convert.ToUInt16(Right));
        case TypeCode.Int32:
          return Operators.IntDivideInt32(0, Convert.ToInt32(Right));
        case TypeCode.UInt32:
          return Operators.IntDivideUInt32(0U, Convert.ToUInt32(Right));
        case TypeCode.Int64:
          return Operators.IntDivideInt64(0L, Convert.ToInt64(Right));
        case TypeCode.UInt64:
          return Operators.IntDivideUInt64(0UL, Convert.ToUInt64(Right));
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return Operators.IntDivideInt64(0L, Convert.ToInt64(Right));
        case TypeCode.String:
          return Operators.IntDivideInt64(0L, Conversions.ToLong(Convert.ToString(Right)));
        case (TypeCode) 57:
          return Operators.IntDivideInt16((short) Operators.ToVBBool(Left), (short) 0);
        case (TypeCode) 60:
          return Operators.IntDivideInt16((short) Operators.ToVBBool(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 62:
          return Operators.IntDivideSByte(Operators.ToVBBool(Left), Convert.ToSByte(Right));
        case (TypeCode) 63:
        case (TypeCode) 64:
          return Operators.IntDivideInt16((short) Operators.ToVBBool(Left), Convert.ToInt16(Right));
        case (TypeCode) 65:
        case (TypeCode) 66:
          return Operators.IntDivideInt32((int) Operators.ToVBBool(Left), Convert.ToInt32(Right));
        case (TypeCode) 67:
        case (TypeCode) 68:
        case (TypeCode) 69:
        case (TypeCode) 70:
        case (TypeCode) 71:
        case (TypeCode) 72:
          return Operators.IntDivideInt64((long) Operators.ToVBBool(Left), Convert.ToInt64(Right));
        case (TypeCode) 75:
          return Operators.IntDivideInt64((long) Operators.ToVBBool(Left), Conversions.ToLong(Convert.ToString(Right)));
        case (TypeCode) 95:
          return Operators.IntDivideSByte(Convert.ToSByte(Left), (sbyte) 0);
        case (TypeCode) 98:
          return Operators.IntDivideSByte(Convert.ToSByte(Left), Operators.ToVBBool(Right));
        case (TypeCode) 100:
          return Operators.IntDivideSByte(Convert.ToSByte(Left), Convert.ToSByte(Right));
        case (TypeCode) 101:
        case (TypeCode) 102:
        case (TypeCode) 119:
        case (TypeCode) 121:
        case (TypeCode) 138:
        case (TypeCode) 139:
        case (TypeCode) 140:
          return Operators.IntDivideInt16(Convert.ToInt16(Left), Convert.ToInt16(Right));
        case (TypeCode) 103:
        case (TypeCode) 104:
        case (TypeCode) 123:
        case (TypeCode) 141:
        case (TypeCode) 142:
        case (TypeCode) 157:
        case (TypeCode) 159:
        case (TypeCode) 161:
        case (TypeCode) 176:
        case (TypeCode) 177:
        case (TypeCode) 178:
        case (TypeCode) 179:
        case (TypeCode) 180:
          return Operators.IntDivideInt32(Convert.ToInt32(Left), Convert.ToInt32(Right));
        case (TypeCode) 105:
        case (TypeCode) 106:
        case (TypeCode) 107:
        case (TypeCode) 108:
        case (TypeCode) 109:
        case (TypeCode) 110:
        case (TypeCode) 125:
        case (TypeCode) 127:
        case (TypeCode) 128:
        case (TypeCode) 129:
        case (TypeCode) 143:
        case (TypeCode) 144:
        case (TypeCode) 145:
        case (TypeCode) 146:
        case (TypeCode) 147:
        case (TypeCode) 148:
        case (TypeCode) 163:
        case (TypeCode) 165:
        case (TypeCode) 166:
        case (TypeCode) 167:
        case (TypeCode) 181:
        case (TypeCode) 182:
        case (TypeCode) 183:
        case (TypeCode) 184:
        case (TypeCode) 185:
        case (TypeCode) 186:
        case (TypeCode) 195:
        case (TypeCode) 197:
        case (TypeCode) 199:
        case (TypeCode) 201:
        case (TypeCode) 203:
        case (TypeCode) 204:
        case (TypeCode) 205:
        case (TypeCode) 214:
        case (TypeCode) 215:
        case (TypeCode) 216:
        case (TypeCode) 217:
        case (TypeCode) 218:
        case (TypeCode) 219:
        case (TypeCode) 220:
        case (TypeCode) 221:
        case (TypeCode) 222:
        case (TypeCode) 223:
        case (TypeCode) 224:
        case (TypeCode) 233:
        case (TypeCode) 235:
        case (TypeCode) 237:
        case (TypeCode) 239:
        case (TypeCode) 241:
        case (TypeCode) 242:
        case (TypeCode) 243:
        case (TypeCode) 252:
        case (TypeCode) 253:
        case (TypeCode) 254:
        case (TypeCode) 255:
        case (TypeCode) 256:
        case (TypeCode) 257:
        case (TypeCode) 258:
        case (TypeCode) 259:
        case (TypeCode) 260:
        case (TypeCode) 261:
        case (TypeCode) 262:
        case (TypeCode) 271:
        case (TypeCode) 272:
        case (TypeCode) 273:
        case (TypeCode) 274:
        case (TypeCode) 275:
        case (TypeCode) 276:
        case (TypeCode) 277:
        case (TypeCode) 278:
        case (TypeCode) 279:
        case (TypeCode) 280:
        case (TypeCode) 281:
        case (TypeCode) 290:
        case (TypeCode) 291:
        case (TypeCode) 292:
        case (TypeCode) 293:
        case (TypeCode) 294:
        case (TypeCode) 295:
        case (TypeCode) 296:
        case (TypeCode) 297:
        case (TypeCode) 298:
        case (TypeCode) 299:
        case (TypeCode) 300:
          return Operators.IntDivideInt64(Convert.ToInt64(Left), Convert.ToInt64(Right));
        case (TypeCode) 113:
        case (TypeCode) 132:
        case (TypeCode) 151:
        case (TypeCode) 170:
        case (TypeCode) 189:
        case (TypeCode) 208:
        case (TypeCode) 227:
        case (TypeCode) 246:
        case (TypeCode) 265:
        case (TypeCode) 284:
        case (TypeCode) 303:
          return Operators.IntDivideInt64(Convert.ToInt64(Left), Conversions.ToLong(Convert.ToString(Right)));
        case (TypeCode) 114:
          return Operators.IntDivideByte(Convert.ToByte(Left), (byte) 0);
        case (TypeCode) 117:
        case (TypeCode) 136:
          return Operators.IntDivideInt16(Convert.ToInt16(Left), (short) Operators.ToVBBool(Right));
        case (TypeCode) 120:
          return Operators.IntDivideByte(Convert.ToByte(Left), Convert.ToByte(Right));
        case (TypeCode) 122:
        case (TypeCode) 158:
        case (TypeCode) 160:
          return Operators.IntDivideUInt16(Convert.ToUInt16(Left), Convert.ToUInt16(Right));
        case (TypeCode) 124:
        case (TypeCode) 162:
        case (TypeCode) 196:
        case (TypeCode) 198:
        case (TypeCode) 200:
          return Operators.IntDivideUInt32(Convert.ToUInt32(Left), Convert.ToUInt32(Right));
        case (TypeCode) 126:
        case (TypeCode) 164:
        case (TypeCode) 202:
        case (TypeCode) 234:
        case (TypeCode) 236:
        case (TypeCode) 238:
        case (TypeCode) 240:
          return Operators.IntDivideUInt64(Convert.ToUInt64(Left), Convert.ToUInt64(Right));
        case (TypeCode) 133:
          return Operators.IntDivideInt16(Convert.ToInt16(Left), (short) 0);
        case (TypeCode) 152:
          return Operators.IntDivideUInt16(Convert.ToUInt16(Left), (ushort) 0);
        case (TypeCode) 155:
        case (TypeCode) 174:
          return Operators.IntDivideInt32(Convert.ToInt32(Left), (int) Operators.ToVBBool(Right));
        case (TypeCode) 171:
          return Operators.IntDivideInt32(Convert.ToInt32(Left), 0);
        case (TypeCode) 190:
          return Operators.IntDivideUInt32(Convert.ToUInt32(Left), 0U);
        case (TypeCode) 193:
        case (TypeCode) 212:
        case (TypeCode) 231:
        case (TypeCode) 250:
        case (TypeCode) 269:
        case (TypeCode) 288:
          return Operators.IntDivideInt64(Convert.ToInt64(Left), (long) Operators.ToVBBool(Right));
        case (TypeCode) 209:
          return Operators.IntDivideInt64(Convert.ToInt64(Left), 0L);
        case (TypeCode) 228:
          return Operators.IntDivideUInt64(Convert.ToUInt64(Left), 0UL);
        case (TypeCode) 247:
        case (TypeCode) 266:
        case (TypeCode) 285:
          return Operators.IntDivideInt64(Convert.ToInt64(Left), 0L);
        case (TypeCode) 342:
          return Operators.IntDivideInt64(Conversions.ToLong(Convert.ToString(Left)), 0L);
        case (TypeCode) 345:
          return Operators.IntDivideInt64(Conversions.ToLong(Convert.ToString(Left)), (long) Operators.ToVBBool(Right));
        case (TypeCode) 347:
        case (TypeCode) 348:
        case (TypeCode) 349:
        case (TypeCode) 350:
        case (TypeCode) 351:
        case (TypeCode) 352:
        case (TypeCode) 353:
        case (TypeCode) 354:
        case (TypeCode) 355:
        case (TypeCode) 356:
        case (TypeCode) 357:
          return Operators.IntDivideInt64(Conversions.ToLong(Convert.ToString(Left)), Convert.ToInt64(Right));
        case (TypeCode) 360:
          return Operators.IntDivideInt64(Conversions.ToLong(Convert.ToString(Left)), Conversions.ToLong(Convert.ToString(Right)));
        default:
          if (typeCode1 != TypeCode.Object && typeCode2 != TypeCode.Object)
            throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.IntegralDivide, Left, Right);
          return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.IntegralDivide, Left, Right);
      }
    }

    private static object IntDivideSByte(sbyte left, sbyte right)
    {
      return left != sbyte.MinValue || right != (sbyte) -1 ? (object) checked ((sbyte) unchecked ((int) left / (int) right)) : (object) (short) 128;
    }

    private static object IntDivideByte(byte left, byte right)
    {
      return (object) checked ((byte) unchecked ((uint) left / (uint) right));
    }

    private static object IntDivideInt16(short left, short right)
    {
      return left != short.MinValue || right != (short) -1 ? (object) checked ((short) unchecked ((int) left / (int) right)) : (object) 32768;
    }

    private static object IntDivideUInt16(ushort left, ushort right)
    {
      return (object) checked ((ushort) unchecked ((uint) left / (uint) right));
    }

    private static object IntDivideInt32(int left, int right)
    {
      return left != int.MinValue || right != -1 ? (object) (left / right) : (object) 2147483648L;
    }

    private static object IntDivideUInt32(uint left, uint right)
    {
      return (object) (left / right);
    }

    private static object IntDivideInt64(long left, long right)
    {
      return (object) (left / right);
    }

    private static object IntDivideUInt64(ulong left, ulong right)
    {
      return (object) (left / right);
    }

    /// <summary>Represents the Visual Basic arithmetic left shift (&lt;&lt;) operator.</summary>
    /// <param name="Operand">Required. Integral numeric expression. The bit pattern to be shifted. The data type must be an integral type (<see langword="SByte" />, <see langword="Byte" />, <see langword="Short" />, <see langword="UShort" />, <see langword="Integer" />, <see langword="UInteger" />, <see langword="Long" />, or <see langword="ULong" />).</param>
    /// <param name="Amount">Required. Numeric expression. The number of bits to shift the bit pattern. The data type must be <see langword="Integer" /> or widen to <see langword="Integer" />.</param>
    /// <returns>An integral numeric value. The result of shifting the bit pattern. The data type is the same as that of <paramref name="Operand" />.</returns>
    public static object LeftShiftObject(object Operand, object Amount)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Operand);
      TypeCode typeCode2 = Operators.GetTypeCode(Amount);
      if (typeCode1 == TypeCode.Object || typeCode2 == TypeCode.Object)
        return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.ShiftLeft, Operand, Amount);
      switch (typeCode1)
      {
        case TypeCode.Empty:
          return (object) (0 << Conversions.ToInteger(Amount));
        case TypeCode.Boolean:
          return (object) (short) ((int) (short) -(Convert.ToBoolean(Operand) ? 1 : 0) << (Conversions.ToInteger(Amount) & 15));
        case TypeCode.SByte:
          return (object) (sbyte) ((int) Convert.ToSByte(Operand) << (Conversions.ToInteger(Amount) & 7));
        case TypeCode.Byte:
          return (object) (byte) ((uint) Convert.ToByte(Operand) << (Conversions.ToInteger(Amount) & 7));
        case TypeCode.Int16:
          return (object) (short) ((int) Convert.ToInt16(Operand) << (Conversions.ToInteger(Amount) & 15));
        case TypeCode.UInt16:
          return (object) (ushort) ((uint) Convert.ToUInt16(Operand) << (Conversions.ToInteger(Amount) & 15));
        case TypeCode.Int32:
          return (object) (Convert.ToInt32(Operand) << Conversions.ToInteger(Amount));
        case TypeCode.UInt32:
          return (object) (uint) ((int) Convert.ToUInt32(Operand) << Conversions.ToInteger(Amount));
        case TypeCode.Int64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return (object) (Convert.ToInt64(Operand) << Conversions.ToInteger(Amount));
        case TypeCode.UInt64:
          return (object) (ulong) ((long) Convert.ToUInt64(Operand) << Conversions.ToInteger(Amount));
        case TypeCode.String:
          return (object) (Conversions.ToLong(Convert.ToString(Operand)) << Conversions.ToInteger(Amount));
        default:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.ShiftLeft, Operand);
      }
    }

    /// <summary>Represents the Visual Basic arithmetic right shift (&gt;&gt;) operator.</summary>
    /// <param name="Operand">Required. Integral numeric expression. The bit pattern to be shifted. The data type must be an integral type (<see langword="SByte" />, <see langword="Byte" />, <see langword="Short" />, <see langword="UShort" />, <see langword="Integer" />, <see langword="UInteger" />, <see langword="Long" />, or <see langword="ULong" />).</param>
    /// <param name="Amount">Required. Numeric expression. The number of bits to shift the bit pattern. The data type must be <see langword="Integer" /> or widen to <see langword="Integer" />.</param>
    /// <returns>An integral numeric value. The result of shifting the bit pattern. The data type is the same as that of <paramref name="Operand" />.</returns>
    public static object RightShiftObject(object Operand, object Amount)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Operand);
      TypeCode typeCode2 = Operators.GetTypeCode(Amount);
      if (typeCode1 == TypeCode.Object || typeCode2 == TypeCode.Object)
        return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.ShiftRight, Operand, Amount);
      switch (typeCode1)
      {
        case TypeCode.Empty:
          return (object) (0 >> Conversions.ToInteger(Amount));
        case TypeCode.Boolean:
          return (object) (short) ((int) (short) -(Convert.ToBoolean(Operand) ? 1 : 0) >> (Conversions.ToInteger(Amount) & 15));
        case TypeCode.SByte:
          return (object) (sbyte) ((int) Convert.ToSByte(Operand) >> (Conversions.ToInteger(Amount) & 7));
        case TypeCode.Byte:
          return (object) (byte) ((uint) Convert.ToByte(Operand) >> (Conversions.ToInteger(Amount) & 7));
        case TypeCode.Int16:
          return (object) (short) ((int) Convert.ToInt16(Operand) >> (Conversions.ToInteger(Amount) & 15));
        case TypeCode.UInt16:
          return (object) (ushort) ((uint) Convert.ToUInt16(Operand) >> (Conversions.ToInteger(Amount) & 15));
        case TypeCode.Int32:
          return (object) (Convert.ToInt32(Operand) >> Conversions.ToInteger(Amount));
        case TypeCode.UInt32:
          return (object) (Convert.ToUInt32(Operand) >> Conversions.ToInteger(Amount));
        case TypeCode.Int64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return (object) (Convert.ToInt64(Operand) >> Conversions.ToInteger(Amount));
        case TypeCode.UInt64:
          return (object) (Convert.ToUInt64(Operand) >> Conversions.ToInteger(Amount));
        case TypeCode.String:
          return (object) (Conversions.ToLong(Convert.ToString(Operand)) >> Conversions.ToInteger(Amount));
        default:
          throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.ShiftRight, Operand);
      }
    }

    /// <summary>Represents the Visual Basic concatenation (&amp;) operator.</summary>
    /// <param name="Left">Required. Any expression.</param>
    /// <param name="Right">Required. Any expression.</param>
    /// <returns>A string representing the concatenation of <paramref name="Left" /> and <paramref name="Right" />.</returns>
    public static object ConcatenateObject(object Left, object Right)
    {
      TypeCode typeCode1 = Operators.GetTypeCode(Left);
      TypeCode typeCode2 = Operators.GetTypeCode(Right);
      if (typeCode1 == TypeCode.Object && Left is char[])
        typeCode1 = TypeCode.String;
      if (typeCode2 == TypeCode.Object && Right is char[])
        typeCode2 = TypeCode.String;
      object obj;
      if (typeCode1 == TypeCode.Object || typeCode2 == TypeCode.Object)
        obj = Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Concatenate, Left, Right);
      else
        obj = (object) (Conversions.ToString(Left) + Conversions.ToString(Right));
      return obj;
    }

    private enum CompareClass
    {
      Less = -1, // 0xFFFFFFFF
      Equal = 0,
      Greater = 1,
      Unordered = 2,
      UserDefined = 3,
      Undefined = 4,
    }
  }
}
