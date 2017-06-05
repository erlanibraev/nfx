using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NFX.ApplicationModel.Pile;

namespace NFX.Utils
{
  public class LinkedList<T> :  ICollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
  {
    internal LinkedListNode<T> head;
    
    protected internal PilePointer m_pp_self { get; set; } = PilePointer.Invalid;
    
    public IEnumerator<T> GetEnumerator()
    {
      throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(T item)
    {
      throw new NotImplementedException();
    }

    public void Clear()
    {
      throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(T item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Array array, int index)
    {
      throw new NotImplementedException();
    }

    int ICollection.Count
    {
      get { throw new NotImplementedException(); }
    }

    public object SyncRoot { get; }
    public bool IsSynchronized { get; }

    int ICollection<T>.Count
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsReadOnly { get; }

    int IReadOnlyCollection<T>.Count
    {
      get { throw new NotImplementedException(); }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      throw new NotImplementedException();
    }

    public void OnDeserialization(object sender)
    {
      throw new NotImplementedException();
    }
    
    protected internal static PilePointer save(LinkedList<T> data)
    {
      PilePointer result = data.m_pp_self;
      if (result != PilePointer.Invalid)
      {
        App.Pile.Put(result, data);
      }
      else
      {
        result = App.Pile.Put(data);
      }
      return result;
    }
  }
}