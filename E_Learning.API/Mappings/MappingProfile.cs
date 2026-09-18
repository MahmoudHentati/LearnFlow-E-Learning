using AutoMapper;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using E_Learning.Domain;

namespace E_Learning.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ShouldMapProperty = propertyInfo => propertyInfo.GetMethod is not null || propertyInfo.SetMethod is not null;
            ShouldMapField = _ => true;

            CreateMap<Categorie, CategorieDTO>()
                .ForMember(dest => dest.SousCategories, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<SousCategorie, SousCategorieDTO>()
                .ForMember(dest => dest.Formations, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Formation, FormationDTO>()
                .ForMember(dest => dest.FormateurNom,
                    opt => opt.MapFrom(src => src.Formateur != null
                        ? $"{src.Formateur.FirstName} {src.Formateur.LastName}".Trim()
                        : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Formateur, opt => opt.Ignore())
                .ForMember(dest => dest.SousCategorie, opt => opt.Ignore());
            CreateMap<Module, ModuleDTO>().ReverseMap()
                .ForMember(dest => dest.Formation, opt => opt.Ignore());
            CreateMap<Video, VideoDTO>().ReverseMap();
            CreateMap<Document, DocumentDTO>().ReverseMap();
            CreateMap<Quiz, QuizDTO>().ReverseMap()
                .ForMember(dest => dest.Module, opt => opt.Ignore());
            CreateMap<Question, QuestionDTO>().ReverseMap()
                .ForMember(dest => dest.Quiz, opt => opt.Ignore());
            CreateMap<OptionReponse, OptionResponseDTO>().ReverseMap()
                .ForMember(dest => dest.Question, opt => opt.Ignore());
            CreateMap<Test, TestDTO>().ReverseMap()
                .ForMember(dest => dest.Module, opt => opt.Ignore());
            CreateMap<RenduTest, RenduTestDTO>().ReverseMap()
                .ForMember(dest => dest.Test, opt => opt.Ignore())
                .ForMember(dest => dest.Inscription, opt => opt.Ignore());
            CreateMap<Inscription, InscriptionDTO>()
                .ForMember(dest => dest.Edudiant, opt => opt.MapFrom(src => src.Edudiant))
                .ReverseMap()
                .ForMember(dest => dest.Edudiant, opt => opt.Ignore());
            CreateMap<AppUser, AppUserDTO>().ReverseMap();
            CreateMap<Avis, AvisDTO>().ReverseMap();
            CreateMap<ParticipationQuiz, ParticipationQuizDTO>().ReverseMap();
        }
    }
}
