
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustVFX;
    [SerializeField] private ParticleSystem _poofDustVFX;
    [SerializeField] private float _tiltAngle = 20f;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private Transform _characterSpriteTransform;
    [SerializeField] private Transform _characterCowboyHatTransform;
    [SerializeField] private float _cowboyHatTiltModifer = 2f;


    private void Update()
    {
        DetectMoveDust();
        ApplyTilt();
    }

    private void OnEnable()
    {
        PlayerController.OnJump += PlayPoofDustVFX;
    }

    private void OnDisable()
    {
        PlayerController.OnJump -= PlayPoofDustVFX;
    }

    private void DetectMoveDust()
    {
        if (PlayerController.Instance.CheckGrounded())
        {
            if (!_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Play();
            }
        }
        else
        {
            if (_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Stop();
            }
        }
    }

    private void PlayPoofDustVFX()
    {
        _poofDustVFX.Play();
    }

    private void ApplyTilt()
    {
        float targetAngle;

        if(PlayerController.Instance.MoveInput.x < 0f)
        {
            targetAngle = _tiltAngle;
        }
        else if (PlayerController.Instance.MoveInput.x > 0f)
        {
            targetAngle= -_tiltAngle;
        }
        else
        {
            targetAngle = 0f;
        }

        // Player Sprite
        Quaternion currentCharacterRotation = _characterSpriteTransform.rotation;
       
        Quaternion targetCharacterRotation = 
            Quaternion.Euler(currentCharacterRotation.eulerAngles.x, currentCharacterRotation.eulerAngles.y, targetAngle);

        _characterSpriteTransform.rotation = 
            Quaternion.Lerp(currentCharacterRotation, targetCharacterRotation, _tiltSpeed * Time.deltaTime);


        // CowBoy Hat Sprite
        Quaternion currentCharacterCowboyHatRotation = _characterCowboyHatTransform.rotation;

        Quaternion targetCharacterCowboyHatRotation =
            Quaternion.Euler(currentCharacterCowboyHatRotation.eulerAngles.x, currentCharacterCowboyHatRotation.eulerAngles.y, -targetAngle / _cowboyHatTiltModifer);

        _characterCowboyHatTransform.rotation =
            Quaternion.Lerp(currentCharacterCowboyHatRotation, targetCharacterCowboyHatRotation, _tiltSpeed * _cowboyHatTiltModifer * Time.deltaTime);
    } 
}
