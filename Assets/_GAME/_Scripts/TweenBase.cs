using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public abstract class TweenBase : MonoBehaviour
{
    public Tweener _tweener;
    [HideInInspector] public bool IsComplete = false;

    [Header("Base Tween Settings")]
    [SerializeField] protected int LoopCount;
    [SerializeField] protected float LoopDuration;
    [SerializeField] protected LoopType LoopType;
    [SerializeField] protected Ease LoopEase;
    [SerializeField] protected float Delay;
    [SerializeField] private bool _playOnAwake;
    [SerializeField] private UnityEvent _eventsOnComplete;
    [SerializeField] private UnityEvent _eventsOnEnable;
    [SerializeField] private UnityEvent _eventsOnDisable;

    protected virtual void Awake()
    {
        SetTweener();
        _tweener.OnComplete(Complete);

        if (_playOnAwake) PlayTween();
    }

    public void PlayTween() => _tweener.Play();
    public void Rewind() => _tweener.Rewind();

    protected abstract void SetTweener();
    protected virtual void Complete()
    {
        IsComplete = true;
        _eventsOnComplete.Invoke();
    }
    protected virtual void OnEnable()
    {
        _eventsOnEnable.Invoke();
    }
    protected virtual void OnDisable()
    {
        _eventsOnDisable.Invoke();
    }
    private void OnDestroy() => _tweener.Kill();

}
