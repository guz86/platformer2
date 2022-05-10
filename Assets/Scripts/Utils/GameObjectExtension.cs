using UnityEngine;

public static class GameObjectExtension
{
    // как будто вызываем метод у нашего обхекта go, находится ли слой
    // go в маске layer
    public static bool IsInLayer(this GameObject go, LayerMask layer)
    {
        return layer == (layer | 1 << go.layer);
        //битовая маска                     побитовый сдвиг  маски
    }
}
