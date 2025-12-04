using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
public class YCB_AreaSub : YCB_Area, IYCB_Area
{
    public List<Transform> baseYCBAreaList = new List<Transform>();
    [SerializeReference]
    public List<Transform> subYCBAreaList = new List<Transform>();

    public bool getGizmos()
    {
        throw new System.NotImplementedException();
    }
    public bool isArea(Vector3 position)
    {
        foreach (Transform e in subYCBAreaList)
        {
            IYCB_Area iycbe = e.GetComponent<IYCB_Area>();
            if (iycbe == null) continue;
            if (iycbe.isArea(position))
                return false;
        }
        foreach (Transform e in baseYCBAreaList)
        {
            IYCB_Area iycbe = e.GetComponent<IYCB_Area>();
            if (iycbe == null) continue;
            if (iycbe.isArea(position))
                return true;
        }
        return false;
    }

    public List<Vector3> samplingArea()
    {
        return YCB_AreaFunction.sampling_grid(this);
    }

    public Vector3 samplingAreaMax()
    {
        float x = float.MinValue;
        float y = float.MinValue;
        float z = float.MinValue;

        foreach (IYCB_Area e in baseYCBAreaList)
        {
            Vector3 vector = e.samplingAreaMax();
            if (vector.x > x) x = vector.x;
            if (vector.y > y) y = vector.y;
            if (vector.z > z) z = vector.z;
        }
        return new Vector3(x, y, z);
    }

    public Vector3 samplingAreaMin()
    {
        float x = float.MaxValue;
        float y = float.MaxValue;
        float z = float.MaxValue;

        foreach (IYCB_Area e in baseYCBAreaList)
        {
            Vector3 vector = e.samplingAreaMin();
            if (vector.x < x) x = vector.x;
            if (vector.y < y) y = vector.y;
            if (vector.z < z) z = vector.z;
        }
        return new Vector3(x, y, z);
    }

    public override void updateGizmos()
    {
        updatePointGizmos();
    }
    protected void updatePointGizmos()
    {
        Debug.Log("updateGizmos is TODO");
        //pointGizmos = YCB_AreaFunction.sampling_grid_arr(this, pointGizmosInter);
    }

    protected override List<YCB_Area> getChild
    {
        get
        {
            List<YCB_Area> result = new List<YCB_Area>();
            foreach(Transform e in baseYCBAreaList)
            {
                YCB_Area ycb = e.GetComponent<YCB_Area>();
                if(ycb!=null) result.Add(ycb);
            }
            foreach(Transform e in subYCBAreaList)
            {
                YCB_Area ycb = e.GetComponent<YCB_Area>();
                if (ycb != null) result.Add(ycb);
            }
            return result;
        }
    }
    void OnDrawGizmos()
    {
    }
    
    protected override void updateValidateChild()
    {
        foreach (YCB_Area e in getChild)
        { e.copyValidate(this); }
    }
    protected override void updateTransformChild()
    {
        baseYCBAreaList.Clear();
        subYCBAreaList.Clear();
        foreach ( Transform e in transform)
        {
            IYCB_Area ycbe = e.GetComponent<IYCB_Area>();
            if (ycbe != null)
            {
                bool sub = false;
                bool plus = false;
                if (e.name.Contains('+')) plus = true;
                if (e.name.Contains('-')) sub = true;
                if (plus || !sub)
                {
                    if(!baseYCBAreaList.Contains(e))
                        baseYCBAreaList.Add(e);
                }
                else
                {
                    if(!subYCBAreaList.Contains(e))
                        subYCBAreaList.Add(e);
                }
            }
        }
    }
}
