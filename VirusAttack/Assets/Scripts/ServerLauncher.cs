using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;


public class ServerLauncher : MonoBehaviourPunCallbacks{
	
	public static ServerLauncher Instance;
	private int instanceCount = 0;
	[SerializeField] TMP_InputField roomNameInputField;
	[SerializeField] TMP_Text errorText;
	[SerializeField] TMP_Text roomNameText;
	[SerializeField] Transform roomListContent;
	[SerializeField] Transform playerListContent;
	[SerializeField] GameObject roomListItemPrefab;
	[SerializeField] GameObject playerListItemPrefab;
	[SerializeField] GameObject startGameButton;

	void Awake(){
		Instance = this;
	}
	
    // Start is called before the first frame update
    private void Start() {
		Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings(); // start server connection
    }
	
	public override void OnConnectedToMaster(){ // is a Photon callback, so it is called automatically when connected to server
		Debug.Log("Connected to Master");
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}
	
	public override void OnMasterClientSwitched(Player newMasterClient){
		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}
	public override void OnJoinedLobby(){ //When lobby is joined loads CJMenu aka Lobby menu
		Debug.Log("Joined Lobby");
		if(instanceCount == 0){
			LobbyMenuManager.Instance.OpenMenu("MapSelect");
			instanceCount++;
		}
		else{
			LobbyMenuManager.Instance.OpenMenu("CJMenu");
		}
		PhotonNetwork.NickName = "Player " + Random.Range(1,100).ToString("00");
	}
	
	public void CreateRoom(){
		if(string.IsNullOrEmpty(roomNameInputField.text)){
			return;
		}
		
		PhotonNetwork.CreateRoom(roomNameInputField.text);
		LobbyMenuManager.Instance.OpenMenu("Connecting");
		
	}
	
	public override void OnJoinedRoom(){
		LobbyMenuManager.Instance.OpenMenu("RoomMenu");
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;
		
		Player[] players = PhotonNetwork.PlayerList;
		
		foreach (Transform child in playerListContent){
			Destroy(child.gameObject);
		}

		for (int i = 0; i < players.Length; i++){
			Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
		}

		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}
	
	public override void OnCreateRoomFailed(short returnCode, string message){
		errorText.text = "Failed to create room: " + message;
		Debug.LogError("Failed to create room: " + message);
		LobbyMenuManager.Instance.OpenMenu("ErrorMenu");
	}
	
	public void StartGame(){
		PhotonNetwork.LoadLevel("MotherboardLevel");
	}
	public void JoinRoom(RoomInfo info){
		PhotonNetwork.JoinRoom(info.Name);
		LobbyMenuManager.Instance.OpenMenu("Connecting");

	}
	
	
	public void LeaveRoom(){
		PhotonNetwork.LeaveRoom();
		LobbyMenuManager.Instance.OpenMenu("Connecting");
	}
	
	public override void OnLeftRoom(){
		LobbyMenuManager.Instance.OpenMenu("CJMenu");
	}
	
	public override void OnRoomListUpdate(List<RoomInfo> roomList){
		
		foreach(Transform trans in roomListContent){
			Destroy(trans.gameObject);
		}
		
		for(int i = 0; i < roomList.Count; i++){
			if(roomList[i].RemovedFromList){
				continue;
			}
			Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}

	}

	public override void OnPlayerEnteredRoom(Player newPlayer){
		Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}
	
}
