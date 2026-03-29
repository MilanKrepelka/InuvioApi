using System;
using System.Collections.Generic;
using System.Text;

namespace ASOL.Inuvio.Api.Client.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Exception"/> class to provide enhanced functionality for exception handling and logging.
    /// </summary>
    public static class ExceptionExtensions
    {
        public static string GetFullException(this Exception? ex)
        {
            var sb = new System.Text.StringBuilder();
            int level = 0;

            while (ex != null)
            {
                sb.AppendLine($"--- Exception Level {level} ---");
                sb.AppendLine($"Type: {ex.GetType().FullName}");
                sb.AppendLine($"ErrorMessage: {ex.Message}");
                sb.AppendLine($"StackTrace: {ex.StackTrace}");

                if (ex.Data?.Count > 0)
                {
                    sb.AppendLine("Data:");
                    foreach (var key in ex.Data.Keys)
                        sb.AppendLine($"  {key}: {ex.Data[key]}");
                }

                ex = ex.InnerException;
                level++;
            }

            return sb.ToString();
        }
    }
}
