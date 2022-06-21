using System;
using System.Collections.Generic;
using Model.Data;
using UnityEditor;
using UnityEngine;

namespace Model.Definitions.Editor
{
    // необходимо понять какой объект мы отрисовываем
    // передаем название типа атрибута который передаем
    [CustomPropertyDrawer(typeof(InventoryIdAttribute))]
    
    public class InventoryIdAttributeDrawer : PropertyDrawer
    {
        // переопределим стандартный сервисный метод
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // получим список всех id
            var defs = DefsFacade.I.Items.itemsForEditor;
            var ids = new List<string>();
            foreach (var itemDef in defs)
            {
                ids.Add(itemDef.Id);
            }
            
            // сделаем дропдаун список
            // объявим индекс, строковое значение переменной которую мы обрабатываем
            // получаем текущий индекс
            //var index = ids.IndexOf(property.stringValue);
            // чтобы не было ошибок при добавлении новых компонентов, если -1 то сделаем 0
            var index = Mathf.Max(ids.IndexOf(property.stringValue), 0);
            
            //      = отрисовываем дроп даун        позиция, как будет называться, индекс и список элементов
            index = EditorGUI.Popup(position, property.displayName, index, ids.ToArray());
            // если выбрали другой элемент, то передаем его в index
            // обновим строковое значение по id
            property.stringValue = ids[index];
        }
    }
}