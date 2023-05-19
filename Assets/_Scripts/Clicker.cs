using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clicker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ConspiracyLevel conspiracyLevel;
    [SerializeField] GameManager_ gameManager;
    [SerializeField] CustomButton clickerButton;

    [Header("Displays")]

    [SerializeField] Image generateProgressBarFill;
    [SerializeField] float generateProgressBarSpeed;

    [Space(10)]
    [SerializeField] Image queueProgressBarFill;
    [SerializeField] TMP_Text queueProgressBarText;
    [SerializeField] float queueProgressBarSpeed;

    [Header("Don't Edit")]
    [SerializeField] float timeUntilCloutGen;
    [SerializeField] float queueLength;

    [Header("Edit")]
    [SerializeField] float cloutGenTime;
    [SerializeField] float cloutOnGen;

    [SerializeField] float queueMaxLength;
    [SerializeField] float clickTimeDecrease;

    void Start(){
        gameManager = FindObjectOfType<GameManager_>(); // find game manager

        clickerButton.AddOnClick(clicked); // add clicked function to button delegate
    }

    void Update(){
        float targetFillAmount;

        if (queueLength == queueMaxLength){
            queueProgressBarText.text = "MAX QUEUE " + queueLength + " / " + queueMaxLength; // update queue text if full
        } else{
            queueProgressBarText.text = "Queued " + queueLength + " / " + queueMaxLength; // update queue text if not full
        }

        // progress bar calculations
        targetFillAmount = timeUntilCloutGen / cloutGenTime; 
        gameManager.progressBarFillCalculate(targetFillAmount, generateProgressBarSpeed, generateProgressBarFill);
        targetFillAmount = queueLength / queueMaxLength;
        gameManager.progressBarFillCalculate(targetFillAmount, queueProgressBarSpeed, queueProgressBarFill);

        if (timeUntilCloutGen < 0 && queueLength > 0){
            queueLength -= 1; // count down queue
            gameManager.AddClout(cloutOnGen); // add clout
            timeUntilCloutGen = cloutGenTime; // reset timer
        } else{
            timeUntilCloutGen -= Time.deltaTime; // count down timer
        }
    }

    void clicked(){
        queueLength = Mathf.Min(queueLength + 1, queueMaxLength); // update queue and stop it from passing max queue length

        timeUntilCloutGen -= clickTimeDecrease; // decrease time needed when clicked


    }
}
