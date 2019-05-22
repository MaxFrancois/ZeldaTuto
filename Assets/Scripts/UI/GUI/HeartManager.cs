//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class HeartHealthger : MonoBehaviour
//{
//    //public Image[] Hearts;
//    //public Sprite FullHeart;
//    //public Sprite HalfHeart;
//    //public Sprite EmptyHeart;
//    //public FloatValue HeartContainers;
//    //public FloatValue CurrentHealth;

//    //void Start()
//    //{
//    //    Init();    
//    //}

//    //public void Init()
//    //{
//    //    for (int i = 0; i < HeartContainers.RuntimeValue; i++)
//    //    {
//    //        Hearts[i].gameObject.SetActive(true);
//    //        Hearts[i].sprite = FullHeart;
//    //    }
//    //    UpdateHearts();
//    //}

//    //public void UpdateHearts()
//    //{
//    //    var tempHealth = CurrentHealth.RuntimeValue / 2;
//    //    for (int i = 0; i < HeartContainers.RuntimeValue; i++)
//    //    {
//    //        if (i <= tempHealth - 1)
//    //        {
//    //            Hearts[i].sprite = FullHeart;
//    //        }
//    //        else if (i >= tempHealth)
//    //        {
//    //            Hearts[i].sprite = EmptyHeart;
//    //        }
//    //        else
//    //        {
//    //            Hearts[i].sprite = HalfHeart;
//    //        }
//    //    }
//    //}

//    //public Slider HealthSlider;
//    public Image HealthBarFiller;
//    public Image HealthBarDelayFiller;
//    public float DelayFillerSpeed;
//    public Inventory PlayerInventory;

//    void Start()
//    {
//        //HealthSlider.maxValue = PlayerInventory.MaxHealth;
//        //HealthSlider.value = PlayerInventory.MaxHealth;
//        PlayerInventory.CurrentHealth = PlayerInventory.MaxHealth;
//        HealthBarFiller.fillAmount = PlayerInventory.CurrentHealth / PlayerInventory.MaxHealth;
//    }

//    public void AddHealth(float amount)
//    {
//        HealthBarFiller.fillAmount += amount / 100;
//        PlayerInventory.CurrentHealth += amount;
//        if (PlayerInventory.CurrentHealth > PlayerInventory.MaxHealth)
//        {
//            HealthBarFiller.fillAmount = 1;
//            PlayerInventory.CurrentHealth = PlayerInventory.MaxHealth;
//        }
//    }

//    public void DecreaseHealth(float amount)
//    {
//        HealthBarFiller.fillAmount -= amount / 100;
//        PlayerInventory.CurrentHealth -= amount;
//        if (PlayerInventory.CurrentHealth <= 0)
//        {
//            HealthBarFiller.fillAmount = 0;
//            PlayerInventory.CurrentHealth = 0;
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (PlayerInventory.CurrentHealth < PlayerInventory.MaxHealth)
//        {
//            PlayerInventory.CurrentHealth += Time.deltaTime * PlayerInventory.PassiveHealthRegenSpeed;
//            HealthBarFiller.fillAmount += Time.deltaTime * PlayerInventory.PassiveHealthRegenSpeed / 100;
//            if (PlayerInventory.CurrentHealth > PlayerInventory.MaxHealth)
//            {
//                PlayerInventory.CurrentHealth = PlayerInventory.MaxHealth;
//                HealthBarFiller.fillAmount = 1;
//            }
//        }
//        if (HealthBarDelayFiller.fillAmount <= HealthBarFiller.fillAmount)
//            HealthBarDelayFiller.fillAmount += Time.deltaTime * DelayFillerSpeed;
//        if (HealthBarDelayFiller.fillAmount >= HealthBarFiller.fillAmount)
//            HealthBarDelayFiller.fillAmount -= Time.deltaTime * DelayFillerSpeed;
//    }
//}
