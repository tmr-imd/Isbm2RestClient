using Isbm2Client.Interface;
using Isbm2Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isbm2Client.Service
{
    public class RestProviderRequest : IProviderRequest
    {
        public Session OpenProviderRequestSession(string description, Channel channel, string[] topics, Uri listenerUrl, string[] filterExpressions) 
        {
            throw new NotImplementedException();
        }
    }
}
