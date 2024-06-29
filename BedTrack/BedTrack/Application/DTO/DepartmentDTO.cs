using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static DepartmentDTO FromModel(Department model)
        {
            return new DepartmentDTO
            {
                Id = model.Id,
                Name = model.Name,
            };
        }
    }
}
