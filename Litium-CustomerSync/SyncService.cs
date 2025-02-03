using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Litium.Accelerator.Services
{
    public class CustomerSyncService
    {
        private readonly HttpClient _httpClient;

        public CustomerSyncService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private static readonly Dictionary<string, string> AddressTypesSystemIds = new Dictionary<string, string>
        {
            { "Delivery", "A9EE068D-2937-4548-BC8E-58F13B4EBF45" },
            { "BillingAndDelivery", "2F37716F-C54D-4B80-83C2-732764664649" },
            { "Address", "35016653-38E6-47D5-8CB5-791C0AD70AA4" },
            { "Billing", "BA20DF6C-3C4C-40D0-8414-C642FDDB949D" },
            { "AlternativeAddress", "F0825AF2-17C2-47FE-876B-D4F2F17FCEAC" }
        };

        public async Task SyncCustomersFromFileAsync()
        {
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string filePath = Path.Combine(projectDirectory, "Dummydata", "Dummydata.xml");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Customer file not found.", filePath);
            }

            Customers customers;
            var serializer = new XmlSerializer(typeof(Customers));

            using (var stream = File.OpenRead(filePath))
            {
                customers = (Customers)serializer.Deserialize(stream);
            }

            foreach (var customer in customers.CustomerList)
            {             
                await SendOrganizationAsync(customer);                
                await SendPersonAsync(customer);
            }
        }

        private async Task SendOrganizationAsync(Customer customer)
        {
            string orgEndpoint = "https://bookstore.localtest.me:5001/Litium/api/admin/customers/organizations";
            var orgSystemId = Guid.NewGuid().ToString();
            var customerOrgAddresses = new List<Address>();

            Random rand = new Random();
            foreach (var address in customer.Addresses)
            {
                customerOrgAddresses.Add(new Address
                {
                    AddressTypeSystemId = AddressTypesSystemIds.ElementAt(rand.Next(0, AddressTypesSystemIds.Count)).Value,
                    SystemId = orgSystemId,
                    Address1 = address.Address1,
                    City = address.City,
                    State = address.State,
                    ZipCode = address.ZipCode,
                    Country = address.Country,
                    PhoneNumber = address.PhoneNumber,
                });
            }

            var organizationPayload = new
            {
                addresses = customerOrgAddresses,
                fields = new
                {
                    _name = new { enUS = customer.Fields.Name.EnUS, svSE = customer.Fields.Name.SvSE },
                    _nameInvariantCulture = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.NameInvariantCulture }
                    },
                    _email = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.Email }
                    },
                    _firstName = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.FirstName }
                    },
                    _lastName = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.LastName }
                    },
                    _phone = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.PhoneNumber }
                    },
                    LegalRegistrationNumber = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.LegalRegistrationNumber }
                    },
                    SocialSecurityNumber = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.SocialSecurityNumber }
                    },
                },
                fieldTemplateSystemId = customer.FieldTemplateSystemId,
                id = Guid.NewGuid().ToString(),
                systemId = orgSystemId
            };

            await SendDataAsync(orgEndpoint, organizationPayload);
        }

        private async Task SendPersonAsync(Customer customer)
        {
            string personEndpoint = "https://bookstore.localtest.me:5001/Litium/api/admin/customers/people";
            var personSystemId = Guid.NewGuid().ToString();
            var personAddresses = new List<Address>();

            Random rand = new Random();
            foreach (var address in customer.Addresses)
            {
                personAddresses.Add(new Address
                {
                    AddressTypeSystemId = AddressTypesSystemIds.ElementAt(rand.Next(0, AddressTypesSystemIds.Count)).Value,
                    SystemId = personSystemId,
                    Address1 = address.Address1,
                    City = address.City,
                    State = address.State,
                    ZipCode = address.ZipCode,
                    Country = address.Country,
                    PhoneNumber = address.PhoneNumber,
                });
            }

            var personPayload = new
            {
                loginCredential = new
                {
                    newPassword = "Test!",
                    lockoutEnabled = false,
                    lockoutEndDate = "2025-12-31T23:59:59.999Z",
                    passwordExpirationDate = "2025-12-31T23:59:59.999Z",
                    username = RandomUsername()
                },
                addresses = personAddresses,
                fields = new
                {
                    _name = new { enUS = customer.Fields.Name.EnUS, svSE = customer.Fields.Name.SvSE },
                    _email = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.Email }
                    },
                    _firstName = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.FirstName }
                    },
                    _lastName = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.LastName }
                    },
                    _phone = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.PhoneNumber }
                    },
                    LegalRegistrationNumber = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.LegalRegistrationNumber }
                    },
                    SocialSecurityNumber = new Dictionary<string, string>
                    {
                        { "*", customer.Fields.SocialSecurityNumber }
                    },
                },
                fieldTemplateSystemId = customer.FieldTemplateSystemId,
                id = Guid.NewGuid().ToString(),
                systemId = personSystemId
            };

            await SendDataAsync(personEndpoint, personPayload);
        }

        private object RandomUsername()
        {
            Random _random = new Random();
            List<string> fName = new List<string>
            {
                "Hans", "Sven", "Claes", "Bosse", "Goran", "Janne", "Lasse", "Manne", "Ricky", "Anders"
            };

            List<string> lName = new List<string>
            {
                "Allbäck", "Larsson", "Zidane", "Henry", "Bergkamp", "Cazorla", "Baggio", "Lustig", "Redondo", "Pirlo"
            };
            
            string randomFName = fName[_random.Next(fName.Count)];
            string randomLName = lName[_random.Next(lName.Count)];
            int randomNumber = _random.Next(100, 1000);
            
            string username = $"{randomFName}{randomLName}{randomNumber}";
            return username;
        }

        private async Task SendDataAsync(string endpoint, object payload)
        {
            try
            {
                if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "ServiceAccount MTo4MmY4NWJlMTYyNTg0NjY2OWYzZDdkMGEyNTdhZTA3OWM2YTVjMTYyMjdhYTQxNTQ5ZDBiMTVkNDFmNjcwMjU3");
                }

                var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    Console.WriteLine($"Response Body: {responseContent}");
                }

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}