using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenuManager : MonoBehaviour {
	
	public static LobbyMenuManager Instance;
	
	[SerializeField] MenuController[] Menus;
	
	
	void Awake(){
		Debug.LogError("Force the build console open...");
		Instance = this;
	}
	
	
	public void OpenMenu(string menuName){
		
		for(int i = 0; i < Menus.Length; i++){
			if(Menus[i].menuName == menuName){
				Menus[i].Open();
			}
			
			else if(Menus[i].open){
				CloseMenu(Menus[i]);
			}
		}
	}
	
	public void OpenMenu(MenuController menuController){
		for(int i = 0; i < Menus.Length; i++){
			if(Menus[i].open){
				CloseMenu(Menus[i]);
			}
		}
		menuController.Open();
	}
	
	public void CloseMenu(MenuController menuController){
		menuController.Close();
	}	
}
