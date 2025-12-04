using UnityEngine;
using System.Collections.Generic;

public class GchRollSystem : MonoBehaviour
{
    public List<Transform> gchroll_transfrom_list = new List<Transform>();
    private List<Transform> gchroll_yet_selected;
    public readonly int gch_roll_max_idx = 12;

    Transform gchroll_select;

    public float offset_x;
    public bool offset_reverse;

    public void Awake()
    {
        gchroll_yet_selected = new List<Transform>();
        foreach (Transform e in gchroll_transfrom_list)
        {
            gchroll_yet_selected.Add(e);
        }
    }
    public Vector3 h_get_gchroll_pos()
    {
        h_gchroll_selected();
        Vector3 origin_pose = gchroll_select.position;
        if(offset_reverse)
        {
            return origin_pose - new Vector3(offset_x, 0, 0);
        }
        else
        {
            return origin_pose + new Vector3(offset_x, 0, 0);
        }
    }
    public void h_gchroll_gripped()
    {
        gchroll_select.gameObject.SetActive(false);
        gchroll_select = null;
    }

    private int h_gchroll_selected()
    {
        int idx  = Random.Range(0, gchroll_yet_selected.Count);
        gchroll_select = gchroll_yet_selected[idx];
        gchroll_yet_selected.RemoveAt(idx);
        return idx;
    }

}
