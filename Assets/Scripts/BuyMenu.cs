using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyMenu : MonoBehaviour
{
    public GameObject buyMenu;
    private GameObject go;
    private bool buyMenuOn = false;
    private WeaponManager weaponManager;

    public void Update()
    {
        

        if (Input.GetKeyDown("b"))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            weaponManager = go.GetComponent<WeaponManager>();
            if (go == null)
                Debug.Log("cant fing the player in buymenu");
            if (buyMenu == null)
            {
                Debug.Log("No buy menu");
            }
            
            PurchaseMenu();
        }
    }
    public void PurchaseMenu()
    {
        //BuyMenu.enabled;

        if (buyMenuOn == false)
        {
            buyMenu.SetActive(true);
            buyMenuOn = true;
           // Time.timeScale = 0;
        }
        else
        {
            buyMenu.SetActive(false);
            buyMenuOn = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Time.timeScale = 1;
        }

    }
    public void BuyAssultRifel()
    {
        weaponManager.GunAK47();
    }
    public void BuySMG()
    {
        weaponManager.GunSMG();
    }
}
