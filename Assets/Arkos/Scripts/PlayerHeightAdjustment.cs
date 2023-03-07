using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeightAdjustment : MonoBehaviour
{
    [SerializeField] private float startHeight;
    [SerializeField] private float startCameraPos;
    [SerializeField] private CharacterController playerController;

    private void FixedUpdate()
    {
        playerController.height = startHeight + (transform.localPosition.y - startCameraPos);
    }



}
