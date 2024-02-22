using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCurrencyRequirementSO : DialogueRequirementSO
{
    public int RequiredCurrency;

    public override bool CheckRequirements(Transform self, Transform other)
    {
        Transform selectedTransform = Self ? self : other;

        CurrencyStorage currencyStorage = selectedTransform.GetComponent<CurrencyStorage>();
        return currencyStorage.HasEnoughCurrency(RequiredCurrency);
    }
}
