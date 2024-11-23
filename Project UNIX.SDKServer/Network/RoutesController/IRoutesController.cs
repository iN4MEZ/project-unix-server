﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_UNIX.SDKServer.Network.RoutesController
{
    internal interface IRoutesController
    {
        Task<Task> Handle(HttpContext httpContext);
    }
}
