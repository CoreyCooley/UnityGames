using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiece : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float deceleration = 5f;
    public float lifetime = 3f;
    public float fadeSpeed = .5f;

    private SpriteRenderer sr;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifetime -= Time.deltaTime;

        if(lifetime <= 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.MoveTowards(sr.color.a,0f,fadeSpeed * Time.deltaTime));

            if(sr.color.a <= 0f)
                Destroy(gameObject);
        }
    }
}
