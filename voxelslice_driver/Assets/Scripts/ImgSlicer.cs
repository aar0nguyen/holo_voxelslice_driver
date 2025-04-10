using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImgSlicer : MonoBehaviour
{
    public Camera slicer;
    public GameObject Target;
    public GameObject TargetMesh;
    public RenderTexture rt;

    public bool Turn;
    float NumberOfSlices = 100;
    float sliceCount;

    Renderer rend;
    Bounds bound;
    Vector3 tarSize;
    Vector3 tarCent;
    float maxSize;

    Texture2D tex;


    private void Start()
    {
        Screen.SetResolution(640, 480, false);

        Target.transform.position = new Vector3(0, 0, 0);

        rend = TargetMesh.GetComponent<Renderer>();
        bound = rend.bounds;
        tarSize = bound.size;
        tarCent = bound.center;
        transform.position = new Vector3(0, 2.75f, 0);

        slicer.enabled = true;
        slicer.orthographic = true;

        slicer.nearClipPlane = 0;

        // slicer.transform.position = new Vector3(tarCent.x, tarCent.y);
        // maxSize = Mathf.Max(tarSize.x / 2.0f, tarSize.y / 2.0f);
        // For 16x16
        //slicer.orthographicSize = 0.7f;
        // slicer.orthographicSize = maxSize + tarSize.z / 15.0f;

        sliceCount = 0;

        StartCoroutine(SliceAndSave());


    }

    private IEnumerator SliceAndSave()
    {
        while (sliceCount < NumberOfSlices)
        {
            if (Turn)
            {
                transform.Rotate(0, 360.0f / NumberOfSlices, 0);
            }

            yield return new WaitForEndOfFrame(); // Let Unity finish rendering this frame

            saveIMG(sliceCount);

            sliceCount = (sliceCount + 1) % NumberOfSlices;

            yield return new WaitForSeconds(0.05f); // Add a slight delay if needed to reduce CPU/GPU load
        }

    }


    private void saveIMG(float sliceCount__)
    {
        slicer.targetTexture = rt;
        slicer.Render();
        RenderTexture.active = rt;

        tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        byte[] byteArray = ImageConversion.EncodeToJPG(tex, quality: 70); // quality 0–100
        
        // Editor become read-only on runtime if running application
        // var path = "Assets/Textures/Rendered Textures/" + filename + ".png";
        string path = "C:\\Users\\aaron\\OneDrive\\Pictures\\slices\\slice" + sliceCount__.ToString() +".png";

        // pi 4
        // string path = $"/home/rpi4/Pictures/slices/slice" + sliceCount__.ToString() + ".png";
        File.WriteAllBytes(path, byteArray);

        slicer.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(tex);

    }

}
