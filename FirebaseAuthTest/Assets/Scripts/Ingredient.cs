using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

[FirestoreData]
public class Ingredient
{
    [FirestoreProperty]
    public string Name { get; set; }
    [FirestoreProperty]
    public int Quantity { get; set; }
    [FirestoreProperty]
    public int Price { get; set; }

    public Ingredient() { }

    public Ingredient(string name, int quantity, int price) {
        Name = name;
        Quantity = quantity;
        Price = price;
    }

    public Ingredient(string name, int price) {
        Name = name;
        Quantity = 1;
        Price = price;
    }
}
