using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MessageSystem.Interface;
using MessageSystem.Core;

namespace MessageSystem.Core
{
    public class MessageSender : IMessageSender
    {
        MessageManager manager;
        Dictionary<string, Delegate> NeedHandle
        {
            get { return manager.m_needHandle; }
        }
        List<IMessage> DelyMessages
        {
            get { return manager.delyMessages; }
        }

        public MessageSender(MessageManager manager)
        {
            this.manager = manager;
        }
        
        public void SendMessage(IMessage rMessage)
        {
            bool lReportMissingRecipient = true;

            // Hold the message for the delay or the next frame (< 0)
            if (rMessage.Delay > 0 || rMessage.Delay < 0)
            {
                if (!DelyMessages.Contains(rMessage))
                {
                    DelyMessages.Add(rMessage);
                }

                lReportMissingRecipient = false;
            }
            // Send the message now if there are handlers
            else if (NeedHandle.ContainsKey(rMessage.Key))
            {
                var dataT0 = rMessage.GetType().GetProperty("Data");
                var dataT = rMessage.GetType().GetProperty("DataT");
                var dataS = rMessage.GetType().GetProperty("DataS");

                if (dataT0 != null)
                {
                    var data = dataT0.GetValue(rMessage, null);

                    NeedHandle[rMessage.Key].DynamicInvoke(data);
                }
                else if (dataT != null && dataS != null)
                {
                    var datat = dataT.GetValue(rMessage, null);
                    var datas = dataS.GetValue(rMessage, null);

                    NeedHandle[rMessage.Key].DynamicInvoke(datat, datas);
                }
                else
                {
                    NeedHandle[rMessage.Key].DynamicInvoke();
                }
                lReportMissingRecipient = false;
            }

            // If we were unable to send the message, we may need to report it
            if (lReportMissingRecipient)
            {
                if (manager.MessageNotHandled == null)
                {
                    Debug.LogWarning("MessageDispatcher: Unhandled Message of type " + rMessage.Key);
                }
                else
                {
                    manager.MessageNotHandled(rMessage);
                }
            }
        }
    }
}
