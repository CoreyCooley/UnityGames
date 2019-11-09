using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMoveable : MonoBehaviour
{
    private bool isMovable;
    private float moveSpeed;
    public void Move(float speed){}
    private bool CanMove()
    {
        return isMovable;
    }
    private void SetMoveSpeed(){}
}
