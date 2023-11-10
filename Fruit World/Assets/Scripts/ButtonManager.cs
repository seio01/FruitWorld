using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    GameObject player;
    Oli oli;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Oli");
        oli = player.GetComponent<Oli>();
    }

    // Start is called before the first frame update
    public void LeftDown()
    {
        oli.inputLeft = true;
    }

    public void LeftUp()
    {
        oli.inputLeft = false;
    }


    public void RightDown()
    {
        oli.inputRight = true;
    }

    public void RightUp()
    {
        oli.inputRight = false;
    }

}
