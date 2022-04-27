using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GP_Audio : MonoBehaviour
{
    PhotonView view;
    string currentScene;
    public AudioSource GameMusic;


    // Start is called before the first frame update
    void Awake()
    {
        view = GetComponent<PhotonView>();
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        if (!view.IsMine)
        {
            return;
        }
        else
        {
            GameMusic.Play();
        }
    }

    void Update()
    { 
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Menus")
        {
            Destroy(this.gameObject);
        }

    }
}
