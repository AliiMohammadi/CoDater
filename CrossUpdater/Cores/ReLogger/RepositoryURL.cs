using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CrossUpdater.Cores.ReLogger
{
    internal class RepositoryURL 
    {
        public Url Address { get; set; }

        public string DomainName { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }

        public RepositoryURL(Url address)
        {
            Address = address;
            string[] roots = address.Value.Replace("https://","").Replace("http://","").Split('/');
            DomainName = roots[0];
            Username = roots[1];
            Name = roots[2];
        }
    }
}
