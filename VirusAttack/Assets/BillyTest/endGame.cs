using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class endGame : MonoBehaviour
{
    public float timeRemaining = 40;
    public bool timerIsRunning = false;
    public bool oneWay = false;
    public bool mapCheck = false;
    public TextMeshProUGUI timeText;
    public GameObject Menu;
    public GameObject Menu2;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        DisplayTime(timeRemaining-10);
        if(GameObject.Find("Cap (3)") == null)
        {
            mapCheck = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            DisplayTime(timeRemaining-10);
            if(timeRemaining < 11 && timeRemaining > 10)
            {
                Menu.SetActive(true);
                Debug.Log("It is a draw");
                
            }
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            //if ((GameObject.Find("Cap") == null && GameObject.Find("Cap (1)") == null &&
            //    GameObject.Find("Cap (2)") == null && GameObject.Find("Cap (3)") == null &&
            //    GameObject.Find("Cap (4)") == null  && GameObject.Find("Cap (5)") == null  &&
            //        GameObject.Find("Cap (6)") == null) || timeRemaining <= 0)
            
            if (timeRemaining <= 0)
            {
                timerIsRunning=false;
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("LobbyScene");
            }
            if (GameObject.Find("Cap (3)") == null && oneWay == false && mapCheck == false)
            {
                Debug.Log("Virus wins ");
                Menu2.SetActive(true);
                timeRemaining = 9;
                oneWay = true;
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
