using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizeNoodles : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Component[] components = other.gameObject.GetComponents(typeof(Component));
        foreach (Component component in components)
        {
            
        
        }
    }
}
