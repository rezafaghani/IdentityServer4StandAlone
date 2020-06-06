using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Primitives;

namespace Apotheek.Base.AuditLogging.Helpers.HttpContextHelpers
{
    public class HttpContextHelpers
    {
        public static IDictionary<string, string> GetFormVariables(HttpContext context)
        {
            if (!context.Request.HasFormContentType)
            {
                return null;
            }
            IFormCollection formCollection;
            try
            {
                formCollection = context.Request.Form;
            }
            catch (InvalidDataException)
            {
                // InvalidDataException could be thrown if the form count exceeds the limit, etc
                return null;
            }
            return ToDictionary(formCollection);
        }

        public static IDictionary<string, string> ToDictionary(IEnumerable<KeyValuePair<string, StringValues>> col)
        {
            if (col == null)
            {
                return null;
            }
            IDictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var k in col)
            {
                dict.Add(k.Key, string.Join(", ", k.Value));
            }
            return dict;
        }
    }
}