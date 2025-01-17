using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("Beam of light")]
    [SerializeField] GameObject Beam;

    bool IsFiring;

    // Start is called before the first frame update
    void Start()
    {
        Beam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && !IsFiring)
        {
            IsFiring = true;
        }
        
        if(Input.GetButtonUp("Fire1") && IsFiring)
        {
            IsFiring = false;
        }

        if(Beam && IsFiring != Beam.activeInHierarchy)
        {
            Beam.SetActive(IsFiring);
        }
    }
}
