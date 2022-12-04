using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoveGrater : TweenBase
{
    [SerializeField] Transform startPos, endPos, beginPos, lidPos;
    [SerializeField] private Vector3 _positionOne;
    [SerializeField] private Vector3 _positionTwo;
    [SerializeField] private Vector3 _positionThree;

    protected override void SetTweener()
    {
        _tweener = transform
            .DOLocalMove(_positionOne, LoopDuration)

            .SetLoops(LoopCount, LoopType)
            .SetEase(LoopEase)
            .SetDelay(Delay);


    }

    public void MoveOut()
    {
        _tweener = transform.parent.transform
   .DOLocalMove(endPos.position, LoopDuration)
   .SetLoops(LoopCount, LoopType)
   .SetEase(LoopEase)
   .SetDelay(Delay);
    }


}

