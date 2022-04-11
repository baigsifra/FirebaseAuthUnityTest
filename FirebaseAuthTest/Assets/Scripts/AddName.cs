using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

public class AddName : MonoBehaviour
{
    [Header("Firebase")]
    public FirebaseFirestore db;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Space]
    [Header("Data")]
    public Text storeName;

    [Space]
    [Header("User")]
    public User userData;

    void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuthManager.auth;
        user = FirebaseAuthManager.user;
        userData = FirebaseHelper.userData;
        storeName.text = userData.StoreName;
    }
}
