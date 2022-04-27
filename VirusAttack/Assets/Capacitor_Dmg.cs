using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class Capacitor_Dmg : MonoBehaviourPunCallbacks
{
	[SerializeField] Material emissiveMaterial;
	[SerializeField] Renderer objectToChange;
	//[SerializeField] private TextMeshProUGUI emissionIntensityValue;
	//[SerializeField] private float defualtIntensity = 6.5f;
	//[ColorUsageAttribute(true, true)]
	public float currentHealth = 1000f;
	public Color color;

	void Start() {
		emissiveMaterial = objectToChange.GetComponent<Renderer>().material;
		color = emissiveMaterial.GetColor("_EmissionColor");
		//emissionIntensityValue.text = defualtIntensity.ToString("6.5");
	}

	void Update()
    {
		if (currentHealth <= 0)
		{
			emissiveMaterial.SetColor("_EmissionColor", color * 1f);

		//	StartCoroutine(waiter());
		}
	}

	[PunRPC]
	public void RPCap_TakeDamage(float damage)
	{
		Debug.Log("capacitor took damage: " + damage);

		currentHealth -= damage;
		//playerHealthText.text = "+" + currentHealth;

		if (currentHealth <= 0)
		{
			Destroy(gameObject);
		//	Die();
		}
	}

	IEnumerator waiter()
	{
	//Capacitor
		emissiveMaterial.SetColor("_EmissionColor", color * 6f);
		yield return new WaitForSeconds(3f);
		emissiveMaterial.SetColor("_EmissionColor", color * 1f);
		yield return new WaitForSeconds(3f);
	}
}
