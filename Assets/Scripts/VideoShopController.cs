using UnityEngine;
using UnityEngine.Events;

public class VideoShopController : MonoBehaviour {
    public int needToUnlock, cubeType;

    public GameObject cube;
    public GameObject definedButton;

    public UnityEvent OnClick = new UnityEvent();

    private void Start() {
        definedButton = this.gameObject;
    }

    public void Update() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject) {
                if (PlayerPrefs.GetString("musicStatus") != "No")
                    GetComponent<AudioSource>().Play();

                if (PlayerPrefs.GetInt("videosCount") >= needToUnlock)
                    PlayerPrefs.SetInt("cubeType", cubeType);
            }
        }
    }
}