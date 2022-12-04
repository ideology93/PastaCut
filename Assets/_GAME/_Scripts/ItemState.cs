using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NLO{


public class ItemState : MonoBehaviour
{
    public bool isAdded, isSelected;

    public static int index;
    public GameObject prefab;
    public Color color;
    public Texture tex;
    public ItemUsage itemUsage;

    public void GetItem()
    {
        GameObject obj = Instantiate(prefab, new Vector3(-0.610203385f, -0.0283428133f, 0.167425871f), Quaternion.identity) as GameObject;
        obj.SetActive(true);


    }
    public void SetColor()
    {

        itemUsage.color = color;
        itemUsage.rainbowTexture = tex;
        
    
    }
}
}



