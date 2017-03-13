using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MessageSystem.Interface
{
    public interface IMessageSender
    {
        void SendMessage(IMessage rMessage);
        void SendMessage<T>(IMessage<T> rMessage);
        void SendMessage<T,S>(IMessage<T,S> rMessage);
    }
}