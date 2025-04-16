using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;

public class Flechas : MonoBehaviour
{
    public GameObject Izqdobstaculo;
    public GameObject Dchaobstaculo;
    public GameObject Arribaobstaculo;
    public GameObject Abajoobstaculo;

    public List<GameObject> objectPrefabs;

    public float spawnInterval = 2f;
    public float spawnDistance = 10f;
    public float cubeSpeed = 5f;
    public int score = 0;
    public TMP_Text scoreText;
    public float destinationOffsetRange = 1f;
    public float bombProbability = 0.2f;

    public GameObject sableDoble;
    // Almacena la altura inicial de la cabeza (posición de la cámara)
    private float fixedHeadHeight;

    void Awake()
    {
        objectPrefabs.Add(Izqdobstaculo);
        objectPrefabs.Add(Dchaobstaculo);
        objectPrefabs.Add(Arribaobstaculo);
        objectPrefabs.Add(Abajoobstaculo);
        Debug.Log(PlayerPrefs.GetInt("puntObj"));
        Debug.Log("sableDoble" + PlayerPrefs.GetInt("sableDoble"));
         if (PlayerPrefs.GetInt("sableDoble")==0)
         {
            Debug.Log("sableDoble" + PlayerPrefs.GetInt("sableDoble"));
            sableDoble.SetActive(false);
         }
        // Al inicio se guarda la altura actual de la cámara
        fixedHeadHeight = Camera.main.transform.position.y;

        int cubeLayer = LayerMask.NameToLayer("Cube");
        int stickLayer = LayerMask.NameToLayer("Stick");

        if (cubeLayer < 0 || stickLayer < 0)
        {
            Debug.LogError("La capa 'Cube' o 'Stick' no está definida. Asegúrate de crearlas en Project Settings > Tags and Layers.");
            return;
        }

        // Configura las colisiones: los cubos (capa "Cube") solo colisionan con objetos de la capa "Stick"
        for (int i = 0; i < 32; i++)
        {
            if (i != stickLayer)
            {
                Physics.IgnoreLayerCollision(cubeLayer, i, true);
            }
        }

        //StartCoroutine(SpawnCubes());
    }

    void OnEnable()
    {
         StartCoroutine(SpawnCubes());
    }

    IEnumerator SpawnCubes()
    {
        while (true)
        {
            SpawnCube();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCube()
    {
        // Calcula la posición de spawn: partimos de la posición de la cámara desplazada hacia adelante,
        // pero establecemos la altura fija para que siempre sea la altura inicial de la cabeza.
        Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
        spawnPos.y = fixedHeadHeight;

        int randomIndex = Random.Range(0, objectPrefabs.Count);

        // Obtener el prefab de la lista usando el índice aleatorio
        GameObject prefabToSpawn = objectPrefabs[randomIndex];

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject obstaculo = GameObject.Instantiate(prefabToSpawn);
        obstaculo.transform.position = spawnPos;
        
        // Reducir el tamaño del cubo al 25% del original (reducción del 75%)
        obstaculo.transform.localScale *= 0.1f;
        
        

        if (Random.value < bombProbability)
        {
            //cube.GetComponent<Renderer>().material.color = Color.red;
            //cube.gameObject.name = "bomba";
        }
        
        Rigidbody rb = obstaculo.AddComponent<Rigidbody>();
        rb.useGravity = false;
        float offset = Random.Range(-destinationOffsetRange, destinationOffsetRange);
        //Debug.Log(new Vector3(Camera.main.transform.position.x + offset,Camera.main.transform.position.y,Camera.main.transform.position.z));
        Vector3 direction = (new Vector3(Camera.main.transform.position.x + offset,Camera.main.transform.position.y,Camera.main.transform.position.z) - spawnPos).normalized;
        rb.velocity = direction * cubeSpeed;
        
        obstaculo.layer = LayerMask.NameToLayer("Cube");
        CollisionFlechas collisionScript = obstaculo.AddComponent<CollisionFlechas>();
        collisionScript.gameManager = this;
    }

    public void AddScore(int amount)
    {
        if (score>=PlayerPrefs.GetInt("puntObj"))
        {
            scoreText.text = "FIN";
        }
        else
        {
            score += amount;
            Debug.Log("Score: " + score);
            scoreText.text = score.ToString();
        }
        
        
    }
}

public class CollisionFlechas : MonoBehaviour
{
    public Flechas gameManager;

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Stick"))
        {

            if (this.gameObject.name.Equals("bomba"))
            {
                gameManager.AddScore(-1);
            }
            else
            {
                gameManager.AddScore(1);
            }
            Destroy(gameObject);
        }
    }
}
