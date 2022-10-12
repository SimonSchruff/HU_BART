using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OneToggleEntry : MonoBehaviour
{
    public Toggle toggleElemRef;  

    public void buttonClick (){
        foreach (var desel in transform.parent.GetComponentsInChildren<OneToggleEntry>())
        {
            Debug.Log("COT");
            desel.deselect();
        }

        GetComponentInParent<OneQuestionReferenceHelp>().isQuestionSelected = true; // Unable to unselect values

        toggleElemRef.isOn = true;
    }
    public void deselect(){
        toggleElemRef.isOn = false;
    }
}
 