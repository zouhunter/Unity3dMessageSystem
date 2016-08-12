using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MessageSystem.Interface;

namespace MessageSystem.Core
{
    public class MessageReceiver: IMessageReceiver
    {
        MessageManager manager;
        Dictionary<string, Delegate> NeedHandle
        {
            get { return manager.m_needHandle; }
        }
        public MessageReceiver(MessageManager manager)
        {
            this.manager = manager;
        }
        public void AddDelegate(string key, Delegate handle)
        {
            // First check if we know about the message type
            if (!NeedHandle.ContainsKey(key))
            {
                NeedHandle.Add(key, handle);
            }
            else
            {
                NeedHandle[key] = Delegate.Combine(NeedHandle[key], handle);
            }
        }
        public void RemoveDelegate(string key, Delegate handle)
        {
            if (NeedHandle.ContainsKey(key))
            {
                NeedHandle[key] = Delegate.Remove(NeedHandle[key], handle);
                if(NeedHandle[key] == null)
                {
                    NeedHandle.Remove(key);
                }
            }
        }
        public void RemoveAllDelegate(string key)
        {
            if (NeedHandle.ContainsKey(key))
            {
                NeedHandle.Remove(key);
            }
        }
    } 
}
