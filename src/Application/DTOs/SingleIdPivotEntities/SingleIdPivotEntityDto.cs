namespace Application.DTOs.SingleIdPivotEntities
{
    public class SingleIdPivotEntityDto
    {
        public Guid Id { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}