using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Sound_Effect_Player : MonoBehaviour
{
    string currentScene;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    void Update() {
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MouseMapFrame" || currentScene == "MotherboardLevel")
        {
            Destroy(this.gameObject);
        }
    }


}
