using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [SerializeField] Transform originalPosition;
    private Vector3 currentRotation, targetRotation, targetPosition, currentPosition, initialPosition;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float kickBackZ;

    public float snappiness, returnAmount;

    public Vector3 InitialPosition
    {
        get { return initialPosition; }
        set { initialPosition = value; }
    }
    void Start()
    {
        initialPosition = GetComponent<Transform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snappiness);
        //transform.localRotation = Quaternion.Euler(currentRotation);
        
    }

    public void Recoil()
    {
        targetPosition -= new Vector3(0,0, 0.001f);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        originalPosition.localPosition = targetPosition;
    }

    public void Back()
    {
        if (transform.localPosition != GetComponent<Weapons>().EquipedPosition.localPosition)
            transform.localPosition = Vector3.Lerp(transform.localPosition, GetComponent<Weapons>().EquipedPosition.localPosition, Time.deltaTime * 1f);
    }
}
