using UnityEngine;
using System.Collections.Generic;

public class YCB_Area : MonoBehaviour 
{
    public bool isGizmos = false;
    public bool autoGizmos = false;
    public bool autoUpdateTransformChild = false;
    public bool autoUpdateValidateChild = false;


    protected Mesh meshGizmos;
    protected Vector3[] pointGizmos;
    public Color colorGizmos = Color.black;

    protected virtual List<YCB_Area> getChild { get { return null; } }
    public void copyValidate(YCB_Area target)
    {
        isGizmos = target.isGizmos;
        autoGizmos = target.autoGizmos;

        autoUpdateTransformChild = target.autoUpdateTransformChild;
        autoUpdateValidateChild = target.autoUpdateValidateChild;
    }

    void OnValidate()
    {
        if(autoGizmos)
        {
            updateGizmos();
        }
        if (autoUpdateTransformChild)
        {
            updateTransformChild();
        }
        if (autoUpdateValidateChild)
        {
            updateValidateChild();
        }
    }
    public virtual void updateGizmos() { }
    protected virtual void updateTransformChild(){ }
    protected virtual void updateValidateChild(){ }
}
