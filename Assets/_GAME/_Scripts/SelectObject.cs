using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NLO
{
public class SelectObject : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] VeggieManager vm;

    public void SetObject()
    {
        vm.currentItem = prefab;
    }
    public void Clear()
    {
        vm.currentItem = null;
    }
}
}