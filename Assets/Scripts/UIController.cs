using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController Instance = null;

    void Awake()
    {
        Instance = this;

        GameObject btnAttack = GameObject.Find("Canvas/BtnAttack");
        Button btn1 = (Button)btnAttack.GetComponent<Button>();
        btn1.onClick.AddListener(delegate () {
            InputController.Instance.notifyBtnAttackClicked(btnAttack);
        });
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
