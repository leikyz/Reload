using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponWheelController : MonoBehaviour
{
    public GameObject weaponWheel;

    public delegate void OpenHandler();
    public event OpenHandler Opened;
    void Start()
    {
        //weaponWheel = GameObject.Find("WeaponWheelInteractives");
        Opened?.Invoke();
    }

    private void OnEnable()
    {
        Opened += OnOpened;
        Opened -= OnClosed;
    }

    private void OnDisable()
    {
        Opened -= OnOpened;
        Opened += OnClosed;
    }

    private void OnOpened()
    {
        //if (Input.GetKey(KeyCode.Tab))
            weaponWheel.SetActive(true);
    }

    private void OnClosed()
    {
        weaponWheel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
         //Debug.Log(openAction.IsPressed());
    }

    private void Open()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Opened?.Invoke();
    }
}
