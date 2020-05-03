// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.ForEachEnum
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Collections;
using System.ComponentModel;

namespace Microsoft.VisualBasic
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  internal sealed class ForEachEnum : IEnumerator, IDisposable
  {
    private bool mDisposed;
    private Collection mCollectionObject;
    private Collection.Node mCurrent;
    private Collection.Node mNext;
    private bool mAtBeginning;
    internal WeakReference WeakRef;

    void IDisposable.Dispose()
    {
      if (!this.mDisposed)
      {
        this.mCollectionObject.RemoveIterator(this.WeakRef);
        this.mDisposed = true;
      }
      this.mCurrent = (Collection.Node) null;
      this.mNext = (Collection.Node) null;
    }

    public ForEachEnum(Collection coll)
    {
      this.mDisposed = false;
      this.mCollectionObject = coll;
      this.Reset();
    }

    public bool MoveNext()
    {
      bool flag;
      if (this.mDisposed)
      {
        flag = false;
      }
      else
      {
        if (this.mAtBeginning)
        {
          this.mAtBeginning = false;
          this.mNext = this.mCollectionObject.GetFirstListNode();
        }
        if (this.mNext == null)
        {
          this.Dispose();
          flag = false;
        }
        else
        {
          this.mCurrent = this.mNext;
          if (this.mCurrent != null)
          {
            this.mNext = this.mCurrent.m_Next;
            flag = true;
          }
          else
          {
            this.Dispose();
            flag = false;
          }
        }
      }
      return flag;
    }

    public void Reset()
    {
      if (this.mDisposed)
      {
        this.mCollectionObject.AddIterator(this.WeakRef);
        this.mDisposed = false;
      }
      this.mCurrent = (Collection.Node) null;
      this.mNext = (Collection.Node) null;
      this.mAtBeginning = true;
    }

    public object Current
    {
      get
      {
        return this.mCurrent != null ? this.mCurrent.m_Value : (object) null;
      }
    }

    public void Adjust(Collection.Node Node, ForEachEnum.AdjustIndexType Type)
    {
      if (Node == null || this.mDisposed)
        return;
      switch (Type)
      {
        case ForEachEnum.AdjustIndexType.Insert:
          if (this.mCurrent == null || Node != this.mCurrent.m_Next)
            break;
          this.mNext = Node;
          break;
        case ForEachEnum.AdjustIndexType.Remove:
          if (Node == this.mCurrent || Node != this.mNext)
            break;
          this.mNext = this.mNext.m_Next;
          break;
      }
    }

    internal void AdjustOnListCleared()
    {
      this.mNext = (Collection.Node) null;
    }

    internal enum AdjustIndexType
    {
      Insert,
      Remove,
    }
  }
}
