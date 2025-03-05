using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    [SerializeField] int width = 512;
    [SerializeField] int length = 512;
    [SerializeField] int height = 10;

    [SerializeField] float baseScale = 100f; // 큰 언덕 Perlin Nosie
    [SerializeField] float deltaScale = 10f; // 작은 언덕 Perlin Noise
    [SerializeField] float roadScale = 50f;
    [SerializeField] float roadTreshold = 0.3f; // 길이 생성될 Nosie Thresold
    [SerializeField] float flatThresold = 0.5f; // 평지 지역 Thresold;
    [SerializeField] float ridgeSharpness = 1.5f; // 능선 강조 ( 1.0 ~ 높을 수록 뾰족하게)


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
                // 기본 지형 생성
                float baseHeight = Mathf.PerlinNoise(x / baseScale, y / baseScale);
                float deltaHeight = Mathf.PerlinNoise(x / deltaScale, y / deltaScale) * 0.2f;

                // 능선 강조
                float ridgeHeight = Mathf.PerlinNoise(baseHeight, ridgeSharpness);

                // 추가할 Mountain Noise
                float mountainNoise = Mathf.PerlinNoise(x / 150f, y / 150f) * 2.0f;
                float mountainMask = Mathf.PerlinNoise(x / 300f, y / 300f); // 마스크

                if (mountainMask > 0.6f) // 산이 특정 지역에서만 형성됨
                {
                    mountainNoise *= mountainMask; // 마스크를 곱해서 점진적으로 높이 증가
                }
                else
                {
                    mountainNoise = 0; // 나머지 지역은 산 없음
                }

                float finalHeight = ridgeHeight + deltaHeight+ mountainNoise;


                // 길 생성
                float roadNoise = Mathf.PerlinNoise(x / roadScale, y / roadScale); 
                if(roadNoise < roadTreshold)
                {
                    finalHeight = Mathf.Lerp(finalHeight, 0.2f, roadNoise / roadTreshold);
                }

                // 평지 생성
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
