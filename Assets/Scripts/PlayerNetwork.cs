using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>(string.Empty);
    //private NetworkVariable<int> materialIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> materialIndex = new NetworkVariable<int>(0);


    private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private List<Material> materialPrefab;
    [SerializeField] private TextMeshPro nameText;

    private Renderer materialRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        materialRenderer = GetComponent<Renderer>();
    }

    public override void OnNetworkSpawn()
    {
        nameText.text = playerName.Value.ToString();
        playerName.OnValueChanged += (oldValue, newValue) =>
        {
            nameText.text = newValue.ToString();
        };

        ApplyMaterial(materialIndex.Value);
        materialIndex.OnValueChanged += (oldValue, newValue) =>
        {
            ApplyMaterial(newValue);
        };
    }
    public void SetName(string name)
    {
        if (IsOwner)
        {
            SendNameToServerRpc(name);
        }
    }

    [Rpc(SendTo.Server)]
    private void SendNameToServerRpc(string name)
    {
        playerName.Value = name;
        SendNameToClientRpc(name);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendNameToClientRpc(string name)
    {
        nameText.text = name;
    }

    public void AssignMaterial(int index)
    {
        /*int randomIndex = Random.Range(0, materialPrefab.Count);
        materialIndex.Value = randomIndex;*/

        if (IsOwner)
        {
            SendMaterialToServerRpc(index);
        }
    }

    [Rpc(SendTo.Server)]
    private void SendMaterialToServerRpc(int index)
    {
        if(index >= 0 && index < materialPrefab.Count)
        {
            materialIndex.Value = index;
        }
    }
    private void ApplyMaterial(int index)
    {
        if (materialPrefab.Count == 0) return;
        materialRenderer.material = materialPrefab[index];
    }

    /*private void Start()
    {
        if (IsOwner) GetComponent<Renderer>().material = materialsPrefab[0];
        else GetComponent<Renderer>().material = materialsPrefab[1];
    }*/

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
