using MediatR;
using MultiShop.Order.Application.Features.CQRS.Results.OrderDetailResults;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResult;

namespace MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries
{
    public class GetOrderingByIdQuery:IRequest<GetOrderingByIdQueryResult>
    {
        public GetOrderingByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
