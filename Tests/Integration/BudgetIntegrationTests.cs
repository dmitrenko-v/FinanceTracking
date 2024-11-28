using System.Net;
using System.Text;
using Application.Dto.Budget;
using Application.Dto.Expense;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.Integration;

[TestFixture]
public class BudgetIntegrationTests
{
    [Test]
    public async Task AddBudgetMustAuthorize()
    {
        var client = CustomWebApplicationFactory.GetClient();

        var newBudget = new AddBudgetDto
        {
            CeilingAmount = 1000,
            CategoryName = "Category",
        };

        var content = new StringContent(JsonConvert.SerializeObject(newBudget), Encoding.UTF8, "application/json");

        var httpResponse = await client.PostAsync("/api/budget", content);

        Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
    
}