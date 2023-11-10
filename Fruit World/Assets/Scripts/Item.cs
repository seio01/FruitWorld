using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        speed = Random.Range(0.5f, 4f);
        rigid.velocity = Vector2.down * speed;
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EndLine")
            gameObject.SetActive(false);
    }
}
