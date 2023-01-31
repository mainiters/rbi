using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Common;
using Terrasoft.Core;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Хелпер для работы с бизнес процессами
    /// </summary>
    public static class ProcessHelper
    {
        /// <summary>
        /// Запусе БП
        /// </summary>
        /// <returns></returns>
        public static bool RunProcess(UserConnection userConnection, string schemaName, Dictionary<string, object> inParams, Dictionary<string, object> outParams)
        {
            var res = false;

            try
            {
                if (string.IsNullOrWhiteSpace(schemaName))
                {
                    throw new ArgumentNullException(nameof(schemaName));
                }

                var parametes = inParams?.ToDictionary(e => e.Key, e => Convert.ToString(e.Value));
                var descriptor = userConnection.ProcessEngine.ProcessExecutor.Execute(schemaName, parametes);

                if (outParams != null)
                {
                    foreach (var item in outParams.Keys.ToArray())
                    {
                        outParams[item] = descriptor.GetPropertyValue(item);
                    }
                }

                if (descriptor.ProcessStatus != Terrasoft.Core.Process.ProcessStatus.Error)
                {
                    res = true;
                }
            }
            catch (Exception ex)
            {

            }

            return res;
        }
    }
}
