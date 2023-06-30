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
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataAttributeRouting]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new CinemaContext().Transaction.AsQueryable());
        }

        [HttpPost]
        public IActionResult Post(TransactionDTO transactionDTO)

        {
            try
            {
                var transaction = _mapper.Map<Transaction>(transactionDTO);
                transaction.Code = Util.GetRandomString(10);
                _transactionRepository.CreateTransaction(transaction);
                return Ok(transaction.Code);
            }           
            catch (Exception ex) 
            {
                return Conflict("Some things went wrong!");
            }
            
        }

    }
}
