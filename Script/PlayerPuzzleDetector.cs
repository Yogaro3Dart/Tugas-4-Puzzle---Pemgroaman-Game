using UnityEngine;

public class PlayerPuzzleDetector : MonoBehaviour
{
    private TileMatchingController puzzleController;
    private PathFollower pathFollower;

    // --- PERUBAHAN 1: Tambahkan "memori" baru ---
    // Penanda bahwa sesi puzzle sedang berjalan
    private bool isPuzzleSessionActive = false;
    // Penanda bahwa puzzle ini sudah PERNAH selesai
    private bool isPuzzleCompleted = false;

    void Start()
    {
        pathFollower = GetComponent<PathFollower>();
        puzzleController = FindAnyObjectByType<TileMatchingController>();
        if (puzzleController == null)
        {
            Debug.LogError("PlayerPuzzleDetector tidak bisa menemukan objek dengan script TileMatchingController di scene!");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Cek apakah yang disentuh adalah pemicu
        if (other.CompareTag("PuzzleTrigger"))
        {
            // --- PERUBAHAN 2: Tambahkan "!isPuzzleCompleted" di kondisi ---
            // Trigger hanya aktif jika player berhenti, sesi puzzle tidak aktif, DAN puzzle belum pernah selesai.
            if (!pathFollower.IsCurrentlyMoving() && !isPuzzleSessionActive && !isPuzzleCompleted)
            {
                Debug.Log("Player berhenti di tile pemicu! Mengaktifkan puzzle...");

                pathFollower.LockMovement();
                if (puzzleController != null)
                {
                    puzzleController.ActivatePuzzle();
                    isPuzzleSessionActive = true;
                }
                TileMatchingController.OnPuzzleCompleted += OnPuzzleFinished;
            }
        }
    }

    private void OnPuzzleFinished()
    {
        Debug.Log("DETEKTOR: Puzzle selesai! Membuka kunci player.");
        if (pathFollower != null)
        {
            pathFollower.UnlockMovement();
        }

        isPuzzleSessionActive = false;

        // --- PERUBAHAN 3: Set "memori" bahwa puzzle ini sudah tamat ---
        isPuzzleCompleted = true;

        TileMatchingController.OnPuzzleCompleted -= OnPuzzleFinished;
    }

    private void OnDestroy()
    {
        // Pastikan kita selalu berhenti berlangganan event
        if (puzzleController != null)
        {
            TileMatchingController.OnPuzzleCompleted -= OnPuzzleFinished;
        }
    }
}