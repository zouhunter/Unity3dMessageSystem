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
        void AddListener(string key, Delegate handle);
        void RemoveListener(string key);
    }
}