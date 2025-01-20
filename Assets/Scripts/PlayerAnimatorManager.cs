using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviourPun
{
    [SerializeField] float directionDampTime = 0.25f;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetOrAddComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }



        if (!animator)
        {
            return;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Base Layer.Run") && Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Jump");
        }

        float h = Input.GetAxis("Horizontal");
        float v = Mathf.Clamp(Input.GetAxis("Vertical"), 0, Input.GetAxis("Vertical"));

        animator.SetFloat("Speed", h * h + v * v);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
    }
}
