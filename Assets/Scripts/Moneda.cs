using UnityEngine;

public class Moneda : MonoBehaviour
{
    public MonedaManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.ReemplazarMoneda(gameObject);
            Destroy(gameObject);
        }
    }
}
