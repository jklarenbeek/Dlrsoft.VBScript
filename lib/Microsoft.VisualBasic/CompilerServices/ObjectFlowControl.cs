// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.ObjectFlowControl
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>The Visual Basic compiler uses this class for object flow control; it is not meant to be called directly from your code.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class ObjectFlowControl
  {
    /// <summary>Checks for a synchronization lock on the specified type.</summary>
    /// <param name="Expression">The data type for which to check for synchronization lock.</param>
    public static void CheckForSyncLockOnValueType(object Expression)
    {
      if (Expression != null && Expression.GetType().IsValueType)
        throw new ArgumentException(Utils.GetResourceString(SR.SyncLockRequiresReferenceType1, Utils.VBFriendlyName(Expression.GetType())));
    }

    /// <summary>Provides services to the Visual Basic compiler for compiling <see langword="For...Next" /> loops.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class ForLoopControl
    {
      private object _counter;
      private object _limit;
      private object _stepValue;
      private bool _positiveStep;
      private Type _enumType;
      private Type _widestType;
      private TypeCode _widestTypeCode;
      private bool _useUserDefinedOperators;
      private Symbols.Method _operatorPlus;
      private Symbols.Method _operatorGreaterEqual;
      private Symbols.Method _operatorLessEqual;

      private ForLoopControl()
      {
      }

      private static Type GetWidestType(Type type1, Type type2)
      {
        Type type;
        if ((object) type1 == null || (object) type2 == null)
        {
          type = (Type) null;
        }
        else
        {
          if (!type1.IsEnum && !type2.IsEnum)
          {
            TypeCode typeCode1 = type1.GetTypeCode();
            TypeCode typeCode2 = type2.GetTypeCode();
            if (Symbols.IsNumericType(typeCode1) && Symbols.IsNumericType(typeCode2))
            {
              type = Symbols.MapTypeCodeToType(ConversionResolution.ForLoopWidestTypeCode[(int) typeCode1][(int) typeCode2]);
              goto label_8;
            }
          }
          Type targetType1 = type2;
          Type sourceType1 = type1;
          Symbols.Method method1 = (Symbols.Method) null;
          ref Symbols.Method local1 = ref method1;
          switch (ConversionResolution.ClassifyConversion(targetType1, sourceType1, ref local1))
          {
            case ConversionResolution.ConversionClass.Identity:
            case ConversionResolution.ConversionClass.Widening:
              type = type2;
              break;
            default:
              Type targetType2 = type1;
              Type sourceType2 = type2;
              Symbols.Method method2 = (Symbols.Method) null;
              ref Symbols.Method local2 = ref method2;
              type = ConversionResolution.ClassifyConversion(targetType2, sourceType2, ref local2) != ConversionResolution.ConversionClass.Widening ? (Type) null : type1;
              break;
          }
        }
label_8:
        return type;
      }

      private static Type GetWidestType(Type type1, Type type2, Type type3)
      {
        return ObjectFlowControl.ForLoopControl.GetWidestType(type1, ObjectFlowControl.ForLoopControl.GetWidestType(type2, type3));
      }

      private static object ConvertLoopElement(
        string elementName,
        object value,
        Type sourceType,
        Type targetType)
      {
        try
        {
          return Conversions.ChangeType(value, targetType);
        }
        catch (OutOfMemoryException ex)
        {
          throw ex;
        }
        catch (Exception ex)
        {
          throw new ArgumentException(Utils.GetResourceString(SR.ForLoop_ConvertToType3, elementName, Utils.VBFriendlyName(sourceType), Utils.VBFriendlyName(targetType)));
        }
      }

      private static Symbols.Method VerifyForLoopOperator(
        Symbols.UserDefinedOperator op,
        object forLoopArgument,
        Type forLoopArgumentType)
      {
        Symbols.Method userDefinedOperator = Operators.GetCallableUserDefinedOperator(op, forLoopArgument, forLoopArgument);
        if ((object) userDefinedOperator == null)
          throw new ArgumentException(Utils.GetResourceString(SR.ForLoop_OperatorRequired2, Utils.VBFriendlyNameOfType(forLoopArgumentType, true), Symbols.OperatorNames[(int) op]));
        MethodInfo methodInfo = userDefinedOperator.AsMethod() as MethodInfo;
        ParameterInfo[] parameters = methodInfo.GetParameters();
        switch (op)
        {
          case Symbols.UserDefinedOperator.Plus:
          case Symbols.UserDefinedOperator.Minus:
            if (parameters.Length != 2 || (object) parameters[0].ParameterType != (object) forLoopArgumentType || ((object) parameters[1].ParameterType != (object) forLoopArgumentType || (object) methodInfo.ReturnType != (object) forLoopArgumentType))
              throw new ArgumentException(Utils.GetResourceString(SR.ForLoop_UnacceptableOperator2, userDefinedOperator.ToString(), Utils.VBFriendlyNameOfType(forLoopArgumentType, true)));
            break;
          case Symbols.UserDefinedOperator.LessEqual:
          case Symbols.UserDefinedOperator.GreaterEqual:
            if (parameters.Length != 2 || (object) parameters[0].ParameterType != (object) forLoopArgumentType || (object) parameters[1].ParameterType != (object) forLoopArgumentType)
              throw new ArgumentException(Utils.GetResourceString(SR.ForLoop_UnacceptableRelOperator2, userDefinedOperator.ToString(), Utils.VBFriendlyNameOfType(forLoopArgumentType, true)));
            break;
        }
        return userDefinedOperator;
      }

      /// <summary>Initializes a <see langword="For...Next" /> loop.</summary>
      /// <param name="Counter">The loop counter variable.</param>
      /// <param name="Start">The initial value of the loop counter.</param>
      /// <param name="Limit">The value of the <see langword="To" /> option.</param>
      /// <param name="StepValue">The value of the <see langword="Step" /> option.</param>
      /// <param name="LoopForResult">An object that contains verified values for loop values.</param>
      /// <param name="CounterResult">The counter value for the next loop iteration.</param>
      /// <returns>
      /// <see langword="False" /> if the loop has terminated; otherwise, <see langword="True" />.</returns>
      public static bool ForLoopInitObj(
        object Counter,
        object Start,
        object Limit,
        object StepValue,
        ref object LoopForResult,
        ref object CounterResult)
      {
        if (Start == null)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidNullValue1, nameof (Start)));
        if (Limit == null)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidNullValue1, nameof (Limit)));
        if (StepValue == null)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidNullValue1, "Step"));
        Type type1 = Start.GetType();
        Type type2 = Limit.GetType();
        Type type3 = StepValue.GetType();
        Type widestType = ObjectFlowControl.ForLoopControl.GetWidestType(type3, type1, type2);
        if ((object) widestType == null)
          throw new ArgumentException(Utils.GetResourceString(SR.ForLoop_CommonType3, Utils.VBFriendlyName(type1), Utils.VBFriendlyName(type2), Utils.VBFriendlyName(StepValue)));
        ObjectFlowControl.ForLoopControl loopFor = new ObjectFlowControl.ForLoopControl();
        TypeCode typeCode1 = widestType.GetTypeCode();
        if (typeCode1 == TypeCode.Object)
          loopFor._useUserDefinedOperators = true;
        if (typeCode1 == TypeCode.String)
          typeCode1 = TypeCode.Double;
        TypeCode typeCode2 = type1.GetTypeCode();
        int typeCode3 = (int) type2.GetTypeCode();
        TypeCode typeCode4 = type3.GetTypeCode();
        Type type4 = (Type) null;
        if (typeCode2 == typeCode1 && type1.IsEnum)
          type4 = type1;
        int num = (int) typeCode1;
        if (typeCode3 == num && type2.IsEnum)
        {
          if ((object) type4 != null && (object) type4 != (object) type2)
          {
            type4 = (Type) null;
            goto label_20;
          }
          else
            type4 = type2;
        }
        if (typeCode4 == typeCode1 && type3.IsEnum)
          type4 = (object) type4 == null || (object) type4 == (object) type3 ? type3 : (Type) null;
