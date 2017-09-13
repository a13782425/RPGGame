using RPGGame.Enums;
using RPGGame.Global;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandle
{
    private static MessageHandle _instance;

    private Dictionary<GameMessageEnum, MessageCallBackDelegate> _allMessage = new Dictionary<GameMessageEnum, MessageCallBackDelegate>();

    private Dictionary<string, MessageCallBackDelegate> _stringMessage = new Dictionary<string, MessageCallBackDelegate>();

    public static MessageHandle Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MessageHandle();
            }
            return _instance;
        }
    }

    private MessageHandle()
    {
    }

    public void RegisterMessage(GameMessageEnum type, MessageCallBackDelegate onMessage)
    {
        if (this._allMessage.ContainsKey(type))
        {
            _allMessage[type] += onMessage;
        }
        else
        {
            this._allMessage.Add(type, onMessage);
        }
    }

    public void RegisterMessage(string name, MessageCallBackDelegate onMessage)
    {
        if (this._stringMessage.ContainsKey(name))
        {
            _stringMessage[name] += onMessage;
        }
        else
        {
            this._stringMessage.Add(name, onMessage);
        }
    }

    public bool ExistMessage(GameMessageEnum type)
    {
        return this._allMessage.ContainsKey(type);
    }

    public bool ExistMessage(string name)
    {
        return this._stringMessage.ContainsKey(name);
    }

    public void ReplaceMessage(GameMessageEnum type, MessageCallBackDelegate onMessage)
    {
        if (this._allMessage.ContainsKey(type))
        {
            this._allMessage[type] = null;
            _allMessage[type] += onMessage;
        }
        else
        {
            this._allMessage.Add(type, onMessage);
        }
    }

    public void ReplaceMessage(string name, MessageCallBackDelegate onMessage)
    {
        if (this._stringMessage.ContainsKey(name))
        {
            this._stringMessage[name] = null;
            _stringMessage[name] += onMessage;
        }
        else
        {
            this._stringMessage.Add(name, onMessage);
        }
    }

    public void UnRegisterMessage(GameMessageEnum type, MessageCallBackDelegate onMessage)
    {
        if (this._allMessage.ContainsKey(type))
        {
            _allMessage[type] -= onMessage;
        }
    }

    public void UnRegisterMessage(string name, MessageCallBackDelegate onMessage)
    {
        if (this._stringMessage.ContainsKey(name))
        {
            _stringMessage[name] -= onMessage;
            //Dictionary<string, MessageCallBackDelegate> stringMessage;
            //(stringMessage = this._stringMessage)[name] = (MessageCallBackDelegate)Delegate.Remove(stringMessage[name], onMessage);
        }
    }

    public bool ExecuteMessage(GameMessageEnum type, object obj = null)
    {
        if (this._allMessage.ContainsKey(type))
        {
            if (this._allMessage[type] != null)
            {
                this._allMessage[type](obj);
                return true;
            }
        }
        Debug.LogWarning(type.ToString() + "方法执行失败！");
        return false;
    }

    public bool ExecuteMessage(string msgName, object obj = null)
    {
        if (this._stringMessage.ContainsKey(msgName))
        {
            if (this._stringMessage[msgName] != null)
            {
                this._stringMessage[msgName](obj);
                return true;
            }
        }
        Debug.LogWarning(msgName + "没有被注册");
        return false;
    }

    ~MessageHandle()
    {
        this._allMessage.Clear();
        this._allMessage = null;
        this._stringMessage.Clear();
        this._stringMessage = null;
    }
}
