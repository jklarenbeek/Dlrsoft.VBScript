// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.CacheSet`1
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System.Collections.Generic;

namespace Microsoft.VisualBasic.CompilerServices
{
  internal sealed class CacheSet<T>
  {
    private readonly Dictionary<T, LinkedListNode<T>> _dict;
    private readonly LinkedList<T> _list;
    private readonly int _maxSize;

    internal CacheSet(int maxSize)
    {
      this._dict = new Dictionary<T, LinkedListNode<T>>();
      this._list = new LinkedList<T>();
      this._maxSize = maxSize;
    }

    internal T GetExistingOrAdd(T key)
    {
      lock ((object) this)
      {
        LinkedListNode<T> node = (LinkedListNode<T>) null;
        if (this._dict.TryGetValue(key, out node))
        {
          if (node.Previous != null)
          {
            this._list.Remove(node);
            this._list.AddFirst(node);
          }
          return node.Value;
        }
        if (this._dict.Count == this._maxSize)
        {
          this._dict.Remove(this._list.Last.Value);
          this._list.RemoveLast();
        }
        node = new LinkedListNode<T>(key);
        this._dict.Add(key, node);
        this._list.AddFirst(node);
        return key;
      }
    }
  }
}
