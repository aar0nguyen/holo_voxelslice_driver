using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class ImgSlicer : MonoBehaviour
{
    public Camera slicer;
    public GameObject Target;
    public GameObject TargetMesh;
    public Animator anime;

    public bool turn;
    public string anim;
    float animSpeed = 0.0f;
    float animStep = 0.1f;


    float numSlices = 100;
    float sliceCount = 0.0f;


    Renderer rend;
    Bounds bound;
    Vector3 tarSize;
    Vector3 tarCent;
    float maxSize;

    Texture2D tex;

    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();


    private void Start()
    {

        Screen.SetResolution(640, 480, false);

        anime.Play(anim, -1, 0.0f); 
        anime.speed = animSpeed;


        Target.transform.position = new Vector3(0, 0, 0);

        rend = TargetMesh.GetComponent<Renderer>();

        transform.position = new Vector3(0, 2.75f, 0);

        slicer.enabled = true;
        slicer.orthographic = true;

        slicer.nearClipPlane = 0;

        sliceCount = 0;

        StartCoroutine(SliceAndSave());
    }

    private IEnumerator SliceAndSave()
    {
        stopwatch.Start();

        while (turn)
        {

            float rotat = 360.0f / numSlices;
            transform.Rotate(0, rotat, 0);

            yield return new WaitForEndOfFrame();

            saveIMG(sliceCount);

            sliceCount = (sliceCount + 1);

            yield return new WaitForSeconds(0.05f);

            if (sliceCount % 100 == 0 && sliceCount != 0)
            {
                anime.Play(anim, -1, animStep); 
                animStep += 0.1f;
                // Debug.Log(animStep);
                if (anim == "Idle" || animStep >= 1.1)
                {
                    Debug.Log("Render 100 img taken: " + (stopwatch.Elapsed));
                    stopwatch.Reset();
                    EditorApplication.isPlaying = false;
                }
            }

            //Debug.Log("Render 1 img taken: " + (stopwatch.Elapsed));
            //stopwatch.Reset();

        }

    }

    private void saveIMG(float sliceCount__)
    {

        RenderTexture rt = new RenderTexture(64, 64, 16);
        rt.filterMode = FilterMode.Bilinear;
        rt.Create();

        slicer.targetTexture = rt;
        tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        RenderTexture.active = rt;

        slicer.Render();

        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        byte[] byteArray = tex.EncodeToPNG();

        string sliceCountName = sliceCount__.ToString();
        if (sliceCount__ <= 9)
        {
            sliceCountName = "0" + sliceCount__.ToString();
        }

        string folder = "slice_" + anim;

        string subpath = "C:\\pictures_temp\\" + folder + "\\";
        string path = subpath + anim + sliceCountName +".png";

        bool exists = System.IO.Directory.Exists(subpath);

        if (!exists) System.IO.Directory.CreateDirectory(subpath);

        File.WriteAllBytes(path, byteArray);


        slicer.targetTexture = null;
        RenderTexture.active = null;
        Destroy(tex);
        Destroy(rt);
    }
}
