using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField NameInput;

    [Tooltip("Max number of players per room.")]
    [SerializeField] byte MaxPlayersPerRoom = 4;

    [SerializeField] GameObject ConnectButton;
    [SerializeField] Text DisconnectionCause;

    bool IsConnecting;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        DisconnectionCause.gameObject.SetActive(false);
        ConnectButton.SetActive(true);
        NameInput.interactable = true;
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom(); print("yes");
        }
        else
        {
            IsConnecting = PhotonNetwork.ConnectUsingSettings(); print("no");
            PhotonNetwork.GameVersion = "1";
        }
    }


    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        print("connectedToMaster");

        if (IsConnecting)
        {
            PhotonNetwork.JoinRandomRoom();

            IsConnecting = false;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected: ", cause);

        ConnectButton.SetActive(true);

        DisconnectionCause.gameObject.SetActive(true);
        DisconnectionCause.text = cause.ToString();

        NameInput.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        print("Room joined");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("Room for 1");
        }
    }

    #endregion
}