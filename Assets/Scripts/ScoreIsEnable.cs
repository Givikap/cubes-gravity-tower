using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreIsEnable : MonoBehaviour
{
    public Text status;

    public int needToUnlock, cubeType;

    private void Start() {
        ShowStatuses();
    }

    private void Update() {
        ShowStatuses();
    }

    private void ShowStatuses () {
        if(PlayerPrefs.GetInt("pointsCount") < needToUnlock)
            status.text = Convert.ToString(needToUnlock - Convert.ToInt32(PlayerPrefs.GetInt("pointsCount"))) + " points\nto unlock";
        else if (PlayerPrefs.GetInt("cubeType") == cubeType)
            status.text = "chosen";
        else
            status.text = "";
    }
}
