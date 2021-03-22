using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.MonicaData
{
    public class MonicaSerie
    {
        public MonicaSerie()
        {
            Id = Guid.NewGuid();
            Values = new List<string>();
        }

        public Guid Id { get; set; }

        public string SerieTitle { get; set; }

        public List<string> Values { get; set; }
    }
}