using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PoseHologram : MonoBehaviour
{
    private Vector3 orgScale;
    private Light light;
    private float orgLightIntensity;

    // Start is called before the first frame update
    void Start()
    {
        orgScale = transform.localScale;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0);
        light = GetComponentInChildren<Light>();
        orgLightIntensity = light.intensity;
        light.intensity = 0;

        gameObject.SetActive(false);
    }

    public void startAnimation()
    {
        gameObject.SetActive(true);
        transform.DOScaleZ(orgScale.z, 0.5f).SetEase(Ease.OutQuart);
        light.DOIntensity(orgLightIntensity, 0.5f).SetEase(Ease.OutQuart);
    }

    public async Task closeAnimation()
    {
        transform.DOScaleZ(0, 0.5f).SetEase(Ease.InQuart);
        await light.DOIntensity(0, 0.5f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
    }
}
