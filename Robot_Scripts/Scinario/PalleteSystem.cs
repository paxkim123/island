using System.Collections.Generic;
using UnityEngine;

public class PalleteSystem : MonoBehaviour
{

    public List<Transform> gchroll_transfrom_list = new List<Transform>();
    private List<Transform> gchroll_yet_selected;
    public readonly int gch_roll_max_idx = 16;

    Transform gchroll_select;

    public float offset_y;
    public int index;
    public void Awake()
    {
        gchroll_yet_selected = new List<Transform>();
        foreach (Transform e in gchroll_transfrom_list)
        {
            gchroll_yet_selected.Add(e);
            e.gameObject.SetActive(false);
        }
        index = 0;
    }
    public Vector3 h_get_gchroll_pos()
    {
        h_gchroll_selected();
        Vector3 origin_pose = gchroll_select.position;
        return origin_pose + new Vector3(0, offset_y, 0);
    }
    public void h_gchroll_setting()
    {
        gchroll_select.gameObject.SetActive(true);
        gchroll_select = null;
    }

    private int h_gchroll_selected()
    {
        gchroll_select = gchroll_yet_selected[0];
        gchroll_yet_selected.RemoveAt(0);
        index++;
        return 0;
    }
}
