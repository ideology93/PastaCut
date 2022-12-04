using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Timers;
using UnityEngine.UI;
using Obi;
namespace NLO
{


    public class ItemUsage : MonoBehaviour
    {
        [SerializeField] Transform startPos, endPos, beginPos;
        [SerializeField] List<GameObject> items = new List<GameObject>();
        [SerializeField] List<Transform> uiList = new List<Transform>();
        [SerializeField] GameObject phase1_UI;
        [SerializeField] int index;
        [SerializeField] string selecteditems;
        [SerializeField] Transform used;
        [SerializeField] private GameObject obiSolver;
        [SerializeField] AnimationManager anim;
        [SerializeField] GameObject itemsToAdd;
        [SerializeField] List<ParticleSystem> psColoring = new List<ParticleSystem>();
        [SerializeField] List<string> itemsAdded = new List<string>();
        [SerializeField] GameObject checkmarkUI;
        [SerializeField] GameObject waterParticle, waterParticleDelay, DropsLid;
        [SerializeField] Animator sauceAnim;
        float waterParticleStartPos = -0.2833f, waterParticleEndPos = -0.074f, particlePosition;
        [SerializeField] private GameObject water, waterPitcher;
        float currentFill, speed, speedPitcher, speedPosition, currentPitcherFill;
        float currentPitcherY, endPitcherY;
        float endFill = 0.3f;
        float endPitcherFill = -0f;
        float duration = 2;
        public GameObject currentObject, previousObject;
        int previousIndex, uiIndex;
        public Texture rainbowTexture;
        private ParticleSystem flourParticle;
        public Color color;
        public GameObject noodle_animation;
        [SerializeField] Material noodleMaterial;
        [SerializeField] GameObject finishedNoodles, finishedNoodlesTwo;
        [SerializeField] bool isWaterAdded = false;
        [SerializeField] GameObject fakeWater;
        [SerializeField] GameObject phase4_UI;
        [SerializeField] GameObject boilingNoodles1;
        [SerializeField] GameObject boilingNoodles2;
        [SerializeField ]  GameObject cockroach;
        [SerializeField] Transform Cockpos;



        private void Start()
        {
            currentPitcherY = waterPitcher.transform.localPosition.y;
            endPitcherY = -0.15f;
            currentFill = water.GetComponent<Renderer>().material.GetFloat("FillAmount");
            currentPitcherFill = waterPitcher.GetComponent<Renderer>().material.GetFloat("FillAmount");


            for (int i = 0; i < phase1_UI.transform.childCount; i++)
            {
                uiList.Add(phase1_UI.transform.GetChild(i));
            }




        }
        public void SetSelecteditems()
        {

            selecteditems = uiList[uiIndex].name;
            for (int i = 0; i < items.Count; i++)
            {

                if (items[i].name == selecteditems)
                {

                    currentObject = items[i];
                    previousObject = items[i];
                    previousObject.SetActive(true);
                    previousIndex = i;
                }
            }



        }
        public void UseWater()
        {
            Transform ui = uiList[0];
            uiList[0].GetComponent<Animator>().enabled = false;
            currentObject = items[0];
            previousObject = currentObject;
            GameObject obj = items[0];
            obj.transform.DOLocalRotate(new Vector3(-65, 0, 0), 0.6f, RotateMode.Fast);
            obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            ui.GetComponent<ItemState>().isAdded = true;
            ui.GetComponent<Button>().interactable = false;
            waterParticle.SetActive(true);
            StartCoroutine(ChangeSpeed(currentFill, endFill, duration, currentPitcherFill, endPitcherFill));
            itemsAdded.Add("Water");
            StartCoroutine(UsageMoveOut());
            fakeWater.transform.DOMove(new Vector3(fakeWater.transform.position.x, fakeWater.transform.position.y + 0.1376f, fakeWater.transform.position.z), 2.5f, false);
            isWaterAdded = true;
            Checklist();

        }
        public void UseEgg()
        {
            Transform ui = uiList[2];
            Transform obj = items[2].transform;
            uiList[2].GetComponent<Animator>().enabled = false;
            Transform eggInsides = obj.transform.GetChild(0);
            anim.CrackEgg();
            eggInsides.gameObject.SetActive(true);
            eggInsides.transform.parent = null;
            eggInsides.DOLocalMove(new Vector3(eggInsides.position.x, eggInsides.position.y - 0.5f, eggInsides.position.z), 2f);

            StartCoroutine(UsageMoveOut());
            if (isWaterAdded)
                waterParticleDelay.GetComponent<ParticleSystem>().Play();

            ui.GetComponent<Button>().interactable = false;
            itemsAdded.Add("Egg");

            Checklist();
        }
        public void UseFlour()
        {
            uiList[1].GetComponent<Animator>().enabled = false;
            Transform ui = uiList[1];
            GameObject obj = items[1];

            flourParticle = obj.transform.GetChild(0).GetComponent<ParticleSystem>();

            obj.transform.DOLocalRotate(new Vector3(-65, 0, 0), 1f, RotateMode.Fast);
            flourParticle.Play();

            ui.GetComponent<ItemState>().isAdded = true;
            ui.GetComponent<Button>().interactable = false;
            StartCoroutine(UsageMoveOut());
            itemsAdded.Add("Flour");
            if (isWaterAdded)
                waterParticle.GetComponent<ParticleSystem>().Play();
            Checklist();

        }

