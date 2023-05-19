using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager_ : MonoBehaviour
{
    [SerializeField] TMP_Text cloutCounterDisplay;
    [SerializeField] ConspiracyLevel[] conspiracyLevels;

    [SerializeField] float totalClout;

    void Start()
    {
    }

    void Update()
    {
        float totalCPS = 0;
        foreach (ConspiracyLevel conspiracyLevel in conspiracyLevels){
            totalCPS += conspiracyLevel.GetToatlCPS();
        }
        cloutCounterDisplay.text = "Total Clout: " + Number(totalClout) + "\nTotal Clout Per Second: " + Number(totalCPS);
    }

    public void progressBarFillCalculate(float value, float speed, Image progressFill){
        float targetFillAmount = Mathf.Min(1, value);
        progressFill.fillAmount = Mathf.Lerp(progressFill.fillAmount, targetFillAmount, speed);
    }

    public void AddClout(float cloutAdded){
        totalClout += cloutAdded;
    }
    public void subtractClout(float cloutSubtracted, ConspiracyLevel conspiracyLevel){
        totalClout -= cloutSubtracted;
        conspiracyLevel.AddTotalSpent(cloutSubtracted);
    }
    public float getClout(){
        return totalClout;
    }

    // horrible code ik
    public string Number(float num){
        string return_;
        int rounding = 100;
        if (num >= 1000 && num < 1000000){
            return_ = (Mathf.Round(num/1000 * rounding)/rounding).ToString() + "k";
            return return_;
        }
        if (num >= 1000000 && num < 1000000000){
            return_ = (Mathf.Round(num/1000000 * rounding)/rounding).ToString() + "mil";
            return return_;
        }
        if (num >= 1000000000 && num < 1000000000000){
            return_ = (Mathf.Round(num/1000000000 * rounding)/rounding).ToString() + "bil";
            return return_;
        }
        if (num >= 1000000000000){
            return_ = (Mathf.Round(num/1000000000000 * rounding)/rounding).ToString() + "tril";
            return return_;
        }
        return num.ToString();
    }

}
