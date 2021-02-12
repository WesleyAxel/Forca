using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int numTentativas;                              //quantas tentativas
    private int maxNumTentativas;                           //maximo de tentativas
    int score = 0;

    public GameObject letra;                                //prefab da letra
    public GameObject centro;                               //objeto que indica o centro da tela

    private string palavraOculta = "";                      //a palavra a ser descoberta
    //private string[] palavrasOcultas = new string[] {"carro","elefante","futebol","aviao","bola","celular","ventilador","tenis","teclado","mouse"};
    private int tamanhoPalavraOculta;                       //tamanho da palavra a ser descoberta
    char[] letrasOcultas;                                   //letras da palavra a ser descoberta
    bool[] letrasDescobertas;                               //indicador de quais letras foram descobertas

    void Start()
    {
        centro = GameObject.Find("centroDaTela");
        InitGame();
        InitLetras();
        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();
    }

    
    void Update()
    {
        checkTeclado();
    }

    void InitLetras() {

        int numLetras = tamanhoPalavraOculta;
        for(int i=0; i<numLetras; i++)
        {
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i-numLetras/2.0f)*80), centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject) Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i + 1);
            l.transform.SetParent(GameObject.Find("Canvas").transform);

        }
    
    }

    void InitGame()
    {
        palavraOculta = pegaPalavraArquivo();
        tamanhoPalavraOculta = palavraOculta.Length;
        palavraOculta = palavraOculta.ToUpper();
        letrasOcultas = new char[tamanhoPalavraOculta];
        letrasDescobertas = new bool[tamanhoPalavraOculta];
        letrasOcultas = palavraOculta.ToCharArray();
    }

    void checkTeclado()
    {
        if (Input.anyKeyDown)
        {
            char letraTeclada = Input.inputString.ToCharArray()[0];
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);
            
            if (letraTecladaComoInt >= 97 && letraTecladaComoInt <= 122)
            {
                numTentativas++;
                UpdateNumTentativas();
                if (numTentativas > maxNumTentativas)
                {
                    SceneManager.LoadScene("Lab1_forca");
                }

                for(int i=0; i<=tamanhoPalavraOculta; i++)
                {
                    if(!letrasDescobertas[i])
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        if(letrasOcultas[i] == letraTeclada)
                        {
                            letrasDescobertas[i] = true;
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            verificaSePalavraDescoberta();
                        }
                    }
                }
            }
        }
    }
    
    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " | " + maxNumTentativas;
    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score: " + score;
    }

    void verificaSePalavraDescoberta()
    {
        bool condicao = true;
        for (int i = 0; i< tamanhoPalavraOculta; i++)
        {
            condicao = condicao && letrasDescobertas[i];
        }
        if (condicao)
        {
            PlayerPrefs.SetString("ultimaPalavraOculta", palavraOculta);
            SceneManager.LoadScene("Lab1_salvo");
        }
    }

    string pegaPalavraArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int palavraAleatorio = Random.Range(0, palavras.Length + 1);
        return (palavras[palavraAleatorio]);
    }
}
