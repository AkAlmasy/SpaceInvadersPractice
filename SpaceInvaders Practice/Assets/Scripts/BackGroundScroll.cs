using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour {

    [SerializeField] float backGroundScrollSpeed = 1f;
    Material myMaterial;
    Vector2 offSet;


	// Use this for initialization
	void Start () {
        myMaterial = GetComponent<Renderer>().material;
        offSet = new Vector2(0, backGroundScrollSpeed);
	}
	
	// Update is called once per frame
	void Update () {
        myMaterial.mainTextureOffset += offSet * Time.deltaTime;
	}
}
