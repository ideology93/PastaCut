using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Scale : TweenBase
{
    [SerializeField] private Vector3 _scaleEnd;
        [SerializeField] private Vector3 _scaleStart;

    protected override void SetTweener()
    {
        _tweener = transform
            .DOScale(_scaleEnd, LoopDuration)
            .SetLoops(LoopCount, LoopType)
            .SetEase(LoopEase)
            .SetDelay(Delay);
        
    }
     public void Upscale()
    {
        _tweener = transform
            .DOScale(_scaleEnd, LoopDuration)
            .SetLoops(LoopCount, LoopType)
            .SetEase(LoopEase)
            .SetDelay(Delay);
        
    }
    public void Rescale()
    {
         _tweener = transform
            .DOScale(_scaleStart, 1)
            .SetLoops(LoopCount, LoopType)
            .SetEase(LoopEase)
            .SetDelay(Delay);
    }

}
