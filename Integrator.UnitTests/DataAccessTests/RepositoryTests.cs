using Integrator.DataAccess.DbContexts;
using Integrator.DataAccess.Models.Complaints;
using Integrator.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Integrator.UnitTests.DataAccessTests;

public class RepositoryTests
{
    private DbContextOptions<IntegratorDbContext> Options
    {
        get
        {
            var optionsBuilder = new DbContextOptionsBuilder<IntegratorDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            return optionsBuilder.Options;
        }
    }
    
    [Fact]
    public async Task GetAll_ReturnsAllModels()
    {
        // Arrange
        await using var context = new IntegratorDbContext(Options);
        
        var data = new List<Complaint>()
        {
            new() {Id = "1", Description = new ComplaintResponse(){ Content = "a" }, Number = "b" }, new() {Id = "2", Description = new ComplaintResponse(){ Content = "c" }, Number = "d" }
        };

        await context.Complaints.AddRangeAsync(data);
        await context.SaveChangesAsync();
        
        Repository<Complaint> repository = new Repository<Complaint>(context);

        // Act
        var result = await repository.GetAll();

        // Assert
        Assert.Equal(data.Count, result.Count);
    }
    
    [Fact]
    public async Task GetById_ReturnsModel_WhenIdExists()
    {
        // Arrange
        await using var context = new IntegratorDbContext(Options);
        
        var data = new List<Complaint>()
        {
            new() {Id = "1", Description = new ComplaintResponse(){ Content = "a" }, Number = "b" }, new() {Id = "2", Description = new ComplaintResponse(){ Content = "c" }, Number = "d" }
        };

        await context.Complaints.AddRangeAsync(data);
        await context.SaveChangesAsync();
        
        Repository<Complaint> repository = new Repository<Complaint>(context);

        // Act
        var result = await repository.GetById("1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
    }
    
    [Fact]
    public async Task GetById_ReturnsNull_WhenIdDoesNotExists()
    {
        // Arrange
        await using var context = new IntegratorDbContext(Options);
        
        Repository<Complaint> repository = new Repository<Complaint>(context);

        // Act
        var result = await repository.GetById("1");

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task Add_AddsModel()
    {
        // Arrange
        await using var context = new IntegratorDbContext(Options);

        var data = new Complaint()
        {
            Id = "1",
            Description = new ComplaintResponse() { Content = "a" },
            Number = "b",
        };
        
        Repository<Complaint> repository = new Repository<Complaint>(context);

        // Act
        await repository.Add(data);

        // Assert
        Assert.NotNull(await context.Complaints.FindAsync("1"));
    }
    
    [Fact]
    public void Remove_RemovesModel_WhenIdExists()
    {
        // Arrange
        using var context = new IntegratorDbContext(Options);

        var data = new Complaint()
        {
            Id = "1",
            Description = new ComplaintResponse() { Content = "a" },
            Number = "b"
        };

        context.Complaints.Add(data);
        context.SaveChanges();
        
        Repository<Complaint> repository = new Repository<Complaint>(context);

        // Act
        repository.Remove("1");
        context.SaveChanges();

        // Assert
        Assert.Null(context.Complaints.Find("1"));
    }
    
    [Fact]
    public void Remove_DoesNotRemoveModel_WhenIdNotExists()
    {
        // Arrange
        using var context = new IntegratorDbContext(Options);
        
        Repository<Complaint> repository = new Repository<Complaint>(context);

        // Act
        repository.Remove("1");

        // Assert
        Assert.Null(context.Complaints.Find("1"));
    }
}