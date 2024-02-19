using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Common.ResponseBase
{
    public class ResponseMessage
    {
        public string Message { get; set; }
        public int? StatusCode { get; set; }
        public ResponseMessage()
        {

        }
        public ResponseMessage(string mess)
        {
            Message = mess;
        }
    }
}
