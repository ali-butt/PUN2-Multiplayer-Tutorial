using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
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
        if(animator)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Mathf.Clamp(Input.GetAxis("Vertical"), 0, Input.GetAxis("Vertical"));

            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }
    }
}
