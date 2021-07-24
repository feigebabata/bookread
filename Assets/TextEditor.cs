using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextEditor : MonoBehaviour
{
    public string SavePath = "D:/save.txt";
    public string LoadPath = "D:/load.txt";
    // Start is called before the first frame update
    void Start()
    {
        editor();
    }

    
    void editor()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        string text = File.ReadAllText(LoadPath);
        string endChars = "。！」";
        var list = text.Split(new string[]{"\n\r","\n","\r"},StringSplitOptions.None);
        bool isNewLine=true;
        foreach (var item in list)
        {
            string line = item;
            if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            
            char end = line[line.Length-1];
            if(endChars.IndexOf(end)!=-1)
            {
                if(!isNewLine)
                {
                    line = line.TrimStart();
                }
                builder.AppendLine(line);
                isNewLine=true;
            }
            else
            {
                if(!isNewLine)
                {
                    line = line.TrimStart();
                }
                builder.Append(line);
                isNewLine=false;
            }
        }
        File.WriteAllText(SavePath,builder.ToString());
    }

}
