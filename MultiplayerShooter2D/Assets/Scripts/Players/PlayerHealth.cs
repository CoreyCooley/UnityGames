using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealth : MonoBehaviourPun
{

    public Image healthFill;
    public float currentHealth;
    public float maxHealth = 1.0f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;

    private PhotonView photonVw;

    void Awake() {
        currentHealth = maxHealth;
        photonVw = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void UpdateHealth(float damage)
    {
        healthFill.fillAmount -= damage;
        currentHealth = healthFill.fillAmount;
        CheckHealth();
    }

    public void CheckHealth()
    {
        if(photonView.IsMine && currentHealth <= 0)
        {
            photonVw.RPC("Death", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void Death()
    {
        sr.enabled = false;
        col.enabled = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    [PunRPC]
    public void Revive()
    {
        sr.enabled = true;
        col.enabled = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1;
    }
}
