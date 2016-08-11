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
        /// <summary>
        /// Send the message object as needed. In this instance, the caller needs to
        /// release the message.
        /// </summary>
        /// <param name="rMessage"></param>
        public void SendMessage<T>(IMessage<T> rMessage)
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
                NeedHandle[rMessage.Key].DynamicInvoke(rMessage.Data);
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
                var property = rMessage.GetType().GetProperty("Data");
                if (property != null)
                {
                    var data = property.GetValue(rMessage, null);
                    try
                    {
                        NeedHandle[rMessage.Key].DynamicInvoke(data);
                    }
                    catch
                    {
                        Debug.LogWarning(rMessage.Key + "参数类型不符");
                    }
                }
                else
                {
                    try
                    {
                        NeedHandle[rMessage.Key].DynamicInvoke();
                    }
                    catch
                    {
                        Debug.LogWarning(rMessage.Key + "缺少参数");
                    }
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
