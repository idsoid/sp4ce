using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SceneSwitch(int scene)
    {
        switch(scene)
        {
            case 0:
                SceneManager.LoadScene("AsherScene");
                break;
        }
    }
}