label_20:
        loopFor._enumType = type4;
        loopFor._widestType = loopFor._useUserDefinedOperators ? widestType : Symbols.MapTypeCodeToType(typeCode1);
        loopFor._widestTypeCode = typeCode1;
        loopFor._counter = ObjectFlowControl.ForLoopControl.ConvertLoopElement(nameof (Start), Start, type1, loopFor._widestType);
        loopFor._limit = ObjectFlowControl.ForLoopControl.ConvertLoopElement(nameof (Limit), Limit, type2, loopFor._widestType);
        loopFor._stepValue = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Step", StepValue, type3, loopFor._widestType);
        if (loopFor._useUserDefinedOperators)
        {
          loopFor._operatorPlus = ObjectFlowControl.ForLoopControl.VerifyForLoopOperator(Symbols.UserDefinedOperator.Plus, loopFor._counter, loopFor._widestType);
          ObjectFlowControl.ForLoopControl.VerifyForLoopOperator(Symbols.UserDefinedOperator.Minus, loopFor._counter, loopFor._widestType);
          loopFor._operatorLessEqual = ObjectFlowControl.ForLoopControl.VerifyForLoopOperator(Symbols.UserDefinedOperator.LessEqual, loopFor._counter, loopFor._widestType);
          loopFor._operatorGreaterEqual = ObjectFlowControl.ForLoopControl.VerifyForLoopOperator(Symbols.UserDefinedOperator.GreaterEqual, loopFor._counter, loopFor._widestType);
        }
        loopFor._positiveStep = Operators.ConditionalCompareObjectGreaterEqual(loopFor._stepValue, Operators.SubtractObject(loopFor._stepValue, loopFor._stepValue), false);
        LoopForResult = (object) loopFor;
        CounterResult = (object) loopFor._enumType == null ? loopFor._counter : Enum.ToObject(loopFor._enumType, loopFor._counter);
        return ObjectFlowControl.ForLoopControl.CheckContinueLoop(loopFor);
      }

      /// <summary>Increments a <see langword="For...Next" /> loop.</summary>
      /// <param name="Counter">The loop counter variable.</param>
      /// <param name="LoopObj">An object that contains verified values for loop values.</param>
      /// <param name="CounterResult">The counter value for the next loop iteration.</param>
      /// <returns>
      /// <see langword="False" /> if the loop has terminated; otherwise, <see langword="True" />.</returns>
      public static bool ForNextCheckObj(object Counter, object LoopObj, ref object CounterResult)
      {
        if (LoopObj == null)
          throw ExceptionUtils.VbMakeIllegalForException();
        if (Counter == null)
          throw new NullReferenceException(Utils.GetResourceString(SR.Argument_InvalidNullValue1, nameof (Counter)));
        ObjectFlowControl.ForLoopControl loopFor = (ObjectFlowControl.ForLoopControl) LoopObj;
        bool flag1 = false;
        if (!loopFor._useUserDefinedOperators)
        {
          TypeCode typeCode1 = Counter.GetType().GetTypeCode();
          if (typeCode1 != loopFor._widestTypeCode || typeCode1 == TypeCode.String)
          {
            if (typeCode1 == TypeCode.Object)
              throw new ArgumentException(Utils.GetResourceString(SR.ForLoop_CommonType2, Utils.VBFriendlyName(Symbols.MapTypeCodeToType(typeCode1)), Utils.VBFriendlyName(loopFor._widestType)));
            TypeCode typeCode2 = ObjectFlowControl.ForLoopControl.GetWidestType(Symbols.MapTypeCodeToType(typeCode1), loopFor._widestType).GetTypeCode();
            if (typeCode2 == TypeCode.String)
              typeCode2 = TypeCode.Double;
            loopFor._widestTypeCode = typeCode2;
            loopFor._widestType = Symbols.MapTypeCodeToType(typeCode2);
            flag1 = true;
          }
        }
        if (flag1 || loopFor._useUserDefinedOperators)
        {
          Counter = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Start", Counter, Counter.GetType(), loopFor._widestType);
          if (!loopFor._useUserDefinedOperators)
          {
            loopFor._limit = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Limit", loopFor._limit, loopFor._limit.GetType(), loopFor._widestType);
            loopFor._stepValue = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Step", loopFor._stepValue, loopFor._stepValue.GetType(), loopFor._widestType);
          }
        }
        bool flag2;
        if (!loopFor._useUserDefinedOperators)
        {
          loopFor._counter = Operators.AddObject(Counter, loopFor._stepValue);
          TypeCode typeCode = loopFor._counter.GetType().GetTypeCode();
          CounterResult = (object) loopFor._enumType == null ? loopFor._counter : Enum.ToObject(loopFor._enumType, loopFor._counter);
          if (typeCode != loopFor._widestTypeCode)
          {
            loopFor._limit = Conversions.ChangeType(loopFor._limit, Symbols.MapTypeCodeToType(typeCode));
            loopFor._stepValue = Conversions.ChangeType(loopFor._stepValue, Symbols.MapTypeCodeToType(typeCode));
            flag2 = false;
            goto label_21;
          }
        }
        else
        {
          loopFor._counter = Operators.InvokeUserDefinedOperator(loopFor._operatorPlus, true, Counter, loopFor._stepValue);
          if ((object) loopFor._counter.GetType() != (object) loopFor._widestType)
            loopFor._counter = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Start", loopFor._counter, loopFor._counter.GetType(), loopFor._widestType);
          CounterResult = loopFor._counter;
        }
        flag2 = ObjectFlowControl.ForLoopControl.CheckContinueLoop(loopFor);
label_21:
        return flag2;
      }

      /// <summary>Checks for valid values for the loop counter, <see langword="Step" />, and <see langword="To" /> values.</summary>
      /// <param name="count">Required. A <see langword="Single" /> value that represents the initial value passed for the loop counter variable.</param>
      /// <param name="limit">Required. A <see langword="Single" /> value that represents the value passed by using the <see langword="To" /> keyword.</param>
      /// <param name="StepValue">Required. A <see langword="Single" /> value that represents the value passed by using the <see langword="Step" /> keyword.</param>
      /// <returns>
      /// <see langword="True" /> if <paramref name="StepValue" /> is greater than zero and <paramref name="count" /> is less than or equal to <paramref name="limit" />, or if <paramref name="StepValue" /> is less than or equal to zero and <paramref name="count" /> is greater than or equal to <paramref name="limit" />; otherwise, <see langword="False" />.</returns>
      public static bool ForNextCheckR4(float count, float limit, float StepValue)
      {
        return (double) StepValue < 0.0 ? (double) count >= (double) limit : (double) count <= (double) limit;
      }

      /// <summary>Checks for valid values for the loop counter, <see langword="Step" />, and <see langword="To" /> values.</summary>
      /// <param name="count">Required. A <see langword="Double" /> value that represents the initial value passed for the loop counter variable.</param>
      /// <param name="limit">Required. A <see langword="Double" /> value that represents the value passed by using the <see langword="To" /> keyword.</param>
      /// <param name="StepValue">Required. A <see langword="Double" /> value that represents the value passed by using the <see langword="Step" /> keyword.</param>
      /// <returns>
      /// <see langword="True" /> if <paramref name="StepValue" /> is greater than zero and <paramref name="count" /> is less than or equal to <paramref name="limit" />, or if <paramref name="StepValue" /> is less than or equal to zero and <paramref name="count" /> is greater than or equal to <paramref name="limit" />; otherwise, <see langword="False" />.</returns>
      public static bool ForNextCheckR8(double count, double limit, double StepValue)
      {
        return StepValue < 0.0 ? count >= limit : count <= limit;
      }

      /// <summary>Checks for valid values for the loop counter, <see langword="Step" />, and <see langword="To" /> values.</summary>
      /// <param name="count">Required. A <see langword="Decimal" /> value that represents the initial value passed for the loop counter variable.</param>
      /// <param name="limit">Required. A <see langword="Decimal" /> value that represents the value passed by using the <see langword="To" /> keyword.</param>
      /// <param name="StepValue">Required. A <see langword="Decimal" /> value that represents the value passed by using the <see langword="Step" /> keyword.</param>
      /// <returns>
      /// <see langword="True" /> if <paramref name="StepValue" /> is greater than zero and <paramref name="count" /> is less than or equal to <paramref name="limit" /> or <paramref name="StepValue" /> is less than or equal to zero and <paramref name="count" /> is greater than or equal to <paramref name="limit" />; otherwise, <see langword="False" />.</returns>
      public static bool ForNextCheckDec(Decimal count, Decimal limit, Decimal StepValue)
      {
        return Decimal.Compare(StepValue, Decimal.Zero) < 0 ? Decimal.Compare(count, limit) >= 0 : Decimal.Compare(count, limit) <= 0;
      }

      private static bool CheckContinueLoop(ObjectFlowControl.ForLoopControl loopFor)
      {
        bool flag;
        if (!loopFor._useUserDefinedOperators)
        {
          try
          {
            int num = ((IComparable) loopFor._counter).CompareTo(loopFor._limit);
            flag = !loopFor._positiveStep ? num >= 0 : num <= 0;
          }
          catch (InvalidCastException ex)
          {
            throw new ArgumentException(Utils.GetResourceString(SR.Argument_IComparable2, "loop control variable", Utils.VBFriendlyName(loopFor._counter)));
          }
        }
        else if (loopFor._positiveStep)
          flag = Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(loopFor._operatorLessEqual, true, loopFor._counter, loopFor._limit));
        else
          flag = Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(loopFor._operatorGreaterEqual, true, loopFor._counter, loopFor._limit));
        return flag;
      }
    }
  }
}
