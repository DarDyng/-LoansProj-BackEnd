using AutoMapper;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s;

namespace LoansAppWebApi.Core.Configuration.Profiles
{
    public class LoanProfiles : Profile
    {
        public LoanProfiles()
        {
            CreateMap<Loans, LoanDTO>()
                .ForMember(dest => dest.Name,
                    opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.StartDate,
                    opts => opts.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate,
                    opts => opts.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Id,
                    opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.PercentsInYear,
                    opts => opts.MapFrom(src => src.PercentsInYear))
                .ForMember(dest => dest.SumOfLoan,
                    opts => opts.MapFrom(src => src.SumOfLoan))
                .ForMember(dest => dest.IsPaid,
                    opts => opts.MapFrom(src => src.IsPaid));
        }
    }
}