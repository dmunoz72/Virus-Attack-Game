using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TankSFX : MonoBehaviour
{
    PhotonView view;
    public AudioSource JumpSound;

    // Start is called before the first frame update
    void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine)
        {
            return;
        }

        else
        {
            if (Input.GetButton("Jump"))
            {
                JumpSound.Play();
            }
        }

    }
}
