namespace Model.Data.Properties
{
    // промежуточный класс где будем сохранять значение ключа
    public abstract class PrefsPersistentProperty<TPropertyType> : PersistentProperty<TPropertyType>
    {
        // сохраним ключ
        protected string Key;
        
        protected PrefsPersistentProperty(TPropertyType defaultValue, string key) : base(defaultValue)
        {
            Key = key;
        }
    }
}