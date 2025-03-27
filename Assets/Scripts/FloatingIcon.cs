using UnityEngine;

public class FloatingIcon : MonoBehaviour
{
    [SerializeField] private float heightOffset = 2f; // Высота над головой персонажа
    [SerializeField] private float followSpeed = 20f; // Скорость следования за персонажем

    private Transform target; // Цель, за которой будет следовать иконка

    void Update()
    {
        if (target != null)
        {
            // Вычисляем позицию над головой персонажа
            Vector3 targetPosition = target.position + Vector3.up * heightOffset;
            // Плавно перемещаем иконку к целевой позиции
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    // Метод для установки цели
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}