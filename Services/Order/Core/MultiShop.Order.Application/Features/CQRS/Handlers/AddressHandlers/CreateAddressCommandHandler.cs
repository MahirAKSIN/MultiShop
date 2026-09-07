using MultiShop.Order.Application.Features.CQRS.Results.AddressResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class CreateAddressCommandHandler
    {
        private readonly IRepository<Address> _repository;

        public CreateAddressCommandHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task Handler(AddressCreateCommand addressCreate)
        {
            await _repository.CreateAsync(new Address
            {
                City = addressCreate.City,
                Detail = addressCreate.Detail,
                District = addressCreate.District,
                UserId = addressCreate.UserId,
            });
        }

    }
}
