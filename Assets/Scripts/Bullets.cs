using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Bullets : MonoBehaviour
{
    private PlayerShoot playerShoot;
    public TextMeshProUGUI MagBullets;
    public GameObject go;
    private void Start()
    {
        playerShoot = go.GetComponent<PlayerShoot>();
    }
    // Update is called once per frame
    void Update()
    {
        
        MagBullets.GetComponent<TextMeshProUGUI>().text = playerShoot.Bullets().ToString();
    }
}
