using UnityEngine;
using System.Collections.Generic;

public class ParticleShapeController : MonoBehaviour
{
    [System.Serializable]
    private struct ColliderToParticleMapping
    {
        public Vector2 colliderSize; // Tamaño exacto del CapsuleCollider2D
        public Vector3 shapePosition; // Posición en el módulo Shape del sistema de partículas
        public Vector3 shapeScale; // Escala en el módulo Shape del sistema de partículas
    }

    [SerializeField] private CapsuleCollider2D capsuleCollider; // Referencia al CapsuleCollider2D
    [SerializeField] private ParticleSystem particleSystemRef; // Referencia al sistema de partículas
    [SerializeField] private List<ColliderToParticleMapping> mappings; // Lista de mapeos configurables desde el inspector

    private ParticleSystem.ShapeModule shapeModule;

    private void Update()
    {
        UpdateParticleShape();
    }

    private void Start()
    {
        if (capsuleCollider == null || particleSystemRef == null)
        {
            Debug.LogError("Faltan referencias en el inspector.");
            return;
        }

        shapeModule = particleSystemRef.shape; // Obtener el módulo Shape del sistema de partículas

        UpdateParticleShape(); // Ajustar las partículas según el tamaño del collider
    }

    private void UpdateParticleShape()
    {
        Vector2 colliderSize = capsuleCollider.size;
        Debug.Log($"Collider Size en tiempo de ejecución: {colliderSize}");

        float tolerance = 0.1f; // Ajusta este valor según sea necesario

        foreach (var mapping in mappings)
        {
            if (Mathf.Abs(colliderSize.x - mapping.colliderSize.x) < tolerance &&
                Mathf.Abs(colliderSize.y - mapping.colliderSize.y) < tolerance)
            {
                shapeModule.position = mapping.shapePosition;
                shapeModule.scale = mapping.shapeScale;
                Debug.Log($"Ajustando Shape: Posición {mapping.shapePosition}, Escala {mapping.shapeScale}");
                return; // Sale de la función cuando encuentra la coincidencia
            }
        }

        Debug.LogWarning($"Tamaño del CapsuleCollider2D no reconocido: {colliderSize.x}, {colliderSize.y}");
    }
}
