using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    public float MoveSpeed;
    //public float DamageNumber;
    public float DisplayDuration;
    public TextMeshProUGUI displayText;
    public Color DamageColor;
    public Color HealColor;

    public void Initialize(float dmg, bool isHealing)
    {
        displayText.text = isHealing ? "" :  "-";
        displayText.text += dmg;
        displayText.color = isHealing ? HealColor : DamageColor;
        var spawnDif = isHealing ? -0.5f : 0.5f;
        transform.position = new Vector3(transform.position.x + spawnDif, transform.position.y, 0);
    }
    void Update()
    {
        DisplayDuration -= Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + (MoveSpeed * Time.deltaTime), 0);
        
        if (DisplayDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}