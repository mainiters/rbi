using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RbiIntegration;
using RbiIntegration.Service.BaseClasses;

namespace RbiIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            var phone = @"ad909-438-909-4";
            
            Console.WriteLine(IntegrationServiceHelper.GetReversedPhone(phone));

            Console.ReadLine();
        }
    }
}
