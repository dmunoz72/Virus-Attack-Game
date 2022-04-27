using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
	
	public void onlineMultiplayer() {
		SceneManager.LoadScene("LobbyScene");
	}
	
	public void ExitGame(){
		Debug.Log("Quit!!!!!!");
		Application.Quit();
	}

}
