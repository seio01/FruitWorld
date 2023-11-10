using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oli : MonoBehaviour
{
    Rigidbody2D rigid;
    
    public GameManager manager;
    SpriteRenderer sp;
    public float speed = 2.0f;
    public AudioClip eatClip;
    public int health;

    public bool inputLeft;
    public bool inputRight;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!manager.isStart)
            return;
        float leftBorder = -2.35f + transform.localScale.x / 2f; 
        float rightBorder = 2.35f - transform.localScale.x / 2f;
        if (transform.position.x <= leftBorder)
            transform.position = new Vector3(leftBorder, transform.position.y, 0);
        else if (transform.position.x >= rightBorder)
            transform.position = new Vector3(rightBorder, transform.position.y, 0);
        else
        {
            if (inputLeft)
            {
                transform.Translate(Vector3.left * Time.fixedDeltaTime * speed);
            }
            if (inputRight)
                transform.Translate(Vector3.right * Time.fixedDeltaTime * speed);
            //float h = Input.GetAxisRaw("Horizontal");

        }
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            switch (collision.name)
            {
                case "Grape":
                    if (manager.grapeCount != manager.grapeTotCount)
                    {
                        manager.grapeCount++;
                        manager.SoundPlayer("Eat");
                    }
                    else
                    {
                        health--;
                        manager.UpdateLifeIcon(health);
                        manager.SoundPlayer("EatWrong");
                    }
                    break;
                case "Peach":
                    if (manager.peachCount != manager.peachTotCount)
                    {
                        manager.peachCount++;
                        manager.SoundPlayer("Eat");
                    }
                    else
                    {
                        health--;
                        manager.UpdateLifeIcon(health);
                        manager.SoundPlayer("EatWrong");
                    }
                    break;
                case "Strawberry":
                    if (manager.strawCount != manager.strawTotCount)
                    {
                        manager.strawCount++;
                        manager.SoundPlayer("Eat");
                    }
                    else
                    {
                        health--;
                        manager.UpdateLifeIcon(health);
                        manager.SoundPlayer("EatWrong");
                    }
                    break;
                case "Icecream":
                    health++;
                    if (health >= 3)
                        health = 3;
                    manager.UpdateLifeIcon(health);
                    manager.SoundPlayer("EatHealth");
                    break;
                case "Bomb":
                    health--;
                    manager.UpdateLifeIcon(health);
                    manager.SoundPlayer("Bomb");
                    Handheld.Vibrate();
                    break;
                case "TrashCan":
                    manager.grapeCount = 0;
                    manager.peachCount = 0;
                    manager.strawCount = 0;
                    manager.SoundPlayer("EatWrong");
                    Handheld.Vibrate();
                    break;
                case "Shoe":
                    StopCoroutine(ChangeStatusByGum());
                    StartCoroutine(ChangeStatusByShoe());
                    manager.SoundPlayer("EatHealth");
                    break;
                case "Gum":
                    StopCoroutine(ChangeStatusByShoe());
                    StartCoroutine(ChangeStatusByGum());
                    manager.SoundPlayer("EatWrong");
                    Handheld.Vibrate();
                    break;
            }
            if (health == 0)
                manager.GameOver();
            collision.gameObject.SetActive(false);

            if ((manager.grapeCount == manager.grapeTotCount) && (manager.peachCount == manager.peachTotCount)
                &&(manager.strawCount == manager.strawTotCount))
            {
                manager.StageEnd();
            }
            
        }   
    }

    IEnumerator ChangeStatusByShoe()
    {
        speed = 4f;
        sp.color = new Color(1, 0, 0, 1);

        yield return new WaitForSeconds(5f);
        speed = 2f;
        sp.color = new Color(1, 1, 1, 1);

    }

    IEnumerator ChangeStatusByGum()
    {
        speed = 0.5f;
        sp.color = new Color(0, 0, 1, 1);

        yield return new WaitForSeconds(5f);
        speed = 2f;
        sp.color = new Color(1, 1, 1, 1);

    }


}
