namespace Model.Data
{
    // позволит иметь доступ к методу для добавления чего либо в инвентарь
    public interface ICanAddInInventory
    {
        void AddInInventory(string id, int value);
    }
}