using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class AsrTest : MonoBehaviour, ICmdCallback
{
    public ASRSpeech asr;
    public int statusRecognize = ASRSpeech.EVENT_ASR_STOP;
    public int errorCode = 0;
    public TextMeshProUGUI textRecognizeResult;
    public TextMeshProUGUI textStatus;
    private string cmd = "Wait for start.";

    public void onCmd(string slot, string word, int conf)
    {
        cmd = string.Format("识别词={0}\n 置信度={1}\n slot={2}\n", word, conf, slot);

        Debug.LogFormat(cmd);

    }

    public void onEvent(int evt, int code)
    {
        Debug.LogFormat("onEvent: evt={0} code={1}", evt, code);

        statusRecognize = evt;
        errorCode = code;
    }

    // Start is called before the first frame update
    void Start()
    {
        while (!Permission.HasUserAuthorizedPermission("android.permission.RECORD_AUDIO"))
        {
            Permission.RequestUserPermission("android.permission.RECORD_AUDIO");
        }

        while (!Permission.HasUserAuthorizedPermission("android.permission.READ_EXTERNAL_STORAGE"))
        {
            Permission.RequestUserPermission("android.permission.READ_EXTERNAL_STORAGE");
        }
        while (!Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE"))
        {
            Permission.RequestUserPermission("android.permission.WRITE_EXTERNAL_STORAGE");
        }


        Debug.Log("start");

        asr = new ASRSpeech();
        asr.addObserver(this);
        asr.init();
    }


    public void buttonClick() {
        if (statusRecognize != ASRSpeech.EVENT_ASR_START)
        {
            asr.startRecognize();
        }

        Debug.LogFormat("buttonClick Recognize status={0}", statusRecognize);
    }
    // Update is called once per frame
    void Update()
    {
        if (textStatus != null)
        {
            if (statusRecognize == ASRSpeech.EVENT_ASR_START)
            {

                textStatus.text = "Started.please speak up";

            }
            else if (statusRecognize == ASRSpeech.EVENT_ASR_STOP)
            {

                textStatus.text = "Stop.click to start";

            }
            else if (statusRecognize == ASRSpeech.EVENT_ASR_ERROR)
            {

                textStatus.text = "Error.code:" + errorCode + ". click to start";
            }

        }

        if (textRecognizeResult != null) {

            textRecognizeResult.text = cmd;
        }
    }
}
