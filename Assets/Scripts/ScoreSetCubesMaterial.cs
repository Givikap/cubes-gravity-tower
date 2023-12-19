using UnityEngine;

public class ScoreSetCubesMaterial : MonoBehaviour {
    public Material blackMaterial;
    public ScoreShopController ssc;

    private void Start() {
        if (PlayerPrefs.GetInt("pointsCount") < ssc.needToUnlock)
            GetComponent<MeshRenderer>().material = blackMaterial;
    }
}
