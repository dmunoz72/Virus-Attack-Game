using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun{

    [SerializeField] Camera cam;
    public override void Use(){
        Shoot();
    }

    void Shoot(){
       // DetermineRecoil();
        
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit)){
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
        }
    }

   /* void DetermineRecoil(){
        transform.localPosition -= Vector3.forward * 0.1f;

        if (randomizeRecoil){
			
            float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
            float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);

            Vector2 recoil = new Vector2(xRecoil, yRecoil);

           // _currentRotation += recoil;
        } */
		
       /* else{
			
            int currentStep = magSize + 1 - _currentAmmoInMag;
            currentStep = Mathf.Clamp(currentStep, 0, recoilPattern.Length - 1);

            _currentRotation += recoilPattern[currentStep];
        } *
    }*/
}