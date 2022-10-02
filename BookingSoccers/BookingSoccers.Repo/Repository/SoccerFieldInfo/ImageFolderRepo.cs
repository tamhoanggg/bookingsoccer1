using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.Repository.SoccerFieldInfo
{
    public class ImageFolderRepo : BaseRepository<ImageFolder>, IImageFolderRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public ImageFolderRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

    }
}
