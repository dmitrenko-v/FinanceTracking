using System.Net;
using System.Text;
using Application.Dto.Expense;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.Integration;

[TestFixture]
public class ExpenseIntegrationTests
{
    [Test]
    public async Task AddExpenseMustAuthorize()
    {
        var client = CustomWebApplicationFactory.GetClient();

        var newExpense = new AddExpenseDto
        {
            Amount = 100,
            CategoryName = "Category",
            Date = DateTime.Now,
            Description = "New expense",
            Title = "Expense title"
        };

        var content = new StringContent(JsonConvert.SerializeObject(newExpense), Encoding.UTF8, "application/json");

        var httpResponse = await client.PostAsync("/api/expense", content);

        Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}