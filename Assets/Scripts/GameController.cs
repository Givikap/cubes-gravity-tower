using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public Transform cubeToPlace;

    public Text scoreTxt, logoTxt, loseTxt;

    public GameObject cubesToPlace, allCubes, vfx, pauseButton, musicButton, rotator, loseMenu;
    public GameObject[] canvasStartPage;
    public GameObject[] cubesToCreate;

    public AdsController ads;

    public Material[] materials;

    public Color[] bgColors;

    private CubePos nowCube = new CubePos(0, 2, 0);

    private Coroutine showCubesPlace;
    private Transform rotatorPos;

    private Rigidbody allCubesRb;

    private Color toCameraColor;

    private bool IsLose, firstCube;

    private float cameraSpeed = 2f, cubeChangePlaceSpeed = 0.75f;

    private List<Vector3> allCubesPositions = new List<Vector3>() {
        new Vector3(0, 1, 0),
        new Vector3(1, 1, 0),
        new Vector3(-1, 1, 0),
        new Vector3(0, 2, 0),
        new Vector3(0, 1, 1),
        new Vector3(0, 1, -1),
        new Vector3(1, 1, 1),
        new Vector3(-1, 1, -1),
        new Vector3(-1, 1, 1),
        new Vector3(1, 1, -1),
    };

    private string textColor;

    void Start() {
        rotatorPos = rotator.transform;
        toCameraColor = Camera.main.backgroundColor;

        allCubesRb = allCubes.GetComponent<Rigidbody>();
        showCubesPlace = StartCoroutine(ShowCubePlace());

        ads.ShowBanner();

        textColor = ColorUtility.ToHtmlStringRGB(materials[PlayerPrefs.GetInt("cubeType")].color);

        UpdateLogo(textColor);
        UpdateScore(textColor);
    }

    void Update() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && cubeToPlace != null && allCubes != null && Time.timeScale == 1f) {
            pauseButton.SetActive(true);

            if (!firstCube) {
                firstCube = true;
                foreach (GameObject obj in canvasStartPage) {
                    Destroy(obj);
                }
            }

            GameObject newCube = Instantiate(
                cubesToCreate[PlayerPrefs.GetInt("cubeType")],
                cubeToPlace.position,
                Quaternion.identity
            ) as GameObject;

            newCube.transform.SetParent(allCubes.transform);
            nowCube.SetVector(cubeToPlace.position);
            allCubesPositions.Add(nowCube.GetVector());

            GetComponent<AudioSource>().Play();

            GameObject newVfx = Instantiate(
                vfx,
                newCube.transform.position,
                Quaternion.identity
            ) as GameObject;

            Destroy(newVfx, 2.5f);

            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;

            if (newCube.transform.position.y == 1) {
                Lose();

                loseTxt.text = "Tower hit bottom";
            }

            SpawnPositions();
            MoveCameraChangeBg();
        }

        if (!IsLose && allCubesRb.velocity.magnitude > 0.1f) {
            Lose();
            IsLose = true;

            loseTxt.text = "Tower collapsed";
        }

        if (Camera.main.backgroundColor != toCameraColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);

        rotator.transform.localPosition = Vector3.MoveTowards(rotatorPos.localPosition, new Vector3(nowCube.x, nowCube.y - 2f, nowCube.z), cameraSpeed * Time.deltaTime);
    }

    IEnumerator ShowCubePlace() {
        while(true) {
            SpawnPositions();

            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions() {
        List<Vector3> positions = new List<Vector3>();
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.z - 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));

        if (positions.Count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        else if (positions.Count == 0) {
            Lose();
            loseTxt.text = "Nowhere to place cube";
        } else
            cubeToPlace.position = positions[0];
    }

    private bool IsPositionEmpty(Vector3 targetPos) {
        if (targetPos.y == 0) return false;

        foreach (Vector3 pos in allCubesPositions) {
            if (pos.x == targetPos.x && pos.y == targetPos.y && pos.z == targetPos.z) return false;
        }
        return true;
    }

    private void MoveCameraChangeBg() {
        int maxX = 0, maxY = 0, maxZ = 0;

        foreach (Vector3 pos in allCubesPositions) {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Convert.ToInt32(pos.x);
            if (Convert.ToInt32(pos.y) > maxY)
                maxY = Convert.ToInt32(pos.y);
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Convert.ToInt32(pos.z);
        }

        maxY--;

        if (PlayerPrefs.GetInt("pointsCount") < maxY)
            PlayerPrefs.SetInt("pointsCount", maxY);

        GameScore(textColor, maxY);

        if (maxY >= 1001) {
            toCameraColor = bgColors[14];
            cubeChangePlaceSpeed = 0.3f;
        } else if (maxY >= 751) { 
            toCameraColor = bgColors[13];
            cubeChangePlaceSpeed = 0.35f;
        } else if (maxY >= 501) { 
            toCameraColor = bgColors[12];
            cubeChangePlaceSpeed = 0.4f;
        } else if (maxY >= 351) {
            toCameraColor = bgColors[11];
            cubeChangePlaceSpeed = 0.45f;
        } else if (maxY >= 251) { 
            toCameraColor = bgColors[10];
            cubeChangePlaceSpeed = 0.5f;
        } else if (maxY >= 191) { 
            toCameraColor = bgColors[9];
            cubeChangePlaceSpeed = 0.525f;
        } else if (maxY >= 141) { 
            toCameraColor = bgColors[8];
            cubeChangePlaceSpeed = 0.55f;
        } else if (maxY >= 101) { 
            toCameraColor = bgColors[7];
            cubeChangePlaceSpeed = 0.575f;
        } else if (maxY >= 71) {
            toCameraColor = bgColors[6];
            cubeChangePlaceSpeed = 0.6f;
        } else if (maxY >= 51) {
            toCameraColor = bgColors[5];
            cubeChangePlaceSpeed = 0.625f;
        } else if (maxY >= 31) { 
            toCameraColor = bgColors[4];
            cubeChangePlaceSpeed = 0.65f;
        } else if (maxY >= 21) {
            toCameraColor = bgColors[3];
            cubeChangePlaceSpeed = 0.675f;
        } else if (maxY >= 11) { 
            toCameraColor = bgColors[2];
            cubeChangePlaceSpeed = 0.7f;
        } else if (maxY >= 6) { 
            toCameraColor = bgColors[1];
            cubeChangePlaceSpeed = 0.725f;
        } else if (maxY >= 1) {
            toCameraColor = bgColors[0];
            cubeChangePlaceSpeed = 0.75f;
        }
    }

    private void UpdateLogo(string tc) {
        logoTxt.text = $"Cubes <color=\"#{tc}\">Tower</color>";
    }

    private void UpdateScore(string tc) {
        scoreTxt.text = $"<size=40><color=#{tc}>Best:</color></size> " + PlayerPrefs.GetInt("pointsCount");
    }

    private void GameScore(string tc, int y) {
        scoreTxt.text = $"<size=40><color=#{tc}>Best:</color></size> " + PlayerPrefs.GetInt("pointsCount") + "\n<size=20>Now</size>: " + y;
    }

    private void Lose() {
        Destroy(cubeToPlace.gameObject);
        StopCoroutine(showCubesPlace);

        IsLose = true;

        pauseButton.SetActive(false);
        loseMenu.SetActive(true);
    }
}

struct CubePos {
    public int x, y, z;

    public CubePos(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetVector() {
        return new Vector3(x, y, z);
    }

    public void SetVector(Vector3 pos) {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}