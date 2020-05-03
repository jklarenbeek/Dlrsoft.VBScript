// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.NewLateBinding
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>This class provides helpers that the Visual Basic compiler uses for late binding calls; it is not meant to be called directly from your code.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class NewLateBinding
  {
    private NewLateBinding()
    {
    }

    /// <summary>Indicates whether a call requires late-bound evaluation. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="instance">An instance of the call object exposing the property or method.</param>
    /// <param name="type">The type of the call object.</param>
    /// <param name="memberName">The name of the property or method on the call object.</param>
    /// <param name="arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="allowFunctionEvaluation">A <see langword="Boolean" /> value that specifies whether to allow function evaluation.</param>
    /// <param name="allowPropertyEvaluation">A <see langword="Boolean" /> value that specifies whether to allow property evaluation.</param>
    /// <returns>A <see langword="Boolean" /> value that indicates whether the expression requires late-bound evaluation.</returns>
    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static bool LateCanEvaluate(
      object instance,
      Type type,
      string memberName,
      object[] arguments,
      bool allowFunctionEvaluation,
      bool allowPropertyEvaluation)
    {
      Symbols.Container container = (object) type == null ? new Symbols.Container(instance) : new Symbols.Container(type);
      MemberInfo[] members = container.GetMembers(ref memberName, false);
      return members.Length == 0 || (members[0].MemberType != MemberTypes.Field ? (members[0].MemberType != MemberTypes.Method ? members[0].MemberType != MemberTypes.Property || allowPropertyEvaluation : allowFunctionEvaluation) : arguments.Length == 0 || (new Symbols.Container(container.GetFieldValue((FieldInfo) members[0])).IsArray || allowPropertyEvaluation));
    }

    /// <summary>Executes a late-bound method or function call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Type">The type of the call object.</param>
    /// <param name="MemberName">The name of the property or method on the call object.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="TypeArguments">An array of argument types; used only for generic calls to pass argument types.</param>
    /// <param name="CopyBack">An array of <see langword="Boolean" /> values that the late binder uses to communicate back to the call site which arguments match <see langword="ByRef" /> parameters. Each <see langword="True" /> value indicates that the arguments matched and should be copied out after the call to <see langword="LateCall" /> is complete.</param>
    /// <param name="IgnoreReturn">A <see langword="Boolean" /> value indicating whether or not the return value can be ignored.</param>
    /// <returns>An instance of the call object.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object LateCall(
      object Instance,
      Type Type,
      string MemberName,
      object[] Arguments,
      string[] ArgumentNames,
      Type[] TypeArguments,
      bool[] CopyBack,
      bool IgnoreReturn)
    {
      if (Arguments == null)
        Arguments = Symbols.NoArguments;
      if (ArgumentNames == null)
        ArgumentNames = Symbols.NoArgumentNames;
      if (TypeArguments == null)
        TypeArguments = Symbols.NoTypeArguments;
      if ((object) Type != null)
      {
        Symbols.Container container1 = new Symbols.Container(Type);
      }
      else
      {
        Symbols.Container container2 = new Symbols.Container(Instance);
      }
      IDynamicMetaObjectProvider idmop = IDOUtils.TryCastToIDMOP(Instance);
      return idmop == null || TypeArguments != Symbols.NoTypeArguments ? NewLateBinding.ObjectLateCall(Instance, Type, MemberName, Arguments, ArgumentNames, TypeArguments, CopyBack, IgnoreReturn) : IDOBinder.IDOCall(idmop, MemberName, Arguments, ArgumentNames, CopyBack, IgnoreReturn);
    }

    /// <summary>Executes a late-bound method or function call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="MemberName">The name of the property or method on the call object.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="IgnoreReturn">A <see langword="Boolean" /> value indicating whether or not the return value can be ignored.</param>
    /// <returns>An instance of the call object.</returns>
    [DebuggerHidden]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerStepThrough]
    [Obsolete("do not use this method", true)]
    public static object FallbackCall(
      object Instance,
      string MemberName,
      object[] Arguments,
      string[] ArgumentNames,
      bool IgnoreReturn)
    {
      return NewLateBinding.ObjectLateCall(Instance, (Type) null, MemberName, Arguments, ArgumentNames, Symbols.NoTypeArguments, IDOBinder.GetCopyBack(), IgnoreReturn);
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    private static object ObjectLateCall(
      object instance,
      Type type,
      string memberName,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      bool[] copyBack,
      bool ignoreReturn)
    {
      Symbols.Container baseReference = (object) type == null ? new Symbols.Container(instance) : new Symbols.Container(type);
      BindingFlags invocationFlags = ReflectionExtensions.BindingFlagsInvokeMethod | ReflectionExtensions.BindingFlagsGetProperty;
      if (ignoreReturn)
        invocationFlags |= ReflectionExtensions.BindingFlagsIgnoreReturn;
      OverloadResolution.ResolutionFailure failure;
      return NewLateBinding.CallMethod(baseReference, memberName, arguments, argumentNames, typeArguments, copyBack, invocationFlags, true, ref failure);
    }

    internal static bool CanBindCall(
      object instance,
      string memberName,
      object[] arguments,
      string[] argumentNames,
      bool ignoreReturn)
    {
      Symbols.Container baseReference = new Symbols.Container(instance);
      BindingFlags lookupFlags = ReflectionExtensions.BindingFlagsInvokeMethod | ReflectionExtensions.BindingFlagsGetProperty;
      if (ignoreReturn)
        lookupFlags |= ReflectionExtensions.BindingFlagsIgnoreReturn;
      MemberInfo[] members = baseReference.GetMembers(ref memberName, false);
      bool flag;
      if (members == null || members.Length == 0)
      {
        flag = false;
      }
      else
      {
        OverloadResolution.ResolutionFailure failure;
        NewLateBinding.ResolveCall(baseReference, memberName, members, arguments, argumentNames, Symbols.NoTypeArguments, lookupFlags, false, ref failure);
        flag = failure == OverloadResolution.ResolutionFailure.None;
      }
      return flag;
    }

    /// <summary>Executes a late-bound call to the default method or function. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="ReportErrors">A <see langword="Boolean" /> value used to specify whether to throw exceptions when an error is encountered. Set to <see langword="True" /> to throw exceptions. Set to <see langword="False" /> to return <see langword="Nothing" /> when an error is encountered.</param>
    /// <returns>An instance of the call object.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object LateCallInvokeDefault(
      object Instance,
      object[] Arguments,
      string[] ArgumentNames,
      bool ReportErrors)
    {
      return NewLateBinding.InternalLateInvokeDefault(Instance, Arguments, ArgumentNames, ReportErrors, IDOBinder.GetCopyBack());
    }

    /// <summary>Executes a late-bound get of the default property or field. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="ReportErrors">A <see langword="Boolean" /> value used to specify whether to throw exceptions when an error is encountered. Set to <see langword="True" /> to throw exceptions. Set to <see langword="False" /> to return <see langword="Nothing" /> when an error is encountered.</param>
    /// <returns>An instance of the call object.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object LateGetInvokeDefault(
      object Instance,
      object[] Arguments,
      string[] ArgumentNames,
      bool ReportErrors)
    {
      return IDOUtils.TryCastToIDMOP(Instance) != null || Arguments != null && Arguments.Length > 0 ? NewLateBinding.InternalLateInvokeDefault(Instance, Arguments, ArgumentNames, ReportErrors, IDOBinder.GetCopyBack()) : Instance;
    }

    private static object InternalLateInvokeDefault(
      object instance,
      object[] arguments,
      string[] argumentNames,
      bool reportErrors,
      bool[] copyBack)
    {
      IDynamicMetaObjectProvider idmop = IDOUtils.TryCastToIDMOP(instance);
      return idmop == null ? NewLateBinding.ObjectLateInvokeDefault(instance, arguments, argumentNames, reportErrors, copyBack) : IDOBinder.IDOInvokeDefault(idmop, arguments, argumentNames, reportErrors, copyBack);
    }

    /// <summary>Executes a late-bound get of the default property or field, or call to the default method or function. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method. If <paramref name="Instance" /> is of type <see cref="T:System.Dynamic.IDynamicMetaObjectProvider" />, then bind using the Dynamic Language Runtime; otherwise perform standard late-binding.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="ReportErrors">A <see langword="Boolean" /> value used to specify whether to throw exceptions when an error is encountered. Set to <see langword="True" /> to throw exceptions. Set to <see langword="False" /> to return <see langword="Nothing" /> when an error is encountered.</param>
    /// <returns>An instance of the call object.</returns>
    [DebuggerStepThrough]
    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerHidden]
    public static object FallbackInvokeDefault1(
      object Instance,
      object[] Arguments,
      string[] ArgumentNames,
      bool ReportErrors)
    {
      return IDOBinder.IDOFallbackInvokeDefault((IDynamicMetaObjectProvider) Instance, Arguments, ArgumentNames, ReportErrors, IDOBinder.GetCopyBack());
    }

    /// <summary>Executes a late-bound get of the default property or field, or call to the default method or function. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="ReportErrors">A <see langword="Boolean" /> value used to specify whether to throw exceptions when an error is encountered. Set to <see langword="True" /> to throw exceptions. Set to <see langword="False" /> to return <see langword="Nothing" /> when an error is encountered.</param>
    /// <returns>An instance of the call object.</returns>
    [Obsolete("do not use this method", true)]
    [DebuggerHidden]
    [DebuggerStepThrough]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static object FallbackInvokeDefault2(
      object Instance,
      object[] Arguments,
      string[] ArgumentNames,
      bool ReportErrors)
    {
      return NewLateBinding.ObjectLateInvokeDefault(Instance, Arguments, ArgumentNames, ReportErrors, IDOBinder.GetCopyBack());
    }

    [DebuggerHidden]
    [DebuggerStepThrough]
    private static object ObjectLateInvokeDefault(
      object instance,
      object[] arguments,
      string[] argumentNames,
      bool reportErrors,
      bool[] copyBack)
    {
      Symbols.Container container = new Symbols.Container(instance);
      OverloadResolution.ResolutionFailure failure;
      object obj = NewLateBinding.InternalLateIndexGet(instance, arguments, argumentNames, reportErrors || arguments.Length != 0 || container.IsArray, ref failure, copyBack);
      return failure != OverloadResolution.ResolutionFailure.None ? instance : obj;
    }

    /// <summary>Executes a late-bound property get or field access call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <returns>An instance of the call object.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object LateIndexGet(object Instance, object[] Arguments, string[] ArgumentNames)
    {
      return NewLateBinding.InternalLateInvokeDefault(Instance, Arguments, ArgumentNames, true, (bool[]) null);
    }

    private static object LateIndexGet(
      object instance,
      object[] arguments,
      string[] argumentNames,
      bool[] copyBack)
    {
      return NewLateBinding.InternalLateInvokeDefault(instance, arguments, argumentNames, true, copyBack);
    }

    private static object InternalLateIndexGet(
      object instance,
      object[] arguments,
      string[] argumentNames,
      bool reportErrors,
      ref OverloadResolution.ResolutionFailure failure,
      bool[] copyBack)
    {
      failure = OverloadResolution.ResolutionFailure.None;
      if (arguments == null)
        arguments = Symbols.NoArguments;
      if (argumentNames == null)
        argumentNames = Symbols.NoArgumentNames;
      Symbols.Container baseReference = new Symbols.Container(instance);
      object obj;
      if (baseReference.IsArray)
      {
        if (argumentNames.Length > 0)
        {
          failure = OverloadResolution.ResolutionFailure.InvalidArgument;
          if (reportErrors)
            throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidNamedArgs));
          obj = (object) null;
        }
        else
        {
          NewLateBinding.ResetCopyback(copyBack);
          obj = baseReference.GetArrayValue(arguments);
        }
      }
      else
        obj = NewLateBinding.CallMethod(baseReference, "", arguments, argumentNames, Symbols.NoTypeArguments, copyBack, ReflectionExtensions.BindingFlagsInvokeMethod | ReflectionExtensions.BindingFlagsGetProperty, reportErrors, ref failure);
      return obj;
    }

    internal static bool CanBindInvokeDefault(
      object instance,
      object[] arguments,
      string[] argumentNames,
      bool reportErrors)
    {
      Symbols.Container container = new Symbols.Container(instance);
      reportErrors = reportErrors || arguments.Length != 0 || container.IsArray;
      return !reportErrors || (!container.IsArray ? NewLateBinding.CanBindCall(instance, "", arguments, argumentNames, false) : argumentNames.Length == 0);
    }

    internal static void ResetCopyback(bool[] copyBack)
    {
      if (copyBack == null)
        return;
      int num = checked (copyBack.Length - 1);
      int index = 0;
      while (index <= num)
      {
        copyBack[index] = false;
        checked { ++index; }
      }
    }

    /// <summary>Executes a late-bound property get or field access call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Type">The type of the call object.</param>
    /// <param name="MemberName">The name of the property or method on the call object.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="TypeArguments">An array of argument types; used only for generic calls to pass argument types.</param>
    /// <param name="CopyBack">An array of <see langword="Boolean" /> values that the late binder uses to communicate back to the call site which arguments match <see langword="ByRef" /> parameters. Each <see langword="True" /> value indicates that the arguments matched and should be copied out after the call to <see langword="LateCall" /> is complete.</param>
    /// <returns>An instance of the call object.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object LateGet(
      object Instance,
      Type Type,
      string MemberName,
      object[] Arguments,
      string[] ArgumentNames,
      Type[] TypeArguments,
      bool[] CopyBack)
    {
      if (Arguments == null)
        Arguments = Symbols.NoArguments;
      if (ArgumentNames == null)
        ArgumentNames = Symbols.NoArgumentNames;
      if (TypeArguments == null)
        TypeArguments = Symbols.NoTypeArguments;
      if ((object) Type != null)
      {
        Symbols.Container container1 = new Symbols.Container(Type);
      }
      else
      {
        Symbols.Container container2 = new Symbols.Container(Instance);
      }
      int flagsInvokeMethod = (int) ReflectionExtensions.BindingFlagsInvokeMethod;
      int flagsGetProperty = (int) ReflectionExtensions.BindingFlagsGetProperty;
      IDynamicMetaObjectProvider idmop = IDOUtils.TryCastToIDMOP(Instance);
      return idmop == null || TypeArguments != Symbols.NoTypeArguments ? NewLateBinding.ObjectLateGet(Instance, Type, MemberName, Arguments, ArgumentNames, TypeArguments, CopyBack) : IDOBinder.IDOGet(idmop, MemberName, Arguments, ArgumentNames, CopyBack);
    }

    /// <summary>Executes a late-bound property get or field access call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="MemberName">The name of the property or method on the call object.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <returns>An instance of the call object.</returns>
    [DebuggerStepThrough]
    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerHidden]
    public static object FallbackGet(
      object Instance,
      string MemberName,
      object[] Arguments,
      string[] ArgumentNames)
    {
      return NewLateBinding.ObjectLateGet(Instance, (Type) null, MemberName, Arguments, ArgumentNames, Symbols.NoTypeArguments, IDOBinder.GetCopyBack());
    }

    [DebuggerHidden]
    [DebuggerStepThrough]
    private static object ObjectLateGet(
      object instance,
      Type type,
      string memberName,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      bool[] copyBack)
    {
      Symbols.Container baseReference = (object) type == null ? new Symbols.Container(instance) : new Symbols.Container(type);
      BindingFlags bindingFlags = ReflectionExtensions.BindingFlagsInvokeMethod | ReflectionExtensions.BindingFlagsGetProperty;
      MemberInfo[] members = baseReference.GetMembers(ref memberName, true);
      if (members[0].MemberType == MemberTypes.Field)
      {
        if (typeArguments.Length > 0)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidValue));
        object fieldValue = baseReference.GetFieldValue((FieldInfo) members[0]);
        return arguments.Length == 0 ? fieldValue : NewLateBinding.LateIndexGet(fieldValue, arguments, argumentNames, copyBack);
      }
      if (argumentNames.Length > arguments.Length || copyBack != null && copyBack.Length != arguments.Length)
        throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidValue));
      OverloadResolution.ResolutionFailure failure;
      Symbols.Method targetProcedure1 = NewLateBinding.ResolveCall(baseReference, memberName, members, arguments, argumentNames, typeArguments, bindingFlags, false, ref failure);
      if (failure == OverloadResolution.ResolutionFailure.None)
        return baseReference.InvokeMethod(targetProcedure1, arguments, copyBack, bindingFlags);
      if (arguments.Length > 0 && members.Length == 1 && NewLateBinding.IsZeroArgumentCall(members[0]))
      {
        Symbols.Method targetProcedure2 = NewLateBinding.ResolveCall(baseReference, memberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, typeArguments, bindingFlags, false, ref failure);
        if (failure == OverloadResolution.ResolutionFailure.None)
        {
          object instance1 = baseReference.InvokeMethod(targetProcedure2, Symbols.NoArguments, (bool[]) null, bindingFlags);
          if (instance1 == null)
            throw new MissingMemberException(Utils.GetResourceString(SR.IntermediateLateBoundNothingResult1, targetProcedure2.ToString(), baseReference.VBFriendlyName));
          object obj = NewLateBinding.InternalLateIndexGet(instance1, arguments, argumentNames, false, ref failure, copyBack);
          if (failure == OverloadResolution.ResolutionFailure.None)
            return obj;
        }
      }
      NewLateBinding.ResolveCall(baseReference, memberName, members, arguments, argumentNames, typeArguments, bindingFlags, true, ref failure);
      throw new InternalErrorException();
    }

    internal static bool CanBindGet(
      object instance,
      string memberName,
      object[] arguments,
      string[] argumentNames)
    {
      Symbols.Container baseReference = new Symbols.Container(instance);
      BindingFlags lookupFlags = ReflectionExtensions.BindingFlagsInvokeMethod | ReflectionExtensions.BindingFlagsGetProperty;
      MemberInfo[] members = baseReference.GetMembers(ref memberName, false);
      bool flag;
      if (members == null || members.Length == 0)
        flag = false;
      else if (members[0].MemberType == MemberTypes.Field)
      {
        flag = true;
      }
      else
      {
        OverloadResolution.ResolutionFailure failure;
        NewLateBinding.ResolveCall(baseReference, memberName, members, arguments, argumentNames, Symbols.NoTypeArguments, lookupFlags, false, ref failure);
        if (failure == OverloadResolution.ResolutionFailure.None)
        {
          flag = true;
        }
        else
        {
          if (arguments.Length > 0 && members.Length == 1 && NewLateBinding.IsZeroArgumentCall(members[0]))
          {
            NewLateBinding.ResolveCall(baseReference, memberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, lookupFlags, false, ref failure);
            if (failure == OverloadResolution.ResolutionFailure.None)
            {
              flag = true;
              goto label_10;
            }
          }
          flag = false;
        }
      }
label_10:
      return flag;
    }

    internal static bool IsZeroArgumentCall(MemberInfo member)
    {
      if (member.MemberType == MemberTypes.Method && ((MethodBase) member).GetParameters().Length == 0)
        return true;
      return member.MemberType == MemberTypes.Property && ((PropertyInfo) member).GetIndexParameters().Length == 0;
    }

    /// <summary>Executes a late-bound property set or field write call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="OptimisticSet">A <see langword="Boolean" /> value used to determine whether the set operation will work. Set to <see langword="True" /> when you believe that an intermediate value has been set in the property or field; otherwise <see langword="False" />.</param>
    /// <param name="RValueBase">A <see langword="Boolean" /> value that specifies when the base reference of the late reference is an <see langword="RValue" />. Set to <see langword="True" /> when the base reference of the late reference is an <see langword="RValue" />; this allows you to generate a run-time exception for late assignments to fields of <see langword="RValues" /> of value types. Otherwise, set to <see langword="False" />.</param>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static void LateIndexSetComplex(
      object Instance,
      object[] Arguments,
      string[] ArgumentNames,
      bool OptimisticSet,
      bool RValueBase)
    {
      IDynamicMetaObjectProvider idmop = IDOUtils.TryCastToIDMOP(Instance);
      if (idmop != null)
        IDOBinder.IDOIndexSetComplex(idmop, Arguments, ArgumentNames, OptimisticSet, RValueBase);
      else
        NewLateBinding.ObjectLateIndexSetComplex(Instance, Arguments, ArgumentNames, OptimisticSet, RValueBase);
    }

    /// <summary>Executes a late-bound property set or field write call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="OptimisticSet">A <see langword="Boolean" /> value used to determine whether the set operation will work. Set to <see langword="True" /> when you believe that an intermediate value has been set in the property or field; otherwise <see langword="False" />.</param>
    /// <param name="RValueBase">A <see langword="Boolean" /> value that specifies when the base reference of the late reference is an <see langword="RValue" />. Set to <see langword="True" /> when the base reference of the late reference is an <see langword="RValue" />; this allows you to generate a run-time exception for late assignments to fields of <see langword="RValues" /> of value types. Otherwise, set to <see langword="False" />.</param>
    [DebuggerHidden]
    [DebuggerStepThrough]
    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void FallbackIndexSetComplex(
      object Instance,
      object[] Arguments,
      string[] ArgumentNames,
      bool OptimisticSet,
      bool RValueBase)
    {
      NewLateBinding.ObjectLateIndexSetComplex(Instance, Arguments, ArgumentNames, OptimisticSet, RValueBase);
    }

    [DebuggerHidden]
    [DebuggerStepThrough]
    internal static void ObjectLateIndexSetComplex(
      object instance,
      object[] arguments,
      string[] argumentNames,
      bool optimisticSet,
      bool rValueBase)
    {
      if (arguments == null)
        arguments = Symbols.NoArguments;
      if (argumentNames == null)
        argumentNames = Symbols.NoArgumentNames;
      Symbols.Container baseReference = new Symbols.Container(instance);
      if (baseReference.IsArray)
      {
        if (argumentNames.Length > 0)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidNamedArgs));
        baseReference.SetArrayValue(arguments);
      }
      else
      {
        if (argumentNames.Length > arguments.Length)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidValue));
        if (arguments.Length < 1)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidValue));
        string memberName = "";
        BindingFlags flagsSetProperty = ReflectionExtensions.BindingFlagsSetProperty;
        MemberInfo[] members = baseReference.GetMembers(ref memberName, true);
        OverloadResolution.ResolutionFailure failure;
        Symbols.Method targetProcedure = NewLateBinding.ResolveCall(baseReference, memberName, members, arguments, argumentNames, Symbols.NoTypeArguments, flagsSetProperty, false, ref failure);
        if (failure == OverloadResolution.ResolutionFailure.None)
        {
          if (rValueBase && baseReference.IsValueType)
            throw new Exception(Utils.GetResourceString(SR.RValueBaseForValueType, baseReference.VBFriendlyName, baseReference.VBFriendlyName));
          baseReference.InvokeMethod(targetProcedure, arguments, (bool[]) null, flagsSetProperty);
        }
        else if (!optimisticSet)
        {
          NewLateBinding.ResolveCall(baseReference, memberName, members, arguments, argumentNames, Symbols.NoTypeArguments, flagsSetProperty, true, ref failure);
          throw new InternalErrorException();
        }
      }
    }

    internal static bool CanIndexSetComplex(
      object instance,
      object[] arguments,
      string[] argumentNames,
      bool optimisticSet,
      bool rValueBase)
    {
      Symbols.Container baseReference = new Symbols.Container(instance);
      bool flag;
      if (baseReference.IsArray)
      {
        flag = argumentNames.Length == 0;
      }
      else
      {
        string memberName = "";
        BindingFlags flagsSetProperty = ReflectionExtensions.BindingFlagsSetProperty;
        MemberInfo[] members = baseReference.GetMembers(ref memberName, false);
        if (members == null || members.Length == 0)
        {
          flag = false;
        }
        else
        {
          OverloadResolution.ResolutionFailure failure;
          NewLateBinding.ResolveCall(baseReference, memberName, members, arguments, argumentNames, Symbols.NoTypeArguments, flagsSetProperty, false, ref failure);
          flag = failure != OverloadResolution.ResolutionFailure.None ? optimisticSet : !rValueBase || !baseReference.IsValueType;
        }
      }
      return flag;
    }

    /// <summary>Executes a late-bound property set or field write call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static void LateIndexSet(object Instance, object[] Arguments, string[] ArgumentNames)
    {
      IDynamicMetaObjectProvider idmop = IDOUtils.TryCastToIDMOP(Instance);
      if (idmop != null)
        IDOBinder.IDOIndexSet(idmop, Arguments, ArgumentNames);
      else
        NewLateBinding.ObjectLateIndexSet(Instance, Arguments, ArgumentNames);
    }

    /// <summary>Executes a late-bound property set or field write call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    [DebuggerHidden]
    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerStepThrough]
    public static void FallbackIndexSet(
      object Instance,
      object[] Arguments,
      string[] ArgumentNames)
    {
      NewLateBinding.ObjectLateIndexSet(Instance, Arguments, ArgumentNames);
    }

    [DebuggerHidden]
    [DebuggerStepThrough]
    private static void ObjectLateIndexSet(
      object Instance,
      object[] Arguments,
      string[] ArgumentNames)
    {
      NewLateBinding.ObjectLateIndexSetComplex(Instance, Arguments, ArgumentNames, false, false);
    }

    /// <summary>Executes a late-bound property set or field write call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Type">The type of the call object.</param>
    /// <param name="MemberName">The name of the property or method on the call object.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="TypeArguments">An array of argument types; used only for generic calls to pass argument types.</param>
    /// <param name="OptimisticSet">A <see langword="Boolean" /> value used to determine whether the set operation will work. Set to <see langword="True" /> when you believe that an intermediate value has been set in the property or field; otherwise <see langword="False" />.</param>
    /// <param name="RValueBase">A <see langword="Boolean" /> value that specifies when the base reference of the late reference is an <see langword="RValue" />. Set to <see langword="True" /> when the base reference of the late reference is an <see langword="RValue" />; this allows you to generate a run-time exception for late assignments to fields of <see langword="RValues" /> of value types. Otherwise, set to <see langword="False" />.</param>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static void LateSetComplex(
      object Instance,
      Type Type,
      string MemberName,
      object[] Arguments,
      string[] ArgumentNames,
      Type[] TypeArguments,
      bool OptimisticSet,
      bool RValueBase)
    {
      IDynamicMetaObjectProvider idmop = IDOUtils.TryCastToIDMOP(Instance);
      if (idmop != null && TypeArguments == null)
        IDOBinder.IDOSetComplex(idmop, MemberName, Arguments, ArgumentNames, OptimisticSet, RValueBase);
      else
        NewLateBinding.ObjectLateSetComplex(Instance, Type, MemberName, Arguments, ArgumentNames, TypeArguments, OptimisticSet, RValueBase);
    }

    /// <summary>Executes a late-bound property set or field write call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="MemberName">The name of the property or method on the call object.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="OptimisticSet">A <see langword="Boolean" /> value used to determine whether the set operation will work. Set to <see langword="True" /> when you believe that an intermediate value has been set in the property or field; otherwise <see langword="False" />.</param>
    /// <param name="RValueBase">A <see langword="Boolean" /> value that specifies when the base reference of the late reference is an <see langword="RValue" />. Set to <see langword="True" /> when the base reference of the late reference is an <see langword="RValue" />; this allows you to generate a run-time exception for late assignments to fields of <see langword="RValues" /> of value types. Otherwise, set to <see langword="False" />.</param>
    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static void FallbackSetComplex(
      object Instance,
      string MemberName,
      object[] Arguments,
      bool OptimisticSet,
      bool RValueBase)
    {
      NewLateBinding.ObjectLateSetComplex(Instance, (Type) null, MemberName, Arguments, new string[0], Symbols.NoTypeArguments, OptimisticSet, RValueBase);
    }

    [DebuggerHidden]
    [DebuggerStepThrough]
    internal static void ObjectLateSetComplex(
      object instance,
      Type type,
      string memberName,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      bool optimisticSet,
      bool rValueBase)
    {
      NewLateBinding.LateSet(instance, type, memberName, arguments, argumentNames, typeArguments, optimisticSet, rValueBase, (CallType) 0);
    }

    /// <summary>Executes a late-bound property set or field write call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Type">The type of the call object.</param>
    /// <param name="MemberName">The name of the property or method on the call object.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="TypeArguments">An array of argument types; used only for generic calls to pass argument types.</param>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static void LateSet(
      object Instance,
      Type Type,
      string MemberName,
      object[] Arguments,
      string[] ArgumentNames,
      Type[] TypeArguments)
    {
      IDynamicMetaObjectProvider idmop = IDOUtils.TryCastToIDMOP(Instance);
      if (idmop != null && TypeArguments == null)
        IDOBinder.IDOSet(idmop, MemberName, ArgumentNames, Arguments);
      else
        NewLateBinding.ObjectLateSet(Instance, Type, MemberName, Arguments, ArgumentNames, TypeArguments);
    }

    /// <summary>Executes a late-bound property set or field write call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="MemberName">The name of the property or method on the call object.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    [DebuggerHidden]
    [DebuggerStepThrough]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    public static void FallbackSet(object Instance, string MemberName, object[] Arguments)
    {
      NewLateBinding.ObjectLateSet(Instance, (Type) null, MemberName, Arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments);
    }

    internal static void ObjectLateSet(
      object instance,
      Type type,
      string memberName,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments)
    {
      NewLateBinding.LateSet(instance, type, memberName, arguments, argumentNames, typeArguments, false, false, (CallType) 0);
    }

    /// <summary>Executes a late-bound property set or field write call. This helper method is not meant to be called directly from your code.</summary>
    /// <param name="Instance">An instance of the call object exposing the property or method.</param>
    /// <param name="Type">The type of the call object.</param>
    /// <param name="MemberName">The name of the property or method on the call object.</param>
    /// <param name="Arguments">An array containing the arguments to be passed to the property or method being called.</param>
    /// <param name="ArgumentNames">An array of argument names.</param>
    /// <param name="TypeArguments">An array of argument types; used only for generic calls to pass argument types.</param>
    /// <param name="OptimisticSet">A <see langword="Boolean" /> value used to determine whether the set operation will work. Set to <see langword="True" /> when you believe that an intermediate value has been set in the property or field; otherwise <see langword="False" />.</param>
    /// <param name="RValueBase">A <see langword="Boolean" /> value that specifies when the base reference of the late reference is an <see langword="RValue" />. Set to <see langword="True" /> when the base reference of the late reference is an <see langword="RValue" />; this allows you to generate a run-time exception for late assignments to fields of <see langword="RValues" /> of value types. Otherwise, set to <see langword="False" />.</param>
    /// <param name="CallType">An enumeration member of type <see cref="T:Microsoft.VisualBasic.CallType" /> representing the type of procedure being called. The value of CallType can be <see langword="Method" />, <see langword="Get" />, or <see langword="Set" />. Only <see langword="Set" /> is used.</param>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static void LateSet(
      object Instance,
      Type Type,
      string MemberName,
      object[] Arguments,
      string[] ArgumentNames,
      Type[] TypeArguments,
      bool OptimisticSet,
      bool RValueBase,
      CallType CallType)
    {
      if (Arguments == null)
        Arguments = Symbols.NoArguments;
      if (ArgumentNames == null)
        ArgumentNames = Symbols.NoArgumentNames;
      if (TypeArguments == null)
        TypeArguments = Symbols.NoTypeArguments;
      Symbols.Container baseReference = (object) Type == null ? new Symbols.Container(Instance) : new Symbols.Container(Type);
      MemberInfo[] members = baseReference.GetMembers(ref MemberName, !OptimisticSet);
      if (members.Length == 0 & OptimisticSet)
        return;
      if (members[0].MemberType == MemberTypes.Field)
      {
        if (TypeArguments.Length > 0)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidValue));
        if (Arguments.Length == 1)
        {
          if (RValueBase && baseReference.IsValueType)
            throw new Exception(Utils.GetResourceString(SR.RValueBaseForValueType, baseReference.VBFriendlyName, baseReference.VBFriendlyName));
          baseReference.SetFieldValue((FieldInfo) members[0], Arguments[0]);
        }
        else
          NewLateBinding.LateIndexSetComplex(baseReference.GetFieldValue((FieldInfo) members[0]), Arguments, ArgumentNames, OptimisticSet, true);
      }
      else
      {
        BindingFlags flagsSetProperty = ReflectionExtensions.BindingFlagsSetProperty;
        if (ArgumentNames.Length > Arguments.Length)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidValue));
        OverloadResolution.ResolutionFailure failure;
        if (TypeArguments.Length == 0)
        {
          Symbols.Method targetProcedure = NewLateBinding.ResolveCall(baseReference, MemberName, members, Arguments, ArgumentNames, Symbols.NoTypeArguments, flagsSetProperty, false, ref failure);
          if (failure == OverloadResolution.ResolutionFailure.None)
          {
            if (RValueBase && baseReference.IsValueType)
              throw new Exception(Utils.GetResourceString(SR.RValueBaseForValueType, baseReference.VBFriendlyName, baseReference.VBFriendlyName));
            baseReference.InvokeMethod(targetProcedure, Arguments, (bool[]) null, flagsSetProperty);
            return;
          }
        }
        BindingFlags bindingFlags = ReflectionExtensions.BindingFlagsInvokeMethod | ReflectionExtensions.BindingFlagsGetProperty;
        if (failure == OverloadResolution.ResolutionFailure.None || failure == OverloadResolution.ResolutionFailure.MissingMember)
        {
          Symbols.Method targetProcedure = NewLateBinding.ResolveCall(baseReference, MemberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, TypeArguments, bindingFlags, false, ref failure);
          if (failure == OverloadResolution.ResolutionFailure.None)
          {
            object Instance1 = baseReference.InvokeMethod(targetProcedure, Symbols.NoArguments, (bool[]) null, bindingFlags);
            if (Instance1 == null)
              throw new MissingMemberException(Utils.GetResourceString(SR.IntermediateLateBoundNothingResult1, targetProcedure.ToString(), baseReference.VBFriendlyName));
            NewLateBinding.LateIndexSetComplex(Instance1, Arguments, ArgumentNames, OptimisticSet, true);
            return;
          }
        }
        if (!OptimisticSet)
        {
          if (TypeArguments.Length == 0)
            NewLateBinding.ResolveCall(baseReference, MemberName, members, Arguments, ArgumentNames, TypeArguments, flagsSetProperty, true, ref failure);
          else
            NewLateBinding.ResolveCall(baseReference, MemberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, TypeArguments, bindingFlags, true, ref failure);
          throw new InternalErrorException();
        }
      }
    }

    internal static bool CanBindSet(
      object instance,
      string memberName,
      object value,
      bool optimisticSet,
      bool rValueBase)
    {
      Symbols.Container baseReference = new Symbols.Container(instance);
      object[] arguments = new object[1]{ value };
      MemberInfo[] members = baseReference.GetMembers(ref memberName, false);
      bool flag;
      if (members == null || members.Length == 0)
        flag = false;
      else if (members[0].MemberType == MemberTypes.Field)
      {
        flag = arguments.Length != 1 || !rValueBase || !baseReference.IsValueType;
      }
      else
      {
        OverloadResolution.ResolutionFailure failure;
        NewLateBinding.ResolveCall(baseReference, memberName, members, arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, ReflectionExtensions.BindingFlagsSetProperty, false, ref failure);
        if (failure == OverloadResolution.ResolutionFailure.None)
        {
          flag = !rValueBase || !baseReference.IsValueType;
        }
        else
        {
          BindingFlags lookupFlags = ReflectionExtensions.BindingFlagsInvokeMethod | ReflectionExtensions.BindingFlagsGetProperty;
          if (failure == OverloadResolution.ResolutionFailure.MissingMember)
          {
            NewLateBinding.ResolveCall(baseReference, memberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, lookupFlags, false, ref failure);
            if (failure == OverloadResolution.ResolutionFailure.None)
            {
              flag = true;
              goto label_10;
            }
          }
          flag = optimisticSet;
        }
      }
label_10:
      return flag;
    }

    private static object CallMethod(
      Symbols.Container baseReference,
      string methodName,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      bool[] copyBack,
      BindingFlags invocationFlags,
      bool reportErrors,
      ref OverloadResolution.ResolutionFailure failure)
    {
      failure = OverloadResolution.ResolutionFailure.None;
      object obj;
      if (argumentNames.Length > arguments.Length || copyBack != null && copyBack.Length != arguments.Length)
      {
        failure = OverloadResolution.ResolutionFailure.InvalidArgument;
        if (reportErrors)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidValue));
        obj = (object) null;
      }
      else if (Symbols.HasFlag(invocationFlags, ReflectionExtensions.BindingFlagsSetProperty) && arguments.Length < 1)
      {
        failure = OverloadResolution.ResolutionFailure.InvalidArgument;
        if (reportErrors)
          throw new ArgumentException(Utils.GetResourceString(SR.Argument_InvalidValue));
        obj = (object) null;
      }
      else
      {
        MemberInfo[] members = baseReference.GetMembers(ref methodName, reportErrors);
        if (members == null || members.Length == 0)
        {
          failure = OverloadResolution.ResolutionFailure.MissingMember;
          if (reportErrors)
            baseReference.GetMembers(ref methodName, true);
          obj = (object) null;
        }
        else
        {
          Symbols.Method targetProcedure = NewLateBinding.ResolveCall(baseReference, methodName, members, arguments, argumentNames, typeArguments, invocationFlags, reportErrors, ref failure);
          obj = failure != OverloadResolution.ResolutionFailure.None ? (object) null : baseReference.InvokeMethod(targetProcedure, arguments, copyBack, invocationFlags);
        }
      }
      return obj;
    }

    internal static MethodInfo MatchesPropertyRequirements(
      Symbols.Method targetProcedure,
      BindingFlags flags)
    {
      return !Symbols.HasFlag(flags, ReflectionExtensions.BindingFlagsSetProperty) ? targetProcedure.AsProperty().GetGetMethod() : targetProcedure.AsProperty().GetSetMethod();
    }

    internal static Exception ReportPropertyMismatch(
      Symbols.Method targetProcedure,
      BindingFlags flags)
    {
      Exception exception;
      if (Symbols.HasFlag(flags, ReflectionExtensions.BindingFlagsSetProperty))
        exception = (Exception) new MissingMemberException(Utils.GetResourceString(SR.NoSetProperty1, targetProcedure.AsProperty().Name));
      else
        exception = (Exception) new MissingMemberException(Utils.GetResourceString(SR.NoGetProperty1, targetProcedure.AsProperty().Name));
      return exception;
    }

    internal static Symbols.Method ResolveCall(
      Symbols.Container baseReference,
      string methodName,
      MemberInfo[] members,
      object[] arguments,
      string[] argumentNames,
      Type[] typeArguments,
      BindingFlags lookupFlags,
      bool reportErrors,
      ref OverloadResolution.ResolutionFailure failure)
    {
      failure = OverloadResolution.ResolutionFailure.None;
      Symbols.Method method;
      if (members[0].MemberType != MemberTypes.Method && members[0].MemberType != MemberTypes.Property)
      {
        failure = OverloadResolution.ResolutionFailure.InvalidTarget;
        if (reportErrors)
          throw new ArgumentException(Utils.GetResourceString(SR.ExpressionNotProcedure, methodName, baseReference.VBFriendlyName));
        method = (Symbols.Method) null;
      }
      else
      {
        int length = arguments.Length;
        object obj1 = (object) null;
        if (Symbols.HasFlag(lookupFlags, ReflectionExtensions.BindingFlagsSetProperty))
        {
          if (arguments.Length == 0)
          {
            failure = OverloadResolution.ResolutionFailure.InvalidArgument;
            if (reportErrors)
              throw new InvalidCastException(Utils.GetResourceString(SR.PropertySetMissingArgument1, methodName));
            method = (Symbols.Method) null;
            goto label_41;
          }
          else
          {
            object[] objArray = arguments;
            arguments = new object[checked (length - 2 + 1)];
            Array.Copy((Array) objArray, (Array) arguments, arguments.Length);
            obj1 = objArray[checked (length - 1)];
          }
        }
        Symbols.Method targetProcedure1 = OverloadResolution.ResolveOverloadedCall(methodName, members, arguments, argumentNames, typeArguments, lookupFlags, reportErrors, ref failure, baseReference);
        if (failure != OverloadResolution.ResolutionFailure.None)
          method = (Symbols.Method) null;
        else if (!targetProcedure1.ArgumentsValidated && !OverloadResolution.CanMatchArguments(targetProcedure1, arguments, argumentNames, typeArguments, false, (List<string>) null))
        {
          failure = OverloadResolution.ResolutionFailure.InvalidArgument;
          if (reportErrors)
          {
            string str1 = "";
            List<string> errors = new List<string>();
            OverloadResolution.CanMatchArguments(targetProcedure1, arguments, argumentNames, typeArguments, false, errors);
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
            throw new InvalidCastException(Utils.GetResourceString(SR.MatchArgumentFailure2, targetProcedure1.ToString(), str1));
          }
          method = (Symbols.Method) null;
        }
        else
        {
          if (targetProcedure1.IsProperty)
          {
            if ((object) NewLateBinding.MatchesPropertyRequirements(targetProcedure1, lookupFlags) == null)
            {
              failure = OverloadResolution.ResolutionFailure.InvalidTarget;
              if (reportErrors)
                throw NewLateBinding.ReportPropertyMismatch(targetProcedure1, lookupFlags);
              method = (Symbols.Method) null;
              goto label_41;
            }
          }
          else if (Symbols.HasFlag(lookupFlags, ReflectionExtensions.BindingFlagsSetProperty))
          {
            failure = OverloadResolution.ResolutionFailure.InvalidTarget;
            if (reportErrors)
              throw new MissingMemberException(Utils.GetResourceString(SR.MethodAssignment1, targetProcedure1.AsMethod().Name));
            method = (Symbols.Method) null;
            goto label_41;
          }
          if (Symbols.HasFlag(lookupFlags, ReflectionExtensions.BindingFlagsSetProperty))
          {
            ParameterInfo[] parameters = NewLateBinding.GetCallTarget(targetProcedure1, lookupFlags).GetParameters();
            ParameterInfo parameterInfo = parameters[checked (parameters.Length - 1)];
            Symbols.Method targetProcedure2 = targetProcedure1;
            object obj2 = obj1;
            ParameterInfo parameter1 = parameterInfo;
            bool flag1 = false;
            ref bool local1 = ref flag1;
            bool flag2 = false;
            ref bool local2 = ref flag2;
            if (!OverloadResolution.CanPassToParameter(targetProcedure2, obj2, parameter1, false, false, (List<string>) null, ref local1, ref local2))
            {
              failure = OverloadResolution.ResolutionFailure.InvalidArgument;
              if (reportErrors)
              {
                string str1 = "";
                List<string> stringList = new List<string>();
                Symbols.Method targetProcedure3 = targetProcedure1;
                object obj3 = obj1;
                ParameterInfo parameter2 = parameterInfo;
                List<string> errors = stringList;
                flag2 = false;
                ref bool local3 = ref flag2;
                flag1 = false;
                ref bool local4 = ref flag1;
                OverloadResolution.CanPassToParameter(targetProcedure3, obj3, parameter2, false, false, errors, ref local3, ref local4);
                try
                {
                  foreach (string str2 in stringList)
                    str1 = str1 + "\r\n    " + str2;
                }
                finally
                {
                  List<string>.Enumerator enumerator;
                  enumerator.Dispose();
                }
                throw new InvalidCastException(Utils.GetResourceString(SR.MatchArgumentFailure2, targetProcedure1.ToString(), str1));
              }
              method = (Symbols.Method) null;
              goto label_41;
            }
          }
          method = targetProcedure1;
        }
      }
label_41:
      return method;
    }

    internal static MethodBase GetCallTarget(
      Symbols.Method targetProcedure,
      BindingFlags flags)
    {
      return !targetProcedure.IsMethod ? (!targetProcedure.IsProperty ? (MethodBase) null : (MethodBase) NewLateBinding.MatchesPropertyRequirements(targetProcedure, flags)) : targetProcedure.AsMethod();
    }

    internal static object[] ConstructCallArguments(
      Symbols.Method targetProcedure,
      object[] arguments,
      BindingFlags lookupFlags)
    {
      ParameterInfo[] parameters = NewLateBinding.GetCallTarget(targetProcedure, lookupFlags).GetParameters();
      object[] matchedArguments = new object[checked (parameters.Length - 1 + 1)];
      int length = arguments.Length;
      object obj = (object) null;
      if (Symbols.HasFlag(lookupFlags, ReflectionExtensions.BindingFlagsSetProperty))
      {
        object[] objArray = arguments;
        arguments = new object[checked (length - 2 + 1)];
        Array.Copy((Array) objArray, (Array) arguments, arguments.Length);
        obj = objArray[checked (length - 1)];
      }
      OverloadResolution.MatchArguments(targetProcedure, arguments, matchedArguments);
      if (Symbols.HasFlag(lookupFlags, ReflectionExtensions.BindingFlagsSetProperty))
      {
        ParameterInfo parameter = parameters[checked (parameters.Length - 1)];
        matchedArguments[checked (parameters.Length - 1)] = OverloadResolution.PassToParameter(obj, parameter, parameter.ParameterType);
      }
      return matchedArguments;
    }
  }
}
