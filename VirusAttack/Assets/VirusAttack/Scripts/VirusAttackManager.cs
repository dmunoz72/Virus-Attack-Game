using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Spawning;
using MLAPI.Transports.UNET;

namespace VirusAttack
{
    public class VirusAttackManager : MonoBehaviour
    {
        public GameObject connectionButtonPanel;

        public string IpAddress = "127.0.0.1"; // if no input IP should be correct

        UNetTransport transport;

        // public Camera playerCamera;

        // happens on server
        public void Host() // run host button event
        {
            //  playerCamera.gameObject.SetActive(false);
            connectionButtonPanel.SetActive(false); // deactivates pannel after connection button is pressed
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck; // approval check to allow people to join
            NetworkManager.Singleton.StartHost(RandomSpawn(), Quaternion.identity); //

        }
        // happens on server

        private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
        {
            // check the incoming data
            bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "ROCK1234"; // if passwords match, connection is approved
            callback(true, null, approve, RandomSpawn(), Quaternion.identity); // call back, is approved

        }


        public void Join() // run join button event.
        {
            //   playerCamera.gameObject.SetActive(false);
            transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
            transport.ConnectAddress = IpAddress;
            connectionButtonPanel.SetActive(false); // deactivates pannel after connection button is pressed
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("ROCK1234"); // connection data checks 
            NetworkManager.Singleton.StartClient();
        }

        Vector3 RandomSpawn() // this function generates and returns a random spawn location for the the map.
        {
            float x = Random.Range(-5, 5f); // random x
            float y = 5f;                   // predetermined y location
            float z = Random.Range(-5, 5f); // random z
            return new Vector3(x, y, z);
        }

        public void IPAddressChanged(string newAddress)
        {
            this.IpAddress = newAddress;
        }

    }
}