using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    public float MoveSpeed;
    public float DamageNumber;
    public float DisplayDuration; 
    public TextMeshProUGUI displayText;

    void Update()
    {
        displayText.text = "-" + DamageNumber;
        DisplayDuration -= Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + (MoveSpeed * Time.deltaTime), 0);
        if (DisplayDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
