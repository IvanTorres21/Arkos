using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void OpenDoor()
    {
        GetComponent<AudioSource>().Play();
        Debug.LogWarning("DoorOpened");
        anim.SetTrigger("Open");
    }

    public void HalfOpen()
    {
        anim.SetTrigger("HalfOpen");
    }
}
