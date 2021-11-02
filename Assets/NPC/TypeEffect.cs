using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TypeEffect : MonoBehaviour
{
    public int CharPerSeconds;
    public GameObject nextText;
    public bool isAnim;

    string targetMsg;
    Text msgText;
    int index;
    

    float interval;
    private void Awake()
    {
        msgText = GetComponent<Text>();
    }
    public void SetMsg(string msg)
    {
        if (isAnim)
        {
            //msgText.text = targetMsg;
            CancelInvoke();

            EffectEnd();
        }
        else
        { 
            targetMsg = msg;
            EffectStart();
        }
    }
    void EffectStart()
    {
        isAnim = true;
        msgText = GetComponent<Text>();
        msgText.text = "";
        index = 0;
        nextText.SetActive(false);
        interval = 1 / CharPerSeconds;
        Invoke("Effecting", 0.1f);
    }
    void Effecting()
    {
        if(msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[index];
        index++;
        Invoke("Effecting", 0.1f);

    }
    void EffectEnd()
    {
        msgText.text = targetMsg;
        isAnim = false;
        nextText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
