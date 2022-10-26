using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DEBUG : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait() {
       // File.Create("/storage/emulated/0/BART_Studie/testFile.txt");
        yield return new WaitForSecondsRealtime(1);
        text.text = SaveManager.instance.getFileLoc();
        StartCoroutine(wait2());
        StartCoroutine(wait3());
        text.text += Directory.Exists(text.text).ToString();
        if (!Directory.Exists(text.text))
        {
            Directory.CreateDirectory(text.text);
            text.text += " Created Dir";
        }
    }
    IEnumerator wait2()
    {
        yield return new WaitForSecondsRealtime(5);
        File.Create("/storage/emulated/0/BART_Studie/testFile.txt");
        text.text += " file created";
    }
    IEnumerator wait3()
    {
        yield return new WaitForSecondsRealtime(3);
        Directory.CreateDirectory("/storage/emulated/0/BART_Studie/");
        text.text += " dir created";
    }

    public void openBrowser()
    {
        LevelManager.lm.openChromeBrowser();

    }

}


