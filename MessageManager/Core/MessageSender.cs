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
        MessageHolder holder;
        Dictionary<string, Delegate> NeedHandle
        {
            get { return holder.m_needHandle; }
        }

        public MessageSender(MessageHolder holder)
        {
            this.holder = holder;
        }
        public void SendMessage(IMessage rMessage)
        {
            if (NeedHandle.ContainsKey(rMessage.Key))
            {
                NeedHandle[rMessage.Key].DynamicInvoke();
            }
            else
            {
                if (holder.MessageNotHandled == null)
                {
                    Debug.LogWarning("MessageDispatcher: Unhandled Message of type " + rMessage.Key);
                }
                else
                {
                    holder.MessageNotHandled(rMessage);
                }
            }
        }
        public void SendMessage<T>(IMessage<T> rMessage)
        {
            if (NeedHandle.ContainsKey(rMessage.Key))
            {
                var dataT0 = rMessage.GetType().GetProperty("Data");
                var data = dataT0.GetValue(rMessage, null);
                NeedHandle[rMessage.Key].DynamicInvoke(data);
            }
            else
            {
                if (holder.MessageNotHandled == null)
                {
                    Debug.LogWarning("MessageDispatcher: Unhandled Message of type " + rMessage.Key);
                }
                else
                {
                    holder.MessageNotHandled(rMessage);
                }
            }
        }
        public void SendMessage<T,S>(IMessage<T,S> rMessage)
        {
            if (NeedHandle.ContainsKey(rMessage.Key))
            {
                var dataT = rMessage.GetType().GetProperty("DataT");
                var dataS = rMessage.GetType().GetProperty("DataS");
                var datat = dataT.GetValue(rMessage, null);
                var datas = dataS.GetValue(rMessage, null);
                NeedHandle[rMessage.Key].DynamicInvoke(datat,datas);
            }
            else
            {
                if (holder.MessageNotHandled == null)
                {
                    Debug.LogWarning("MessageDispatcher: Unhandled Message of type " + rMessage.Key);
                }
                else
                {
                    holder.MessageNotHandled(rMessage);
                }
            }
        }
    }
}
