using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Habr.BL.Services;
using Habr.BL.Services.V1;
using Habr.Common.Exceptions;
using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Habr.DataAccess.UoW;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Habr.Tests.Posts;

public class PostDeletionTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Delete_SinglePost_Success(bool isPublished)
    {
        // Arrange
        const int postId = 1;
        const int authorId = 1;

        var post = new Post()
        {
            Title = "Test post",
            Text = "Test text",
            IsPublished = isPublished,
            AuthorId = authorId
        };

        var mapperConfig = new MapperConfiguration(cfg =>
            cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        var mapper = new Mapper(mapperConfig);

        var mockPostRepository = new Mock<IPostRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockLogger = new Mock<ILogger<PostService>>();

        mockPostRepository.Setup(x => x.DeleteEntityAsync(It.IsAny<Post>()));
        mockPostRepository.Setup(x => x
                .GetEntityByIdAsync(
                    postId,
                    It.IsAny<Func<IQueryable<Post>, IIncludableQueryable<Post, object>>>(),
                    It.IsAny<bool>()).Result)
            .Returns(post);

        mockUserRepository.Setup(x => x
            .GetEntityByIdAsync(authorId, null, It.IsAny<bool>()).Result).Returns(new User());

        mockUnitOfWork
            .Setup(x => x.GetRepository<IPostRepository>())
            .Returns(mockPostRepository.Object);
        mockUnitOfWork
            .Setup(x => x.GetRepository<IUserRepository>())
            .Returns(mockUserRepository.Object);

        var postService = new PostService(mockUnitOfWork.Object, mapper, mockLogger.Object);

        // Act
        await postService.DeletePostAsync(postId, authorId);

        // Assert
    }
    
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Delete_SinglePost_ThrowsAuthorNotFoundException(bool isPublished)
    {
        // Arrange
        const int postId = 1;
        const int authorId = 1;

        var post = new Post()
        {
            Title = "Test post",
            Text = "Test text",
            IsPublished = isPublished,
            AuthorId = authorId
        };

        var mapperConfig = new MapperConfiguration(cfg =>
            cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        var mapper = new Mapper(mapperConfig);

        var mockPostRepository = new Mock<IPostRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockLogger = new Mock<ILogger<PostService>>();

        mockPostRepository.Setup(x => x.DeleteEntityAsync(It.IsAny<Post>()));
        mockPostRepository.Setup(x => x
            .GetEntityByIdAsync(postId, null, It.IsAny<bool>()).Result).Returns(post);

        mockUnitOfWork
            .Setup(x => x.GetRepository<IPostRepository>())
            .Returns(mockPostRepository.Object);
        mockUnitOfWork
            .Setup(x => x.GetRepository<IUserRepository>())
            .Returns(mockUserRepository.Object);

        var postService = new PostService(mockUnitOfWork.Object, mapper, mockLogger.Object);

        // Act
        async Task Actual() => await postService.DeletePostAsync(postId, authorId);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(Actual);
    }

    [Fact]
    public async Task Publish_SinglePost_ThrowsPostNotFoundException()
    {
        // Arrange
        const int postId = 1;
        const int authorId = 1;

        var mapperConfig = new MapperConfiguration(cfg =>
            cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        var mapper = new Mapper(mapperConfig);

        var mockPostRepository = new Mock<IPostRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockLogger = new Mock<ILogger<PostService>>();

        mockPostRepository.Setup(x => x.DeleteEntityAsync(It.IsAny<Post>()));

        mockUserRepository.Setup(x => x
                .GetEntityByIdAsync(authorId, null, It.IsAny<bool>()).Result).Returns(new User());

        mockUnitOfWork
            .Setup(x => x.GetRepository<IPostRepository>())
            .Returns(mockPostRepository.Object);
        mockUnitOfWork
            .Setup(x => x.GetRepository<IUserRepository>())
            .Returns(mockUserRepository.Object);

        var postService = new PostService(mockUnitOfWork.Object, mapper, mockLogger.Object);

        // Act
        async Task Actual() => await postService.DeletePostAsync(postId, authorId);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(Actual);
    }
    
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Publish_SinglePost_ThrowsAccessDeniedException(bool isPublished)
    {
        // Arrange
        const int postId = 1;
        const int authorId = 1;
        const int otherAuthorId = 2;

        var post = new Post()
        {
            Title = "Test post",
            Text = "Test text",
            IsPublished = isPublished,
            AuthorId = otherAuthorId
        };

        var mapperConfig = new MapperConfiguration(cfg =>
            cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        var mapper = new Mapper(mapperConfig);

        var mockPostRepository = new Mock<IPostRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockLogger = new Mock<ILogger<PostService>>();

        mockPostRepository.Setup(x => x.DeleteEntityAsync(It.IsAny<Post>()));
        mockPostRepository.Setup(x => x
            .GetEntityByIdAsync(postId, null, It.IsAny<bool>()).Result).Returns(post);

        mockUserRepository.Setup(x => x
            .GetEntityByIdAsync(authorId, null, It.IsAny<bool>()).Result).Returns(new User());

        mockUnitOfWork
            .Setup(x => x.GetRepository<IPostRepository>())
            .Returns(mockPostRepository.Object);
        mockUnitOfWork
            .Setup(x => x.GetRepository<IUserRepository>())
            .Returns(mockUserRepository.Object);

        var postService = new PostService(mockUnitOfWork.Object, mapper, mockLogger.Object);
        
        // Act
        async Task Actual() => await postService.DeletePostAsync(postId, authorId);

        // Assert
        await Assert.ThrowsAsync<AccessDeniedException>(Actual);
    }
}