using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models.Chart;
using Microsoft.AspNetCore.Http;

namespace FitMeApp.Services
{
    public class FileStorage: IFileStorage
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;

        public FileStorage(IRepository repository)
        {
            _repository = repository;
            _mapper = new EntityModelMapper();
        }


        public async Task SaveFileAsync(IFormFile uploadedFile, string fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath))
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
        }


        public void AddVisitingChartDataToDb(IEnumerable<AttendanceChartModel> data)
        {
            var entities = new List<NumberOfVisitorsPerHourEntityBase>();
            foreach (var dayData in data)
            {
                entities.AddRange(_mapper.MapVisitingChartModelToNumberOfVisitorsPerHourEntityBase(dayData));
            }
           
            _repository.DeleteNumberOfVisitorsPerHourChartData(entities.Select(x=>x.GymId).First());
            _repository.AddNumberOfVisitorsPerHourChartData(entities);
        }
    }
}
