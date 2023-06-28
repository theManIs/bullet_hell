using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerlinNoiseGenerator
{
    public List<NoiseBlock> noiseBlockList = new List<NoiseBlock>();
    public int ConstantWidth = 1;
    public int ConstantHeight = 1;
    public float ConstantScale = 0.1f;
    public int ConstantDomain = 1;
    public int[] ConstantDomainArray = Array.Empty<int>();

    public PerlinNoiseGenerator(int width, int height, float scale, int domain)
    {
        ConstantWidth = width;
        ConstantHeight = height;
        ConstantScale = scale;
        ConstantDomain = domain;
    }    
    
    public PerlinNoiseGenerator(float scale, int[] domain, int width, int height)
    {
        ConstantWidth = width;
        ConstantHeight = height;
        ConstantScale = scale;
        ConstantDomainArray = domain;
    }

    private float[][] GenerateNoise(int width, int height, int x, int y, float scale)
    {
        int xOffset = width * x;
        int yOffset = height * y;
        float[][] noiseMap = new float[width][];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                noiseMap[i][j] = Mathf.PerlinNoise(j * scale + xOffset, i * scale + yOffset);
            }
        }

        return noiseMap;
    }

    private NoiseBlock GetNoiseBlock(int xPos, int yPos)
    {
        int x = xPos / ConstantWidth;
        int y = yPos / ConstantHeight;
        //int xMax = 0;
        //int yMax = 0;
        //int xMin = 0;
        //int yMin = 0;

        foreach (NoiseBlock noiseBlock in noiseBlockList)
        {
            foreach (NoiseBlock block in noiseBlockList)
            {
                if (block.x == x && block.y == y)
                {
                    return noiseBlock;
                }

                //xMax = block.x > xMax ? block.x : xMax;
                //xMin = xMin > block.x ? block.x : xMin;
                //yMax = block.y > yMax ? block.y : yMax;
                //yMin = yMin > block.y ? block.y : yMin;
            }
        }

        return CreateNoiseBlock(ConstantWidth, ConstantHeight, x, y, ConstantScale);

        //throw new Exception("No noise block found");
    }

    private float GetCellValue(float xPos, float yPos)
    {
        int width = (int)xPos % ConstantWidth;
        int height = (int)yPos % ConstantHeight;
        NoiseBlock noiseBlock = GetNoiseBlock((int)xPos, (int)yPos);

        return noiseBlock.noiseList[height][width];
    }

    private NoiseBlock CreateNoiseBlock(int width, int height, int x, int y, float scale)
    {
        return new NoiseBlock()
        {
            noiseList = GenerateNoise(width, height, x, y, scale),
            width = width,
            height = height,
            x = x,
            y = y
        };
    }

    private float GetDirectCellValue(float xPos, float yPos, float scale, int width, int height)
    {
        //Debug.Log(xPos + " " + yPos + " " + scale + " " + width + " " + height);
        float f = Mathf.PerlinNoise((xPos + width) * scale, (yPos + height) * scale);
        return f;
    }

    private int PickFromDomainInt(float value, int domain)
    {
        return Mathf.FloorToInt(Mathf.Clamp(value, 0, domain - Int32.MinValue) * domain);
    }

    private int PickFromDomainArrayInt(float value, int[] domain)
    {
        int weight = domain.Sum();
        int pickValue = Mathf.Clamp(Convert.ToInt32(value * weight), 0, weight);
        int accumulator = 0;

        for (int i = 0; i < domain.Length; i++)
        {
            accumulator += domain[i];

            if (accumulator >= pickValue)
            {
                return i;
            }
        }

        throw new Exception("Can't find any value in domain");
    }

    public int GetIndex(float xPos, float yPos, int domain)
    {
        return PickFromDomainInt(GetCellValue(xPos, yPos), domain);
    }

    private void IncreaseNoiseRange(float xPos, float yPos)
    {
        if (Mathf.Abs(xPos) > ConstantWidth || Mathf.Abs(yPos) > ConstantHeight)
        {
            ConstantWidth *= 10;
            ConstantHeight *= 10;
        }
    }

    public int GetIndexWeighted(float xPos, float yPos)
    {
        IncreaseNoiseRange(xPos, yPos);

        return PickFromDomainArrayInt(
            GetDirectCellValue(xPos, yPos, ConstantScale, ConstantWidth, ConstantHeight), 
            ConstantDomainArray
        );
    }


    public struct NoiseBlock
    {
        public float[][] noiseList;
        public int width;
        public int height;
        public int x;
        public int y;
    }
}
