using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color_SpotLight : MonoBehaviour
{
    [SerializeField] private GameObject _spotLightHead;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _discoRoteSpeed = 120f;
    [SerializeField] private float _maxRotation = 45f;

    private float _currentRotation;

    private void Start()
    {
        RandomStartingRotation();
    }

    private void Update()
    {
        RotateHead();
    }

    public IEnumerator SpotLightDiscoParty(float discopartyTime)
    {
        float defaultRotationSpeed = _rotationSpeed;
        _rotationSpeed = _discoRoteSpeed;
        yield return new WaitForSeconds(discopartyTime);
        _rotationSpeed = defaultRotationSpeed;
    }

    private void RotateHead()
    {
        _currentRotation += Time.deltaTime * _rotationSpeed;
        float z = Mathf.PingPong(_currentRotation, _maxRotation);
        _spotLightHead.transform.localRotation = Quaternion.Euler(0f, 0f, z);
    }

    private void RandomStartingRotation()
    {
        float randomStartingZ = Random.Range(-_maxRotation, _maxRotation);
        _spotLightHead.transform.localRotation = Quaternion.Euler(0f, 0f, randomStartingZ);
        _currentRotation = randomStartingZ + _maxRotation;
        _currentRotation = randomStartingZ + _maxRotation;

    }
}
