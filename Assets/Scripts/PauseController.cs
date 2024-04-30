using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public ShipPreparer ShipPreparer;  // manually set to GarageBackup

    private GameObject canvas;
    private GameObject[] pauseObjs;

    [HideInInspector]
    public bool paused;

    void Start()
    {
        if (canvas == null) {
            canvas = GameObject.Find("Canvas");
            pauseObjs = FindChildrenWithTag(canvas, "Pause");
        }
        
        paused = true;
        FlipPauseStatus();

        foreach (GameObject obj in pauseObjs) {
            Button button = obj.GetComponent<Button>();
            if (button != null) {
                button.onClick.AddListener(() => OnButtonClick(button));
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            FlipPauseStatus();
        }
    }

    private void FlipPauseStatus() {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        foreach (GameObject obj in pauseObjs) {
            obj.SetActive(paused);
        }
    }

    public GameObject[] FindChildrenWithTag(GameObject parent, string tag) {
        if (parent != null) {
            List<GameObject> objs = new List<GameObject>();

            foreach (Transform child in parent.transform) {
                if (child.gameObject.CompareTag(tag))
                    objs.Add(child.gameObject);
            }

            return objs.ToArray();
        }
        return null;
    }

    private void OnButtonClick(Button button) {
        if (button.name == "PausedResumeButton") {  // please use these 3 names in all maps
            FlipPauseStatus();
        }
        else if (button.name == "PausedRestartButton") {
            ShipPreparer.GetComponentInChildren<Ship>().RestartRound();
            FlipPauseStatus();
        }
        else if (button.name == "PausedMainMenuButton") {
            SceneManager.LoadScene("Loading");
        }
    }
}
