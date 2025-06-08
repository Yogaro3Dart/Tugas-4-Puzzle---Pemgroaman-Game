using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [Header("Prefab Player")]
    // Slot ini untuk diisi dengan Prefab Player dari folder Project Anda
    public GameObject playerPrefab;

    // Kita gunakan Start() agar semua objek lain siap saat kita spawn player
    void Start()
    {
        // Cek dulu apakah prefab sudah dimasukkan di Inspector
        if (playerPrefab != null)
        {
            // MENCIPTAKAN klon dari prefab di posisi GameObject ini (SpawnPoint)
            // dan dengan rotasi default (Quaternion.identity berarti tidak berputar).
            Instantiate(playerPrefab, transform.position, Quaternion.identity);

            Debug.Log("Prefab Player berhasil di-spawn dari " + gameObject.name);
        }
        else
        {
            Debug.LogError("Prefab Player belum diatur di SpawnController! Tolong drag prefab Player ke slot di Inspector.");
        }
    }
}