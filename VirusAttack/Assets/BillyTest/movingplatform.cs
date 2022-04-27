using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingplatform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform _targetA, _targetB;
    [SerializeField]
    private float _speed = 1.0f;
    private Vector3 _currentTarget;
    void Start()
    {
        _currentTarget = _targetB.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        if (transform.position == _targetA.position)
        {
            _currentTarget = _targetB.position;
        }
        else if (transform.position == _targetB.position)
        {
            _currentTarget = _targetA.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wizard" | other.gameObject.tag == "tank" | other.gameObject.tag == "glasscannon" | other.gameObject.tag == "medic" | other.gameObject.tag == "virus")
        {
            other.transform.parent = this.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wizard" | other.gameObject.tag == "tank" | other.gameObject.tag == "glasscannon" | other.gameObject.tag == "medic" | other.gameObject.tag == "virus")
        {
            other.transform.parent = null;
        }
    }
}
