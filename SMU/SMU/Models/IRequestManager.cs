using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public interface IRequestManager
    {
        public bool Create(Request request);
        public bool Update(Request request);
        public bool Delete(int requestId);
        public Request Find(int requestId);
        public List<Request> GetRequests();
        public List<Request> GetRequestsByUserId(string id);

    }
}
