using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerSwitchWeapon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerShooter playerShooter;
    private float switchLayerWeight;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetLayerWeight(5, Mathf.Lerp(animator.GetLayerWeight(5), switchLayerWeight, Time.deltaTime * 5f));
    }

    

    public void ResetAnimatorSwitchWeapon()
    {
        switchLayerWeight = 0;
        animator.SetBool("IsTakingWeapon", false);
    }
}
