using System.Collections.Generic;


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
        public bool AcceptBySupervisor(int requestId);
        public bool Accept(int requestId);
        public bool Reject(int requestId);

    }
}
