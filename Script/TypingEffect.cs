using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypingEffect : MonoBehaviour
{
    [Header("Pengaturan Teks")]
    [TextArea(5, 10)]
    public string fullText;
    public float typingSpeed = 0.05f;

    [Header("Pengaturan Pindah Scene")]
    // Kita tidak lagi menggunakan variabel publik untuk scene tujuan
    [Tooltip("Jeda waktu (detik) setelah teks penuh sebelum pindah scene")]
    public float delayAfterComplete = 3f;

    [Header("Audio (Opsional)")]
    public AudioClip typingSound;

    private TextMeshProUGUI textComponent;
    private AudioSource audioSource;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        audioSource = gameObject.AddComponent<AudioSource>();
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        textComponent.text = "";

        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            if (typingSound != null)
            {
                audioSource.PlayOneShot(typingSound);
            }
            yield return new WaitForSeconds(typingSpeed);
        }

        StartCoroutine(LoadNextSceneAfterDelay());
    }

    // Fungsi Update() sudah kita hapus agar tidak bisa di-skip

    // --- FUNGSI INI YANG KITA UBAH ---
    private IEnumerator LoadNextSceneAfterDelay()
    {
        // Definisikan nama scene tujuan langsung di dalam kode
        string namaSceneSelanjutnya = "Level 1"; // <-- PASTIKAN NAMA INI SAMA PERSIS

        Debug.Log("Teks selesai ditampilkan. Menunggu " + delayAfterComplete + " detik...");
        yield return new WaitForSeconds(delayAfterComplete);

        Debug.Log("Memuat scene secara langsung: " + namaSceneSelanjutnya);
        SceneManager.LoadScene(namaSceneSelanjutnya);
    }
}