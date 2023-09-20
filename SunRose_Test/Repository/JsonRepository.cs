using System.Text.Json;

namespace SunRose_Test.Repository
{
    public class JsonRepository<T>: IRepository<T>
    {
        public string Create(List<T> entities)
        {
            string fileName = "repo.json";
            File.WriteAllText(fileName, JsonSerializer.Serialize(entities));
            return fileName;
        }

        public List<T> Read(string path= "repo.json")
        {
            if (File.Exists(path))
            {
                var file = File.ReadAllText(path);

                return JsonSerializer.Deserialize<List<T>>(file);
            }
            else
                return new List<T>();
        }
    }
}
