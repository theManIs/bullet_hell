using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public virtual void Start()
    {
        gameObject.SetActive(false);
        GetComponent<RectTransform>().localPosition = Vector3.zero;
    }
}
