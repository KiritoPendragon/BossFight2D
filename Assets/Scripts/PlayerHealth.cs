using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 1;
    public bool playerDead = false;
    public bool iFrame;

    private HUDManager _hudManager;
    private Animator _anim;
    private Rigidbody2D _rb;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _hudManager = FindObjectOfType<HUDManager>();
    }

    public void TakeDamage(int damage)
    {
        if (iFrame)
        {
            return;
        }

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        _anim.SetTrigger("Death");

        
    }

    public void GameOver()
    {
        Time.timeScale = 0;

        _hudManager.tryAgainButton.SetActive(true);
        _hudManager.menuButton.SetActive(true);
        _hudManager.gameOverText.SetActive(true);
    }

    public void StopMove()
    {
        _rb.velocity = Vector2.zero;
    }
}
