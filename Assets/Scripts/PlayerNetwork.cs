using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;
using System.Collections.Generic;

public class PlayerNetwork : NetworkBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private List<Material> materialsPrefab = new List<Material>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        if (IsOwner) GetComponent<Renderer>().material = materialsPrefab[0];
        else GetComponent<Renderer>().material = materialsPrefab[1];
    }
    private void Update()
    {
        if (IsOwner)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            rb.linearVelocity = new Vector3(h * speed, rb.linearVelocity.y, v * speed);
        }
    }
}
