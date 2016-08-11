using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MessageSystem.Interface
{
    public interface IMessage
    {
        string Key { get; set; }
        float Delay { get; set; }
        bool IsSent { get; set; }
        void Clear();
        void Release();
    }
    public interface IMessage<T>:IMessage
    {
        T Data { get; set; }
    }
}
