using UnityEngine;
using System.Collections;

public class BlinkMaterialEfficient : MonoBehaviour
{
    public float interval = 0.5f;              // Jeda waktu antar pergantian material
    public Material[] materials;               // Daftar material yang akan digilir

    private Renderer objRenderer;
    private int currentMaterialIndex = 0;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();

        // Preload material (akses dulu supaya tidak ngelag saat ganti)
        foreach (var mat in materials)
        {
            var dummy = mat.name;
        }

        if (materials.Length > 0)
        {
            objRenderer.sharedMaterial = materials[currentMaterialIndex];
            StartCoroutine(ChangeMaterialLoop());
        }
    }

    IEnumerator ChangeMaterialLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;
            objRenderer.sharedMaterial = materials[currentMaterialIndex];
        }
    }
}
