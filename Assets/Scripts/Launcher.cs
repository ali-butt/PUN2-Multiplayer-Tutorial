using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Tooltip("Max number of players per room.")]
    [SerializeField] byte MaxPlayersPerRoom = 4;

    bool IsConnecting;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();print("yes");
        }
        else
        {
            IsConnecting = PhotonNetwork.ConnectUsingSettings();print("no");
            PhotonNetwork.GameVersion = "1";
        }
    }

    public override void OnConnectedToMaster()
    {
        print("connectedToMaster");

        if(IsConnecting)
        {
            PhotonNetwork.JoinRandomRoom();

            IsConnecting = false;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("Joining random room failed");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected: ", cause);
    }

    public override void OnJoinedRoom()
    {
        print("Room joined");

        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("Room for 1");
        }
    }
}