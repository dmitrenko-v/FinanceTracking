using System.Net;
using System.Text;
using Application.Dto.Income;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.Integration;

[TestFixture]
public class IncomeIntegrationTests
{
    [Test]
    public async Task AddIncomeMustAuthorize()
    {
        var client = CustomWebApplicationFactory.GetClient();

        var newIncome = new AddIncomeDto
        {
            Title = "Income",
            Amount = 1000,
            Description = "description",
            Date = new DateTime(2020, 01, 01)
        };

        var content = new StringContent(JsonConvert.SerializeObject(newIncome), Encoding.UTF8, "application/json");

        var httpResponse = await client.PostAsync("/api/income", content);

        Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}