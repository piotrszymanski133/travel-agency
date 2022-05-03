using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ApiGateway.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using CommonComponents;
using CommonComponents.Models;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripController : ControllerBase
    {
        private IRequestClient<GetTripsQuery> _tripsClient;
        private IRequestClient<GetTripOfferQuery> _tripOfferclient;

        public TripController(IRequestClient<GetTripsQuery> tripsClient, IRequestClient<GetTripOfferQuery> tripOfferclient)
        {
            _tripOfferclient = tripOfferclient;
            _tripsClient = tripsClient;
        }

        [HttpGet]
        public async Task<GetTripsResponse> Index([FromQuery] TripParameters tripParameters)
        {
            var response =
                await _tripsClient.GetResponse<GetTripsResponse>(new GetTripsQuery {TripParameters = tripParameters});
            return response.Message;
        }
        
        [HttpGet]
        [Route("GetTrip")]
        public async Task<GetTripOfferResponse> GetTrip([FromQuery] TripOfferQueryParameters tripOfferQueryParameters)
        {
            var response = await _tripOfferclient.GetResponse<GetTripOfferResponse>(new GetTripOfferQuery
            {
                TripOfferQueryParameters = tripOfferQueryParameters
            });
            return response.Message;
        }
        
        [HttpGet]
        [Route("GetDestinations")]
        public async Task<List<String>> GetDestinations()
        {
            List<String> response = new List<String> { 
                "Albania",
                "Armenia",
                "Australia",
                "Austria", 
                "Azerbejdżan", 
                "Bali",
                "Bośnia i Hercegowina",
                "Bułgaria",
                "Chiny", 
                "Chorwacja",
                "Cypr",
                "Czarnogóra", 
                "Czechy",
                "Dania",
                "Dominikana",
                "Egipt", 
                "Ekwador",
                "Etiopia",
                "Francja",
                "Grecja",
                "Gwadelupa",
                "Hiszpania", 
                "Holandia",
                "Indie", 
                "Indonezja",
                "Irlandia", 
                "Islandia",
                "Izrael",
                "Kambodża",
                "Kanada",
                "Kostaryka", 
                "Kuba",
                "Liechtenstein",
                "Luksemburg",
                "Madagaskar",
                "Malediwy", 
                "Malezja",
                "Malta",
                "Maroko", 
                "Martynika",
                "Mauritius", 
                "Meksyk",
                "Mołdawia", 
                "Monako",
                "Nepal", 
                "Niemcy",
                "Norwegia",
                "Nowa Zelandia",
                "Panama",
                "Peru",
                "Polska", 
                "Portoryko", 
                "Portugalia",
                "Rumunia",
                "Seszele",
                "Słowacja", 
                "Słowenia", 
                "Somalia", 
                "Sri Lanka", 
                "Stany Zjednoczone",
                "Szwajcaria", 
                "Szwecja",
                "Tajlandia",
                "Tunezja",
                "Turcja",
                "Wielka Brytania", 
                "Wietnam", 
                "Włochy",
                "Zimbabwe", 
                "Zjednoczone Emiraty Arabskie"};
            
            return response;
        }
    }
}