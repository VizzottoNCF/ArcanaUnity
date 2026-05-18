using System.Collections.Generic;
using UnityEngine;

public class FGAttacks : MonoBehaviour
{
    [Header("Props")]
    public GameObject Area1;
    public GameObject Area2;
    public GameObject lavaTiles;
    public GameObject lavaTilesVisual;

    [Header("Slam/Push")]
    public List<Transform> platforms;
    public GameObject LHand;
    public GameObject RHand;

    [Header("Lava")]
    public float startAngle;
    public float endAngle;
    public GameObject projectilePrefab;
    public int projectileCount;
    public List<GameObject> AttackSource;
}
