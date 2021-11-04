using UnityEngine;
using System.Collections;
using Mirror;
using UnityEngine.VFX;
[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(PlayerMotor))]

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon currentWeapon;
    //reference of relode class
    public Relode RelodeInst;// creating a new animation for relode
    //event for reloading Gun
    public delegate void RelodeGun(bool relodeState);
    public RelodeGun relodeGun;
    //event for shooting
    public delegate void ShootGun(bool shoot);
    public ShootGun shootGun;
    public static bool canShoot = true;
    public Camera cam;
    public LayerMask mask;
    private Rigidbody rb;
    private string PlayerTag = "Player";
    private WeaponManager weaponManager;
    private PlayerMotor playerMotor;
    public float recoilForce = 5.0f;
    private Vector3 originalRotation;
    PlayerWeapon Gun = new PlayerWeapon();
   static WeaponGraphics gra;
    public static float Mag;
    //Muzzel Flashes for different weapons
    public VisualEffect AKmuzzelFlash;
    public VisualEffect SMGMuzzelFlash;
    public VisualEffect PistolMuzzelFlash;
    private void Start()
    {
        FindObjectOfType<animationController>().equipFlash += CanShoot;
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
        WeaponGraphics gra = weaponManager.GetCurrentWeaponGraphics();
    }
    private void Update()
    {

        currentWeapon = weaponManager.GetCurrentWeapon();
        {
            if (currentWeapon.fireRate <= 0)
            {
                if (Input.GetButtonDown("Fire1")&&canShoot == true)
                {
                    originalRotation = new Vector3(originalRotation.x, transform.localEulerAngles.y, originalRotation.z);
                    Shoot();
                    AddRecoil();

                }
            }
            if (Input.GetButtonDown("Fire1") && Mag > 0&&canShoot==true)
            {
                cam.transform.Rotate(Vector3.up, 1);
                originalRotation = new Vector3(originalRotation.x, transform.localEulerAngles.y, originalRotation.z);
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                InvokeRepeating("AddRecoil", 0f, 1f / currentWeapon.fireRate);
                shootGun?.Invoke(true);
            }
            else if (Input.GetButtonUp("Fire1") || Mag == 0)
            {
                CancelInvoke("Shoot");
                CancelInvoke("AddRecoil");
                shootGun?.Invoke(false);
            }
            if(Input.GetKeyDown("r")&&isLocalPlayer)
            {
                RelodeInst.StartRelode(currentWeapon.timeForRelode);
                StartCoroutine(Relode(currentWeapon.Magzine,currentWeapon.timeForRelode,gra));
                relodeGun?.Invoke(true);//whever r is pressed we use announce this event 
                CanShoot(false);
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
      /* if(weaponManager.name == "Ak47")
        {
            AKmuzzelFlash.Play();
        }
        if (weaponManager.name == "SMG")
        {
            SMGMuzzelFlash.Play();
        }
        if (weaponManager.name == "pistol")
        {
            PistolMuzzelFlash.Play();
        }*/
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
    //[Client]
    void Shoot()
    {

        if (!isLocalPlayer)
        {
            return;
        }
        CmdOnShoot();
        Mag -= 1;
        Debug.Log("shoot!"+canShoot);
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
    private IEnumerator Relode(float r, float rt, WeaponGraphics g)//to call a method after a delay
    {
        //Destroy(g.magzinePrefab);
        yield return new WaitForSeconds(rt);//change for different guns later.
        Mag = r;
        relodeGun?.Invoke(false);
        //RelodeInst.StopRelode();
        CanShoot(true);
    }
    public void ResetBullets(float r)
    {
        Mag = r;
    }
    public float Bullets()
    {
        return Mag;
    }
    public void CanShoot(bool _canShoot)
    {
        canShoot = _canShoot;
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
    
