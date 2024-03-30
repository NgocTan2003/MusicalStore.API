using MusicalStore.Application.AutoConfiguration;
using MusicalStore.Common.ResponseBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<ResponseMessage> SendEmail(MessageEmail request);
        MessageEmail ChangeToMessageEmail(string To, string Subject, string Body);

    }
}
