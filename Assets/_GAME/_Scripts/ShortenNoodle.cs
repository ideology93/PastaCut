using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
public class ShortenNoodle : MonoBehaviour
{

    public float _speed = 0;
    ObiRopeCursor cursor;
    ObiRope rope;



    void Start()
    {
        cursor = GetComponentInChildren<ObiRopeCursor>();
        rope = cursor.GetComponent<ObiRope>();

        // cursor = GetComponentInChildren<ObiRopeCursor>();
        // rope = cursor.GetComponent<ObiRope>();
    }

    void FixedUpdate()
    {
        if(_speed < 0)
        cursor.ChangeLength(rope.restLength + _speed);

    }
}