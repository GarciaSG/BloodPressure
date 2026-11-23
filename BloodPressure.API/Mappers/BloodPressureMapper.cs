using BloodPressureApp.API.Helpers;
using BloodPressureApp.API.Models;
using BloodPressureApp.Core.DTOs;

namespace BloodPressureApp.API.Mappers;

public static class BloodPressureMapper
{
    public static BloodPressureDTO ToDTO(this BloodPressure entity)
    {
        return new BloodPressureDTO
        {
            Id = entity.Id,
            Systolic = entity.Systolic,
            Diastolic = entity.Diastolic,
            RecordedAt = entity.RecordedAt,
            Notes = entity.Notes,
            Classification = BloodPressureHelper.GetClassification(
            entity.Systolic,
            entity.Diastolic)
        };
    }

    public static BloodPressure ToEntity(this BloodPressureCreateDTO dto)
    {
        return new BloodPressure
        {
            Systolic = dto.Systolic!.Value,
            Diastolic = dto.Diastolic!.Value,
            RecordedAt = dto.RecordedAt!.Value,
            Notes = dto.Notes?.Trim()
        };
    }

    public static void UpdateEntity(this BloodPressureUpdateDTO dto, BloodPressure entity)
    {
        entity.Systolic = dto.Systolic!.Value;
        entity.Diastolic = dto.Diastolic!.Value;
        entity.RecordedAt = dto.RecordedAt!.Value;
        entity.Notes = dto.Notes?.Trim();
    }

}
