using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScripting : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button Button;
    [SerializeField] private GameObject panel;
    private void Awake()
    {
        Button.onClick.AddListener(SetConfiguration);
    }
    private void SetConfiguration()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject p in player)
        {
            var configuration = p.GetComponent<PlayerNetwork>();

            configuration.SetName(nameInputField.text);
            configuration.AssignMaterial(dropdown.value);
        }
        Destroy(panel);
     }
}
