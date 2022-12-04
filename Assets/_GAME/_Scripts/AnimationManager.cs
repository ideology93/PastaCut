using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    [SerializeField] protected Animator _lid_left;
    [SerializeField] protected Animator _lid_right;

    [SerializeField] protected Animator _crackEgg_Top;
    [SerializeField] protected Animator _crackEgg_Bottom;



    public void OpenLids()
    {
        _lid_left.Play("LidOpenLeft");
        _lid_right.Play("LidOpenRight");
    }
    public void CloseLids()
    {
        _lid_left.Play("LidCloseLeft");
        _lid_right.Play("LidCloseRight");
    }

    public void CrackEgg()
    {
        _crackEgg_Top.Play("CrackEgg_Top");
        _crackEgg_Bottom.Play("CrackEgg_Bottom");
    }
}
