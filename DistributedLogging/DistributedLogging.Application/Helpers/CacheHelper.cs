using DistributedLogging.common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DistributedLogging.Application.Helpers
{
    public static class CacheHelper
    {

        public static string BuildCachKey<T>(string entity, T request)
        {
            List<PropertyInfo> props = new List<PropertyInfo>(request.GetType().GetProperties().Where(c => c.GetValue(request) != null));
            var filteration = GetFilterations(props, request);
            return $"{SettingsManager.RedisSettings.InstanceName}{entity}:{string.Join("&", filteration)}:Culture={CultureInfo.CurrentCulture.Name}".ToLower();
        }

        static List<string> GetFilterations<T>(List<PropertyInfo> props, T request)
             => props.OrderBy(p => p.Name).Select(p => $"{p.Name}={(!IsEnumerableType(p.PropertyType) ? p.GetValue(request) : JsonSerializer.Serialize(props.FirstOrDefault().GetValue(request)))}").ToList();
        static bool IsEnumerableType(Type type)
            => typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
    }
}
