using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : Singleton<PlatformManager>
{
    // Start is called before the first frame update
    [SerializeField] private float minspeed;
    [SerializeField] private float platformStartPoition;
    [SerializeField] private float maxspeed;
    [SerializeField] private float leftExtream;
    [SerializeField] private float rightExtream;
    [SerializeField] private float distanceToTravel;
    [SerializeField] private int initialPlatformCount;
    private int totalPlatformCount;
    private bool keepInstanciatingPlatform = false;
    private WaitForSeconds waitingPeroid;
    private float currentPlatformSpped;
    private float spawnDistance = 0;
    float lastPlatformPosition = -25;
    
    [SerializeField] private MovingPlatform platformPrefab;

    [SerializeField]
    private Queue<MovingPlatform> inactivePlatform;

    public delegate void PlatforReadyAfterReset();
    public static event PlatforReadyAfterReset onPlatforReadyAfterReset;

    private void OnEnable()
    {
        MovingPlatform.onPlatformInvisible += ReturnPlatform;
        LevelManager.onIncrementSpeed += IncreasePlatformSpeed;
        UIManager.onResetLevel += ResetLevel;
    }

    private void OnDisable()
    {
        MovingPlatform.onPlatformInvisible -= ReturnPlatform;
        LevelManager.onIncrementSpeed -= IncreasePlatformSpeed;
        UIManager.onResetLevel -= ResetLevel;
    }

    void Start()
    {
        totalPlatformCount = 0;
        CachePlatforms();
        getSpawnDiatance();
        Initialized();


    }

    void ResetLevel()
    {
        StartCoroutine(ResetPlatformController());
    }

    private void Initialized()
    {
        keepInstanciatingPlatform = true;
        setNewSpeed(minspeed);
        StartCoroutine(PlatformController());
    }

    void IncreasePlatformSpeed()
    {
        float speed = currentPlatformSpped + 1;
        if (speed >= maxspeed)
            speed = maxspeed;

        setNewSpeed(speed);
    }

    void setNewSpeed(float speed)
    {
        currentPlatformSpped = speed;
        getNewSpawnTime();
    }

    void getNewSpawnTime()
    {
        float time = spawnDistance / currentPlatformSpped;
        //Debug.Log("getNewSpawnTime: "+time);
        waitingPeroid = new WaitForSeconds(time);
    }

    void getSpawnDiatance()
    {
        var screenBottomCenter = new Vector3(Screen.width / 2, Screen.height, 10);
        var inBottomWorld = Camera.main.ScreenToWorldPoint(screenBottomCenter);

        var screenoneThirdCenter = new Vector3(Screen.width / 2, Screen.height/2, 10);
        var inOnethirdWorld = Camera.main.ScreenToWorldPoint(screenoneThirdCenter);

        float dis = Vector3.Distance(inOnethirdWorld, inBottomWorld);
        //Debug.Log("dis: "+dis+ " inBottomWorld:  "+ inBottomWorld + "  screenBottomCenter: "+ screenBottomCenter + "  inOnethirdWorld:  "+ inOnethirdWorld + "  screenoneThirdCenter: "+ screenoneThirdCenter);
        spawnDistance = dis;
        //return dis;
    }

    public void moveNextPlatform()
    {
        MovingPlatform movingPlatform = null;
        if (inactivePlatform.Count > 0)
        {
            movingPlatform = inactivePlatform.Dequeue();
        }
        else
        {
            movingPlatform = InstansiatePlatform();
        }
        float platformXpos = Random.Range(leftExtream, rightExtream);
        while (platformXpos == lastPlatformPosition)
        {
            platformXpos = Random.Range(leftExtream, rightExtream);
        }
       
        movingPlatform.activatePlatform(platformXpos, platformStartPoition, currentPlatformSpped, distanceToTravel);
        lastPlatformPosition = platformXpos;
    }

    void InstanciateAndAddPlatforms()
    {
        MovingPlatform movingPlatform = InstansiatePlatform();
        inactivePlatform.Enqueue(movingPlatform);
    }

    MovingPlatform InstansiatePlatform()
    {
        MovingPlatform movingPlatform = Instantiate(platformPrefab) as MovingPlatform;
        movingPlatform.setPlatformInactive();
        totalPlatformCount++;
        return movingPlatform;
    }

    void CachePlatforms()
    {
        inactivePlatform = new Queue<MovingPlatform>();
        for (int i =0;i< initialPlatformCount; i++)
        {
            InstanciateAndAddPlatforms();
        }
    }

    void ReturnPlatform(MovingPlatform platform)
    {
        inactivePlatform.Enqueue(platform);
        platform.setPlatformInactive();
    }


    IEnumerator ResetPlatformController()
    {
        yield return new WaitUntil(() => inactivePlatform.Count == totalPlatformCount);
        setNewSpeed(minspeed);
        if (onPlatforReadyAfterReset != null)
            onPlatforReadyAfterReset();
    }
    IEnumerator PlatformController()
    {
        while (keepInstanciatingPlatform)
        {
            moveNextPlatform();
            yield return waitingPeroid;
        }
    }
}
