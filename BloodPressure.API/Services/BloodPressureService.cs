using BloodPressureApp.API.Helpers;
using BloodPressureApp.API.Mappers;
using BloodPressureApp.API.Models;
using BloodPressureApp.API.Repositories;
using BloodPressureApp.Core.DTOs;

namespace BloodPressureApp.API.Services;

public class BloodPressureService
{
    private readonly BloodPressureRepository _repository;

    public BloodPressureService(BloodPressureRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BloodPressureDTO>> GetAllAsync(int user)
    {
        var entities = await _repository.GetByUserIdAsync(user);

        

        return entities.Select(entity => entity.ToDTO()).ToList();
    }

    public async Task<BloodPressureDTO?> GetByIdAsync(int id, int user)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
        {
            return null;
        }

        if (entity.UserId != user)
        {
            return null;
        }

        return entity.ToDTO();
    }

    public async Task<BloodPressureDTO> CreateAsync(BloodPressureCreateDTO dto, int user)
    {
        var entity = dto.ToEntity();
        entity.UserId = user;

        await _repository.AddAsync(entity);

        return entity.ToDTO();
    }

    public async Task<BloodPressureDTO?> UpdateAsync(int id, BloodPressureUpdateDTO dto , int user)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
        {
            return null;
        }

        if (entity.UserId != user)
        {
            return null;
        }

        dto.UpdateEntity(entity);

        await _repository.UpdateAsync(entity);

        return entity.ToDTO();
    }

    public async Task<bool> DeleteAsync(int id, int user)
    {
        var entity = await _repository.GetByIdAsync(id);
        
        if (entity == null)
        {
            return false;
        }

        if (entity.UserId != user) //los usuarios solo pueden eliminar sus propios registros
        {
            return false;
        }

        await _repository.DeleteAsync(entity);

        return true;
    }


}
