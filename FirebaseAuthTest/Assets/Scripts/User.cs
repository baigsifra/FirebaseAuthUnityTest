using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

[FirestoreData]
public class User
{
    [FirestoreProperty]
    public string StoreName { get; set; }

    [FirestoreProperty]
    public int Currency { get; set; }

    [FirestoreProperty]
    public int Level { get; set; }

    [FirestoreProperty]
    public int TotalExp { get; set; }

    [FirestoreProperty]
    public int CurrentExp { get; set; }

    [FirestoreProperty]
    public int CompletedOrders { get; set; }

    [FirestoreProperty]
    public int TotalProfit { get; set; }

    [FirestoreProperty]
    public int TotalTips { get; set; }

    [FirestoreProperty]
    public List<Ingredient> Inventory { get; set; }

    public User() {}

    public User(string storeName) {
        StoreName = storeName;
        Currency = 0;
        Level = 1;
        TotalExp = 0;
        CurrentExp = 0;
        TotalProfit = 0;
        TotalTips = 0;
        CompletedOrders = 0;
        Inventory = new List<Ingredient>();
    }

    public User(string storeName, int currency, int level, int totalExp, int currentExp, int totalProfit, int totalTips, int completedOrders, List<Ingredient> inventory) {
        StoreName = storeName;
        Currency = currency;
        Level = level;
        TotalExp = totalExp;
        CurrentExp = currentExp;
        TotalProfit = totalProfit;
        TotalTips = totalTips;
        CompletedOrders = completedOrders;
        Inventory = inventory;
    }

    public int addToInventory(Ingredient i) {
        foreach(Ingredient ing in Inventory) {
            if(i.Name == ing.Name) {
                ing.Quantity += i.Quantity;
                return ing.Quantity;
            }
        }
        Inventory.Add(i);
        return 0;
    }
}
