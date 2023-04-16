using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState GameState;

    [SerializeField] private InputActionReference KeyPress;

    [Header("Prefab Objects")]
    [SerializeField] private GameObject OxygenPrefab;
    [SerializeField] private GameObject MooncakePrefab;
    [SerializeField] private Meteor meteorPrefab;

    [Header("Spawn Rates")]
    [SerializeField] private float oxygenSpawnRate;
    [SerializeField] private float mooncakeSpawnRate;
    [SerializeField] private float meteorSpawnRate;
    private float oxygenSpawnTimer;
    private float mooncakeSpawnTimer;
    private float meteorSpawnTimer;
    private float gameTimer;

    [SerializeField] private int maxOxygen;
    [SerializeField] private int maxMooncake;
    private int oxygenCount;
    private int mooncakeCount;

    [Header("Oxygen")]
    [SerializeField] private float oxygenTime;
    [SerializeField] private float oxygenSupplyAmount;
    private float oxygenTimer;

    [Header("Meteor")]
    [SerializeField] private float minSpawnRadius;
    [SerializeField] private float maxSpawnRadius;
    [SerializeField] private float targetRadius;


    [Header("Spawned Object Parents")]
    [SerializeField] private Transform oxygenParent;
    [SerializeField] private Transform mooncakeParent;
    [SerializeField] private Transform meteorParent;

    private float score;

    [Header("UI")]
    [SerializeField] private TMP_Text ScoreUI;
    [SerializeField] private GameObject IntroUI;
    [SerializeField] private GameObject EndUI;
    [SerializeField] private TMP_Text DeathReasonUI;
    [SerializeField] private TMP_Text AliveForUI;
    [SerializeField] private TMP_Text EndScoreUI;
    [SerializeField] private Slider OxygenSlider;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        GameState = GameState.Paused;
        Cursor.lockState = CursorLockMode.Locked;

        IntroUI.SetActive(true);
        oxygenSpawnTimer = oxygenSpawnRate;
        mooncakeSpawnTimer = mooncakeSpawnRate;
        meteorSpawnTimer = meteorSpawnRate;
        oxygenTimer = oxygenTime;
        score = 0;
        gameTimer = 0;
        OxygenSlider.value = oxygenTimer;


        InitializeGame();
    }

    private void OnEnable()
    {
        KeyPress.action.performed += OnAnyKeyPress;
    }

    private void OnDisable()
    {
        KeyPress.action.performed -= OnAnyKeyPress;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameState != GameState.Playing) return;
        if (Player.Instance.freezeTime) return;
        gameTimer += Time.deltaTime;
        PlayerOxygen();
        SpawnOxygen();
        SpawnCheese();
        SpawnMeteor();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void EndGame(string reason)
    {
        AudioController.Instance.PlaySFX("death");
        string reasonText = "";
        if (reason == "oxygen")
        {
            reasonText = "You ran out of oxygen!";
        } else if (reason == "meteor")
        {
            reasonText = "You got pulverized by a meteor!";
        }
        DeathReasonUI.text = reasonText;
        AliveForUI.text = "Game Time: " + gameTimer.ToString("0.00") + " seconds";

        float highScore = PlayerPrefs.GetFloat("HIGHSCORE", 0);
        string scoreText = "";
        if (score > highScore)
        {
            scoreText = "New highscore of " + score;
        } else
        {
            scoreText = "Score: " + score;
        }
        EndScoreUI.text = scoreText;
        PlayerPrefs.SetFloat("HIGHSCORE", Mathf.Max(PlayerPrefs.GetFloat("HIGHSCORE", 0), score));
        GameState = GameState.End;
        Cursor.lockState = CursorLockMode.None;
        EndUI.SetActive(true);
    }

    public void ObtainedOxygen(GameObject oxygen)
    {
        AudioController.Instance.PlaySFX("oxygen");
        oxygenTimer += oxygenSupplyAmount;
        oxygenTimer = Mathf.Clamp(oxygenTimer, 0, 20f);
        OxygenSlider.value = oxygenTimer;

        Destroy(oxygen);
        oxygenCount--;
    }

    public void ObtainedCheese(GameObject cheese)
    {
        AudioController.Instance.PlaySFX("cheese");
        UpdateScore(1);
        Destroy(cheese);
        mooncakeCount--;
    }

    private void InitializeGame()
    {
        for (int i = 0; i < maxOxygen; i++)
        {
            Vector3 spawnPosition = Planet.GetRandomPointOnPlanet();
            GameObject oxygen = Instantiate(OxygenPrefab, spawnPosition, Quaternion.identity, oxygenParent);
            oxygen.transform.up = (oxygen.transform.position - Planet.Instance.transform.position).normalized;
            oxygenCount++;
        }

        for (int i = 0; i < maxMooncake; i++)
        {
            Vector3 spawnPosition = Planet.GetRandomPointOnPlanet();
            GameObject cheese = Instantiate(MooncakePrefab, spawnPosition, Quaternion.identity, mooncakeParent);
            cheese.transform.up = (cheese.transform.position - Planet.Instance.transform.position).normalized;

            mooncakeCount++;
        }
    }

    private void PlayerOxygen()
    {
        oxygenTimer -= Time.deltaTime;
        OxygenSlider.value = oxygenTimer;
        if (oxygenTimer <= 0)
        {
            Debug.Log("Lose No Oxygen");
            EndGame("oxygen");
        }
    }

    private void SpawnOxygen()
    {
        oxygenSpawnTimer -= Time.deltaTime;
        if (oxygenSpawnTimer > 0 || oxygenCount >= maxOxygen) return;
        oxygenSpawnTimer = oxygenSpawnRate;

        Vector3 spawnPosition = Planet.GetRandomPointOnPlanet();
        GameObject oxygen = Instantiate(OxygenPrefab, spawnPosition, Quaternion.identity, oxygenParent);
        oxygen.transform.up = (oxygen.transform.position - Planet.Instance.transform.position).normalized;

        oxygenCount++;
    }

    private void SpawnCheese()
    {
        mooncakeSpawnTimer -= Time.deltaTime;
        if (mooncakeSpawnTimer > 0 || mooncakeCount >= maxMooncake) return;
        mooncakeSpawnTimer = mooncakeSpawnRate;

        Vector3 spawnPosition = Planet.GetRandomPointOnPlanet();
        GameObject cheese = Instantiate(MooncakePrefab, spawnPosition, Quaternion.identity, mooncakeParent);
        cheese.transform.up = (cheese.transform.position - Planet.Instance.transform.position).normalized;

        mooncakeCount++;
    }

    private void SpawnMeteor()
    {
        meteorSpawnTimer -= Time.deltaTime;
        if (meteorSpawnTimer > 0) return;
        meteorSpawnTimer = meteorSpawnRate;

        Vector3 impactLocation = CalculateImpactLocation();

        Vector3 spawnPosition = CalculateSpawnPosition(impactLocation);
        if (spawnPosition == Vector3.zero) return;
        Meteor meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity, meteorParent);
        meteor.Initialize(impactLocation);
    }

    private Vector3 CalculateImpactLocation()
    {
        Vector3 impactLocation = Player.Instance.transform.position + Player.Instance.transform.forward * 2;
        Vector2 offset = Random.insideUnitCircle * targetRadius;
        impactLocation.x += offset.x;
        impactLocation.z += offset.y;
        return impactLocation;
    }

    private Vector3 CalculateSpawnPosition(Vector3 impactLocation)
    {
        RaycastHit hit;
        Debug.DrawLine(impactLocation, Planet.Instance.transform.position, Color.blue, 5f);
        if (Physics.Raycast(impactLocation, (Planet.Instance.transform.position - impactLocation).normalized, out hit, 10f, 1 << 3))
        {
            Vector3 spawnAngle = Random.insideUnitSphere;
            if (Vector3.Dot(hit.normal, spawnAngle) < 0)
            {
                spawnAngle *= -1;
            }
            Vector3 spawnPosition = impactLocation + spawnAngle * Random.Range(minSpawnRadius, maxSpawnRadius);
            Debug.DrawLine(impactLocation, spawnPosition, Color.red, 5f);
            return spawnPosition;
        }
        //return Vector3.zero;
        return Random.onUnitSphere * Random.Range(minSpawnRadius, maxSpawnRadius);
    }

    private void UpdateScore(int value)
    {
        score += value;
        ScoreUI.text = score.ToString();

        
        
    }

    private void OnAnyKeyPress(InputAction.CallbackContext obj)
    {
        if (GameState == GameState.Paused)
        {
            GameState = GameState.Playing;
            IntroUI.SetActive(false);
        }
    }
}

public enum GameState { Paused, Playing, End }