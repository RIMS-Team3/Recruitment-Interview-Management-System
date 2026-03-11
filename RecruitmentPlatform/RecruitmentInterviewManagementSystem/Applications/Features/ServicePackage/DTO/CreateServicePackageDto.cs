namespace RecruitmentInterviewManagementSystem.Applications.Features.ServicePackage.DTO
{
    public class CreateServicePackageDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? DurationDays { get; set; }
        public int? MaxPost { get; set; }
        public bool? IsActive { get; set; }
    }
}