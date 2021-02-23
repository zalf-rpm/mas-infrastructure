using Core.MonicaData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Application.Convertor
{
    public interface IMonicaJsonMapper
    {
        List<MonicaBaseData> Map(string data);
    }
}
