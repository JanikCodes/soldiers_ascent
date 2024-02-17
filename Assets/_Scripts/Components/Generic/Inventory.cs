using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventoryMaxSize = 54;

    private List<Item> items = new List<Item>();
}
