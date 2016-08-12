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
        void AddDelegate(string key, Delegate handle);
        void RemoveDelegate(string key, Delegate handle);
        void RemoveAllDelegate(string key);
    }
}