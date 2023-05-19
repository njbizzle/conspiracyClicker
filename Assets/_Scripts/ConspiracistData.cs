using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ConspiracistData : ScriptableObject
{
    [Header("EDIT")]
    public string conspiracistName;
    public Sprite conspiracistImage;
    
    [Space(10)]
    public float cloutPerGenerate;
    public float generateSpeed;

    [Space(10)]
    public float defaultPrice;
    public float priceIncreaseMultiplyer;
    public float unlockPrice;


    [Space(10)]
    [Header("DONT EDIT")]
    public Conspiracist conspiracist;

    public float cloutGenMult;
    public float speedMult;

    public float currentPrice = 0;
    public float conspiracistQuantity = 0;
    public bool unlocked = false;
    public bool restartLoop = false;

    [Space(10)]
    public float totalGenerated = 0;
    public float totalSpent = 0;

    public void Reset(){
        currentPrice = 0;
        conspiracistQuantity = 0;
        unlocked = false;
        
        totalGenerated = 0;
        totalSpent = 0;
    }

    public void Upgrade(UpgradeManager.upgradeTypes upgradeType, float conspiracistUpgradeMultiplier){
        if (upgradeType == UpgradeManager.upgradeTypes.cloutPerGen){
            cloutGenMult*=conspiracistUpgradeMultiplier;
        } 
        else if (upgradeType == UpgradeManager.upgradeTypes.cloutGenSpeed){
            speedMult*=conspiracistUpgradeMultiplier;
            restartLoop = true;
        }
    }
}
