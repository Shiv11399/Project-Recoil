﻿using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerShoot))]
public class WeaponManager : NetworkBehaviour
{
 
    [SerializeField]
    private string weaponLayerName = "Weapon";
    [SerializeField]
    private PlayerWeapon primaryWeapon;
    [SerializeField]
    private PlayerWeapon currentWeapon;
    [SerializeField]
    private WeaponGraphics currentWeaponGraphics;
    [SerializeField]
    private Transform weaponHolder;
    private PlayerShoot playerShoot;
    public GameObject AK47;
    public GameObject SMG;
    GameObject weaponInst;

    private void Start()
    {
        //BuyMenu = GameObject.Find("BuyMenu");
        //create a buy phase later.
        EquipWeapon(primaryWeapon);
        playerShoot = GetComponent<PlayerShoot>();
        if(playerShoot == null)
        {
            Debug.LogError("No playerShoot in weaponManager");
        }
    }
 
   
    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    public WeaponGraphics GetCurrentWeaponGraphics()
    {
        return currentWeaponGraphics;
    }
    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        weaponInst = Instantiate(_weapon.WeaponGraphics, weaponHolder.position, weaponHolder.rotation);
        weaponInst.transform.SetParent(weaponHolder);//to set weapon holder its parent
        currentWeaponGraphics = weaponInst.GetComponent<WeaponGraphics>();//we check for the component graphics on the weapon Instance
        if (currentWeaponGraphics == null)
        {
            Debug.LogError("no graphics on weapon"+weaponInst.name);
        }
        if (isLocalPlayer)
        {
           Util.SetLayerRecursively(weaponInst, LayerMask.NameToLayer(weaponLayerName));
        }
    }
    void UnEquipt()
    {
        Destroy(weaponInst);
    }
    public void GunAK47()
    {
        //PlayerWeapon Gun = GetComponent<PlayerWeapon>();
        UnEquipt();
        PlayerWeapon Gun = new PlayerWeapon();
        Gun.Name = "Ak47";
        Gun.Range = 70f;
        Gun.Damage = 31;
        Gun.WeaponGraphics = AK47;
        Gun.fireRate = 9f;
        Gun.Magzine = 28;
        Gun.recoilAmount = 0.6f;
        Gun.timeForRelode = 3f;
        EquipWeapon(Gun);
        playerShoot.ResetBullets(Gun.Magzine);
    }
    public void GunSMG()
    {
        //PlayerWeapon Gun = GetComponent<PlayerWeapon>();
        UnEquipt();
        PlayerWeapon Gun = new PlayerWeapon();
        Gun.Name = "SMG";
        Gun.Range = 70f;
        Gun.Damage = 26;
        Gun.WeaponGraphics = SMG;
        Gun.fireRate = 13f;
        Gun.Magzine = 22;
        Gun.recoilAmount = 0.3f;
        Gun.timeForRelode = 2.25f;
        EquipWeapon(Gun);
        playerShoot.ResetBullets(Gun.Magzine);
    }

}
