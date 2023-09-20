namespace SunRose_Test.Repository
{
    public interface IRepository<T>
    {
        string Create(List<T> entities);
        List<T> Read(string filename);
    }
}
