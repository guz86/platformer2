using UnityEngine;

namespace Utils
{
    public static class GameObjectExtension
    {
        // как будто вызываем метод у нашего обхекта go, находится ли слой
        // go в маске layer
        public static bool IsInLayer(this GameObject go, LayerMask layer)
        {
            return layer == (layer | 1 << go.layer);
            //битовая маска                     побитовый сдвиг  маски
            // если GameObject будет не в том слое 
        }
        
        // метод который будет возвращать интерфейсы
        public static TInterfaceType GetInterface<TInterfaceType>(this GameObject go)
        {
            // с объекта забираем все компоненты
            var components = go.GetComponents<Component>();
            // если компонент интерфейс то возвращаем его
            foreach (var component in components)
            {
                if (component is TInterfaceType type)
                {
                    return type;
                }
            }
            return default;  
        }
    }
}
