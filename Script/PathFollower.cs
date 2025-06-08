using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator))]
public class PathFollower : MonoBehaviour
{
    [Header("Pengaturan Dasar")]
    private Tilemap pathTilemap;
    public float moveSpeed = 3f;
    private Tilemap blockingTilemap;

    [Header("Status Debug")]
    public bool canMove = true; // Saklar untuk mengunci/membuka gerakan

    // Komponen & Variabel Internal
    private Animator animator;
    private Queue<Vector3> moveQueue = new Queue<Vector3>();
    private bool isMoving = false;
    private Vector3 currentTargetPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Cari GameObject dengan nama "Ground" di Hierarchy
        GameObject groundObject = GameObject.Find("Path");
        if (groundObject != null)
        {
            // Jika ketemu, ambil komponen Tilemap dari objek tersebut
            pathTilemap = groundObject.GetComponent<Tilemap>();
        }

        // Lakukan hal yang sama untuk tilemap puzzle Anda
        // Ganti "puzzle" dengan nama Tilemap puzzle Anda di Hierarchy jika berbeda
        GameObject blockingObject = GameObject.Find("Puzzle");
        if (blockingObject != null)
        {
            blockingTilemap = blockingObject.GetComponent<Tilemap>();
        }

        // Pengecekan terakhir untuk memastikan semuanya berhasil ditemukan
        if (pathTilemap == null || blockingTilemap == null)
        {
            Debug.LogError("PathFollower pada Player tidak bisa menemukan Tilemap 'Ground' atau 'puzzle'! Pastikan nama GameObject di Hierarchy sudah benar.");
        }
    }

    void Update()
    {
        // Jika player tidak bisa bergerak, hentikan semua proses di Update
        if (!canMove)
        {
            return;
        }

        // Proses input dan pergerakan
        HandleMouseInput();
        ProcessMovement();
    }

    // --- FUNGSI BARU UNTUK MENGECEK STATUS GERAK ---
    public bool IsCurrentlyMoving()
    {
        // Player dianggap bergerak jika boolean isMoving true ATAU antrian geraknya masih ada isinya
        return isMoving || moveQueue.Count > 0;
    }

    // --- FUNGSI KONTROL GERAKAN (untuk dipanggil script lain nanti) ---
    public void LockMovement()
    {
        moveQueue.Clear();
        isMoving = false;
        canMove = false;
        animator.SetFloat("speed", 0f); // Langsung set animasi ke Idle
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    // --- FUNGSI INTERNAL ---
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = pathTilemap.WorldToCell(mouseWorldPos);

            if (pathTilemap.HasTile(gridPos))
            {
                FindAndSetPath(pathTilemap.GetCellCenterWorld(gridPos));
            }
        }
    }

    private void ProcessMovement()
    {
        // Jika tidak sedang bergerak tapi ada antrian, mulai gerakan baru
        if (!isMoving && moveQueue.Count > 0)
        {
            currentTargetPos = moveQueue.Dequeue();
            isMoving = true;

            // --> INI BAGIAN BARU UNTUK ANIMASI <---
            // Beri tahu Animator bahwa kita mulai bergerak (speed > 0)
            animator.SetFloat("speed", 1f);

            // Hitung arah ke tile berikutnya untuk menentukan animasi jalan
            Vector3 direction = (currentTargetPos - transform.position).normalized;
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }

        // Jika sedang bergerak, lanjutkan gerakan ke target
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTargetPos, moveSpeed * Time.deltaTime);

        // Cek jika sudah sampai di target tile
        if (Vector3.Distance(transform.position, currentTargetPos) < 0.01f)
        {
            transform.position = currentTargetPos;
            isMoving = false;

            // Jika antrian sudah kosong, berarti kita sudah sampai di tujuan akhir
            if (moveQueue.Count == 0)
            {
                // --> INI BAGIAN BARU UNTUK ANIMASI <---
                // Beri tahu Animator bahwa kita sudah berhenti total (speed = 0)
                animator.SetFloat("speed", 0f);
            }
        }
    }

    // --- FUNGSI PATHFINDING (Tidak ada perubahan signifikan) ---
    private void FindAndSetPath(Vector3 destination)
    {
        moveQueue.Clear();
        isMoving = false;

        Vector3Int startGrid = pathTilemap.WorldToCell(transform.position);
        Vector3Int destinationGrid = pathTilemap.WorldToCell(destination);

        List<Vector3Int> path = FindPathBFS(startGrid, destinationGrid);

        if (path != null)
        {
            foreach (Vector3Int cell in path)
            {
                moveQueue.Enqueue(pathTilemap.GetCellCenterWorld(cell));
            }
        }
        else
        {
            Debug.LogWarning("Path tidak ditemukan!");
        }
    }

    private List<Vector3Int> FindPathBFS(Vector3Int start, Vector3Int goal)
    {
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();
        frontier.Enqueue(start);
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        cameFrom[start] = start;

        Vector3Int[] directions = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };

        bool pathFound = false;
        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();
            if (current == goal)
            {
                pathFound = true;
                break;
            }
            foreach (Vector3Int dir in directions)
            {
                Vector3Int next = current + dir;
                if (pathTilemap.HasTile(next) && !blockingTilemap.HasTile(next) && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        if (!pathFound) return null;

        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int currentCell = goal;
        while (currentCell != start)
        {
            path.Add(currentCell);
            currentCell = cameFrom[currentCell];
        }
        path.Reverse();
        return path;
    }
}