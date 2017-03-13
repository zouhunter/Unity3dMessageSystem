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
using Candlelight;
namespace MessageSystem
{
    public class MessageManager
    {
        #region
        protected static MessageManager instance = default(MessageManager);
        private static object lockHelper = new object();
        private static bool isQuit = false;

        public static MessageManager GetInstance()
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null && !isQuit)
                    {
                        instance = new MessageManager();
                    }
                }
            }
            return instance;
        }

        #endregion
        MessageSender sender;
        MessageReceiver receiver;
        MessageHolder holder;
        public MessageManager()
        {
            holder = new MessageHolder();
            sender = new MessageSender(holder);
            receiver = new MessageReceiver(holder);
        }

        public void AddListener(string key, UnityAction handle)
        {
            receiver.AddDelegate(key, handle);
        }
        public void RemoveListener(string key, UnityAction handle)
        {
            receiver.RemoveDelegate(key, handle);
        }
        public void AddListener<T>(string key,UnityAction<T> handle)
        {
            receiver.AddDelegate(key, handle);
        }
        public void RemoveListener<T>(string key,UnityAction<T> handle)
        {
            receiver.RemoveDelegate(key,handle);
        }
        public void AddListener<T,S>(string key, UnityAction<T,S> handle)
        {
            receiver.AddDelegate(key, handle);
        }
        public void RemoveAllListener(string key)
        {
            receiver.RemoveAllDelegate(key);
        }

        public void NotifyObserver(string key)
        {
            Message message = Message.Allocate(key);
            sender.SendMessage(message);
            message.Release();
        }
        public void NotifyObserver<T>(string key,T data)
        {
            Message<T> message = Message<T>.Allocate(key,data);
            sender.SendMessage(message);
            message.Release();
        }
        public void NotifyObserver<T,S>(string key, T datat, S datas)
        {
            Message<T,S> message = Message<T,S>.Allocate(key, datat,datas);
            sender.SendMessage(message);
            message.Release();
        }
        public void NotifyObserver(string key, MonoBehaviour handle, float dely)
        {
            handle.StartCoroutine(DelyNoti(key,dely));
        }
        public void NotifyObserver<T>(string key,T data, MonoBehaviour handle, float dely)
        {
            handle.StartCoroutine(DelyNoti(key,data,dely));
        }
        public void NotifyObserver<T,S>(string key, T datat,S datas, MonoBehaviour handle, float dely)
        {
            handle.StartCoroutine(DelyNoti(key, datat,datas, dely));
        }

        IEnumerator DelyNoti(string key, float dely)
        {
            yield return new WaitForSeconds(dely);
            NotifyObserver(key);
        }
        IEnumerator DelyNoti<T>(string key,T data,float dely)
        {
            yield return new WaitForSeconds(dely);
            NotifyObserver(key, data);
        }
        IEnumerator DelyNoti<T,S>(string key, T data,S datas, float dely)
        {
            yield return new WaitForSeconds(dely);
            NotifyObserver(key, data,datas);
        }
    }
}