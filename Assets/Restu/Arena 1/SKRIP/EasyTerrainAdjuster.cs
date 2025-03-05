using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class EasyTerrainAdjuster : MonoBehaviour
{
    public Terrain terrain;
    public MeshFilter trackMeshFilter;
    public float blendRadius = 5f; // Radius blending untuk transisi halus (dalam indeks heightmap)
    public float roadWidthOffset = 2f; // Offset area luar jalan yang tidak terkena raycast
    public bool smoothTerrain = true; // Opsi untuk smoothing terrain
    [Range(1, 10)] public int smoothIntensity = 5; // Intensitas smoothing (1 = minimal, 10 = maksimal)
    public float heightOffset = 0.1f; // Offset ketinggian untuk menyesuaikan gap

    private TerrainData terrainData;
    private Vector3 terrainPosition;
    private Mesh trackMesh;
    private bool[,] roadMask;

    private void OnValidate()
    {
        if (terrain != null)
        {
            terrainData = terrain.terrainData;
            terrainPosition = terrain.transform.position;
        }
        if (trackMeshFilter != null)
        {
            trackMesh = trackMeshFilter.sharedMesh;
        }
    }

    public void AdjustTerrain()
    {
        if (terrain == null || trackMeshFilter == null)
        {
            Debug.LogError("Terrain atau Track Mesh belum diassign!");
            return;
        }

        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

        roadMask = new bool[heightmapHeight, heightmapWidth];

        float terrainWidth = terrainData.size.x;
        float terrainHeight = terrainData.size.y;
        float terrainLength = terrainData.size.z;

        Vector3[] vertices = trackMesh.vertices;
        Matrix4x4 localToWorld = trackMeshFilter.transform.localToWorldMatrix;

        // Penyesuaian terrain berdasarkan mesh track
        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldVertex = localToWorld.MultiplyPoint3x4(vertex);
            int x = Mathf.RoundToInt((worldVertex.x - terrainPosition.x) / terrainWidth * (heightmapWidth - 1));
            int y = Mathf.RoundToInt((worldVertex.z - terrainPosition.z) / terrainLength * (heightmapHeight - 1));

            if (x >= 0 && x < heightmapWidth && y >= 0 && y < heightmapHeight)
            {
                float targetHeight = (worldVertex.y - terrainPosition.y + heightOffset) / terrainHeight;
                heights[y, x] = targetHeight;
                roadMask[y, x] = true;

                ApplyBlend(heights, x, y, targetHeight, heightmapWidth, heightmapHeight);
            }
        }

        terrainData.SetHeights(0, 0, heights);

        if (smoothTerrain)
        {
            SmoothTerrain();
        }
    }

    private void ApplyBlend(float[,] heights, int centerX, int centerY, float targetHeight, int width, int height)
    {
        int blendSteps = Mathf.RoundToInt(blendRadius);
        for (int i = -blendSteps; i <= blendSteps; i++)
        {
            for (int j = -blendSteps; j <= blendSteps; j++)
            {
                int blendX = centerX + i;
                int blendY = centerY + j;
                if (blendX >= 0 && blendX < width && blendY >= 0 && blendY < height)
                {
                    float distance = Mathf.Sqrt(i * i + j * j);
                    float blendFactor = Mathf.Clamp01(1f - (distance / blendRadius));
                    heights[blendY, blendX] = Mathf.Lerp(heights[blendY, blendX], targetHeight, blendFactor);
                    roadMask[blendY, blendX] = true;
                }
            }
        }
    }

    private void SmoothTerrain()
    {
        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, width, height);

        // Konversi roadWidthOffset dari satuan dunia ke indeks heightmap
        int roadOffsetIndices = Mathf.RoundToInt((roadWidthOffset / terrainData.size.x) * width);

        float[,] tempHeights = (float[,])heights.Clone();

        for (int iteration = 0; iteration < smoothIntensity; iteration++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    // Periksa apakah titik ini cukup jauh dari area jalan
                    if (IsOutsideRoadArea(x, y, width, height, roadOffsetIndices))
                    {
                        tempHeights[y, x] = (
                            heights[y - 1, x] + heights[y + 1, x] +
                            heights[y, x - 1] + heights[y, x + 1]
                        ) / 4f;
                    }
                }
            }
            // Copy hasil smoothing ke heights untuk iterasi selanjutnya
            System.Array.Copy(tempHeights, heights, heights.Length);
        }

        terrainData.SetHeights(0, 0, heights);
    }

    private bool IsOutsideRoadArea(int x, int y, int width, int height, int roadOffsetIndices)
    {
        for (int i = -roadOffsetIndices; i <= roadOffsetIndices; i++)
        {
            for (int j = -roadOffsetIndices; j <= roadOffsetIndices; j++)
            {
                int checkX = Mathf.Clamp(x + i, 0, width - 1);
                int checkY = Mathf.Clamp(y + j, 0, height - 1);
                if (roadMask[checkY, checkX])
                {
                    return false; // Masih dekat area jalan
                }
            }
        }
        return true; // Berada di luar area jalan
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EasyTerrainAdjuster))]
public class EasyTerrainAdjusterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EasyTerrainAdjuster script = (EasyTerrainAdjuster)target;
        if (GUILayout.Button("Adjust Terrain"))
        {
            script.AdjustTerrain();
        }
    }
}
#endif
