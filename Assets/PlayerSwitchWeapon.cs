using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchWeapon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerShooter playerShooter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetAnimatorEquip()
    {
        animator.SetBool("IsTakingWeapon", false);
        animator.SetLayerWeight(5, 0);
        playerShooter.IsArmed = true;
    }
}
