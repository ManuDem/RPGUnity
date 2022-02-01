using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{

    [TextArea] [SerializeField] string dialog;

    [SerializeField] List<string> lines;

    public List<string> Lines
    {
        get
        {
            lines.Clear();
            if (dialog.Contains('\n'))
            {
                string[] splittedStringNewLine = dialog.Split(new char[] { '\n' });
                for (int countNewLine = 0; countNewLine < splittedStringNewLine.Length; countNewLine++)
                {
                    string[] splittedStringSpace = splittedStringNewLine[countNewLine].Split(new char[] { ' ' });
                    string tempString = "";

                    for (int count = 0; count < splittedStringSpace.Length; count++)
                    {
                        if (tempString == "")
                        {
                            tempString = splittedStringSpace[count];
                        }
                        else
                        {
                            tempString = tempString + " " + splittedStringSpace[count];
                        }

                        if (tempString.Length >= 80)
                        {
                            lines.Add(tempString);
                            tempString = "";
                        }
                        else if (count == (splittedStringSpace.Length - 1) && tempString != "")
                        {
                            lines.Add(tempString);
                            tempString = "";
                        }
                    }
                }
            }
            else
            {
                string[] splittedStringSpace = dialog.Split(new char[] { ' ' });
                string tempString = "";

                for (int count = 0; count < splittedStringSpace.Length; count++)
                {
                    if (tempString == "")
                    {
                        tempString = splittedStringSpace[count];
                    }
                    else
                    {
                        tempString = tempString + " " + splittedStringSpace[count];
                    }

                    if (tempString.Length >= 80)
                    {
                        lines.Add(tempString);
                        tempString = "";
                    }
                    else if (count == (splittedStringSpace.Length - 1) && tempString != "")
                    {
                        lines.Add(tempString);
                        tempString = "";
                    }
                }
            }
            if (lines != null)
                return lines;
            else
                return new List<string>();
        }
    }
}



