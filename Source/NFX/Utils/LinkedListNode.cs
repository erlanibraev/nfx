using NFX.ApplicationModel.Pile;

namespace NFX.Utils
{
  public class LinkedListNode<T>
  {
    
    protected internal PilePointer m_pp_next { get; set; } = PilePointer.Invalid;
    protected internal PilePointer m_pp_prev { get; set; } = PilePointer.Invalid;
    protected internal PilePointer m_pp_self { get; set; } = PilePointer.Invalid;
    protected internal PilePointer m_pp_value { get; set; } = PilePointer.Invalid;
    protected internal PilePointer m_pp_list { get; set; } = PilePointer.Invalid;

    protected internal IPile Pile { get; set; }

    public LinkedListNode(IPile pile, T value)
    {
      Pile = pile;
      Value = value;
      save(this);
    }

    public LinkedListNode(IPile pile, LinkedList<T> linkedList, T value)
    {
      Pile = pile;
      m_pp_list = linkedList.m_pp_self;
      Value = value;
      save(this);
    }

    public LinkedList<T> List => m_pp_list != PilePointer.Invalid ? (LinkedList<T>) Pile.Get(m_pp_list) : null;

    public LinkedListNode<T> Next => m_pp_next != PilePointer.Invalid ? (LinkedListNode<T>) Pile.Get(m_pp_next) : null;
    
    public LinkedListNode<T> Previous => m_pp_prev != PilePointer.Invalid ? (LinkedListNode<T>) Pile.Get(m_pp_prev) : null;

    public T Value
    {
      get => m_pp_value !=  PilePointer.Invalid ? (T) Pile.Get(m_pp_value) : default(T);
      set 
      {
        if (m_pp_value != PilePointer.Invalid)
        {
          Pile.Put(m_pp_value, value);
        }
        else
        {
          m_pp_value = Pile.Put(value);
        }
      }
    }

    protected internal PilePointer save(LinkedListNode<T> data)
    {
      PilePointer result = data.m_pp_self;
      if (result != PilePointer.Invalid)
      {
        Pile.Put(result, data);
      }
      else
      {
        result = Pile.Put(data);
      }
      return result;
    }

  }
}