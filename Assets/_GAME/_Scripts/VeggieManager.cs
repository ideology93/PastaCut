using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NLO
{
    public class VeggieManager : MonoBehaviour
    {
        [SerializeField] Vector3 mOffset;
        [SerializeField] float mZCoord;
        public Grating grating;
        [SerializeField] private GameObject grater;
        [SerializeField] Button cheeseUI, carrotUI;
        [SerializeField] private float time = 5;
        [SerializeField] public float cheeseTimer = 4, carrotTimer = 4;
        public List<GameObject> cheeses = new List<GameObject>();
        public List<GameObject> carrots = new List<GameObject>();
        public GameObject cheeseSlice, carrotSlice, tomatoSlice;
        public Transform spawnLocation;
        public bool cheeseGrated, carrotGrated;
        public GameObject items;
        public GameObject currentItem;

        void Update()
        {
            if (currentItem != null)
            {
                CastRay();
                if (cheeseTimer <= 0 && carrotTimer <= 0)
                {
                    grater.SetActive(false);
                }
            }

        }

        public void CastRay()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {

                    if (currentItem != null)
                    {
                        Quaternion randomRotation = Quaternion.Euler(-90, Random.Range(0, 360), 0);
                        GameObject obj = Instantiate(currentItem, hit.point, randomRotation);
                        if (CheckIfUsed.isSauceUsed && hit.transform.tag == "sphere")
                        {
                            obj.transform.localPosition = obj.transform.localPosition + new Vector3(0, 0.01f, 0);
                        }
                        else if (hit.transform.tag != "sphere" && CheckIfUsed.isSauceUsed)
                        {
                            Debug.Log("Sauce was used");
                            obj.transform.localPosition = obj.transform.localPosition + new Vector3(0, 0.03f, 0);
                        }
                        else
                        {
                            Debug.Log("hit a flat surface with no sauce used");
                        }

                        // if (obj.GetComponent<Rigidbody>() == null)
                        //     obj.AddComponent<Rigidbody>();
                        obj.SetActive(true);
                    }
                }
            }
        }
        private Vector3 GetMouseAsWorldPoint()

        {

            // Pixel coordinates of mouse (x,y) 
            Vector3 mousePoint = Input.mousePosition;
            // z coordinate of game object on screen
            mousePoint.z = mZCoord;
            // Convert it to world points
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }
        public void Timer(List<GameObject> name, GameObject toggle)
        {

            if (toggle.name == "Cheese")
            {
                time = cheeseTimer;
                if (time > 0)
                {
                    cheeseTimer -= Time.deltaTime;

                }
                if (cheeseTimer < 3 && cheeseTimer > 2 && !name[1].activeSelf)
                {
                    name[1].SetActive(true);
                    name[0].SetActive(false);
                }
                if (cheeseTimer < 2 && cheeseTimer > 1 && !name[2].activeSelf)
                {
                    name[2].SetActive(true);
                    name[1].SetActive(false);
                }
                if (cheeseTimer < 1 && cheeseTimer > 0 && !name[3].activeSelf)
                {
                    name[3].SetActive(true);
                    name[2].SetActive(false);
                }

                if (cheeseTimer <= 0)
                {
                    toggle.SetActive(false);
                    cheeseUI.interactable = false;
                }
            }
            else
            {

                time = carrotTimer;
                if (time > 0)
                {
                    carrotTimer -= Time.deltaTime;

                }
                if (carrotTimer < 3 && carrotTimer > 2 && !name[1].activeSelf)
                {
                    name[1].SetActive(true);
                    name[0].SetActive(false);
                }
                if (carrotTimer < 2 && carrotTimer > 1 && !name[2].activeSelf)
                {
                    name[2].SetActive(true);
                    name[1].SetActive(false);
                }
                if (carrotTimer < 1 && carrotTimer > 0 && !name[3].activeSelf)
                {
                    name[3].SetActive(true);
                    name[2].SetActive(false);
                }

                if (carrotTimer <= 0)
                {
                    toggle.SetActive(false);
                    carrotUI.interactable = false;
                }
            }

        }

        public void ClearScreen()
        {
            for (int i = 0; i < items.transform.childCount; i++)
            {
                items.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }
}