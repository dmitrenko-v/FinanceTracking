using Application.Dto.Budget;
using Application.Dto.Expense;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace Tests.Services;

[TestFixture]
public class BudgetServiceTests
{
    [Test]
    public async Task AddBudgetAsyncAddsValidBudget()
    {
        var newBudget = new AddBudgetDto
        {
            CeilingAmount = 1000,
            CategoryName = "Category",
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.CategoryRepository.FindByNameAsync(newBudget.CategoryName))
            .ReturnsAsync(new Category());

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindByUserIdAndCategoryName(userId, newBudget.CategoryName))
            .ReturnsAsync((Budget?)null);

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);
        await budgetService.AddBudgetAsync(newBudget, userId);

        mockUnitOfWork.Verify(uow =>
            uow.BudgetRepository.Add(It.Is<Budget>(x =>
                x.UserId == userId &&
                x.CategoryName == newBudget.CategoryName &&
                x.CeilingAmount == newBudget.CeilingAmount &&
                x.CurrentAmount == 0
            )), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }

    [Test]
    public void AddBudgetAsyncThrowsForInvalidBudget()
    {
        var newBudget = new AddBudgetDto
        {
            CeilingAmount = 1000,
            CategoryName = "Category",
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.CategoryRepository.FindByNameAsync(newBudget.CategoryName))
            .ReturnsAsync((Category?)null);

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);
        Assert.ThrowsAsync<BadRequestException>(async () => await budgetService.AddBudgetAsync(newBudget, userId));
    }

    [Test]
    public void AddBudgetAsyncThrowsForExistingBudget()
    {
        var newBudget = new AddBudgetDto
        {
            CeilingAmount = 1000,
            CategoryName = "Category",
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.CategoryRepository.FindByNameAsync(newBudget.CategoryName))
            .ReturnsAsync(new Category());

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindByUserIdAndCategoryName(userId, newBudget.CategoryName))
            .ReturnsAsync(new Budget());

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);
        Assert.ThrowsAsync<BadRequestException>(async () => await budgetService.AddBudgetAsync(newBudget, userId));
    }

    [Test]
    public async Task UpdateBudgetAsyncUpdatesValidBudget()
    {
        var existingBudget = new Budget
        {
            Id = 1,
            CeilingAmount = 1000,
            CategoryName = "Category",
            UserId = "user1"
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindByIdAsync(existingBudget.Id))
            .ReturnsAsync(existingBudget);

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.Update(It.IsAny<Budget>()));

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);
        await budgetService.UpdateBudgetAsync(existingBudget.Id, 2000, userId);

        mockUnitOfWork.Verify(uow =>
            uow.BudgetRepository.Update(It.Is<Budget>(x =>
                x.Id == existingBudget.Id &&
                x.UserId == userId &&
                x.CategoryName == existingBudget.CategoryName &&
                x.CeilingAmount == 2000)), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }

    [Test]
    public void UpdateBudgetAsyncThrowsForNullBudget()
    {
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindByIdAsync(id))
            .ReturnsAsync((Budget?)null);

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);
        Assert.ThrowsAsync<NotFoundException>(async () => await budgetService.UpdateBudgetAsync(id, 100, userId));
    }

    [Test]
    public void UpdateBudgetAsyncThrowsForForbiddenAction()
    {
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindByIdAsync(id))
            .ReturnsAsync(new Budget { UserId = "another" });

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);
        Assert.ThrowsAsync<ForbiddenException>(async () => await budgetService.UpdateBudgetAsync(id, 100, userId));
    }

    [Test]
    public async Task DeleteBudgetAsyncDeletesValidBudget()
    {
        var existingBudget = new Budget
        {
            Id = 1,
            CeilingAmount = 1000,
            CategoryName = "Category",
            UserId = "user1"
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindByIdAsync(existingBudget.Id))
            .ReturnsAsync(existingBudget);

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.Delete(existingBudget));

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);
        await budgetService.DeleteBudgetAsync(existingBudget.Id, userId);

        mockUnitOfWork.Verify(uow =>
            uow.BudgetRepository.Delete(It.Is<Budget>(x =>
                x.Id == existingBudget.Id &&
                x.UserId == userId &&
                x.CategoryName == existingBudget.CategoryName &&
                x.CeilingAmount == existingBudget.CeilingAmount)), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }


    [Test]
    public void DeleteBudgetAsyncThrowsForNullBudget()
    {
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindByIdAsync(id))
            .ReturnsAsync((Budget?)null);

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<NotFoundException>(async () => await budgetService.DeleteBudgetAsync(id, userId));
    }

    [Test]
    public void DeleteBudgetAsyncThrowsForForbiddenAction()
    {
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindByIdAsync(id))
            .ReturnsAsync(new Budget { UserId = "another" });

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<ForbiddenException>(async () => await budgetService.DeleteBudgetAsync(id, userId));
    }

    [Test]
    public async Task GetUserBudgetsAsyncGetsUserBudgets()
    {
        var existingBudgets = new List<Budget>
        {
            new Budget
            {
                Id = 1,
                CeilingAmount = 1000,
                CurrentAmount = 100,
                CategoryName = "Category",
                UserId = "user1"
            },
            new Budget
            {
                Id = 2,
                CeilingAmount = 1000,
                CurrentAmount = 200,
                CategoryName = "Category",
                UserId = "user2"
            }
        };

        var expectedBudget = new BudgetDto
        {
            Id = 1,
            CeilingAmount = 1000,
            CurrentAmount = 100,
            CategoryName = "Category",
        };

        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.FindUserBudgetsAsync(userId))
            .ReturnsAsync(new List<Budget> { existingBudgets[0] });

        var budgetService = new BudgetService(mockUnitOfWork.Object, mapper);
        var result = (await budgetService.GetUserBudgetsAsync(userId)).ToList()[0];

        Assert.That(result.Id == expectedBudget.Id &&
                    result.CategoryName == expectedBudget.CategoryName &&
                    result.CeilingAmount == expectedBudget.CeilingAmount &&
                    result.CurrentAmount == expectedBudget.CurrentAmount, Is.True);
    }
}