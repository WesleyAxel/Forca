using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditButton : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {

    }
    public void GoToCredits()
    {
        SceneManager.LoadScene("Lab1_creditos");
    }
}
