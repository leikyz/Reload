using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponWheelInputs : MonoBehaviour
{
    public GameObject weaponWheel;

    private float timeOnPressed;

    public event Action<float> OnTabPressed;

    [SerializeField] private bool isOpened = false;

    [SerializeField] private TimeController timeController;

    void Start()
    {
        OnTabPressed += OnOpened;
    }

    private void OnOpened(float obj)
    {
        Debug.Log("number : " + obj);
        timeController.DoSlowMotion();
        weaponWheel.SetActive(true);
        isOpened = true;
    }

    public void Close()
    {
        timeOnPressed = 0;
        isOpened = false;
        weaponWheel.SetActive(false);
        timeController.DoBaseMotion();

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            OnTabPressed?.Invoke(timeOnPressed += 1 * Time.deltaTime);
        }
            
        else
        {
            if (isOpened)
                Close();
        }
            
        
    }

}
