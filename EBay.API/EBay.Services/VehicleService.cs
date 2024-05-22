using EBay.API.Domain.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;

        public VehicleService(IApplicationDbContextFactory applicationDbContextFactory,
        IRepository<VehicleDetail> vehicleRepo, IUnitOfWork unitOfWork)
        {
            _dbContext = applicationDbContextFactory.Get();
            _vehicleRepo = vehicleRepo;
            _unitOfWork = unitOfWork;
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
                await _vehicleRepo.InsertBulkAsync(vehicleList);
                await _unitOfWork.CommitAsync();
            }
        }


        public async Task<IEnumerable<string>> GetMakes()
        {
            var makes = await _vehicleRepo.GetAllQueryable()
                                           .GroupBy(s => s.Make)
                                           .Select(g => g.Key)
                                           .ToListAsync();
            if (makes is null)
            {
                throw new ApplicationException("Make is empty");
            }

            return makes;
        }


        public async Task<IEnumerable<string>> GetModels(string make)
        {
            var models = await _vehicleRepo.GetAllQueryable()
                        .Where(s => s.Make == make)
                        .GroupBy(s => new { s.Make, s.Model })
                        .Select(g => g.Key.Model)
                        .ToListAsync();

            if (models is null)
            {
                throw new ApplicationException("Model is empty");
            }

            return models;
        }

        public async Task<IEnumerable<int>> GetYears(string make, string model)
        {
            var years = await _vehicleRepo.GetAllQueryable()
                        .Where(s => s.Make == make && s.Model == model)
                        .GroupBy(s => new { s.Make, s.Model, s.Year })
                        .Select(g => g.Key.Year)
                        .ToListAsync();
            if (years is null)
            {
                throw new ApplicationException("Year is empty");
            }

            return years;
        }


        public IEnumerable<VehicleDetailDto> GetAllVehicleDetail()
        {

            var res = _vehicleRepo.GetAll();

            return GetVehicleDetailListDto(res);

        }

        public async Task<IEnumerable<VehicleDetailDto>> GetSearchResults(string query)
        {
            var res = await _dbContext.VehicleDetails.Where(s => s.Notes.Contains(query) || s.Make.Contains(query) || s.Year.ToString().Contains(query) || s.Model.Contains(query) || s.Trim.Contains(query)).ToListAsync();
            return GetVehicleDetailListDto(res);
        }

        private IEnumerable<VehicleDetailDto> GetVehicleDetailListDto(IEnumerable<VehicleDetail> vehicleDetails)
        {
            if (vehicleDetails is null)
            {
                return Enumerable.Empty<VehicleDetailDto>();
            }
            var vehicleDetailListDto = new List<VehicleDetailDto>();
            foreach (var item in vehicleDetails)
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

        public async Task<IEnumerable<VehicleDetailDto>> GetVehicleDetailList(VehicleDataListRequestDto vehicleDataListRequestDto)
        {
            var res = await _dbContext.VehicleDetails.Where(s => s.Make == vehicleDataListRequestDto.Make && s.Model == vehicleDataListRequestDto.Model && s.Year == vehicleDataListRequestDto.Year).ToListAsync();
            return GetVehicleDetailListDto(res);
        }
    }
}
