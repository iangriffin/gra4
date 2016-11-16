using GRA.Domain.Abstract;
using GRA.Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace GRA.Domain
{
    public class Service
    {
        private readonly ILogger<Service> logger;
        private readonly ISiteRepository siteRepository;
        public Service(ILogger<Service> logger, ISiteRepository siteRepository)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            this.logger = logger;
            if (siteRepository == null)
            {
                throw new ArgumentNullException("repository");
            }
            this.siteRepository = siteRepository;
        }

        public IEnumerable<Site> GetSitePaths()
        {
            
            return siteRepository.GetAll();
        }

        public void InitialSetup(User user)
        {
            // todo verify paritipcant is admin?

            // create default site
            siteRepository.Add(0, new Model.Site
            {
                Name = "Default Site",
                Path = "default"
            });
        }
    }
}