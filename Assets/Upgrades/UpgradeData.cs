using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu()]
public class UpgradeData : ScriptableObject
{
    [Header("Basics")]
    public string upgradeName;
    public float upgradeCost;

    public ConspiracyLevel conspiracyLevel;
    public string conspiracyLevelName;

    public Sprite upgradeImage;

    [Header("Conspiracist Upgrade")]

    public ConspiracistData conspiracistData;
    public UpgradeManager.upgradeTypes conspiracistUpgrade;
    public float conspiracistUpgradeMultiplier;

    [Space(10)]
    public bool addToAvailable;

    [Header("Tooltip")]
    [TextArea] 
    public string tooltipText;

    [Header("Don't Edit")]
    public bool canPurchase;
    public bool added;
    public bool purchased;
    [SerializeField] GameManager_ gameManager;

    void Start(){
    }
}
