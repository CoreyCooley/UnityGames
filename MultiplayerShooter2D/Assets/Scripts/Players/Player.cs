using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviourPun
{

    public GameObject playerCamera;
    public SpriteRenderer spriteRenderer;
    public PhotonView photonVw;
    public Animator animator;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public Text playerName;

    private bool canMove = true;

    public float moveSpeed = 5f;

    private void Awake() 
    {
        // Checks if it is the local player's camera
        if(photonView.IsMine)
        {
            playerCamera.SetActive(true);
            //playerName.text = PhotonNetwork.NickName;
            //playerName.color = Color.green;
        }
        else
        {
            playerName.text = photonVw.Owner.NickName;
            playerName.color = Color.red;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            CheckInputs();
        }
    }

    private void CheckInputs()
    {
        if(canMove)
        {
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f);
            transform.position += movement * moveSpeed * Time.deltaTime;
        }
        // Left mouse button and player isn't moving
        if(Input.GetKeyDown(KeyCode.Mouse0) && !animator.GetBool("isMoving"))
        {
            Shoot();
        }
        else //if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetBool("isShooting", false);
            canMove = true;
        }

        // Have player facing the move direction
        // Move Left
        if(Input.GetKeyDown(KeyCode.D) && !animator.GetBool("isShooting"))
        {
            animator.SetBool("isMoving", true);
            photonVw.RPC("FlipPlayer",RpcTarget.AllBuffered,false);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isMoving", false);
        }

        // Move Right
        if (Input.GetKeyDown(KeyCode.A) && !animator.GetBool("isShooting"))
         {
            animator.SetBool("isMoving", true);
            photonVw.RPC("FlipPlayer", RpcTarget.AllBuffered,true);
         }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void Shoot()
    {
        animator.SetBool("isShooting", true);
        PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation, 0);

        canMove = false;
    }

    // PunRPC required to do the network sync
    [PunRPC]
    private void FlipPlayer(bool isFlipped)
    {
        if(isFlipped)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            playerName.transform.rotation = Quaternion.Euler(0, 180, 0);
            firePoint.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localScale = Vector3.one;
            playerName.transform.rotation = Quaternion.Euler(0, 0, 0);
            firePoint.rotation = Quaternion.Euler(0, 0, 0);
        }        
    }
}
