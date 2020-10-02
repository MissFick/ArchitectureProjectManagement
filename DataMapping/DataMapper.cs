using System;
using AutoMapper;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.Models;

namespace ArchitectureProjectManagement.DataMapping
{
    public class DataMapper : Profile
    {
        public DataMapper()
        {
            CreateMap<Company, CompanyViewModel>().ReverseMap();
            CreateMap<ProjectItem, ProjectItemViewModel>().ReverseMap();
            CreateMap<ProjectItemStatus, ProjectItemStatusViewModel>().ReverseMap();
            CreateMap<Project, ProjectViewModel>().ReverseMap();
            CreateMap<ProjectState, ProjectStateViewModel>().ReverseMap();
            CreateMap<Property, PropertyViewModel>().ReverseMap();
            CreateMap<AppUserRole, AppUserRoleViewModel>().ReverseMap();
        }
    }
}
