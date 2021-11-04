using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
[RequireComponent(typeof(PlayerController))]
public class animationController : MonoBehaviour
{
    public delegate void EquipFlash(bool _CanShoot);
    public event EquipFlash equipFlash;//sends the shooting state to player shoot
    public Animator animator;//put in animator component manually
    private Animator fpsAnimator;
    public Transform gunPosition;
    public GameObject AKGunModel;
    public GameObject SMGGunModel;
    public GameObject FpsModel;
    public GameObject flashHands;
    public Transform flashHandPos;
    private bool equiptNade = false;
    private GameObject Fps;
    public VisualEffect MuzzelVFX;
    bool MuzzelVFXanimate = true;
    //input game gun models 23/04
    // Start is called before the first frame update
    void Start()
    {
        MuzzelVFX = GameObject.FindGameObjectWithTag("MuzzelFlash").GetComponentInChildren<VisualEffect>();//#
        MuzzelVFX.Stop();
        Fps = GameObject.FindGameObjectWithTag("Fps");
        fpsAnimator = Fps.GetComponent<Animator>();
        FindObjectOfType<PlayerShoot>().relodeGun += RelodeFPS;
        FindObjectOfType<PlayerShoot>().shootGun += ShootAnimation;
        FindObjectOfType<WeaponManager>().switchWeapon += SwitchGunRig;


        if (animator == null)
            Debug.Log("No animator here");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Flash();
        }
        if (fpsAnimator == null)
        {
            fpsAnimator = GameObject.FindGameObjectWithTag("Fps").GetComponent<Animator>();
            if(fpsAnimator == null)
            {
                fpsAnimator = GameObject.FindGameObjectWithTag("Fps").GetComponentInChildren<Animator>();
            }
        }   
            if (MuzzelVFX == null)
            {
                MuzzelVFXanimate = false;
            }
            else
            {
                MuzzelVFXanimate = true;
            }
            //error due to instance
    }
    public void Animate(float horiVelocity, float vertiVelocity)
    {
        if (animator == null)
            return;
        animator.SetFloat("isWalking", vertiVelocity);
        animator.SetFloat("isSideWalking", horiVelocity);

    }
    public void RelodeFPS(bool relodeState)//we have to check which gun we are using then use the respective animation
    {
        fpsAnimator.SetBool("Reloding", relodeState);

    }
    public void ShootAnimation(bool shoot)//can move this script to playershoot.
    {
       // if (MuzzelVFXanimate)
        
            fpsAnimator.SetBool("Firing", shoot);
            if (shoot)
            {
            MuzzelVFX.Play();
                
            }
            if (!shoot)
            {
                MuzzelVFX.Stop();
            }
    }
    public void Flash()
    {
        if (equiptNade == false)
        {
            FindObjectOfType<PlayerShoot>().relodeGun -= RelodeFPS;

            FindObjectOfType<PlayerShoot>().shootGun -= ShootAnimation;
            equipFlash?.Invoke(false);// sets canShoot variable
            Destroy(Fps);
            Fps = Instantiate(flashHands, FpsModel.transform);
            Fps.tag = "Fps";
            equiptNade = true;
        }
        else if (equiptNade == true)
        {
            SwitchGunRig("SMG");
            SwitchingMuzzelFlash("switched to SMG");
            /*FindObjectOfType<PlayerShoot>().relodeGun += RelodeFPS;
            FindObjectOfType<PlayerShoot>().shootGun += ShootAnimation;
            equipFlash?.Invoke(true);// sets canShoot variable
            equiptNade = false;
            Destroy(GameObject.FindGameObjectWithTag("Fps"));
            Fps = Instantiate(AKGunModel, gunPosition);
            Fps.tag = "Fps";*/
        }
    }
    public void SwitchGunRig(string name)//this method will switch gun when called in FPS rig
    {
        switch(name)
        {
            case "Ak47":
                FindObjectOfType<PlayerShoot>().relodeGun += RelodeFPS;
                FindObjectOfType<PlayerShoot>().shootGun += ShootAnimation;
               equipFlash?.Invoke(true);
               equiptNade = false;
                Destroy(GameObject.FindGameObjectWithTag("Fps"));
                Fps = Instantiate(AKGunModel, gunPosition);
                SwitchingMuzzelFlash("switched AK47");
                Fps.tag = "Fps";
               
                break;
            case "SMG":
                FindObjectOfType<PlayerShoot>().relodeGun += RelodeFPS;
                FindObjectOfType<PlayerShoot>().shootGun += ShootAnimation;
                equipFlash?.Invoke(true);
                equiptNade = false;
                Destroy(GameObject.FindGameObjectWithTag("Fps"));
                Fps = Instantiate(SMGGunModel, gunPosition);
                SwitchingMuzzelFlash("switched to SMG");
                Fps.tag = "Fps";
               
                break;


        }
    }
      public void SwitchingMuzzelFlash(string weapon)
    { 
        Debug.Log(weapon);
        MuzzelVFX = Fps.GetComponentInChildren<VisualEffect>();
        MuzzelVFX.Stop();
    }
}
