using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // Player
    public GameObject playerPrefab;
    // Spawn Canvas
    public GameObject canvas;
    // Main Camera
    public GameObject sceneCam;
    // UI Canvas Ping Text
    public Text pingText;

    void Awake()
    {
        canvas.SetActive(true);
    }

    void Update() 
    {
        pingText.text = $"Ping: {PhotonNetwork.GetPing()}";
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