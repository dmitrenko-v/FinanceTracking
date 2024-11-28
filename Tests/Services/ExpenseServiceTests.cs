using Application.Dto.Expense;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace Tests.Services;

[TestFixture]
public class ExpenseServiceTests
{
    [Test]
    public async Task AddExpenseAsyncAddsValidExpense()
    {
        var newExpense = new AddExpenseDto
        {
            Amount = 100,
            CategoryName = "Category",
            Date = DateTime.Now,
            Description = "New expense",
            Title = "Expense title"
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.CategoryRepository.FindByNameAsync(newExpense.CategoryName))
            .ReturnsAsync(new Category());

        mockUnitOfWork
            .Setup(x => x.ExpenseRepository.Add(It.IsAny<Expense>()));

        var expenseService = new ExpenseService(mockUnitOfWork.Object, mapper);
        await expenseService.AddExpenseAsync(newExpense, userId);

        mockUnitOfWork.Verify(uow =>
            uow.ExpenseRepository.Add(It.Is<Expense>(x =>
                x.UserId == userId &&
                x.CategoryName == newExpense.CategoryName &&
                x.Amount == newExpense.Amount &&
                x.Date == newExpense.Date &&
                x.Description == newExpense.Description &&
                x.Title == newExpense.Title)), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }

    [Test]
    public void AddExpenseAsyncThrowsForInvalidExpense()
    {
        var newExpense = new AddExpenseDto
        {
            Amount = 100,
            CategoryName = "Category",
            Date = DateTime.Now,
            Description = "New expense",
            Title = "Expense title"
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.CategoryRepository.FindByNameAsync(newExpense.CategoryName))
            .ReturnsAsync((Category?)null);

        var expenseService = new ExpenseService(mockUnitOfWork.Object, mapper);
        Assert.ThrowsAsync<BadRequestException>(async () => await expenseService.AddExpenseAsync(newExpense, userId));
    }

    [Test]
    public async Task UpdateExpenseAsyncUpdatesValidExpense()
    {
        var existingExpense = new Expense
        {
            Id = 1,
            UserId = "user1",
            CategoryName = "Category",
            Amount = 100,
            Description = "New expense",
            Title = "Expense title",
            Date = DateTime.Now,
        };
        var updatedExpense = new UpdateExpenseDto
        {
            Amount = 300,
            CategoryName = "Category",
            Description = "New expense",
            Title = "Expense title"
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.ExpenseRepository.FindByIdAsync(existingExpense.Id))
            .ReturnsAsync(existingExpense);

        mockUnitOfWork.Setup(x => x.ExpenseRepository.Update(It.IsAny<Expense>()));
        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindByUserIdAndCategoryName(userId, updatedExpense.CategoryName))
            .ReturnsAsync((Budget?)null);

        var expenseService = new ExpenseService(mockUnitOfWork.Object, mapper);
        await expenseService.UpdateExpenseAsync(existingExpense.Id, updatedExpense, userId);

        mockUnitOfWork.Verify(uow =>
            uow.ExpenseRepository.Update(It.Is<Expense>(x =>
                x.Id == existingExpense.Id &&
                x.UserId == userId &&
                x.CategoryName == updatedExpense.CategoryName &&
                x.Amount == updatedExpense.Amount &&
                x.Date == existingExpense.Date &&
                x.Description == updatedExpense.Description &&
                x.Title == updatedExpense.Title)), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }
    
    [Test]
    public void UpdateExpenseAsyncThrowsForNullExpense()
    {
        var updatedExpense = new UpdateExpenseDto
        {
            Amount = 300,
            CategoryName = "Category",
            Description = "New expense",
            Title = "Expense title"
        };
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.ExpenseRepository.FindByIdAsync(id))
            .ReturnsAsync((Expense?)null);

        var expenseService = new ExpenseService(mockUnitOfWork.Object, mapper);
        Assert.ThrowsAsync<NotFoundException>(async () =>
            await expenseService.UpdateExpenseAsync(id, updatedExpense, userId));
    }

