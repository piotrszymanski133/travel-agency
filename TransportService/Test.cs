using System;
using Microsoft.AspNetCore.Mvc;
using TripService.Repository;

namespace TransportService
{
    public class Test : Controller
    {
        private ITransportRepository _repository;
        // GET
        public Test(ITransportRepository repository)
        {
            _repository = repository;
        }

        [Route("/XXX")]
        public bool Index()
        {
            var guid = new Guid("7981f206-d0de-4c51-9596-338c6e598f16");
            var ret2 = _repository.ReserveTransport(1, 2, 2,guid,new DateTime(2022,6,1),new DateTime(2022,6,3),"user1");
            _repository.RollbackReserveTransport(guid);

            return ret2.Item2;
        }
    }
}