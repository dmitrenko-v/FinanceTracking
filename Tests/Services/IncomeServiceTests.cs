using Application.Dto.Income;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace Tests.Services;

[TestFixture]
public class IncomeServiceTests
{
    [Test]
    public async Task AddIncomeAsyncAddsValidIncome()
    {
        var newIncome = new AddIncomeDto
        {
            Title = "Income",
            Amount = 1000,
            Description = "description",
            Date = new DateTime(2020, 01, 01)
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.Add(It.IsAny<Income>()));

        var incomeService = new IncomeService(mockUnitOfWork.Object, mapper);
        await incomeService.AddIncomeAsync(newIncome, userId);

        mockUnitOfWork.Verify(uow =>
            uow.IncomeRepository.Add(It.Is<Income>(x =>
                x.UserId == userId &&
                x.Title == newIncome.Title &&
                x.Amount == newIncome.Amount &&
                x.Description == newIncome.Description &&
                x.Date == newIncome.Date
            )), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateIncomeAsyncUpdatesValidIncome()
    {
        var existingIncome = new Income
        {
            Id = 1,
            UserId = "user1",
            Title = "Income",
            Amount = 1000,
            Description = "description",
            Date = new DateTime(2020, 01, 01)
        };

        var updatedIncome = new UpdateIncomeDto
        {
            Title = "Income",
            Amount = 2000,
            Description = "description",
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.FindByIdAsync(existingIncome.Id)).ReturnsAsync(existingIncome);

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.Update(It.IsAny<Income>()));

        var incomeService = new IncomeService(mockUnitOfWork.Object, mapper);
        await incomeService.UpdateIncomeAsync(existingIncome.Id, updatedIncome, userId);

        mockUnitOfWork.Verify(uow =>
            uow.IncomeRepository.Update(It.Is<Income>(x =>
                x.Id == existingIncome.Id &&
                x.UserId == userId &&
                x.Title == updatedIncome.Title &&
                x.Amount == updatedIncome.Amount &&
                x.Description == updatedIncome.Description &&
                x.Date == existingIncome.Date
            )), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }

    [Test]
    public void UpdateIncomeAsyncThrowsForNullIncome()
    {
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.FindByIdAsync(id)).ReturnsAsync((Income?)null);

        var incomeService = new IncomeService(mockUnitOfWork.Object, mapper);
        Assert.ThrowsAsync<NotFoundException>(() =>
            incomeService.UpdateIncomeAsync(id, It.IsAny<UpdateIncomeDto>(), userId));
    }

    [Test]
    public void UpdateIncomeAsyncThrowsForForbiddenAction()
    {
        var existingIncome = new Income
        {
            Id = 1,
            UserId = "anotherUserId",
            Title = "Income",
            Amount = 1000,
            Description = "description",
            Date = new DateTime(2020, 01, 01)
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.FindByIdAsync(existingIncome.Id)).ReturnsAsync(existingIncome);


        var incomeService = new IncomeService(mockUnitOfWork.Object, mapper);
        Assert.ThrowsAsync<ForbiddenException>(async () =>
            await incomeService.UpdateIncomeAsync(existingIncome.Id, It.IsAny<UpdateIncomeDto>(), userId));
    }
    
    [Test]
    public async Task DeleteIncomeAsyncDeletesExistingIncome()
    {
        var existingIncome = new Income
        {
            Id = 1,
            UserId = "user1",
            Title = "Income",
            Amount = 1000,
            Description = "description",
            Date = new DateTime(2020, 01, 01)
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.FindByIdAsync(existingIncome.Id)).ReturnsAsync(existingIncome);

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.Delete(existingIncome));

        var incomeService = new IncomeService(mockUnitOfWork.Object, mapper);
        await incomeService.DeleteIncomeAsync(existingIncome.Id, userId);

        mockUnitOfWork.Verify(uow =>
            uow.IncomeRepository.Delete(It.Is<Income>(x =>
                x.Id == existingIncome.Id &&
                x.UserId == userId &&
                x.Title == existingIncome.Title &&
                x.Amount == existingIncome.Amount &&
                x.Description == existingIncome.Description &&
                x.Date == existingIncome.Date
            )), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }

    [Test]
    public void DeleteIncomeAsyncThrowsForNullIncome()
    {
        var existingIncome = new Income
        {
            Id = 1,
            UserId = "user1",
            Title = "Income",
            Amount = 1000,
            Description = "description",
            Date = new DateTime(2020, 01, 01)
        };
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.FindByIdAsync(id)).ReturnsAsync((Income?)null);
        
        var incomeService = new IncomeService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<NotFoundException>(async () => await incomeService.DeleteIncomeAsync(id, userId));
    }
    
    [Test]
    public void DeleteIncomeAsyncThrowsForForbiddenAction()
    {
        var existingIncome = new Income
        {
            Id = 1,
            UserId = "anotherUserId",
            Title = "Income",
            Amount = 1000,
            Description = "description",
            Date = new DateTime(2020, 01, 01)
        };
        
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.FindByIdAsync(existingIncome.Id)).ReturnsAsync(existingIncome);
        
        var incomeService = new IncomeService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<ForbiddenException>(async () => await incomeService.DeleteIncomeAsync(existingIncome.Id, userId));
    }
    
    [Test]
    public async Task GetUserIncomesAsyncReturnsUserIncomes()
    {
        var existingIncomes = new List<Income>
        {
            new Income
            {
                Id = 1,
                UserId = "user1",
                Title = "Income",
                Amount = 1000,
                Description = "description",
                Date = new DateTime(2020, 01, 01)
            },
            new Income
            {
                Id = 2,
                UserId = "anotherUserId",
                Title = "Income",
                Amount = 1000,
                Description = "description",
                Date = new DateTime(2020, 01, 01)
            },
        };

        var expectedIncome = new IncomeDto
        {
            Id = 1,
            Title = "Income",
            Amount = 1000,
            Description = "description",
            Date = new DateTime(2020, 01, 01)
        };

        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.IncomeRepository.FindUserIncomesAsync(userId))
            .ReturnsAsync(new List<Income> { existingIncomes[0] });

        var incomeService = new IncomeService(mockUnitOfWork.Object, mapper);
        var result = (await incomeService.GetUserIncomesAsync(userId)).ToList()[0];

        Assert.That(result.Id == expectedIncome.Id &&
                    result.Title == expectedIncome.Title &&
                    result.Description == expectedIncome.Description &&
                    result.Amount == expectedIncome.Amount &&
                    result.Date == expectedIncome.Date, Is.True);
    }
}