using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseMenuScript : MonoBehaviourPunCallbacks {
    public GameObject pauseMenu;
    public GameObject hud;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        // Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
         if(Input.GetKeyDown(KeyCode.Tab)){
            if(pauseMenu.activeSelf == false){
                hud.SetActive(false);
                pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
		        Cursor.visible = true;
            }
            else{
                pauseMenu.SetActive(false);
                hud.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
		        Cursor.visible = false;
            }
         }
    }

    public void DisconnectPlayer(){
        Debug.Log("in DisconnectPlayer");
        StartCoroutine(DisconnectAndLoad());
    }

    public void LockMouse(){
        Cursor.lockState = CursorLockMode.Locked;
	    Cursor.visible = false;
    }
    IEnumerator DisconnectAndLoad(){
        Debug.Log("in DisconnectAndLoad");
        PhotonNetwork.Disconnect();
       // while (PhotonNetwork.IsConnected){
       //     Debug.Log("waiting to disconnect");
       //     yield return null;
      //  }
        Debug.Log("disconnected");
        SceneManager.LoadScene(0);
        return null;
    }
}
