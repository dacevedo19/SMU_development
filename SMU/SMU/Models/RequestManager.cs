﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public class RequestManager : IRequestManager
    {
        private readonly AppDbContext context;

        public RequestManager(AppDbContext context)
        {
            this.context = context;
        }

        public bool Create(Request request)
        {
            if (request == null) { return false; }
            try {
                context.Requests.Add(request);
                context.SaveChanges();
                return true;
            } catch { return false; }

        }

        public bool Delete(int requestId)
        {
            Request r = context.Requests.Find(requestId);
            if (r == null) { return false; }
            try {
                context.Requests.Remove(r);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool Update(Request request)
        {
            try
            {
                var r = context.Requests.Attach(request);
                r.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public Request Find(int requestId)
        {
            return context.Requests.Find(requestId);            
        }

        public List<Request> GetRequests()
        {
            return context.Requests.ToList();
        }

       

    }
}