using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities.JoinEntityBase;
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


        public async void SaveImageFileAsync(IFormFile uploadedFile, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
        }


        public void AddVisitingChartDataForGym(IEnumerable<VisitingChartModel> data)
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
