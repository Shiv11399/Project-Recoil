using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relode : MonoBehaviour
{
    private Animation relode;
    private void Start()
    {
        relode = GetComponentInChildren<Animation>();

    }

    // Update is called once per frame
    public void StartRelode(float relodeSpeed)
    {
        relode.Play();
        relode["Relode"].speed = relodeSpeed;
    }
    public void StopRelode()
    {
       //stop speed

    }
}
