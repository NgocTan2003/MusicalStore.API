﻿using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.AutoConfiguration
{
    public class ResponseCreateUser
    {
        public string Message { get; set; }
        public int? StatusCode { get; set; }
        public string token { get; set; }
        public string Email { get; set; }

        public ResponseCreateUser()
        {

        }
        public ResponseCreateUser(string mess)
        {
            Message = mess;
        }
    }
}
