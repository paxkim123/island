using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_hover_color : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalColor;
    private bool isMouseOver = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    void Update()
    {
        if (isMouseOver)
        {
            ChangeColor();
            Debug.Log("º¯°æ¿Ï");
        }
        else
        {
            ResetColor();
        }

        isMouseOver = false; // Reset for the next frame
    }

    void OnMouseOver()
    {
        isMouseOver = true;
    }

    void ChangeColor()
    {
        if (objectRenderer != null)
        {
            Color newColor = originalColor;
            newColor.r = Mathf.Clamp01(newColor.r - 100f / 255f);
            newColor.g = Mathf.Clamp01(newColor.g - 100f / 255f);
            newColor.b = Mathf.Clamp01(newColor.b - 100f / 255f);
            objectRenderer.material.color = newColor;
        }
    }

    void ResetColor()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }
    }
}