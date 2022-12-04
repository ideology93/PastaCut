using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NLO
{
    public class ShitCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "col")
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            if (other.gameObject.tag == "destroy")
            {
                gameObject.SetActive(false);
            }
            if (other.gameObject.tag == "sticky")
            {
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().mass = 5;
            }
        }
    }
}
