using EBay.API.Infrastructure;
using EBay.Domain;
using EBay.Domain.Entities;
using EBay.Domain.Interfaces;
using EBay.Dto;
using EBay.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace EBay.Service
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _dbContext;
        public readonly IRepository<VehicleDetail> _vehicleRepo;

        public VehicleService(IApplicationDbContextFactory applicationDbContextFactory,
        IRepository<VehicleDetail> vehicleRepo)
        {
            _dbContext = applicationDbContextFactory.Get();
            _vehicleRepo = vehicleRepo;
        }

        public async Task ImportVehicles(IFormFile file)
        {
            var vehicleList = new List<VehicleDetail>();
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var vehicle = new VehicleDetail
                        {
                            Make = worksheet.Cells[row, 1].Text,
                            Model = worksheet.Cells[row, 2].Text,
                            Year = Convert.ToInt32(worksheet.Cells[row, 3].Text),
                            Trim = worksheet.Cells[row, 4].Text,
                            Engine = worksheet.Cells[row, 5].Text,
                            Status = Convert.ToInt32(worksheet.Cells[row, 6].Text),
                            VehicleDetailId = Guid.NewGuid(),
                        };
                        vehicleList.Add(vehicle);
                    }
                }
                await _dbContext.AddRangeAsync(vehicleList);
            }
        }


        public async Task<IEnumerable<string>> GetMakes()
        {
            var makes = await _vehicleRepo.GetAllQueryable().DistinctBy(s => s.Make).Select(s => s.Make).ToListAsync();
            if (makes is null)
            {
                throw new ApplicationException("Make is empty");
            }

            return makes;
        }


        public async Task<IEnumerable<string>> GetModels(string make)
        {
            var model = await _vehicleRepo.GetAllQueryable().Where(s => s.Make == make).DistinctBy(s => s.Model).Select(s => s.Model).ToListAsync();
            if (model is null)
            {
                throw new ApplicationException("Model is empty");
            }

            return model;
        }

        public async Task<IEnumerable<int>> GetYears(string make, string model)
        {
            var year = await _vehicleRepo.GetAllQueryable().Where(s => s.Make == make && s.Model == model).DistinctBy(s => s.Year).Select(s => s.Year).ToListAsync();
            if (year is null)
            {
                throw new ApplicationException("Year is empty");
            }

            return year;
        }


        public IEnumerable<VehicleDetailDto> GetvehicleDetail()
        {

            var vehicleDetailList = _vehicleRepo.GetAll();

            if (vehicleDetailList is null)
            {
                throw new ApplicationException("Year is empty");
            }

            var vehicleDetailListDto = new List<VehicleDetailDto>();

            foreach (var item in vehicleDetailList)
            {
                vehicleDetailListDto.Add(new VehicleDetailDto()
                {
                    Make = item.Make,
                    Model = item.Model,
                    Year = item.Year,
                    Trim = item.Trim,
                    Engine = item.Engine,
                    Status = item.Status,
                    VehicleDetailId = item.VehicleDetailId,
                });
            }

            return vehicleDetailListDto;
        }
    }
}
