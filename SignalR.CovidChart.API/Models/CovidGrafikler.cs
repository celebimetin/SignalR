namespace SignalR.CovidChart.API.Models
{
    public class CovidGrafikler
    {
        public CovidGrafikler()
        {
            Counts = new List<int>();
        }

        public string CovidDate { get; set; }
        public List<int> Counts { get; set; }
    }
}
