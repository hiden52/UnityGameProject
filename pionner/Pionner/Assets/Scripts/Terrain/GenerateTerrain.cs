using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    [SerializeField] int width = 512;
    [SerializeField] int length = 512;
    [SerializeField] int height = 10;

    [SerializeField] float baseScale = 100f; // ū ��� Perlin Nosie
    [SerializeField] float deltaScale = 10f; // ���� ��� Perlin Noise
    [SerializeField] float roadScale = 50f;
    [SerializeField] float roadTreshold = 0.3f; // ���� ������ Nosie Thresold
    [SerializeField] float flatThresold = 0.5f; // ���� ���� Thresold;
    [SerializeField] float ridgeSharpness = 1.5f; // �ɼ� ���� ( 1.0 ~ ���� ���� �����ϰ�)


    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
    }

    float[,] GenerateHeights(int res)
    {
        float[,] heights = new float[res, res];

        for (int x = 0; x< res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                // �⺻ ���� ����
                float baseHeight = Mathf.PerlinNoise(x / baseScale, y / baseScale);
                float deltaHeight = Mathf.PerlinNoise(x / deltaScale, y / deltaScale) * 0.2f;

                // �ɼ� ����
                float ridgeHeight = Mathf.PerlinNoise(baseHeight, ridgeSharpness);

                // �߰��� Mountain Noise
                float mountainNoise = Mathf.PerlinNoise(x / 150f, y / 150f) * 2.0f;
                float mountainMask = Mathf.PerlinNoise(x / 300f, y / 300f); // ����ũ

                if (mountainMask > 0.6f) // ���� Ư�� ���������� ������
                {
                    mountainNoise *= mountainMask; // ����ũ�� ���ؼ� ���������� ���� ����
                }
                else
                {
                    mountainNoise = 0; // ������ ������ �� ����
                }

                float finalHeight = ridgeHeight + deltaHeight+ mountainNoise;


                // �� ����
                float roadNoise = Mathf.PerlinNoise(x / roadScale, y / roadScale); 
                if(roadNoise < roadTreshold)
                {
                    finalHeight = Mathf.Lerp(finalHeight, 0.2f, roadNoise / roadTreshold);
                }

                // ���� ����
                float flatNoise = Mathf.PerlinNoise(x / (baseScale * 2), y / (baseScale * 2));
                if(flatNoise > flatThresold)
                {
                    finalHeight = Mathf.Lerp(finalHeight, 0.1f, 0.5f);
                }

                

                heights[x, y] = finalHeight;
            }
        }

        return heights;
    }

    TerrainData GenerateTerrainData (TerrainData terrainData)
    {
        int resolution = 513;
        terrainData.heightmapResolution = resolution;
        terrainData.size = new Vector3(width, height, length);
        terrainData.SetHeights(0, 0, GenerateHeights(resolution));

        return terrainData;

    }
}
