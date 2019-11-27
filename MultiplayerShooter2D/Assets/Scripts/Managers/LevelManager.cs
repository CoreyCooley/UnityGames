using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LevelManager : MonoBehaviour
{
    // Player
    public GameObject playerPrefab;
    // Spawn Button
    public GameObject canvas;
    // Main Camera
    public GameObject sceneCam;

    void Awake()
    {
        canvas.SetActive(true);
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-5, 5);
        
        Vector2 spawnPosition = new Vector2(playerPrefab.transform.position.x * randomValue, playerPrefab.transform.position.y);

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity, 0);
        canvas.SetActive(false);
        sceneCam.SetActive(false);
    }
}