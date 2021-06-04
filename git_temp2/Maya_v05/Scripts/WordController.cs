using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class WordController
{
    // Start is called before the first frame update
    public List<String> wordList;
    private List<String> paramNameList;
    private List<int> paramIntList;

    private TextAsset csvFile;

    // Update is called once per frame

    public void CheckWord(string sentence, out string paramName ,out int paramInt)
    {
        paramName = "none";
        paramInt = 0;
        for(int i= 0; i<wordList.Count; i++)
        {
            var word = wordList[i];
            if (sentence.Contains(word))
            {
                paramName = paramNameList[i];
                paramInt = paramIntList[i];
                break;
            }
        }
    }

    public void ReadFile()
    {
        wordList = new List<string>();
        paramNameList = new List<string>();
        paramIntList = new List<int>();
        csvFile = Resources.Load("wordAnimation") as TextAsset;
        ;
        StringReader reader = new StringReader(csvFile.text);
        while(reader.Peek() >= 0)
        {
            string[] cols = reader.ReadLine().Split(',');
            if(cols.Length!=3){
                Debug.Log("Wrong CSV Format!");
            }
            wordList.Add(cols[0]);
            paramNameList.Add(cols[1]);
            paramIntList.Add(int.Parse(cols[2]));
        }
    }
    
}
