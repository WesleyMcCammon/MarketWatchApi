using MarketWatch.Models;

namespace MarketWatch.LiveData
{
    public class DataManager
    {
        public static List<DateTimeModel> GetData()
        {
            var r = new Random();
            return new List<DateTimeModel>()
            {
                new DateTimeModel { CurrentDateTime = DateTime.Now.ToLongTimeString() }
            };
        }
    }
}
