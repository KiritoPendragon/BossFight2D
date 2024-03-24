using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    private Animator _anim;
    private HUDManager _hudManager;

    public int health = 500;

    public bool isInvulnerable = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _hudManager = GetComponent<HUDManager>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _anim.SetTrigger("Death");

        /*_hudManager.winText.SetActive(true);
        _hudManager.tryAgainButton.SetActive(true);
        _hudManager.menuButton.SetActive(true);*/
    }

}
