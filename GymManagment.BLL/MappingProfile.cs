using AutoMapper;
using GymManagment.BLL.ViewModel.Member;
using GymManagment.BLL.ViewModel.Session;
using GymManagment.DAL.Data.Models;
using System.Reflection;

namespace GymManagment.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapMember();
            MapSession();


        }
        private void MapMember()
        {

            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber}_{src.Address.Street}_{src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<MemberToUpdateViewModel, Member>()
                   .ForMember(dest => dest.Name, opt => opt.Ignore())
                   .ForMember(dest => dest.Photo, opt => opt.Ignore())
                   .AfterMap((src, dest) =>
                   {
                       dest.Address.BuildingNumber = src.BuildingNumber;
                       dest.Address.City = src.City;
                       dest.Address.Street = src.Street;

                   });

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }))

                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));


        }

        private void MapSession()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer,TrainerSelectViewModel>();
            CreateMap<Category,CategorySelectViewModel>();
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }



    }

}
