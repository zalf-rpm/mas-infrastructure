using System;
using System.Collections.Generic;
using System.Text;

namespace Core.MonicaData
{
    public class MonicaBaseData
    {
        public MonicaBaseData(int id)
        {
            Id = id;
            MonicaSeries = new List<MonicaSerie>();
        }

        public int Id { get; set; }

        public string OrigSpec { get; set; }

        public List<MonicaSerie> MonicaSeries { get; set; }

    }
}
