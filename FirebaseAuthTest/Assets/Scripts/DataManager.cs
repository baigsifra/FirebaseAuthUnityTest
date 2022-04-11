using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static List<Ramen> allRamen;
    public static List<Ramen> availableRamen;

    void Awake() {
        allRamen = new List<Ramen>();
        availableRamen = new List<Ramen>();
        
        allRamen.Add(new Ramen("[NAME]", new List<Ingredient> {
          new Ingredient("miso broth", 1, 10),
          new Ingredient("carrot", 1, 10),
          new Ingredient("pork", 1, 10),
          new Ingredient("noodles", 1, 10)
          }, 1));

        allRamen.Add(new Ramen("[NAME]", new List<Ingredient> {
          new Ingredient("miso broth", 1, 10),
          new Ingredient("corn", 1, 10),
          new Ingredient("tofu", 1, 10),
          new Ingredient("noodles", 1, 10)
          }, 2));

        allRamen.Add(new Ramen("[NAME]", new List<Ingredient> {
          new Ingredient("miso broth", 1, 10),
          new Ingredient("carrot", 1, 10),
          new Ingredient("corn", 1, 10),
          new Ingredient("fishcake", 1, 10),
          new Ingredient("noodles", 1, 10)
          }, 2));

        allRamen.Add(new Ramen("[NAME]", new List<Ingredient> {
          new Ingredient("miso broth", 1, 10),
          new Ingredient("spinach", 1, 10),
          new Ingredient("tomato", 1, 10),
          new Ingredient("soft-boiled eggs", 1, 10),
          new Ingredient("noodles", 1, 10)
          }, 3));

        allRamen.Add(new Ramen("[NAME]", new List<Ingredient> {
          new Ingredient("miso broth", 1, 10),
          new Ingredient("pepper", 1, 10),
          new Ingredient("mushroom", 1, 10),
          new Ingredient("fishcake", 1, 10),
          new Ingredient("pork", 1, 10),
          new Ingredient("noodles", 1, 10)
          }, 3));

        allRamen.Add(new Ramen("[NAME]", new List<Ingredient> {
          new Ingredient("miso broth", 1, 10),
          new Ingredient("spinach", 1, 10),
          new Ingredient("corn", 1, 10),
          new Ingredient("pepper", 1, 10),
          new Ingredient("pork", 1, 10),
          new Ingredient("soft-boiled eggs", 1, 10),
          new Ingredient("noodles", 1, 10)
          }, 4));

        allRamen.Add(new Ramen("[NAME]", new List<Ingredient> {
          new Ingredient("miso broth", 1, 10),
          new Ingredient("spinach", 1, 10),
          new Ingredient("tomato", 1, 10),
          new Ingredient("pepper", 1, 10),
          new Ingredient("mushroom", 1, 10),
          new Ingredient("tofu", 1, 10),
          new Ingredient("fishcake", 1, 10),
          new Ingredient("noodles", 1, 10)
          }, 4));

        updateAvailableRecipes();
    }

    public static void updateAvailableRecipes() {
        Debug.Log(allRamen);
        int userLevel = FirebaseHelper.userData.Level;
        foreach(Ramen r in allRamen) {
            int ramenLevel = r.LevelUnlocked;
            if(ramenLevel <= userLevel) {
                availableRamen.Add(r);
            }
        }
    }
}
