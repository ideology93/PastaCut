using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NLO
{
    public class BoilingAnimEvent : MonoBehaviour
    {

        [SerializeField] GameFlow flow;
        [SerializeField] GameObject ui, checkmark;
        [SerializeField] private GameObject phase3_ui;
        [SerializeField] private GameObject phase4_ui;
        [SerializeField] GameObject noodles;


        void StartNextPhase()
        {
            noodles.SetActive(true);
            flow.StartPhaseThree();
            gameObject.SetActive(false);
            ui.SetActive(true);
            checkmark.SetActive(true);
            phase3_ui.SetActive(false);
            phase4_ui.SetActive(true);
            
        }

    }
}
