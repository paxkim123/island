using UnityEngine;

public class YCB_Area_Tester : MonoBehaviour
{
    public YCB_Area ycb_area;
    public void Testing()
    {
        IYCB_Area e = ycb_area as IYCB_Area;
        if( e.isArea(transform.position))
        {
            Debug.Log("is Area!");
        }
        else
        {
            Debug.Log("is not Area!");
        }
    }
}
