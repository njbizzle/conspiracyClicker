using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Conspiracist : MonoBehaviour
{
    [SerializeField] bool clickToUpdate;

    [Header("References")]
    [SerializeField] ConspiracistData conspiracistData;
    [SerializeField] GameManager_ gameManager;
    [SerializeField] ConspiracyLevel conspiracyLevel;
    
    [Header("Main Display")]
    [SerializeField] TMP_Text nameQuantityDisplay;
    [SerializeField] Image conspiracistImage;

    [Header("Generate Progress Bar")]
    [SerializeField] Image generateProgressFill;
    [SerializeField] float lastGenerateTime;
    [SerializeField] float generateProgressSpeed;

    [Header("Price Progress Bar")]
    [SerializeField] TMP_Text priceDisplay;
    [SerializeField] Image priceProgressFill;
    [SerializeField] float priceProgressSpeed;

    [Header("Buy Button")]
    [SerializeField] CustomButton buyButton;
    [SerializeField] Image buyButtonBackground;

    [Header("Stats")]
    [SerializeField] TMP_Text statsBody;

    [Header("Affordable")]
    [SerializeField] Color32 affordableTextColor;
    [SerializeField] Color32 unaffordableTextColor;
    [SerializeField] bool isAffordable;

    [Header("Unlockablity")]
    [SerializeField] GameObject lockedScreen;

    [Space(10)]
    [SerializeField] TMP_Text unlockTitleText;
    [SerializeField] CustomButton unlockButton;
    [SerializeField] TMP_Text unlockButtonText;
    [SerializeField] Image unlockablityProgressFill;

    [Space(10)]
    [SerializeField] float unlockablityProgressSpeed;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager_>();
        
        buyButton.ResetButton();
        buyButton.AddOnClick(BuyConspiracist);

        unlockButton.ResetButton();
        unlockButton.AddOnClick(UnlockConspiracist);

        conspiracistData.conspiracist = this;
    }

    public void SetConspiracistDataAndLevel(ConspiracistData newConspiracistData, ConspiracyLevel newConspiracyLevel){
        conspiracistData = newConspiracistData;
        conspiracyLevel = newConspiracyLevel;
        
        Setup();
    }

    void Setup(){

        if (conspiracistData.unlocked){
            lockedScreen.SetActive(false);
        } else{
            lockedScreen.SetActive(true);
        }
        this.name = conspiracistData.conspiracistName + " (conspiracist)";

        conspiracistData.currentPrice = conspiracistData.defaultPrice;
        conspiracistImage.sprite = conspiracistData.conspiracistImage;
    }

    void UnlockConspiracist(){
        if (gameManager.getClout() < conspiracistData.unlockPrice) {return;}
        if (conspiracistData.unlocked) {return;}
        
        gameManager.subtractClout(conspiracistData.unlockPrice, conspiracyLevel);
        lockedScreen.SetActive(false);

        conspiracistData.unlocked = true;
        conspiracistData.conspiracistQuantity += 1;

        conspiracistData.cloutGenMult = 1;
        conspiracistData.speedMult = 1;
        conspiracistData.totalSpent += conspiracistData.unlockPrice;


        RestartGenerateLoop();
    }

    void Update()
    {
        float progressBarTarget = 0;
        
        if (conspiracistData.restartLoop){
            conspiracistData.restartLoop = false;
            RestartGenerateLoop();
        }

        if (!conspiracistData.unlocked){
            unlockTitleText.text = conspiracistData.conspiracistName + " (locked)";

            progressBarTarget = gameManager.getClout() / conspiracistData.unlockPrice;
            
            if (progressBarTarget >= 1){
                unlockButtonText.text = "Unlock " + conspiracistData.conspiracistName + " for " +  gameManager.Number(conspiracistData.unlockPrice) + " Clout";
            } else{
                unlockButtonText.text = "Need " + gameManager.Number(conspiracistData.unlockPrice) + " Clout to Unlock " + conspiracistData.conspiracistName;
            }
            gameManager.progressBarFillCalculate(progressBarTarget, unlockablityProgressSpeed, unlockablityProgressFill);
            return;
        }

        isAffordable = gameManager.getClout() >= conspiracistData.currentPrice;

        if (clickToUpdate){
            clickToUpdate = false;
            Setup();
        }

        nameQuantityDisplay.text = conspiracistData.conspiracistName + " x" + conspiracistData.conspiracistQuantity;

        progressBarTarget = gameManager.getClout() / conspiracistData.currentPrice;
        gameManager.progressBarFillCalculate(progressBarTarget, priceProgressSpeed, priceProgressFill);

        if (lastGenerateTime != 0){
            progressBarTarget = (Time.time - lastGenerateTime) / conspiracistData.generateSpeed / conspiracistData.speedMult;
            gameManager.progressBarFillCalculate(progressBarTarget, generateProgressSpeed, generateProgressFill);
        }

        if (isAffordable){
            priceDisplay.text = "Hire for " + gameManager.Number(conspiracistData.currentPrice) + " Clout";
            priceDisplay.color = affordableTextColor;
            buyButtonBackground.color = affordableTextColor;
        } else{
            priceDisplay.text = "Needs " + gameManager.Number(conspiracistData.currentPrice) + " Clout";
            priceDisplay.color = unaffordableTextColor;
            buyButtonBackground.color = unaffordableTextColor;
        }

        statsBody.text = "Bought: " + conspiracistData.conspiracistQuantity + "\n" +
        "Generate Speed: " + gameManager.Number(conspiracistData.generateSpeed / conspiracistData.speedMult) + "\n" +
        "Clout Per Generate: " + gameManager.Number(conspiracistData.cloutPerGenerate * conspiracistData.cloutGenMult * conspiracistData.conspiracistQuantity) + "\n" +
        "Clout Per Second: " + gameManager.Number((conspiracistData.cloutPerGenerate * conspiracistData.cloutGenMult * conspiracistData.conspiracistQuantity) / conspiracistData.generateSpeed) + "\n" +
        "Generated Total: " + gameManager.Number(conspiracistData.totalGenerated) + "\n" +
        "Spent Total: " + gameManager.Number(conspiracistData.totalSpent);
    }

    void BuyConspiracist(){
        if (!isAffordable){return;}

        if (conspiracistData.conspiracistQuantity == 0){
            RestartGenerateLoop();
        }

        gameManager.subtractClout(conspiracistData.currentPrice, conspiracyLevel);
        conspiracistData.totalSpent += conspiracistData.currentPrice;

        conspiracistData.currentPrice = Mathf.Round(conspiracistData.currentPrice * (1 + conspiracistData.priceIncreaseMultiplyer));
        conspiracistData.conspiracistQuantity += 1;
    }

    public void RestartGenerateLoop(){
        lastGenerateTime = Time.time;

        CancelInvoke("GenerateClout");
        InvokeRepeating("GenerateClout", conspiracistData.generateSpeed / conspiracistData.speedMult, conspiracistData.generateSpeed / conspiracistData.speedMult);
    }

    void GenerateClout(){
        lastGenerateTime = Time.time;
        float cloutGenerated = conspiracistData.cloutPerGenerate * conspiracistData.cloutGenMult * conspiracistData.conspiracistQuantity;
        conspiracistData.totalGenerated += cloutGenerated;
        gameManager.AddClout(cloutGenerated);
    }

}
