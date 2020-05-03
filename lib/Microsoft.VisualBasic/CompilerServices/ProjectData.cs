// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.ProjectData
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.VisualBasic.CompilerServices
{
  /// <summary>Provides helpers for the Visual Basic <see langword="Err" /> object.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  public sealed class ProjectData
  {
    internal int m_rndSeed;
    [ThreadStatic]
    private static ProjectData m_oProject;

    private ProjectData()
    {
      this.m_rndSeed = 327680;
    }

    /// <summary>The Visual Basic compiler uses this helper method to capture exceptions in the <see langword="Err" /> object.</summary>
    /// <param name="ex">The <see cref="T:System.Exception" /> object to be caught.</param>
    public static void SetProjectError(Exception ex)
    {
    }

    /// <summary>The Visual Basic compiler uses this helper method to capture exceptions in the <see langword="Err" /> object.</summary>
    /// <param name="ex">The <see cref="T:System.Exception" /> object to be caught.</param>
    /// <param name="lErl">The line number of the exception.</param>
    public static void SetProjectError(Exception ex, int lErl)
    {
    }

    /// <summary>Performs the work for the <see langword="Clear" /> method of the <see langword="Err" /> object. A helper method.</summary>
    public static void ClearProjectError()
    {
    }

    internal static ProjectData GetProjectData()
    {
      ProjectData projectData = ProjectData.m_oProject;
      if (projectData == null)
      {
        projectData = new ProjectData();
        ProjectData.m_oProject = projectData;
      }
      return projectData;
    }
  }
}
