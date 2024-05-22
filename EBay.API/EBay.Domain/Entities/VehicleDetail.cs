using EBay.Domain.Entities.Base;

namespace EBay.Domain.Entities
{
    public class VehicleDetail : BaseEntity
    {
        public Guid VehicleDetailId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Status { get; set; }
        public string Trim { get; set; }
        public string Engine { get; set; }
        public string? Notes { get; set; }
    }
}
