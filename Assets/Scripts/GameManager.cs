using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MatchSettings matchSettings;
    public GameObject sceneCamera;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("game Manager error");
        }
        else
        {
            instance = this;
        }
        
    }
    public void SetSceneCameraActive(bool isActive)
    {
        if(sceneCamera == null)
            return;
        sceneCamera.SetActive(isActive);
    }

    

    #region PlayerTracking
    private const string PLAYER_ID_PREFIX = "Player";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();//A dictionary which will store string os type player 
    public static void RegisterPlayer(string _netID, Player _player)//giving a specific name to every player
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);//adding the player to the dictionary
        _player.transform.name = _playerID;
    }
    public static void UnRegister(string playerID)
    {
        players.Remove(playerID);
    }
    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }
    /*private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200,200,200,500));
        GUILayout.BeginVertical();
        foreach(string _playerID in players.Keys)//looping through different elements of dictionary
        {
            GUILayout.Label(_playerID + "  " + players[_playerID].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }*/
    #endregion

}
