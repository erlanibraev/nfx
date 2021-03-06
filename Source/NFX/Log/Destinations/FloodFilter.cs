/*<FILE_LICENSE>
* NFX (.NET Framework Extension) Unistack Library
* Copyright 2003-2017 ITAdapter Corp. Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
</FILE_LICENSE>*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX.Environment;

namespace NFX.Log.Destinations
{
  /// <summary>
  /// Implements a destination group that stops message flood
  /// </summary>
  public class FloodFilter : CompositeDestination
  {
    #region consts

      public const int DEFAULT_MAX_TEXT_LENGTH = 0;

      public const int DEFAULT_MAX_COUNT = 25;
      public const int MAX_MAX_COUNT = 1000;
      public const int DEFAULT_INTERVAL_SEC = 10;

    #endregion

    #region .ctor

      /// <summary>
      /// Creates a filter that prevents message flood
      /// </summary>
      public FloodFilter() : base()
      {
      }

      /// <summary>
      /// Creates a filter that prevents message flood
      /// </summary>
      public FloodFilter(params Destination[] inner) : base(inner)
      {
      }

      /// <summary>
      /// Creates a filter that prevents message flood
      /// </summary>
      public FloodFilter(string name, params Destination[] inner) : base (name, inner)
      {

      }

      protected override void Destructor()
      {
        base.Destructor();
      }

    #endregion


    #region Pvt/Protected Fields

      private int m_IntervalSec = DEFAULT_INTERVAL_SEC;

      private int m_MaxCount = DEFAULT_MAX_COUNT;
      private int m_MaxTextLength = DEFAULT_MAX_TEXT_LENGTH;
      private MessageList m_List = new MessageList();
      private DateTime m_LastFlush;

      private int m_Count;

      private MessageType m_MessageType;
      private string m_MessageTopic;
      private string m_MessageFrom;
      private int m_MessageSource;

    #endregion


    #region Properties

      [Config]
      public int IntervalSec
      {
        get { return m_IntervalSec; }
        set
        {
          if (value<1) value = 1;
          m_IntervalSec = value;
        }
      }

      /// <summary>
      /// Sets how many messages may be batched per interval. If more messages arrive then their data is not going to be logged
      /// </summary>
      [Config]
      public int MaxCount
      {
        get { return m_MaxCount; }
        set
        {
          if (value<0) value = 0;
          if (value>MAX_MAX_COUNT) value = MAX_MAX_COUNT;
          m_MaxCount = value;
        }
      }

      /// <summary>
      /// Imposes a limit in character length of combined message test
      /// </summary>
      [Config]
      public int MaxTextLength
      {
        get { return m_MaxTextLength; }
        set
        {
          if (value<0) value = 0;
          m_MaxTextLength = value;
        }
      }

      /// <summary>
      /// Determines the message type for message emitted when flood is detected
      /// </summary>
      [Config]
      public MessageType MessageType
      {
        get { return m_MessageType;}
        set { m_MessageType = value; }
      }

      /// <summary>
      /// Determines the message topic for message emitted when flood is detected
      /// </summary>
      [Config]
      public string MessageTopic
      {
        get { return m_MessageTopic;}
        set { m_MessageTopic = value; }
      }


      /// <summary>
      /// Determines the message from for message emitted when flood is detected
      /// </summary>
      [Config]
      public string MessageFrom
      {
        get { return m_MessageFrom;}
        set { m_MessageFrom = value; }
      }

      /// <summary>
      /// Determines the message topic for message emitted when flood is detected
      /// </summary>
      [Config]
      public int MessageSource
      {
        get { return m_MessageSource;}
        set { m_MessageSource = value; }
      }


    #endregion

    #region Public
      public override void Open()
      {
          m_LastFlush = Service.Now;
          m_Count = 0;
          base.Open();
      }

      public override void Close()
      {
          flush();
          base.Close();
      }
    #endregion


    #region Protected /.pvt

      protected internal override void DoSend(Message entry)
      {
          if (m_List.Count<m_MaxCount)
            m_List.Add(entry);

          m_Count++;
      }


      protected internal override void DoPulse()
      {
        if ((Service.Now - m_LastFlush).TotalSeconds > m_IntervalSec)
          flush();
      }



      private void flush()
      {
        try
        {
          if (m_Count==0) return;

          Message msg = null;

          if (m_List.Count==1)
            msg = m_List[0];
          else
          {
            msg = new Message();

            msg.Type = m_MessageType;
            msg.Topic = m_MessageTopic;
            msg.From = m_MessageFrom;
            msg.Source = m_MessageSource;

            if (msg.Topic.IsNullOrWhiteSpace()) msg.Topic = CoreConsts.LOG_TOPIC;
            if (msg.From.IsNullOrWhiteSpace()) msg.From = "FloodFilter";

            var txt = new StringBuilder();
            foreach(var m in m_List)
            {
              txt.Append("--> ");
              txt.AppendLine( m.ToString() );
              txt.AppendLine();
              txt.AppendLine();

              if (m_MaxTextLength>0 && txt.Length>m_MaxTextLength) break;
            }

            var txtl = txt.ToString();

            if (m_MaxTextLength>0)
              if (txtl.Length > m_MaxTextLength)
              {
               txtl = txtl.Substring(0, m_MaxTextLength) + " ...... " + "truncated at {0} chars".Args(m_MaxTextLength);
              }

            msg.Text = "{0} log msgs, {1} combined:\r\n\n{2}".Args(m_Count, m_List.Count, txtl);
          }

          m_List.Clear();
          m_Count = 0;

          base.DoSend(msg);
        }
        finally
        {
          m_LastFlush = Service.Now;
        }
      }


    #endregion

  }
}
