using System;
using System.Threading.Tasks;
using AutoMapper;
using Habr.BL.Services;
using Habr.BL.Services.V1;
using Habr.Common.DTOs.V1.Posts;
using Habr.Common.Exceptions;
using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Habr.DataAccess.UoW;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Habr.Tests.Posts;

public class PostCreationTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Add_SinglePost_Success(bool isPublished)
    {
        // Arrange
        const int authorId = 1;

        var addPostRequest = new AddPostRequest()
        {
            Title = "Test post",
            Text = "Test text",
            IsPublished = isPublished
        };
        
        var mapperConfig = new MapperConfiguration(cfg =>
            cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        var mapper = new Mapper(mapperConfig);

        var mockPostRepository = new Mock<IPostRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockLogger = new Mock<ILogger<PostService>>();

        mockPostRepository.Setup(x => x.AddEntityAsync(It.IsAny<Post>()));
        mockUserRepository.Setup(x => x
            .GetEntityByIdAsync(authorId, null, It.IsAny<bool>()).Result)
            .Returns(new User());

        mockUnitOfWork
            .Setup(x => x.GetRepository<IPostRepository>())
            .Returns(mockPostRepository.Object);
        mockUnitOfWork
            .Setup(x => x.GetRepository<IUserRepository>())
            .Returns(mockUserRepository.Object);

        var postService = new PostService(mockUnitOfWork.Object, mapper, mockLogger.Object);

        // Act
        var response = await postService.AddPostAsync(addPostRequest, authorId);

        // Assert
        Assert.NotNull(response);
    }
    
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Add_SinglePost_ThrowsAuthorNotFoundException(bool isPublished)
    {
        // Arrange
        const int authorId = 1;

        var addPostRequest = new AddPostRequest()
        {
            Title = "Test post",
            Text = "Test text",
            IsPublished = isPublished
        };
        
        var mapperConfig = new MapperConfiguration(cfg =>
            cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        var mapper = new Mapper(mapperConfig);

        var mockPostRepository = new Mock<IPostRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockLogger = new Mock<ILogger<PostService>>();

        mockPostRepository.Setup(x => x.AddEntityAsync(It.IsAny<Post>()));

        mockUnitOfWork
            .Setup(x => x.GetRepository<IPostRepository>())
            .Returns(mockPostRepository.Object);
        mockUnitOfWork
            .Setup(x => x.GetRepository<IUserRepository>())
            .Returns(mockUserRepository.Object);

        var postService = new PostService(mockUnitOfWork.Object, mapper, mockLogger.Object);

        // Act
        async Task Actual() => await postService.AddPostAsync(addPostRequest, authorId);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(Actual);
    }
}