using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFollow : MonoBehaviour
{
    [SerializeField] private Animator enforcer;
    [SerializeField] private Animator door;

    private void Start()
    {
        Invoke("StartSequence", 5f);
    }

    public void StartSequence()
    {
        enforcer.SetTrigger("Follow");
        door.SetTrigger("GetBroken");
    }
}
