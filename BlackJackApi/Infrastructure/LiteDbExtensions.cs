﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackApi.Infrastructure
{
    public static class LiteDbExtensions
    {
        public static void AddLiteDb(this IServiceCollection services, string databasePath)
        {
            services.AddSingleton<LiteDbContext, LiteDbContext>();
            services.Configure<LiteDbConfig>(options => options.DatabasePath = databasePath);
        }
    }
}
