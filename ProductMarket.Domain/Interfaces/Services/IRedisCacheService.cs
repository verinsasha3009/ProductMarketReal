using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces.Services
{
    public interface IRedisCacheService
    {
        T Get<T>(string key);
        T Set<T>(string key, T value);
        T Delete<T>(string key);
        List<T> GetAllKeys<T>();
    }
}
