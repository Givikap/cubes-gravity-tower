using UnityEngine;

public class SetCubeMaterial : MonoBehaviour
{
    public Material[] materials;

    private void Start() {
        GetComponent<MeshRenderer>().material = materials[PlayerPrefs.GetInt("cubeType")];
    }
}