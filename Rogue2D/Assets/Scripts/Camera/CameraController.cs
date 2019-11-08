using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;
    
    public float moveSpeed = 30f;
    public Transform target;

    private void Awake() 
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 cameraTargetPostition = new Vector3(target.position.x, target.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, cameraTargetPostition , moveSpeed * Time.deltaTime);
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
