using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "ScriptableObjects/BulletStat", order = 1)]
public class BulletStats : ScriptableObject
{
    public string gunName;
    [TextArea(10, 10)]
    public string gunInfo;
    [Space(20)]
    public Bullet bullet;
    public GameObject bulletObj;

    public int price;

}
