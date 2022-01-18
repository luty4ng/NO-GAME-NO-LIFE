using UnityEngine;
using UnityEngine.UI;
using GameKit;
public class Protagonist_Flip : Enemy
{
    private Image healthBar;
    public float initHealth = 100;
    protected override void Initialize()
    {
        healthBar = GameObject.Find("EnemyHealth").GetComponent<Image>();
        health = initHealth;
    }
}