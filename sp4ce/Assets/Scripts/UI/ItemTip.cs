using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ItemTip : MonoBehaviour
{
    public float fTime_elapsed;

    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDesc;
    [SerializeField] private Image durationBar;

    // Start is called before the first frame update
    void Start()
    {
        fTime_elapsed = 6f;
    }

    void OnEnable()
    {
        fTime_elapsed = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        if(fTime_elapsed > 0f)
        {
            fTime_elapsed-=Time.deltaTime;
            durationBar.fillAmount = Mathf.SmoothStep(0f,1f,fTime_elapsed/6f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    Coroutine nameCoroutine, descCoroutine;
    public void SetDetails(string name, string desc)
    {
        fTime_elapsed = 6f;

        if(nameCoroutine != null || descCoroutine != null) return;
        
        nameCoroutine = StartCoroutine(ScrollName(name));
        descCoroutine = StartCoroutine(ScrollDesc(desc));
    }

    private IEnumerator ScrollName(string name)
    {
        itemName.text = "";
        foreach(char letter in name)
        {
            itemName.text = itemName.text + letter;
            PlayerAudioController.instance.PlayAudio(AUDIOSOUND.SCROLL);
            yield return new WaitForSeconds(0.02f);
        }

        nameCoroutine = null;
    }

    private IEnumerator ScrollDesc(string desc)
    {
        itemDesc.text = "";
        foreach(char letter in desc)
        {
            itemDesc.text = itemDesc.text + letter;
            PlayerAudioController.instance.PlayAudio(AUDIOSOUND.SCROLL);
            yield return new WaitForSeconds(0.02f);
        }

        descCoroutine = null;
    }
}