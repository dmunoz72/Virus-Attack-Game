using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;


public class PlayerListItem : MonoBehaviourPunCallbacks{
    [SerializeField] TMP_Text text;
    Player player;
    public GameObject leftButton;
    public GameObject rightButton;
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;

    private void Awake() { }

    // function below takes a photonplayer variable
    // and gives the player a name sets the cahracter
    // to an initial sprite for selection.
    public void SetUp(Player _player){
        player = _player;
        text.text = _player.NickName;
        UpdatePlayerItem(player);
    }

    // function below only effects a local player and activates the buttons
    // attached to the player prefab,
    public void ApplyLocalChanges()
    {
            leftButton.SetActive(true);
            rightButton.SetActive(true);
    }

    // function below destroys the prefab of the player that left
    // the room.
    public override void OnPlayerLeftRoom(Player otherPlayer){
        if(player == otherPlayer){
            Destroy(gameObject);
        }
    }

    // Function below updates the player sprites that can be looped through
    // the function is set up in a way that if the array bounds are met
    // it just loops the selection choices.
    public void OnClickLeftButton()
    {
        Debug.Log("LEFT BUTTON PRESSED");
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        //this line notifies other players of our choice
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        Debug.Log("LEFT BUTTON after photonnetwork");

    }

    // Function below updates the player sprites that can be looped through
    // the function is set up in a way that if the array bounds are met
    // it just loops the selection choices.
    public void OnClickRightButton()
    {
        Debug.Log("Right BUTTON PRESSED");
        if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        //this line notifes other players of our casted properties
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
     //   Debug.Log("righT BUTTON after network, pprop:", playerProperties);

    }

    // Function below calls for updates on player properties any time a
    // player item property is modified over the network.
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    // function below checks if the player has a custom property "playerAvatar" linked to him
    // if so the player avatar will be updated over all copmuters,
    // once selected this choice should be set throughout the duration of the game
    // ** BUG IS THAT BUTTON MUST BE PUSHED 1 TIME B4 STARTING GAME **
    void UpdatePlayerItem(Player player)
    {
        Debug.Log("updating player item");

        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
          //  PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }
    }

    public override void OnLeftRoom(){
        Destroy(gameObject);   
    }

}
