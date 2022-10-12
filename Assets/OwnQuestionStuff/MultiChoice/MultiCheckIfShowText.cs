using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MultiCheckIfShowText : MonoBehaviour
{
    [SerializeField] QuestionCheckMulti questionRef;

    private void Update()
    {
        if( questionRef == null)
        {
            questionRef = GetComponentInParent<QuestionCheckMulti>();
           // questionRef = transform.parent.transform.parent.transform.parent.GetComponent<QuestionCheckMulti>();
            Debug.Log("Ref set");
        }
        transform.GetChild(0).gameObject.SetActive(questionRef.textOnEveryEntry);
    }
}
