using UnityEngine;

public class VideoSetCubesMaterial : MonoBehaviour
{
    public Material blackMaterial;
    public VideoShopController vds;

    private void Start() {
        if (PlayerPrefs.GetInt("videosCount") < vds.needToUnlock)
            GetComponent<MeshRenderer>().material = blackMaterial;
    }
}