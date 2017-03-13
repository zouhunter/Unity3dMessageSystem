using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MessageSystem.Interface;
using Collections;

namespace MessageSystem.Core
{
    public class Message<T,S> : IMessage<T,S>
    {
        protected string m_Key;
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        protected T mDataT;
        public T DataT
        {
            get { return mDataT; }
            set { mDataT = value; }
        }
        protected S mDataS;
        public S DataS
        {
            get { return mDataS; }
            set { mDataS = value; }
        }
        private static ObjectPool<Message<T,S>> sPool = new ObjectPool<Message<T,S>>(1, 1);
        public static Message<T,S> Allocate(string key, T datat,S datas)
        {
            Message<T,S> lInstance = sPool.Allocate();
            lInstance.Key = key;
            lInstance.DataT = datat;
            lInstance.DataS = datas;
            if (lInstance == null) { lInstance = new Message<T,S>(); }
            return lInstance;
        }
        public void Release()
        {
            if (this == null) { return; }
            if (this is Message<T,S>)
            {
                sPool.Release((Message<T,S>)this);
            }
        }
    }
    public class Message<T> :IMessage<T>
    {
        protected string m_Key;
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        protected T mData;
        public T Data
        {
            get { return mData; }
            set { mData = value; }
        }

        private static ObjectPool<Message<T>> sPool = new ObjectPool<Message<T>>(1, 1);

        public static Message<T> Allocate(string key,T data)
        {
            Message<T> lInstance = sPool.Allocate();

            lInstance.Key = key;
            lInstance.Data = data;
            if (lInstance == null) { lInstance = new Message<T>(); }
            return lInstance;
        }

        public static void Release(Message<T> rInstance)
        {
            if (rInstance == null) { return; }
            sPool.Release(rInstance);
        }
        public void Release()
        {
            if (this == null) { return; }
            if (this is Message<T>)
            {
                sPool.Release((Message<T>)this);
            }
        }
    }
    public class Message : IMessage
    {
        protected string m_Key;
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }
        public void Release()
        {
            if (this == null) { return; }
            if (this is Message)
            {
                sPool.Release((Message)this);
            }
        }

        protected static ObjectPool<Message> sPool = new ObjectPool<Message>(1, 1);
        public static Message Allocate(string key)
        {
            Message lInstance = sPool.Allocate();
            lInstance.Key = key;
            if (lInstance == null) { lInstance = new Message(); }
            return lInstance;
        }
        public static void Release(Message rInstance)
        {
            if (rInstance == null) { return; }
            sPool.Release(rInstance);
        }
    }
}
