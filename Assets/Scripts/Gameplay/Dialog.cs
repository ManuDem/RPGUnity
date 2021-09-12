using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;

[System.Serializable]
public class Dialog
{
    [TextArea] [SerializeField] string dialog;

    List<string> lines = new List<string>();
    StringBuilder stringBuilder = new StringBuilder();

    public void setLines() {
        for (int count = 0; count < dialog.Length; count++)
        {
            char c = dialog[count];
            
            if (!count.Equals(dialog.Length-1)) { 
                if (stringBuilder.Length <= 80)
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    if (char.IsWhiteSpace(dialog, count))
                    {
                        lines.Add(stringBuilder.ToString());
                        stringBuilder.Clear();
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }
                }
            } else { 
                stringBuilder.Append(c);
                lines.Add(stringBuilder.ToString());
                stringBuilder.Clear();

            }
        }  
    }

    public List<string> Lines {
        get { return lines; }
    }
}
