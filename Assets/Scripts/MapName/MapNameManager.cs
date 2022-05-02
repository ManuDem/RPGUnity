using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNameManager : MonoBehaviour
{
    [SerializeField] Text mapName;

    Vector3 originalLocation;
    bool animRunning;
    Resolution res;

    public static MapNameManager i { get; private set; }
    private void Awake()
    {
        i = this;
        originalLocation.y = this.transform.position.y;
        res = Screen.currentResolution;
    }

    public void setMapName(string mapName) {
        this.gameObject.GetComponent<Image>().DOFade(1f, 0.2f).From().SetEase(Ease.Linear);
        this.mapName.text = mapName;
    }


    public IEnumerator moveWitBothWays() {

      var sequence = DOTween.Sequence();


           if (!animRunning)
           {

           animRunning = true;

            this.gameObject.GetComponent<Image>().DOFade(0f, 0f).SetEase(Ease.Linear);
            this.mapName.DOFade(0f, 0.2f).SetEase(Ease.Linear);

            this.gameObject.SetActive(true);

            this.gameObject.GetComponent<Image>().DOFade(1f, 0.2f).SetEase(Ease.Linear);
            this.mapName.DOFade(1f, 0.2f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(2f);

            this.gameObject.GetComponent<Image>().DOFade(0f, 0.2f).SetEase(Ease.Linear);
            this.mapName.DOFade(0f, 0.2f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.2f);

            this.gameObject.SetActive(false);

            animRunning = false;
           }

   }


    /*public IEnumerator moveWitBothWays() {

       var sequence = DOTween.Sequence();

        if (!res.Equals(Screen.currentResolution)) {
            originalLocation.y = this.transform.position.y;
            res = Screen.currentResolution;
        }


        if (res.Equals( Screen.currentResolution))
        {
            if (!animRunning)
            {
                animRunning = true;

                sequence.Append(this.transform.DOMoveY(originalLocation.y - (originalLocation.y / 4), 0.5f).SetEase(Ease.Linear));
                yield return new WaitForSeconds(2);
                sequence.Append(this.transform.DOMoveY(originalLocation.y, 0.5f).SetEase(Ease.Linear));

                
                sequence.Append(this.transform.DOMoveY(originalLocation.y - 150f, 0.5f).SetEase(Ease.Linear));
                yield return new WaitForSeconds(2);
                sequence.Append(this.transform.DOMoveY(originalLocation.y, 0.5f).SetEase(Ease.Linear));
                

                animRunning = false;
            }
        }

      
    }*/


}
