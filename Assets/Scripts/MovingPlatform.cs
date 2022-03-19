using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    public delegate void PlatformInvisible(MovingPlatform unit);
    public static event PlatformInvisible onPlatformInvisible;

    private void OnEnable()
    {
        UIManager.onResetLevel += ResetLevel;
    }


    private void OnDisable()
    {
        UIManager.onResetLevel -= ResetLevel;
    }
    bool isPlatformActive = false;
    public void setPlatformSpeed(float val)
    {
        speed = val;
    }

   
    // Update is called once per frame
    void Update()
    {
        if(isPlatformActive)
            transform.position += Vector3.up * speed * Time.deltaTime;

       
    }

    public void setPlatformInactive()
    {
        isPlatformActive = false;
        this.gameObject.SetActive(false);
    }

    public void activatePlatform(float xPos, float yPos, float currentSpeed, float distToTravel)
    {
        isPlatformActive = true;
        this.gameObject.SetActive(true);
        this.transform.position = new Vector3(xPos, yPos, 0.001f);
        float time = distToTravel / currentSpeed;
        StartCoroutine(DiaablePlatform(time));
        setPlatformSpeed(currentSpeed);
    }

    void ResetLevel()
    {
        if (onPlatformInvisible != null)
        {
            //Debug.Log("onPlatformInvisible: ");
            onPlatformInvisible(this);
        }
    }

    IEnumerator DiaablePlatform(float time)
    {
        yield return new WaitForSeconds(time);
        if (onPlatformInvisible != null)
        {
            //Debug.Log("onPlatformInvisible: ");
            onPlatformInvisible(this);
        }

    }

   
}