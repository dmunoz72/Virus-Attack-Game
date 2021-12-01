/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Weapons;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !Weapon.activeSelf)
        {
            StartCoroutine(hammerswing());
        }


        IEnumerator hammerswing()
        {
            hammerswing.SetActive(true);
            float time = Weapon.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
            yield return new WaitForSeconds(1);
            hammerswing.SetActive(false);
        }

    }
}
*/