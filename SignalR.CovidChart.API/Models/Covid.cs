﻿using SignalR.CovidChart.API.Enums;

namespace SignalR.CovidChart.API.Models
{
    public class Covid
    {
        public int Id { get; set; }
        public ECity City { get; set; }
        public int Count { get; set; }
        public DateTime CovidDate { get; set; }
    }
}