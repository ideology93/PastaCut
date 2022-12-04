using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NLO
{
    public class Raycaster : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;
        [SerializeField] ItemUsage item;
        private Ray ray;
        void Start()
        {
            item = GetComponent<ItemUsage>();
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CastRay();
            }
        }
        public void CastRay()
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.tag == "Water")
                {
                    item.UseWater();
                }
                if (hit.transform.tag == "Drops")
                {
                    item.UseColoring();
                }
                if (hit.transform.tag == "Egg")
                {
                    item.UseEgg();
                }
                if (hit.transform.tag == "Flour")
                {
                    item.UseFlour();
                }
                if (hit.transform.tag == "Cockroach")
                {
                    item.UseCockroach();
                }
                if (hit.transform.tag == "Sauce")
                {
                    item.UseSauce(hit.transform);
                }

            }
        }

    }
}