using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public GameObject obstaclePrefab;
    public LayerMask groundMask;

    [Header("Spawning")]
    public float spacing = 20f;       // distancia entre obstáculos
    public float variance = 4f;       // variación aleatoria
    public float startDistance = 25f; // primer obstáculo desde el jugador
    public float aheadDistance = 100f;// cuánto por delante mantener poblado

    [Header("Cleanup")]
    public float despawnBehind = 30f; // elimina los muy atrás

    float nextZ;
    readonly List<GameObject> spawned = new List<GameObject>();

    void Start()
    {
        if (!player)
        {
            var p = GameObject.Find("Player");
            if (p) player = p.transform;
        }
        nextZ = (player ? player.position.z : 0f) + startDistance;
    }

    void Update()
    {
        if (!player || !obstaclePrefab) return;

        while (nextZ < player.position.z + aheadDistance)
        {
            float z = nextZ + Random.Range(-variance, variance);

            // colocar sobre el suelo
            float y = 0.5f; // fallback si no damos al suelo
            if (Physics.Raycast(new Vector3(0f, 10f, z), Vector3.down, out RaycastHit hit, 50f, groundMask))
                y = hit.point.y + (obstaclePrefab.transform.localScale.y * 0.5f);

            Vector3 pos = new Vector3(0f, y, z);
            spawned.Add(Instantiate(obstaclePrefab, pos, Quaternion.identity));
            nextZ += spacing;
        }

        // limpiar los que quedaron atrás
        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            var o = spawned[i];
            if (!o) { spawned.RemoveAt(i); continue; }
            if (o.transform.position.z < player.position.z - despawnBehind)
            {
                Destroy(o);
                spawned.RemoveAt(i);
            }
        }
    }
}
