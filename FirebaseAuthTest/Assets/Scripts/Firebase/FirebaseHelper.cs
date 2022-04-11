using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

public class FirebaseHelper : MonoBehaviour
{
    [Header("Firebase")]
    public FirebaseFirestore db;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Space]
    [Header("StoreName")]
    public InputField storeNameField;

    [Space]
    [Header("Ingredient")]
    public InputField ingredientNameField;
    public InputField ingredientQuantityField;

    [Space]
    [Header("User")]
    public static User userData;

    private void Awake() {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuthManager.auth;
        user = FirebaseAuthManager.user;
        getUserDataAsync();
    }

    public void addUserToFirebase() {
        StartCoroutine(addUserToFirebaseAsync(storeNameField.text));
    }

    private IEnumerator addUserToFirebaseAsync(string storeName) {
        DocumentReference docRef = db.Collection(user.UserId).Document(user.UserId);

        FirebaseHelper.userData = new User(storeName);

        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "StoreName", storeName },
            { "Currency", 0 },
            { "Level", 1 },
            { "TotalExp", 0 },
            { "CurrentExp", 0 },
            { "TotalProfit", 0 },
            { "TotalTips", 0 },
            { "CompletedOrders", 0 }
        };

        var setDataTask = docRef.SetAsync(data);
        yield return new WaitUntil(() => setDataTask.IsCompleted);
        Debug.Log("Completed");

        getUserDataAsync();

        UnityEngine.SceneManagement.SceneManager.LoadScene("AddInventory");
    }

    public void addIngredientToInventory() {
        StartCoroutine(addIngredientToInventoryAsync(new Ingredient(ingredientNameField.text, Convert.ToInt32(ingredientQuantityField.text), 0)));
    }

    private IEnumerator addIngredientToInventoryAsync(Ingredient i) {
        DocumentReference docRef = db.Collection(user.UserId).Document(user.UserId).Collection("inventory").Document(i.Name);
        var setDataTask = docRef.SetAsync(i);
        yield return new WaitUntil(() => setDataTask.IsCompleted);
        getUserDataAsync();
        Debug.Log("Added Ingredient");
    }

    public void updateIngredientInInventory(Ingredient i) {
        StartCoroutine(updateIngredientInInventoryAsync(i));
    }

    private IEnumerator updateIngredientInInventoryAsync(Ingredient i) {
        DocumentReference docRef = db.Collection(user.UserId).Document(user.UserId).Collection("inventory").Document(i.Name);
        Dictionary<string, object> updates = new Dictionary<string, object> {
            { "Quantity", i.Quantity }
        };
        var updateDataTask = docRef.UpdateAsync(updates);
        yield return new WaitUntil(() => updateDataTask.IsCompleted);
        getUserDataAsync();
        Debug.Log("Ingredient Updated");
    }

    public void deleteIngredientInInventory(string iName) {
        StartCoroutine(deleteIngredientInInventoryAsync(iName));
    }

    private IEnumerator deleteIngredientInInventoryAsync(string iName) {
        DocumentReference docRef = db.Collection(user.UserId).Document(user.UserId).Collection("inventory").Document(iName);
        var deletedDataTask = docRef.DeleteAsync();
        yield return new WaitUntil(() => deletedDataTask.IsCompleted);
        getUserDataAsync();
        Debug.Log("Ingredient Deleted");
    }

    public async void getUserDataAsync() {
        DocumentReference docRef = db.Collection(user.UserId).Document(user.UserId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists) {
            string sname = snapshot.GetValue<string>("StoreName");
            int currency = snapshot.GetValue<int>("Currency");
            int level = snapshot.GetValue<int>("Level");
            int totalExp = snapshot.GetValue<int>("TotalExp");
            int currentExp = snapshot.GetValue<int>("CurrentExp");
            int totalProfit = snapshot.GetValue<int>("TotalProfit");
            int totalTips = snapshot.GetValue<int>("TotalTips");
            int completedOrders = snapshot.GetValue<int>("CompletedOrders");
            FirebaseHelper.userData = new User(sname, currency, level, totalExp, currentExp, totalProfit, totalTips, completedOrders, new List<Ingredient>());
            CollectionReference invRef = docRef.Collection("inventory");
            QuerySnapshot qs = await invRef.GetSnapshotAsync();
            if(qs.Count != 0) {
                List<Ingredient> userInv = new List<Ingredient>();
                foreach(DocumentSnapshot documentSnapshot in qs.Documents) {
                    Ingredient ing = documentSnapshot.ConvertTo<Ingredient>();
                    userInv.Add(ing);
                }
                FirebaseHelper.userData.Inventory = userInv;
                Debug.Log("Attached Inventory");
            }
            Debug.Log("Attached User Data");
        } else {
            Debug.Log("Document " + snapshot.Id + " does not exist!");
        }
    }

    public void buyIngredient() {
        StartCoroutine(buyIngredientAsync(new Ingredient("miso broth", 1, 0)));
    }

    private IEnumerator buyIngredientAsync(Ingredient i) {
        i.Quantity = FirebaseHelper.userData.addToInventory(i);
        FirebaseHelper.userData.Currency -= i.Price;

        StartCoroutine(addIngredientToInventoryAsync(i));
        DocumentReference docRef = db.Collection(user.UserId).Document(user.UserId);

        Dictionary<string, object> updates = new Dictionary<string, object> {
            { "Currency", FirebaseHelper.userData.Currency }
        };
        var setDataTask = docRef.UpdateAsync(updates);
        yield return new WaitUntil(() => setDataTask.IsCompleted);
        getUserDataAsync();
        Debug.Log("Bought Ingredient");
    }

    public void customerPaidTest() {
        customerPaid(25, 16, 89);
    }

    public void customerPaid(int priceOfRamen, int marketPriceOfRamen, int ramenScore) {
        // These will be constants
        double MAX_TIP = 0.6, HIGH_TIP = 0.4, MEDIUM_TIP = 0.2, LOW_TIP = 0.1, MIN_TIP = .05;
        int MAX_EXTRA_EXP = 15, HIGH_EXTRA_EXP = 10, MEDIUM_EXTRA_EXP = 5, LOW_EXTRA_EXP = 3, MIN_EXTRA_EXP = 0;
        int BASE_EXP = 50;
        // Regular variables
        int profit = priceOfRamen - marketPriceOfRamen;
        int tip = 0;
        int extraExp = 0;

        if(ramenScore >= 80 && ramenScore <= 100) {
            tip = (int)(priceOfRamen * MAX_TIP);
            extraExp = MAX_EXTRA_EXP;
        } else if(ramenScore >= 60 && ramenScore < 80) {
          tip = (int)(priceOfRamen * HIGH_TIP);
          extraExp = HIGH_EXTRA_EXP;
        } else if(ramenScore >= 40 && ramenScore < 60) {
          tip = (int)(priceOfRamen * MEDIUM_TIP);
          extraExp = MEDIUM_EXTRA_EXP;
        } else if(ramenScore >= 20 && ramenScore < 40) {
          tip = (int)(priceOfRamen * LOW_TIP);
          extraExp = LOW_EXTRA_EXP;
        } else if(ramenScore >= 0 && ramenScore < 20) {
          tip = (int)(priceOfRamen * MIN_TIP);
          extraExp = MIN_EXTRA_EXP;
        } else {
            Debug.Log("Ramen Score is either below 0 or above 100");
        }

        profit += tip;
        int exp = BASE_EXP + extraExp;

        customerPaidUserDataUpdate(profit, exp, tip);
    }

    public void customerPaidUserDataUpdate(int profit, int exp, int tip) {
        StartCoroutine(cusPaidUserDataUpdateAsync(profit, exp, tip));
    }

    private IEnumerator cusPaidUserDataUpdateAsync(int profit, int exp, int tip) {
        // Update the user's totalcompletedorders in Firestore (+1)
        getUserDataAsync();
        FirebaseHelper.userData.CompletedOrders += 1;
        // Update the user's currency in Firestore (+profit)
        FirebaseHelper.userData.Currency += profit;
        // Update the user's totalProfit in Firestore (+profit)
        FirebaseHelper.userData.TotalProfit += profit;
        // Update the user's totalTips in Firestore (+tip)
        FirebaseHelper.userData.TotalTips += tip;
        // Update the user's exp -> addExp(exp)
        FirebaseHelper.userData.TotalExp += exp;
        updateLevel(exp);

        DocumentReference docRef = db.Collection(user.UserId).Document(user.UserId);

        Dictionary<string, object> updates = new Dictionary<string, object> {
            { "CompletedOrders", FirebaseHelper.userData.CompletedOrders },
            { "Currency", FirebaseHelper.userData.Currency },
            { "TotalProfit", FirebaseHelper.userData.TotalProfit },
            { "TotalExp", FirebaseHelper.userData.TotalExp },
            { "TotalTips", FirebaseHelper.userData.TotalTips },
            { "CurrentExp", FirebaseHelper.userData.CurrentExp },
            { "Level", FirebaseHelper.userData.Level },
        };
        var setDataTask = docRef.UpdateAsync(updates);
        yield return new WaitUntil(() => setDataTask.IsCompleted);
        getUserDataAsync();
        Debug.Log("Customer Paid");
    }

    public void updateLevel(int exp) {
        // This will be a constant
        int LEVEL_UP_EXP = 200;
        int MAX_LEVEL = 5;

        int neededExp = userData.Level * LEVEL_UP_EXP;

        if((userData.CurrentExp + exp) < neededExp) {
            userData.CurrentExp += exp;
        } else {
            if(userData.Level != MAX_LEVEL) {
                userData.Level += 1;
                DataManager.updateAvailableRecipes();
                int newExp = (userData.CurrentExp + exp) - neededExp;
                userData.CurrentExp = newExp;
                Debug.Log("User has leveled up! New recipes unlocked (TODO)");
            } else {
                Debug.Log("User is maxxed out");
            }
        }
    }

    public bool hasIngredients(Ramen r) {
        foreach(Ingredient i in r.IngredientsNeeded) {
            bool hasIngredient = false;
            foreach(Ingredient userI in userData.Inventory) {
                if(userI.Name == i.Name) {
                    hasIngredient = true;
                }
            }
            if(!hasIngredient) {
                return false;
            }
        }
        return true;
    }

    public void makeRamen(Ramen r) {
        if(hasIngredients(r)) {
          foreach(Ingredient i in r.IngredientsNeeded) {
              foreach(Ingredient userI in userData.Inventory) {
                  if(userI.Name == i.Name) {
                      userI.Quantity -= 1;
                      if(userI.Quantity == 0) {
                          deleteIngredientInInventory(userI.Name);
                      } else {
                          updateIngredientInInventory(userI);
                      }
                  }
              }
          }

          getUserDataAsync();
          Debug.Log("Ingredients removed from inventory. Can now make ramen !");
        } else {
          Debug.Log("Does not have enough ingredients.");
        }
    }

    public void goForward() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StatisticsScreen");
    }

    public void goBack() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("AddInventory");
    }

    public void goSettings() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SettingsScreen");
    }
}
