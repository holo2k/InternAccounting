using AutoMapper;
using InternAccounting.BusinessLayer.DataTransferObjects.Direction;
using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using InternAccounting.BusinessLayer.DataTransferObjects.Project;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DataLayer.Entities;
using InternAccounting.ViewModels;

namespace InternAccounting.BusinessLayer.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddInternViewModel, CreateInternDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.DirectionId, opt => opt.MapFrom(src => src.DirectionId))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex));

            CreateMap<InternDetailDto, AddInternViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.Split()[0]))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.Split()[1]))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.DirectionId, opt => opt.MapFrom(src => src.DirectionId))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Gender));

            CreateMap<DirectionEntity, DirectionDetailDto>()
                .ForMember(dest => dest.InternsCount, opt => opt.MapFrom(src => src.Interns.Count))
                .ForMember(dest => dest.Interns, opt => opt.MapFrom(src => src.Interns));

            CreateMap<DirectionEntity, DirectionDto>()
            .ForMember(dest => dest.InternsCount,
                opt => opt.MapFrom(src => src.Interns.Count))
              .ForMember(dest => dest.Interns, opt => opt.MapFrom(src => src.Interns));

            CreateMap<ProjectEntity, ProjectDetailDto>()
               .ForMember(dest => dest.InternsCount, opt => opt.MapFrom(src => src.Interns.Count))
               .ForMember(dest => dest.Interns, opt => opt.MapFrom(src => src.Interns));

            CreateMap<ProjectEntity, ProjectDto>()
            .ForMember(dest => dest.InternsCount,
                opt => opt.MapFrom(src => src.Interns.Count))
            .ForMember(dest => dest.Interns, opt => opt.MapFrom(src => src.Interns));

            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
            .ConvertUsing(typeof(PaginatedListConverter<,>));

            CreateMap<CreateDirectionDto, DirectionEntity>();
            CreateMap<CreateDirectionDto, UpdateDirectionDto>();

            CreateMap<CreateProjectDto, ProjectEntity>();
            CreateMap<CreateProjectDto, UpdateProjectDto>();

            CreateMap<ProjectEntity, ProjectDetailDto>()
                .ForMember(dest => dest.InternsCount, opt => opt.MapFrom(src => src.Interns.Count))
                .ForMember(dest => dest.Interns, opt => opt.MapFrom(src => src.Interns));

            CreateMap<CreateProjectDto, ProjectEntity>();

            CreateMap<InternEntity, InternDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null ? src.PhoneNumber.Number : null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Sex.ToString()))
                .ForMember(dest => dest.DirectionName, opt => opt.MapFrom(src => src.Direction != null ? src.Direction.Title : null))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.Title : null));

            CreateMap<InternEntity, InternDetailDto>()
                .IncludeBase<InternEntity, InternDto>();

            CreateMap<InternEntity, InternShortDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));

            CreateMap<CreateInternDto, InternEntity>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.PhoneNumber) ? new PhoneNumber(src.PhoneNumber) : null));
        }

        public class PaginatedListConverter<TSource, TDestination> :
                ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
        {
            public PaginatedList<TDestination> Convert(
                PaginatedList<TSource> source,
                PaginatedList<TDestination> destination,
                ResolutionContext context)
            {
                var items = context.Mapper.Map<List<TDestination>>(source);
                return new PaginatedList<TDestination>(
                    items,
                    source.Count,
                    source.PageIndex,
                    source.PageSize
                );
            }
        }
    }
}
