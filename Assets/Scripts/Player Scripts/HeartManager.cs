using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] Hearts;
    public Sprite FullHeart;
    public Sprite HalfHeart;
    public Sprite EmptyHeart;
    public FloatValue HeartContainers;
    public FloatValue CurrentHealth;

    void Start()
    {
        Init();    
    }

    public void Init()
    {
        for (int i = 0; i < HeartContainers.RuntimeValue; i++)
        {
            Hearts[i].gameObject.SetActive(true);
            Hearts[i].sprite = FullHeart;
        }
    }

    public void UpdateHearts()
    {
        var tempHealth = CurrentHealth.RuntimeValue / 2;
        for (int i = 0; i < HeartContainers.RuntimeValue; i++)
        {
            if (i <= tempHealth - 1)
            {
                Hearts[i].sprite = FullHeart;
            }
            else if (i >= tempHealth)
            {
                Hearts[i].sprite = EmptyHeart;
            }
            else
            {
                Hearts[i].sprite = HalfHeart;
            }
        }
    }
}
