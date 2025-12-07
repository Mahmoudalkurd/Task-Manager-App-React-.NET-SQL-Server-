using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using backend.Domain.Entities;
using backend.DTOs;
using backend.Services;
using backend.Services.Interfaces;
using backend.Infrastructure.Repositories;

namespace backend.Tests
{
    public class TagServiceTests
    {
        private readonly TagService _tagService;
        private readonly Mock<ITagRepository> _mockRepo;
        private readonly IMapper _mapper;

        public TagServiceTests()
        {
            // CORRECT: Initialize configuration using a lambda expression
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, TagDto>().ReverseMap();
            });

            _mapper = config.CreateMapper();

            _mockRepo = new Mock<ITagRepository>();
            _tagService = new TagService(_mockRepo.Object, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedTagDto()
        {
            // Arrange
            var dto = new TagDto(0, "TestTag");
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Tag>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _tagService.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Tag>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTags()
        {
            // Arrange
            var tags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Tag1" },
                new Tag { Id = 2, Name = "Tag2" }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tags);

            // Act
            var result = await _tagService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTagDto()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Tag1" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tag);

            // Act
            var result = await _tagService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tag.Name, result.Name);
            _mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRemoveAndSaveChanges()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Tag1" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tag);
            _mockRepo.Setup(r => r.Remove(tag));
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _tagService.DeleteAsync(1);

            // Assert
            _mockRepo.Verify(r => r.Remove(tag), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTag()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "OldName" };
            var dto = new TagDto(1, "NewName");
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tag);
            _mockRepo.Setup(r => r.Update(tag));
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _tagService.UpdateAsync(1, dto);

            // Assert
            Assert.Equal("NewName", tag.Name);
            _mockRepo.Verify(r => r.Update(tag), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistent_ShouldReturnNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Tag?)null);

            // Act
            var result = await _tagService.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }
    }
}