        public void UseColoring()
        {

            Transform ui = uiList[uiIndex];
            GameObject obj = items[uiIndex];

            psColoring[uiIndex].Play();


            ui.GetComponent<ItemState>().isAdded = true;
            ui.GetComponent<Button>().interactable = false;
            itemsAdded.Add("Coloring");
            MoveDropsLid();
            StartCoroutine(UsageMoveOut());
            StartCoroutine(ApplyColor(rainbowTexture, color));
            if (isWaterAdded)
                waterParticleDelay.GetComponent<ParticleSystem>().Play();
            obj.transform.GetChild(0).parent = fakeWater.transform;
            Checklist();

        }
        public void UseCockroach()
        {

            Transform ui = uiList[uiIndex];
            GameObject obj = items[uiIndex];

            ui.GetComponent<ItemState>().isAdded = true;
            ui.GetComponent<Button>().interactable = false;
            // itemsAdded.Add("Cockroach");

            
            obj.transform.DOMove(new Vector3(obj.transform.position.x, obj.transform.position.y - 1f, obj.transform.position.z - 0.2f), 3.5f);
            StartCoroutine(ApplyColor(rainbowTexture, color));

            StartCoroutine(SpawnRoaches());
            if (isWaterAdded)
                waterParticleDelay.GetComponent<ParticleSystem>().Play();
            Checklist();
            
            

        }
        public void UseSauce(Transform objTransform)
        {
            sauceAnim.Play("test4");
            CheckIfUsed.isSauceUsed = true;
            for (int i = 4; i < 8; i++)
            {
                phase4_UI.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
        IEnumerator Wait(float duration, GameObject go)
        {
            yield return new WaitForSeconds(duration);
            go.transform.position = go.transform.position + new Vector3(-100,-100,-100);
            go.SetActive(false);
        }
        public void ClearScreen()
        {
            StartCoroutine(MoveOut());
        }
        public void Checklist()
        {
            if (itemsAdded.Contains("Water") && itemsAdded.Contains("Flour") && itemsAdded.Contains("Egg"))
            {
                checkmarkUI.SetActive(true);
            }
        }
        public IEnumerator ControlFill()
        {

            yield return null;
        }
        IEnumerator ChangeSpeed(float currentFill, float endFill, float duration, float currentPitcherFill, float endPitcherFill)
        {
            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                speed = Mathf.Lerp(currentFill, endFill, elapsed / duration);
                speedPitcher = Mathf.Lerp(currentPitcherFill, endPitcherFill, elapsed / duration);
                speedPosition = Mathf.Lerp(currentPitcherY, endPitcherY, elapsed / duration);
                particlePosition = Mathf.Lerp(waterParticleStartPos, waterParticleEndPos, elapsed / duration);
                water.GetComponent<Renderer>().material.SetFloat("FillAmount", speed);
                waterPitcher.GetComponent<Renderer>().material.SetFloat("FillAmount", speedPitcher);
                waterPitcher.transform.localPosition = new Vector3(waterPitcher.transform.localPosition.x, speedPosition, waterPitcher.transform.localPosition.z);
                waterParticle.transform.localPosition = new Vector3(waterParticle.transform.localPosition.x, particlePosition, waterParticle.transform.localPosition.z);
                elapsed += Time.deltaTime;
                yield return null;

            }
            speed = endFill;
            speedPitcher = endPitcherFill;
            speedPosition = endPitcherY;
            particlePosition = waterParticleEndPos;
        }
        public IEnumerator MoveOut()
        {
            if (previousObject != null)
            {


                int indexToReset = previousIndex;

                GameObject obj = previousObject;
                Quaternion prevRotation = obj.transform.rotation;

                if (previousObject != null)
                {
                    previousObject.transform.DOLocalRotate(obj.transform.position, 0.5f);
                    previousObject.transform.DOMove(endPos.position, 1f).OnComplete(() => Disable(obj, indexToReset, prevRotation));
                    if (obj.tag == "Flour")
                    {
                        flourParticle.transform.parent = null;
                        flourParticle.Stop();
                    }
                    yield return new WaitForSeconds(0.5f);

                }
            }
        }
        void Disable(GameObject obj, int index, Quaternion rotation)
        {
            obj.SetActive(false);
            items[index].transform.position = beginPos.transform.position;
            items[index].transform.rotation = rotation;
        }
        void ActivateEmitter(Transform tr)
        {
            tr.GetChild(0).gameObject.SetActive(true);
        }
        public void ReturnIndex(int i)
        {
            uiIndex = i;

        }
        public IEnumerator UsageMoveOut()
        {
            phase1_UI.transform.parent.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(MoveOut());
            yield return new WaitForSeconds(0.5f);
            phase1_UI.transform.parent.gameObject.SetActive(true);
        }
        public void MoveDropsLid()
        {

            items[uiIndex].transform.GetChild(1).transform.DOLocalMove(new Vector3(0, 2.16099977f, 0), 0.2f);
            items[uiIndex].transform.GetChild(1).transform.DOLocalMove(new Vector3(0, 0.537999868f, -1.67399895f), 0.5f);

        }
        IEnumerator ApplyColor(Texture tex, Color col)
        {

            Material materialClone = new Material(noodleMaterial);
            yield return new WaitForSeconds(1f);
            if (tex != null)
            {
                water.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
                materialClone.SetTexture("_BaseMap", tex);
                materialClone.SetColor("_BaseColor", Color.white);
                for (int i = 0; i < obiSolver.transform.childCount; i++)
                {
                    obiSolver.transform.GetChild(i).GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                    finishedNoodles.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                    finishedNoodlesTwo.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                    noodle_animation.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                    boilingNoodles1.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                    boilingNoodles2.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                }
                fakeWater.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
            }
            else
            {
                materialClone.SetColor("_BaseColor", col);
                for (int i = 0; i < obiSolver.transform.childCount; i++)
                {
                    obiSolver.transform.GetChild(i).GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);


                }
                finishedNoodlesTwo.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                finishedNoodles.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                noodle_animation.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                fakeWater.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                boilingNoodles1.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
                boilingNoodles2.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(materialClone);
            }
            water.GetComponent<Renderer>().material.SetColor("LiquidColor", col);
            water.GetComponent<Renderer>().material.SetColor("SurfaceColor", col);
            water.GetComponent<Renderer>().material.SetColor("FresnelColor", col);
        }
        public IEnumerator SpawnRoaches()
        {
            for (int i = 0; i < 15; i++)
            {   

                GameObject cock = Instantiate(cockroach, Cockpos.position, Quaternion.Euler(Random.Range(0,90),Random.Range(0,90), Random.Range(0,90) ));
                cock.SetActive(true);
                cock.AddComponent<Rigidbody>();
                yield return new WaitForSeconds(0.15f);
            }
        }

    }
}