    [Test]
    public void UpdateExpenseAsyncThrowsForForbiddenAction()
    {
        var existingExpense = new Expense
        {
            Id = 1,
            UserId = "anotherUserId",
            CategoryName = "Category",
            Amount = 100,
            Description = "New expense",
            Title = "Expense title",
            Date = DateTime.Now,
        };
        var updatedExpense = new UpdateExpenseDto
        {
            Amount = 300,
            CategoryName = "Category",
            Description = "New expense",
            Title = "Expense title"
        };
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.ExpenseRepository.FindByIdAsync(id))
            .ReturnsAsync(existingExpense);

        var expenseService = new ExpenseService(mockUnitOfWork.Object, mapper);
        Assert.ThrowsAsync<ForbiddenException>(async () =>
            await expenseService.UpdateExpenseAsync(id, updatedExpense, userId));
    }

    [Test]
    public async Task DeleteExpenseAsyncDeletesValidExpense()
    {
        var existingExpense = new Expense
        {
            Id = 1,
            UserId = "user1",
            CategoryName = "Category",
            Amount = 100,
            Description = "New expense",
            Title = "Expense title",
            Date = DateTime.Now,
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.ExpenseRepository.FindByIdAsync(existingExpense.Id))
            .ReturnsAsync(existingExpense);

        mockUnitOfWork.Setup(x => x.ExpenseRepository.Delete(existingExpense));

        var expenseService = new ExpenseService(mockUnitOfWork.Object, mapper);
        await expenseService.DeleteExpenseAsync(existingExpense.Id, userId);

        mockUnitOfWork.Verify(uow =>
            uow.ExpenseRepository.Delete(It.Is<Expense>(x =>
                x.Id == existingExpense.Id &&
                x.UserId == userId &&
                x.CategoryName == existingExpense.CategoryName &&
                x.Amount == existingExpense.Amount &&
                x.Date == existingExpense.Date &&
                x.Description == existingExpense.Description &&
                x.Title == existingExpense.Title)), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }


    [Test]
    public void DeleteExpenseAsyncThrowsForNullExpense()
    {
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.ExpenseRepository.FindByIdAsync(id))
            .ReturnsAsync((Expense?)null);

        var expenseService = new ExpenseService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<NotFoundException>(async () => await expenseService.DeleteExpenseAsync(id, userId));
    }

    [Test]
    public void DeleteExpenseAsyncThrowsForForbiddenAction()
    {
        var existingExpense = new Expense
        {
            Id = 1,
            UserId = "anotherUserId",
            CategoryName = "Category",
            Amount = 100,
            Description = "New expense",
            Title = "Expense title",
            Date = DateTime.Now,
        };
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.ExpenseRepository.FindByIdAsync(id))
            .ReturnsAsync(existingExpense);

        var expenseService = new ExpenseService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<ForbiddenException>(async () => await expenseService.DeleteExpenseAsync(id, userId));
    }

    [Test]
    public async Task GetUserExpensesAsyncGetsUserExpenses()
    {
        Expense[] existingExpenses =
        {
            new Expense
            {
                Id = 1,
                UserId = "userId",
                CategoryName = "Category",
                Amount = 100,
                Description = "New expense",
                Title = "Expense title",
                Date = DateTime.Now,
            },
            new Expense
            {
                Id = 2,
                UserId = "anotherUserId",
                CategoryName = "Category",
                Amount = 100,
                Description = "New expense",
                Title = "Expense title",
                Date = DateTime.Now,
            }
        };

        var expectedResult = new List<ExpenseDto>
        {
            new()
            {
                Id = 1,
                CategoryName = "Category",
                Amount = 100,
                Description = "New expense",
                Title = "Expense title",
                Date = DateTime.Now,
            }
        };

        var userId = "userId";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.ExpenseRepository.FindUserExpensesAsync(userId))
            .ReturnsAsync(new List<Expense> { existingExpenses[0] });
        
        var expenseService = new ExpenseService(mockUnitOfWork.Object, mapper);
        var result = (await expenseService.GetUserExpensesAsync(userId)).ToList();

        Assert.That(result[0].Id == expectedResult[0].Id &&
                    result[0].Title == expectedResult[0].Title &&
                    result[0].Description == expectedResult[0].Description &&
                    result[0].CategoryName == expectedResult[0].CategoryName &&
                    result[0].Amount == expectedResult[0].Amount, Is.True);
    }
}