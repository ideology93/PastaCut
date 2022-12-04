using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraPositions : MonoBehaviour
{
    private Camera main;
    public GameObject phaseOne;
    public GameObject phaseTwo;
    public GameObject phaseThree;

    public GameObject phaseFour;
    public GameObject phaseFourFive;
    public GameObject  phaseTwoPointFive;

    void Start()
    {
        main = Camera.main;
    }
    public void PhaseOneCamera()
    {
        DOTween.To(() => main.transform.position, x => main.transform.position = x, phaseOne.transform.position, 0.75f);
        main.transform.DORotateQuaternion(phaseOne.transform.rotation, 0.75f);
    }
    public void PhaseTwoCamera()
    {
        DOTween.To(() => main.transform.position, x => main.transform.position = x, phaseTwo.transform.position, 0.75f);
        main.transform.DORotateQuaternion(phaseTwo.transform.rotation, 0.75f);
    }
    public void PhaseTwoPointFiveCamera()
    {
        DOTween.To(() => main.transform.position, x => main.transform.position = x, phaseTwoPointFive.transform.position, 0.75f);
        main.transform.DORotateQuaternion(phaseTwoPointFive.transform.rotation, 0.75f);
    }
    public void PhaseThreeCamera()
    {
        transform.DOLocalRotate(new Vector3(transform.rotation.x, transform.rotation.y - 45, transform.rotation.z), 0.3f);
        DOTween.To(() => main.transform.position, x => main.transform.position = x, phaseThree.transform.position, 0.75f);
        main.transform.DORotateQuaternion(phaseThree.transform.rotation, 0.75f);
        transform.DOLocalRotate(new Vector3(transform.rotation.x, transform.rotation.y + 45, transform.rotation.z), 0.3f);
    }
    public void PhaseFourCamera()
    {
        phaseFour.SetActive(true);
        phaseFour.transform.DOLocalRotate(phaseFourFive.transform.position, 2f, RotateMode.Fast);
        phaseFour.transform.DOMove(phaseFourFive.transform.position, 2.5f, false);
    }
}
