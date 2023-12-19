using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ScreenCapture.CaptureScreenshot($"/Users/givikap/Desktop/Screenshots/screenshot{PlayerPrefs.GetInt("screen")}.png");
            Debug.Log("Cheese:");
            PlayerPrefs.SetInt("screen", PlayerPrefs.GetInt("screen") + 1);
        }
    }
}
