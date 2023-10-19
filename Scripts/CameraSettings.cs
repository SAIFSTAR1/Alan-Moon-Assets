using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{

    private GameObject _player;
    private const string PlayerTag = "Player";
    
    void Start()
    {
        _player = GameObject.FindWithTag(PlayerTag);
    }

    void Update()
    {
        CameraFollow();
    }

    void CameraFollow()
    {
        Vector3 playerPos = _player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        
    }
}
