using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageSystem.Interface;
using System;
namespace MessageSystem.Core
{
    public class MessageHolder
    {
        public Action<IMessage> MessageNotHandled;
        public Dictionary<string, Delegate> m_needHandle = new Dictionary<string, Delegate>();
    }
}