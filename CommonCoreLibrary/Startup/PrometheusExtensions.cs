using Microsoft.AspNetCore.Builder;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCoreLibrary.Startup
{
    public static class PrometheusExtensions
    {
        public static void UsePrometheusMetric(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMetricServer();
        }
    }
}
