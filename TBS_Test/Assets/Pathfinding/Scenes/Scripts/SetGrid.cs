using UnityEngine;
using System.Collections;

public class SetGrid : MonoBehaviour {

    public float X = 1;
    public float Y = 1;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(X , Y);
    }
}
