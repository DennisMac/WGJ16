using UnityEngine;
using System.Collections;
using Boo.Lang;
using System;

public class ExampleClass : MonoBehaviour
{
    public bool additive = false;
    public enum ActionMode
    {
        rake, cirle, curve, draggingRock,
        flatten
    }
    public ActionMode actionMode = ActionMode.rake;
    Mesh mesh;
    float meshScale = 0.1f;
    const int w = 160;
    const int h = 160;
    MeshFilter mf;
    float[] heights = new float[w * h];
    public static ExampleClass Instance;
    // Use this for initialization
    float heightScale = 0.5f;
    GameObject rockBeingDragged;

    public void ResetHeights()
    {
        for (int i = 0; i < heights.Length; i++)
        {
            heights[i] = 0f;
        }
    }
    void Start()
    {
        Instance = this;
        mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                //heights[j * w + i] = Random.Range(0f,1f);
                vertices.Add(meshScale * new Vector3((float)i, heights[j * w + i], (float)j));
                float x = (float)i / (float)w;
                float y = (float)j / (float)h;
                uvs.Add(new Vector2(x, y));
            }
        }

        List<int> triangles = new List<int>();
        for (int i = 0; i < h - 1; i++)
        {
            for (int j = 0; j < w - 1; j++)
            {
                triangles.Add(j * w + i);
                triangles.Add(j * w + i + 1);
                triangles.Add((j + 1) * w + i);

                triangles.Add((j + 1) * w + i + 1);
                triangles.Add((j + 1) * w + i);
                triangles.Add(j * w + i + 1);
            }
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

    }

    Vector3 downPoint = new Vector3();
    Vector3 upPoint = new Vector3();
    private bool free = true; //done forming sand
    bool hitSand = false;
    void Update()
    {
        switch (actionMode)
        {
            case ActionMode.draggingRock:
                DragRock();
                break;
            case ActionMode.rake:
                Rake();
                break;
            case ActionMode.curve:
                Curve();
                break;
            case ActionMode.flatten:
                Flatten();
                break;
            case ActionMode.cirle:
                Circle();
                break;
        }
    }

    private void Circle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.collider.tag == "sand")
                {
                    int x = (int)(hitInfo.point.x / meshScale);
                    int z = (int)(hitInfo.point.z / meshScale);
                    downPoint.x = x;
                    downPoint.z = z;

                }
                else if (hitInfo.collider.tag == "rock")
                {

                    previousActionMode = actionMode;
                    actionMode = ActionMode.draggingRock;
                    rockBeingDragged = hitInfo.collider.gameObject;
                    rockBeingDragged.GetComponent<MeshCollider>().enabled = false;

                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.collider.tag == "sand")
                {
                    int x = (int)(hitInfo.point.x / meshScale);
                    int z = (int)(hitInfo.point.z / meshScale);
                    upPoint.x = x;
                    upPoint.z = z;
                    StartCoroutine("MakeCircle", 0f);
                    SoundManager.Instance.PlayCircleClip();
                }
            }
        }
    }
                

    private void Curve()
    {
        if (Input.GetMouseButtonUp(0)) SoundManager.Instance.StopPlaying();
        if (Input.GetMouseButtonDown(0))
        {
            hitSand = false;
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.collider.tag == "sand")
                {
                    int x = (int)(hitInfo.point.x / meshScale);
                    int z = (int)(hitInfo.point.z / meshScale);
                    downPoint.x = x;
                    downPoint.z = z;
                    hitSand = true;
                    SoundManager.Instance.PlayCurveClip();
                }
                else if (hitInfo.collider.tag == "rock")
                {

                    previousActionMode = actionMode;
                    actionMode = ActionMode.draggingRock;
                    rockBeingDragged = hitInfo.collider.gameObject;
                    rockBeingDragged.GetComponent<MeshCollider>().enabled = false;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.collider.tag == "sand" && hitSand)
                {
                    int x = (int)(hitInfo.point.x / meshScale);
                    int z = (int)(hitInfo.point.z / meshScale);
                    Vector3 thisHit = new Vector3(x, 0, z);
                    if ((downPoint - thisHit).magnitude < 4f) return;
                    upPoint.x = x;
                    upPoint.z = z;
                    StartCoroutine(MakeCurve());
                }
            }
        }
    }
    private void Flatten()
    {
        if(Input.GetMouseButtonUp(0)) SoundManager.Instance.StopPlaying();
        if (Input.GetMouseButtonDown(0))
        {
            hitSand = false;
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.collider.tag == "sand")
                {
                    int x = (int)(hitInfo.point.x / meshScale);
                    int z = (int)(hitInfo.point.z / meshScale);
                    downPoint.x = x;
                    downPoint.z = z;
                    hitSand = true;
                    SoundManager.Instance.PlayFlattenClip();
                }
                else if (hitInfo.collider.tag == "rock")
                {

                    previousActionMode = actionMode;
                    actionMode = ActionMode.draggingRock;
                    rockBeingDragged = hitInfo.collider.gameObject;
                    rockBeingDragged.GetComponent<MeshCollider>().enabled = false;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.collider.tag == "sand" && hitSand)
                {
                    int x = (int)(hitInfo.point.x / meshScale);
                    int z = (int)(hitInfo.point.z / meshScale);
                    Vector3 thisHit = new Vector3(x, 0, z);
                    if ((downPoint - thisHit).magnitude < 4f) return;
                    upPoint.x = x;
                    upPoint.z = z;
                    StartCoroutine(MakeFlat());
                    
                }
            }
        }
    }

    private void Rake()
    {
        if (free)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hitSand = false;
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    if (hitInfo.collider.tag == "sand")
                    {
                        int x = (int)(hitInfo.point.x / meshScale);
                        int z = (int)(hitInfo.point.z / meshScale);
                        downPoint.x = x;
                        downPoint.z = z;
                        hitSand = true;
                    }
                    else if (hitInfo.collider.tag == "rock")
                    {

                        previousActionMode = actionMode;
                        actionMode = ActionMode.draggingRock;
                        rockBeingDragged = hitInfo.collider.gameObject;
                        rockBeingDragged.GetComponent<MeshCollider>().enabled = false;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    if (hitInfo.collider.tag == "sand" && hitSand)
                    {
                        int x = (int)(hitInfo.point.x / meshScale);
                        int z = (int)(hitInfo.point.z / meshScale);
                        upPoint.x = x;
                        upPoint.z = z;
                        free = false;
                        Cursor.visible = false;
                        timesLeft = 1;
                        //StartCoroutine(SingleSplash());
                        SoundManager.Instance.PlayLineClip();
                    }
                }
            }
        }
    }

    private void DragRock()
    {
        rockBeingDragged.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            rockBeingDragged.transform.position = hitInfo.point + 1f * Vector3.up;
        }
        else
        {
            actionMode = previousActionMode;
            rockBeingDragged.GetComponent<MeshCollider>().enabled = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            actionMode = previousActionMode;
            rockBeingDragged.GetComponent<MeshCollider>().enabled = true;
        }

    }

    int lastUpdate = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        switch (actionMode)
        {
            case ActionMode.rake:
                if (timesLeft-- > 0)
                {
                    StartCoroutine(Raking());
                }
                else
                {
                    free = true;
                    Cursor.visible = true;
                }
                break;
        }

        if (lastUpdate++ < 10) return;
        lastUpdate = 0;
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                //heights[j * w + i] += Random.Range(-0.1f, 0.1f);
                vertices.Add(meshScale * new Vector3((float)i, heights[j * w + i], (float)j));
                float x = 2f * (float)i / (float)w;
                float y = 4f * (float)j / (float)h;
                uvs.Add(new Vector2(x, y));
            }
        }

        List<int> triangles = new List<int>();
        for (int i = 0; i < h - 1; i++)
        {
            for (int j = 0; j < w - 1; j++)
            {
                triangles.Add(j * w + i);
                triangles.Add(j * w + i + 1);
                triangles.Add((j + 1) * w + i);

                triangles.Add((j + 1) * w + i + 1);
                triangles.Add((j + 1) * w + i);
                triangles.Add(j * w + i + 1);
            }
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
    }

    int timesLeft = 0;
    private ActionMode previousActionMode;

    IEnumerator Raking()
    {
        int downX = (int)downPoint.x;
        int upX = (int)upPoint.x;
        int downZ = (int)downPoint.z;
        int upZ = (int)upPoint.z;

        if (downX == upX && downZ == upZ)
        {
            //make a splash instead of a line
        }
        else
        {
            float s = 10;
            float distanceFromLine = 0;
            //float distanceBetweenPoints = Vector2.Distance(new Vector2(downX, downZ), new Vector2(upX, upZ));
            for (int z = 0; z < h; z++)
            {
                for (int x = 0; x < w; x++)
                {

                    distanceFromLine = Mathf.Abs((upZ - downZ) * x - (upX - downX) * z + upX * downZ - upZ * downX)
                        / Mathf.Sqrt((upZ - downZ) * (upZ - downZ) + (upX - downX) * (upX - downX));
                    if (distanceFromLine < s)
                    {
                        float distanceFromUp = Mathf.Sqrt((x - upX) * (x - upX) + (z - upZ) * (z - upZ));
                        float distanceFromDown = Mathf.Sqrt((x - downX) * (x - downX) + (z - downZ) * (z - downZ));
                        float distanceBetweenPoints = Mathf.Sqrt((downX - upX) * (downX - upX) + (downZ - upZ) * (downZ - upZ));

                        if (distanceFromUp * distanceFromUp < s * s + distanceBetweenPoints * distanceBetweenPoints &&
                            distanceFromDown * distanceFromDown < s * s + distanceBetweenPoints * distanceBetweenPoints)
                        {
                            float desiredHeight = heightScale * Mathf.Cos(2f * distanceFromLine);
                            // heights[z * w + x] = Mathf.Lerp(heights[z * w + x], desiredHeight, Time.deltaTime);
                            if (additive)
                            {
                                heights[z * w + x] += (1 - timesLeft) * desiredHeight;// * Time.deltaTime;
                            }
                            else
                            {
                                heights[z * w + x] = (1 - timesLeft) * desiredHeight;
                            }

                        }
                    }
                }
            }
        }
        yield return (0.1f);
    }

    IEnumerator MakeCurve()
    {
        int downX = (int)downPoint.x;
        int upX = (int)upPoint.x;
        int downZ = (int)downPoint.z;
        int upZ = (int)upPoint.z;

        if (downX == upX && downZ == upZ)
        {
            //make a splash instead of a line
        }
        else
        {
            float s = 10;
            float distanceFromLine = 0;
            //float distanceBetweenPoints = Vector2.Distance(new Vector2(downX, downZ), new Vector2(upX, upZ));
            for (int z = 0; z < h; z++)
            {
                for (int x = 0; x < w; x++)
                {

                    distanceFromLine = Mathf.Abs((upZ - downZ) * x - (upX - downX) * z + upX * downZ - upZ * downX)
                        / Mathf.Sqrt((upZ - downZ) * (upZ - downZ) + (upX - downX) * (upX - downX));
                    if (distanceFromLine < s)
                    {
                        float distanceFromUp = Mathf.Sqrt((x - upX) * (x - upX) + (z - upZ) * (z - upZ));
                        float distanceFromDown = Mathf.Sqrt((x - downX) * (x - downX) + (z - downZ) * (z - downZ));
                        float distanceBetweenPoints = Mathf.Sqrt((downX - upX) * (downX - upX) + (downZ - upZ) * (downZ - upZ));

                        if (distanceFromUp * distanceFromUp < s * s + distanceBetweenPoints * distanceBetweenPoints &&
                            distanceFromDown * distanceFromDown < s * s + distanceBetweenPoints * distanceBetweenPoints)
                        {
                            float desiredHeight = heightScale * Mathf.Cos(2f * distanceFromLine);
                            // heights[z * w + x] = Mathf.Lerp(heights[z * w + x], desiredHeight, Time.deltaTime);
                            if (additive)
                            {
                                heights[z * w + x] += desiredHeight;
                            }
                            else
                            {
                                heights[z * w + x] = desiredHeight;
                            }

                        }
                    }
                }
            }
        }
        downPoint = upPoint;
        yield return (0.1f);
    }
    IEnumerator MakeFlat()
    {
        int downX = (int)downPoint.x;
        int upX = (int)upPoint.x;
        int downZ = (int)downPoint.z;
        int upZ = (int)upPoint.z;

        if (downX == upX && downZ == upZ)
        {
            //make a splash instead of a line
        }
        else
        {
            float s = 10;
            float distanceFromLine = 0;
            //float distanceBetweenPoints = Vector2.Distance(new Vector2(downX, downZ), new Vector2(upX, upZ));
            for (int z = 0; z < h; z++)
            {
                for (int x = 0; x < w; x++)
                {

                    distanceFromLine = Mathf.Abs((upZ - downZ) * x - (upX - downX) * z + upX * downZ - upZ * downX)
                        / Mathf.Sqrt((upZ - downZ) * (upZ - downZ) + (upX - downX) * (upX - downX));
                    if (distanceFromLine < s)
                    {
                        float distanceFromUp = Mathf.Sqrt((x - upX) * (x - upX) + (z - upZ) * (z - upZ));
                        float distanceFromDown = Mathf.Sqrt((x - downX) * (x - downX) + (z - downZ) * (z - downZ));
                        float distanceBetweenPoints = Mathf.Sqrt((downX - upX) * (downX - upX) + (downZ - upZ) * (downZ - upZ));

                        if (distanceFromUp * distanceFromUp < s * s + distanceBetweenPoints * distanceBetweenPoints &&
                            distanceFromDown * distanceFromDown < s * s + distanceBetweenPoints * distanceBetweenPoints)
                        {
                            float desiredHeight = heightScale * Mathf.Cos(2f * distanceFromLine);
                            // heights[z * w + x] = Mathf.Lerp(heights[z * w + x], desiredHeight, Time.deltaTime);

                            heights[z * w + x] = 0f;


                        }
                    }
                }
            }
        }

        downPoint = upPoint;
        yield return (0.1f);
    }

    IEnumerator MakeCircle()
    {
        float s = (downPoint-upPoint).magnitude;
        float distanceFromPoint = 0;
        for (int z = 0; z < h; z++)
        {
            for (int x = 0; x < w; x++)
            {
                distanceFromPoint = (downPoint - new Vector3(x, 0, z)).magnitude;
                if (distanceFromPoint < s)
                {
                    float desiredHeight = heightScale * Mathf.Cos(2f * distanceFromPoint);
                    if (additive)
                    {
                        heights[z * w + x] += desiredHeight;
                    }
                    else
                    {
                        heights[z * w + x] = desiredHeight;
                    }
                }
            }
        }
        yield return (0.1f);
    }
    public void SetAdditive(bool value)
    {
        additive = value;
    }
}









