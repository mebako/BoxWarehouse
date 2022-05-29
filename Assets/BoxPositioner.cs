using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class BoxPositioner : MonoBehaviour
{
    public List<GameObject> Boxes;
    List<string> textList = new List<string>();
    List<Vector3> positions = new List<Vector3>();
    TextAsset textSource;

    void Start()
    {
        textSource = Resources.Load("BoxPositions") as TextAsset;
        Reposition();
        //FillTextfile();
    }

    void FillTextfile()
    {
        List<string> defBoxPositions = new List<string>();
        int i;
        for (i = 0; i < Boxes.Count; i++)
        {
            defBoxPositions.Add(Boxes[i].transform.localPosition.ToString().Replace(',', '|'));
        }
        File.WriteAllLines("Assets/Resources/BoxPositions.txt", defBoxPositions);
    }

    void Reposition()
    {
        textList = textSource.text.Split('\n').ToList();
        int i;
        for (i = 0; i < textList.Count - 1; i++)
        {
            positions.Add(String2Vector(textList[i].Substring(1, textList[i].Length - 3)));
        }

        for (i = 0; i < Boxes.Count; i++)
        {
            Boxes[i].transform.localPosition = positions[i];
        }

    }

    Vector3 String2Vector(string inputString)
    {
        Vector3 result = Vector3.zero;

        List<float> floats = new List<float>();
        string[] strings = inputString.Split('|');

        int i;
        for (i = 0; i < strings.Length; i++)
        {
            floats.Add(float.Parse(strings[i], System.Globalization.CultureInfo.GetCultureInfo("en-US")));
        }

        result = new Vector3(floats[0], floats[1], floats[2]);

        return result;
    }
}
