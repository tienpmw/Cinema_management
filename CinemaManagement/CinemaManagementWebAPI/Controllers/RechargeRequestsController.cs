using AutoMapper;
using BusinessObject;
using CinemaWebAPI.Utilities;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RechargeRequestsController : ControllerBase
    {
        private readonly IRechargeRequestRepository _rechargeRequestRepository;
        private readonly IMapper _mapper;

        public RechargeRequestsController(IRechargeRequestRepository rechargeRequestRepository, IMapper mapper)
        {
            _rechargeRequestRepository = rechargeRequestRepository;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataAttributeRouting]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(CinemaContext.Instance.RechargeRequest.AsQueryable());
        }

        [HttpPost]
        public IActionResult Post([FromForm]long userId, [FromForm]long amount)
        {
            
            try
            {
                RechargeRequest rechargeRequest = new RechargeRequest()
                {
                    Amount = amount,
                    UserId = userId
                };
                rechargeRequest.Code = Util.Instance.GetRandomString(7);
                //_rechargeRequestRepository.CreateRechargeRequest(rechargeRequest);
                return Ok(rechargeRequest.Code);
            }
            catch (Exception ex) 
            {
                return Conflict("Some things went wrong!");
            }
            
        }

    }
}
