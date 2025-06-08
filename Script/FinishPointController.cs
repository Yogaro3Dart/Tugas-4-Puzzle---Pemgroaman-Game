using UnityEngine;

public class FinishPointController : MonoBehaviour
{
    // Slot untuk menghubungkan Panel UI kita
    public GameObject levelCompletePanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek jika yang masuk adalah Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player mencapai titik finish!");

            // Kunci gerakan player agar tidak bisa jalan lagi
            PathFollower playerMovement = other.GetComponent<PathFollower>();
            if (playerMovement != null)
            {
                playerMovement.LockMovement();
            }

            // Tampilkan panel UI "Level Selesai"
            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true);
            }

            // Nonaktifkan trigger ini agar tidak terpicu berulang kali
            gameObject.SetActive(false);
        }
    }
}