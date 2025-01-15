using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] TMP_InputField NameField;
    const string NamePrefabKey = "PlayerName";

    // Start is called before the first frame update
    void Start()
    {
        NameField = gameObject.GetOrAddComponent<TMP_InputField>();

        if (NameField != null && PlayerPrefs.HasKey(NamePrefabKey))
            NameField.text = PlayerPrefs.GetString(NamePrefabKey);
        else
            NameField.text = "New Player";

        PhotonNetwork.NickName = NameField.text;
    }

    public void SetPlayerName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            PhotonNetwork.NickName = name;

            PlayerPrefs.SetString(NamePrefabKey, name);
            PlayerPrefs.Save();
        }
    }
}