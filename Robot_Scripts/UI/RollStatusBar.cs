using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RollStatusBar : MonoBehaviour
{
    [Header("References")]
    public Transform m_roll_transform;
    public Image m_status_out_bar;
    public Image m_status_in_bar;
    public Text m_status_text;
    public TMPro.TextMeshProUGUI m_roll_text;

    [Header("Data")]
    [Range(0f, 1f)] public float m_percent;
    public string m_name;
    public bool m_draw_line = true;
    [Range(1f, 10f)] public float m_thickness; 
    public Color m_draw_color = Color.white;
    
    private void OnValidate()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (m_roll_text != null)
            m_roll_text.text = m_name;

        if (m_status_in_bar != null)
            m_status_in_bar.fillAmount = m_percent;

        if (m_status_text != null)
        {
            int pct = Mathf.RoundToInt(m_percent * 100f);
            m_status_text.text = pct + " %";
        }
    }
}
