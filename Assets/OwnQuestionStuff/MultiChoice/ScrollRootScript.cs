using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRootScript : MonoBehaviour
{
    [SerializeField] GameObject contiueButton;

    private void Start()
    {
        if(contiueButton != null)
        {
            contiueButton.SetActive(false);
        }
    }

    public bool checkIfALLQuestionsAnswered()
    {
        QuestionCheckMulti[] quesMultis = GetComponentsInChildren<QuestionCheckMulti>();
        foreach (var ques in quesMultis)
        {
            if(ques.allQuestionsAnswered == false)
            {
                return false;
            }
        }
        if(contiueButton != null)
        {
            contiueButton.SetActive(true);
        }
        return true;
    }
}
