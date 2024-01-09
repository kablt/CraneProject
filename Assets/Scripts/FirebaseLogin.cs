using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class FirebaseLogin : MonoBehaviour
{
    private FirebaseAuth auth;

    public TMP_InputField email;
    public TMP_InputField password;


    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Create()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("계정 생성 실패");
                return;
            }
            if (task.IsCanceled)
            {
                Debug.Log("계정 생성 취소됨");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("계정 생성 성공");
        });

    }
    public async void Login()
    {
        try
        {
            await auth.SignInWithEmailAndPasswordAsync(email.text, password.text);
            Debug.Log("로그인 성공");
            SceneManager.LoadScene("SampleScene");
        }
        catch (Exception ex)
        {
            Debug.Log("로그인 실패: " + ex.Message);
        }
    }
    //public void Login()
    //{
    //    auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            Debug.Log("로그인 실패");
    //            return;
    //        }
    //        if (task.IsCanceled)
    //        {
    //            Debug.Log("로그인 취소됨");
    //            return;
    //        }
    //        else
    //        {
    //            Debug.Log("로그인 성공");
    //            SceneManager.LoadScene("SampleScene");
    //        }

    //    });
    //}

    //public void Logout()
    //{
    //    auth.SignOut();
    //    Debug.Log("로그아웃");
    //}

    // Update is called once per frame
    void Update()
    {

    }
}
