using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SplitToPixels : MonoBehaviour
{
    public float CrumbleSpeed = 5;
    public float CrumbleLuft = .2f;

    private Dictionary<Transform, Vector2> _movingPoints = new Dictionary<Transform, Vector2>();

    // Start is called before the first frame update
    public void SplitByPixel()
    {
        Vector3 rootPoint = transform.position;

        GameObject hostGm = new GameObject("HostSandEffect");
        Vector3 sandPoint = transform.position;
        hostGm.transform.parent = transform;
        hostGm.transform.position += rootPoint;
        //hostGm.transform.localPosition = Vector3.zero;
        //Debug.Log(hostGm.transform.localPosition + " " + hostGm.transform.position);
        //Debug.Log(hostGm.transform.TransformPoint(rootPoint) + " " + rootPoint);

        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        Sprite initialSprite = sp.sprite;
        Texture2D initialTexture = initialSprite.texture;

        float unityPointToPixelX = initialTexture.width / sp.bounds.size.x;
        float unityPointToPixelY = initialTexture.height / sp.bounds.size.y;
        Vector3 ext = sp.bounds.extents;


        //Texture2D txtr = new Texture2D(initialTexture.width, initialTexture.height);
        //Sprite spr2 = Sprite.Create(txtr, new Rect(0, 0, txtr.width, txtr.height), Vector2.zero);
        //SpriteRenderer sp2 = hostGm.AddComponent<SpriteRenderer>();
        //sp2.sprite = initialSprite;

        for (int i = 0; i < initialTexture.width; i++)
        {
            for (int k = 0; k < initialTexture.height; k++)
            {
                Color initColor = initialTexture.GetPixel(i, k);

                if (initColor.a > 0 && Random.value > .95f)
                {
                    GameObject grainOfSand = new GameObject(i + "x" + k);
                    grainOfSand.transform.parent = hostGm.transform;
                    //grainOfSand.transform.localPosition = Vector3.zero;
                    grainOfSand.transform.position = new Vector3(i / unityPointToPixelX - ext.x, k / unityPointToPixelY - ext.y);
                    grainOfSand.transform.position += rootPoint;

                    SpriteRenderer grainSr = grainOfSand.AddComponent<SpriteRenderer>();
                    Texture2D onePx = new Texture2D(1, 1);
                    Sprite onePxSpr = Sprite.Create(onePx, new Rect(0, 0, 1, 1), Vector2.zero);
                    grainSr.sprite = onePxSpr;

                    onePx.SetPixel(1, 1, initialTexture.GetPixel(i, k));
                    onePx.Apply();
                }
            }
        }

        sp.enabled = false;
    }

    public void ToAshes()
    {
        SpriteRenderer picSp = GetComponent<SpriteRenderer>();
        Sprite picSpr = picSp.sprite;
        Texture2D picTxr = picSpr.texture;
        Vector3 picExt = picSp.bounds.extents;
        Vector2 shiftY = new Vector2(picExt.x, picExt.y + CrumbleLuft);

        float ptpx = picSp.bounds.size.x / picTxr.width;
        float ptpy = picSp.bounds.size.y / picTxr.height;
        Vector2 ptpv2 = new Vector2(ptpx, ptpy);

        Transform host = transform.Find("HostSandEffect");
        List<Vector2> points = new List<Vector2>();

        //Debug.Log(host.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x);
        Debug.Log(host.childCount);
        // EditorApplication.isPaused = true;

        for (int i = 0, k = (int)Mathf.Floor(host.childCount / 2f), j = k + 1; i < host.childCount; i++)
        {
            Transform grain = host.GetChild(i);

            /*if (i % 2 == 0)
            {
                grain = k >= 0 ? host.GetChild(k) : host.GetChild(j);
                k--;
            }
            else
            {
                grain = j < host.childCount ? host.GetChild(j) : host.GetChild(k);
                j++;
            }*/

            /*if (Random.value > .9f)
            {*/
                string[] xyStrings = grain.gameObject.name.Split("x");
                int x = Convert.ToInt32(xyStrings[0]);
                Vector2 newPosition = DownTheGrainPath(grain, x, points, picTxr.height);

                if (newPosition != Vector2.zero)
                {
                    //grain.transform.localPosition = newPosition * ptpv2 - shiftY;
                    if (!_movingPoints.ContainsKey(grain.transform))
                    {
                        _movingPoints.Add(grain.transform, newPosition * ptpv2 - shiftY);
                    }
                    
                }
                // else
                // {
                //     _movingPoints.Add(grain.transform, new Vector2(x, 0) * ptpv2 - shiftY);
                // }
            /*}
            else
            {
                //_movingPoints.Add(grain.transform, new Vector2(Convert.ToSingle(grain.gameObject.name.Split("x")[0]), 0) * ptpv2 - shiftY);
                grain.gameObject.SetActive(false);
            }*/

            //Debug.Log(newPosition + " " + newPosition * ptpv2);

            /*int xPx = (int)(grain.localPosition.x / ptpx);
            int yPx = (int)(grain.localPosition.y / ptpy);

            List<Vector3> xPoints = points.FindAll(v3 => v3.x == (int)(grain.localPosition.x / ptpx));

            if (points.Count == 0 || xPoints.Count == 0)
            {
                grain.transform.localPosition = new Vector3(grain.localPosition.x, -picExt.y);
                points.Add(new Vector3((int)(grain.localPosition.x / ptpx), 0));
            } 
            else
            {
                xPoints.Sort((a, b) => (int)(a.y - b.y));
                Vector3 lastFilledPoint = xPoints[0];
                Vector3 xRight = Vector3.zero;
                //Vector3 xRight = points.Find(v3 => (int)v3.x == (int)(lastFilledPoint.x + 1) && (int)v3.y == (int)lastFilledPoint.y);
                //Vector3 xLeft = points.Find(v3 => (int)v3.x == (int)(lastFilledPoint.x - 1) && (int)v3.y == (int)lastFilledPoint.y);

                foreach (Vector3 foo in points)
                {

                    Debug.Log(foo.x + " " + (lastFilledPoint.x - 1) + " " + (foo.x == lastFilledPoint.x - 1));
                    if (foo.x == lastFilledPoint.x - 1 && foo.y == lastFilledPoint.y)
                    {
                        xRight = foo;
                    }
                }
                
                Debug.Log(lastFilledPoint + " " + xRight + " " + (lastFilledPoint.x - 1) + " " + lastFilledPoint.y);

            } */         
        }

        //foreach (Vector3 foo in points)
        //{
        //    Debug.Log(foo.x + " " + foo.y);
        //}

    }

    public Vector2 DownTheGrainPath(Transform grain, int x, List<Vector2> sand, int stepLimit)
    {
        //string[] xyStrings = grain.gameObject.name.Split("x");
        //int x = Convert.ToInt32(xyStrings[0]);
        // int y = Convert.ToInt32(xyStrings[1]);
        /*Debug.Log(x + " " + xyStrings[1] + " " + stepLimit);  */  
        for (int y = 0; y < stepLimit; y++)
        {
            Vector2 center = sand.Find(v2 => (int)v2.x == x && (int)v2.y == y);
            Vector2 right = sand.Find(v2 => (int)v2.x == x + 1 && (int)v2.y == y);
            Vector2 left = sand.Find(v2 => (int)v2.x == x - 1 && (int)v2.y == y);
            /*Debug.Log(center + " " + right + "  " + left); */

            //string debugString = "";
            //sand.ForEach(v2 => debugString += v2.x +  " " + v2 + "\n");
            //DebugEx.LogList(sand);

            if (center != Vector2.zero && right != Vector2.zero && left != Vector2.zero)
            {
                continue;
            }

            if (center == Vector2.zero)
            {
                Vector2 nCenter = new Vector2(x, y);
                sand.Add(nCenter);

                return nCenter;
            }

            if (right == Vector2.zero)
            {
                //Vector2 nRight = new Vector2(x + 1, y);
                //sand.Add(nRight);

                return DownTheGrainPath(grain, x + 1, sand, stepLimit);
            }

            if (left == Vector2.zero)
            {
                //sand.Add(new Vector2(x - 1, y));

                return DownTheGrainPath(grain, x - 1, sand, stepLimit);
            }
        }

        return Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject hostGm = transform.Find("HostSandEffect").gameObject;
        //Debug.Log(hostGm.transform.localPosition);

        foreach (var (point, pos) in _movingPoints)
        {
            point.localPosition += ((Vector3)pos - point.localPosition) * Time.deltaTime * CrumbleSpeed * Random.Range(.8f, 1.2f);
        }
    }
}
