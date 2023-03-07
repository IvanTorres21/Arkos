using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    public GameObject button;
    [SerializeField] private UnityEvent clicked;
    AudioSource audioSource;
    GameObject presser;
    bool isPressed;

    private void Start()
    {
        isPressed = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isPressed)
        {
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, 0.8141f, button.transform.localPosition.z);
            isPressed = true;
            presser = other.gameObject;
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        button.transform.localPosition = new Vector3(button.transform.localPosition.x, 0.827f, button.transform.localPosition.z);
        clicked.Invoke();
        isPressed = false;
        presser = null;
    }
}
