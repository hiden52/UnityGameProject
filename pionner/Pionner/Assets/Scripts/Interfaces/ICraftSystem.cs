public interface ICraftSystem
{
    bool CanCraft();
    void StartCrafting(); 
    void CancelCrafting();
    void ProcessCraft();
}