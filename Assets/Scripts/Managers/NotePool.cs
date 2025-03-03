using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NotePool : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform spawnPoint;
    public Transform endPoint;
    public int poolSize = 20;

    private Queue<GameObject> notePool;
    private Queue<GameObject> activeNotes;

    private void Awake()
    {
        InitializePool();
    }

    private void OnEnable()
    {
        GameEvents.OnNoteSpawn += SpawnNote;
    }

    private void OnDisable()
    {
        GameEvents.OnNoteSpawn -= SpawnNote;
    }

    private void InitializePool()
    {
        notePool = new Queue<GameObject>();
        activeNotes = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject note = Instantiate(notePrefab, transform);
            note.SetActive(false);
            notePool.Enqueue(note);
        }
    }

    private void SpawnNote(float spawnTime, float travelDuration)
    {
        if (notePool.Count == 0) return;

        GameObject note = notePool.Dequeue();

        note.SetActive(true);

        Vector2 endPos = new Vector2(endPoint.position.x, Random.Range(-2, 2));
        note.GetComponent<GameNote>().Initialize(spawnPoint.position, endPos, spawnTime, travelDuration);

        activeNotes.Enqueue(note);
    }

    public void ReturnToPool(GameObject note)
    {
        note.SetActive(false);
        notePool.Enqueue(note);
        if (activeNotes.Peek() == note)
        {
            activeNotes.Dequeue();
        }
    }

    
    public void CheckClosestActiveNote()
    {
        
        if (activeNotes.Count == 0) return;
        GameObject activeNote = activeNotes.Peek();
        GameNote active = activeNote.GetComponent<GameNote>();
        if (active.GetHitResult() == GameNote.HitResult.MISS)
        {
            activeNotes.Dequeue();
            CheckClosestActiveNote();
        }
        active.CheckHit();
    }
}
