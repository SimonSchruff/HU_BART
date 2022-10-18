using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class SimpleContinueToNextLevel : MonoBehaviour
{
    SaveManager sm = SaveManager.instance;
    // Start is called before the first frame update
    public void loadNextLevelAndSave()
    {
        ScrollRootScript [] srs = FindObjectsOfType<ScrollRootScript>();
        foreach (var sr in srs)
        {
            QuestionCheckMulti[] children = sr.GetComponentsInChildren<QuestionCheckMulti>();
            foreach (var ch in children)
            {
                sm.allSaveData.Add(ch.saveDictionary);
            }
        }
        SaveManager.instance.finishSavingAndCloseProgram();

        LevelManager.lm.nextLevel();
    }
}
