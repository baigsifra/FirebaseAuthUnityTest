using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramen
{
    public string Name { get; set; }
    public List<Ingredient> IngredientsNeeded { get; set; }
    public int MarketPrice { get; set; }
    public int LevelUnlocked { get; set; }

    public Ramen(string name, List<Ingredient> ingredientsNeeded, int levelUnlocked) {
        Name = name;
        LevelUnlocked = levelUnlocked;
        IngredientsNeeded = ingredientsNeeded;
        foreach(Ingredient i in IngredientsNeeded) {
            MarketPrice += i.Price;
        }
    }
}
