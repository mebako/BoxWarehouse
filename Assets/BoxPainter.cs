using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxPainter : MonoBehaviour
{
    public List<GameObject> Ranged_Boxes;
    public Material MAT_Nearest, MAT_Near, MAT_Default;
    public List<float> Ranged_Boxes_Ranges = new List<float>();
    public List<MeshRenderer> Ranged_Boxes_Renderers = new List<MeshRenderer>();
    private GameObject _nearestBox;//Green
    private GameObject _nearBox;//Yellow

    private void Update()
    {
        FindNearestBoxes();
    }
    void CheckBoxRanges()
    {
        int i;
        for (i = 0; i < Ranged_Boxes.Count; i++)
        {
            Ranged_Boxes_Ranges[i] = Vector3.Distance(transform.position, Ranged_Boxes[i].transform.position);
        }
    }
    void FindNearestBoxes()
    {
        if (Ranged_Boxes.Count > 0)
        {
            int nearestIndex = Ranged_Boxes_Ranges.IndexOf(Ranged_Boxes_Ranges.Min());
            float savedNearestDistance = Ranged_Boxes_Ranges[nearestIndex];
            Ranged_Boxes_Ranges[nearestIndex] = Ranged_Boxes_Ranges.Max();
            int nearIndex = Ranged_Boxes_Ranges.IndexOf(Ranged_Boxes_Ranges.Min());//Near box found!
            Ranged_Boxes_Ranges[nearestIndex] = savedNearestDistance;

            int i;
            for (i = 0; i < Ranged_Boxes.Count; i++)
            {
                if (i == nearestIndex)//Nearest box found!
                    Ranged_Boxes_Renderers[i].material = MAT_Nearest;
                else Ranged_Boxes_Renderers[i].material = MAT_Default;
            }
            Ranged_Boxes_Renderers[nearIndex].material = MAT_Near;
            CheckBoxRanges();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Box")
        {
            if (!Ranged_Boxes.Contains((other.gameObject)))
            {
                Ranged_Boxes.Add(other.gameObject);
                Ranged_Boxes_Ranges.Add(Vector3.Distance(transform.position, other.transform.position));
                Ranged_Boxes_Renderers.Add(other.GetComponent<MeshRenderer>());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Box")
        {
            if (Ranged_Boxes.Contains((other.gameObject)))
            {
                Ranged_Boxes_Ranges.Remove(Ranged_Boxes_Ranges[Ranged_Boxes.IndexOf(other.gameObject)]);
                Ranged_Boxes_Renderers.Remove(other.GetComponent<MeshRenderer>());
                Ranged_Boxes.Remove(other.gameObject);
            }
        }
    }
}
