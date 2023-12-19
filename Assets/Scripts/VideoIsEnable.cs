using System;
using UnityEngine;
using UnityEngine.UI;

public class VideoIsEnable : MonoBehaviour
{
    public Text status;

    public int needToUnlock, cubeType;

    private void Start() {
        if (PlayerPrefs.GetInt("videoCount") < needToUnlock)
            status.text = Convert.ToString(needToUnlock - Convert.ToInt32(PlayerPrefs.GetInt("videosCount"))) + " video\nto unlock";
        else if (PlayerPrefs.GetInt("cubeType") == cubeType)
            status.text = "chosen";
    }

    private void Update() {
        if (PlayerPrefs.GetInt("videosCount") < needToUnlock)
            status.text = Convert.ToString(needToUnlock - Convert.ToInt32(PlayerPrefs.GetInt("videosCount"))) + " video\nto unlock";
        else if (PlayerPrefs.GetInt("cubeType") == cubeType)
            status.text = "chosen";
        else
            status.text = "";
    }
}
