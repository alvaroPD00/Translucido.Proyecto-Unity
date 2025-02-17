using UnityEngine;

public class AguaColor : MonoBehaviour
{
    [SerializeField] private Material material; // Material con el shader que contiene las propiedades HSV
    [SerializeField] private Transform player; // Referencia al objeto jugador

    [Header("Rango del eje X del jugador")]
    [SerializeField] private float playerMinX = -6f;
    [SerializeField] private float playerMaxX = 340f;

    [Header("Rangos para ShallowColor")]
    [SerializeField] private Vector2 shallowHRange = new Vector2(120f, 240f); // Rango de H
    [SerializeField] private Vector2 shallowSRange = new Vector2(0f, 1f);     // Rango de S
    [SerializeField] private Vector2 shallowVRange = new Vector2(0.2f, 0.75f); // Rango de V

    [Header("Rangos para DeepColor")]
    [SerializeField] private Vector2 deepHRange = new Vector2(60f, 180f); // Rango de H
    [SerializeField] private Vector2 deepSRange = new Vector2(0.5f, 1f);  // Rango de S
    [SerializeField] private Vector2 deepVRange = new Vector2(0.3f, 0.9f); // Rango de V

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("El objeto Player no está asignado.");
            return;
        }

        if (material == null)
        {
            Debug.LogError("El material no está asignado.");
            return;
        }

        // Obtener la posición X del jugador
        float playerX = player.position.x;
        Debug.Log($"Posición X del jugador: {playerX}");

        // Mapear valores para ShallowColor
        float shallowH = Map(playerX, playerMinX, playerMaxX, shallowHRange.x, shallowHRange.y);
        float shallowS = Map(playerX, playerMinX, playerMaxX, shallowSRange.x, shallowSRange.y);
        float shallowV = Map(playerX, playerMinX, playerMaxX, shallowVRange.x, shallowVRange.y);
        Debug.Log($"ShallowColor mapeado: H={shallowH}, S={shallowS}, V={shallowV}");

        // Mapear valores para DeepColor
        float deepH = Map(playerX, playerMinX, playerMaxX, deepHRange.x, deepHRange.y);
        float deepS = Map(playerX, playerMinX, playerMaxX, deepSRange.x, deepSRange.y);
        float deepV = Map(playerX, playerMinX, playerMaxX, deepVRange.x, deepVRange.y);
        Debug.Log($"DeepColor mapeado: H={deepH}, S={deepS}, V={deepV}");

        // Convertir HSV a RGB y asignar a las propiedades del material
        Color shallowColor = Color.HSVToRGB(shallowH / 360f, shallowS, shallowV);
        Color deepColor = Color.HSVToRGB(deepH / 360f, deepS, deepV);
        Debug.Log($"ShallowColor RGB: {shallowColor}");
        Debug.Log($"DeepColor RGB: {deepColor}");

        Debug.Log($"Aplicando ShallowColor: {shallowColor}");
        material.SetColor("_ShallowColor", shallowColor);

        Debug.Log($"Aplicando DeepColor: {deepColor}");
        material.SetColor("_DeepColor", deepColor);

        Debug.Log("Colores asignados al material.");
        // Verificar después de la asignación
        Debug.Log($"Verificando después: ShallowColor={material.GetColor("_ShallowColor")}, DeepColor={material.GetColor("_DeepColor")}");
    }

    // Función para mapear un valor de un rango a otro
    private float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float mappedValue = Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
        Debug.Log($"Mapeando {value} de [{fromMin}, {fromMax}] a [{toMin}, {toMax}]: {mappedValue}");
        return mappedValue;
    }
}