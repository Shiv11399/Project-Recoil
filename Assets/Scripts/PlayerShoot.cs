﻿using UnityEngine;
using System.Collections;
using Mirror;
[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(PlayerMotor))]

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon currentWeapon;

    public Camera cam;
    public LayerMask mask;
    private Rigidbody rb;
    private string PlayerTag = "Player";
    private WeaponManager weaponManager;
    private PlayerMotor playerMotor;
    public float recoilForce = 5.0f;
    private Vector3 originalRotation;
    PlayerWeapon Gun = new PlayerWeapon();
    public static float Mag;
    private void Start()
    {
        Mag = Gun.Magzine;
        if (cam == null)
        {
            Debug.LogError("PlayerShoot : No camera component");
            this.enabled = false;
        }
        originalRotation = transform.localEulerAngles;

        // weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);//assign layer to weaponGFX
        weaponManager = GetComponent<WeaponManager>();
        playerMotor = GetComponent<PlayerMotor>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {

        currentWeapon = weaponManager.GetCurrentWeapon();
        {
            if (currentWeapon.fireRate <= 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    originalRotation = new Vector3(originalRotation.x, transform.localEulerAngles.y, originalRotation.z);
                    Shoot();
                    AddRecoil();

                }
            }
                if (Input.GetButtonDown("Fire1")&& Mag>0)
                {
                    cam.transform.Rotate(Vector3.up, 1);
                    originalRotation = new Vector3(originalRotation.x, transform.localEulerAngles.y, originalRotation.z);
                    InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                    InvokeRepeating("AddRecoil", 0f, 1f / currentWeapon.fireRate);
                }
                else if (Input.GetButtonUp("Fire1")|| Mag == 0)
                {
                    CancelInvoke("Shoot");
                    CancelInvoke("AddRecoil");
                }
            
            if(Input.GetKeyDown("r"))
            {
                StartCoroutine(Relode(currentWeapon.Magzine,currentWeapon.timeForRelode));
                
            }
        }
    }
        

    
    [Command]
    void CmdOnShoot()//will only called on the server
    {
        RpcDoShootEffect();
    }
    [ClientRpc]
    void RpcDoShootEffect()//will be called on all the clients
    {
        Debug.Log("muzzel flash");
        weaponManager.GetCurrentWeaponGraphics().muzzelFlash.Play();
    }
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)//will be called on all the clients
    {
        GameObject _hitEffect = Instantiate(weaponManager.GetCurrentWeaponGraphics().hiteffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 2f);
    }
    [Client]
    void Shoot()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        CmdOnShoot();
        Mag -= 1;
        Debug.Log("shoot!");
        rb.AddForce(transform.forward * -recoilForce);
        rb.AddForce(transform.up * recoilForce);
        
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.Range, mask))
        {
            if (_hit.collider.tag == PlayerTag)
            {
                CmdPlayerHasBeenShot(_hit.collider.name, currentWeapon.Damage);
            }
            CmdOnHit(_hit.point, _hit.normal);
            Debug.Log("we hit " + _hit.collider.name);
        }
    }
    [Command]
    void CmdPlayerHasBeenShot(string PlayerID, int _damage)
    {
        Debug.Log(PlayerID + "has been shot");
        Player _player = GameManager.GetPlayer(PlayerID);
        _player.RpcTakeDamage(_damage);
    }
    private void AddRecoil()
    {
        playerMotor.Recoil(Gun.recoilAmount);
        // transform.localEulerAngles += upRecoil;
    }
    private IEnumerator Relode(float r, float rt)//to call a method after a delay
    {
        yield return new WaitForSeconds(rt);//change for different guns later.
        Mag = r;
    }
    public void ResetBullets(float r)
    {
        Mag = r;
    }
    public float Bullets()
    {
        return Mag;
    }
    /*private void StopRecoil()
    {
         transform.localEulerAngles = originalRotation;
        

       /* if(transform.localEulerAngles.x != originalRotation.x)
        {
            transform.localEulerAngles += new Vector3(recoilSpeed , 0, 0);
            if (transform.localEulerAngles.x > originalRotation.x)
            {
                transform.localEulerAngles = originalRotation;
                return;
            }
            StopRecoil();
        }  */
}
    
