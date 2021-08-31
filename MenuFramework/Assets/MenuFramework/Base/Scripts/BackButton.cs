using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public bool BckOnState;
    public GameObject BckButton;
    public GameObject MainFunctions;
    public GameObject LeftPan;

    

    // Start is called before the first frame update
    void Start()
    {
        BckOnState = false;
    }

    // Update is called once per frame
    public void BeenClicked()
    {
        BckOnState = !BckOnState;

        if(BckOnState == true)
        {
            MainFunctions.SetActive(true);
            LeftPan.SetActive(false);
        }

        if(BckOnState == false)
        {
            MainFunctions.SetActive(false);
            LeftPan.SetActive(true);
        }
    }
}
