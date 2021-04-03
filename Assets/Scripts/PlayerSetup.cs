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
    private GameObject thirdPersonModel;
    public GameObject firstPersonModel;
    GameObject FPP;
    public GameObject handPlacement;
   // Camera sceneCamera;
    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();//disable components on enemyes or team mates!!!
            AssignRemoteLayer();//change layers for weapon and main camera
        }
        else if(isLocalPlayer)
        {
            thirdPersonModel = GameObject.FindGameObjectWithTag("Graphics");
            SetupFirstPersonGraphics();//setup first person model.
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

        SetLayerRecursively(gameObject, LayerMask.NameToLayer(remoteLayerName));
    }
    void DisableComponents()
    {
        //I want a different model for enemy player I whould set it here
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
    void SetupFirstPersonGraphics()
    {
        //Destroy(thirdPersonModel); will do it later to setup the first person player.
        //FPP = Instantiate(firstPersonModel, handPlacement.transform);// not setting up the fpp character RN
        //whenever you change the gun change the setup here 
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
