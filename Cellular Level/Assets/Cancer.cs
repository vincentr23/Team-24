using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancer : MonoBehaviour
{
    [SerializeField] float rad;
    [SerializeField] float PopRad;
    [SerializeField] float MaxSize;
    [SerializeField] float SizeMult;
    [SerializeField] float PlayerInRange;
    [SerializeField] float RespawnTimer;
    [SerializeField] float MaxRespawn;
    [SerializeField] LayerMask mask;
    private readonly Collider[] things = new Collider[3];
    private readonly Collider[] popThings = new Collider[3];
    private GameManager manager;
    private float size;
    [SerializeField] int numThings;
    [SerializeField] AudioSource sound;

    private void Start()
    {
        PlayerInRange = 1f;
        RespawnTimer = 0;
        transform.localScale = new Vector3(1f, 1f, 1f);
        manager = GameObject.FindObjectOfType<GameManager>();

    }
    // Update is called once per frame
    void Update()
    {
        PlayerWithinRange();
    }

    void PlayerWithinRange()
    {
        numThings = Physics.OverlapSphereNonAlloc(this.transform.position, rad, things, (int)mask);
        if (numThings > 0)
        {
            var player = things[0].GetComponent<PlayerController>();
            {
                float radDiff = rad - PopRad;
                float normalizedRad = rad -
                    Vector3.Distance(transform.position, player.transform.position);
                size = (normalizedRad / radDiff) * MaxSize;
                if (size < 1)
                {
                    size = 1;
                }
                transform.localScale = new Vector3(size, size, size);
            }
        }
        if (Physics.OverlapSphereNonAlloc(this.transform.position, PopRad, popThings, (int)mask) > 0)
        {
            foreach (var thing in popThings)
            {
                var player = thing.GetComponent<PlayerController>();
                if (player.CompareTag("Player"))
                {
                    player.GetComponent<PlayerController>().Slow();
                    sound.Play();
                    gameObject.SetActive(false);
                    RespawnTimer = MaxRespawn;
                }
            }
            //var player = things[0].GetComponent<PlayerController>();
            //if (player.CompareTag("Player"))
            //{
            //    player.Slow();
            //    gameObject.SetActive(false);
            //    RespawnTimer = MaxRespawn;
            //}
        }
    }

    public void Respawn()
    {
        if (RespawnTimer > MaxRespawn)
        {
            GetComponent<GameObject>().SetActive(true);
            RespawnTimer -= MaxRespawn;
            PlayerInRange = 1f;
        }
        RespawnTimer++;
    }
}
