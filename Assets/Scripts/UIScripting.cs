using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScripting : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button Button;
    [SerializeField] private GameObject panel;
    private void Awake()
    {
        Button.onClick.AddListener(SetName);
    }
    private void SetName()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject p in player)
        {
            p.GetComponent<PlayerNetwork>().SetName(nameInputField.text);
        }
        Destroy(panel);
     }
}
