using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mkey;
public class CoinManager : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject wrenchPrefab;
    public Transform coinParent;
    public Transform wrenchParent;
    public Transform coinStart;
    public Transform wrenchStart;
    public Transform coinEnd;
    public Transform wrenchEnd;
    public float moveduaration;
    public Ease moveEase;
    public int coinAmount;
    public int wrenchAmount;
    public float coinperDelay;
    public GameObject openLevelReaward;
    public float wrenchperDelay;
    public AudioClip coins;
    public AudioClip wrench;
    public bool isPlay;
    public static CoinManager instance;
    private void Start()
    {
        instance = this;
        StartWrenchCoinAnimation();
    }
    
    public void StartWrenchCoinAnimation()
    {
        StartCoroutine(PlayAnimation());
    }
   
    public IEnumerator PlayAnimation()
    {
        WrenchAnim();
        yield return new WaitForSeconds(1.2f);
        CoinsAnim();
        yield return new WaitForSeconds(1.5f);
        if (PlayerPrefs.GetInt("Levelreward") == 1)
        {
           openLevelReaward.SetActive(true);
            PlayerPrefs.SetInt("Levelreward", 0);
        }
        //yield return new WaitForSeconds(4f);
      
    }
  
    public void CoinsAnim()
    {
        if (GetComponent<AudioSource>() != null )
        {
            GetComponent<AudioSource>().PlayOneShot(coins);

        }
        for (int i = 0; i <= coinAmount; i++)
        {
            var targetDelay = i * coinperDelay;
            CoinAnimation(targetDelay);
        }
    }
    public void WrenchAnim()
    {
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().PlayOneShot(wrench);

        }
        for (int i = 0; i <= wrenchAmount; i++)
        {
            var targetDelay = i * wrenchperDelay;
            WrenchAnimation(targetDelay);
        }
    }
    public void CoinAnimation(float delay)
    {
        var coinObject = Instantiate(coinPrefab, coinParent);
        var offset = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0f);
        var startPos = offset + coinStart.transform.position;
        coinObject.transform.position = startPos;
        coinObject.transform.localScale = new Vector3(.1f, .1f, .1f);
        coinObject.transform.DOScale(Vector3.one, delay);
        
        coinObject.transform.DOMove(coinEnd.position, moveduaration).SetEase(moveEase).SetDelay(delay).OnComplete(() =>
        {
            Destroy(coinObject);
        });
    }
   
    public void WrenchAnimation(float delay)
    {
        var coinObject = Instantiate(wrenchPrefab, wrenchParent);
        var offset = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0f);
        var startPos = offset + wrenchStart.transform.position;
        coinObject.transform.position = startPos;
        coinObject.transform.localScale = new Vector3(.1f, .1f, .1f);
        coinObject.transform.DOScale(Vector3.one, delay);
        coinObject.transform.DOMove(wrenchEnd.position, moveduaration).SetEase(moveEase).SetDelay(delay).OnComplete(() => 
        {
            Destroy(coinObject);
        });
    }
}
