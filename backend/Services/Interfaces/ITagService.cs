using backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services.Interfaces
{
    public interface ITagService
    {
        Task<TagDto> CreateAsync(TagDto dto);
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<TagDto?> GetByIdAsync(int id);
        Task UpdateAsync(int id, TagDto dto);
        Task DeleteAsync(int id);
    }
}
