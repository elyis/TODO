namespace Todo.Core.IService
{
    public interface IHashPasswordService
    {
        string Compute(string password);
    }
}