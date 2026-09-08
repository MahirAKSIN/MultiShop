using MultiShop.Order.Application.Features.CQRS.Results.AddressResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class UpdateAddressCommandHandler
    {
        private readonly IRepository<Address> _repository;

        public UpdateAddressCommandHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }


        public async Task Handler(AddressUpdateQueryResult command)
        {
            var values = await _repository.GetByIdAsync(command.AddressId);

            values.Detail = command.Detail;
            values.City = command.City;
            values.District = command.District;
            values.UserId = command.UserId;
            await _repository.UpdateAsync(values);
        }
    }
}
