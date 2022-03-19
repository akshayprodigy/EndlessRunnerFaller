using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rigidbody;
    private float dirX;

    public delegate void PlayerScore();
    public static event PlayerScore onPlayerScore;

    public delegate void PlayerOutOfBound();
    public static event PlayerOutOfBound onPlayerOutOfBound;

    public delegate void PauseGame();
    public static event PauseGame onPauseGame;

    Vector3 initialPosition;

    private void OnEnable()
    {
        PlatformManager.onPlatforReadyAfterReset += ResetPlayer;
    }

    private void OnDisable()
    {
        PlatformManager.onPlatforReadyAfterReset -= ResetPlayer;
    }
    void Start()
    {
        initialPosition = transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }


    public void ResetPlayer()
    {
        transform.position = initialPosition;
        //gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxis("Horizontal") * speed;

        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Escape key is being pressed");
            if (onPauseGame != null)
                onPauseGame();
        }
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(dirX, rigidbody.velocity.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Utility.TagPlatform))
        {
          // Debug.Log("Platform Triggred");
            if (onPlayerScore != null)
                onPlayerScore();
        }
        else if (other.CompareTag(Utility.TagOutOfBound))
        {
            if (onPlayerOutOfBound != null)
                onPlayerOutOfBound();

            gameObject.SetActive(false);
        }
    }

}
