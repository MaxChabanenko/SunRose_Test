namespace SunRose_Test.Models.ModelViews
{
    public enum SortingOption
    {
        Id,
        Date
    }
    public class MessagesAndSort
    {
       public IEnumerable<Message> Messages { get; set; }
       public SortingOption SortBy { get; set; }
    }
}
