using UnityEngine;
using System.Collections.Generic;

public class ParticleShapeController : MonoBehaviour
{
    [System.Serializable]
    private struct ColliderToParticleMapping
    {
        public Vector2 colliderSize; // Tama�o exacto del CapsuleCollider2D
        public Vector3 shapePosition; // Posici�n en el m�dulo Shape del sistema de part�culas
        public Vector3 shapeScale; // Escala en el m�dulo Shape del sistema de part�culas
    }

    [SerializeField] private CapsuleCollider2D capsuleCollider; // Referencia al CapsuleCollider2D
    [SerializeField] private ParticleSystem particleSystemRef; // Referencia al sistema de part�culas
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

        shapeModule = particleSystemRef.shape; // Obtener el m�dulo Shape del sistema de part�culas

        UpdateParticleShape(); // Ajustar las part�culas seg�n el tama�o del collider
    }

    private void UpdateParticleShape()
    {
        Vector2 colliderSize = capsuleCollider.size;
        Debug.Log($"Collider Size en tiempo de ejecuci�n: {colliderSize}");

        float tolerance = 0.1f; // Ajusta este valor seg�n sea necesario

        foreach (var mapping in mappings)
        {
            if (Mathf.Abs(colliderSize.x - mapping.colliderSize.x) < tolerance &&
                Mathf.Abs(colliderSize.y - mapping.colliderSize.y) < tolerance)
            {
                shapeModule.position = mapping.shapePosition;
                shapeModule.scale = mapping.shapeScale;
                Debug.Log($"Ajustando Shape: Posici�n {mapping.shapePosition}, Escala {mapping.shapeScale}");
                return; // Sale de la funci�n cuando encuentra la coincidencia
            }
        }

        Debug.LogWarning($"Tama�o del CapsuleCollider2D no reconocido: {colliderSize.x}, {colliderSize.y}");
    }
}
