using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* eðer küp 90 derecenin tam katlarý olarak dönerse 90,180,270 ve 360 derece döndüðü zaman karþýdan bakýldýðýna ayný gözükür.
 * küp dönmeye baþladýðý zaman hep sabit olan referans küp (rotation deðerleri = 0,0,0) ile arasýndaki açý ölçülüyor.
 * bu açý 90 derecenin tam katlarý olduðu zaman skoru 1 arttýrýyor.
 * ardýndan zaman akýþý false olup 3 saniye sonra coroutine ile tekrar true oluyor. her açý için 5 derece hata payý verildi.
 */
public class timeManager : MonoBehaviour
{
    public TextMeshProUGUI scoreController;
    public bool timeControl = true;   //zaman akýþýný kontrol etme
    private int initialValue;   // ilk skor deðeri
    private float angleValue;   //açý deðeri
    public int myScore;    // skor
    public GameObject myCube;   //dönen küp
    public float rotatingSpeed; //dönme hýzý
    public GameObject passiveCube;  //referans amaçlý pasif küp ama boþ obe olarak oluþturuldu.
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
        myCube.transform.Rotate(rotatingSpeed*Time.deltaTime, 0, 0);    //küp dönmesi
        angleValue = Quaternion.Angle(myCube.transform.rotation, passiveCube.transform.rotation);   //dönen küp ve referans küp arasýndaki açý
    }

    public void scoreUpdater()
    {
        if (angleValue > 85f && angleValue < 95f)   //Quaternion.Angle iki nesne arasýndaki açý eksi olsa bile artý gösteriyor. +90 ve -90 (270) kontorlü
        {
            initialValue = 1;
            timeDelay = Mathf.Abs(90 - angleValue);    // butona týklanýldýðýnda 90 dereceden ne kadar uzaksa o kadar çok týklama bekleme süresi oluyor.
            // butona týklanýldýðýnda açý 86.5 ise bekleme süresi 3.5 saniye. 90.1 ise 0.1 saniye bekleme süresi.
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