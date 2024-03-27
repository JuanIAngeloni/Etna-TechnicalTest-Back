using AutoMapper;
using Etna_Data.Entities;
using Etna_Data.Models;



namespace gringotts_application
{
    /// <summary>
    /// Configures object-to-object mappings using AutoMapper.
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the AutoMapper class and defines object mappings.
        /// </summary>
        public MapperProfile()
        {
            /// <summary>
            /// Configures mapping from UserRegisterModel to UserEntity.
            /// </summary>
            /// 
            CreateMap<UserRegisterModel, UserEntity>();
            CreateMap<UserEntity, UserRegisterModel>();

            CreateMap<TaskEntity,TaskRegisterModel>();
            CreateMap<TaskRegisterModel,TaskEntity>();
            CreateMap<TaskEntity, TaskUpdateModel>();
            
            CreateMap<CategoryEntity, CategoryModel>();

        }
    }
}
