using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    private AudioSource audioSource;

    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;

    OVRGrabbable grabbable;
    [SerializeField] OVRGrabber left;
    bool justGrabbed = false;

    private void Start()
    {
        grabbable = GetComponent<OVRGrabbable>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(grabbable.isGrabbed && grabbable.grabbedBy == left)
        {
            
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }

        if(grabbable.isGrabbed && grabbable.grabbedBy == left && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            StartCoroutine(ShootBullet());
        } else if (grabbable.isGrabbed && grabbable.grabbedBy != left && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
           StartCoroutine(ShootBullet());
        }
    }

    private IEnumerator ShootBullet()
    {
        OVRInput.SetControllerVibration(1f, 1f, grabbable.grabbedBy == left ? OVRInput.Controller.LHand : OVRInput.Controller.RHand);
        Instantiate(bullet, shootPoint.position, shootPoint.rotation).GetComponent<Rigidbody>().velocity = shootPoint.forward * bulletSpeed;
        audioSource.Play();
        yield return new WaitForSeconds(.1f);
        OVRInput.SetControllerVibration(0f, 0f, grabbable.grabbedBy == left ? OVRInput.Controller.LHand : OVRInput.Controller.RHand);
    }

    
}
