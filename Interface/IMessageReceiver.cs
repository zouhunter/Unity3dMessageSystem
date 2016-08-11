using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MessageSystem.Interface
{
    public interface IMessageReceiver
    {
        void AddListener<T>(string key, UnityAction<T> handle);
        void RemoveListener<T>(string key,UnityAction<T> handle);
        void RemoveAllListener(string key);
    }
}