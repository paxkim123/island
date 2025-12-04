using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
public class YCB_AreaInter : YCB_Area ,  IYCB_Area
{
    public List<Transform> YCBAreaList = new List<Transform>();

    public bool getGizmos()
    {
        throw new System.NotImplementedException();
    }

    public bool isArea(Vector3 position)
    {
        foreach (Transform e in YCBAreaList)
        {
            IYCB_Area iycbe = e.GetComponent<IYCB_Area>();
            if (iycbe == null) continue;
            if (!iycbe.isArea(position))
                return false;
        }
        return true;
    }
    void OnDrawGizmos()
    {
    }
    public List<Vector3> samplingArea()
    {
        return YCB_AreaFunction.sampling_grid(this);
    }

    public Vector3 samplingAreaMax()
    {
        float x = float.MaxValue;
        float y = float.MaxValue;
        float z = float.MaxValue;

        foreach (IYCB_Area e in YCBAreaList)
        {
            Vector3 vector = e.samplingAreaMax();
            if (vector.x < x) x = vector.x;
            if (vector.y < y) y = vector.y;
            if (vector.z < z) z = vector.z;
        }
        return new Vector3(x, y, z);
    }

    public Vector3 samplingAreaMin()
    {
        float x = float.MinValue;
        float y = float.MinValue;
        float z = float.MinValue;

        foreach (IYCB_Area e in YCBAreaList)
        {
            Vector3 vector = e.samplingAreaMin();
            if (vector.x > x) x = vector.x;
            if (vector.y > y) y = vector.y;
            if (vector.z > z) z = vector.z;
        }
        return new Vector3(x, y, z);
    }
    public override void updateGizmos ()
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
            foreach (Transform e in YCBAreaList)
            {
                YCB_Area ycb = e.GetComponent<YCB_Area>();
                if (ycb != null) result.Add(ycb);
            }
            return result;
        }
    }
    protected override void updateValidateChild()
    {
        foreach (YCB_Area e in getChild)
        {
            e.copyValidate(this);
        }
    }
    protected override void updateTransformChild()
    {

        YCBAreaList.Clear();
        foreach (Transform e in transform)
        {
            IYCB_Area ycbe = e.GetComponent<IYCB_Area>();
            if (ycbe != null)
            {
                YCBAreaList.Add(e);
            }
        }
    }
}
