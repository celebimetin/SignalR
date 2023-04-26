using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.CovidChart.API.Hubs;
using SignalR.CovidChart.API.Models;
using System.Data;

namespace SignalR.CovidChart.API.Services
{
    public class CovidService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService(AppDbContext appDbContext, IHubContext<CovidHub> hubContext)
        {
            _appDbContext = appDbContext;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _appDbContext.Covids.AsQueryable();
        }

        public async Task SavedCovid(Covid covid)
        {
            await _appDbContext.Covids.AddAsync(covid);
            await _appDbContext.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidChartList());
        }

        public List<CovidGrafikler> GetCovidChartList()
        {
            List<CovidGrafikler> covidCharts = new List<CovidGrafikler>();

            using (var command = _appDbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select tarih,[1],[2],[3],[4],[5] from\r\n(select [City],[Count],Cast([CovidDate] as date) as tarih from Covids) as covidTable\r\nPIVOT\r\n(sum(count) for City IN([1],[2],[3],[4],[5])) as pTable\r\norder by tarih asc";

                command.CommandType = CommandType.Text;

                _appDbContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidGrafikler covidGrafikler = new CovidGrafikler();
                        covidGrafikler.CovidDate = reader.GetDateTime(0).ToShortDateString();

                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (DBNull.Value.Equals(reader[x]))
                            {
                                covidGrafikler.Counts.Add(0);
                            }
                            else
                            {
                                covidGrafikler.Counts.Add(reader.GetInt32(x));
                            }
                        });

                        covidCharts.Add(covidGrafikler);
                    }
                }

                _appDbContext.Database.CloseConnection();
                return covidCharts;
            }
        }
    }
}