using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers;
using MultiShop.Order.Application.Features.CQRS.Queries.AddressQueries;
using MultiShop.Order.Application.Features.CQRS.Results.AddressResults;

namespace MultiShop.Order.Presention.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly CreateAddressCommandHandler _createAddressCommandHandler;
        private readonly UpdateAddressCommandHandler _updateAddressCommandHandler;
        private readonly RemoveAddressCommandHandler _removeAddressCommandHandler;
        private readonly GetAddressByIdQueryHandler _getAddressByIdQueryHandler;
        private readonly GetAddressQueryHandler _getAddressQueryHandler;

        public AddressesController(CreateAddressCommandHandler createAddressCommandHandler, UpdateAddressCommandHandler updateAddressCommandHandler, RemoveAddressCommandHandler removeAddressCommandHandler, GetAddressByIdQueryHandler getAddressByIdQueryHandler, GetAddressQueryHandler getAddressQueryHandler)
        {
            _createAddressCommandHandler = createAddressCommandHandler;
            _updateAddressCommandHandler = updateAddressCommandHandler;
            _removeAddressCommandHandler = removeAddressCommandHandler;
            _getAddressByIdQueryHandler = getAddressByIdQueryHandler;
            _getAddressQueryHandler = getAddressQueryHandler;
        }
        [HttpGet]
        public async Task<IActionResult> AddressList()
        {
            var values = await _getAddressQueryHandler.Handler();
            return Ok(values);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAddress(AddressUpdateQueryResult command)
        {
            await _updateAddressCommandHandler.Handler(command);
            return Ok("Address update succesed");
        }
        [HttpPost]
        public async Task<IActionResult> CreateAddress(AddressCreateCommand command)
        {
            await _createAddressCommandHandler.Handler(command);
            return Ok("Address create succesed");

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> AddressListById(int id)
        {
            var values = await _getAddressByIdQueryHandler.Handler(new GetAddressByIdQuery(id));
            return Ok(values);

        }

        [HttpDelete]
        public async Task RemoveAddress(int id)
        {
            await _removeAddressCommandHandler.Handler(new AddressDeleteCommand(id));
            Ok("Adress deleted succes");
        }
    
    }
}
