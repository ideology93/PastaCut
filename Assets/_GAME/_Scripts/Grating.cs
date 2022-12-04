using System.Collections;

using System.Collections.Generic;
using Timers;
using UnityEngine;
using DG.Tweening;

namespace NLO
{

    public class Grating : MonoBehaviour

    {
        [SerializeField] Vector3 mOffset;
        [SerializeField] float mZCoord;

        [SerializeField] bool isHeld;
        [SerializeField] Transform target;
        [SerializeField] ParticleSystem cheeseParticle;
        [SerializeField] ParticleSystem carrotParticle;
        // public List<GameObject> cheeses = new List<GameObject>();

        [SerializeField] float wideTimer;
        [SerializeField] VeggieManager veggieManager;
        float spawnTimer = 0;
        Rigidbody rigidBody;


        void Start()
        {
            TimersManager.SetTimer(this, 0.15f, 25, CarrotTimer);
            TimersManager.SetTimer(this, 0.05f, 80, CheeseTimer);
            TimersManager.SetPaused(CarrotTimer, true);
            TimersManager.SetPaused(CheeseTimer, true);

            veggieManager = transform.parent.parent.GetComponent<VeggieManager>();
        }
        void OnMouseDown()
        {
            mZCoord = Camera.main.WorldToScreenPoint(
                gameObject.transform.position).z;
            // Store offset = gameobject world pos - mouse world pos
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Grateable")
                {
                    target = hit.transform;
                }
                isHeld = true;

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

        void OnMouseDrag()
        {

            spawnTimer -= Time.deltaTime;
            transform.position = new Vector3(target.position.x, GetMouseAsWorldPoint().y + mOffset.y, target.position.z);

            string name = transform.name;
            if (name == "Cheese")
            {
                veggieManager.cheeseGrated = true;
                TimersManager.SetPaused(CheeseTimer, false);
                veggieManager.Timer(veggieManager.cheeses, gameObject);
            }
            else if (name == "Carrot")
            {
                veggieManager.carrotGrated = true;
                TimersManager.SetPaused(CarrotTimer, false);
                veggieManager.Timer(veggieManager.carrots, gameObject);
            }



        }
        void OnMouseUp()
        {

            isHeld = false;
            TimersManager.SetPaused(CarrotTimer, true);
            TimersManager.SetPaused(CheeseTimer, true);
        }


        void CarrotTimer()
        {
            GameObject carrot = Instantiate(veggieManager.carrotSlice, veggieManager.spawnLocation.transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
            carrot.SetActive(true);
            carrot.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * 0.8f;


        }
        void CheeseTimer()
        {
            GameObject cheese = Instantiate(veggieManager.cheeseSlice, veggieManager.spawnLocation.transform.position, Quaternion.Euler(90, 0, 0)   ) as GameObject;
            cheese.SetActive(true);

            cheese.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * 0.85f;
            // cheese.transform.DOLocalRotate(new Vector3(0, 0, Random.Range(0, 360f)), 0.1f);
        }


    }
}