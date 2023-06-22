using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services
{
    public class DietService : IDietService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;
        public DietService(IRepository repository)
        {
            _repository = repository;
            _mapper = new EntityModelMapper();
        }


        public int AddAnthropometricInfo(AnthropometricInfoModel infoModel)
        {
            var infoEntityBase = _mapper.MapAnthropometricInfoModelToEntityBase(infoModel);
            int infoId = _repository.AddAnthropometricInfo(infoEntityBase);
            return infoId;
        }
    }
}
