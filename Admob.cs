using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class Admob : MonoBehaviour
{ 

    [Header("Admob Ads ID's")]
    public string appId = "ca-app-pub-5045763199037674~4309885548";
    [SerializeField] string AndriodInterstitial;
    [SerializeField] string IosInterstititial;

    [SerializeField] string AndriodRewarded;
    [SerializeField] string IosRewarded;



    [Header("Unity Ads ID's")]
    [SerializeField] string _androidGameId= "4626987";
    [SerializeField] string _iOSGameId= "4626986";
    [SerializeField] bool _testMode = true;
    private string _gameId;





    [SerializeField]  string _AndriodUnityInter = "Interstitial_Android";
    [SerializeField] string _iOsUnityInter = "Interstitial_iOS";
    string _InterUnitId;




    [SerializeField] string _AndriodUnityReward = "Rewarded_Android";
    [SerializeField] string _iOsUnityReward = "Rewarded_iOS";
    string _rewardUnitId;






    public static Admob _instance;
    

    private BannerView bannerView;
    [HideInInspector] public InterstitialAd interstitial;
    [HideInInspector] public RewardedAd rewardedAd;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance != this || _instance == null) { _instance = this; }
        MobileAds.Initialize(initStatus => { });
        RequestInterstitial();
        RequestRewarded();
        InitializeAds();
        RequestUniyIntertitial();

    }


    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode);
    }





    #region Admob
    private void RequestBanner()
    {

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);


        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);
    }

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = AndriodInterstitial;// "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId =IosInterstititial;// "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public bool LoadIntertitial()
    {
        bool _check = true;
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
            _check = true;
        }
        else { _check = false; }
        RequestInterstitial();
        return _check;
    }

    public void RequestRewarded()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = AndriodRewarded;// "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId =IosRewarded; //"ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);


        AdRequest request = new AdRequest.Builder().Build();

        this.rewardedAd.LoadAd(request);
    }

    public void LoadRewardedvedio()
    {

        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
        RequestRewarded();

    }


    public void Get100CoinsFree(object sender, Reward args)
    {
        //setting reward in user coins
        Debug.Log("100 Coins Wins");
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 100);
        UIManage._instance.coinText.text = PlayerPrefs.GetInt("Coins").ToString();
        this.rewardedAd.OnUserEarnedReward -= Get100CoinsFree;
    }
    #endregion

    #region UnityAds
    public void RequestUniyIntertitial()
    {

        _InterUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsUnityInter
            : _AndriodUnityInter;
        Advertisement.Load(_InterUnitId);
       
    }
    public bool LoadUnityIntertitial(ShowOptions _option)
    {
        bool _check = true;
        if (Advertisement.IsReady(_InterUnitId))
        {
            Advertisement.Show(_InterUnitId, _option);
        }
        else { _check = false; }
        RequestUniyIntertitial();
        return _check;
    }

    public void RequestUnityRewarded()
    {
        _rewardUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
           ? _iOsUnityReward
           : _AndriodUnityReward;
        Advertisement.Load(_rewardUnitId);
    }
    public bool LoadUnityRewarded(ShowOptions _option)
    {
        bool _check = true;
        if (Advertisement.IsReady(_rewardUnitId))
        {
            Advertisement.Show(_rewardUnitId,_option);
        }
        else { _check = false; }
        RequestUnityRewarded();
        return _check;
    }


    public void GetCoinsFromUnityRewardAds(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 100);
                UIManage._instance.coinText.text = PlayerPrefs.GetInt("Coins").ToString();
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    
    #endregion


}






