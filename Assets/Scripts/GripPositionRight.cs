using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GripPositionRight : NetworkBehaviour
{
    WeaponManager weaponManager;
    private GameObject player;
    public GameObject rightHandPos;


    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weaponManager = player.GetComponent<WeaponManager>();
        //transform.Rotate(weaponManager.GetCurrentWeaponGraphics().leftGunGrip.eulerAngles);
    }
    private void Update()
    {
       if (isLocalPlayer)
        {
            //Debug.Log(weaponManager.GetCurrentWeaponGraphics().name);
            rightHandPos.transform.position = weaponManager.GetCurrentWeaponGraphics().rightGunGrip.position;

        }
           
        if (Input.GetKeyDown("c"))
        {
            Debug.Log("work!!!!");
        }


    }
}
