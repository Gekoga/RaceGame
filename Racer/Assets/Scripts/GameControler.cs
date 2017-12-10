using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControler : MonoBehaviour {


	public CheckPointScript[] checkpoints;

    //countdown
    [Header("3,2,1 Go")]
    public bool startable = false;
    public Text counter;

    [Header("Canvas stuff")]
    public GameObject quitMenu;
    public GameObject mainMenu;
    public GameObject pauzeMenu;
    public GameObject helpText;
    public Text highScoreText;


    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(CountDown());
        pauzeMenu.SetActive(false);
        mainMenu.SetActive(false);
        helpText.SetActive(false);

		foreach (CheckPointScript checpoint in checkpoints) {
			checpoint.gamecontroller = this;


		}
		
	}
	
	// Update is called once per frame
	void Update () {
        //PauseMenu
        if (Input.GetKeyDown("escape")) {
            if (Time.timeScale == 1.0) {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauzeMenu.SetActive(true);
                PlayerPrefs.GetInt("Score1");
                highScoreText.text = "Highscore is: " + PlayerPrefs.GetInt("Score1");
                
            }
            else if (Time.timeScale != 1.0){
                ButtonContinue();
            }
        }


	}
    //counting checkpoints
	public void UpdateCheckpoints(GameObject go, int checkpointIndex) {

        CarScripts ps = go.GetComponentInParent<CarScripts>();
        if (ps != null) {
            if (checkpointIndex == ps.stats.currentCheckpoint) {
                ps.stats.currentCheckpoint++;
                ps.points += 100;
            }

            if (ps.stats.currentCheckpoint == checkpoints.Length && checkpointIndex == 0) {
                ps.stats.currentLap++;
                ps.stats.currentCheckpoint = 1;
                ps.points += 500;
            }
        }
        if (PlayerPrefs.GetInt("Score1") < ps.points) {
            PlayerPrefs.SetInt("Score1", ps.points);
            PlayerPrefs.Save();
        }

        

    }
    //Buttons for life
    public void ButtonQuit() {
        Application.Quit();
    }
    public void ButtonMainMenu() {
        Time.timeScale = 0f;
        pauzeMenu.SetActive(false);
        mainMenu.SetActive(true);
        quitMenu.SetActive(true);


    Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ButtonContinue() {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauzeMenu.SetActive(false);
    }
    public void ButtonHelp() {
        helpText.SetActive(true);
        pauzeMenu.SetActive(false);
        mainMenu.SetActive(false);
        quitMenu.SetActive(false);
    }
    public void ButtonStart() { 
        Time.timeScale = 1.0f; 
        SceneManager.LoadScene("Level 2");
        //pauzeMenu.SetActive(true);
    }
    public void ButtonBack() {
        helpText.SetActive(false);
        pauzeMenu.SetActive(true);
    }
    public void ButtonLevel1() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Level 1");

    }
    //reset highscore
    public void Slider() {
        PlayerPrefs.SetInt("Score1", 0);
        highScoreText.text = "Highscore is: " + PlayerPrefs.GetInt("Score1");
    }

    // countdown
    IEnumerator CountDown() {
        counter.text = "3";
        yield return new WaitForSeconds(1);
        counter.text = "2";
        yield return new WaitForSeconds(1);
        counter.text = "1";
        yield return new WaitForSeconds(1);
        counter.text = "Go!";
        startable = true;
        yield return new WaitForSeconds(1);
        counter.text = "";
    }
}
