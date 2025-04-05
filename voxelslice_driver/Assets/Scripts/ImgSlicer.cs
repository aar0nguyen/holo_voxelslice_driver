using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ImgSlicer : MonoBehaviour
{
    public Camera slicer;
    public GameObject Target;
    public GameObject TargetMesh;
    public RenderTexture rt;

    public bool Turn;
    public float NumberOfSlices;
    public float CamFar;
    public float camZ;

    float sliceCount;


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
        slicer.farClipPlane = CamFar;

        slicer.transform.position = new Vector3(tarCent.x, tarCent.y, camZ);

        maxSize = Mathf.Max(tarSize.x / 2.0f, tarSize.y / 2.0f);

        // For 16x16
        //slicer.orthographicSize = 0.7f;
        slicer.orthographicSize = maxSize + tarSize.z / 15.0f;

        sliceCount = 0;
    }

    private void Update()
    {

        if (Turn)
        {
            transform.Rotate(0, 360.0f / NumberOfSlices % 360.0f, 0);
        }
        savePNG();
        Debug.Log(sliceCount);
        sliceCount = (sliceCount + 1) % NumberOfSlices;
    }


    private void savePNG()
    {
        string filename = "slice" + sliceCount.ToString();

        slicer.targetTexture = rt;
        slicer.Render();
        RenderTexture.active = rt;

        var tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        byte[] byteArray = tex.EncodeToPNG();

        // Editor become read-only on runtime if running application
        // var path = "Assets/Textures/Rendered Textures/" + filename + ".png";
        // var path = "C:\\Users\\aaron\\OneDrive\\Pictures\\slices\\" + filename + ".png";
        
        // pi 4
        var path = "/home/rpi4/Pictures/slices" + filename + ".png";

        File.WriteAllBytes(path, byteArray);

        DestroyImmediate(tex);
    }

}
