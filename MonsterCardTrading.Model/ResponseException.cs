using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class ResponseException : Exception
    {
        public string Message { get; set; }
        public int  ErrorCode { get; set; }
        public ResponseException(string message, int errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }


    }
}
