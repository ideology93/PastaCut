using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

namespace NLO
{
    public class ChangeEmitterSpeed : MonoBehaviour
    {
        [SerializeField] ObiEmitter emitter;
 
        private void Start()
        {
            emitter = transform.GetComponent<ObiEmitter>();

        }
        public void Emit()
        {
            emitter.speed = 5;
        }               
        public void NextPhase()
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);

        }
    }
}
