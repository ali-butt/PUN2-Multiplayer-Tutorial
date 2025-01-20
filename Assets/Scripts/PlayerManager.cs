using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Tooltip("Beam of light")]
    [SerializeField] GameObject Beam;

    bool IsFiring;

    public float Health = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Beam.SetActive(false);

        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=green><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(photonView.IsMine)
        {
            ProcessInput();
        }

        if (Beam && IsFiring != Beam.activeInHierarchy)
        {
            Beam.SetActive(IsFiring);
        }

        if(Health <= 0.0f)
            GameManager.instance.LeaveRoom();
    }

    void ProcessInput()
    {
        if (Input.GetButtonDown("Fire1") && !IsFiring)
        {
            IsFiring = true;
        }

        if (Input.GetButtonUp("Fire1") && IsFiring)
        {
            IsFiring = false;
        }
    }


    /// <summary>
    /// MonoBehaviour method called when the Collider 'other' enters the trigger.
    /// Affect Health of the Player if the collider is a beam
    /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
    /// One could move the collider further away to prevent this or check if the beam belongs to the player.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Beam"))
        {
            return;
        }
        Health -= 0.1f;
    }
    /// <summary>
    /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
    /// We're going to affect health while the beams are touching the player
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerStay(Collider other)
    {
        // we dont' do anything if we are not the local player.
        if (!photonView.IsMine)
        {
            return;
        }
        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Beam"))
        {
            return;
        }
        // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
        Health -= 0.1f * Time.deltaTime;
    }



    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(IsFiring);
            stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    }
    #endregion
}
