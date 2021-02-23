using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public string Name = "SMG";
    public int Damage = 26;
    public float Range = 70f;
    public GameObject WeaponGraphics;
    public float fireRate = 13f;
    public float Magzine = 22;
    public float recoilAmount = 0.3f;
    public float timeForRelode = 2.25f;

}
