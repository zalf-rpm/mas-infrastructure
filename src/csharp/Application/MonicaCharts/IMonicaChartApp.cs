using Core.MonicaData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.MonicaCharts
{
    public interface IMonicaChartApp
    {
        Task<List<MonicaBaseData>> GetAsync(string jsonFilePath);

        List<MonicaBaseData> GetFromJson(string jsonContent);
    }
}
