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
    public class Message<T> : IMessage<T>
    {
        protected string m_Key;
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        protected float mDelay = 0;
        public float Delay
        {
            get { return mDelay; }
            set { mDelay = value; }
        }

        protected T mData;
        public T Data
        {
            get { return mData; }
            set { mData = value; }
        }

        protected bool mIsSent = false;
        public bool IsSent
        {
            get { return mIsSent; }
            set { mIsSent = value; }
        }

        protected bool mIsHandled = false;
        public bool IsHandled
        {
            get { return mIsHandled; }
            set { mIsHandled = value; }
        }

        /// <summary>
        /// Clear this instance.
        /// </summary>
        public virtual void Clear()
        {
            m_Key = null;
            mIsSent = false;
            Data = default(T);
            mIsHandled = false;
            mDelay = 0.0f;
        }

        // ******************************** OBJECT POOL ********************************

        /// <summary>
        /// Allows us to reuse objects without having to reallocate them over and over
        /// </summary>
        private static ObjectPool<Message<T>> sPool = new ObjectPool<Message<T>>(1, 1);

        //public static int Length { get { return sPool.Length; } }
        /// <summary>
        /// Pulls an object from the pool.
        /// </summary>
        /// <returns></returns>
        public static Message<T> Allocate(string key,T data,float delay)
        {
            // Grab the next available object
            Message<T> lInstance = sPool.Allocate();

            lInstance.Key = key;
            lInstance.Data = data;
            lInstance.Delay = delay;
            
            // Reset the sent flags. We do this so messages are flagged as 'completed'
            // by default.
            lInstance.IsSent = false;
            lInstance.IsHandled = false;

            // For this type, guarentee we have something
            // to hand back tot he caller
            if (lInstance == null) { lInstance = new Message<T>(); }
            return lInstance;
        }

        /// <summary>
        /// Returns an element back to the pool.
        /// </summary>
        /// <param name="rEdge"></param>
        public static void Release(Message<T> rInstance)
        {
            if (rInstance == null) { return; }

            // Reset the sent flags. We do this so messages are flagged as 'completed'
            // and removed by default.
            rInstance.IsSent = true;
            rInstance.IsHandled = true;

            // Make it available to others.
            sPool.Release(rInstance);
        }

        /// <summary>
        /// Returns an element back to the pool.
        /// </summary>
        /// <param name="rEdge"></param>
        public void Release()
        {
            if (this == null) { return; }

            // We should never release an instance unless we're
            // sure we're done with it. So clearing here is fine
            Clear();

            // Reset the sent flags. We do this so messages are flagged as 'completed'
            // and removed by default.
            IsSent = true;
            IsHandled = true;

            // Make it available to others.
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

        protected float mDelay = 0;
        public float Delay
        {
            get { return mDelay; }
            set { mDelay = value; }
        }

        protected bool mIsSent = false;
        public bool IsSent
        {
            get { return mIsSent; }
            set { mIsSent = value; }
        }

        protected bool mIsHandled = false;
        public bool IsHandled
        {
            get { return mIsHandled; }
            set { mIsHandled = value; }
        }

        /// <summary>
        /// Clear this instance.
        /// </summary>
        public virtual void Clear()
        {
            m_Key = null;
            mIsSent = false;
            mIsHandled = false;
            mDelay = 0.0f;
        }

        // ******************************** OBJECT POOL ********************************

        /// <summary>
        /// Allows us to reuse objects without having to reallocate them over and over
        /// </summary>
        private static ObjectPool<Message> sPool = new ObjectPool<Message>(1, 1);

        //public static int Length { get { return sPool.Length; } }
        /// <summary>
        /// Pulls an object from the pool.
        /// </summary>
        /// <returns></returns>
        public static Message Allocate(string key,float delay)
        {
            // Grab the next available object
            Message lInstance = sPool.Allocate();

            lInstance.Key = key;
            lInstance.Delay = delay;

            // Reset the sent flags. We do this so messages are flagged as 'completed'
            // by default.
            lInstance.IsSent = false;
            lInstance.IsHandled = false;

            // For this type, guarentee we have something
            // to hand back tot he caller
            if (lInstance == null) { lInstance = new Message(); }
            return lInstance;
        }

        /// <summary>
        /// Returns an element back to the pool.
        /// </summary>
        /// <param name="rEdge"></param>
        public static void Release(Message rInstance)
        {
            if (rInstance == null) { return; }

            // Reset the sent flags. We do this so messages are flagged as 'completed'
            // and removed by default.
            rInstance.IsSent = true;
            rInstance.IsHandled = true;

            // Make it available to others.
            sPool.Release(rInstance);
        }

        /// <summary>
        /// Returns an element back to the pool.
        /// </summary>
        /// <param name="rEdge"></param>
        public void Release()
        {
            if (this == null) { return; }

            // We should never release an instance unless we're
            // sure we're done with it. So clearing here is fine
            Clear();

            // Reset the sent flags. We do this so messages are flagged as 'completed'
            // and removed by default.
            IsSent = true;
            IsHandled = true;

            // Make it available to others.
            if (this is Message)
            {
                sPool.Release((Message)this);
            }
        }
    }
}
