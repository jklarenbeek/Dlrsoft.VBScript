// Decompiled with JetBrains decompiler
// Type: System.SR
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Resources;
using System.Runtime.CompilerServices;

namespace System
{
  internal class SR
  {
    private static ResourceManager s_resourceManager;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool UsingResourceKeys()
    {
      return false;
    }

    internal static string GetResourceString(string resourceKey, string defaultString = null)
    {
      string str1;
      if (SR.UsingResourceKeys())
      {
        str1 = defaultString ?? resourceKey;
      }
      else
      {
        string str2 = (string) null;
        try
        {
          str2 = SR.ResourceManager.GetString(resourceKey);
        }
        catch (MissingManifestResourceException ex)
        {
        }
        str1 = defaultString == null || !resourceKey.Equals(str2, StringComparison.Ordinal) ? str2 : defaultString;
      }
      return str1;
    }

    internal static string Format(string resourceFormat, params object[] args)
    {
      return args == null ? resourceFormat : (!SR.UsingResourceKeys() ? string.Format(resourceFormat, args) : resourceFormat + string.Join(", ", args));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static string Format(string resourceFormat, object p1)
    {
      string str;
      if (SR.UsingResourceKeys())
        str = string.Join(", ", (object) resourceFormat, p1);
      else
        str = string.Format(resourceFormat, p1);
      return str;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static string Format(string resourceFormat, object p1, object p2)
    {
      string str;
      if (SR.UsingResourceKeys())
        str = string.Join(", ", (object) resourceFormat, p1, p2);
      else
        str = string.Format(resourceFormat, p1, p2);
      return str;
    }

    internal static ResourceManager ResourceManager
    {
      get
      {
        if (SR.s_resourceManager == null)
          SR.s_resourceManager = new ResourceManager(typeof (FxResources.Microsoft.VisualBasic.SR));
        return SR.s_resourceManager;
      }
    }

    internal static string ID91
    {
      get
      {
        return SR.GetResourceString(nameof (ID91), (string) null);
      }
    }

    internal static string ID92
    {
      get
      {
        return SR.GetResourceString(nameof (ID92), (string) null);
      }
    }

    internal static string Argument_GEZero1
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_GEZero1), (string) null);
      }
    }

    internal static string Argument_GTZero1
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_GTZero1), (string) null);
      }
    }

    internal static string Argument_LengthGTZero1
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_LengthGTZero1), (string) null);
      }
    }

    internal static string Argument_RangeTwoBytes1
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_RangeTwoBytes1), (string) null);
      }
    }

    internal static string Argument_MinusOneOrGTZero1
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_MinusOneOrGTZero1), (string) null);
      }
    }

    internal static string Argument_RankEQOne1
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_RankEQOne1), (string) null);
      }
    }

    internal static string Argument_IComparable2
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_IComparable2), (string) null);
      }
    }

    internal static string Argument_InvalidValue1
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_InvalidValue1), (string) null);
      }
    }

    internal static string Argument_InvalidValueType2
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_InvalidValueType2), (string) null);
      }
    }

    internal static string Argument_InvalidValue
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_InvalidValue), (string) null);
      }
    }

    internal static string Collection_BeforeAfterExclusive
    {
      get
      {
        return SR.GetResourceString(nameof (Collection_BeforeAfterExclusive), (string) null);
      }
    }

    internal static string Collection_DuplicateKey
    {
      get
      {
        return SR.GetResourceString(nameof (Collection_DuplicateKey), (string) null);
      }
    }

    internal static string ForLoop_CommonType2
    {
      get
      {
        return SR.GetResourceString(nameof (ForLoop_CommonType2), (string) null);
      }
    }

    internal static string ForLoop_CommonType3
    {
      get
      {
        return SR.GetResourceString(nameof (ForLoop_CommonType3), (string) null);
      }
    }

    internal static string ForLoop_ConvertToType3
    {
      get
      {
        return SR.GetResourceString(nameof (ForLoop_ConvertToType3), (string) null);
      }
    }

    internal static string ForLoop_OperatorRequired2
    {
      get
      {
        return SR.GetResourceString(nameof (ForLoop_OperatorRequired2), (string) null);
      }
    }

    internal static string ForLoop_UnacceptableOperator2
    {
      get
      {
        return SR.GetResourceString(nameof (ForLoop_UnacceptableOperator2), (string) null);
      }
    }

    internal static string ForLoop_UnacceptableRelOperator2
    {
      get
      {
        return SR.GetResourceString(nameof (ForLoop_UnacceptableRelOperator2), (string) null);
      }
    }

    internal static string InternalError_VisualBasicRuntime
    {
      get
      {
        return SR.GetResourceString(nameof (InternalError_VisualBasicRuntime), (string) null);
      }
    }

    internal static string Argument_InvalidNullValue1
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_InvalidNullValue1), (string) null);
      }
    }

    internal static string Argument_InvalidRank1
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_InvalidRank1), (string) null);
      }
    }

    internal static string InvalidCast_FromTo
    {
      get
      {
        return SR.GetResourceString(nameof (InvalidCast_FromTo), (string) null);
      }
    }

    internal static string InvalidCast_FromStringTo
    {
      get
      {
        return SR.GetResourceString(nameof (InvalidCast_FromStringTo), (string) null);
      }
    }

    internal static string Argument_UnsupportedFieldType2
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_UnsupportedFieldType2), (string) null);
      }
    }

    internal static string MissingMember_NoDefaultMemberFound1
    {
      get
      {
        return SR.GetResourceString(nameof (MissingMember_NoDefaultMemberFound1), (string) null);
      }
    }

    internal static string MissingMember_MemberNotFoundOnType2
    {
      get
      {
        return SR.GetResourceString(nameof (MissingMember_MemberNotFoundOnType2), (string) null);
      }
    }

    internal static string IntermediateLateBoundNothingResult1
    {
      get
      {
        return SR.GetResourceString(nameof (IntermediateLateBoundNothingResult1), (string) null);
      }
    }

    internal static string Argument_CollectionIndex
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_CollectionIndex), (string) null);
      }
    }

    internal static string RValueBaseForValueType
    {
      get
      {
        return SR.GetResourceString(nameof (RValueBaseForValueType), (string) null);
      }
    }

    internal static string ExpressionNotProcedure
    {
      get
      {
        return SR.GetResourceString(nameof (ExpressionNotProcedure), (string) null);
      }
    }

    internal static string MissingMember_ReadOnlyField2
    {
      get
      {
        return SR.GetResourceString(nameof (MissingMember_ReadOnlyField2), (string) null);
      }
    }

    internal static string Invalid_VBFixedArray
    {
      get
      {
        return SR.GetResourceString(nameof (Invalid_VBFixedArray), (string) null);
      }
    }

    internal static string Invalid_VBFixedString
    {
      get
      {
        return SR.GetResourceString(nameof (Invalid_VBFixedString), (string) null);
      }
    }

    internal static string Argument_InvalidNamedArgs
    {
      get
      {
        return SR.GetResourceString(nameof (Argument_InvalidNamedArgs), (string) null);
      }
    }

    internal static string SyncLockRequiresReferenceType1
    {
      get
      {
        return SR.GetResourceString(nameof (SyncLockRequiresReferenceType1), (string) null);
      }
    }

    internal static string NullReference_InstanceReqToAccessMember1
    {
      get
      {
        return SR.GetResourceString(nameof (NullReference_InstanceReqToAccessMember1), (string) null);
      }
    }

    internal static string MatchArgumentFailure2
    {
      get
      {
        return SR.GetResourceString(nameof (MatchArgumentFailure2), (string) null);
      }
    }

    internal static string NoGetProperty1
    {
      get
      {
        return SR.GetResourceString(nameof (NoGetProperty1), (string) null);
      }
    }

    internal static string NoSetProperty1
    {
      get
      {
        return SR.GetResourceString(nameof (NoSetProperty1), (string) null);
      }
    }

    internal static string MethodAssignment1
    {
      get
      {
        return SR.GetResourceString(nameof (MethodAssignment1), (string) null);
      }
    }

    internal static string NoViableOverloadCandidates1
    {
      get
      {
        return SR.GetResourceString(nameof (NoViableOverloadCandidates1), (string) null);
      }
    }

    internal static string NoArgumentCountOverloadCandidates1
    {
      get
      {
        return SR.GetResourceString(nameof (NoArgumentCountOverloadCandidates1), (string) null);
      }
    }

    internal static string NoTypeArgumentCountOverloadCandidates1
    {
      get
      {
        return SR.GetResourceString(nameof (NoTypeArgumentCountOverloadCandidates1), (string) null);
      }
    }

    internal static string NoCallableOverloadCandidates2
    {
      get
      {
        return SR.GetResourceString(nameof (NoCallableOverloadCandidates2), (string) null);
      }
    }

    internal static string NoNonNarrowingOverloadCandidates2
    {
      get
      {
        return SR.GetResourceString(nameof (NoNonNarrowingOverloadCandidates2), (string) null);
      }
    }

    internal static string NoMostSpecificOverload2
    {
      get
      {
        return SR.GetResourceString(nameof (NoMostSpecificOverload2), (string) null);
      }
    }

    internal static string AmbiguousCast2
    {
      get
      {
        return SR.GetResourceString(nameof (AmbiguousCast2), (string) null);
      }
    }

    internal static string NotMostSpecificOverload
    {
      get
      {
        return SR.GetResourceString(nameof (NotMostSpecificOverload), (string) null);
      }
    }

    internal static string NamedParamNotFound2
    {
      get
      {
        return SR.GetResourceString(nameof (NamedParamNotFound2), (string) null);
      }
    }

    internal static string NamedParamArrayArgument1
    {
      get
      {
        return SR.GetResourceString(nameof (NamedParamArrayArgument1), (string) null);
      }
    }

    internal static string NamedArgUsedTwice2
    {
      get
      {
        return SR.GetResourceString(nameof (NamedArgUsedTwice2), (string) null);
      }
    }

    internal static string OmittedArgument1
    {
      get
      {
        return SR.GetResourceString(nameof (OmittedArgument1), (string) null);
      }
    }

    internal static string OmittedParamArrayArgument
    {
      get
      {
        return SR.GetResourceString(nameof (OmittedParamArrayArgument), (string) null);
      }
    }

    internal static string ArgumentMismatch3
    {
      get
      {
        return SR.GetResourceString(nameof (ArgumentMismatch3), (string) null);
      }
    }

    internal static string ArgumentMismatchAmbiguous3
    {
      get
      {
        return SR.GetResourceString(nameof (ArgumentMismatchAmbiguous3), (string) null);
      }
    }

    internal static string ArgumentNarrowing3
    {
      get
      {
        return SR.GetResourceString(nameof (ArgumentNarrowing3), (string) null);
      }
    }

    internal static string ArgumentMismatchCopyBack3
    {
      get
      {
        return SR.GetResourceString(nameof (ArgumentMismatchCopyBack3), (string) null);
      }
    }

    internal static string ArgumentMismatchAmbiguousCopyBack3
    {
      get
      {
        return SR.GetResourceString(nameof (ArgumentMismatchAmbiguousCopyBack3), (string) null);
      }
    }

    internal static string ArgumentNarrowingCopyBack3
    {
      get
      {
        return SR.GetResourceString(nameof (ArgumentNarrowingCopyBack3), (string) null);
      }
    }

    internal static string UnboundTypeParam1
    {
      get
      {
        return SR.GetResourceString(nameof (UnboundTypeParam1), (string) null);
      }
    }

    internal static string TypeInferenceFails1
    {
      get
      {
        return SR.GetResourceString(nameof (TypeInferenceFails1), (string) null);
      }
    }

    internal static string FailedTypeArgumentBinding
    {
      get
      {
        return SR.GetResourceString(nameof (FailedTypeArgumentBinding), (string) null);
      }
    }

    internal static string UnaryOperand2
    {
      get
      {
        return SR.GetResourceString(nameof (UnaryOperand2), (string) null);
      }
    }

    internal static string BinaryOperands3
    {
      get
      {
        return SR.GetResourceString(nameof (BinaryOperands3), (string) null);
      }
    }

    internal static string NoValidOperator_StringType1
    {
      get
      {
        return SR.GetResourceString(nameof (NoValidOperator_StringType1), (string) null);
      }
    }

    internal static string NoValidOperator_NonStringType1
    {
      get
      {
        return SR.GetResourceString(nameof (NoValidOperator_NonStringType1), (string) null);
      }
    }

    internal static string PropertySetMissingArgument1
    {
      get
      {
        return SR.GetResourceString(nameof (PropertySetMissingArgument1), (string) null);
      }
    }

    internal static string EmptyPlaceHolderMessage
    {
      get
      {
        return SR.GetResourceString(nameof (EmptyPlaceHolderMessage), (string) null);
      }
    }
  }
}
