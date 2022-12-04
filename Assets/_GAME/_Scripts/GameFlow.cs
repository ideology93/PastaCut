using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Animator anim, animateBoil;
    [SerializeField] bool isPhaseOne, isPhaseTwo, isPhaseTwoPointFive, isPhaseThree, isPhaseFour;
    [SerializeField] CameraPositions camPositions;
    [SerializeField] GameObject phaseOneUI, phaseTwoUI, phaseThreeUI;
    [SerializeField] GameObject particleWater;
    [SerializeField] private List<GameObject> noodles = new List<GameObject>();
    [SerializeField] private GameObject obiSolver;
    [SerializeField] GameObject checkmark2;
    [SerializeField] GameObject finalNoodle, drainer;
    [SerializeField] Animator finalAnim1;
    [SerializeField] Animator finalAnim2;
    [SerializeField] SceneFader sceneFader;
    [SerializeField] Stars stars;
    [SerializeField] GameObject endScreen;

    private void Awake()
    {
            // PlayerPrefs.SetInt("level", -1);
        Application.targetFrameRate = 60;

    }
    private void Start()
    {
        obiSolver.GetComponent<NoodleExtender>()._speed = 0;
        cam = Camera.main;
    }
    public void Begin()
    {
        StartPhaseOne();
        Debug.Log(PlayerPrefs.GetInt("level"));

    }
    public void StartPhaseOne()
    {
        isPhaseOne = true;
        camPositions.PhaseOneCamera();
    }
    public void EndPhaseOne()
    {
        isPhaseOne = false;
        StartPhaseTwo();
    }
    public void StartPhaseTwo()
    {
        obiSolver.GetComponent<NoodleExtender>()._speed = 0.003f;
        for (int i = 0; i < obiSolver.transform.childCount; i++)
        {
            obiSolver.transform.GetChild(i).GetComponent<RopeSweepCut>().enabled = true;
            obiSolver.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
        }
        isPhaseTwo = true;
        camPositions.PhaseTwoCamera();
        obiSolver.SetActive(true);
    }
    public void EndPhaseTwo()
    {
        isPhaseTwo = false;
        obiSolver.GetComponent<NoodleExtender>()._speed = 0;
        checkmark2.SetActive(true);
    }
    public void StartPhaseTwoPointFive()
    {
        for (int i = 0; i < obiSolver.transform.childCount; i++)
        {
            obiSolver.transform.GetChild(i).gameObject.SetActive(false);
            
        }
        isPhaseTwoPointFive = true;
        camPositions.PhaseTwoPointFiveCamera();
        drainer.SetActive(true);
        animateBoil.Play("boil");
    }
    public void EndPhaseTwoPointFive()
    {
        isPhaseTwoPointFive = false;
        StartPhaseThree();
    }
    public void StartPhaseThree()
    {
        isPhaseThree = true;
        camPositions.PhaseThreeCamera();
    }
    public void EndPhaseThree()
    {
        isPhaseThree = false;
    }
    public void EnableWinScreen()
    {
        endScreen.SetActive(true);
    }
    public void StartPhaseFour()
    {
        stars.StarsScore(3);
    }
    public void EndPhaseFour()
    {
        isPhaseFour = false;
        sceneFader.FadeTo("MainScene");
    }


}
