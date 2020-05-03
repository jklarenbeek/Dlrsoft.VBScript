// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.MyGroupCollectionAttribute
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic
{
  /// <summary>This attribute supports <see langword="My.Forms" /> and <see langword="My.WebServices" /> in Visual Basic.</summary>
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class MyGroupCollectionAttribute : Attribute
  {
    /// <summary>This attribute supports <see langword="My.Forms" /> and <see langword="My.WebServices" /> in Visual Basic.</summary>
    /// <param name="typeToCollect">
    /// <see langword="String" />. Initializes the <see cref="P:Microsoft.VisualBasic.MyGroupCollectionAttribute.MyGroupName" /> property. The compiler generates accessor properties for classes that derive from this type.</param>
    /// <param name="createInstanceMethodName">
    /// <see langword="String" />. Initializes the <see cref="P:Microsoft.VisualBasic.MyGroupCollectionAttribute.CreateMethod" /> property. Specifies the method in the class that creates the type's instances.</param>
    /// <param name="disposeInstanceMethodName">
    /// <see langword="String" />. Initializes the <see cref="P:Microsoft.VisualBasic.MyGroupCollectionAttribute.DisposeMethod" /> property. Specifies the method in the class that disposes of the type's instances.</param>
    /// <param name="defaultInstanceAlias">
    /// <see langword="String" />. Initializes the <see cref="P:Microsoft.VisualBasic.MyGroupCollectionAttribute.DefaultInstanceAlias" /> property. Specifies the name of the property that returns the default instance of the class.</param>
    public MyGroupCollectionAttribute(
      string typeToCollect,
      string createInstanceMethodName,
      string disposeInstanceMethodName,
      string defaultInstanceAlias)
    {
      this.MyGroupName = typeToCollect;
      this.CreateMethod = createInstanceMethodName;
      this.DisposeMethod = disposeInstanceMethodName;
      this.DefaultInstanceAlias = defaultInstanceAlias;
    }

    /// <summary>This property supports <see langword="My" /> in Visual Basic.</summary>
    /// <returns>Specifies the name of the type for which the compiler generates accessor properties.</returns>
    public string MyGroupName { get; }

    /// <summary>This property supports <see langword="My" /> in Visual Basic.</summary>
    /// <returns>Specifies the method in the class that creates the type's instances.</returns>
    public string CreateMethod { get; }

    /// <summary>This property supports <see langword="My" /> in Visual Basic.</summary>
    /// <returns>Specifies the method in the class that disposes of the type's instances.</returns>
    public string DisposeMethod { get; }

    /// <summary>This property supports <see langword="My" /> in Visual Basic.</summary>
    /// <returns>Specifies the name of the property that returns the default instance of the class.</returns>
    public string DefaultInstanceAlias { get; }
  }
}
