using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelElectricityController : MonoBehaviour
{
    [SerializeField] GameObject lights;
    [SerializeField] GameObject liftDoor;
    
    public void TurnLightsOff()
    {
        foreach (Transform light in lights.transform)
        {
            light.gameObject.SetActive(false);
        }
    }

    public void TurnLightsOn()
    {
        foreach (Transform light in lights.transform)
        {
            light.gameObject.SetActive(true);
        }
        liftDoor.gameObject.SetActive(false);
    }

}
