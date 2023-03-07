using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    private bool isOn = false;

    [SerializeField] private AudioClip clipStart;
    [SerializeField] private AudioClip clipGo;


    private AudioSource audioSource;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(isOn)
            transform.localPosition = transform.localPosition + (Vector3.down * Time.deltaTime * 2);
    }

    public void StartLift()
    {
        audioSource.PlayOneShot(clipStart);
        isOn = true;
        transform.GetChild(6).gameObject.SetActive(true);
        StartCoroutine(nextSound());
    }

    private IEnumerator nextSound()
    {
        yield return new WaitForSeconds(1.2f);
        audioSource.loop = true;
        audioSource.clip = clipGo;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(null);
        }
    }
}
