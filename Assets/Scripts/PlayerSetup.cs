using UnityEngine;
using Mirror;
[RequireComponent(typeof(Player))]//to check if the player component is available
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    public Behaviour[] componentsToDisable;
    [SerializeField]
    private string remoteLayerName = "RemoteplayerLayer";
    public GameObject playerUiPrefab;
    public GameObject playerUiInstance;
   // Camera sceneCamera;
    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else if(isLocalPlayer)
        {
           /* sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);//will disable secene camera
            }*/
           playerUiInstance = Instantiate(playerUiPrefab);
            playerUiInstance.name = playerUiPrefab.name;
            //configure playerUI
            PlayerUI ui = playerUiInstance.GetComponent<PlayerUI>();
            if (ui == null)
                Debug.LogError("no playerUI");
            ui.SetController(GetComponent<PlayerController>()); //???

            GetComponent<Player>().PlayerSetup(); 
        }
      
       
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, player);
    }
    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);//will assign layer name to the game object
    }
    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;//desabling different components in the enemy player.
        }
    }
    private void OnDisable()//to enable the scene camera...
    {
        Destroy(playerUiInstance);
        if(isLocalPlayer)
        GameManager.instance.SetSceneCameraActive(true);

        GameManager.UnRegister(transform.name);
    }


}
