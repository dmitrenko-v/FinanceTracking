using Application.Dto.Goal;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace Tests.Services;

[TestFixture]
public class GoalServiceTests
{
    [Test]
    public async Task AddGoalAsyncAddsValidGoal()
    {
        var newGoal = new AddGoalDto
        {
            Title = "New car",
            Description = "New car description",
            GoalAmount = 10000,
            CurrentAmount = 1000,
            StoredIn = "Cash",
            Deadline = DateTime.Now + TimeSpan.FromDays(2)
        };
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork.Setup(u => u.GoalRepository.Add(It.IsAny<Goal>()));

        var goalService = new GoalService(mockUnitOfWork.Object, mapper);
        await goalService.AddGoalAsync(newGoal, userId);

        mockUnitOfWork.Verify(uow =>
            uow.GoalRepository.Add(It.Is<Goal>(x =>
                x.UserId == userId &&
                x.CurrentAmount == newGoal.CurrentAmount &&
                x.GoalAmount == newGoal.GoalAmount &&
                x.Title == newGoal.Title &&
                x.Deadline == newGoal.Deadline &&
                x.Description == newGoal.Description
            )), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateGoalAsyncUpdatesValidGoal()
    {
        var existingGoal = new Goal
        {
            Id = 1,
            Title = "New car",
            Description = "New car description",
            GoalAmount = 10000,
            CurrentAmount = 1000,
            StoredIn = "Cash",
            UserId = "user1",
            Deadline = DateTime.Now + TimeSpan.FromDays(2)
        };

        var updatedGoal = new UpdateGoalDto
        {
            Title = "New car",
            Description = "New car description",
            GoalAmount = 10000,
            CurrentAmount = 3000,
            StoredIn = "Cash",
            Deadline = DateTime.Now + TimeSpan.FromDays(2)
        };

        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.GoalRepository.FindByIdAsync(existingGoal.Id))
            .ReturnsAsync(existingGoal);

        mockUnitOfWork
            .Setup(x => x.BudgetRepository.Update(It.IsAny<Budget>()));

        var goalService = new GoalService(mockUnitOfWork.Object, mapper);
        await goalService.UpdateGoalAsync(existingGoal.Id, updatedGoal, userId);

        mockUnitOfWork.Verify(uow =>
            uow.GoalRepository.Update(It.Is<Goal>(x =>
                x.Id == existingGoal.Id &&
                x.UserId == userId &&
                x.CurrentAmount == updatedGoal.CurrentAmount &&
                x.GoalAmount == updatedGoal.GoalAmount &&
                x.Title == updatedGoal.Title &&
                x.Deadline == updatedGoal.Deadline &&
                x.Description == updatedGoal.Description
            )), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }

    [Test]
    public void UpdateGoalAsyncThrowsForNullGoal()
    {
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.GoalRepository.FindByIdAsync(id))
            .ReturnsAsync((Goal?)null);

        var goalService = new GoalService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<NotFoundException>(() => goalService.UpdateGoalAsync(id, It.IsAny<UpdateGoalDto>(), userId));
    }

    [Test]
    public void UpdateGoalAsyncThrowsForForbiddenAction()
    {
        var existingGoal = new Goal
        {
            Id = 1,
            Title = "New car",
            Description = "New car description",
            GoalAmount = 10000,
            CurrentAmount = 1000,
            StoredIn = "Cash",
            UserId = "anotherUserId",
            Deadline = DateTime.Now + TimeSpan.FromDays(2)
        };

        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.GoalRepository.FindByIdAsync(id))
            .ReturnsAsync(existingGoal);

        var goalService = new GoalService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<ForbiddenException>(() =>
            goalService.UpdateGoalAsync(id, It.IsAny<UpdateGoalDto>(), userId));
    }

