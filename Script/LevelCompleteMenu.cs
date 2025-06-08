using UnityEngine;
using UnityEngine.SceneManagement; // <-- PENTING untuk pindah scene

public class LevelCompleteMenu : MonoBehaviour
{
    // Fungsi ini akan dipanggil oleh tombol "Lanjut"
    public void LanjutButton()
    {
        // Pindah ke scene berikutnya di dalam urutan Build Settings
        // Contoh: dari level 1 ke level 2
        Debug.Log("Lanjut ke level berikutnya...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Fungsi ini akan dipanggil oleh tombol "Kembali"
    public void KembaliButton()
    {
        // Pindah ke scene Menu Utama (pastikan namanya "HomeScene" atau sesuaikan)
        // Atau bisa juga ke level 0: SceneManager.LoadScene(0);
        Debug.Log("Kembali ke Menu Utama...");
        SceneManager.LoadScene("HomeScene");
    }
}