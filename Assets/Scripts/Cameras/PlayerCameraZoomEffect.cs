using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCameraZoomEffect : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float baseOrthoSize = 14.8f;

    [Header("Animation curves")]
    [Header("Should start and end at 0")]
    [SerializeField] public AnimationCurve deathZoomCurve;
    [SerializeField] public AnimationCurve portalZoomCurve;
    [SerializeField] public AnimationCurve speedArrowZoomCurve;
    [SerializeField] public AnimationCurve winLevelZoomCurve;


    public static PlayerCameraZoomEffect Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    private Coroutine zoomCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera.m_Lens.OrthographicSize = baseOrthoSize;
    }


    public void PlayZoomEffect(AnimationCurve zoomCurve, float zoomScale = 1, float duration = 0.5f) {
        if (zoomCoroutine != null) {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomCoroutine(zoomCurve, zoomScale, duration));
    }

    private IEnumerator ZoomCoroutine(AnimationCurve zoomCurve, float zoomScale = 1, float duration = 0.5f) {
        float time = 0;
        while (time < duration) {
            time += Time.deltaTime;
            float timeNormalized = time / duration;
            float val = zoomCurve.Evaluate(timeNormalized);
            float zoom = baseOrthoSize - val * zoomScale;
            virtualCamera.m_Lens.OrthographicSize = zoom;
            yield return null;
        }
        virtualCamera.m_Lens.OrthographicSize = baseOrthoSize;

        zoomCoroutine = null;
    }
}
