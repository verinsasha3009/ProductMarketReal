using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Result
{
    public class CollectResult<T> : BaseResult<T>
    {
        public List<T> Result {  get; set; }
        public int Count { get; set; }
    }
}
