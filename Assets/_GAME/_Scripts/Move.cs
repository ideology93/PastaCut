using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace NLO
{
    public class Move : TweenBase
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
        public new void OnEnable()
        {
            _tweener = transform
               .DOLocalMove(startPos.localPosition, LoopDuration)

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
        public void OnDisable()
        {
            _tweener = transform
               .DOLocalMove(beginPos.localPosition, 0.1f)

               .SetLoops(LoopCount, LoopType)
               .SetEase(LoopEase)
               .SetDelay(Delay);

        }


    }
}

