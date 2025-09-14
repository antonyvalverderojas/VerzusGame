using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class AutoTileToScale : MonoBehaviour
{
    [Tooltip("Cuántos repetidos por metro (ej: 0.2 = 1 repetición cada 5 m)")]
    public float tilesPerMeter = 0.2f;

    Renderer rend;

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        UpdateTiling();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) UpdateTiling();
#endif
    }

    void UpdateTiling()
    {
        if (!rend || !rend.sharedMaterial) return;

        // Un Plane de Unity mide 10x10 unidades a escala 1.
        float worldWidth = 10f * transform.localScale.x;
        float worldLength = 10f * transform.localScale.z;

        float tileX = Mathf.Max(1f, worldWidth * tilesPerMeter);
        float tileY = Mathf.Max(1f, worldLength * tilesPerMeter);

        // sharedMaterial para no crear instancias en editor
        rend.sharedMaterial.mainTextureScale = new Vector2(tileX, tileY);
    }
}
