using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* e�er k�p 90 derecenin tam katlar� olarak d�nerse 90,180,270 ve 360 derece d�nd��� zaman kar��dan bak�ld���na ayn� g�z�k�r.
 * k�p d�nmeye ba�lad��� zaman hep sabit olan referans k�p (rotation de�erleri = 0,0,0) ile aras�ndaki a�� �l��l�yor.
 * bu a�� 90 derecenin tam katlar� oldu�u zaman skoru 1 artt�r�yor.
 * ard�ndan zaman ak��� false olup 3 saniye sonra coroutine ile tekrar true oluyor. her a�� i�in 5 derece hata pay� verildi.
 */
public class timeManager : MonoBehaviour
{
    public TextMeshProUGUI scoreController;
    public bool timeControl = true;   //zaman ak���n� kontrol etme
    private int initialValue;   // ilk skor de�eri
    private float angleValue;   //a�� de�eri
    public int myScore;    // skor
    public GameObject myCube;   //d�nen k�p
    public float rotatingSpeed; //d�nme h�z�
    public GameObject passiveCube;  //referans ama�l� pasif k�p ama bo� obe olarak olu�turuldu.
    [SerializeField] private float timeDelay;

    void Start()
    {
        initialValue = 0;
    }

    IEnumerator timeStopper()
    {
        yield return new WaitForSeconds(timeDelay);
        timeControl = true;
    }

    void Update()
    {
        myCube.transform.Rotate(rotatingSpeed*Time.deltaTime, 0, 0);    //k�p d�nmesi
        angleValue = Quaternion.Angle(myCube.transform.rotation, passiveCube.transform.rotation);   //d�nen k�p ve referans k�p aras�ndaki a��
    }

    public void scoreUpdater()
    {
        if (angleValue > 85f && angleValue < 95f)   //Quaternion.Angle iki nesne aras�ndaki a�� eksi olsa bile art� g�steriyor. +90 ve -90 (270) kontorl�
        {
            initialValue = 1;
            timeDelay = Mathf.Abs(90 - angleValue);    // butona t�klan�ld���nda 90 dereceden ne kadar uzaksa o kadar �ok t�klama bekleme s�resi oluyor.
            // butona t�klan�ld���nda a�� 86.5 ise bekleme s�resi 3.5 saniye. 90.1 ise 0.1 saniye bekleme s�resi.
        }
        else if (angleValue > 175f)                 // 175 ve -175 (185)
        {
            initialValue = 1;
            timeDelay = Mathf.Abs(180 - angleValue);
        }
        else if (angleValue < 5f)                   // 5 ve -5 (365)
        {
            initialValue = 1;
            timeDelay = Mathf.Abs(0 - angleValue);
        }

        if (timeControl == true && initialValue != 0)
        {
            myScore += initialValue;
            scoreController.text = "skor: " + myScore.ToString();
            initialValue = 0;
            timeControl = false;
            StartCoroutine(timeStopper());
        }
    }
}