using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottom : MonoBehaviour
{
    float currentPos;
    float direction = 1.5f;
    public GameManager manager;
    Rigidbody2D rigid;

    private void Awake()
    {
        currentPos = transform.position.y;
        rigid = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isStart)
            return;
        currentPos += Time.deltaTime * direction;
        if (currentPos >= 0)
        {
            direction *= -1;
            currentPos = 0;
        }
        else if (currentPos <= -3.5f)
        {
            direction *= -1;
            currentPos = -3.5f;
        }

        transform.position = new Vector3(0, currentPos, 0);
    }
}
