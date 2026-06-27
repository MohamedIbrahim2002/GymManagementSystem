using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Trainer;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Data.Models.Enums;
using GymManagment.DAL.Repositories.Interfaces;
namespace GymManagment.BLL.Services.Classes
{
    public class TrainerService : ITrainerServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExist = await  _unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Email == model.Email);
            var phoneExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Phone == model.Phone);
            if (emailExist || phoneExist) return false;
            // add trainer to database if not exist && casting from viewmodel to entity
            var trainer = new Trainer()
            {
                Id = 0,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
            };
               _unitOfWork.GetRepository<Trainer>().Add(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {

            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            if(trainers == null) return null;

            return trainers.Select(t => new TrainerViewModel()
            {
                id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Specialization = Enum.Parse<Specialty>(t.specialty.ToString()).ToString(),
                phone = t.Phone,
                address = $"{t.Address.Street} {t.Address.BuildingNumber} {t.Address.City}",
                DateOfBirth = t.DateOfBirth


            });
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsByIdAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return null;
            var model = new TrainerViewModel()
            {
                id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Specialization = trainer.specialty.ToString(),
                phone = trainer.Phone,
                address = $"{trainer.Address.Street} {trainer.Address.BuildingNumber} {trainer.Address.City}",
                DateOfBirth = trainer.DateOfBirth
            };
            return model;

        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return null;
            var model = new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth,
                Gender = trainer.Gender,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
                Specialties = trainer.specialty
            };
            return model;
        }


        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return false;

            // Update trainer properties
            trainer.Name = model.Name;
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.DateOfBirth = model.DateOfBirth;
            trainer.Gender = model.Gender;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.specialty = model.Specialties;

             _unitOfWork.GetRepository<Trainer>().Update(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return false;

            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

    }
}
