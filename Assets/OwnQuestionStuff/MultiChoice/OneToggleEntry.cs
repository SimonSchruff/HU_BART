using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OneToggleEntry : MonoBehaviour
{
    public int index;
    public Toggle toggleElemRef;  

    public void buttonClick (){
        foreach (var desel in transform.parent.GetComponentsInChildren<OneToggleEntry>())
        {
       //     Debug.Log("KOT");
            desel.deselect();
        }
        Debug.Log(index + " index");

        GetComponentInParent<OneQuestionElem>().assignValue(index.ToString()); // Unable to unselect values

        toggleElemRef.isOn = true;
    }
    public void deselect(){
        toggleElemRef.isOn = false;
    }
}
 