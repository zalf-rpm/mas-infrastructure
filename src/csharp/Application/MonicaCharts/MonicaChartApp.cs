using Application.Convertor;
using Core.MonicaData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Application.MonicaCharts
{
    public class MonicaChartApp : IMonicaChartApp
    {
        private readonly IMonicaJsonMapper _monicaJsonMapper;

        public MonicaChartApp(IMonicaJsonMapper monicaJsonMapper)
        {
            _monicaJsonMapper = monicaJsonMapper;
        }

        public async Task<List<MonicaBaseData>> GetAsync(string jsonFilePath)
        {
            return _monicaJsonMapper.Map(File.ReadAllText(jsonFilePath));
        }

        public List<MonicaBaseData> GetFromJson(string jsonContent)
        {
            return _monicaJsonMapper.Map(jsonContent);
        }
    }
}
