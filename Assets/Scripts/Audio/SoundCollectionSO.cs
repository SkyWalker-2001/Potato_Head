using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundCollectionSO : ScriptableObject
{
    [Header("Music")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoParty;

    [Header("SFX")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
    public SoundSO[] JetPack;
    public SoundSO[] GrenadeShoot;
    public SoundSO[] GrenadeExplode;
    public SoundSO[] GrenadeBeep;
    public SoundSO[] PlayerHit;
}
