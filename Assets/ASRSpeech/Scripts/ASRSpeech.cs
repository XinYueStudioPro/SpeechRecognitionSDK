using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICmdCallback
{
    void onCmd(string slot, string word, int conf);
    void onEvent(int evt, int code);
}

public class ASRSpeech : AndroidJavaProxy
{
    public static int EVENT_ASR_START = 1;
    public static int EVENT_ASR_STOP = 2;
    public static int EVENT_ASR_ERROR = 3;
 

    AndroidJavaObject mSpeechM = null;

    AndroidJavaObject mActivity = null;

    public ASRSpeech() : base("com.unity.speechrecognitionsdk.ICmdCallback")
    {
        AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        mActivity = player.GetStatic<AndroidJavaObject>("currentActivity");

        mSpeechM = new AndroidJavaObject("com.unity.speechrecognitionsdk.Asr", mActivity, this);
    }

    public int setCommandList(AndroidJavaObject map)
    {
        return   mSpeechM.Call<int>("setCommandList", map);
    }

    public ASRSpeech init()
    {
        mSpeechM.Call<int>("init");
        return this;
    }

    public ASRSpeech startRecognize()
    {
        mSpeechM.Call<int>("startRecognize", false);
        return this;
    }

    public ASRSpeech stopRecognize()
    {
        mSpeechM.Call("stopRecognize");
        return this;
    }

    public ASRSpeech cancelRecognize()
    {
        mSpeechM.Call("cancelRecognize");
        return this;
    }

    public void onCmd(string slot, string word, int conf)
    {
        foreach (ICmdCallback ob in mObservers)
        {
            if (ob != null)
            {
                ob.onCmd(slot, word, conf);
            }
        }

    }


    public void onEvent(int evt, int code)
    {
        foreach (ICmdCallback ob in mObservers)
        {
            if (ob != null)
            {
                ob.onEvent(evt, code);
            }
        }
    }

    private List<ICmdCallback> mObservers = new List<ICmdCallback>();

    public ASRSpeech addObserver(ICmdCallback observer)
    {
        mObservers.Add(observer);

        return this;
    }

    public ASRSpeech removeObserver(ICmdCallback observer)
    {
        mObservers.Remove(observer);
        return this;
    }
}