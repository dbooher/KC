using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerInventory : MonoBehaviour
{
    public List<item> items;
    public Vector2 displaying = new Vector2(0,1);
    public Image slot1;
    public Image slot2;
    public Image slot3;
    public Image slot4;
    public Image slot5;

    // Start is called before the first frame update
    void Start()
    {
        slot1 = GameObject.Find("itemIcon1").GetComponent<Image>();
        slot2 = GameObject.Find("itemIcon2").GetComponent<Image>();
        slot3 = GameObject.Find("itemIcon3").GetComponent<Image>();
        slot4 = GameObject.Find("itemIcon4").GetComponent<Image>();
        slot5 = GameObject.Find("itemIcon5").GetComponent<Image>();
        updateImages();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //update sprites
    void updateImages()
    {
        int iterator = (int)displaying.x;
        slot3.sprite = items[iterator].icon;
        if (iterator + 1 <= displaying.y)
        {
            iterator++;
            slot4.sprite = items[iterator].icon;
            if (iterator + 1 <= displaying.y)
            {
                iterator++;
                slot5.sprite = items[iterator].icon;
                if (iterator + 1 <= displaying.y)
                {
                    iterator++;
                    slot1.sprite = items[iterator].icon;
                    if (iterator + 1 <= displaying.y)
                    {
                        iterator++;
                        slot2.sprite = items[iterator].icon;
                        if (iterator + 1 <= displaying.y)
                        {
                            iterator++;
                        }
                    }
                }
            }
        }
    }

    //update the currently displayed images
    void updateDisplayed()
    {
        if(displaying.y > items.Count)
        {
            int math = items.Count - (int)displaying.y;

        }
    }
}


