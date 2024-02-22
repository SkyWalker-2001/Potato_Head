using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    public static Action OnDiscoBallHitEvent;


    [SerializeField] private float _discoBallPartyTime = 2f;

    [SerializeField] private float _discoGlobalLightIntensity = .2f;
    [SerializeField] private Light2D _globalLight;

    private float _defaultGlobalIntensity;
    private Coroutine _discoCoroutine;
    private Color_SpotLight[] _allSpotLights;

    private void Awake()
    {
        _defaultGlobalIntensity = _globalLight.intensity;
    }

    private void Start()
    {
        _allSpotLights = FindObjectsByType<Color_SpotLight>(FindObjectsSortMode.None);
    }

    private void OnEnable()
    {
        OnDiscoBallHitEvent += DimTheLight;
    }

    private void OnDisable()
    {
        OnDiscoBallHitEvent -= DimTheLight;
    }

    public void DiscoBallParty()
    {
        if(_discoCoroutine != null)
        {
            return;
        }

        OnDiscoBallHitEvent?.Invoke();
    }

    private void DimTheLight()
    {
        foreach (Color_SpotLight spotLight in _allSpotLights)
        {
            StartCoroutine(spotLight.SpotLightDiscoParty(_discoBallPartyTime));
        }
        _discoCoroutine = StartCoroutine(GloabalLightResetRoutine());
    }

    private IEnumerator GloabalLightResetRoutine()
    {
        _globalLight.intensity = _discoGlobalLightIntensity;
        yield return new WaitForSeconds(_discoBallPartyTime);
        _globalLight.intensity = _defaultGlobalIntensity;
        _discoCoroutine = null;
    }
} 
