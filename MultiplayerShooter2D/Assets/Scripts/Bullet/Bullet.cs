using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{

    public float flightSpeed = 7.5f;
    public float lifeTime = 3f;
    public float damage = .15f;

    private PhotonView photonVw;
    private Rigidbody2D rb;

    private void Awake() {
        photonVw = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        StartCoroutine(DestoryBullet());
    }


    private void FixedUpdate() {
        
        rb.velocity = transform.right * flightSpeed;
        Debug.Log($"Bullet Transform {transform.forward}");
        Debug.Log($"Bullet Speed: {flightSpeed}");
        Debug.Log($"Bullet Velocity: {rb.velocity}");
    }

    IEnumerator DestoryBullet()
    {
        yield return new WaitForSeconds(lifeTime);
        // Destory bullet on the network
        photonVw.RPC("Destory", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Destory()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!photonVw.IsMine)
        {
            return;
        }

        PhotonView target = other.GetComponent<PhotonView>();
        if(target != null && (!target.IsMine || target.IsSceneView))
        {
            if(target.tag == "Player")
            {
                target.RPC("UpdateHealth", RpcTarget.AllBuffered, damage);
            }
            photonVw.RPC("Destory", RpcTarget.AllBuffered);
        }
    }

}
