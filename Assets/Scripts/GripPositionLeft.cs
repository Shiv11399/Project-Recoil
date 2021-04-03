using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GripPositionLeft : NetworkBehaviour
{
    WeaponManager weaponManager;
    private GameObject player;
    public GameObject leftHandPos;
    
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
            leftHandPos.transform.position = weaponManager.GetCurrentWeaponGraphics().leftGunGrip.position;
        }
        
        

    }
}
