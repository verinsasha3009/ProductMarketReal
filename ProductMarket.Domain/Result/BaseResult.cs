using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Result
{
    public class BaseResult
    {
        public bool IsSucces => ErrorMessage == null;
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        
    }
    public class BaseResult<T> : BaseResult
    {
        public BaseResult(string errorMessage,int errorCode,T result)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = result;
        }
        public BaseResult() { }
        public T Data { get; set; }
    }
}
