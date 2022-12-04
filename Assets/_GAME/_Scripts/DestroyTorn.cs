using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Obi;

[RequireComponent(typeof(ObiRope))]
public class DestroyTorn : MonoBehaviour
{

    void Start()
    {
        GetComponent<ObiRope>().OnRopeTorn += DestroyTeared_OnRopeTorn;

    }

    public void DestroyTeared_OnRopeTorn(ObiRope rope, ObiRope.ObiRopeTornEventArgs tearInfo)
    {
        StartCoroutine(DelayDestroy());

        for (int i = rope.elements.Count - 1; i >= 0; --i)
        {
            var elm = rope.elements[i];

            rope.DeactivateParticle(rope.solver.particleToActor[elm.particle2].indexInActor);


            rope.elements.RemoveAt(i);

            if (elm == tearInfo.element)
                break;
        }

        rope.RebuildConstraintsFromElements();

    }
    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(1f);
    }


}