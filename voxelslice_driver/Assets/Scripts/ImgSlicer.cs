using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImgSlicer : MonoBehaviour
{
    public Camera slicer;
    public GameObject Target;
    public GameObject TargetMesh;

    public float rpm;
    public int NumberOfSlices;
    public float camTHICC;
    public float THICC;
    public float camZ;


    Renderer rend;
    Bounds bound;
    Vector3 tarSize;
    Vector3 tarCent;
    float maxSize;




    private void Start()
    {
        Target.transform.position = new Vector3(0, 0, 0);

        rend = TargetMesh.GetComponent<Renderer>();
        bound = rend.bounds;
        tarSize = bound.size;
        tarCent = bound.center;
        transform.position = new Vector3(tarCent.x, tarCent.y, 0);

        slicer.enabled = true;
        slicer.orthographic = true;

        slicer.nearClipPlane = 0;
        slicer.farClipPlane = THICC;

        slicer.transform.position = new Vector3(tarCent.x, tarCent.y, camZ);

        maxSize = Mathf.Max(tarSize.x / 2.0f, tarSize.y / 2.0f);

        slicer.orthographicSize = maxSize + tarSize.z / 15.0f;





    }

    private void Update()
    {
        transform.Rotate(0, rpm * 6f * Time.deltaTime, 0);
    }

}
