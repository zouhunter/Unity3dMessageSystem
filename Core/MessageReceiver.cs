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
        public void AddListener(string key,UnityAction handle)
        {
            AddDelegate(key, handle);
        }
        public void AddListener<T>(string key, UnityAction<T> handle)
        {
            AddDelegate(key, handle);
        }
        private void AddDelegate(string key, Delegate handle)
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
        public void RemoveListener<T>(string key, UnityAction<T> handle)
        {
            RemoveDelegate(key, handle);
        }
        public void RemoveListener(string key,UnityAction handle)
        {
            RemoveDelegate(key, handle);
        }
        private void RemoveDelegate(string key, Delegate handle)
        {
            if (NeedHandle.ContainsKey(key))
            {
                Delegate.Remove(NeedHandle[key], handle);
            }
        }
        public void RemoveAllListener(string key)
        {
            if (NeedHandle.ContainsKey(key))
            {
                NeedHandle.Remove(key);
            }
        }
    } 
}
