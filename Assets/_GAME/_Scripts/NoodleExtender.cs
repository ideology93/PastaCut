using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
public class NoodleExtender : MonoBehaviour
{

    [SerializeField]public float _speed = 0.001f;
    ObiRopeCursor cursor;
    ObiRope rope;
    [SerializeField] ObiRope[] ropes;
    [SerializeField] ObiRopeCursor[] cursors;
    

    void Start()
    {
		ropes = new ObiRope[transform.childCount];
		cursors = new ObiRopeCursor[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
	
			cursors[i] = transform.GetChild(i).GetComponentInChildren<ObiRopeCursor>();
			ropes[i] = transform.GetChild(i).GetComponent<ObiRope>();
        }
        // cursor = GetComponentInChildren<ObiRopeCursor>();
        // rope = cursor.GetComponent<ObiRope>();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < ropes.Length; i++)	
        {
            cursors[i].ChangeLength(ropes[i].restLength + _speed);
        }	

    }
}