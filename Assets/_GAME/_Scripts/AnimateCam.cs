using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AnimateCam : TweenBase
{
    [SerializeField] Transform startPos, endPos, beginPos, lidPos;
    [SerializeField] private Vector3 _positionOne;
    [SerializeField] private Vector3 _positionTwo;
    [SerializeField] private Vector3 _positionThree;

    [SerializeField] private Transform AnimatedCamera;
    protected override void SetTweener()
    {
        _tweener = transform
            .DOMove(AnimatedCamera.position, LoopDuration)

            .SetLoops(LoopCount, LoopType)
            .SetEase(LoopEase)
            .SetDelay(Delay);
        _tweener = transform
      .DORotate(AnimatedCamera.position, LoopDuration)

      .SetLoops(LoopCount, LoopType)
      .SetEase(LoopEase)
      .SetDelay(Delay);

    }

}

