using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData.asset", menuName = "ScriptableObject/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite sprite;
    public int damage;
    public float fireRate;
    public string description;
    public int metalPlatesNeeded;
    public int energyCoresNeeded;
    public int gearsNeeded;
    public int circuitBoardsNeeded;
}
