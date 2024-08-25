using Grpc.Core;

namespace gRPC_nonDocker.Services
{
    public class CustomerService : Customer.CustomerBase
    {
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(ILogger<CustomerService> logger) {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();
            if (request.UserId == 1)
            {
                output.FirstName = "Cody";
                output.LastName = "Bryant";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Monty";
                output.LastName = "Cristo";
            }
            else
            {
                output.FirstName = "Unknown";
                output.LastName = "Unknown";
            }

            return Task.FromResult(output);
        }

        public override async Task GetNewCustomers(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel
                {
                    FirstName = "Tim",
                    LastName = "Cody",
                    Age = 15,
                    Email = "timcody@gmail.com",
                    IsAlive = true
                },
                new CustomerModel
                {
                    FirstName = "Ben",
                    LastName = "Cook",
                    Age = 17,
                    Email = "bencook@gmail.com",
                    IsAlive = false
                }
            };

            foreach (var c in customers)
            {
                await responseStream.WriteAsync(c);
            }
        }
    }
}
