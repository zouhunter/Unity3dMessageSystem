using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MessageSystem.Interface;
using MessageSystem.Core;
using System.Reflection;

namespace MessageSystem
{

    public class MessageManager:Singleton<MessageManager>
    {
        public MessageSender sender;
        public MessageReceiver receiver;
        public MessageManager()
        {
            sender = new MessageSender(this);
            receiver = new MessageReceiver(this);
        }

        public System.Action<IMessage> MessageNotHandled;

        public List<IMessage> delyMessages = new List<IMessage>();

        public Dictionary<string, Delegate> m_needHandle = new Dictionary<string, Delegate>();

        void Update()
        {
            for (int i = 0; i < delyMessages.Count; i++)
            {
                IMessage iMessage = delyMessages[i];

                // Reduce the delay
                iMessage.Delay -= Time.deltaTime;

                if (iMessage.Delay < 0)
                {
                    iMessage.Delay = 0;
                }

                // If it's time, send the message and flag for removal
                if (!iMessage.IsSent && iMessage.Delay == 0)
                {
                    sender.SendMessage(iMessage);
                    iMessage.IsSent = true;
                }
            }

            // Remove sent messages
            for (int i = delyMessages.Count - 1; i >= 0; i--)
            {
                IMessage iMessage = delyMessages[i];

                if (iMessage.IsSent)
                {
                    delyMessages.RemoveAt(i);
                    iMessage.Release();
                }
            }
        }

        public void AddListener(string key, UnityAction handle)
        {
            receiver.AddListener(key, handle);
        }
        public void AddListener<T>(string key,UnityAction<T> handle)
        {
            receiver.AddListener(key, handle);
        }
        public void RemoveListener<T>(string key,UnityAction<T> handle)
        {
            receiver.RemoveListener<T>(key,handle);
        }
        public void RemoveListener(string key, UnityAction handle)
        {
            receiver.RemoveListener(key, handle);
        }
        public void RemoveAllListener(string key)
        {
            receiver.RemoveAllListener(key);
        }
        public void NotifyObserver<T>(string key,T data,float delay = 0)
        {
            Message<T> message = Message<T>.Allocate(key,data,delay);

            if (message.Delay == 0)
            {
                sender.SendMessage(message);
                message.Release();
            }
            else if (message.Delay < 0 || message.Delay > 0)
            {
                delyMessages.Add(message);
            }
        }
        public void NotifyObserver(string key,float delay = 0)
        {
            Message message = Message.Allocate(key, delay);

            if (message.Delay == 0)
            {
                sender.SendMessage(message);
                message.Release();
            }
            else if (message.Delay < 0 || message.Delay > 0)
            {
                delyMessages.Add(message);
            }
        }
    }
}