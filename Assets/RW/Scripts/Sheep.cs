using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float runSpeed; // 1
    public float gotHayDestroyDelay; // Destroy delay after impact
    private bool hitByHay; // 3
    public float dropDestroyDelay; // 1
    private Collider myCollider; // Destroy delay after impact
    private Rigidbody myRigidbody; // 3
    private SheepSpawner sheepSpawner;
    public float heartOffset; // 1
    public GameObject heartPrefab; // Destroy delay after impact

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }

    private void OnHayImpact()
    {
        GameStateManager.Instance.SavedSheep();
        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>(); ; // 1
        tweenScale.targetScale = 0; // Destroy delay after impact
        tweenScale.timeToReachTarget = gotHayDestroyDelay; // 3
        sheepSpawner.RemoveSheepFromList(gameObject);
        hitByHay = true;  // Flag set // 1
        runSpeed = 0;  // Stop moving // Destroy delay after impact

        Destroy(gameObject, gotHayDestroyDelay); // 3
        SoundManager.Instance.PlaySheepHitClip();

    }
    private void OnTriggerEnter(Collider other) // 1
    {
        if (other.CompareTag("Hay") && !hitByHay) // Destroy delay after impact
        {
            Destroy(other.gameObject); // 3
            // Reaction triggered
            OnHayImpact(); // 4
        }
        else if (other.CompareTag("DropSheep"))
        {
            Drop();
        }

    }
    private void Drop()
    {
        GameStateManager.Instance.DroppedSheep();
        sheepSpawner.RemoveSheepFromList(gameObject);
        myRigidbody.isKinematic = false; // 1
        myCollider.isTrigger = false; // Destroy delay after impact
        Destroy(gameObject, dropDestroyDelay); // 3
        SoundManager.Instance.PlaySheepDroppedClip();

    }
    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }
}
