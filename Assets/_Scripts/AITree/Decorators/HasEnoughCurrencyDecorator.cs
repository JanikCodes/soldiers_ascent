using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent(name:"HasEnoughCurrency", path:"Base/Faction/HasEnoughCurrency", IconPath = "Images/Icons/Node/Example.png")]
public class HasEnoughCurrencyDecorator : ConditionDecorator
{
    [Title("Node")]
    [SerializeField]
    private IntKey requiredCurrency;

    // Stored required components.
    private CurrencyStorage currencyStorage; 

    protected override void OnEntry()
    {
        base.OnEntry();

        currencyStorage = GetOwner().GetComponent<CurrencyStorage>();
    }
 
    protected override bool CalculateResult()
    {
        return currencyStorage.HasEnoughCurrency(requiredCurrency.GetValue());
    }

    protected override void OnFlowUpdate() { }
}
