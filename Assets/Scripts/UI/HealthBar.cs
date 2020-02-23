using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image ImgBar;
    public int Min;
    public int Max;

    private float percent;
    private int currentValue;

    // Start is called before the first frame update
    void Start()
    {
        SetHealth(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(int health){
        if(Max - Min == 0){
            currentValue = 0;
            percent = 0;
        }else{
            currentValue = health;
            percent = currentValue/(float)(Max-Min);
        }

        ImgBar.fillAmount = percent;
    } 

    public float Percent{
        get{return percent;}
    }

    public int CurrentValue{
        get{return currentValue;}
    }
}
