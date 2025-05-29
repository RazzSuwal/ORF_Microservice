using BookingApi.Application.DTOs;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Respositories.Interfaces;
using Confluent.Kafka;
using Microservice.SharedLibrary.CURDHelper.Interfaces;
using Microservice.SharedLibrary.Logs;
using Microservice.SharedLibrary.Response;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace BookingApi.Infrastructure.Respositories
{
    public class BookingRepo : IBookingRepo
    {
        private readonly ICURDHelper _helper;
        private readonly IConsumer<Null, string> _consumer;
        public BookingRepo(ICURDHelper helper, IConsumer<Null, string> consumer)
        {
            _helper = helper;
            _consumer = consumer;
            Task.Run(() => StartConsuming());
        }

        private const string AddPropertyTopic = "add-property-topic";
        private const string DeletePropertyTopic = "delete-property-topic";
        private const string AddAppUserTopic = "add-appUser-topic";

        public List<PropertyDTO> Properties { get; set; } = new();

        private async Task StartConsuming()
        {
            await Task.Delay(10);
            _consumer.Subscribe([AddPropertyTopic]);

            try
            {
                while (true)
                {
                    var response = _consumer.Consume(CancellationToken.None);
                    if (!string.IsNullOrEmpty(response.Message.Value))
                    {
                        // check if topic == add product topic
                        if (response.Message.Value == AddPropertyTopic) 
                        {
                            var properties = JsonSerializer.Deserialize<PropertyDTO>(response.Message.Value);
                            Properties.Add(properties!);
                        }
                        else //Delete Property Topic
                        {
                            Properties.Remove
                                (Properties.FirstOrDefault(p => p.Id == int.Parse(response.Message.Value))!);
                        }
                        ConsoleProperties();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }

        private void ConsoleProperties()
        {
            Console.Clear();
            foreach(var item in Properties) 
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Description: {item.Description}, Area: {item.Area}");
        }

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
                return new Response(false, "Error occurred booking");
            }
        }

        public Task<IEnumerable<BookingDTO>> GetBookingByClientId(int bookBy)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BookingDetailsDTO>> GetBookingByPropertyId(int propertyId)
        {
            try
            {
                var bookings = await _helper.GetListAsync<Booking>(new { PropertyId = propertyId });

                if (bookings == null || !bookings.Any())
                    return Enumerable.Empty<BookingDetailsDTO>();

                var property = Properties.FirstOrDefault(p => p.Id == propertyId);
                if (property == null)
                    return Enumerable.Empty<BookingDetailsDTO>();

                var bookingDetails = new List<BookingDetailsDTO>();

                foreach (var booking in bookings)
                {
                    bookingDetails.Add(new BookingDetailsDTO(
                        booking.Id,
                        booking.Description,
                        booking.BookOn,
                        booking.PropertyId,
                        property.Name,
                        property.Description,
                        property.Area,
                        property.Location,
                        property.NumberOfRooms,
                        property.Price
                    ));
                }

                return bookingDetails;
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                // display scary-free message in the client
                throw new Exception("Error occurred while retrieving booking");
            }
        }


        public Task<BookingDTO> GetBookingDetails(int bookingId)
        {
            throw new NotImplementedException();
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
