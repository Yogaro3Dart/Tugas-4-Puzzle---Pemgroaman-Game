using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    // Fungsi untuk tombol Play
    public void TombolPlay()
    {
        // Langsung tuju ke scene Introduce
        // Kita tulis langsung namanya karena ini sudah terbukti berhasil
        SceneManager.LoadScene("Introduce");
    }

    // Fungsi untuk tombol Keluar
    public void TombolKeluar()
    {
        Debug.Log("Tombol Keluar ditekan!");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // Anda bisa menambahkan fungsi untuk tombol lain di sini nanti
    // public void TombolOpsi() { ... }
}