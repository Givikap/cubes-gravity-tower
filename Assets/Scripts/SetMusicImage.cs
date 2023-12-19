using UnityEngine;
using UnityEngine.UI;

public class SetMusicImage : MonoBehaviour {
    public GameObject music;

    public Sprite musicOn, musicOff;

    private void Start() {
        if (AudioListener.pause == false)
            GetComponent<Image>().sprite = musicOn;
        else
            GetComponent<Image>().sprite = musicOff;
    }

    private void Update() {
        if(AudioListener.pause == false)
            GetComponent<Image>().sprite = musicOn;
        else
            GetComponent<Image>().sprite = musicOff;
    }
}
