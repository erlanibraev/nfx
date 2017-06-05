
using System;
using NFX.ApplicationModel.Pile;
using NFX.Log;
using NFX.Utils;
using NUnit.Framework;

namespace NFX.NUnit.Utils
{
  [TestFixture]
  public class LinkedListTest001
  {

    private readonly DefaultPile m_Pile = new DefaultPile(){AllocMode = AllocationMode.ReuseSpace, SegmentSize = 256 * 1025 * 1024};

    [SetUp]
    public void setUp()
    {
      m_Pile.Start();
    }

    [TearDown]
    public void tearDown()
    {
      m_Pile.WaitForCompleteStop();
    }
   
    [Test]
    public void emptyTest()
    {
      Assert.IsTrue(true);
    }
    [Test]
    public void createLinkedListNode()
    {
      var test = new LinkedListNode<int>(m_Pile, 11);
            
      Console.WriteLine(test.Value);
      Assert.NotNull(test);
      Assert.True(test.Value == 11);
    }
  }
}