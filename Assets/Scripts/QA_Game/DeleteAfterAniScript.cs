using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterAniScript : MonoBehaviour
{
    [SerializeField] float delteTime = 2.1f;
    void Start (){
        StartCoroutine(delete());
    }

    IEnumerator delete (){
        yield return new WaitForSeconds (delteTime);
        Destroy (gameObject);
    }
}
