using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using Goblin.Service_Resource.Contract.Repository.Models;
using Goblin.Service_Resource.Core.Models;

namespace Goblin.Service_Resource.Mapper
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<UploadFileModel, FileEntity>()
                .IgnoreAllNonExisting();
            
            CreateMap<FileEntity, FileModel>()
                .IgnoreAllNonExisting();
        }
    }
}