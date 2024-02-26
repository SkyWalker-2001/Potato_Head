using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSplatterHandler : MonoBehaviour
{
    private void OnEnable()
    {
        Health.OnDeath += SpawnDeathSplatterPrefab;
        Health.OnDeath += SpawnDeathVFX;
    }

    private void OnDisable()
    {
        Health.OnDeath -= SpawnDeathSplatterPrefab;
        Health.OnDeath -= SpawnDeathVFX;
    }

    private void SpawnDeathSplatterPrefab(Health sender)
    {
        Debug.Log("WORKING");
        GameObject newSplatterPrefab = Instantiate(sender.SplatterPrefab, sender.transform.position, transform.rotation);

        SpriteRenderer deathSplatterSpriteRenderere = newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();

        if (colorChanger)
        {
            Color currentColor = colorChanger.DefaultColor;

            deathSplatterSpriteRenderere.color = currentColor;
        }


        newSplatterPrefab.transform.SetParent(this.transform);
    }

    private void SpawnDeathVFX(Health sender)
    {
        GameObject deathVFX = Instantiate(sender.DeathVFX, sender.transform.position, transform.rotation);
        ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;

        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();


        if (colorChanger)
        {
            Color currentColor = colorChanger.DefaultColor;

            ps.startColor = currentColor;
        }

        
        deathVFX.transform.SetParent(this.transform);

    }
}
