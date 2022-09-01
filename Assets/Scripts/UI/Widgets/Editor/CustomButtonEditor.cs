using UnityEditor;
using UnityEditor.UI;

namespace UI.Widgets.Editor
{
    // для того чтобы отобразить нужные нам поля через CustomEditor
    
    // для компонента CustomButton мы будем его использовать
    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : ButtonEditor
    {
        // код который будет работать только в редакторе, для добавления полей
        public override void OnInspectorGUI()
        {
            // добавим два свойства из CustomButton 
            // отрисует нам нужные property
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressed"));
            // применить изменения
            serializedObject.ApplyModifiedProperties();
            
            base.OnInspectorGUI();
        }
    }
}