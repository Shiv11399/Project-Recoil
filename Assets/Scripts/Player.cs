using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{

    public GameObject deathEffect;
    public GameObject spawnEffect;
    [SyncVar]
    private bool _isDead = false;
    public bool firstSetup = true;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    public int maxHealth;
    [SyncVar]
    private int currHealth;
    public Behaviour[] disableOnDeath = null;//access dataType[] Name
    [SerializeField]
    private GameObject[] disableGameObjectOnDeath;
    private bool[] wasEnabled;
    public string Team;

    public void PlayerSetup()
    {
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUiInstance.SetActive(true);
        }  
        
        CmdBroadCastNewPlayerSetup();
    }
    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
        //Debug.Log("is this working");
    }
    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {

            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;//check if the component is enabled or not
            }
            DefaultSetup();
            firstSetup = false;
        }
        
    }
    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(9999);
        }
    }
    [ClientRpc]
    public void RpcTakeDamage(int _DamAmount)
    {
        if (isDead)//check if the player is already dead
            return;
        currHealth -= _DamAmount;
        Debug.Log(transform.name + "has" + currHealth + "health");
        if (currHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        
        isDead = true;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(false);
        }
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }
       GameObject GfxInst = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(GfxInst, 10f);
        //switching cameras
       if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUiInstance.SetActive(false);
        }
        Debug.Log(transform.name + "is DEAD");
        StartCoroutine(Respawn());

    }

    private IEnumerator Respawn()//to call a method after a delay
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnDelay);
        Transform _spawnposition = NetworkManager.singleton.GetStartPosition();//to find the spawn point
        transform.position = _spawnposition.position;
        transform.rotation = _spawnposition.rotation;
       // yield return new WaitForSeconds(0.8f);
        //wating for the spawnpoint to set so that we can respawn at the right point
        
        
        DefaultSetup();
        Debug.Log(transform.name + "respawned");
        GameManager.instance.SetSceneCameraActive(false);
        GetComponent<PlayerSetup>().playerUiInstance.SetActive(true);
    }
    void DefaultSetup()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(true);
        }
        currHealth = maxHealth;
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            Debug.Log("collider active");
            _col.enabled = true;
        }
        isDead = false;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
       
        
        GameObject gfxInst = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(gfxInst, 10f);

    }

}
