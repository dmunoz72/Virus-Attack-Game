using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class endGameMouse : MonoBehaviour
{
    public float timeRemaining = 40;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;
    public GameObject Menu;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        DisplayTime(timeRemaining - 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            DisplayTime(timeRemaining - 10);
            if (timeRemaining < 11 && timeRemaining > 10)
            {
                Menu.SetActive(true);
                Debug.Log("Game is Over");

            }
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            
            if (timeRemaining <= 0)
            {
                timerIsRunning = false;
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("LobbyScene");
            }
         
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
