using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{

    public static GameEngine instance;

    void Awake()
    {
        instance = this;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartCoroutine(IEnumerator func)
    {
        StartCoroutine(func);
    }
}
