using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using DG.Tweening;

[RequireComponent(typeof(ObiRope))]
public class RopeSweepCut : MonoBehaviour
{
    public GameObject knife;
    [SerializeField] Vector3 mOffsetStart, mOffsetEnd;
    [SerializeField] float mZCoordStart, mZCoordEnd;
    public Camera cam;
    public float distance;
    ObiRope rope;
    LineRenderer lineRenderer;
    Vector3 cutStartPosition;
    public int cuts = 0;
    public GameFlow flow;
    private DestroyTorn ds;
    public float sliceTimer;
    public bool isCut;
    Vector3 startCutPosition, endCutPosition;
    public GameObject cubeStart, cubeEnd;
    public Transform s, e;



    private void Awake()
    {
        rope = GetComponent<ObiRope>();
        flow = FindObjectOfType<GameManager>().GetComponent<GameFlow>();
        AddMouseLine();
    }
    void Start()
    {
        cubeStart = GameObject.Find("StartKnife");
        cubeEnd = GameObject.Find("EndKnife");
        s = cubeStart.transform;
        e = cubeEnd.transform;



    }
    private void OnDestroy()
    {
        DeleteMouseLine();
    }

    private void AddMouseLine()
    {
        GameObject line = new GameObject("Mouse Line");
        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.005f;
        lineRenderer.endWidth = 0.005f;
        lineRenderer.numCapVertices = 2;
        lineRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.sharedMaterial.color = Color.white;
        lineRenderer.enabled = false;
    }

    private void DeleteMouseLine()
    {
        if (lineRenderer != null)
            Destroy(lineRenderer.gameObject);
    }

    private void LateUpdate()
    {
        // do nothing if we don't have a camera to cut from.
        if (cam == null) return;

        // process user input and cut the rope if necessary.
        ProcessInput();
    }
    private Vector3 GetMouseAsWorldPoint()

    {
        // Pixel coordinates of mouse (x,y) 
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoordStart;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    /**
     * Very simple mouse-based input. Not ideal for multitouch screens as it only supports one finger, though.
     */
    private void ProcessInput()
    {
        // When the user clicks the mouse, start a line cut:
        if (Input.GetMouseButtonDown(0))
        {
            mZCoordStart = Camera.main.WorldToScreenPoint(
            cubeStart.transform.position).z;
            // Store offset = gameobject world pos - mouse world pos

            cubeStart.transform.position = new Vector3(cubeStart.transform.position.x, GetMouseAsWorldPoint().y, GetMouseAsWorldPoint().z);
            startCutPosition.x = s.position.x;
            cutStartPosition = Input.mousePosition;
            lineRenderer.SetPosition(0, cam.ScreenToWorldPoint(new Vector3(cutStartPosition.x, cutStartPosition.y, 0.45f)));
            lineRenderer.enabled = true;
        }

        if (lineRenderer.enabled)
            lineRenderer.SetPosition(1, cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.45f)));

        // When the user lifts the mouse, proceed to cut.
        if (Input.GetMouseButtonUp(0))
        {
            mZCoordStart = Camera.main.WorldToScreenPoint(
            cubeEnd.transform.position).z;
            // Store offset = gameobject world pos - mouse world pos

            endCutPosition.x = e.position.x;
            cubeEnd.transform.position = new Vector3(cubeEnd.transform.position.x, GetMouseAsWorldPoint().y, GetMouseAsWorldPoint().z);
          
            knife.SetActive(true);
            AnimateKnife(cubeStart.transform.position, cubeEnd.transform.position);
            ScreenSpaceCut(cutStartPosition, Input.mousePosition);
            lineRenderer.enabled = false;
        }
    }


    /**
     * Cuts the rope using a line segment, expressed in screen-space.
     */
    private void ScreenSpaceCut(Vector2 lineStart, Vector2 lineEnd)
    {
        // keep track of whether the rope was cut or not.
        bool cut = false;

        // iterate over all elements and test them for intersection with the line:
        for (int i = 0; i < rope.elements.Count; ++i)
        {

            // project the both ends of the element to screen space.
            Vector3 screenPos1 = cam.WorldToScreenPoint(rope.solver.positions[rope.elements[i].particle1]);
            Vector3 screenPos2 = cam.WorldToScreenPoint(rope.solver.positions[rope.elements[i].particle2]);

            // test if there's an intersection:
            if (SegmentSegmentIntersection(screenPos1, screenPos2, lineStart, lineEnd, out float r, out float s))
            {
                cut = true;
                rope.Tear(rope.elements[i]);
                cuts++;
                if (cuts == 6)
                {
                    flow.EndPhaseTwo();
                }
            }
        }

        // If the rope was cut at any point, rebuilt constraints:
        if (cut) rope.RebuildConstraintsFromElements();

    }
    private void AnimateKnife(Vector3 start, Vector3 end)
    {

        start.x = knife.transform.position.x;
        end.x = knife.transform.position.x;
        distance = start.z - end.z;

        if (distance > -0.2)
        {
          
            start.z -= 0.05f;
            end.z += 0.1f;

        }
        knife.transform.position = start;
        knife.transform.DOLocalMove(end, 1f, false).OnComplete(() => DisableKnife());
    }
    void DisableKnife()
    {
        knife.SetActive(false);
    }

    /**
     * line segment 1 is AB = A+r(B-A)
     * line segment 2 is CD = C+s(D-C)
     * if they intesect, then A+r(B-A) = C+s(D-C), solving for r and s gives the formula below.
     * If both r and s are in the 0,1 range, it meant the segments intersect.
     */
    private bool SegmentSegmentIntersection(Vector2 A, Vector2 B, Vector2 C, Vector2 D, out float r, out float s)
    {
        float denom = (B.x - A.x) * (D.y - C.y) - (B.y - A.y) * (D.x - C.x);
        float rNum = (A.y - C.y) * (D.x - C.x) - (A.x - C.x) * (D.y - C.y);
        float sNum = (A.y - C.y) * (B.x - A.x) - (A.x - C.x) * (B.y - A.y);

        if (Mathf.Approximately(rNum, 0) || Mathf.Approximately(denom, 0))
        { r = -1; s = -1; return false; }

        r = rNum / denom;
        s = sNum / denom;

        return (r >= 0 && r <= 1 && s >= 0 && s <= 1);
    }
}
