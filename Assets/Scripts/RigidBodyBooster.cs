using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyBooster : MonoBehaviour
{
    [SerializeField] private float _forceAmount = 100f;
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _rigidbody.AddForce(Vector3.up * _forceAmount);
        }
    }
}
