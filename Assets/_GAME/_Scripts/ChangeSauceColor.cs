using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;


namespace NLO
{
    public class ChangeSauceColor : MonoBehaviour
    {

        [SerializeField] GameObject sauce;
        [SerializeField] Color col;
        [SerializeField] Material mat;
        [SerializeField] ObiEmitter emitter;
        Color sauceLiquid;
        Color sauceSurface;
        Color sauceFresnel;
        void Start()
        {
            Material materialClone = new Material(mat);
            sauceLiquid = sauce.GetComponent<Renderer>().material.GetColor("LiquidColor");
            sauceSurface = sauce.GetComponent<Renderer>().material.GetColor("SurfaceColor");
            sauceFresnel = sauce.GetComponent<Renderer>().material.GetColor("FresnelColor");
        }
        public void ChangeColor()
        {
            sauce.GetComponent<Renderer>().material.SetColor("LiquidColor", col);
            sauce.GetComponent<Renderer>().material.SetColor("SurfaceColor", col);
            sauce.GetComponent<Renderer>().material.SetColor("FresnelColor", col);
            emitter.GetComponent<ObiParticleRenderer>().particleColor = col;

            Debug.Log("Color is now:" + col);   
        }
    }
}
