using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneQuestionElem : MonoBehaviour
{
    public bool isQuestionSelected = false;
    public string saveName = "";
    public string questionValue = "";

    public void assignValue (string value){
        isQuestionSelected = true;
        questionValue = value;
        GetComponentInParent<QuestionCheckMulti>().addSaveData(saveName,value);
    }
}
