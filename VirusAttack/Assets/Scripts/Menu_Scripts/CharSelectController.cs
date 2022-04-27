using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class CharSelectController : MonoBehaviourPunCallbacks {

    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject charListItemPrefab;
    public List<GameObject> PlayerListItem = new List<GameObject>();
    public Transform playerListContent2;
    PhotonView PV;
    int i = 0;
    Player player;

    public void Awake()
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        PV = GetComponent<PhotonView>();
        Player[] players = PhotonNetwork.PlayerList;

        // clearing previously playerListItem with player prefab
        foreach (GameObject item in PlayerListItem)
        {
            Destroy(item.gameObject);
        }
        PlayerListItem.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        // the explanation of the foreach loop can be described as:
        // getting list of players in room as a key value pair, a double list,
        // the int represents a key and the value (Player) is grabbing the
        // previously created username.

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // parent charListitemPrefab to playerListContent2
            GameObject newCharItem = Instantiate(charListItemPrefab, playerListContent2);
            newCharItem.GetComponent<PlayerListItem>().SetUp(players[i]);
           /* in listofpeopleinROOM
                randomly pick 1 to be assigned delete from list
                newcharItem("virus")
                waiting for other characterss
            else:
           */

           //if statement below is checking a local individual
           // if so 'ApplyLocalChanges' from PlayerListItem scripts gets called to active
           // the buttons to allow character selection.
           //**current bugs are this allows other players to see all buttons, but they only
           //  effects the local players choice
            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newCharItem.GetComponent<PlayerListItem>().ApplyLocalChanges();
                //***BELOW CODE ALLOWS FOR BUTTONS TO ONLY BE SEEN BY 1 PLAYER,
                // BUT DOES NOT APPREAR IN THE RIGHT SPOT. *****
               // newCharItem.GetComponent<PlayerListItem>().SetUp(players[i]);
             //   PlayerListItem.Add(newCharItem);
            }

            // player is now updated and added to the list.
            PlayerListItem.Add(newCharItem); // WORKS
            // i is incremented to prepare the next position in list for updates.
            i++;
        }
    }
}