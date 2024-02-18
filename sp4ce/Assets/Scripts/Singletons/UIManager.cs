using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TMP_Text hoverText;
    [SerializeField] private Image staminaBar;
    [SerializeField] private GameObject nightVisionUI;
    [SerializeField] private TMP_Text temperatureText;

    [SerializeField] private GameObject itemTipObject;
    [SerializeField] private ItemTip itemTip;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject discovIndex;

    private void Awake()
    {
        instance = this;
        Color col = staminaBar.color;
        col.a = 0f;
        staminaBar.color = col;
        nightVisionUI.SetActive(false);
        itemTipObject.SetActive(false);
    }

    public void OnHover(string text)
    {
        hoverText.text = text; // make it a prefab and put E key image
    }
    public void OnHoverExit()
    {
        hoverText.text = ""; 
    }

    public void SetStaminaLength(float lerpval)
    {
        Vector3 scale = Vector3.Lerp(new Vector3(0f,1f,1f), Vector3.one, lerpval);
        staminaBar.transform.localScale = scale;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            itemTipObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            discovIndex.SetActive(!discovIndex.activeInHierarchy);
            Cursor.lockState = discovIndex.activeInHierarchy?CursorLockMode.None:CursorLockMode.Locked;
            GameManager.instance.isInUI = discovIndex.activeInHierarchy;
        }
    }

    public void SetStaminaAlpha(float lerpval)
    {
        float alpha = Mathf.SmoothStep(0f,1,lerpval);
        Color col = staminaBar.color;
        col.a = alpha;
        staminaBar.color = col;
    }

    public void ToggleNightVisionUI()
    {
        nightVisionUI.SetActive(!nightVisionUI.activeInHierarchy);
    }

    public void UpdateTemperatureText(float temp)
    {
        temperatureText.text = Mathf.Round(temp*10)*0.1f + "C";
        temperatureText.color = Color.Lerp(Color.white,Color.red,(temp-60f)/(30f));
    }

    public void DisplayTip(string name, string desc, bool saveToIndex)
    {
        StartCoroutine(DisplayTipCoroutine(name,desc,saveToIndex));
    }

    private IEnumerator DisplayTipCoroutine(string name, string desc, bool saveToIndex)
    {
        yield return new WaitForSeconds(0.5f);
        if(saveToIndex)
        {
            if(playerData.DiscoveryIndex.Contains(name)) yield break;
            playerData.DiscoveryIndex.Add(name);
            playerData.DiscoveryIndex.Add(desc);
        }
        itemTip.fTime_elapsed = 6f;
        itemTipObject.SetActive(true);
        itemTip.SetDetails(name,desc);
        PlayerAudioController.instance.PlayAudio(AUDIOSOUND.ITEMTIP);
    }
}