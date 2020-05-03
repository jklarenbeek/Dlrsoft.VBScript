// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.OverloadResolution
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal class OverloadResolution
  {
    private static bool IsExactSignatureMatch(
      ParameterInfo[] leftSignature,
      int leftTypeParameterCount,
      ParameterInfo[] rightSignature,
      int rightTypeParameterCount)
    {
      ParameterInfo[] parameterInfoArray1;
      ParameterInfo[] parameterInfoArray2;
      if (leftSignature.Length >= rightSignature.Length)
      {
        parameterInfoArray1 = leftSignature;
        parameterInfoArray2 = rightSignature;
      }
      else
      {
        parameterInfoArray1 = rightSignature;
        parameterInfoArray2 = leftSignature;
      }
      int length = parameterInfoArray2.Length;
      int num1 = checked (parameterInfoArray1.Length - 1);
      int index1 = length;
      bool flag;
      while (index1 <= num1)
      {
        if (!parameterInfoArray1[index1].IsOptional)
        {
          flag = false;
          goto label_18;
        }
        else
          checked { ++index1; }
      }
      int num2 = checked (parameterInfoArray2.Length - 1);
      int index2 = 0;
      while (index2 <= num2)
      {
        Type type1 = parameterInfoArray2[index2].ParameterType;
        Type type2 = parameterInfoArray1[index2].ParameterType;
        if (type1.IsByRef)
          type1 = type1.GetElementType();
        if (type2.IsByRef)
          type2 = type2.GetElementType();
        if ((object) type1 != (object) type2 && (!parameterInfoArray2[index2].IsOptional || !parameterInfoArray1[index2].IsOptional))
        {
          flag = false;
          goto label_18;
        }
        else
          checked { ++index2; }
      }
      flag = true;
label_18:
      return flag;
    }

    private static void CompareNumericTypeSpecificity(
      Type leftType,
      Type rightType,
      ref bool leftWins,
      ref bool rightWins)
    {
      if ((object) leftType == (object) rightType)
        return;
      if (ConversionResolution.NumericSpecificityRank[(int) leftType.GetTypeCode()] < ConversionResolution.NumericSpecificityRank[(int) rightType.GetTypeCode()])
        leftWins = true;
      else
        rightWins = true;
    }

    private static void CompareParameterSpecificity(
      Type argumentType,
      ParameterInfo leftParameter,
      MethodBase leftProcedure,
      bool expandLeftParamArray,
      ParameterInfo rightParameter,
      MethodBase rightProcedure,
      bool expandRightParamArray,
      ref bool leftWins,
      ref bool rightWins,
      ref bool bothLose)
    {
      bothLose = false;
      Type type1 = leftParameter.ParameterType;
      Type type2 = rightParameter.ParameterType;
      if (type1.IsByRef)
        type1 = Symbols.GetElementType(type1);
      if (type2.IsByRef)
        type2 = Symbols.GetElementType(type2);
      if (expandLeftParamArray && Symbols.IsParamArray(leftParameter))
        type1 = Symbols.GetElementType(type1);
      if (expandRightParamArray && Symbols.IsParamArray(rightParameter))
        type2 = Symbols.GetElementType(type2);
      if (Symbols.IsNumericType(type1) && Symbols.IsNumericType(type2) && (!Symbols.IsEnum(type1) && !Symbols.IsEnum(type2)))
      {
        OverloadResolution.CompareNumericTypeSpecificity(type1, type2, ref leftWins, ref rightWins);
      }
      else
      {
        if ((object) leftProcedure != null && (object) rightProcedure != null && (Symbols.IsRawGeneric(leftProcedure) && Symbols.IsRawGeneric(rightProcedure)))
        {
          if ((object) type1 == (object) type2)
            return;
          int num1 = Symbols.IndexIn(type1, leftProcedure);
          int num2 = Symbols.IndexIn(type2, rightProcedure);
          if (num1 == num2 && num1 >= 0)
            return;
        }
        Symbols.Method operatorMethod = (Symbols.Method) null;
        switch (ConversionResolution.ClassifyConversion(type2, type1, ref operatorMethod))
        {
          case ConversionResolution.ConversionClass.Identity:
            break;
          case ConversionResolution.ConversionClass.Widening:
            if ((object) operatorMethod != null && ConversionResolution.ClassifyConversion(type1, type2, ref operatorMethod) == ConversionResolution.ConversionClass.Widening)
            {
              if ((object) argumentType != null && (object) argumentType == (object) type1)
              {
                leftWins = true;
                break;
              }
              if ((object) argumentType != null && (object) argumentType == (object) type2)
              {
                rightWins = true;
                break;
              }
              bothLose = true;
              break;
            }
            leftWins = true;
            break;
          default:
            if (ConversionResolution.ClassifyConversion(type1, type2, ref operatorMethod) == ConversionResolution.ConversionClass.Widening)
            {
              rightWins = true;
              break;
            }
            bothLose = true;
            break;
        }
      }
    }

    private static void CompareGenericityBasedOnMethodGenericParams(
      ParameterInfo leftParameter,
      ParameterInfo rawLeftParameter,
      Symbols.Method leftMember,
      bool expandLeftParamArray,
      ParameterInfo rightParameter,
      ParameterInfo rawRightParameter,
      Symbols.Method rightMember,
      bool expandRightParamArray,
      ref bool leftIsLessGeneric,
      ref bool rightIsLessGeneric,
      ref bool signatureMismatch)
    {
      if (!leftMember.IsMethod || !rightMember.IsMethod)
        return;
      Type type1 = leftParameter.ParameterType;
      Type type2 = rightParameter.ParameterType;
      Type type3 = rawLeftParameter.ParameterType;
      Type type4 = rawRightParameter.ParameterType;
      if (type1.IsByRef)
      {
        type1 = Symbols.GetElementType(type1);
        type3 = Symbols.GetElementType(type3);
      }
      if (type2.IsByRef)
      {
        type2 = Symbols.GetElementType(type2);
        type4 = Symbols.GetElementType(type4);
      }
      if (expandLeftParamArray && Symbols.IsParamArray(leftParameter))
      {
        type1 = Symbols.GetElementType(type1);
        type3 = Symbols.GetElementType(type3);
      }
      if (expandRightParamArray && Symbols.IsParamArray(rightParameter))
      {
        type2 = Symbols.GetElementType(type2);
        type4 = Symbols.GetElementType(type4);
      }
      if ((object) type1 != (object) type2)
      {
        signatureMismatch = true;
      }
      else
      {
        MethodBase method1 = leftMember.AsMethod();
        MethodBase method2 = rightMember.AsMethod();
        if (Symbols.IsGeneric(method1))
          method1 = (MethodBase) ((MethodInfo) method1).GetGenericMethodDefinition();
        if (Symbols.IsGeneric(method2))
          method2 = (MethodBase) ((MethodInfo) method2).GetGenericMethodDefinition();
        if (Symbols.RefersToGenericParameter(type3, method1))
        {
          if (Symbols.RefersToGenericParameter(type4, method2))
            return;
          rightIsLessGeneric = true;
        }
        else
        {
          if (!Symbols.RefersToGenericParameter(type4, method2) || Symbols.RefersToGenericParameter(type3, method1))
            return;
          leftIsLessGeneric = true;
        }
      }
    }

    private static void CompareGenericityBasedOnTypeGenericParams(
      ParameterInfo leftParameter,
      ParameterInfo rawLeftParameter,
      Symbols.Method leftMember,
      bool expandLeftParamArray,
      ParameterInfo rightParameter,
      ParameterInfo rawRightParameter,
      Symbols.Method rightMember,
      bool expandRightParamArray,
      ref bool leftIsLessGeneric,
      ref bool rightIsLessGeneric,
      ref bool signatureMismatch)
    {
      Type type1 = leftParameter.ParameterType;
      Type type2 = rightParameter.ParameterType;
      Type type3 = rawLeftParameter.ParameterType;
      Type type4 = rawRightParameter.ParameterType;
      if (type1.IsByRef)
      {
        type1 = Symbols.GetElementType(type1);
        type3 = Symbols.GetElementType(type3);
      }
      if (type2.IsByRef)
      {
        type2 = Symbols.GetElementType(type2);
        type4 = Symbols.GetElementType(type4);
      }
      if (expandLeftParamArray && Symbols.IsParamArray(leftParameter))
      {
        type1 = Symbols.GetElementType(type1);
        type3 = Symbols.GetElementType(type3);
      }
      if (expandRightParamArray && Symbols.IsParamArray(rightParameter))
      {
        type2 = Symbols.GetElementType(type2);
        type4 = Symbols.GetElementType(type4);
      }
      if ((object) type1 != (object) type2)
      {
        signatureMismatch = true;
      }
      else
      {
        Type rawDeclaringType1 = leftMember.RawDeclaringType;
        Type rawDeclaringType2 = rightMember.RawDeclaringType;
        if (Symbols.RefersToGenericParameterCLRSemantics(type3, rawDeclaringType1))
        {
          if (Symbols.RefersToGenericParameterCLRSemantics(type4, rawDeclaringType2))
            return;
          rightIsLessGeneric = true;
        }
        else
        {
          if (!Symbols.RefersToGenericParameterCLRSemantics(type4, rawDeclaringType2))
            return;
          leftIsLessGeneric = true;
        }
      }
    }

    private static Symbols.Method LeastGenericProcedure(
      Symbols.Method left,
      Symbols.Method right,
      OverloadResolution.ComparisonType compareGenericity,
      ref bool signatureMismatch)
    {
      bool leftIsLessGeneric = false;
      bool rightIsLessGeneric = false;
      signatureMismatch = false;
      Symbols.Method method;
      if (!left.IsMethod || !right.IsMethod)
      {
        method = (Symbols.Method) null;
      }
      else
      {
        int index = 0;
        int length1 = left.Parameters.Length;
        int length2 = right.Parameters.Length;
        while (index < length1 && index < length2)
        {
          switch (compareGenericity)
          {
            case OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams:
              OverloadResolution.CompareGenericityBasedOnMethodGenericParams(left.Parameters[index], left.RawParameters[index], left, left.ParamArrayExpanded, right.Parameters[index], right.RawParameters[index], right, false, ref leftIsLessGeneric, ref rightIsLessGeneric, ref signatureMismatch);
              break;
            case OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams:
              OverloadResolution.CompareGenericityBasedOnTypeGenericParams(left.Parameters[index], left.RawParameters[index], left, left.ParamArrayExpanded, right.Parameters[index], right.RawParameters[index], right, false, ref leftIsLessGeneric, ref rightIsLessGeneric, ref signatureMismatch);
              break;
          }
          if (signatureMismatch || leftIsLessGeneric && rightIsLessGeneric)
          {
            method = (Symbols.Method) null;
            goto label_11;
          }
          else
            checked { ++index; }
        }
        method = index < length1 || index < length2 ? (Symbols.Method) null : (!leftIsLessGeneric ? (!rightIsLessGeneric ? (Symbols.Method) null : right) : left);
      }
label_11:
      return method;
    }

    internal static Symbols.Method LeastGenericProcedure(
      Symbols.Method left,
      Symbols.Method right)
    {
      Symbols.Method method1;
      if (!left.IsGeneric && !right.IsGeneric && (!Symbols.IsGeneric(left.DeclaringType) && !Symbols.IsGeneric(right.DeclaringType)))
      {
        method1 = (Symbols.Method) null;
      }
      else
      {
        bool signatureMismatch = false;
        Symbols.Method method2 = OverloadResolution.LeastGenericProcedure(left, right, OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams, ref signatureMismatch);
        if ((object) method2 == null && !signatureMismatch)
          method2 = OverloadResolution.LeastGenericProcedure(left, right, OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams, ref signatureMismatch);
        method1 = method2;
      }
      return method1;
    }

    private static void InsertIfMethodAvailable(
      MemberInfo newCandidate,
      ParameterInfo[] newCandidateSignature,
      int newCandidateParamArrayIndex,
      bool expandNewCandidateParamArray,
      object[] arguments,
      int argumentCount,
      string[] argumentNames,
      Type[] typeArguments,
      bool collectOnlyOperators,
      List<Symbols.Method> candidates,
      Symbols.Container baseReference)
    {
      Symbols.Method candidate1 = (Symbols.Method) null;
      if (!collectOnlyOperators)
      {
        MethodBase methodBase1 = newCandidate as MethodBase;
        bool flag = false;
        if (newCandidate.MemberType == MemberTypes.Method && Symbols.IsRawGeneric(methodBase1))
        {
          candidate1 = new Symbols.Method(methodBase1, newCandidateSignature, newCandidateParamArrayIndex, expandNewCandidateParamArray);
          OverloadResolution.RejectUncallableProcedure(candidate1, arguments, argumentNames, typeArguments);
          newCandidate = (MemberInfo) candidate1.AsMethod();
          newCandidateSignature = candidate1.Parameters;
        }
        if ((object) newCandidate != null && newCandidate.MemberType == MemberTypes.Method && Symbols.IsRawGeneric(newCandidate as MethodBase))
          flag = true;
        int num1 = checked (candidates.Count - 1);
        int index1 = 0;
        while (index1 <= num1)
        {
          Symbols.Method candidate2 = candidates[index1];
          if ((object) candidate2 != null)
          {
            ParameterInfo[] parameters = candidate2.Parameters;
            MethodBase methodBase2 = !candidate2.IsMethod ? (MethodBase) null : candidate2.AsMethod();
            if (!(newCandidate == candidate2))
            {
              int index2 = 0;
              int index3 = 0;
              int num2 = argumentCount;
              int num3 = 1;
              while (num3 <= num2)
              {
                bool bothLose = false;
                bool leftWins = false;
                bool rightWins = false;
                OverloadResolution.CompareParameterSpecificity((Type) null, newCandidateSignature[index2], methodBase1, expandNewCandidateParamArray, parameters[index3], methodBase2, candidate2.ParamArrayExpanded, ref leftWins, ref rightWins, ref bothLose);
                if (!(bothLose | leftWins | rightWins))
                {
                  if (index2 != newCandidateParamArrayIndex || !expandNewCandidateParamArray)
                    checked { ++index2; }
                  if (index3 != candidate2.ParamArrayIndex || !candidate2.ParamArrayExpanded)
                    checked { ++index3; }
                  checked { ++num3; }
                }
                else
                  goto label_28;
              }
              if (!OverloadResolution.IsExactSignatureMatch(newCandidateSignature, Symbols.GetTypeParameters(newCandidate).Length, candidate2.Parameters, candidate2.TypeParameters.Length))
              {
                if (!flag && ((object) methodBase2 == null || !Symbols.IsRawGeneric(methodBase2)))
                {
                  if (!expandNewCandidateParamArray && candidate2.ParamArrayExpanded)
                  {
                    candidates[index1] = (Symbols.Method) null;
                  }
                  else
                  {
                    if (expandNewCandidateParamArray && !candidate2.ParamArrayExpanded)
                      return;
                    if (expandNewCandidateParamArray || candidate2.ParamArrayExpanded)
                    {
                      if (index2 > index3)
                        candidates[index1] = (Symbols.Method) null;
                      else if (index3 > index2)
                        return;
                    }
                  }
                }
              }
              else if ((object) newCandidate.DeclaringType != (object) candidate2.DeclaringType && (baseReference == null || !baseReference.IsWindowsRuntimeObject || (!Symbols.IsCollectionInterface(newCandidate.DeclaringType) || !Symbols.IsCollectionInterface(candidate2.DeclaringType))))
              {
                if (flag || (object) methodBase2 == null || !Symbols.IsRawGeneric(methodBase2))
                  return;
              }
              else
                break;
            }
          }
label_28:
          checked { ++index1; }
        }
      }
      if ((object) candidate1 != null)
        candidates.Add(candidate1);
      else if (newCandidate.MemberType == MemberTypes.Property)
        candidates.Add(new Symbols.Method((PropertyInfo) newCandidate, newCandidateSignature, newCandidateParamArrayIndex, expandNewCandidateParamArray));
      else
        candidates.Add(new Symbols.Method((MethodBase) newCandidate, newCandidateSignature, newCandidateParamArrayIndex, expandNewCandidateParamArray));
    }

    private static bool CanConvert(
      Type targetType,
      Type sourceType,
      bool rejectNarrowingConversion,
      List<string> errors,
      string parameterName,
      bool isByRefCopyBackContext,
      ref bool requiresNarrowingConversion,
      ref bool allNarrowingIsFromObject)
    {
      Type targetType1 = targetType;
      Type sourceType1 = sourceType;
      Symbols.Method method = (Symbols.Method) null;
      ref Symbols.Method local = ref method;
      ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyConversion(targetType1, sourceType1, ref local);
      bool flag;
      switch (conversionClass)
      {
        case ConversionResolution.ConversionClass.Identity:
        case ConversionResolution.ConversionClass.Widening:
          flag = true;
          break;
        case ConversionResolution.ConversionClass.Narrowing:
          if (rejectNarrowingConversion)
          {
            if (errors != null)
              OverloadResolution.ReportError(errors, Interaction.IIf<string>(isByRefCopyBackContext, SR.ArgumentNarrowingCopyBack3, SR.ArgumentNarrowing3), parameterName, sourceType, targetType);
            flag = false;
            break;
          }
          requiresNarrowingConversion = true;
          if ((object) sourceType != (object) typeof (object))
            allNarrowingIsFromObject = false;
          flag = true;
          break;
        default:
          if (errors != null)
            OverloadResolution.ReportError(errors, Interaction.IIf<string>(conversionClass == ConversionResolution.ConversionClass.Ambiguous, Interaction.IIf<string>(isByRefCopyBackContext, SR.ArgumentMismatchAmbiguousCopyBack3, SR.ArgumentMismatchAmbiguous3), Interaction.IIf<string>(isByRefCopyBackContext, SR.ArgumentMismatchCopyBack3, SR.ArgumentMismatch3)), parameterName, sourceType, targetType);
          flag = false;
          break;
      }
      return flag;
    }

    private static bool InferTypeArgumentsFromArgument(
      Type argumentType,
      Type parameterType,
      Type[] typeInferenceArguments,
      MethodBase targetProcedure,
      bool digThroughToBasesAndImplements)
    {
      bool flag1 = OverloadResolution.InferTypeArgumentsFromArgumentDirectly(argumentType, parameterType, typeInferenceArguments, targetProcedure, digThroughToBasesAndImplements);
      bool flag2;
      if (flag1 || !digThroughToBasesAndImplements || !Symbols.IsInstantiatedGeneric(parameterType) || !parameterType.IsClass && !parameterType.IsInterface)
      {
        flag2 = flag1;
      }
      else
      {
        Type genericTypeDefinition = parameterType.GetGenericTypeDefinition();
        if (Symbols.IsArrayType(argumentType))
        {
          if (argumentType.GetArrayRank() > 1 || parameterType.IsClass)
          {
            flag2 = false;
            goto label_28;
          }
          else
          {
            argumentType = typeof (IList<>).MakeGenericType(argumentType.GetElementType());
            if ((object) typeof (IList<>) == (object) genericTypeDefinition)
              goto label_27;
          }
        }
        else if (!argumentType.IsClass && !argumentType.IsInterface)
        {
          flag2 = false;
          goto label_28;
        }
        else if (Symbols.IsInstantiatedGeneric(argumentType) && (object) argumentType.GetGenericTypeDefinition() == (object) genericTypeDefinition)
        {
          flag2 = false;
          goto label_28;
        }
        if (parameterType.IsClass)
        {
          if (!argumentType.IsClass)
          {
            flag2 = false;
            goto label_28;
          }
          else
          {
            Type baseType = argumentType.BaseType;
            while ((object) baseType != null && (!Symbols.IsInstantiatedGeneric(baseType) || (object) baseType.GetGenericTypeDefinition() != (object) genericTypeDefinition))
              baseType = baseType.BaseType;
            argumentType = baseType;
          }
        }
        else
        {
          Type type1 = (Type) null;
          Type[] interfaces = argumentType.GetInterfaces();
          int index = 0;
          while (index < interfaces.Length)
          {
            Type type2 = interfaces[index];
            if (Symbols.IsInstantiatedGeneric(type2) && (object) type2.GetGenericTypeDefinition() == (object) genericTypeDefinition)
            {
              if ((object) type1 != null)
              {
                flag2 = false;
                goto label_28;
              }
              else
                type1 = type2;
            }
            checked { ++index; }
          }
          argumentType = type1;
        }
        if ((object) argumentType == null)
        {
          flag2 = false;
          goto label_28;
        }
label_27:
        flag2 = OverloadResolution.InferTypeArgumentsFromArgumentDirectly(argumentType, parameterType, typeInferenceArguments, targetProcedure, digThroughToBasesAndImplements);
      }
label_28:
      return flag2;
    }

    private static bool InferTypeArgumentsFromArgumentDirectly(
      Type argumentType,
      Type parameterType,
      Type[] typeInferenceArguments,
      MethodBase targetProcedure,
      bool digThroughToBasesAndImplements)
    {
      bool flag;
      if (!Symbols.RefersToGenericParameter(parameterType, targetProcedure))
      {
        flag = true;
      }
      else
      {
        if (Symbols.IsGenericParameter(parameterType))
        {
          if (Symbols.AreGenericMethodDefsEqual(parameterType.DeclaringMethod, targetProcedure))
          {
            int parameterPosition = parameterType.GenericParameterPosition;
            if ((object) typeInferenceArguments[parameterPosition] == null)
              typeInferenceArguments[parameterPosition] = argumentType;
            else if ((object) typeInferenceArguments[parameterPosition] != (object) argumentType)
            {
              flag = false;
              goto label_30;
            }
          }
        }
        else if (Symbols.IsInstantiatedGeneric(parameterType))
        {
          Type type1 = (Type) null;
          if (Symbols.IsInstantiatedGeneric(argumentType) && (object) argumentType.GetGenericTypeDefinition() == (object) parameterType.GetGenericTypeDefinition())
            type1 = argumentType;
          if ((object) type1 == null && digThroughToBasesAndImplements)
          {
            Type[] interfaces = argumentType.GetInterfaces();
            int index = 0;
            while (index < interfaces.Length)
            {
              Type type2 = interfaces[index];
              if (Symbols.IsInstantiatedGeneric(type2) && (object) type2.GetGenericTypeDefinition() == (object) parameterType.GetGenericTypeDefinition())
              {
                if ((object) type1 == null)
                {
                  type1 = type2;
                }
                else
                {
                  flag = false;
                  goto label_30;
                }
              }
              checked { ++index; }
            }
          }
          if ((object) type1 != null)
          {
            Type[] typeArguments1 = Symbols.GetTypeArguments(parameterType);
            Type[] typeArguments2 = Symbols.GetTypeArguments(type1);
            int num = checked (typeArguments2.Length - 1);
            int index = 0;
            while (index <= num)
            {
              if (!OverloadResolution.InferTypeArgumentsFromArgument(typeArguments2[index], typeArguments1[index], typeInferenceArguments, targetProcedure, false))
              {
                flag = false;
                goto label_30;
              }
              else
                checked { ++index; }
            }
            flag = true;
            goto label_30;
          }
          else
          {
            flag = false;
            goto label_30;
          }
        }
        else if (Symbols.IsArrayType(parameterType))
        {
          flag = Symbols.IsArrayType(argumentType) && parameterType.GetArrayRank() == argumentType.GetArrayRank() && OverloadResolution.InferTypeArgumentsFromArgument(Symbols.GetElementType(argumentType), Symbols.GetElementType(parameterType), typeInferenceArguments, targetProcedure, digThroughToBasesAndImplements);
          goto label_30;
        }
        flag = true;
      }
label_30:
      return flag;
    }

    private static bool CanPassToParamArray(
      Symbols.Method targetProcedure,
      object argument,
      ParameterInfo parameter)
    {
      bool flag;
      if (argument == null)
      {
        flag = true;
      }
      else
      {
        Type parameterType = parameter.ParameterType;
        Type argumentType = OverloadResolution.GetArgumentType(argument);
        Symbols.Method method = (Symbols.Method) null;
        ref Symbols.Method local = ref method;
        ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyConversion(parameterType, argumentType, ref local);
        flag = conversionClass == ConversionResolution.ConversionClass.Widening || conversionClass == ConversionResolution.ConversionClass.Identity;
      }
      return flag;
    }

    internal static bool CanPassToParameter(
      Symbols.Method targetProcedure,
      object argument,
      ParameterInfo parameter,
      bool isExpandedParamArray,
      bool rejectNarrowingConversions,
      List<string> errors,
      ref bool requiresNarrowingConversion,
      ref bool allNarrowingIsFromObject)
    {
      bool flag1;
      if (argument == null)
      {
        flag1 = true;
      }
      else
      {
        Type type = parameter.ParameterType;
        bool isByRef = type.IsByRef;
        if (isByRef || isExpandedParamArray)
          type = Symbols.GetElementType(type);
        Type argumentType = OverloadResolution.GetArgumentType(argument);
        if (argument == Missing.Value)
        {
          if (parameter.IsOptional)
          {
            flag1 = true;
            goto label_14;
          }
          else if (!Symbols.IsRootObjectType(type) || !isExpandedParamArray)
          {
            if (errors != null)
            {
              if (isExpandedParamArray)
                OverloadResolution.ReportError(errors, SR.OmittedParamArrayArgument);
              else
                OverloadResolution.ReportError(errors, SR.OmittedArgument1, parameter.Name);
            }
            flag1 = false;
            goto label_14;
          }
        }
        bool flag2 = OverloadResolution.CanConvert(type, argumentType, rejectNarrowingConversions, errors, parameter.Name, false, ref requiresNarrowingConversion, ref allNarrowingIsFromObject);
        flag1 = !isByRef || !flag2 ? flag2 : OverloadResolution.CanConvert(argumentType, type, rejectNarrowingConversions, errors, parameter.Name, true, ref requiresNarrowingConversion, ref allNarrowingIsFromObject);
      }
label_14:
      return flag1;
    }

    internal static bool InferTypeArgumentsFromArgument(
      Symbols.Method targetProcedure,
      object argument,
      ParameterInfo parameter,
      bool isExpandedParamArray,
      List<string> errors)
    {
      bool flag;
      if (argument == null)
      {
        flag = true;
      }
      else
      {
        Type type = parameter.ParameterType;
        if (type.IsByRef || isExpandedParamArray)
          type = Symbols.GetElementType(type);
        if (!OverloadResolution.InferTypeArgumentsFromArgument(OverloadResolution.GetArgumentType(argument), type, targetProcedure.TypeArguments, targetProcedure.AsMethod(), true))
        {
          if (errors != null)
            OverloadResolution.ReportError(errors, SR.TypeInferenceFails1, parameter.Name);
          flag = false;
        }
        else
          flag = true;
      }
      return flag;
    }

    internal static object PassToParameter(
      object argument,
      ParameterInfo parameter,
      Type parameterType)
    {
      int num = parameterType.IsByRef ? 1 : 0;
      if (num != 0)
        parameterType = parameterType.GetElementType();
      if (argument is Symbols.TypedNothing)
        argument = (object) null;
      if (argument == Missing.Value && parameter.IsOptional)
        argument = parameter.DefaultValue;
      if (num != 0)
      {
        Type argumentType = OverloadResolution.GetArgumentType(argument);
        if ((object) argumentType != null && Symbols.IsValueType(argumentType))
          argument = Conversions.ForceValueCopy(argument, argumentType);
      }
      return Conversions.ChangeType(argument, parameterType);
    }

    private static bool FindParameterByName(ParameterInfo[] parameters, string name, ref int index)
    {
      int index1 = 0;
      bool flag;
      while (index1 < parameters.Length)
      {
        if (Operators.CompareString(name, parameters[index1].Name, true) == 0)
        {
          index = index1;
          flag = true;
          goto label_6;
        }
        else
          checked { ++index1; }
      }
      flag = false;
label_6:
      return flag;
    }

    private static bool[] CreateMatchTable(int size, int lastPositionalMatchIndex)
    {
      bool[] flagArray = new bool[checked (size - 1 + 1)];
      int num = lastPositionalMatchIndex;
      int index = 0;
      while (index <= num)
      {
        flagArray[index] = true;
        checked { ++index; }
      }
      return flagArray;
    }

    internal static bool CanMatchArguments(
      Symbols.Method targetProcedure,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      bool rejectNarrowingConversions,
      List<string> errors)
    {
      bool flag1 = errors != null;
      targetProcedure.ArgumentsValidated = true;
      bool flag2;
      if (targetProcedure.IsMethod && Symbols.IsRawGeneric(targetProcedure.AsMethod()))
      {
        if (typeArguments.Length == 0)
        {
          typeArguments = new Type[checked (targetProcedure.TypeParameters.Length - 1 + 1)];
          targetProcedure.TypeArguments = typeArguments;
          if (!OverloadResolution.InferTypeArguments(targetProcedure, arguments, argumentNames, typeArguments, errors))
          {
            flag2 = false;
            goto label_58;
          }
        }
        else
          targetProcedure.TypeArguments = typeArguments;
        if (!OverloadResolution.InstantiateGenericMethod(targetProcedure, typeArguments, errors))
        {
          flag2 = false;
          goto label_58;
        }
      }
      ParameterInfo[] parameters = targetProcedure.Parameters;
      int length = argumentNames.Length;
      int index1 = 0;
      while (length < arguments.Length && index1 != targetProcedure.ParamArrayIndex)
      {
        if (!OverloadResolution.CanPassToParameter(targetProcedure, arguments[length], parameters[index1], false, rejectNarrowingConversions, errors, ref targetProcedure.RequiresNarrowingConversion, ref targetProcedure.AllNarrowingIsFromObject) && !flag1)
        {
          flag2 = false;
          goto label_58;
        }
        else
        {
          checked { ++length; }
          checked { ++index1; }
        }
      }
      if (targetProcedure.HasParamArray)
      {
        if (targetProcedure.ParamArrayExpanded)
        {
          if (length == checked (arguments.Length - 1) && arguments[length] == null)
          {
            flag2 = false;
            goto label_58;
          }
          else
          {
            while (length < arguments.Length)
            {
              if (!OverloadResolution.CanPassToParameter(targetProcedure, arguments[length], parameters[index1], true, rejectNarrowingConversions, errors, ref targetProcedure.RequiresNarrowingConversion, ref targetProcedure.AllNarrowingIsFromObject) && !flag1)
              {
                flag2 = false;
                goto label_58;
              }
              else
                checked { ++length; }
            }
          }
        }
        else if (checked (arguments.Length - length) != 1)
        {
          flag2 = false;
          goto label_58;
        }
        else if (!OverloadResolution.CanPassToParamArray(targetProcedure, arguments[length], parameters[index1]))
        {
          if (flag1)
            OverloadResolution.ReportError(errors, SR.ArgumentMismatch3, parameters[index1].Name, OverloadResolution.GetArgumentType(arguments[length]), parameters[index1].ParameterType);
          flag2 = false;
          goto label_58;
        }
        checked { ++index1; }
      }
      bool[] flagArray = (bool[]) null;
      if (argumentNames.Length > 0 || index1 < parameters.Length)
        flagArray = OverloadResolution.CreateMatchTable(parameters.Length, checked (index1 - 1));
      if (argumentNames.Length > 0)
      {
        int[] numArray = new int[checked (argumentNames.Length - 1 + 1)];
        int index2 = 0;
        while (index2 < argumentNames.Length)
        {
          if (!OverloadResolution.FindParameterByName(parameters, argumentNames[index2], ref index1))
          {
            if (!flag1)
            {
              flag2 = false;
              goto label_58;
            }
            else
              OverloadResolution.ReportError(errors, SR.NamedParamNotFound2, argumentNames[index2], targetProcedure);
          }
          else if (index1 == targetProcedure.ParamArrayIndex)
          {
            if (!flag1)
            {
              flag2 = false;
              goto label_58;
            }
            else
              OverloadResolution.ReportError(errors, SR.NamedParamArrayArgument1, argumentNames[index2]);
          }
          else if (flagArray[index1])
          {
            if (!flag1)
            {
              flag2 = false;
              goto label_58;
            }
            else
              OverloadResolution.ReportError(errors, SR.NamedArgUsedTwice2, argumentNames[index2], targetProcedure);
          }
          else if (!OverloadResolution.CanPassToParameter(targetProcedure, arguments[index2], parameters[index1], false, rejectNarrowingConversions, errors, ref targetProcedure.RequiresNarrowingConversion, ref targetProcedure.AllNarrowingIsFromObject) && !flag1)
          {
            flag2 = false;
            goto label_58;
          }
          else
          {
            flagArray[index1] = true;
            numArray[index2] = index1;
          }
          checked { ++index2; }
        }
        targetProcedure.NamedArgumentMapping = numArray;
      }
      if (flagArray != null)
      {
        int num = checked (flagArray.Length - 1);
        int index2 = 0;
        while (index2 <= num)
        {
          if (!flagArray[index2] && !parameters[index2].IsOptional)
          {
            if (!flag1)
            {
              flag2 = false;
              goto label_58;
            }
            else
              OverloadResolution.ReportError(errors, SR.OmittedArgument1, parameters[index2].Name);
          }
          checked { ++index2; }
        }
      }
      flag2 = errors == null || errors.Count <= 0;
label_58:
      return flag2;
    }

    private static bool InstantiateGenericMethod(
      Symbols.Method targetProcedure,
      Type[] typeArguments,
      List<string> errors)
    {
      bool flag1 = errors != null;
      int num = checked (typeArguments.Length - 1);
      int index = 0;
      bool flag2;
      while (index <= num)
      {
        if ((object) typeArguments[index] == null)
        {
          if (!flag1)
          {
            flag2 = false;
            goto label_12;
          }
          else
            OverloadResolution.ReportError(errors, SR.UnboundTypeParam1, targetProcedure.TypeParameters[index].Name);
        }
        checked { ++index; }
      }
      if ((errors == null || errors.Count == 0) && !targetProcedure.BindGenericArguments())
      {
        if (!flag1)
        {
          flag2 = false;
          goto label_12;
        }
        else
          OverloadResolution.ReportError(errors, SR.FailedTypeArgumentBinding);
      }
      flag2 = errors == null || errors.Count <= 0;
label_12:
      return flag2;
    }

    internal static void MatchArguments(
      Symbols.Method targetProcedure,
      object[] arguments,
      object[] matchedArguments)
    {
      ParameterInfo[] parameters = targetProcedure.Parameters;
      int[] namedArgumentMapping = targetProcedure.NamedArgumentMapping;
      int index1 = 0;
      if (namedArgumentMapping != null)
        index1 = namedArgumentMapping.Length;
      int index2 = 0;
      while (index1 < arguments.Length && index2 != targetProcedure.ParamArrayIndex)
      {
        matchedArguments[index2] = OverloadResolution.PassToParameter(arguments[index1], parameters[index2], parameters[index2].ParameterType);
        checked { ++index1; }
        checked { ++index2; }
      }
      if (targetProcedure.HasParamArray)
      {
        if (targetProcedure.ParamArrayExpanded)
        {
          int length = checked (arguments.Length - index1);
          ParameterInfo parameter = parameters[index2];
          Type elementType = parameter.ParameterType.GetElementType();
          Array instance = Array.CreateInstance(elementType, length);
          int index3 = 0;
          while (index1 < arguments.Length)
          {
            instance.SetValue(OverloadResolution.PassToParameter(arguments[index1], parameter, elementType), index3);
            checked { ++index1; }
            checked { ++index3; }
          }
          matchedArguments[index2] = (object) instance;
        }
        else
          matchedArguments[index2] = OverloadResolution.PassToParameter(arguments[index1], parameters[index2], parameters[index2].ParameterType);
        checked { ++index2; }
      }
      bool[] flagArray = (bool[]) null;
      if (namedArgumentMapping != null || index2 < parameters.Length)
        flagArray = OverloadResolution.CreateMatchTable(parameters.Length, checked (index2 - 1));
      if (namedArgumentMapping != null)
      {
        int index3 = 0;
        while (index3 < namedArgumentMapping.Length)
        {
          int index4 = namedArgumentMapping[index3];
          matchedArguments[index4] = OverloadResolution.PassToParameter(arguments[index3], parameters[index4], parameters[index4].ParameterType);
          flagArray[index4] = true;
          checked { ++index3; }
        }
      }
      if (flagArray == null)
        return;
      int num = checked (flagArray.Length - 1);
      int index5 = 0;
      while (index5 <= num)
      {
        if (!flagArray[index5])
          matchedArguments[index5] = OverloadResolution.PassToParameter((object) Missing.Value, parameters[index5], parameters[index5].ParameterType);
        checked { ++index5; }
      }
    }

    private static bool InferTypeArguments(
      Symbols.Method targetProcedure,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      List<string> errors)
    {
      bool flag1 = errors != null;
      ParameterInfo[] rawParameters = targetProcedure.RawParameters;
      int length = argumentNames.Length;
      int index1 = 0;
      bool flag2;
      while (length < arguments.Length && index1 != targetProcedure.ParamArrayIndex)
      {
        if (!OverloadResolution.InferTypeArgumentsFromArgument(targetProcedure, arguments[length], rawParameters[index1], false, errors) && !flag1)
        {
          flag2 = false;
          goto label_23;
        }
        else
        {
          checked { ++length; }
          checked { ++index1; }
        }
      }
      if (targetProcedure.HasParamArray)
      {
        if (targetProcedure.ParamArrayExpanded)
        {
          while (length < arguments.Length)
          {
            if (!OverloadResolution.InferTypeArgumentsFromArgument(targetProcedure, arguments[length], rawParameters[index1], true, errors) && !flag1)
            {
              flag2 = false;
              goto label_23;
            }
            else
              checked { ++length; }
          }
        }
        else if (checked (arguments.Length - length) != 1)
        {
          flag2 = true;
          goto label_23;
        }
        else if (!OverloadResolution.InferTypeArgumentsFromArgument(targetProcedure, arguments[length], rawParameters[index1], false, errors))
        {
          flag2 = false;
          goto label_23;
        }
        checked { ++index1; }
      }
      if (argumentNames.Length > 0)
      {
        int index2 = 0;
        while (index2 < argumentNames.Length)
        {
          if (OverloadResolution.FindParameterByName(rawParameters, argumentNames[index2], ref index1) && index1 != targetProcedure.ParamArrayIndex && (!OverloadResolution.InferTypeArgumentsFromArgument(targetProcedure, arguments[index2], rawParameters[index1], false, errors) && !flag1))
          {
            flag2 = false;
            goto label_23;
          }
          else
            checked { ++index2; }
        }
      }
      flag2 = errors == null || errors.Count <= 0;
label_23:
      return flag2;
    }

    internal static void ReorderArgumentArray(
      Symbols.Method targetProcedure,
      object[] parameterResults,
      object[] arguments,
      bool[] copyBack,
      BindingFlags lookupFlags)
    {
      if (copyBack == null)
        return;
      int num = checked (copyBack.Length - 1);
      int index1 = 0;
      while (index1 <= num)
      {
        copyBack[index1] = false;
        checked { ++index1; }
      }
      if (Symbols.HasFlag(lookupFlags, ReflectionExtensions.BindingFlagsSetProperty) || !targetProcedure.HasByRefParameter)
        return;
      ParameterInfo[] parameters = targetProcedure.Parameters;
      int[] namedArgumentMapping = targetProcedure.NamedArgumentMapping;
      int index2 = 0;
      if (namedArgumentMapping != null)
        index2 = namedArgumentMapping.Length;
      int index3 = 0;
      while (index2 < arguments.Length && index3 != targetProcedure.ParamArrayIndex)
      {
        if (parameters[index3].ParameterType.IsByRef)
        {
          arguments[index2] = parameterResults[index3];
          copyBack[index2] = true;
        }
        checked { ++index2; }
        checked { ++index3; }
      }
      if (namedArgumentMapping == null)
        return;
      int index4 = 0;
      while (index4 < namedArgumentMapping.Length)
      {
        int index5 = namedArgumentMapping[index4];
        if (parameters[index5].ParameterType.IsByRef)
        {
          arguments[index4] = parameterResults[index5];
          copyBack[index4] = true;
        }
        checked { ++index4; }
      }
    }

    private static Symbols.Method RejectUncallableProcedures(
      List<Symbols.Method> candidates,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      ref int candidateCount,
      ref bool someCandidatesAreGeneric)
    {
      Symbols.Method method = (Symbols.Method) null;
      int num = checked (candidates.Count - 1);
      int index = 0;
      while (index <= num)
      {
        Symbols.Method candidate = candidates[index];
        if (!candidate.ArgumentMatchingDone)
          OverloadResolution.RejectUncallableProcedure(candidate, arguments, argumentNames, typeArguments);
        if (candidate.NotCallable)
        {
          checked { --candidateCount; }
        }
        else
        {
          method = candidate;
          if (candidate.IsGeneric || Symbols.IsGeneric(candidate.DeclaringType))
            someCandidatesAreGeneric = true;
        }
        checked { ++index; }
      }
      return method;
    }

    private static void RejectUncallableProcedure(
      Symbols.Method candidate,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments)
    {
      if (!OverloadResolution.CanMatchArguments(candidate, arguments, argumentNames, typeArguments, false, (List<string>) null))
        candidate.NotCallable = true;
      candidate.ArgumentMatchingDone = true;
    }

    private static Type GetArgumentType(object argument)
    {
      return argument != null ? (!(argument is Symbols.TypedNothing typedNothing) ? argument.GetType() : typedNothing.Type) : (Type) null;
    }

    private static Symbols.Method MoreSpecificProcedure(
      Symbols.Method left,
      Symbols.Method right,
      object[] arguments,
      string[] argumentNames,
      OverloadResolution.ComparisonType compareGenericity,
      ref bool bothLose,
      bool continueWhenBothLose = false)
    {
      bothLose = false;
      bool flag1 = false;
      bool flag2 = false;
      MethodBase leftProcedure = !left.IsMethod ? (MethodBase) null : left.AsMethod();
      MethodBase rightProcedure = !right.IsMethod ? (MethodBase) null : right.AsMethod();
      int index1 = 0;
      int index2 = 0;
      int length = argumentNames.Length;
      Symbols.Method method;
      while (length < arguments.Length)
      {
        Type argumentType = OverloadResolution.GetArgumentType(arguments[length]);
        switch (compareGenericity)
        {
          case OverloadResolution.ComparisonType.ParameterSpecificty:
            OverloadResolution.CompareParameterSpecificity(argumentType, left.Parameters[index1], leftProcedure, left.ParamArrayExpanded, right.Parameters[index2], rightProcedure, right.ParamArrayExpanded, ref flag1, ref flag2, ref bothLose);
            break;
          case OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams:
            OverloadResolution.CompareGenericityBasedOnMethodGenericParams(left.Parameters[index1], left.RawParameters[index1], left, left.ParamArrayExpanded, right.Parameters[index2], right.RawParameters[index2], right, right.ParamArrayExpanded, ref flag1, ref flag2, ref bothLose);
            break;
          case OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams:
            OverloadResolution.CompareGenericityBasedOnTypeGenericParams(left.Parameters[index1], left.RawParametersFromType[index1], left, left.ParamArrayExpanded, right.Parameters[index2], right.RawParametersFromType[index2], right, right.ParamArrayExpanded, ref flag1, ref flag2, ref bothLose);
            break;
        }
        if (bothLose && !continueWhenBothLose || flag1 && flag2)
        {
          method = (Symbols.Method) null;
          goto label_25;
        }
        else
        {
          if (index1 != left.ParamArrayIndex)
            checked { ++index1; }
          if (index2 != right.ParamArrayIndex)
            checked { ++index2; }
          checked { ++length; }
        }
      }
      int index3 = 0;
      while (index3 < argumentNames.Length)
      {
        int num = OverloadResolution.FindParameterByName(left.Parameters, argumentNames[index3], ref index1) ? 1 : 0;
        bool parameterByName = OverloadResolution.FindParameterByName(right.Parameters, argumentNames[index3], ref index2);
        if (num == 0 || !parameterByName)
          throw new InternalErrorException();
        Type argumentType = OverloadResolution.GetArgumentType(arguments[index3]);
        switch (compareGenericity)
        {
          case OverloadResolution.ComparisonType.ParameterSpecificty:
            OverloadResolution.CompareParameterSpecificity(argumentType, left.Parameters[index1], leftProcedure, true, right.Parameters[index2], rightProcedure, true, ref flag1, ref flag2, ref bothLose);
            break;
          case OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams:
            OverloadResolution.CompareGenericityBasedOnMethodGenericParams(left.Parameters[index1], left.RawParameters[index1], left, true, right.Parameters[index2], right.RawParameters[index2], right, true, ref flag1, ref flag2, ref bothLose);
            break;
          case OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams:
            OverloadResolution.CompareGenericityBasedOnTypeGenericParams(left.Parameters[index1], left.RawParameters[index1], left, true, right.Parameters[index2], right.RawParameters[index2], right, true, ref flag1, ref flag2, ref bothLose);
            break;
        }
        if (bothLose && !continueWhenBothLose || flag1 && flag2)
        {
          method = (Symbols.Method) null;
          goto label_25;
        }
        else
          checked { ++index3; }
      }
      method = !flag1 ? (!flag2 ? (Symbols.Method) null : right) : left;
label_25:
      return method;
    }

    private static Symbols.Method MostSpecificProcedure(
      List<Symbols.Method> candidates,
      ref int candidateCount,
      object[] arguments,
      string[] argumentNames)
    {
      Symbols.Method method1;
      try
      {
        foreach (Symbols.Method candidate1 in candidates)
        {
          if (!candidate1.NotCallable && !candidate1.RequiresNarrowingConversion)
          {
            bool flag1 = true;
            try
            {
              foreach (Symbols.Method candidate2 in candidates)
              {
                if (!candidate2.NotCallable && !candidate2.RequiresNarrowingConversion && (!(candidate2 == candidate1) || candidate2.ParamArrayExpanded != candidate1.ParamArrayExpanded))
                {
                  Symbols.Method left = candidate1;
                  Symbols.Method right = candidate2;
                  object[] arguments1 = arguments;
                  string[] argumentNames1 = argumentNames;
                  bool flag2 = false;
                  ref bool local = ref flag2;
                  Symbols.Method method2 = OverloadResolution.MoreSpecificProcedure(left, right, arguments1, argumentNames1, OverloadResolution.ComparisonType.ParameterSpecificty, ref local, true);
                  if ((object) method2 == (object) candidate1)
                  {
                    if (!candidate2.LessSpecific)
                    {
                      candidate2.LessSpecific = true;
                      checked { --candidateCount; }
                    }
                  }
                  else
                  {
                    flag1 = false;
                    if ((object) method2 == (object) candidate2 && !candidate1.LessSpecific)
                    {
                      candidate1.LessSpecific = true;
                      checked { --candidateCount; }
                    }
                  }
                }
              }
            }
            finally
            {
              List<Symbols.Method>.Enumerator enumerator;
              enumerator.Dispose();
            }
            if (flag1)
            {
              method1 = candidate1;
              goto label_17;
            }
          }
        }
      }
      finally
      {
        List<Symbols.Method>.Enumerator enumerator;
        enumerator.Dispose();
      }
      method1 = (Symbols.Method) null;
label_17:
      return method1;
    }

    private static Symbols.Method RemoveRedundantGenericProcedures(
      List<Symbols.Method> candidates,
      ref int candidateCount,
      object[] arguments,
      string[] argumentNames)
    {
      int num1 = checked (candidates.Count - 1);
      int index1 = 0;
      Symbols.Method method1;
      while (index1 <= num1)
      {
        Symbols.Method candidate1 = candidates[index1];
        if (!candidate1.NotCallable)
        {
          int num2 = checked (index1 + 1);
          int num3 = checked (candidates.Count - 1);
          int index2 = num2;
          while (index2 <= num3)
          {
            Symbols.Method candidate2 = candidates[index2];
            if (!candidate2.NotCallable && candidate1.RequiresNarrowingConversion == candidate2.RequiresNarrowingConversion)
            {
              Symbols.Method method2 = (Symbols.Method) null;
              bool bothLose = false;
              if (candidate1.IsGeneric || candidate2.IsGeneric)
              {
                method2 = OverloadResolution.MoreSpecificProcedure(candidate1, candidate2, arguments, argumentNames, OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams, ref bothLose, false);
                if ((object) method2 != null)
                {
                  checked { --candidateCount; }
                  if (candidateCount == 1)
                  {
                    method1 = method2;
                    goto label_23;
                  }
                  else if ((object) method2 == (object) candidate1)
                  {
                    candidate2.NotCallable = true;
                  }
                  else
                  {
                    candidate1.NotCallable = true;
                    break;
                  }
                }
              }
              if (!bothLose && (object) method2 == null && (Symbols.IsGeneric(candidate1.DeclaringType) || Symbols.IsGeneric(candidate2.DeclaringType)))
              {
                Symbols.Method method3 = OverloadResolution.MoreSpecificProcedure(candidate1, candidate2, arguments, argumentNames, OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams, ref bothLose, false);
                if ((object) method3 != null)
                {
                  checked { --candidateCount; }
                  if (candidateCount == 1)
                  {
                    method1 = method3;
                    goto label_23;
                  }
                  else if ((object) method3 == (object) candidate1)
                  {
                    candidate2.NotCallable = true;
                  }
                  else
                  {
                    candidate1.NotCallable = true;
                    break;
                  }
                }
              }
            }
            checked { ++index2; }
          }
        }
        checked { ++index1; }
      }
      method1 = (Symbols.Method) null;
label_23:
      return method1;
    }

    private static void ReportError(
      List<string> errors,
      string resourceID,
      string substitution1,
      Type substitution2,
      Type substitution3)
    {
      errors.Add(Utils.GetResourceString(resourceID, substitution1, Utils.VBFriendlyName(substitution2), Utils.VBFriendlyName(substitution3)));
    }

    private static void ReportError(
      List<string> errors,
      string resourceID,
      string substitution1,
      Symbols.Method substitution2)
    {
      errors.Add(Utils.GetResourceString(resourceID, substitution1, substitution2.ToString()));
    }

    private static void ReportError(List<string> errors, string resourceID, string substitution1)
    {
      errors.Add(Utils.GetResourceString(resourceID, substitution1));
    }

    private static void ReportError(List<string> errors, string resourceID)
    {
      errors.Add(Utils.GetResourceString(resourceID));
    }

    private static Exception ReportOverloadResolutionFailure(
      string overloadedProcedureName,
      List<Symbols.Method> candidates,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      string errorID,
      OverloadResolution.ResolutionFailure failure,
      OverloadResolution.ArgumentDetector detector,
      OverloadResolution.CandidateProperty candidateFilter)
    {
      StringBuilder stringBuilder = new StringBuilder();
      List<string> errors = new List<string>();
      int num1 = 0;
      int num2 = checked (candidates.Count - 1);
      int index1 = 0;
      while (index1 <= num2)
      {
        Symbols.Method candidate = candidates[index1];
        if (candidateFilter(candidate))
        {
          if (candidate.HasParamArray)
          {
            int index2 = checked (index1 + 1);
            while (index2 < candidates.Count)
            {
              if (!candidateFilter(candidates[index2]) || !(candidates[index2] == candidate))
                checked { ++index2; }
              else
                goto label_12;
            }
          }
          checked { ++num1; }
          errors.Clear();
          int num3 = detector(candidate, arguments, argumentNames, typeArguments, errors) ? 1 : 0;
          stringBuilder.Append("\r\n    '");
          stringBuilder.Append(candidate.ToString());
          stringBuilder.Append("':");
          try
          {
            foreach (string str in errors)
            {
              stringBuilder.Append("\r\n        ");
              stringBuilder.Append(str);
            }
          }
          finally
          {
            List<string>.Enumerator enumerator;
            enumerator.Dispose();
          }
        }
label_12:
        checked { ++index1; }
      }
      string resourceString = Utils.GetResourceString(errorID, overloadedProcedureName, stringBuilder.ToString());
      return num1 != 1 ? (Exception) new AmbiguousMatchException(resourceString) : (Exception) new InvalidCastException(resourceString);
    }

    private static bool DetectArgumentErrors(
      Symbols.Method targetProcedure,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      List<string> errors)
    {
      return OverloadResolution.CanMatchArguments(targetProcedure, arguments, argumentNames, typeArguments, false, errors);
    }

    private static bool CandidateIsNotCallable(Symbols.Method candidate)
    {
      return candidate.NotCallable;
    }

    private static Exception ReportUncallableProcedures(
      string overloadedProcedureName,
      List<Symbols.Method> candidates,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      OverloadResolution.ResolutionFailure failure)
    {
      return OverloadResolution.ReportOverloadResolutionFailure(overloadedProcedureName, candidates, arguments, argumentNames, typeArguments, SR.NoCallableOverloadCandidates2, failure, new OverloadResolution.ArgumentDetector(OverloadResolution.DetectArgumentErrors), new OverloadResolution.CandidateProperty(OverloadResolution.CandidateIsNotCallable));
    }

    private static bool DetectArgumentNarrowing(
      Symbols.Method targetProcedure,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      List<string> errors)
    {
      return OverloadResolution.CanMatchArguments(targetProcedure, arguments, argumentNames, typeArguments, true, errors);
    }

    private static bool CandidateIsNarrowing(Symbols.Method candidate)
    {
      return !candidate.NotCallable && candidate.RequiresNarrowingConversion;
    }

    private static Exception ReportNarrowingProcedures(
      string overloadedProcedureName,
      List<Symbols.Method> candidates,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      OverloadResolution.ResolutionFailure failure)
    {
      return OverloadResolution.ReportOverloadResolutionFailure(overloadedProcedureName, candidates, arguments, argumentNames, typeArguments, SR.NoNonNarrowingOverloadCandidates2, failure, new OverloadResolution.ArgumentDetector(OverloadResolution.DetectArgumentNarrowing), new OverloadResolution.CandidateProperty(OverloadResolution.CandidateIsNarrowing));
    }

    private static bool DetectUnspecificity(
      Symbols.Method targetProcedure,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      List<string> errors)
    {
      OverloadResolution.ReportError(errors, SR.NotMostSpecificOverload);
      return false;
    }

    private static bool CandidateIsUnspecific(Symbols.Method candidate)
    {
      return !candidate.NotCallable && !candidate.RequiresNarrowingConversion && !candidate.LessSpecific;
    }

    private static Exception ReportUnspecificProcedures(
      string overloadedProcedureName,
      List<Symbols.Method> candidates,
      OverloadResolution.ResolutionFailure failure)
    {
      return OverloadResolution.ReportOverloadResolutionFailure(overloadedProcedureName, candidates, (object[]) null, (string[]) null, (Type[]) null, SR.NoMostSpecificOverload2, failure, new OverloadResolution.ArgumentDetector(OverloadResolution.DetectUnspecificity), new OverloadResolution.CandidateProperty(OverloadResolution.CandidateIsUnspecific));
    }

    internal static Symbols.Method ResolveOverloadedCall(
      string methodName,
      List<Symbols.Method> candidates,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      BindingFlags lookupFlags,
      bool reportErrors,
      ref OverloadResolution.ResolutionFailure failure)
    {
      failure = OverloadResolution.ResolutionFailure.None;
      int count = candidates.Count;
      bool someCandidatesAreGeneric = false;
      Symbols.Method method1 = OverloadResolution.RejectUncallableProcedures(candidates, arguments, argumentNames, typeArguments, ref count, ref someCandidatesAreGeneric);
      Symbols.Method method2;
      switch (count)
      {
        case 0:
          failure = OverloadResolution.ResolutionFailure.InvalidArgument;
          if (reportErrors)
            throw OverloadResolution.ReportUncallableProcedures(methodName, candidates, arguments, argumentNames, typeArguments, failure);
          method2 = (Symbols.Method) null;
          break;
        case 1:
          method2 = method1;
          break;
        default:
          if (someCandidatesAreGeneric)
          {
            method1 = OverloadResolution.RemoveRedundantGenericProcedures(candidates, ref count, arguments, argumentNames);
            if (count == 1)
            {
              method2 = method1;
              break;
            }
          }
          int num = 0;
          Symbols.Method method3 = (Symbols.Method) null;
          try
          {
            foreach (Symbols.Method candidate in candidates)
            {
              if (!candidate.NotCallable)
              {
                if (candidate.RequiresNarrowingConversion)
                {
                  checked { --count; }
                  if (candidate.AllNarrowingIsFromObject)
                  {
                    checked { ++num; }
                    method3 = candidate;
                  }
                }
                else
                  method1 = candidate;
              }
            }
          }
          finally
          {
            List<Symbols.Method>.Enumerator enumerator;
            enumerator.Dispose();
          }
          switch (count)
          {
            case 0:
              if (num == 1)
              {
                method2 = method3;
                break;
              }
              failure = OverloadResolution.ResolutionFailure.AmbiguousMatch;
              if (reportErrors)
                throw OverloadResolution.ReportNarrowingProcedures(methodName, candidates, arguments, argumentNames, typeArguments, failure);
              method2 = (Symbols.Method) null;
              break;
            case 1:
              method2 = method1;
              break;
            default:
              Symbols.Method method4 = OverloadResolution.MostSpecificProcedure(candidates, ref count, arguments, argumentNames);
              if ((object) method4 != null)
              {
                method2 = method4;
                break;
              }
              failure = OverloadResolution.ResolutionFailure.AmbiguousMatch;
              if (reportErrors)
                throw OverloadResolution.ReportUnspecificProcedures(methodName, candidates, failure);
              method2 = (Symbols.Method) null;
              break;
          }
          break;
      }
      return method2;
    }

    internal enum ResolutionFailure
    {
      None,
      MissingMember,
      InvalidArgument,
      AmbiguousMatch,
      InvalidTarget,
    }

    private enum ComparisonType
    {
      ParameterSpecificty,
      GenericSpecificityBasedOnMethodGenericParams,
      GenericSpecificityBasedOnTypeGenericParams,
    }

    private delegate bool ArgumentDetector(
      Symbols.Method targetProcedure,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      List<string> errors);

    private delegate bool CandidateProperty(Symbols.Method candidate);
  }
}
