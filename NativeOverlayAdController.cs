using System;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UIElements;

namespace GoogleMobileAds.Sample
{
    /// <summary>
    /// Demonstrates how to use Google Mobile Ads native ads.
    /// </summary>
    [AddComponentMenu("GoogleMobileAds/Samples/NativeOverlayAdController")]
    public class NativeOverlayAdController : MonoBehaviour
    {
        // These ad units are configured to always serve test ads.
        public const string _adUnitIdReal= "ca-app-pub-4729985843912961/7960221460";
#if UNITY_ANDROID
        private const string _adUnitId = "ca-app-pub-3940256099942544/2247696110";
#elif UNITY_IPHONE
        private const string _adUnitId = "ca-app-pub-3940256099942544/3986624511";
#else
        private const string _adUnitId = "unused";
#endif

        public GameObject AdLoadedStatus;

        public RectTransform AdPlacmentTarget;
        
        public NativeAdOptions Option = new NativeAdOptions
        {
            AdChoicesPlacement = AdChoicesPlacement.TopRightCorner,
            MediaAspectRatio = MediaAspectRatio.Square,
        };
        public NativeTemplateStyle Style = new NativeTemplateStyle
        {
            TemplateId = NativeTemplateId.Medium,
        };

        private NativeOverlayAd _nativeOverlayAd;
        private void Start()
        {
            LoadAd();
        }
        public void LoadAd()
        {
            // Clean up the old ad before loading a new one.
            if (_nativeOverlayAd != null)
            {
                DestroyAd();
            }
         
            Debug.Log("Loading native overlay ad.");

            var adRequest = new AdRequest();
            NativeOverlayAd.Load(_adUnitId, adRequest, Option,
                (NativeOverlayAd ad, LoadAdError error) =>
                {
                Debug.LogError("Native Overlay ad failed to load an ad with error ");
                // If the operation failed with a reason.
                if (error != null)
                {
                    Debug.LogError("Native Overlay ad failed to load an ad with error : " + error);
                    return;
                }
                // If the operation failed for unknown reasons.
                // This is an unexpected error, please report this bug if it happens.
                if (ad == null)
                {
                    Debug.LogError("Unexpected error: Native Overlay ad load event fired with " +
                    " null ad and null error.");
                    return;
                }

                // The operation completed successfully.
                Debug.Log("Native Overlay ad loaded with response : " + ad.GetResponseInfo());
                _nativeOverlayAd = ad;

                // Register to ad events to extend functionality.
                RegisterEventHandlers(ad);

                // Inform the UI that the ad is ready.
                AdLoadedStatus?.SetActive(true);
                RenderAd();
            });
        }

        private void RegisterEventHandlers(NativeOverlayAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Native Overlay ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Native Overlay ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Native Overlay ad was clicked.");
            };
            // Raised when the ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Native Overlay ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Native Overlay ad full screen content closed.");
            };
        }
        public void ShowAd()
        {
            if (_nativeOverlayAd != null)
            {
                Debug.Log("Showing Native Overlay ad.");
                _nativeOverlayAd.Show();
            }
        }

        public void HideAd()
        {
            if (_nativeOverlayAd != null)
            {
                Debug.Log("Hiding Native Overlay ad.");
                _nativeOverlayAd.Hide();
            }
        }

        /// <summary>
        /// Renders the ad.
        /// </summary>
        //public void RenderAd()
        //{
        //    if (_nativeOverlayAd != null)
        //    {
        //        int width = 500;
        //        int heith = 500;
        //        Debug.Log("Rendering Native Overlay ad.");
        //        AdSize adSize = new AdSize(width, heith);
        //        _nativeOverlayAd.RenderTemplate(Style, adSize, AdPosition.Center);


        //    }
        //}
        public void RenderAd()
        {
            if (_nativeOverlayAd != null && AdPlacmentTarget != null)
            {
                Debug.Log("Rendering Native Overlay ad.");

                Vector3 position = AdPlacmentTarget.position; // World position
                Rect rect = AdPlacmentTarget.rect; // Size of the RectTransform
                int width = Mathf.RoundToInt(rect.width);
                int height = Mathf.RoundToInt(rect.height);
                AdSize adSize = new AdSize(width, height);
                _nativeOverlayAd.RenderTemplate(Style, adSize, AdPosition.Center/*100,320*/);
            }
            else
            {
                Debug.LogWarning("Native Overlay ad or AdPlacementTarget is not set.");
            }
        }

        public void DestroyAd()
        {
            if (_nativeOverlayAd != null)
            {
                Debug.Log("Destroying Native Overlay ad.");
                _nativeOverlayAd.Destroy();
                _nativeOverlayAd = null;
            }
            AdLoadedStatus?.SetActive(false);
        }

        public void LogResponseInfo()
        {
            if (_nativeOverlayAd != null)
            {
                var responseInfo = _nativeOverlayAd.GetResponseInfo();
                if (responseInfo != null)
                {
                    Debug.Log(responseInfo);
                }
            }
        }
    }
}
