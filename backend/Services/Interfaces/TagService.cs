using AutoMapper;
using backend.DTOs;
using backend.Domain.Entities;
using backend.Infrastructure.Repositories;
using backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repo;
        private readonly IMapper _mapper;

        public TagService(ITagRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TagDto> CreateAsync(TagDto dto)
        {
            var tag = new Tag { Name = dto.Name };
            await _repo.AddAsync(tag);
            await _repo.SaveChangesAsync();
            return _mapper.Map<TagDto>(tag);
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _repo.GetByIdAsync(id);
            if (tag == null) throw new KeyNotFoundException("Tag not found");
            _repo.Remove(tag);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            var tags = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<TagDto?> GetByIdAsync(int id)
        {
            var tag = await _repo.GetByIdAsync(id);
            return tag == null ? null : _mapper.Map<TagDto>(tag);
        }

        public async Task UpdateAsync(int id, TagDto dto)
        {
            var tag = await _repo.GetByIdAsync(id);
            if (tag == null) throw new KeyNotFoundException("Tag not found");

            tag.Name = dto.Name;
            _repo.Update(tag);
            await _repo.SaveChangesAsync();
        }
    }
}
