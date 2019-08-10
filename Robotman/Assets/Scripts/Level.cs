
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public static Level instance;

    public AudioClip levelClear;
    public AudioClip playerDefeat;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void LevelFinished()
    {
        DestroyEnemies();
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = levelClear;
        audioSource.Play();
        Invoke("RestartLevel", 5f);
    }

    public void LevelFailed()
    {
        DestroyEnemies();
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = playerDefeat;
        audioSource.Play();
        Invoke("RestartLevel", 3f);
    }
    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void DestroyEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
    }
   
}
