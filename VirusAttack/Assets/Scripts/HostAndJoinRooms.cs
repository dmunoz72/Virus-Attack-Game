using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HostAndJoinRooms : MonoBehaviourPunCallbacks{
	public InputField hostInput;
	public InputField joinInput;
	
	public void HostRoom(){
		PhotonNetwork.CreateRoom(hostInput.text);
		
	}

	public void JoinRoom(){
		PhotonNetwork.JoinRoom(joinInput.text);
		
	}
	
	public override void OnJoinedRoom(){ //When Room is joined loads Selected Level
		PhotonNetwork.LoadLevel("MotherboardLevel");
	}
	

}