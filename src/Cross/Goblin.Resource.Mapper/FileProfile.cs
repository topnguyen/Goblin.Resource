using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using Goblin.Resource.Contract.Repository.Models;
using Goblin.Resource.Share.Models;

namespace Goblin.Resource.Mapper
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<GoblinResourceUploadFileModel, FileEntity>()
                .IgnoreAllNonExisting();
            
            CreateMap<FileEntity, GoblinResourceFileModel>()
                .IgnoreAllNonExisting();
        }
    }
}