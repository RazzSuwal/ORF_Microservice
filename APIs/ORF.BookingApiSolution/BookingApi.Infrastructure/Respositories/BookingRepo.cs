using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Respositories.Interfaces;
using Microservice.SharedLibrary.CURDHelper.Interfaces;
using Microservice.SharedLibrary.Logs;
using Microservice.SharedLibrary.Response;

namespace BookingApi.Infrastructure.Respositories
{
    public class BookingRepo(ICURDHelper helper) : IBookingRepo
    {
        private readonly ICURDHelper _helper = helper;
        public async Task<Response> CreateAsync(Booking entity)
        {
            try
            {
                var currentEntity = await _helper.InsertAsync(entity);
                if (currentEntity == 1)
                    return new Response(true, $"Booked successfully");
                else
                    return new Response(false, $"Error occured while booking");
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                // display scary-free message in the client
                return new Response(false, "Error occurred booling");
            }
        }

        public async Task<Response> UpdateAsync(Booking entity)
        {

            try
            {
                var booking = await _helper.GetAsync<Booking>(entity.Id);
                if (booking is null)
                    return new Response(false, "Booking notfound");
                var currentEntity = await _helper.UpdateAsync(entity);
                if (currentEntity == 1)
                    return new Response(true, "Booking update successfully");
                else
                    return new Response(false, $"Error occured while updating");
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                // display scary-free message in the client
                return new Response(false, "Error occurred updating");
            }
        }
    }
}
