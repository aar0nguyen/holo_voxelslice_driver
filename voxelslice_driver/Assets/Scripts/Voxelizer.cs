using UnityEngine;

public class Voxelizer : MonoBehaviour
{
    public ComputeShader voxelComputeShader;
    public int gridSize = 64; // Grid resolution (NxNxN)
    public float voxelSize = 0.1f;
    private ComputeBuffer voxelBuffer;
    private int[] voxelData;

    public GameObject voxelPrefab; // Prefab for visualizing voxels

    void Start()
    {
        InitializeVoxelGrid();
    }

    void InitializeVoxelGrid()
    {
        int totalVoxels = gridSize * gridSize * gridSize;
        voxelData = new int[totalVoxels];

        voxelBuffer = new ComputeBuffer(totalVoxels, sizeof(int));
        voxelBuffer.SetData(voxelData);

        int kernel = voxelComputeShader.FindKernel("CSVoxelize");

        voxelComputeShader.SetBuffer(kernel, "voxelGrid", voxelBuffer);
        voxelComputeShader.SetInts("gridSize", new int[] { gridSize, gridSize, gridSize });
        voxelComputeShader.SetFloat("voxelSize", voxelSize);

        voxelComputeShader.Dispatch(kernel, gridSize / 8, gridSize / 8, gridSize / 8);

        voxelBuffer.GetData(voxelData);
        VisualizeVoxels();
    }

    void VisualizeVoxels()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    int index = x + gridSize * (y + gridSize * z);
                    if (voxelData[index] == 1) // If voxel is filled
                    {
                        Instantiate(voxelPrefab, new Vector3(x, y, z) * voxelSize, Quaternion.identity);
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        voxelBuffer.Release();
    }
}
