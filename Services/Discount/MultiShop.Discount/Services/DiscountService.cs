using Dapper;
using MultiShop.Discount.Context;
using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services
{
    public class DiscountService : IDiscountService
    {

        private readonly DapperContext _dapperContext;

        public DiscountService(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task CreateCouponDtoAsync(CreateCouponDto createCouponDto)
        {
            string query = "insert into [dbo].[Coupons] (Code,Rate,IsActive,Validate) values(@code,@rate,@isActive,@validDate)";
            var parameters = new DynamicParameters();
            parameters.Add("@code", createCouponDto.Code);
            parameters.Add("@rate", createCouponDto.Rate);
            parameters.Add("@isActive", createCouponDto.IsActive);
            parameters.Add("@validDate", createCouponDto.Validate);

            using (var con = _dapperContext.createConnection())
            {
                await con.ExecuteAsync(query, parameters);
            }

        }

        public async Task DeleteCouponDtoAsync(int id)
        {
            string query = "delete from [dbo].[Coupons] from CouponId=@couponId";

            var parameters = new DynamicParameters();
            parameters.Add("couponId", id);
            using (var con = _dapperContext.createConnection())
            {
                await con.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<ResultCouponDto>> GetAllCouponAsync()
        {
            string query = "select * from [dbo].[Coupons]";
            using (var con = _dapperContext.createConnection())
            {
                var result = await con.QueryAsync<ResultCouponDto>(query);
                return result.ToList();

            }
        }

        public async Task<GetByIdCouponDto> GetByIdCouponDto(int id)
        {
            string query = "select * from [dbo].[Coupons] where CouponId=@couponId ";

            var parameters = new DynamicParameters();
            parameters.Add("@couponId", id);
            using (var con = _dapperContext.createConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<GetByIdCouponDto>(query, parameters);
                return result;

            }
        }

        public async Task UpdateCouponDtoAsync(UpdateCouponDto updateCouponDto)
        {
            string query = "UPDATE [dbo].[Coupons] " +
                                                    "SET " +
                                                    "Code = @code, " +
                                                    "Rate = @rate," +
                                                    "IsActive = @isActive," +
                                                    "Validate = @validDate WHERE CouponId = @couponId;";
            var parameters = new DynamicParameters();
            parameters.Add("@code", updateCouponDto.Code);
            parameters.Add("@rate", updateCouponDto.Rate);
            parameters.Add("@isActive", updateCouponDto.IsActive);
            parameters.Add("@validDate", updateCouponDto.Validate);
            parameters.Add("@couponId", updateCouponDto.CouponId);

            using (var con = _dapperContext.createConnection())
            {
                await con.ExecuteAsync(query, parameters);
            }
        }

    }
}
