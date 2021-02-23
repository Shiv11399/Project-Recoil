using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HostGame : MonoBehaviour
{
    public int roomSize = 6;
    public string roomName;
     public void RoomSet(string roomN , int roomS)
    {
        roomSize = roomS;
        roomName = roomN;
    }

}

