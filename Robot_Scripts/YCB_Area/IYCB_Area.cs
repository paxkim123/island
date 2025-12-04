using UnityEngine;
using System.Collections.Generic;
public interface IYCB_Area 
{
    public List<Vector3> samplingArea();
    public bool isArea(Vector3 position);
    public Vector3 samplingAreaMax();
    public Vector3 samplingAreaMin();


}