    [Test]
    public async Task DeleteGoalAsyncDeletesExistingGoal()
    {
        var existingGoal = new Goal
        {
            Id = 1,
            Title = "New car",
            Description = "New car description",
            GoalAmount = 10000,
            CurrentAmount = 1000,
            StoredIn = "Cash",
            UserId = "user1",
            Deadline = DateTime.Now + TimeSpan.FromDays(2)
        };

        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.GoalRepository.FindByIdAsync(existingGoal.Id))
            .ReturnsAsync(existingGoal);

        mockUnitOfWork
            .Setup(x => x.GoalRepository.Delete(existingGoal));

        var goalService = new GoalService(mockUnitOfWork.Object, mapper);
        await goalService.DeleteGoalAsync(existingGoal.Id, userId);

        mockUnitOfWork.Verify(uow =>
            uow.GoalRepository.Delete(It.Is<Goal>(x =>
                x.Id == existingGoal.Id &&
                x.UserId == userId &&
                x.CurrentAmount == existingGoal.CurrentAmount &&
                x.GoalAmount == existingGoal.GoalAmount &&
                x.Title == existingGoal.Title &&
                x.Deadline == existingGoal.Deadline &&
                x.Description == existingGoal.Description
            )), Times.Once);
        mockUnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }

    [Test]
    public void DeleteGoalAsyncThrowsForNullGoal()
    {
        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.GoalRepository.FindByIdAsync(id))
            .ReturnsAsync((Goal?)null);

        var goalService = new GoalService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<NotFoundException>(() => goalService.DeleteGoalAsync(id, userId));
    }

    [Test]
    public void DeleteGoalAsyncThrowsForForbiddenAction()
    {
        var existingGoal = new Goal
        {
            Id = 1,
            Title = "New car",
            Description = "New car description",
            GoalAmount = 10000,
            CurrentAmount = 1000,
            StoredIn = "Cash",
            UserId = "anotherUserId",
            Deadline = DateTime.Now + TimeSpan.FromDays(2)
        };

        var id = 1;
        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.GoalRepository.FindByIdAsync(id))
            .ReturnsAsync(existingGoal);

        var goalService = new GoalService(mockUnitOfWork.Object, mapper);

        Assert.ThrowsAsync<ForbiddenException>(() => goalService.DeleteGoalAsync(id, userId));
    }

    [Test]
    public async Task GetUserGoalsAsyncGetsBudgetsExpenses()
    {
        var existingGoals = new List<Goal>
        {
            new Goal
            {
                Id = 1,
                Title = "New car",
                Description = "New car description",
                GoalAmount = 10000,
                CurrentAmount = 1000,
                StoredIn = "Cash",
                UserId = "user1",
                Deadline = DateTime.Now + TimeSpan.FromDays(2)
            },
            new Goal
            {
                Id = 2,
                Title = "New car",
                Description = "New car description",
                GoalAmount = 10000,
                CurrentAmount = 1000,
                StoredIn = "Cash",
                UserId = "anotherUserId",
                Deadline = DateTime.Now + TimeSpan.FromDays(2)
            },
        };

        var expectedBudget = new GoalDto
        {
            Id = 1,
            Title = "New car",
            Description = "New car description",
            GoalAmount = 10000,
            CurrentAmount = 1000,
            StoredIn = "Cash",
            Deadline = DateTime.Now + TimeSpan.FromDays(2)
        };

        var userId = "user1";
        var mapper = MapperHelper.CreateMapper();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork
            .Setup(x => x.GoalRepository.FindUserGoalsAsync(userId))
            .ReturnsAsync(new List<Goal> { existingGoals[0] });

        var goalService = new GoalService(mockUnitOfWork.Object, mapper);
        var result = (await goalService.GetUserGoalsAsync(userId)).ToList()[0];

        Assert.That(result.Id == expectedBudget.Id &&
                    result.Title == expectedBudget.Title &&
                    result.Description == expectedBudget.Description &&
                    result.GoalAmount == expectedBudget.GoalAmount &&
                    result.StoredIn == expectedBudget.StoredIn &&
                    result.CurrentAmount == expectedBudget.CurrentAmount, Is.True);
    }
}